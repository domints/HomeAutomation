using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using src.Models.MQTT;

namespace src.MqttMessageHandlers
{
    [TopicHandler("zigbee2mqtt/door/main")]
    public class DoorHandler : IMqttMessageHandler
    {
        private readonly IHomeStateProvider _homeStateProvider;
        private readonly IBot _bot;

        public DoorHandler(IHomeStateProvider homeStateProvider, IBot bot)
        {
            _homeStateProvider = homeStateProvider;
            _bot = bot;
        }

        public async Task Handle(string topic, string message)
        {
            var state = JsonConvert.DeserializeObject<TS0203DoorSensor>(message);
            if (state == null)
                return;

            if (_homeStateProvider.DoorOpened && state.Contact)
            {
                var longPressed = _homeStateProvider.CorridorLongPressed.UpdatedIn(TimeSpan.FromMinutes(1));
                if (longPressed)
                {
                    await _bot.SendMessage("Door should be probably locking now?");
                }
            }

            _homeStateProvider.DoorOpened = !state.Contact;
        }
    }
}