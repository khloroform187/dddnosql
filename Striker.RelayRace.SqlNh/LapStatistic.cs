using System;

namespace Striker.RelayRace.SqlNh
{
    public class LapStatistic
    {
        public string TeamName { get; set; }

        public string RaceId { get; set; }

        public string TeamId { get; set; }

        public int DistanceInMeters { get; set; }

        public string Pace { get; set; }

        public TimeSpan Length { get; set; }

        public DateTime CompletedOn { get; set; }
    }
}