using Cday24.Aspire.Customer.Models.ViewModels;
using Cday24.Aspire.Models.Messages;

namespace Cday24.Aspire.CustomerApi.Extensions
{
    internal static class TicketServiceExtensionMethods
    {
        public static TicketCreationMessage ToTicketRequestMessage(this TicketRequestViewModel request)
        {
            return new TicketCreationMessage
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Creator = request.Creator,
                CreationDate = DateTimeOffset.Now
            };
        }
    }
}
