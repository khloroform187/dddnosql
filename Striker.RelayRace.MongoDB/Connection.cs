using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Striker.RelayRace.MongoDB
{
    public static class Connection
    {
        //local
        public const string MongoDbConnectionString = "mongodb://127.0.0.1:27017";

        public const string SqlConnectionString = "Data Source=she-dev-1295a;Initial Catalog=RelayRace;Integrated Security=True";

        //remote
        //public const string MongoDbConnectionString = "mongodb://normandbedard:Xtkl45!!@ds133094.mlab.com:33094/relayrace";
    }
}
