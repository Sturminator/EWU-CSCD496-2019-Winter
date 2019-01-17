using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
    public class PairingService
    {
        private SecretSantaDbContext DbContext { get; set; }

        public PairingService(SecretSantaDbContext context)
        {
            DbContext = context;
        }

        public Pairing AddPairing(Pairing pairing)
        {
            DbContext.Pairings.Add(pairing);
            DbContext.SaveChanges();

            return pairing;
        }
    }
}
