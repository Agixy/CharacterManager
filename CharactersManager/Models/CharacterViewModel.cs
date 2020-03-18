
using System.Collections.Generic;

namespace CharactersManager.Models
{
    public class CharacterViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Sex { get; set; }        
        public OriginViewModel Origin { get; set; }
        public string Profesion { get; set; } 
        public int BirthErrar { get; set; }
        public List<RelationshipViewModel> Relationships { get; set; }
        public PersonalityViewModel Personality { get; set; }
        public AppearanceViewModel Appearance { get; set; }

        public CharacterViewModel()
        {
            Origin = new OriginViewModel();
            Relationships = new List<RelationshipViewModel>();
            Personality = new PersonalityViewModel();
            Appearance = new AppearanceViewModel();
        }

        public override string ToString()
        {
            return Name + " " + Surname;
        }
    }   
}
