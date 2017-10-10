using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Striker.RelayRace.SqlNh
{
    [Table("Races")]
    public class Race
    {
        [Key]
        public Guid RaceId { get; set; }

        public string Name { get; set; }

        public int LapDistanceInMeters { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public string TeamIds { get; set; }

        public string ChipIds { get; set; }
    }
}
