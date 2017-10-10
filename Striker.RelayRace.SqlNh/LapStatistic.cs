using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Striker.RelayRace.SqlNh
{
    [Table("LapStatistics")]
    public class LapStatistic
    {
        [Key]
        public Guid Id { get; set; }

        public string TeamName { get; set; }

        public Guid RaceId { get; set; }

        public Guid TeamId { get; set; }

        public int DistanceInMeters { get; set; }

        public string Pace { get; set; }

        public string Length { get; set; }

        public DateTime CompletedOn { get; set; }
    }
}