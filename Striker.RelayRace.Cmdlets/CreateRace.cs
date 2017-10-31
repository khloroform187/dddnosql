using System;
using System.Collections.Generic;
using System.Management.Automation;
using Striker.RelayRace.Cmdlets.IoC;
using Striker.RelayRace.MongoDB;
using Striker.RelayRace.Service;
using StructureMap;

namespace Striker.RelayRace.Cmdlets
{
    [Cmdlet("Create", "Race")]
    public class CreateRace : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Name { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public int LapDistanceInMeters { get; set; }

        [Parameter(Position = 2, Mandatory = true)]
        public List<string> ChipIds { get; set; }

        [Parameter(Position = 3, Mandatory = true)]
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

            var raceManager = container.GetInstance<RaceManager>();

            var race = raceManager.CreateRace(this.Name,  this.LapDistanceInMeters, this.ChipIds);

            Console.WriteLine($"Race Id: {race.Id}");
        }
    }
}
