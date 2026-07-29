using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(
    queue: "messageVolki",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null

    );

Console.WriteLine("Waiting for messages...");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += Consumer_ReceivedAsync;

async Task Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs @event)
{
    byte[] body = @event.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);

    
    Console.WriteLine($"Received : {message}");

    await ((AsyncEventingBasicConsumer)sender).Channel.BasicAckAsync(@event.DeliveryTag, multiple: false);


}

await channel.BasicConsumeAsync("messageVolki", autoAck: false, consumer);

Console.ReadLine();

