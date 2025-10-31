using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Config
{
    public class MQTTConfig
    {
        public string IP { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Pass { get; set; } = string.Empty;
    }
}