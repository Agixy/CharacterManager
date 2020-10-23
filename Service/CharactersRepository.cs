using Microsoft.EntityFrameworkCore;
using Service.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;
using Service.Interfaces;

namespace Service
{
    public class CharactersRepository : ICharactersRepository
    {
        private readonly CharacterDbContext characterDb;
        private readonly ImageDbContext imageDb;

        public CharactersRepository(CharacterDbContext characterDbContext, ImageDbContext imageDbCOntext)
        {
            characterDb = characterDbContext;
            imageDb = imageDbCOntext;
            CreateDatabase();
        }

        public IList<Character> GetAllCharacters()
        {
            return characterDb.Characters.ToList();
        }

        public IList<Character> GetCharactersByBreed(int breedId)
        {
            return characterDb.Characters.Where(ch => ch.Appearance.BreedId == breedId).ToList();
        }

        public IList<Breed> GetAllBreeds()
        {
            return characterDb.Breeds.ToList();
        }

        public IList<Orientation> GetOrientations()
        {
            return characterDb.Orientations.ToList();
        }

        public IList<TypeOfCharacter> GetTypesOfCharacter()
        {
            return characterDb.TypeOfCharacters.ToList();
        }

        public IList<AlignmentChart> GetAlignmentChatrs()
        {
            return characterDb.AlignmentCharts.ToList();
        }

        public IList<Image> GetAllImages()
        {
            return imageDb.Images.ToList();
        }

        public IList<Image> GetAllAvatars()
        {
            return imageDb.Images.Where(i => i.IsAvatar == true).ToList();
        }

        public Image GetImageById(int imageId)
        {
            return imageDb.Images.FirstOrDefault(i => i.Id == imageId);
        }

        public IList<Image> GetImagesByCharacterId(int characterId)
        {
            return imageDb.Images.Where(i => i.CharacterId == characterId).ToList();
        }

        public void AddImage(int characterId, IFormFileCollection fileList)
        {
            foreach (var file in fileList)
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

                imageDb.Images.Add(img);
                imageDb.SaveChanges();
            }
        }

        public Character GetCharacterById(int id)
        {
            return characterDb.Characters.Include(ch => ch.Appearance).Include(ch => ch.Origin).Include(ch => ch.Personality).FirstOrDefault(ch => ch.Id == id);
        }

        public void AddCharacter(Character character)
        {
            characterDb.Add(character);
            characterDb.SaveChanges();
        }

        public void UpdateCharacter(Character character)
        {
            UpdateCharacterPropertiesIds(character);

            characterDb.Characters.Update(character);
            characterDb.SaveChanges();
        }

        public void DeleteCharacter(int characterId)
        {
            var character = characterDb.Characters.Include(ch => ch.Appearance).Include(ch => ch.Origin).Include(ch => ch.Personality)
                .FirstOrDefault(ch => ch.Id == characterId);

            characterDb.Origins.Remove(character.Origin);
            characterDb.Pertonalities.Remove(character.Personality);
            characterDb.Appearances.Remove(character.Appearance);
            characterDb.Characters.Remove(character);
            characterDb.SaveChanges();
        }

        private void CreateDatabase()
        {
            characterDb.Database.EnsureCreated();
            characterDb.SaveChanges();

            imageDb.Database.EnsureCreated();
            imageDb.SaveChanges();
        }

        private void UpdateCharacterPropertiesIds(Character character)
        {
            character.Appearance.Id = characterDb.Characters.Include(ch => ch.Appearance).FirstOrDefault(ch => ch.Id == character.Id).Appearance.Id;
            character.Personality.Id = characterDb.Characters.Include(ch => ch.Personality).FirstOrDefault(ch => ch.Id == character.Id).Personality.Id;
            character.Origin.Id = characterDb.Characters.Include(ch => ch.Origin).FirstOrDefault(ch => ch.Id == character.Id).Origin.Id;
        }
    }
}
