using Cday24.Aspire.Customer.Models.ViewModels;
using Cday24.Aspire.CustomerApi.Services;

namespace Cday24.Aspire.CustomerApi.Extensions
{
    internal static class ApiExtensionMethods
    {
        public static IEndpointRouteBuilder MapApplicationEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/ticket", async (TicketRequestViewModel request, TicketService ticketService) => await ticketService.CreateTicketAsync(request));

            return app;
        }
    }
}
