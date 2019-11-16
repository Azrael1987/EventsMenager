using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evento.Infrastructure.Services
{

    public class DateInitializer : IDateInitializer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IEventService _eventService;
        private readonly IUserService _userService;

        public DateInitializer(IUserService userService, IEventService eventService)
        {
            _userService = userService;
            _eventService = eventService;
        }

        public async Task SeedAsync()
        {
            Logger.Info("Initializing date.");
            var tasks = new List<Task>();
            tasks.Add(_userService.RegistryAsync(new Guid(), "admin2@poczta.pl", "Admin2", "superHasło%1", "admin"));
            tasks.Add(_userService.RegistryAsync(new Guid(), "user2@poczta.pl", "User2", "tajneHAsło*3", "user"));
            tasks.Add(_userService.RegistryAsync(new Guid(), "moderator@poczta.pl", "Moderator2", "sekretneHAsło@2", "moderator"));
            Logger.Info($"Create users: 'Moderator2', 'User2' and 'Admin2'");

            for (int i = 0; i < 5; i++)
            {
                var eventId =Guid.NewGuid();
                tasks.Add(_eventService.CreateAsync(eventId, $"Event no {i + 1}", $"Event no {i + 1} - description", DateTime.UtcNow.AddDays(1 + i), DateTime.UtcNow.AddDays(1 + i).AddHours(2 * i)));
                tasks.Add(_eventService.AddTicketsAsync(eventId, 55 + 10 * i, 100.50m));
                Logger.Info($"Create event 'Event no {i + 1}'");
            }
            await Task.WhenAll(tasks);
            Logger.Info("Date initializating was finish.");
        }
    }
}
