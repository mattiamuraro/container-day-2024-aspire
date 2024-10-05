using Cday24.Aspire.BackofficeApi.HttpClients;
using Cday24.Aspire.BackofficeApi.Services;
using Cday24.Aspire.Models.Messages;
using MassTransit;

namespace Cday24.Aspire.BackofficeApi.Consumers.Handlers
{

    public class TicketRequestMessageConsumer : IConsumer<TicketCreationMessage>
    {
        private readonly ILogger<TicketRequestMessageConsumer> _logger;
        private readonly WeightApiClient _weightApiClient;
        private readonly TicketService _ticketService;
        private readonly OpenAiService _openAiService;

        public TicketRequestMessageConsumer(ILogger<TicketRequestMessageConsumer> logger, WeightApiClient weightApiClient, TicketService ticketService, OpenAiService openAiService)
        {
            _logger = logger;
            _weightApiClient = weightApiClient;
            _ticketService = ticketService;
            _openAiService = openAiService;
        }

        public async Task Consume(ConsumeContext<TicketCreationMessage> context)
        {
            var ticketRequest = context.Message;

            if (ticketRequest == null)
            {
                _logger.LogError("Empty message received");
                return;
            }

            _logger.LogInformation($"Message with ticket id {ticketRequest.Id} received");
            var ticketWeight = await _weightApiClient.GetWeightAsync(ticketRequest.Title, ticketRequest.Description);
            _logger.LogInformation($"Ticket with id {ticketRequest.Id} received a weight of {ticketWeight.Value}");
            var explaination = _openAiService.GetTicketExplanation(ticketRequest, ticketWeight.Value);

            await _ticketService.CreateTicketASync(ticketRequest, explaination, ticketWeight.Value);
        }
    }
}