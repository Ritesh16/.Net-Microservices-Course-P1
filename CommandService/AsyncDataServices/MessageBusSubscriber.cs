using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IServiceProvider services;
        private IConnection connection;
        private IModel channel;
        private string queueName;

        public MessageBusSubscriber(IServiceProvider services)
        {
            this.services = services;
            InitializeRabbitMQ();
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("--> Event Received.");
                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                using (var scope = services.CreateScope())
                {
                    var eventProcessor =
                        scope.ServiceProvider
                            .GetRequiredService<IEventProcessor>();

                    eventProcessor.ProcessEvent(notificationMessage);
                }

            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private void InitializeRabbitMQ()
        {
            using (var scope = services.CreateScope())
            {
                var configuration =
                    scope.ServiceProvider
                        .GetRequiredService<IConfiguration>();


                var factory = new ConnectionFactory() { HostName = configuration["RabbitMQHost"], Port = int.Parse(configuration["RabbitMQPort"]) };
                connection = factory.CreateConnection();
                channel = connection.CreateModel();
                channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queueName,
                        exchange: "trigger",
                        routingKey: "");

                Console.WriteLine("--> Listening on Message Bus.");

                connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            }
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("Shutting down connection.");
        }

        public override void Dispose()
        {
            if (connection.IsOpen)
            {
                channel.Close();
                connection.Close();
            }
        }
    }
}