using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharactersManager.Models
{
    public class OriginViewModel
    {
        public string DistrictOfResidence { get; set; }
        public string DistrictOfBirth { get; set; }
        public int FatherId { get; set; }
        public int MotherId { get; set; }
        public IList<int> BookIds { get; set; }
        public int CharacterId { get; set; }

        public OriginViewModel()
        {
            BookIds = new List<int>() { 3, 1};
        }
    }

}
