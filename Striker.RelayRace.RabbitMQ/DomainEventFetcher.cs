using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Striker.RelayRace.Domain.DomainEvents;

namespace Striker.RelayRace.RabbitMQ
{
    public class DomainEventFetcher
    {
        public DomainEvent Fetch()
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@mq001.core.xtkl:5672/core.nbedard");

            var conn = factory.CreateConnection();

            var model = conn.CreateModel();

            var exchangeName = "xtkl.core";
            var queueName = "striker-relayrace";
            var routingKey = "key";

            model.ExchangeDeclare(exchangeName, ExchangeType.Topic, true);
            model.QueueDeclare(queueName, false, false, false, null);
            model.QueueBind(queueName, exchangeName, routingKey, null);

            var message = model.BasicGet(queueName, true);

            if (message == null)
            {
                return null;
            }

            var type = Encoding.Unicode.GetString((byte[])message.BasicProperties.Headers.Single().Value);
            var body = Encoding.Unicode.GetString(message.Body);

            if (type == typeof(RaceStarted).FullName)
            {
                var result = JsonConvert.DeserializeObject<RaceStarted>(body);

                return result;
            }

            if (type == typeof(LapCompleted).FullName)
            {
                var result = JsonConvert.DeserializeObject<LapCompleted>(body);

                return result;
            }

            if (type == typeof(TeamCreated).FullName)
            {
                var result = JsonConvert.DeserializeObject<TeamCreated>(body);

                return result;
            }

            if (type == typeof(TeamRaceStarted).FullName)
            {
                var result = JsonConvert.DeserializeObject<TeamRaceStarted>(body);

                return result;
            }

            if (type == typeof(TeamRaceFinished).FullName)
            {
                var result = JsonConvert.DeserializeObject<TeamRaceFinished>(body);

                return result;
            }

            if (type == typeof(RaceFinished).FullName)
            {
                var result = JsonConvert.DeserializeObject<RaceFinished>(body);

                return result;
            }



            throw new NotSupportedException();
        }
    }
}
