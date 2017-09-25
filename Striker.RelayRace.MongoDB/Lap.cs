using System;
using MongoDB.Bson;

namespace Striker.RelayRace.MongoDB
{
    public class Lap
    {
        public ObjectId Id { get; set; } // mongodb internal requirement

        public string LapId { get; set; }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }
    }
}
