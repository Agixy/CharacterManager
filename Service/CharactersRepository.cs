using Microsoft.EntityFrameworkCore;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service
{
    public class CharactersRepository
    {
        public CharactersRepository()
        {
            CreateDatabase();
        }

        public List<Character> GetAllCharacters()
        {
            using (var context = new CharacterDbContext())
            {
                return context.Characters.ToList();
            }
        }

        public List<Character> GetCharactersByBreed(int breedId)
        {
            using (var context = new CharacterDbContext())
            {
                return context.Characters.Where(ch => ch.Appearance.BreedId == breedId).ToList();
            }
        }       

        public List<Breed> GetAllBreeds()
        {
            using (var context = new CharacterDbContext())
            {
                return context.Breeds.ToList();
            }
        }

        public List<Orientation> GetOrientations()
        {
            using (var context = new CharacterDbContext())
            {
                return context.Orientations.ToList();
            }
        }

        public List<TypeOfCharacter> GetTypesOfCharacter()
        {
            using (var context = new CharacterDbContext())
            {
                return context.TypeOfCharacters.ToList();
            }
        }

        public List<AlignmentChart> GetAlignmentChatrs()
        {
            using (var context = new CharacterDbContext())
            {
                return context.AlignmentCharts.ToList();
            }
        }

        public List<Image> GetAllImages()
        {
            using (var context = new ImageDbContext())
            {
                return context.Images.ToList();
            }
        }

        public List<Image> GetAllAvatars()
        {
            using (var context = new ImageDbContext())
            {
                return context.Images.Where(i => i.IsAvatar == true).ToList();
            }
        }

        public Image GetImageById(int imageId)
        {
            using (var context = new ImageDbContext())
            {
                return context.Images.FirstOrDefault(i => i.Id == imageId);
            }
        }

        public List<Image> GetImagesByCharacterId(int characterId)
        {
            using (var context = new ImageDbContext())
            {
                return context.Images.Where(i => i.CharacterId == characterId).ToList();
            }
        }

        public Character GetCharacterById(int id)
        {
            using (var context = new CharacterDbContext())
            {
                return context.Characters.Include(ch => ch.Appearance).Include(ch => ch.Origin).Include(ch => ch.Personality).FirstOrDefault(ch => ch.Id == id);
            }
        }

        public void AddCharacter(Character character)
        {
            using (var context = new CharacterDbContext())
            {
                context.Add(character);
                context.SaveChanges();
            }
        }

        public void UpdateCharacter(Character character)
        {
            UpdateCharacterPropertiesIds(character);

            using (var context = new CharacterDbContext())
            {
                context.Characters.Update(character);
                context.SaveChanges();
            }
        }

        private void UpdateCharacterPropertiesIds(Character character)
        {
            using (var context = new CharacterDbContext())
            {
                character.Appearance.Id = context.Characters.Include(ch => ch.Appearance).FirstOrDefault(ch => ch.Id == character.Id).Appearance.Id;
                character.Personality.Id = context.Characters.Include(ch => ch.Personality).FirstOrDefault(ch => ch.Id == character.Id).Personality.Id;
                character.Origin.Id = context.Characters.Include(ch => ch.Origin).FirstOrDefault(ch => ch.Id == character.Id).Origin.Id;
            }
        }

        public void DeleteCharacter(int characterId)
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
        }

        private static void CreateDatabase()
        {
            using (var context = new CharacterDbContext())
            {
                context.Database.EnsureCreated();
                context.SaveChanges();
            }

            using (var context = new ImageDbContext())
            {
                context.Database.EnsureCreated();
                context.SaveChanges();
            }
        }
    }
}
