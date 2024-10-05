using Cday24.Aspire.Data.Models;
using Cday24.Aspire.Models.Messages;

namespace Cday24.Aspire.BackofficeApi.Extensions
{
    internal static class TicketServiceExtensionMethods
    {
        public static Ticket ToTicket(this TicketCreationMessage ticketRequest, int ticketWeight, string explaination)
        {
            return new Ticket
            {
                Id = ticketRequest.Id,
                Title = ticketRequest.Title,
                Description = ticketRequest.Description,
                AiExplanation = explaination,
                Creator = ticketRequest.Creator,
                CreationDate = ticketRequest.CreationDate.UtcDateTime,
                Weight = ticketWeight
            };
        }
    }
}
