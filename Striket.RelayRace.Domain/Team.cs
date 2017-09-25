using System;
using System.Collections.Generic;
using System.Linq;
using Striker.RelayRace.Domain.DomainEvents;

namespace Striker.RelayRace.Domain
{
    public class Team
    {
        public Guid Id { get; private set; }// must have private set for reflection set

        public string Name { get; }

        public Guid RaceId { get; }

        public string ChipId { get; }

        public List<DomainEvent> Events { get; }

        public IReadOnlyCollection<Lap> Laps
        {
            get { return this._laps; }
            private set { this._laps = value.ToList(); } // must have private set for reflection set
        }

        private List<Lap> _laps { get; set; }

        public Team(
            string name,
            Guid raceId,
            string chipId)
        {
            this.Name = name;
            this.RaceId = raceId;
            this.Id = Guid.NewGuid();
            this.ChipId = chipId;
            this._laps = new List<Lap>();
            this.Events = new List<DomainEvent>();

            this.Events.Add(new TeamCreated {RaceId = this.RaceId, TeamId = this.Id });
        }

        public void StartRace(DateTime start)
        {
            var lap = new Lap(start);
            this._laps.Add(lap);
        }

        public TimeSpan LapCompleted(DateTime date)
        {
            var length = this.StopLastLap(date);

            var lap = new Lap(date);
            this._laps.Add(lap);

            return length;
        }

        public void FinishRace()
        {
            var lastLap = this._laps.Last();

            if (!lastLap.IsDone())
            {
                this._laps.Remove(lastLap);
            }
        }

        private TimeSpan StopLastLap(DateTime date)
        {
            var lastLap = this._laps.Last();
            lastLap.EndLap(date);

            var length = lastLap.Length;

            return length;
        }
    }
}
