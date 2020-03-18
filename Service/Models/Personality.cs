using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Service.Models
{
    public class Personality
    {
        [ForeignKey("Character")]
        public int Id { get; set; }
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
