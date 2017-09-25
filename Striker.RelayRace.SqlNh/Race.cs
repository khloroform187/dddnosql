using System;
using System.Collections.Generic;

namespace Striker.RelayRace.SqlNh
{
    public class Race
    {
        public string RaceId { get; set; }

        public string Name { get; set; }

        public int LapDistanceInMeters { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public List<Guid> TeamIds { get; set; }

        public List<string> ChipIds { get; set; }
    }
}
