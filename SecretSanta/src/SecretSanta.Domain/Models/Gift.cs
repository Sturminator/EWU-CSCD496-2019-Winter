namespace SecretSanta.Domain.Models
{
    public class Gift : Entity
    {
        public string Title { get; set; }
        public int OrderOfImportance { get; set; }
        public string URL { get; set; }
        public string Description { get; set; }
        public User User { get; set; }
    }
}
