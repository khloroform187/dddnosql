using MongoDB.Driver;
using Striker.RelayRace.Domain.Repositories;
using Striker.RelayRace.MongoDB;
using Striker.RelayRace.SqlNh;
using StructureMap.Configuration.DSL;

namespace Striker.RelayRace.Cmdlets.IoC
{
    public class CmdletRegistry : Registry
    {
        public CmdletRegistry(
            string mongodbConnectionString,
            string sqlConnectionString,
            string databaseName, bool mongo)
        {
            if (mongo)
            {
                this.For<IMongoClient>()
                    .Use(x => new MongoClient(mongodbConnectionString));

                this.For<IRaceRepository>()
                    .Use<MongoDB.RaceRepository>()
                    .Ctor<string>("databaseName").Is(databaseName);

                this.For<ITeamRepository>()
                    .Use<MongoDB.TeamRepository>()
                    .Ctor<string>("databaseName").Is(databaseName);

                this.For<IActiveTeamRepository>()
                    .Use<MongoDB.ActiveTeamRepository>()
                    .Ctor<string>("databaseName").Is(databaseName);

                this.For<ILapStatisticRepository>()
                    .Use<MongoDB.LapStatisticRepository>()
                    .Ctor<string>("databaseName").Is(databaseName);
            }
            else
            {
               this.For<RelayRaceDbContext>()
                    .HybridHttpOrThreadLocalScoped()
                    .Use<RelayRaceDbContext>()
                    .Ctor<string>()
                    .Is(sqlConnectionString);

                this.For<IRaceRepository>()
                    .HybridHttpOrThreadLocalScoped()
                    .Use<SqlNh.RaceRepository>();

                this.For<ITeamRepository>()
                    .HybridHttpOrThreadLocalScoped()
                    .Use<SqlNh.TeamRepository>();

                this.For<IActiveTeamRepository>()
                    .HybridHttpOrThreadLocalScoped()
                    .Use<SqlNh.ActiveTeamRepository>();

                this.For<ILapStatisticRepository>()
                    .HybridHttpOrThreadLocalScoped()
                    .Use<SqlNh.LapStatisticRepository>();
            }
           
        }
    }
}
