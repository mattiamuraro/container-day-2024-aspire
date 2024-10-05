using Cday24.Aspire.Customer.Models.ViewModels;
using Cday24.Aspire.CustomerApi.Extensions;
using Cday24.Aspire.Models.Messages;
using Cday24.Aspire.Models.Options;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Cday24.Aspire.CustomerApi.Services
{
    public class TicketService
    {
        private ILogger<TicketService> _logger;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly string _queueName;
        public TicketService(ILogger<TicketService> logger, IOptions<RabbitMqSettings> rabbitMqSettingsOptions, ISendEndpointProvider sendEndpointProvider)
        {
            var rabbitMqSettings = rabbitMqSettingsOptions.Value;

            _logger = logger;
            _sendEndpointProvider = sendEndpointProvider;
            _queueName = rabbitMqSettings.QueueName;
        }

        public async Task<RequestResult<Guid>> CreateTicketAsync(TicketRequestViewModel request, CancellationToken cancellationToken = default)
        {
            try
            {
                var message = request.ToTicketRequestMessage();
                await WriteMessageOnQueue(message);

                return RequestResult<Guid>.Success(message.Id);

            }
            catch (Exception err)
            {
                _logger.LogError(err, "Error during ticket creation");

                return RequestResult<Guid>.Fail(err.Message);
            }
        }

        private async Task WriteMessageOnQueue(TicketCreationMessage message)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{_queueName}"));
            await endpoint.Send(message);

            _logger.LogInformation($"Message written on queue {_queueName} for ticket with id {message.Id}");
        }
    }
}
