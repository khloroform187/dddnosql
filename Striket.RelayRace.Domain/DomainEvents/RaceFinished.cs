using System;

namespace Striker.RelayRace.Domain.DomainEvents
{
    public class RaceFinished : DomainEvent
    {
        public Guid RaceId { get; set; }

        public DateTime Date { get; set; }
    }
}
