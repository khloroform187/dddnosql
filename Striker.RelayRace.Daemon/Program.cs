using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Striker.RelayRace.Cmdlets.IoC;
using Striker.RelayRace.Domain.DomainEvents;
using Striker.RelayRace.MongoDB;
using Striker.RelayRace.RabbitMQ;
using Striker.RelayRace.Service.EventHandlers;
using StructureMap;

namespace Striker.RelayRace.Daemon
{
    class Program
    {
        static void Main(string[] args)
        {
            const string ConnectionString = Connection.ConnectionString;
            const string DatabaseaName = "relayrace";

            var container = new Container(x =>
                x.IncludeRegistry(new CmdletRegistry(ConnectionString, DatabaseaName)));

            var eventDispatcher = container.GetInstance<EventDispatcher>();

            while (true)
            {
                var fetcher = new DomainEventFetcher();
                var result = fetcher.Fetch();

                if (result != null)
                {
                    try
                    {
                        eventDispatcher.DispatchEvents(new List<DomainEvent> { result });
                    }
                    catch (Exception)
                    {
                        var sender = new DomainEventSender();
                        sender.Send(result);
                    }
                }

                Thread.Sleep(200);
            }
            
        }
    }
}
