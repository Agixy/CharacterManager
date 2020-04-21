using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharactersManager.Models
{
    public class ImageViewModel
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public bool IsAvatar { get; set; }
        public string ImageTitle { get; set; }
        public string ImageData { get; set; }
    }
}
