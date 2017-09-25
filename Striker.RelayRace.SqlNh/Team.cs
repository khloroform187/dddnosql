using System.Collections.Generic;

namespace Striker.RelayRace.SqlNh
{
    public class Team
    {
        public string TeamId { get; set; }

        public string Name { get; set; }

        public string RaceId { get; set; }

        public string ChipId { get; set; }

        public List<Lap> Laps { get; set; }
    }
}
