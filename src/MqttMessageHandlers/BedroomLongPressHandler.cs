using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MQTTnet;
using Newtonsoft.Json;
using src.Models.MQTT;

namespace src.MqttMessageHandlers
{
    [TopicHandler("zigbee2mqtt/light/bedroom")]
    public class BedroomLongPressHandler : IMqttMessageHandler
    {
        private readonly IHomeStateProvider _homeStateProvider;
        private readonly IMqttClient _mqttClient;

        public BedroomLongPressHandler(IHomeStateProvider homeStateProvider, IMqttClient mqttClient)
        {
            _homeStateProvider = homeStateProvider;
            _mqttClient = mqttClient;
        }

        public async Task Handle(string topic, string message)
        {
            var switchState = JsonConvert.DeserializeObject<OXTSwitch>(message);
            if (switchState == null)
                return;

            if (_homeStateProvider.CorridorLastPressAction != Models.MQTT.Enums.OXTPressAction.LongPress && switchState.SwitchPressActionSwitch == Models.MQTT.Enums.OXTPressAction.LongPress)
            {
                _homeStateProvider.CorridorLastPressAction = switchState.SwitchPressActionSwitch;

                var allLights = _homeStateProvider.LightRelays;
                foreach (var light in allLights)
                {
                    if (light.StartsWith("light/bedroom"))
                        continue;
                        
                    await _mqttClient.PublishStringAsync($"zigbee2mqtt/{light}/set", "OFF");
                }
            }

            _homeStateProvider.CorridorLastPressAction = switchState.SwitchPressActionSwitch;
        }
    }
}