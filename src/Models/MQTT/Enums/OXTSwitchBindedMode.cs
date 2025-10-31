using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace src.Models.MQTT.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OXTSwitchBindedMode
    {
        [EnumMember(Value = "press_start")]
        PressStart,
        [EnumMember(Value = "short_press")]
        ShortPress,
        [EnumMember(Value = "long_press")]
        LongPress
    }
}