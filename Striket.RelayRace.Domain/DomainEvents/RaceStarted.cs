using System;

namespace Striker.RelayRace.Domain.DomainEvents
{
    public class RaceStarted : DomainEvent
    {
        public Guid RaceId { get; set; }

        public DateTime Date { get; set; }
    }
}
