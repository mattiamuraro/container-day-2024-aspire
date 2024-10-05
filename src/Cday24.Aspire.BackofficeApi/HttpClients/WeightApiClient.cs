using Cday24.Aspire.BackofficeApi.Models;

namespace Cday24.Aspire.BackofficeApi.HttpClients
{
    public class WeightApiClient(HttpClient httpClient)
    {
        public async Task<TicketWeight> GetWeightAsync(string title, string description, CancellationToken cancellationToken = default)
        {
            var result = await httpClient.GetFromJsonAsync<TicketWeight>("/weight", cancellationToken);

            if (result == null)
                throw new Exception("Remote service unvailable");

            return result;
        }
    }
}
