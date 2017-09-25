using System;

namespace Striker.RelayRace.SqlNh
{
    public class Lap
    {
        public string LapId { get; set; }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }
    }
}
