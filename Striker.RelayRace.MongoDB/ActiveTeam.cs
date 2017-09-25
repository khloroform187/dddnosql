using MongoDB.Bson;

namespace Striker.RelayRace.MongoDB
{
    public class ActiveTeam
    {
        public ObjectId Id { get; set; } // mongodb internal requirement

        public string TeamId { get; set; }

        public string ChipId { get; set; }
    }
}