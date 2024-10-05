using Cday24.Aspire.BackofficeApi.Consumers.Handlers;
using Cday24.Aspire.Models.Messages;
using Cday24.Aspire.Models.Options;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Cday24.Aspire.BackofficeApi.Consumers.Definitions
{
    public class TicketRequestMessageDefinition : ConsumerDefinition<TicketRequestMessageConsumer>
    {
        public TicketRequestMessageDefinition(IOptions<RabbitMqSettings> rabbitMqSettingsOptions)
        {
            var rabbitMqSettings = rabbitMqSettingsOptions.Value;

            EndpointName = rabbitMqSettings.QueueName;
            EndpointConvention.Map<TicketCreationMessage>(new Uri($"queue:{rabbitMqSettings.QueueName}"));
        }
    }
}
