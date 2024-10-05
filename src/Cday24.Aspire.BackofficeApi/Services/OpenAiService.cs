using Cday24.Aspire.Models.Messages;
using Cday24.Aspire.Models.Options;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;

namespace Cday24.Aspire.BackofficeApi.Services
{
    public class OpenAiService
    {
        private readonly ILogger<OpenAiService> _logger;
        private readonly ChatClient _chatClient;

        public OpenAiService(ILogger<OpenAiService> logger, OpenAIClient openAIClient, IOptions<AzureOpenAiSettings> azureOpenAiSettingsOptions)
        {
            var azureOpenAiSettings = azureOpenAiSettingsOptions.Value;

            _chatClient = openAIClient.GetChatClient(azureOpenAiSettings.DeploymentName);
            _logger = logger;
        }

        public string GetTicketExplanation(TicketCreationMessage message, int ticketWeight)
        {
            var chatMessages = GetChatMessages(message, ticketWeight);
            ChatCompletion completion = _chatClient.CompleteChat(chatMessages);

            _logger.LogInformation($"Explanation for ticket with id {message.Id} received");

            return $"{completion.Content[0].Text}";
        }

        private static ChatMessage[] GetChatMessages(TicketCreationMessage message, int ticketWeight)
        {
            return [
                // System messages represent instructions or other guidance about how the assistant should behave
                new SystemChatMessage("You are a helpful assistant that provides suggestion about ticket resolution."),
                // User messages represent user input, whether historical or the most recen tinput
                new UserChatMessage($"What you suggest to a colleague that receive a ticket with the info  below? \n" +
                                    $"- title {message.Title} \n" +
                                    $"- description: {message.Description} \n" +
                                    $"- weight (available value from 1 to 1000): {ticketWeight} \n" +
                                    $"- Customer: {message.Creator}\n" +
                                    $"- Creation date: {message.CreationDate}"),
                                    // Our current customer is {message.Creator} and send is request on {message.CreationDate}
                new SystemChatMessage("Provide the response in Italian with a deep description off possible resolution."),
            // Assistant messages in a request represent conversation history for responses
            ];
        }
    }
}
