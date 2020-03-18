using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class BookOrigin
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int OriginId { get; set; }
        public Origin Origin { get; set; }
    }
}
