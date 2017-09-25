using System;

namespace Striker.RelayRace.Domain.DomainEvents
{
    public class LapCompleted : DomainEvent
    {
        public Guid RaceId { get; set; }

        public Guid TeamId { get; set; }

        public string TeamName { get; set; }

        public TimeSpan LapLength { get; set; }

        public DateTime CompletedOn { get; set; }
    }
}
