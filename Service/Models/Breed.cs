using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Service.Models
{
    public class Breed
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Appearance> Appearances { get; set; }
    }
}
