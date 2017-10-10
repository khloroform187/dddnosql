using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Striker.RelayRace.SqlNh
{
    [Table("ActiveTeams")]
    public class ActiveTeam
    {
        [Key]
        [Column(Order = 1)]
        public Guid TeamId { get; set; }

        [Key]
        [Column(Order = 2)]
        public string ChipId { get; set; }
    }
}