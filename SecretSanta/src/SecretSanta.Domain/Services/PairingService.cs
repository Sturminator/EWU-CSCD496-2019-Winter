using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Domain.Services
{
    public class PairingService : IPairingService
    {
        private ApplicationDbContext DbContext { get; }

        public async Task<List<Pairing>> GeneratePairings(int groupId)
        {
            Group group = await DbContext.Groups
                .Include(x => x.GroupUsers)
                .SingleOrDefaultAsync(x => x.Id == groupId);

            List<int> ids = group?.GroupUsers?.Select(x => x.UserId).ToList();

            List<Pairing> pairings = new List<Pairing>();

            if (ids.Count > 1)
            {
                pairings = await Task.Run(() => GetPairings(ids));

                await DbContext.Pairings.AddRangeAsync(pairings);
                await DbContext.SaveChangesAsync();
            }

            return pairings;
        }

        private List<Pairing> GetPairings(List<int> ids)
        {
            var pairings = new List<Pairing>();

            for (int i = 0; i < ids.Count; i++)
            {
                pairings.Add(new Pairing
                {
                    SantaId = ids[i],
                    RecipientId = ids[i + 1]
                });
            }

            pairings.Add(new Pairing
            {
                SantaId = ids.Last(),
                RecipientId = ids.First()
            });

            return pairings;
        }
    }
}
