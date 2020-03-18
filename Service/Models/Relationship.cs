using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class Relationship
    {
        public int Id { get; set; }
        public Character TargetRelationshipCharacter { get; set; }       
        public string Type { get; set; }
        public int CharacterId { get; set; }
        public virtual Character Character { get; set; }
    }
}
