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
    [Cmdlet("Bench", "RaceStatistics")]
    public class BenchGetRaceStatistics : PSCmdlet
    {
        [Parameter(Position = 1, Mandatory = true)]
        public Guid RaceId{ get; set; }

        [Parameter(Position = 2, Mandatory = true)]
        public bool UseMongo { get; set; }

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

            var statisticsManager = container.GetInstance<StatisticManager>();

                var stopWatch = new Stopwatch();
                stopWatch.Start();
                statisticsManager.GetRaceStatistics(this.RaceId);
                stopWatch.Stop();
                StatsPrinter.Print("GetRaceStatistics", stopWatch.ElapsedMilliseconds);

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