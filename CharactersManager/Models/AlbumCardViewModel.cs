using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharactersManager.Models
{
    public class AlbumCardViewModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public ImageViewModel Avatar { get; set; }
    }
}
