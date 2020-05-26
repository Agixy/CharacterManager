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
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using System;
using Microsoft.Extensions.Configuration;

namespace CharactersManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CharactersRepository charactersRepository;
        private readonly IConfiguration Configuration;

        private static readonly Lazy<IMapper> LazyMapper = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg => {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<MappingProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });

        private static IMapper Mapper => LazyMapper.Value;       

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            charactersRepository = new CharactersRepository();
            Configuration = new ConfigurationBuilder().AddJsonFile("secrets.json").Build();
        }             

        public IActionResult Index()
        {           
            var characters = charactersRepository.GetAllCharacters().Select(x => new AlbumCardViewModel()
            { Id = x.Id, DisplayName = x.Name + " " + x.Surname }).OrderBy(ch => ch.DisplayName).ToList();

            var avatars = charactersRepository.GetAllAvatars();

            foreach (var character in characters)
            {
                character.Avatar = Mapper.Map<ImageViewModel>(avatars.FirstOrDefault(i => i.CharacterId == character.Id)); 
            }
           
            ViewData["Breeds"] = charactersRepository.GetAllBreeds().ToDictionary(b => b.Id, b => b.Name);
            return View(characters);
        }

        public IActionResult CharacterView(int characterId)
        {
            var character = Mapper.Map<CharacterViewModel>(charactersRepository.GetCharacterById(characterId));

            character.Images = charactersRepository.GetImagesByCharacterId(characterId).Select(i => Mapper.Map<ImageViewModel>(i)).ToList();

            HttpContext.Session.SetComplexData("NewRelationships", character.Relationships);

            ViewData["Characters"] = charactersRepository.GetAllCharacters().ToDictionary(ch => ch.Id, ch => ch.Name + " " + ch.Surname);
            ViewData["TypeOfCharacters"] = charactersRepository.GetTypesOfCharacter().ToDictionary(b => b.Id, b => b.Name); ;
            ViewData["Orientations"] = charactersRepository.GetOrientations().ToDictionary(b => b.Id, b => b.Name); ;
            ViewData["AlignmentCharts"] = charactersRepository.GetAlignmentChatrs().ToDictionary(b => b.Id, b => b.Name);
            ViewData["Breeds"] = charactersRepository.GetAllBreeds().ToDictionary(b => b.Id, b => b.Name);
            ViewData["DisableEditing"] = "true";

            return View("~/Views/Character/CharacterView.cshtml", character);
        }

        public IActionResult Informations()
        {
            return View();
        }

        public IActionResult Worlds()
        {
            var worldsImageId = 25; // TODO: It is temporary image. It will be a page with text in the future.
            var imageViewModel = Mapper.Map<ImageViewModel>(charactersRepository.GetImageById(worldsImageId));
            return View(imageViewModel);
        }

        public IActionResult FullSizeImage(int imageId)
        {
            var image = Mapper.Map<ImageViewModel>(charactersRepository.GetImageById(imageId));
            return View("~/Views/Character/FullSizeImage.cshtml", image);
        }

        [HttpPost]
        public IActionResult Save(CharacterViewModel characterViewModel)
        {
            var mapper = new MappingProfile();
            characterViewModel.Relationships.AddRange(HttpContext.Session.GetComplexData<List<RelationshipViewModel>>("NewRelationships"));
            var character = Mapper.Map<Character>(characterViewModel);

            if (characterViewModel.Id == 0)
            {
                charactersRepository.AddCharacter(character);
            }
            else
            {
                charactersRepository.UpdateCharacter(character);
            }

            HttpContext.Session.Clear();

            return Redirect($"/Home/CharacterView?characterId={character.Id}");
        }

        [HttpGet]
        public bool IsPasswordCorrect(string password)
        {          
            if (password != null && password.Equals(Configuration["Editing:Password"]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IActionResult DeleteCharacter(int characterId)
        {
            charactersRepository.DeleteCharacter(characterId);

            return Redirect("/Home/Index");
        }

        [HttpGet]
        public IActionResult CreateNewCharacter()
        {
            var newCharacter = new CharacterViewModel();

            ViewData["Characters"] = charactersRepository.GetAllCharacters().ToDictionary(ch => ch.Id, ch => ch.Name + " " + ch.Surname);    
            ViewData["TypeOfCharacters"] = charactersRepository.GetTypesOfCharacter().ToDictionary(b => b.Id, b => b.Name); ;
            ViewData["Orientations"] = charactersRepository.GetOrientations().ToDictionary(b => b.Id, b => b.Name); ;
            ViewData["AlignmentCharts"] = charactersRepository.GetAlignmentChatrs().ToDictionary(b => b.Id, b => b.Name);
            ViewData["Breeds"] = charactersRepository.GetAllBreeds().ToDictionary(b => b.Id, b => b.Name);
            ViewData["DisableEditing"] = "false";

            return View("~/Views/Character/CharacterView.cshtml", newCharacter); ;
        }

        public IActionResult Filter(int breedId, int other)
        {
            var characters = charactersRepository.GetCharactersByBreed(breedId).Select(x => new AlbumCardViewModel()
            { Id = x.Id, DisplayName = x.Name + " " + x.Surname }).OrderBy(ch => ch.DisplayName).ToList();

            ViewData["Breeds"] = charactersRepository.GetAllBreeds();
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
            return charactersRepository.GetAllCharacters().FirstOrDefault(ch => (ch.Name + " " + ch.Surname).Equals(relationshipCharacterName)).Id;
        }

        [HttpGet]
        public IActionResult GetRelationhips(int characterId)
        {
            var relationships = charactersRepository.GetAllCharacters().FirstOrDefault(ch => ch.Id == characterId).Relationships;
            return Json(relationships);
        }    

        [HttpGet]
        public IActionResult GetAllCharacters()
        {
            var characters = charactersRepository.GetAllCharacters().ToDictionary(ch => ch.Id, ch => ch.Name + " " + ch.Surname);
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
