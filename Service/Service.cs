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
                return context.Characters.ToList();
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
