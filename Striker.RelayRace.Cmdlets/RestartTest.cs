using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using Newtonsoft.Json;
using Striker.RelayRace.Cmdlets.IoC;
using Striker.RelayRace.Domain.Repositories;
using Striker.RelayRace.MongoDB;
using Striker.RelayRace.Service;
using StructureMap;
using ActiveTeam = Striker.RelayRace.Domain.ActiveTeam;
using LapStatistic = Striker.RelayRace.Domain.LapStatistic;
using Race = Striker.RelayRace.Domain.Race;
using Team = Striker.RelayRace.Domain.Team;

namespace Striker.RelayRace.Cmdlets
{
    [Cmdlet("Restart", "Test")]
    public class RestartTest : PSCmdlet
    {
        [Parameter(Position = 1, Mandatory = true)]
        public int RaceQuantity { get; set; }

        [Parameter(Position = 2, Mandatory = true)]
        public int TeamPerRace { get; set; }

        [Parameter(Position = 3, Mandatory = true)]
        public int CompletedLapPerTeam { get; set; }

        [Parameter(Position = 4, Mandatory = true)]
        public bool UseMongo { get; set; }

        protected override void ProcessRecord()
        {
            const string DatabaseaName = "relayrace";

            var container = new Container(x =>
                x.IncludeRegistry(
                    new CmdletRegistry(
                        Connection.MongoDbConnectionString,
                        Connection.SqlConnectionString,
                        DatabaseaName,
                        this.UseMongo)));

            var raceRepository = container.GetInstance<IRaceRepository>();
            raceRepository.Cleanup();

            var activeTeamRepository = container.GetInstance<IActiveTeamRepository>();
            activeTeamRepository.Cleanup();

            var teamRepository = container.GetInstance<ITeamRepository>();
            teamRepository.Cleanup();

            var lapStatisticRepository = container.GetInstance<ILapStatisticRepository>();
            lapStatisticRepository.Cleanup();

            var statisticsManager = container.GetInstance<StatisticManager>();

            var random = new Random();

            var races = new List<Race>();
            var teams = new List<Team>();
            var activeTeams = new List<ActiveTeam>();
            var lapStatistics = new List<LapStatistic>();

            Console.WriteLine("Begin generating data in memory");
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

            Console.WriteLine("End generating data in memory");

            Console.WriteLine("Begin race persistence");
            raceRepository.BulkCreate(races);
            Console.WriteLine("End race persistence");
            Console.WriteLine("Begin team persistence");
            teamRepository.BulkCreate(teams);
            Console.WriteLine("End team persistence");
            Console.WriteLine("Begin activeTeam persistence");
            activeTeamRepository.BulkAdd(activeTeams);
            Console.WriteLine("End activeTeam persistence");
            Console.WriteLine("Begin lapStatistic persistence");
            lapStatisticRepository.BulkAddLapStatistic(lapStatistics);
            Console.WriteLine("End lapStatistic persistence");

            var raceId = races.First().Id;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            statisticsManager.GetRaceStatistics(raceId);
            stopWatch.Stop();
            Console.WriteLine($"statisticsManager.GetRaceStatistics(raceId) --> {stopWatch.ElapsedMilliseconds}");

        }
    }
}