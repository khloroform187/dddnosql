using System.Management.Automation;
using Striker.RelayRace.Cmdlets.IoC;
using Striker.RelayRace.Domain.Repositories;
using Striker.RelayRace.MongoDB;
using StructureMap;

namespace Striker.RelayRace.Cmdlets
{
    [Cmdlet("Cleanup", "Database")]
    public class CleanupDatabase : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            const string ConnectionString = Connection.ConnectionString;
            const string DatabaseaName = "relayrace";

            var container = new Container(x =>
                x.IncludeRegistry(new CmdletRegistry(ConnectionString, DatabaseaName)));

            var raceRepository = container.GetInstance<IRaceRepository>();
            raceRepository.Cleanup();

            var activeTeamRepository = container.GetInstance<IActiveTeamRepository>();
            activeTeamRepository.Cleanup();

            var teamRepository = container.GetInstance<ITeamRepository>();
            teamRepository.Cleanup();

            var lapStatisticRepository = container.GetInstance<ILapStatisticRepository>();
            lapStatisticRepository.Cleanup();
        }
    }
}
