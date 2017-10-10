using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Striker.RelayRace.SqlNh
{
    [Table("Laps")]
    public class Lap
    {
        [Key]
        public Guid LapId { get; set; }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }
    }
}
