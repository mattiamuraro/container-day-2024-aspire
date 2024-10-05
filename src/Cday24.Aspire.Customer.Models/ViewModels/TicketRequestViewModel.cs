namespace Cday24.Aspire.Customer.Models.ViewModels
{
    public class TicketRequestViewModel
    {
        public TicketRequestViewModel()
        {
            Title = "";
            Description = "";
            Creator = "Mattia Muraro";
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
    }
}
