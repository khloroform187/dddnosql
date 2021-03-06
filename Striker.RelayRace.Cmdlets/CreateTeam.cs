﻿using System;
using System.Management.Automation;
using Striker.RelayRace.Cmdlets.IoC;
using Striker.RelayRace.MongoDB;
using Striker.RelayRace.Service;
using StructureMap;

namespace Striker.RelayRace.Cmdlets
{
    [Cmdlet("Add", "Team")]
    public class AddTeam : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Name { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public string RaceId { get; set; }

        [Parameter(Position = 2, Mandatory = true)]
        public string ChipId { get; set; }

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

            raceManager.AddTeam(this.Name, new Guid(this.RaceId), ChipId);
        }
    }
}
