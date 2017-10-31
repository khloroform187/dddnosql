using System;
using System.Collections.Generic;
using System.Threading;
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
            const bool UseMongo = true;
            const string DatabaseaName = "relayrace";

            StatsPrinter.Configure(UseMongo);

            var container = new Container(x =>
                x.IncludeRegistry(
                    new CmdletRegistry(
                        Connection.MongoDbConnectionString,
                        Connection.SqlConnectionString,
                        DatabaseaName,
                        UseMongo)));

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
                    catch (Exception e)
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
