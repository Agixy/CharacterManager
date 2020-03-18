using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharactersManager.Models
{
    public class PersonalityViewModel
    {
        public string CharacterTraits { get; set; }
        public string Values { get; set; }
        public string Ambitions { get; set; }
        public string Goals { get; set; }
        public int TypeOfCharacterId { get; set; }
        public int OrientationId { get; set; }
        public int AligmentChartId { get; set; }
        public string PoliticalViews { get; set; }
        public string TheBiggestFear { get; set; }
        public string Intrestings { get; set; }
        public string FavouriteQuote { get; set; }
        public string AdditionalInformation { get; set; }
    }
}
