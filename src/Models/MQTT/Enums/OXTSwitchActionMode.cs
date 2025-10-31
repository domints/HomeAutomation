using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace src.Models.MQTT.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OXTSwitchActionMode
    {
        [EnumMember(Value = "on_off")]
        OnOff,
        [EnumMember(Value = "off_on")]
        OffOn,
        [EnumMember(Value = "toggle_simple")]
        ToggleSimple,
        [EnumMember(Value = "toggle_smart_sync")]
        ToggleSmartSync,
        [EnumMember(Value = "toggle_smart_opposite")]
        ToggleSmartOpposite
    }
}