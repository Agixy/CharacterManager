
using System.Collections.Generic;

namespace CharactersManager.Models
{
    public class CharacterViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DisplayName 
        {
            get => Name + " " + Surname;
            private set { }
        }
        public string Sex { get; set; }        
        public OriginViewModel Origin { get; set; }
        public string Profesion { get; set; } 
        public int BirthErrar { get; set; }
        public List<string> Relationships { get; set; }
        public PersonalityViewModel Personality { get; set; }
        public AppearanceViewModel Appearance { get; set; }
        public List<ImageViewModel> Images { get; set; }

        public CharacterViewModel()
        {
            Origin = new OriginViewModel();
            Relationships = new List<string>();
            Personality = new PersonalityViewModel();
            Appearance = new AppearanceViewModel();
            Images = new List<ImageViewModel>();
        }
    }   
}
