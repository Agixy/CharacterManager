using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Service.Models
{
    public class Origin
    {
        [ForeignKey("Character")]
        public int Id { get; set; }
        public string DistrictOfResidence { get; set; }
        public string DistrictOfBirth { get; set; }
        public int FatherId { get; set; }
        public int MotherId { get; set; }
    }
}
