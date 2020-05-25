namespace Service.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Sex { get; set; }
        public virtual Origin Origin { get; set; }
        public string Profesion { get; set; }
        public int BirthErrar { get; set; }
        public string Relationships { get; set; }
        public  Personality Personality { get; set; }
        public Appearance Appearance { get; set; }
    }
}
