using System;
using System.Collections.Generic;
using System.Linq;
using Striker.RelayRace.Domain.DomainEvents;

namespace Striker.RelayRace.Domain
{
    public class Race
    {
        public Guid Id { get; private set; } // must have private set for reflection set

        public string Name { get; private set; }

        public int LapDistanceInMeters { get; }

        public DateTime? Start { get; private set; }

        public DateTime? End { get; private set; }

        public IReadOnlyCollection<Guid> TeamIds
        {
            get { return this._teamIds; }
            private set { this._teamIds = value.ToList(); } // must have private set for reflection set
        } 

        private List<Guid> _teamIds { get; set; } // must have private set for reflection set

        public List<string> ChipIds { get; }

        public List<DomainEvent> Events { get; }

        public Race(
            string name,
            int lapDistanceInMeters,
            List<string> chipIds)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.LapDistanceInMeters = lapDistanceInMeters;
            this.Start = null;
            this.End = null;
            this._teamIds = new List<Guid>();
            this.ChipIds = chipIds;
            this.Events = new List<DomainEvent>();
        }

        public void Rename(string newName)
        {
            this.Name = newName;
        }

        public void RegisterTeam(Guid teamId)
        {
            if (this._teamIds.Contains(teamId))
            {
                throw new Exception($"Team {teamId} already registered");
            }

            this._teamIds.Add(teamId);
        }

        public void StartRace(DateTime start)
        {
            this.Start = start;

            this.Events.Add(new RaceStarted { Date = this.Start.Value, RaceId = this.Id });
        }

        public void EndRace(DateTime end)
        {
            this.End = end;

            this.Events.Add(new RaceFinished { Date = this.End.Value, RaceId = this.Id });
        }

        public TimeSpan Elapsed()
        {
            return DateTime.UtcNow - this.Start.Value;
        }

        public TimeSpan ElapsedForTesting(DateTime now)
        {
            return now - this.Start.Value;
        }
    }
}
