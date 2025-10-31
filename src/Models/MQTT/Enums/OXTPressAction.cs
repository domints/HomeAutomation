using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace src.Models.MQTT.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OXTPressAction
    {
        [EnumMember(Value = "released")]
        Released,
        [EnumMember(Value = "press")]
        Press,
        [EnumMember(Value = "long_press")]
        LongPress
    }
}