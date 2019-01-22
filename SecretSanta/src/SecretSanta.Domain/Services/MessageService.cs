using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
    public class MessageService
    {
        private SecretSantaDbContext DbContext { get; set; }

        public MessageService(SecretSantaDbContext context)
        {
            DbContext = context;
        }

        public Message StoreMessage(Message message)
        {
            DbContext.Messages.Add(message);
            DbContext.SaveChanges();

            return message;
        }
    }
}
