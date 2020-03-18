using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<BookOrigin> BookOrigins { get; set; }
    }
}
