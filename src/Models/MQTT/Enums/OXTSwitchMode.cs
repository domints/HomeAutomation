using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace src.Models.MQTT.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OXTSwitchMode
    {
        [EnumMember(Value = "toggle")]
        Toggle,
        [EnumMember(Value = "momentary")]
        Momentary,
        [EnumMember(Value = "momentary_nc")]
        MomentaryNC,
        [EnumMember(Value = "multifunction")]
        Multifunction
    }
}