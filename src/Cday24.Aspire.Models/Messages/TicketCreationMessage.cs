namespace Cday24.Aspire.Models.Messages
{
    public class TicketCreationMessage
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Creator { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}
