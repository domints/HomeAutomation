using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace src.Models.MQTT
{
    public class TS0203DoorSensor
    {
        [JsonProperty("battery")]
        public int Battery { get; set; }

        [JsonProperty("battery_low")]
        public bool BatteryLow { get; set; }

        [JsonProperty("contact")]
        public bool Contact { get; set; }

        [JsonProperty("last_seen")]
        public DateTime LastSeen { get; set; }

        [JsonProperty("linkquality")]
        public int Linkquality { get; set; }

        [JsonProperty("tamper")]
        public bool Tamper { get; set; }

        [JsonProperty("voltage")]
        public int Voltage { get; set; }
    }
}