using System.ComponentModel.DataAnnotations.Schema;

namespace Service.Models
{
    public class Appearance
    {
        [ForeignKey("Character")]
        public int Id { get; set; }
        public double Tall { get; set; }
        public string BodyType { get; set; }
        public string Description { get; set; }
        public  int BreedId { get; set; }
     //   public Character Character { get; set; }
    }
}
