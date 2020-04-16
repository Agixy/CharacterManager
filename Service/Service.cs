using Microsoft.EntityFrameworkCore;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class Service                    // TODO: Move all database loading from controller here
    {      
        public Service()
        {
            CreateDatabase();
        }

        public List<Character> GetAllCharacters()
        {
            using (var context = new CharacterDbContext())
            {
                return context.Characters.Include(ch => ch.Appearance).Include(ch => ch.Origin).Include(ch => ch.Personality).ToList(); ;
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

        public Character GetCharacterById(int id)
        {
            using (var context = new CharacterDbContext())
            {
                return context.Characters.FirstOrDefault(ch => ch.Id == id);
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
