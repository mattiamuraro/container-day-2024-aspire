namespace Cday24.Aspire.Data.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string AiExplanation { get; set; }
        public required string Creator { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public int Weight { get; set; }
    }
}
