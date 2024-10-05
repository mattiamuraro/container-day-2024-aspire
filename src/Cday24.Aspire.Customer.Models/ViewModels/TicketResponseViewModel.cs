namespace Cday24.Aspire.Customer.Models.ViewModels
{
    public record TicketResponseViewModel(Guid Id, string Title, string Description, DateTimeOffset CreationDate);
}
