using System;

namespace Striker.RelayRace.Domain
{
    public class TeamStatistics
    {
        public string TeamName { get; }

        public Guid TeamId { get; }

        public int TotalDistanceInMetersCompleted { get; }

        public int NumberOfLapsCompleted { get; }

        public string Pace { get; }

        public TeamStatistics(
            Guid teamId,
            string teamName, 
            int totalDistanceInMetersCompleted, 
            int numberOfLapsCompleted,
            TimeSpan elapsedTime)
        {
            this.TeamId = teamId;
            this.TeamName = teamName;
            this.TotalDistanceInMetersCompleted = totalDistanceInMetersCompleted;
            this.NumberOfLapsCompleted = numberOfLapsCompleted;

            var kilometers = this.TotalDistanceInMetersCompleted / (double)1000;
            var seconds = elapsedTime.TotalSeconds;

            var secondsPerKilometer = seconds / kilometers;

            var span = new TimeSpan(0, 0, 0, (int)secondsPerKilometer);

            this.Pace = $"{span.Minutes}:{span.Seconds} per km";
        }


    }
}
