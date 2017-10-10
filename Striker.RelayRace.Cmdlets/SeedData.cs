using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using MongoDB.Driver;
using Newtonsoft.Json;
using Striker.RelayRace.Cmdlets.IoC;
using Striker.RelayRace.Domain.Repositories;
using Striker.RelayRace.MongoDB;
using Striker.RelayRace.Service;
using Striker.RelayRace.SqlNh;
using StructureMap;
using ActiveTeam = Striker.RelayRace.Domain.ActiveTeam;
using LapStatistic = Striker.RelayRace.Domain.LapStatistic;
using Race = Striker.RelayRace.Domain.Race;
using Team = Striker.RelayRace.Domain.Team;

namespace Striker.RelayRace.Cmdlets
{
    [Cmdlet("Seed", "Data")]
    public class SeedData : PSCmdlet
    {
        [Parameter(Position = 1, Mandatory = true)]
        public int RaceQuantity { get; set; }

        [Parameter(Position = 2, Mandatory = true)]
        public int TeamPerRace { get; set; }

        [Parameter(Position = 3, Mandatory = true)]
        public int CompletedLapPerTeam { get; set; }

        [Parameter(Position = 4, Mandatory = true)]
        public bool UseMongo { get; set; }

        [Parameter(Position = 5, Mandatory = true)]
        public bool CleanupDB { get; set; }

        protected override void ProcessRecord()
        {
            const string DatabaseaName = "relayrace";
            StatsPrinter.IsMongoDb = this.UseMongo;

            var container = new Container(x =>
                x.IncludeRegistry(
                    new CmdletRegistry(
                        Connection.MongoDbConnectionString,
                        Connection.SqlConnectionString,
                        DatabaseaName,
                        this.UseMongo)));

            var raceRepository = container.GetInstance<IRaceRepository>();
            var activeTeamRepository = container.GetInstance<IActiveTeamRepository>();
            var teamRepository = container.GetInstance<ITeamRepository>();
            var lapStatisticRepository = container.GetInstance<ILapStatisticRepository>();

            if (this.CleanupDB)
            {
                raceRepository.Cleanup();
                activeTeamRepository.Cleanup();
                teamRepository.Cleanup();
                lapStatisticRepository.Cleanup();
            }

            var random = new Random();

            var races = new List<Race>();
            var teams = new List<Team>();
            var activeTeams = new List<ActiveTeam>();
            var lapStatistics = new List<LapStatistic>();

            this.WriteVerbose("Begin generating data in memory");
            for (var i = 0; i < this.RaceQuantity; i++)
            {
                var chipIds = new List<string>();

                for (var j = 0; j < this.TeamPerRace; j++)
                {
                    chipIds.Add(Guid.NewGuid().ToString());
                }

                var race = new Race(Guid.NewGuid().ToString(), random.Next(3000, 5000), chipIds);

                var raceTeams = new List<Team>();
                foreach (var chipId in chipIds)
                {
                    var team = new Team(Guid.NewGuid().ToString(), race.Id, chipId);
                    raceTeams.Add(team);
                    race.RegisterTeam(team.Id);
                }

                races.Add(race);
                race.StartRace(DateTime.UtcNow);

                foreach (var raceTeam in raceTeams)
                {
                    raceTeam.StartRace(race.Start.Value);
                    var activeTeam = new ActiveTeam { TeamId = raceTeam.Id, ChipId = raceTeam.ChipId };
                    activeTeams.Add(activeTeam);

                    var lastComplete = race.Start.Value;
                    for (var k = 0; k < this.CompletedLapPerTeam; k++)
                    {
                        var secondsToCompleteLap = random.Next(15, 25) * 60;

                        lastComplete = lastComplete.AddSeconds(secondsToCompleteLap);
                        raceTeam.LapCompleted(lastComplete);

                        var lapStatistic = new LapStatistic(
                            raceTeam.RaceId,
                            raceTeam.Id,
                            raceTeam.Name,
                            race.LapDistanceInMeters,
                            new TimeSpan(0, 0, secondsToCompleteLap),
                            lastComplete);

                        lapStatistics.Add(lapStatistic);
                    }

                    teams.Add(raceTeam);
                }
            }

            this.WriteVerbose("End generating data in memory");

            this.WriteVerbose("Begin race persistence");
            raceRepository.BulkCreate(races);
            this.WriteVerbose("End race persistence");
            this.WriteVerbose("Begin team persistence");
            teamRepository.BulkCreate(teams);
            this.WriteVerbose("End team persistence");
            this.WriteVerbose("Begin activeTeam persistence");
            activeTeamRepository.BulkAdd(activeTeams);
            this.WriteVerbose("End activeTeam persistence");
            this.WriteVerbose("Begin lapStatistic persistence");
            lapStatisticRepository.BulkAddLapStatistic(lapStatistics);
            this.WriteVerbose("End lapStatistic persistence");

            Console.WriteLine($"Random Race Id: '{races.Last().Id}'");

            container.Dispose();
            container.EjectAllInstancesOf<IMongoClient>();
            container.EjectAllInstancesOf<IRaceRepository>();
            container.EjectAllInstancesOf<ITeamRepository>();
            container.EjectAllInstancesOf<IActiveTeamRepository>();
            container.EjectAllInstancesOf<ILapStatisticRepository>();
            container.EjectAllInstancesOf<RelayRaceDbContext>();
        }
    }
}
