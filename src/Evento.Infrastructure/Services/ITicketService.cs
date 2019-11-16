using Evento.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evento.Infrastructure.Services
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDetailsDto>>GetTicktesForUserAsync(Guid userId);
        Task<TicketDto> GetAsync(Guid userId, Guid eventId, Guid ticketId);
        Task PurchaseTicketsAsync(Guid userId, Guid eventId, int amount);
        Task CancelTicketAsync(Guid userId, Guid eventId, int amount);
    }
}
