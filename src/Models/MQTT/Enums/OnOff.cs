using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace src.Models.MQTT.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OnOff
    {
        [EnumMember(Value = "ON")]
        On,
        [EnumMember(Value = "OFF")]
        Off
    }

    public static class OnOffExtensions
    {
        public static bool IsOn(this OnOff value) => value == OnOff.On;
        public static bool IsOff(this OnOff value) => value == OnOff.Off;
    }
}