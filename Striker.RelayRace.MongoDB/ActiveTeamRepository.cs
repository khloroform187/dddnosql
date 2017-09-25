using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Striker.RelayRace.Domain.Repositories;

namespace Striker.RelayRace.MongoDB
{
    public class ActiveTeamRepository : IActiveTeamRepository
    {
        private const string CollectionName = "activeTeams";

        private readonly IMongoDatabase _db;

        public ActiveTeamRepository(IMongoClient mongoClient, string databaseName)
        {
            var database = mongoClient.GetDatabase(databaseName);

            _db = database;
        }

        public void Cleanup()
        {
            var collection = this._db.GetCollection<ActiveTeam>(CollectionName);
            collection.DeleteMany(new BsonDocument());
        }

        public void Add(Domain.ActiveTeam activeTeam)
        {
            var mongoActiveTeam = Convert(activeTeam);

            var collection = _db.GetCollection<ActiveTeam>(CollectionName);

            collection.InsertOne(mongoActiveTeam);
        }

        public void BulkAdd(List<Domain.ActiveTeam> activeTeams)
        {
            var mongoActiveTeams = activeTeams.Select(Convert).ToList();

            var collection = this._db.GetCollection<ActiveTeam>(CollectionName);

            collection.InsertMany(mongoActiveTeams);
        }

        public Domain.ActiveTeam Find(string chipId)
        {
            var collection = _db.GetCollection<ActiveTeam>(CollectionName);
            var activeTeam = collection.Find(x => x.ChipId == chipId).ToCursorAsync().Result.ToList();

            var result = Convert(activeTeam.Single());

            return result;
        }

        public Domain.ActiveTeam Find(Guid teamId)
        {
            var collection = _db.GetCollection<ActiveTeam>(CollectionName);
            var activeTeam = collection.Find(x => x.TeamId == teamId.ToString()).ToCursorAsync().Result.ToList();

            var result = Convert(activeTeam.Single());

            return result;
        }

        public void Remove(Domain.ActiveTeam activeTeam)
        {
            var collection = _db.GetCollection<ActiveTeam>(CollectionName);

            collection.DeleteOne(x => x.TeamId == activeTeam.TeamId.ToString());
        }

        private static ActiveTeam Convert(Domain.ActiveTeam activeTeam)
        {
            var result = new ActiveTeam
            {
                TeamId = activeTeam.TeamId.ToString(),
                ChipId = activeTeam.ChipId
            };

            return result;
        }

        private static Domain.ActiveTeam Convert(ActiveTeam activeTeam)
        {
            var result = new Domain.ActiveTeam
            {
                TeamId = new Guid(activeTeam.TeamId),
                ChipId = activeTeam.ChipId
            };

            return result;
        }
    }
}