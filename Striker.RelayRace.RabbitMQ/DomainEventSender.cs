using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using Striker.RelayRace.Domain.DomainEvents;

namespace Striker.RelayRace.RabbitMQ
{
    public class DomainEventSender
    {
        public void Send(DomainEvent domainEvent)
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
            var json = JsonConvert.SerializeObject(domainEvent);

            var bytes = Encoding.Unicode.GetBytes(json);

            var props = model.CreateBasicProperties();
            props.ContentType = "text/plain";
            props.DeliveryMode = 2;
            props.Headers = new Dictionary<string, object>();
            props.Headers.Add("type", Encoding.Unicode.GetBytes(domainEvent.GetType().FullName));

            model.BasicPublish(exchangeName, routingKey, props, bytes);
        }
    }
}
