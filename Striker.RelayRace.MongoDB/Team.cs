using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Striker.RelayRace.MongoDB
{
    public class Team
    {
        public ObjectId Id { get; set; } // mongodb internal requirement

        public string TeamId { get; set; }

        public string Name { get; set; }

        public string RaceId { get; set; }

        public string ChipId { get; set; }

        public List<Lap> Laps { get; set; }
    }
}
