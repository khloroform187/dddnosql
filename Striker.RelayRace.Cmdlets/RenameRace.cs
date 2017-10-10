using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Striker.RelayRace.Cmdlets.IoC;
using Striker.RelayRace.Domain.Repositories;
using Striker.RelayRace.MongoDB;
using StructureMap;

namespace Striker.RelayRace.Cmdlets
{
    [Cmdlet("Rename", "Race")]
    public class RenameRace : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Id { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public string Name { get; set; }

        [Parameter(Position = 2, Mandatory = true)]
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

            var race = raceRepository.Get(new Guid(Id));

            race.Rename(this.Name);

            raceRepository.Update(race);
        }
    }
}
