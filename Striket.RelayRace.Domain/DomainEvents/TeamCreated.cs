using System;

namespace Striker.RelayRace.Domain.DomainEvents
{
    public class TeamCreated : DomainEvent
    {
        public Guid TeamId { get; set; }

        public Guid RaceId { get; set; }
    }
}