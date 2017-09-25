using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Striker.RelayRace.Domain;
using Striker.RelayRace.Domain.Repositories;

namespace Striker.RelayRace.MongoDB
{
    public class LapStatisticRepository : ILapStatisticRepository
    {
        private const string CollectionName = "lapStatistics";

        private readonly IMongoDatabase _db;

        public LapStatisticRepository(IMongoClient mongoClient, string databaseName)
        {
            var database = mongoClient.GetDatabase(databaseName);

            this._db = database;
        }

        public void Cleanup()
        {
            var collection = this._db.GetCollection<ActiveTeam>(CollectionName);
            collection.DeleteMany(new BsonDocument());
        }

        public void AddLapStatistic(Domain.LapStatistic lapStatistic)
        {
            var mongoActiveTeam = Convert(lapStatistic);

            var collection = this._db.GetCollection<LapStatistic>(CollectionName);

            collection.InsertOne(mongoActiveTeam);
        }

        public void BulkAddLapStatistic(List<Domain.LapStatistic> lapStatistics)
        {
            var mongoLapStatistics = lapStatistics.Select(Convert).ToList();

            var collection = this._db.GetCollection<LapStatistic>(CollectionName);

            collection.InsertMany(mongoLapStatistics);
        }

        public List<Domain.LapStatistic> Find(Guid raceId)
        {
            var collection = this._db.GetCollection<LapStatistic>(CollectionName);
            var lapStatistics = collection.Find(x => x.RaceId == raceId.ToString()).ToCursorAsync().Result.ToList();

            var result = lapStatistics.Select(Convert).ToList();

            return result;
        }

        private static LapStatistic Convert(Domain.LapStatistic lapStatistic)
        {
            var result = new LapStatistic
            {
               CompletedOn = lapStatistic.CompletedOn,
               DistanceInMeters = lapStatistic.DistanceInMeters,
               Length = lapStatistic.Length,
               Pace = lapStatistic.Pace,
               RaceId = lapStatistic.RaceId.ToString(),
               TeamId = lapStatistic.TeamId.ToString(),
               TeamName = lapStatistic.TeamName
            };

            return result;
        }

        private static Domain.LapStatistic Convert(LapStatistic lapStatistic)
        {
            var result = new Domain.LapStatistic(
                new Guid(lapStatistic.RaceId),
                new Guid(lapStatistic.TeamId),
                lapStatistic.TeamName,
                lapStatistic.DistanceInMeters,
                lapStatistic.Length,
                lapStatistic.CompletedOn);

            return result;
        }
    }
}
