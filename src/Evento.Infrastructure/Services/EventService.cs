using AutoMapper;
using Evento.Core.Domain;
using Evento.Core.Repositories;
using Evento.Infrastructure.DTO;
using Evento.Infrastructure.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evento.Infrastructure.Services
{
    public class EventService : IEventService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger(); // NLog
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EventService> _logger;  // .Net Logger
        private readonly IMemoryCache _memoryCache;

        //public IActionResult Index()
        //{
        //    _logger.LogInformation("Index page for test NLog");
        //    return View();
        //}

        public EventService(IEventRepository eventRepository, IMapper mapper, ILogger<EventService> logger, IMemoryCache memoryCache)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _logger = logger;
            _memoryCache = memoryCache;
        }
        public async Task<EventDetailsDto> GetAsync(Guid id)
        {
            var @event = await _eventRepository.GetAsync(id);
            return _mapper.Map<EventDetailsDto>(@event);
        }

        public async Task<EventDetailsDto> GetAsync(string name)
        {
            var @event = await _eventRepository.GetAsync(name);
            return _mapper.Map<EventDetailsDto>(@event);
        }


        public async Task<IEnumerable<EventDto>> BrowseAsync(string name = null)
        {
            _logger.LogTrace("Fetchnig events");  //z .Net Core
            Logger.Info("Logg for test from NLog"); //z NLogu

            string key = "events";
            IEnumerable<Event> events;

            if (!_memoryCache.TryGetValue(key, out events))
            {
                events = await _eventRepository.BrowseAsync(name);
                _memoryCache.Set(key, events, TimeSpan.FromSeconds(90));
                Logger.Info("Fetching events form repository");
            }
            else
            {
                Logger.Info("Fetching events from cache"); // for test
            }
            // if U don't need check TryGetValue
            // var events = _memoryCache.Get<IEnumerable<Event>>("events"); //klucz dla cache'a

            // if You want GetOrCreate cache - not perfect implementation but work ;)
            /*
            events = await _eventRepository.BrowseAsync(name);
            events = _memoryCache.GetOrCreate<IEnumerable<Event>>(key, cacheEntry => { return  events; });
            */
            return _mapper.Map<IEnumerable<EventDto>>(events);
        }

        public async Task DeleteAsync(Guid id)
        {
            var @event = await _eventRepository.GetOrFailAsync(id);
            await _eventRepository.DeleteAsync(@event);
        }

        public async Task CreateAsync(Guid id, string name, string description, DateTime startDate, DateTime endDate)
        {
            var @event = await _eventRepository.CheckOrFailAsync(name);

            @event = new Event(id, name, description, startDate, endDate);
            Logger.Info($"Event '{name}' was create.");
            await _eventRepository.AddAsync(@event);
        }

        public async Task UpdateAsync(Guid id, string name, string description)
        {
            var @event = await _eventRepository.GetOrFailAsync(id);
            var @tempEvent = await _eventRepository.CheckOrFailAsync(name);

            if (@tempEvent != null)
            {
                throw new Exception($"Two events coulnd not have the same name like {@event.Name} and {tempEvent.Name}");
            }
            @event.SetName(name);
            @event.SetDescription(description);
            await _eventRepository.UpdateAsync(@event);
        }

        public async Task AddTicketsAsync(Guid eventId, int amount, decimal price)
        {
            var @event = await _eventRepository.GetOrFailAsync(eventId);
            @event.AddTickets(amount, price);
            await _eventRepository.UpdateAsync(@event);
        }
    }
}