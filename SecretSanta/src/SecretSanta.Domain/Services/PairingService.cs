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

        public async Task<bool> GeneratePairingsAsync(int groupId)
        {
            Group group = await DbContext.Groups
                .Include(x => x.GroupUsers)
                .SingleOrDefaultAsync(x => x.Id == groupId);

            List<int> ids = group?.GroupUsers?.Select(x => x.UserId).ToList();

            if (ids == null || ids.Count < 2)
            {
                return false;
            }

            List<Pairing> pairings = await Task.Run(() => GetPairings(ids));

            await DbContext.Pairings.AddRangeAsync(pairings);
            await DbContext.SaveChangesAsync();

            return true;
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
