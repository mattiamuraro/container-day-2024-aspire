using Cday24.Aspire.Customer.Models.ViewModels;
using System.Text.Json;

namespace Cday24.Aspire.CustomerApp.HttpClients
{
    public class CustomerApiClient(HttpClient httpClient, ILogger<CustomerApiClient> logger)
    {
        public async Task<bool> CreateNewTicketAsync(TicketRequestViewModel request, CancellationToken cancellationToken = default)
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync("/ticket", request, JsonSerializerOptions.Default, cancellationToken);
            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RequestResult<Guid>>(cancellationToken);

            if (response == null)
            {
                logger.LogError($"Ticket creation failed. The response is empty.");
                return false;
            }
            else if (!response.Result)
            {
                logger.LogError($"Ticket creation failed with message: {response.Message}");
                return false;
            }

            logger.LogInformation($"Ticket created with id {response.Value}");
            return true;
        }
    }
}
