using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandService.EventProcessing;
using RabbitMQ.Client;

namespace CommandService.AsyncDataServices
{
    // public class MessageBusSubscriber : BackgroundService
    // {
    //     private readonly IConfiguration configuration;
    //     private readonly IEventProcessor eventProcessor;
    //     private IConnection connection;
    //     private IModel channel;
    //     private string queueName;

    //     public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
    //     {
    //         configuration = configuration;
    //         eventProcessor = eventProcessor;
    //     }
    //     protected override Task ExecuteAsync(CancellationToken stoppingToken)
    //     {

    //     }

    //     private void InitializeRabbitMQ()
    //     {
    //         var factory = new ConnectionFactory() { HostName = configuration["RabbitMQHost"], Port = int.Parse(configuration["RabbitMQPort"]) };
    //         connection = factory.CreateConnection();
    //         channel = connection.CreateModel();
    //         channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
    //         queueName = channel.QueueDeclare().QueueName;
            
    //         channel.QueueBind(queue: queueName,
    //                 exchange: "trigger",
    //                 routingKey: "");
    //     }
    //}
}