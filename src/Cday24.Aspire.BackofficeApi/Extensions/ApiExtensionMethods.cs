using Cday24.Aspire.BackofficeApi.Services;

namespace Cday24.Aspire.BackofficeApi.Extensions
{
    internal static class ApiExtensionMethods
    {
        public static IEndpointRouteBuilder MapApplicationEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/ticket", (TicketService ticketService) => ticketService.GetTickets());

            return app;
        }
    }
}