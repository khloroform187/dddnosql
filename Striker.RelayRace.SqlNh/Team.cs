using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Striker.RelayRace.SqlNh
{
    [Table("Teams")]
    public class Team
    {
        [Key]
        public Guid TeamId { get; set; }

        public string Name { get; set; }

        public Guid RaceId { get; set; }

        public string ChipId { get; set; }

        public virtual ICollection<Lap> Laps { get; set; }
    }
}
