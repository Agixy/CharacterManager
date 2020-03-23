using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class Image
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public bool IsAvatar { get; set; }
        public string ImageTitle { get; set; }
        public byte[] ImageData { get; set; }
    }
}
