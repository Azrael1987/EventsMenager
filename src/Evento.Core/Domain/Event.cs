using System;
using System.Collections.Generic;
using System.Linq;

namespace Evento.Core.Domain
{
    public class Event : Entity
    {
        private ISet<Ticket> _tickets = new HashSet<Ticket>();
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public DateTime CreateAt { get; protected set; }
        public DateTime StartDate { get; protected set; }
        public DateTime EndDate { get; protected set; }
        public DateTime UpdateAt { get; protected set; }
        public IEnumerable<Ticket> Tickets => _tickets;
        /// <summary> kolekcja sprzedanych biletów </summary>
        public IEnumerable<Ticket> PurchasedTickets => Tickets.Where(x => x.IsPurchased);
        /// <summary> kolekcja dostêpnych biletów </summary>
        public IEnumerable<Ticket> AvailableTickets => Tickets.Except(PurchasedTickets);
        // public IEnumerable<Ticket> AvailableTickets => Tickets.Where(x => !x.IsPurchased);

        protected Event()
        {

        }

        public Event(Guid id, string name, string description, DateTime startDate, DateTime endDate)
        {
            Id = id;
            SetName(name);
            SetDescription(description);
            CreateAt = DateTime.UtcNow;
            SetEventsDates(startDate, endDate);
            UpdateAt = CreateAt;
        }

        public void SetEventsDates(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new Exception($"Event with id: {Id} must have end date letter than start date or in the some day");
            }
            if(endDate < DateTime.UtcNow)
            {
                throw new Exception($"Time machine dose not exsist. Set date in future");
            }
            StartDate = startDate;
            EndDate = endDate;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception($"Event with id: '{Id}' can not have an empty name or white space.");
            }
            Name = name;
            UpdateAt = DateTime.UtcNow;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new Exception($"Event with id: '{Id}' can not have an empty description or white space.");
            }
            Description = description;
            UpdateAt = DateTime.UtcNow;
        }

        public void AddTickets(int amount, decimal price)
        {
            int emptySeating = _tickets.Count + 1;
            if(amount < 10)
            {
                throw new Exception($"Min amount of seating is 10, bust have just {amount}");
            }
            for (int i = 0; i < amount; i++)
            {
                _tickets.Add(new Ticket(this, emptySeating, price));
                emptySeating++;
            }
        }

        public void PurchaseTickets(User user, int amount)
        {
            if (amount > AvailableTickets.Count())
            {
                throw new Exception($"Not enought ticket(s) for this event. We have only {AvailableTickets.Count()} availble ticket(s).");
            }
            var tickets = AvailableTickets.Take(amount);
            foreach (var ticket in tickets)
            {
                ticket.PurchaseTickets(user);
            }
        }

        public void CancelTickets(User user, int amount)
        {
            var tickets = GetTicketsPurchasedByUser(user);
            if (amount > tickets.Count())
            {
                throw new Exception($"You do not have enought tickets. You have only {tickets.Count()}");
            }
            foreach (var ticket in tickets.Take(amount))
            {
                ticket.CancelTicket();
            }
        }

        public IEnumerable<Ticket> GetTicketsPurchasedByUser(User user)
             => PurchasedTickets.Where(x => x.UserId == user.Id);
    }
}