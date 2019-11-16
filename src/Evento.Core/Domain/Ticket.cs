using System;

namespace Evento.Core.Domain
{
    public class Ticket : Entity
    {
        //  public string Name { get; protected set; }
        public Guid EventId { get; protected set; }
        public int Seating { get; protected set; }
        public decimal Price { get; protected set; }
        public Guid? UserId { get; protected set; }
        public string UserName { get; protected set; }
        public DateTime? PurchasedAt { get; protected set; }
        //readonly
        public bool IsPurchased => UserId.HasValue;

        protected Ticket()
        {

        }

        public Ticket(Event @event, int seating, decimal price)
        {
            EventId = @event.Id;
            SetSeating(seating);
            SetPrice(price);
        }

        public void PurchaseTickets(User user)
        {
            if (user == null)
            {
                throw new NullReferenceException($"Not found user");
            }
            else if (IsPurchased)
            {
                throw new Exception($"Ticket was already purchased by user: '{UserName}' at {PurchasedAt}.");
            }
            UserId = user.Id;
            UserName = user.Name;
            PurchasedAt = DateTime.UtcNow;
        }

        public void CancelTicket()
        {
            if (!IsPurchased)
            {
                throw new Exception($"Tikcet was not purchased and can not by canceled.");
            }
            UserId = null;
            UserName = null;
            PurchasedAt = null;
        }

        public void SetSeating(int numberOfSeating)
        {
            Seating = numberOfSeating;
        }

        public void SetPrice(decimal price)
        {
            if(price <0)
            {
                throw new Exception($"Price can not have  value under zero like '{price}'");
            }
            Price = price;
        }
    }
}