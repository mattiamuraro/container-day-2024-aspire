using Cday24.Aspire.BackofficeApi.Extensions;
using Cday24.Aspire.Data;
using Cday24.Aspire.Data.Models;
using Cday24.Aspire.Models.Messages;
using Microsoft.EntityFrameworkCore;

namespace Cday24.Aspire.BackofficeApi.Services
{
    public class TicketService
    {
        private readonly ILogger<TicketService> _logger;
        private readonly Cday24DbContext _dbContext;
        public TicketService(ILogger<TicketService> logger, Cday24DbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task CreateTicketASync(TicketCreationMessage ticketRequest, string explaination, int ticketWeight, CancellationToken cancellationToken = default)
        {
            var ticket = ticketRequest.ToTicket(ticketWeight, explaination);
            _dbContext.Tickets.Add(ticket);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Ticket with id {ticket.Id} added to database");
        }

        public List<Ticket> GetTickets()
        {
            _logger.LogInformation($"Backoffice user request all tickets");

            return _dbContext.Tickets.AsNoTracking().ToList();
        }
    }
}
