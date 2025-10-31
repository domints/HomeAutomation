using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.MqttMessageHandlers
{
    public interface IMqttMessageHandler
    {
        public Task Handle(string topic, string message);
    }
}