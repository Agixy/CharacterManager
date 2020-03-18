using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class Orientation
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Personality> Personalities { get; set; }
    }
}
