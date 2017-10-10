using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Striker.RelayRace.Cmdlets.IoC;
using Striker.RelayRace.Domain.Repositories;
using Striker.RelayRace.MongoDB;
using Striker.RelayRace.Service;
using StructureMap;

namespace Striker.RelayRace.Cmdlets
{
    [Cmdlet("Get", "RaceStatistics")]
    public class GetRaceStatistics : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string RaceId { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
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

            var statisticManager = container.GetInstance<StatisticManager>();

            var id = new Guid(RaceId);
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = statisticManager.GetRaceStatistics(id);
            stopWatch.Stop();
            Console.WriteLine($"statisticManager.GetRaceStatistics(id) --> {stopWatch.ElapsedMilliseconds}");

            Console.WriteLine(JsonConvert.SerializeObject(result));
        }
    }
}
