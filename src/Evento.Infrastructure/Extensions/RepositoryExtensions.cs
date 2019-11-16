using Evento.Core.Domain;
using Evento.Core.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Evento.Infrastructure.Extensions
{
    public static class RepositoryExtensions
    {
        public static async Task<Event> GetOrFailAsync(this IEventRepository repository, Guid eventId)
        {
            var @event = await repository.GetAsync(eventId);
            if (@event == null)
            {
                throw new Exception($"Event with id '{eventId}' does not exists.");
            }
            return @event;
        }

        public static async Task<Event> GetOrFailAsync(this IEventRepository reposytory, string name)
        {
            var @event = await reposytory.GetAsync(name);
            if (@event == null)
            {
                throw new Exception($"Event named '{name}' does not exists.");
            }
            return @event;
        }

        public static async Task<Event> CheckOrFailAsync(this IEventRepository repository, string eventName)
        {
            var @event = await repository.GetAsync(eventName);
            if (@event != null)
            {
                throw new Exception($"Event named: '{eventName}' already exists.");
            }
            return @event;
        }

        public static async Task<User> GetOrFailAsync(this IUserRepository reposytory, Guid id)
        {
            var user = await reposytory.GetAsync(id);
            if (user == null)
            {
                throw new Exception($"User with id: '{id}' does not exists.");
            }
            return user;
        }

        public static async Task<User> GetForEmailOrFailAsync(this IUserRepository reposytory, string email)
        {
            var user = await reposytory.GetForEmailAsync(email);
            if (user == null)
            {
                throw new Exception($"User with id: '{email}' does not exists.");
            }
            return user;
        }

        public static async Task<Ticket> GetTicketOrFailAsync(this IEventRepository repository, Guid eventId, Guid ticketId)
        {
            var @event = await repository.GetOrFailAsync(eventId);
            var ticket = @event.Tickets.SingleOrDefault(x => x.Id == ticketId);
            if (ticket == null)
            {
                throw new Exception($"Ticket with  id: '{ticketId}' was found for event: '{@event.Name}'");
            }
            return ticket;
        }
    }
}
