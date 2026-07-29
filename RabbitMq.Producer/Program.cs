using RabbitMQ.Client;
using System.Text;
using System.Text.Json.Serialization.Metadata;

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

for (int i = 0; i < 50; i++)
{
    var msg = $"{DateTime.Now} -{Guid.CreateVersion7()}";
    var body = Encoding.UTF8.GetBytes(msg);

    await channel.BasicPublishAsync(

        exchange: string.Empty,
        routingKey: "messageVolki",
        mandatory: true,
        basicProperties: new BasicProperties { Persistent = true },
        body: body
        );

    Console.WriteLine($"Sent: {msg}");
    await Task.Delay(2000);
     
     
}


 