using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Striker.RelayRace.MongoDB
{
    public class Race
    {
        public ObjectId Id { get; set; } // mongodb internal requirement

        public string RaceId { get; set; }

        public string Name { get; set; }

        public int LapDistanceInMeters { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public List<Guid> TeamIds { get; set; }

        public List<string> ChipIds { get; set; }
    }
}
