using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace src.Models.MQTT
{
    public class Z2MUpdate
    {

        [JsonProperty("installed_version")]
        public int InstalledVersion { get; set; }

        [JsonProperty("latest_version")]
        public int LatestVersion { get; set; }

        [JsonProperty("state")]
        public string? State { get; set; }
    }
}