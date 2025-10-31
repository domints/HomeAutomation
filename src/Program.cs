using MQTTnet;
using src;
using src.Config;

var builder = WebApplication.CreateBuilder(args);
//builder.Configuration.AddJsonFile("secrets.json", true);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IHomeStateProvider, HomeStateProvider>();
builder.Services.AddSingleton<IMQTTHandler, MQTTHandler>();

var mqttFactory = new MqttClientFactory();
var mqttConfig = builder.Configuration.GetRequiredSection("MQTT").Get<MQTTConfig>() ?? throw new Exception("MQTT config missing");
using var mqttClient = mqttFactory.CreateMqttClient();
builder.Services.AddSingleton(mqttClient);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(mqttConfig.IP).WithCredentials(mqttConfig.User, mqttConfig.Pass).Build();
mqttClient.DisconnectedAsync += async e =>
{
    if (e.ClientWasConnected)
    {
        // Use the current options as the new options.
        await mqttClient.ConnectAsync(mqttClientOptions);
    }
};

mqttClient.ConnectedAsync += async e =>
{
    var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder().WithTopicFilter("zigbee2mqtt/#").Build();
    await mqttClient.SubscribeAsync(mqttSubscribeOptions);
};

var mqttHandler = app.Services.GetRequiredService<IMQTTHandler>();

mqttClient.ApplicationMessageReceivedAsync += mqttHandler.Handle;
await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

app.Run();

if (mqttClient.IsConnected)
{
    var mqttClientDisconnectOptions = mqttFactory.CreateClientDisconnectOptionsBuilder().Build();
    await mqttClient.DisconnectAsync(mqttClientDisconnectOptions, CancellationToken.None);
}
