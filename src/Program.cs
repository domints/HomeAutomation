using MQTTnet;
using src;
using src.Config;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Telegram.Bot;
using Microsoft.Extensions.Options;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//builder.Configuration.AddJsonFile("secrets.json", true);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.Configure<TelegramConfig>(builder.Configuration.GetSection("Telegram"));
builder.Services.AddSingleton<IDatabase, SqliteDatabase>();
builder.Services.AddSingleton<IHomeStateProvider, HomeStateProvider>();
builder.Services.AddSingleton<IMQTTHandler, MQTTHandler>();
builder.Services.AddSingleton((IServiceProvider sp) =>
{
    var cfg = sp.GetRequiredService<IOptions<TelegramConfig>>();
    return new TelegramBotClient(cfg.Value.Token!);
});
builder.Services.AddScoped<IBot, BotWrapper>();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

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
    var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder().WithTopicFilter("#").Build();
    await mqttClient.SubscribeAsync(mqttSubscribeOptions);
};

var mqttHandler = app.Services.GetRequiredService<IMQTTHandler>();

mqttClient.ApplicationMessageReceivedAsync += mqttHandler.Handle;
await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

app.MapGet("/", (HttpContext cx, IMqttClient client, IHomeStateProvider homeState) =>
{
    return new
    {
        Connected = client.IsConnected,
        Lights = homeState.LightRelays,
        CorridorLastAction = homeState.CorridorLastPressAction,
        BedroomLastAction = homeState.BedroomLastPressAction,
        DoorOpened = homeState.DoorOpened
    };
});

app.MapGet("/calls", async (IDatabase db) =>
{
    var rows = await db.WorkAsync(async (conn) =>
    {
        var results = new List<(int number, long timestamp, string state, int duration)>();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT number, timestamp, state, duration FROM calls;";
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            int n = reader.GetInt16(0);
            long t = reader.GetInt64(1);
            string s = reader.GetString(2);
            int d = reader.GetInt32(3);
            results.Add((n, t, s, d));
        }

        return results;
    });

    var toDate = (long timestamp) =>
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();
        return dateTime;
    };

    var rowBuilder = new StringBuilder();
    foreach (var row in rows)
    {
        rowBuilder.Append("<tr><td>");
        rowBuilder.Append(row.number);
        rowBuilder.Append("</td><td>");
        rowBuilder.Append(toDate(row.timestamp).ToString("yyyy-MM-dd HH:mm:ss"));
        rowBuilder.Append("</td><td>");
        rowBuilder.Append(row.state);
        rowBuilder.Append("</td><td>");
        rowBuilder.Append(row.state == "code" ? null : row.duration);
        rowBuilder.Append("</td></tr>");
    }

    return Results.Extensions.Html(@$"
    <html>
    <head>
    <link href=""https://cdn.jsdelivr.net/npm/bootstrap@6.0.0-alpha.1/dist/css/bootstrap.min.css"" rel=""stylesheet"" integrity=""sha384-B/GM4XqrwHnWXNOWMbloTmrYXZg10cakYGmpfsR/bbzQ6JAJI4ihuyADKLnBgrCe"" crossorigin=""anonymous"">
    </head>
    <body>
    <div class=""container"">
    <table class=""table"">
    {rowBuilder}
    </table>
    </div>
    </body>
    </html>");
});

app.Run();

if (mqttClient.IsConnected)
{
    var mqttClientDisconnectOptions = mqttFactory.CreateClientDisconnectOptionsBuilder().Build();
    await mqttClient.DisconnectAsync(mqttClientDisconnectOptions, CancellationToken.None);
}
