﻿using System;
using System.Management.Automation;
using Newtonsoft.Json;
using Striker.RelayRace.Cmdlets.IoC;
using Striker.RelayRace.Domain.Repositories;
using Striker.RelayRace.MongoDB;
using StructureMap;

namespace Striker.RelayRace.Cmdlets
{
    [Cmdlet("Get", "Race")]
    public class GetRace : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Id { get; set; }

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

            var raceRepository = container.GetInstance<IRaceRepository>();

            var result = raceRepository.Get(new Guid(Id));

            Console.WriteLine(JsonConvert.SerializeObject(result));
        }
    }
}
