using System;
using System.Management.Automation;
using Newtonsoft.Json;
using Striker.RelayRace.Cmdlets.IoC;
using Striker.RelayRace.Domain.Repositories;
using Striker.RelayRace.MongoDB;
using StructureMap;

namespace Striker.RelayRace.Cmdlets
{
    [Cmdlet("Get", "Team")]
    public class GetTeam : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Id { get; set; }

        protected override void ProcessRecord()
        {
            const string ConnectionString = Connection.ConnectionString;
            const string DatabaseaName = "relayrace";

            var container = new Container(x =>
                x.IncludeRegistry(new CmdletRegistry(ConnectionString, DatabaseaName)));

            var teamRepository = container.GetInstance<ITeamRepository>();

            var result = teamRepository.Get(new Guid(Id));

            Console.WriteLine(JsonConvert.SerializeObject(result));
        }
    }
}
