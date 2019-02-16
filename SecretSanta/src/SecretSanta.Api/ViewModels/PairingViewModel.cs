using System.ComponentModel.DataAnnotations;

namespace SecretSanta.Api.ViewModels
{
    public class PairingViewModel
    {
        public int Id { get; set; }
        [Required]
        public int SantaId { get; set; }
        [Required]
        public int RecipientId { get; set; }
    }
}
