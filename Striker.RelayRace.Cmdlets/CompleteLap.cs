﻿using System;
using System.Management.Automation;
using Striker.RelayRace.Cmdlets.IoC;
using Striker.RelayRace.MongoDB;
using Striker.RelayRace.Service;
using StructureMap;

namespace Striker.RelayRace.Cmdlets
{
    [Cmdlet("Complete", "Lap")]
    public class CompleteLap : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string ChipId { get; set; }

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

            var raceManager = container.GetInstance<RaceManager>();

            raceManager.LapCompleted(this.ChipId, DateTime.UtcNow);
        }
    }
}
