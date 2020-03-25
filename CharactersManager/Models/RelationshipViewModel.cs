using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharactersManager.Models
{
    public class RelationshipViewModel
    {
        public int CharacterId { get; set; }   
        public string TargetRelationshipCharacterName { get; set; }
        public string Type { get; set; }
    }
}
