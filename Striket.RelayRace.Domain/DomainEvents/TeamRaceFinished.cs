using System;

namespace Striker.RelayRace.Domain.DomainEvents
{
    public class TeamRaceFinished : DomainEvent
    {
        public Guid TeamId { get; set; }

        public DateTime Date { get; set; }
    }
}
