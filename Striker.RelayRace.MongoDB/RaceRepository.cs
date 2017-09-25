using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Striker.RelayRace.Domain.Repositories;

namespace Striker.RelayRace.MongoDB
{
    public class RaceRepository : IRaceRepository
    {
        private const string CollectionName = "races";

        private readonly IMongoDatabase _db;

        public RaceRepository(IMongoClient mongoClient, string databaseName)
        {
            var database = mongoClient.GetDatabase(databaseName);

            this._db = database;
        }

        public Domain.Race Get(Guid id)
        {
            var collection = this._db.GetCollection<Race>(CollectionName);
            this._db.GetCollection<Race>(CollectionName);
            var race = collection.Find(x => x.RaceId == id.ToString()).ToCursorAsync().Result.ToList();

            var result = Convert(race.Single());

            return result;
        }

        public void Create(Domain.Race race)
        {
            var mongoRace = Convert(race);

            var collection = this._db.GetCollection<Race>(CollectionName);
            
            collection.InsertOne(mongoRace);
        }

        public void BulkCreate(List<Domain.Race> races)
        {
            var mongoRaces = races.Select(Convert).ToList();

            var collection = this._db.GetCollection<Race>(CollectionName);

            collection.InsertMany(mongoRaces);
        }

        public void Update(Domain.Race race)
        {
            var collection = this._db.GetCollection<Race>(CollectionName);
            var existingRace = collection.Find(x => x.RaceId == race.Id.ToString()).ToCursorAsync().Result.Single();

            var replacement = Convert(race);
            replacement.Id = existingRace.Id;

            collection.ReplaceOne(x => x.RaceId == race.Id.ToString(), replacement);
        }

        public void Cleanup()
        {
            var collection = this._db.GetCollection<Race>(CollectionName);
            collection.DeleteMany(new BsonDocument());
        }

        private static Race Convert(Domain.Race race)
        {
            var result = new Race
            {
                RaceId = race.Id.ToString(),
                ChipIds = race.ChipIds,
                End = race.End,
                LapDistanceInMeters = race.LapDistanceInMeters,
                Name = race.Name,
                Start = race.Start,
                TeamIds = race.TeamIds.ToList()
            };

            return result;
        }

        private static Domain.Race Convert(Race race)
        {
            var result = new Domain.Race(race.Name, race.LapDistanceInMeters, race.ChipIds);

            result.SetPropertyValue("End", race.End);
            result.SetPropertyValue("Start", race.Start);
            result.SetPropertyValue("Id", new Guid(race.RaceId));
            result.SetPropertyValue("TeamIds", race.TeamIds);

            return result;
        }
    }
}
