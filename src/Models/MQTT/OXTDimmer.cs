using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using src.Models.MQTT.Enums;

namespace src.Models.MQTT
{
    public class OXTDimmer
    {
        [JsonProperty("last_seen")]
        public DateTime LastSeen { get; set; }

        [JsonProperty("linkquality")]
        public int Linkquality { get; set; }
        [JsonProperty("brightness_l1")]
        public int BrightnessL1 { get; set; }
        [JsonProperty("brightness_l2")]
        public int BrightnessL2 { get; set; }
        [JsonProperty("max_brightness")]
        public int MaxBrightness { get; set; }
        [JsonProperty("min_brightness")]
        public int MinBrightness { get; set; }
        [JsonProperty("state_l1")]
        public OnOff? StateL1 { get; set; }
        [JsonProperty("state_l2")]
        public OnOff? StateL2 { get; set; }
    }
}