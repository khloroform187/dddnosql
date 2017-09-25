using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Striker.RelayRace.Domain.Repositories;

namespace Striker.RelayRace.MongoDB
{
    public class TeamRepository : ITeamRepository
    {
        private const string CollectionName = "teams";

        private readonly IMongoDatabase _db;

        public TeamRepository(IMongoClient mongoClient, string databaseName)
        {
            var database = mongoClient.GetDatabase(databaseName);

            _db = database;
        }

        public void BulkCreate(List<Domain.Team> teams)
        {
            var mongTeams = teams.Select(Convert).ToList();

            var collection = this._db.GetCollection<Team>(CollectionName);

            collection.InsertMany(mongTeams);
        }

        public Domain.Team Get(Guid id)
        {
            var collection = _db.GetCollection<Team>(CollectionName);
            _db.GetCollection<Race>(CollectionName);

            var team = collection.Find(x => x.TeamId == id.ToString()).ToCursorAsync().Result.Single();

            var result = Convert(team);

            return result;
        }

        public void Create(Domain.Team team)
        {
            var mongoTeam = Convert(team);

            var collection = _db.GetCollection<Team>(CollectionName);
            collection.InsertOne(mongoTeam);
        }

        public void Update(Domain.Team team)
        {
            var collection = _db.GetCollection<Team>(CollectionName);

            var existingRace = collection.Find(x => x.TeamId == team.Id.ToString()).ToCursorAsync().Result.Single();

            var replacement = Convert(team);
            replacement.Id = existingRace.Id;

            collection.ReplaceOne(x => x.TeamId == team.Id.ToString(), replacement);
        }

        public void Cleanup()
        {
            var collection = _db.GetCollection<Team>(CollectionName);
            collection.DeleteMany(new BsonDocument());
        }

        private static Team Convert(Domain.Team team)
        {
            var result = new Team
            {
                RaceId = team.RaceId.ToString(),
                TeamId = team.Id.ToString(),
                ChipId = team.ChipId,
                Laps = team.Laps.Select(Convert).ToList(),
                Name = team.Name
            };

            return result;
        }

        private static Lap Convert(Domain.Lap lap)
        {
            var result = new Lap
            {
                LapId = lap.Id.ToString(),
                End = lap.End,
                Start = lap.Start
            };

            return result;
        }

        private static Domain.Team Convert(Team team)
        {
            var result = new Domain.Team(team.Name, new Guid(team.RaceId), team.ChipId);

            result.SetPropertyValue(
                "Id",
                new Guid(team.TeamId));
            result.SetPropertyValue(
                "Laps",
                new ReadOnlyCollection<Domain.Lap>(team.Laps.Select(Convert).ToList()));

            return result;
        }

        private static Domain.Lap Convert(Lap lap)
        {
            var result = new Domain.Lap(lap.Start);

            result.SetPropertyValue("Id", new Guid(lap.LapId));
            result.SetPropertyValue("End", lap.End);

            return result;
        }
    }
}