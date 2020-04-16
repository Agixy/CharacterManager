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
        private Dictionary<int, string> TypesOfCharacter;
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
            AllCharacters = service.GetAllCharacters();
            Breeds = service.GetAllBreeds().ToDictionary(b => b.Id, b => b.Name);
            TypesOfCharacter = service.GetTypesOfCharacter().ToDictionary(b => b.Id, b => b.Name);
            Orientations = service.GetOrientations().ToDictionary(b => b.Id, b => b.Name);
            AlignmentCharts = service.GetAlignmentChatrs().ToDictionary(b => b.Id, b => b.Name);

            AllImages = service.GetAllImages();
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

            result.Images = AllImages.Where(i => i.CharacterId == character.Id).Select(i => _mapper.Map<ImageViewModel>(i)).ToList();

            ViewData["Characters"] = AllCharacters.ToDictionary(ch => ch.Id, ch => ch.Name + " " + ch.Surname);
            ViewData["TypeOfCharacters"] = TypesOfCharacter;
            ViewData["Orientations"] = Orientations;
            ViewData["AlignmentCharts"] = AlignmentCharts;
            ViewData["Breeds"] = Breeds;

            return View("~/Views/Character/CharacterView.cshtml", result);
        }

        public IActionResult Dictionary()
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
                if (characterViewModel.Id == 0)
                {
                    context.Characters.Add(character);
                }
                else
                {
                    character.Appearance.Id = AllCharacters.FirstOrDefault(ch => ch.Id == characterViewModel.Id).Appearance.Id;
                    character.Personality.Id = AllCharacters.FirstOrDefault(ch => ch.Id == characterViewModel.Id).Personality.Id;
                    character.Origin.Id = AllCharacters.FirstOrDefault(ch => ch.Id == characterViewModel.Id).Origin.Id;
                    var relationships = HttpContext.Session.GetComplexData<List<RelationshipViewModel>>("NewRelationships");
                    if (character.Relationships.Length == 0 && relationships != null)
                    {
                        character.Relationships = String.Join(",", relationships.Select(r => r.TargetRelationshipCharacterName + "-" + r.Type));
                    }
                    else if (relationships != null)
                    {
                        character.Relationships += "," + relationships.Select(r => r.TargetRelationshipCharacterName + "-" + r.Type);
                    }


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
                var character = context.Characters.Include(ch => ch.Appearance).Include(ch => ch.Origin).Include(ch => ch.Personality)
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
            ViewData["TypeOfCharacters"] = TypesOfCharacter;
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
        public void AddRelationship(int characterId, string relationshipCharacterName, string relationship)
        {
            var list = HttpContext.Session.GetComplexData<List<RelationshipViewModel>>("NewRelationships")?? new List<RelationshipViewModel>();
            list.Add(new RelationshipViewModel()
            {
                Type = relationship,
                TargetRelationshipCharacterName = relationshipCharacterName,
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
        public IActionResult UploadImage(int characterId)
        {
            foreach (var file in Request.Form.Files)
            {
                Image img = new Image();
                img.ImageTitle = file.FileName;

                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                img.IsAvatar = true;
                img.CharacterId = characterId;
                img.ImageData = ms.ToArray();

                ms.Close();
                ms.Dispose();

                using (var context = new ImageDbContext())
                {
                    context.Images.Add(img);
                    context.SaveChanges();
                }
            }

            return Redirect($"/Home/CharacterView?characterId={characterId}");
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
