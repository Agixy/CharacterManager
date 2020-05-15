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
        private readonly CharactersRepository repository;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            repository = new CharactersRepository();            
          
        }             

        public IActionResult Index()
        {           
            var characters = repository.GetAllCharacters().Select(x => new AlbumCardViewModel()
            { Id = x.Id, DisplayName = x.Name + " " + x.Surname }).OrderBy(ch => ch.DisplayName).ToList();

            var avatars = repository.GetAllAvatars();

            foreach (var item in characters)
            {
                item.Avatar = _mapper.Map<ImageViewModel>(avatars.FirstOrDefault(i => i.CharacterId == item.Id)); 
            }
           
            ViewData["Breeds"] = repository.GetAllBreeds().ToDictionary(b => b.Id, b => b.Name);
            return View(characters);
        }

        public IActionResult CharacterView(int characterId)
        {
            var mapper = new CharacterMapper();
            var character = mapper.MapToView(repository.GetCharacterById(characterId));

            character.Images = repository.GetAllImages().Where(i => i.CharacterId == characterId).Select(i => _mapper.Map<ImageViewModel>(i)).ToList();

            HttpContext.Session.SetComplexData("NewRelationships", character.Relationships);

            ViewData["Characters"] = repository.GetAllCharacters().ToDictionary(ch => ch.Id, ch => ch.Name + " " + ch.Surname);
            ViewData["TypeOfCharacters"] = repository.GetTypesOfCharacter().ToDictionary(b => b.Id, b => b.Name); ;
            ViewData["Orientations"] = repository.GetOrientations().ToDictionary(b => b.Id, b => b.Name); ;
            ViewData["AlignmentCharts"] = repository.GetAlignmentChatrs().ToDictionary(b => b.Id, b => b.Name);
            ViewData["Breeds"] = repository.GetAllBreeds().ToDictionary(b => b.Id, b => b.Name);

            return View("~/Views/Character/CharacterView.cshtml", character);
        }

        public IActionResult Informations()
        {
            return View();
        }

        public IActionResult FullSizeImage(int imageId)
        {
            var image = _mapper.Map<ImageViewModel>(repository.GetAllImages().FirstOrDefault(i => i.Id == imageId));
            return View("~/Views/Character/FullSizeImage.cshtml", image);
        }

        [HttpPost]
        public IActionResult Save(CharacterViewModel characterViewModel)
        {
            var mapper = new CharacterMapper();
            characterViewModel.Relationships.AddRange(HttpContext.Session.GetComplexData<List<RelationshipViewModel>>("NewRelationships"));
            var character = mapper.MapToModel(characterViewModel);
         
            if (characterViewModel.Id == 0)
            {
                repository.AddCharacter(character);
            }
            else
            {
                repository.UpdateCharacter(character);
            }

            HttpContext.Session.Clear();

            return Redirect($"/Home/CharacterView?characterId={character.Id}");
        }

        [HttpGet]
        public bool IsPasswordCorrect(string password)
        {
            return password.Equals("password");
        }

        public IActionResult DeleteCharacter(int characterId)
        {
            repository.DeleteCharacter(characterId);

            return Redirect("/Home/Index");
        }

        [HttpGet]
        public IActionResult CreateNewCharacter()
        {
            var newCharacter = new CharacterViewModel();

            ViewData["Characters"] = repository.GetAllCharacters().ToDictionary(ch => ch.Id, ch => ch.Name + " " + ch.Surname);    
            ViewData["TypeOfCharacters"] = repository.GetTypesOfCharacter().ToDictionary(b => b.Id, b => b.Name); ;
            ViewData["Orientations"] = repository.GetOrientations().ToDictionary(b => b.Id, b => b.Name); ;
            ViewData["AlignmentCharts"] = repository.GetAlignmentChatrs().ToDictionary(b => b.Id, b => b.Name);
            ViewData["Breeds"] = repository.GetAllBreeds().ToDictionary(b => b.Id, b => b.Name);

            return View("~/Views/Character/CharacterView.cshtml", newCharacter); ;
        }

        public IActionResult Filter(int breedId, int other)
        {
            var characters = repository.GetAllCharacters().Where(ch => ch.Appearance.BreedId == breedId).Select(x => new AlbumCardViewModel()
            { Id = x.Id, DisplayName = x.Name + " " + x.Surname }).OrderBy(ch => ch.DisplayName).ToList();

            ViewData["Breeds"] = repository.GetAllBreeds();
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
            return repository.GetAllCharacters().FirstOrDefault(ch => (ch.Name + " " + ch.Surname).Equals(relationshipCharacterName)).Id;
        }

        [HttpGet]
        public IActionResult GetRelationhips(int characterId)
        {
            var relationships = repository.GetAllCharacters().FirstOrDefault(ch => ch.Id == characterId).Relationships;
            return Json(relationships);
        }    

        [HttpGet]
        public IActionResult GetAllCharacters()
        {
            var characters = repository.GetAllCharacters().ToDictionary(ch => ch.Id, ch => ch.Name + " " + ch.Surname);
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
