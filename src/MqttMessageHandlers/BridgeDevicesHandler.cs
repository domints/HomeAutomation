using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using src.Models.MQTT;

namespace src.MqttMessageHandlers
{
    [TopicHandler("zigbee2mqtt/bridge/devices")]
    public class BridgeDevicesHandler : IMqttMessageHandler
    {
        private readonly IHomeStateProvider _homeStateProvider;

        public BridgeDevicesHandler(IHomeStateProvider homeStateProvider)
        {
            _homeStateProvider = homeStateProvider;
        }

        public Task Handle(string topic, string message)
        {
            var devices = JsonConvert.DeserializeObject<List<Z2MDevice>>(message);
            if (devices == null)
                return Task.CompletedTask;
            var lightEndpoints = new List<string>();
            foreach (var dev in devices)
            {
                if (dev.FriendlyName != null && dev.FriendlyName.StartsWith("light/"))
                {
                    foreach (var endpoint in dev.Endpoints.Values)
                    {
                        if (endpoint.Name != null && endpoint.Name.StartsWith("relay"))
                        {
                            lightEndpoints.Add($"{dev.FriendlyName}/{endpoint.Name}");
                        }
                    }
                }
            }

            _homeStateProvider.LightRelays = lightEndpoints;

            return Task.CompletedTask;
        }
    }
}