using System;

namespace Striker.RelayRace.Domain
{
    public class LapStatistic
    {
        public string TeamName { get; }

        public Guid RaceId { get; }

        public Guid TeamId { get; }

        public int DistanceInMeters { get; }

        public string Pace { get; }

        public TimeSpan Length { get; }

        public DateTime CompletedOn { get; }

        public LapStatistic(
            Guid raceId,
            Guid teamId,
            string teamName,
            int distanceInMeters,
            TimeSpan length,
            DateTime completedOn)
        {
            this.RaceId = raceId;
            this.TeamId = teamId;
            this.TeamName = teamName;
            this.DistanceInMeters = distanceInMeters;
            this.Length = length;
            this.CompletedOn = completedOn;

            var kilometers = this.DistanceInMeters / (double)1000;
            var seconds = this.Length.TotalSeconds;

            var secondsPerKilometer = seconds / kilometers;

            var span = new TimeSpan(0, 0, 0, (int) secondsPerKilometer);

            this.Pace = $"{span.Minutes}:{span.Seconds} per km";
        }
    }
}
