using System;

namespace Striker.RelayRace.Domain.DomainEvents
{
    public class TeamRaceStarted : DomainEvent
    {
        public Guid TeamId { get; set; }

        public DateTime StartDate { get; set; }
    }
}
