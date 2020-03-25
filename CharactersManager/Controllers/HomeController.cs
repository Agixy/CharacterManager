using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CharactersManager.Models;
using AutoMapper;
using Service.Models;
using Service;
using CharactersManager.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using System;

namespace CharactersManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Service.Service service;
        private readonly IMapper _mapper;

        private IList<Character> AllCharacters;
        private IList<Image> AllImages;
        private Dictionary<int, string> Breeds;
        private Dictionary<int, string> TypeOfCharacters;
        private Dictionary<int, string> Orientations;
        private Dictionary<int, string> AlignmentCharts;

        public HomeController(ILogger<HomeController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            service = new Service.Service();
            
            LoadAllData(); 
        }
             

        private void LoadAllData()
        {
            using (var context = new CharacterDbContext())
            {
                AllCharacters = context.Characters.Include(ch => ch.Appearance).Include(ch => ch.Origin).Include(ch => ch.Personality).ToList();
                Breeds = context.Breeds.ToDictionary(b => b.Id, b => b.Name);
                TypeOfCharacters = context.TypeOfCharacters.ToDictionary(b => b.Id, b => b.Name);
                Orientations = context.Orientations.ToDictionary(b => b.Id, b => b.Name);
                AlignmentCharts = context.AlignmentCharts.ToDictionary(b => b.Id, b => b.Name);
            }

            using (var context = new ImageDbContext())
            {
                AllImages = context.Images.ToList();
            }
        }

        public string GetAvatar()
        {
            using (var context = new ImageDbContext())
            {
                Image img = context.Images.OrderByDescending
             (i => i.Id).FirstOrDefault();
                string imageBase64Data =
            Convert.ToBase64String(img.ImageData);
               return string.Format("data:image/jpg;base64,{0}",
            imageBase64Data);

                
            }
        }

        public IActionResult Index()
        {
            
            var characters = AllCharacters.Select(x => _mapper.Map<CharacterViewModel>(x)).OrderBy(ch => ch.Name).ToList();

            foreach (var item in characters)
            {
                item.Images = AllImages.Where(i => i.CharacterId == item.Id).Select(i => _mapper.Map<ImageViewModel>(i)).ToList();
            }
           
            ViewData["Breeds"] = Breeds;
            return View(characters);
        }

        public IActionResult CharacterView(int characterId)
        {
            var character = AllCharacters.FirstOrDefault(ch => ch.Id == characterId);
            var result = _mapper.Map<CharacterViewModel>(character);
         
            ViewData["Characters"] = AllCharacters.ToDictionary(ch => ch.Id, ch => ch.Name + " " + ch.Surname);
            ViewData["TypeOfCharacters"] = TypeOfCharacters;
            ViewData["Orientations"] = Orientations;
            ViewData["AlignmentCharts"] = AlignmentCharts;
            ViewData["Breeds"] = Breeds;

            return View("~/Views/Character/CharacterView.cshtml", result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Save(CharacterViewModel characterViewModel)
        {

            var mapper = new CharacterViewToModelMapper();
            var character = mapper.MapToModel(characterViewModel);

            using (var context = new CharacterDbContext())
            {
                if(characterViewModel.Id == 0)
                {             
                    context.Characters.Add(character);
                }
                else
                {                    
                    character.Appearance.Id = AllCharacters.FirstOrDefault(ch => ch.Id == characterViewModel.Id).Appearance.Id;
                    character.Personality.Id = AllCharacters.FirstOrDefault(ch => ch.Id == characterViewModel.Id).Personality.Id;
                    character.Origin.Id = AllCharacters.FirstOrDefault(ch => ch.Id == characterViewModel.Id).Origin.Id;
                    character.Relationships += "," + String.Join(",", HttpContext.Session.GetComplexData<List<RelationshipViewModel>>("NewRelationships").Select(r => r.TargetRelationshipCharacterName + "-" + r.Type));

                    context.Characters.Update(character);                   
                }             

                context.SaveChanges();
                HttpContext.Session.Clear();
            }

            return Redirect($"/Home/CharacterView?characterId={character.Id}");
        }
                
        public IActionResult DeleteCharacter(int characterId)
        {
            using (var context = new CharacterDbContext())
            {
                var character = context.Characters.Include(ch => ch.Appearance).Include(ch => ch.Origin).Include(ch => ch.Personality).Include(ch => ch.Relationships)
                    .FirstOrDefault(ch => ch.Id == characterId);

                context.Origins.Remove(character.Origin);
                context.Pertonalities.Remove(character.Personality);
                context.Appearances.Remove(character.Appearance);
                context.Characters.Remove(character);
                context.SaveChanges();
            }

            return Redirect("/Home/Index");
        }

        public IActionResult CreateNewCharacter()
        {
            var newCharacter = new CharacterViewModel();

            ViewData["Characters"] = AllCharacters.ToDictionary(ch => ch.Id, ch => ch.Name + " " + ch.Surname);
            ViewData["TypeOfCharacters"] = TypeOfCharacters;
            ViewData["Orientations"] = Orientations ;
            ViewData["AlignmentCharts"] = AlignmentCharts;
            ViewData["Breeds"] = Breeds;

            return View("~/Views/Character/CharacterView.cshtml", newCharacter); ;
        }

        public IActionResult Filter(int breedId, int other)
        {
            var characters = AllCharacters.Where(ch => ch.Appearance.BreedId == breedId).Select(x => _mapper.Map<CharacterViewModel>(x)).ToList();
            ViewData["Breeds"] = Breeds;
            return View("~/Views/Home/Index.cshtml", characters);
        }

        [HttpPost]
        public void AddRelationship(int characterId, int relationshipCharacterId, string relationship)
        {
            var list = HttpContext.Session.GetComplexData<List<RelationshipViewModel>>("NewRelationships")?? new List<RelationshipViewModel>();
            list.Add(new RelationshipViewModel()
            {
                Type = relationship,
                TargetRelationshipCharacterId = relationshipCharacterId,
                CharacterId = characterId
            });
            HttpContext.Session.SetComplexData("NewRelationships", list);
        }

        [HttpGet]
        public int Exist(string relationshipCharacterName)
        {
            return AllCharacters.FirstOrDefault(ch => (ch.Name + " " + ch.Surname).Equals(relationshipCharacterName)).Id;
        }

        [HttpGet]
        public IActionResult GetRelationhips(int characterId)
        {
            var relationships = AllCharacters.FirstOrDefault(ch => ch.Id == characterId).Relationships;
            return Json(relationships);
        }
    

        [HttpGet]
        public IActionResult GetAllCharacters()
        {
            var characters = AllCharacters.ToDictionary(ch => ch.Id, ch => ch.Name + " " + ch.Surname);
            return Json(characters);
        }


        [HttpPost]
        public IActionResult AddImage()
        {
            byte[] bytes;

            foreach (var file in Request.Form.Files)
            {
                // http://www.binaryintellect.net/articles/2f55345c-1fcb-4262-89f4-c4319f95c5bd.aspx
                // https://docs.microsoft.com/en-us/learn/modules/publish-azure-web-app-with-visual-studio/7-exercise-publish-an-update-to-your-site
                // https://github.com/aspnet/Tooling/blob/AspNetVMs/docs/create-asp-net-vm-with-webdeploy.md
                // https://www.codeproject.com/Articles/786085/ASP-NET-MVC-List-Editor-with-Bootstrap-Modals
                // https://portal.azure.com/?quickstart=true#@aglowacka224gmail.onmicrosoft.com/resource/subscriptions/f8828453-cf83-42a4-b5c5-184f7eb75b81/resourceGroups/Characters/providers/Microsoft.Web/sites/ApocalyptusArt/appServices

                //Image img = new Image();
                //img.ImageTitle = file.FileName;

                //MemoryStream ms = new MemoryStream();
                //file.CopyTo(ms);
                //img.ImageData = ms.ToArray();

                //ms.Close();
                //ms.Dispose();

                //db.Images.Add(img);
                //db.SaveChanges();
            }

            return View("~/Views/Character/CharacterView.cshtml", new CharacterViewModel());
        }

        [HttpPost]
        public IActionResult UploadImage()
        {
            foreach (var file in Request.Form.Files)
            {
                Image img = new Image();
                img.ImageTitle = file.FileName;

                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                img.CharacterId = 3;
                img.ImageTitle = "Cos";
                img.ImageData = ms.ToArray();
                img.IsAvatar = true;

                ms.Close();
                ms.Dispose();

                using (var context = new ImageDbContext())
                {
                    context.Images.Add(img);
                    context.SaveChanges();
                }
           
            }

            return Redirect("/Home/Index");
        }

        [HttpPost]
        public ActionResult RetrieveImage()
        {
            using (var context = new ImageDbContext())
            {
                Image img = context.Images.OrderByDescending
             (i => i.Id).SingleOrDefault();
                string imageBase64Data =
            Convert.ToBase64String(img.ImageData);
                string imageDataURL =
            string.Format("data:image/jpg;base64,{0}",
            imageBase64Data);
                ViewBag.ImageTitle = img.ImageTitle;
                ViewBag.ImageDataUrl = imageDataURL;

                ViewData["Breeds"] = Breeds;

                return View("Index", AllCharacters.Select(x => _mapper.Map<CharacterViewModel>(x)).ToList());
            }         
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    public static class SessionExtensions
    {
        public static T GetComplexData<T>(this ISession session, string key)
        {
            var data = session.GetString(key);
            if (data == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static void SetComplexData(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
    }
}
