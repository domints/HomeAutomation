using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using src.Models.MQTT.Enums;

namespace src.Models.MQTT
{
    public class OXTSwitch
    {

        [JsonProperty("device_config_switch")]
        public string? DeviceConfigSwitch { get; set; }

        [JsonProperty("last_seen")]
        public DateTime LastSeen { get; set; }

        [JsonProperty("linkquality")]
        public int Linkquality { get; set; }

        [JsonProperty("network_led_switch")]
        public OnOff? NetworkLedSwitch { get; set; }

        [JsonProperty("power_on_behavior_relay")]
        public OXTPowerOnBehaviour? PowerOnBehaviorRelay { get; set; }

        [JsonProperty("state_relay")]
        public OnOff? StateRelay { get; set; }

        [JsonProperty("switch_action_mode_switch")]
        public OXTSwitchActionMode? SwitchActionModeSwitch { get; set; }

        [JsonProperty("switch_binded_mode_switch")]
        public OXTSwitchBindedMode? SwitchBindedModeSwitch { get; set; }

        [JsonProperty("switch_level_move_rate_switch")]
        public int SwitchLevelMoveRateSwitch { get; set; }

        [JsonProperty("switch_long_press_duration_switch")]
        public int SwitchLongPressDurationSwitch { get; set; }

        [JsonProperty("switch_mode_switch")]
        public OXTSwitchMode? SwitchModeSwitch { get; set; }

        [JsonProperty("switch_press_action_switch")]
        public OXTPressAction? SwitchPressActionSwitch { get; set; }

        [JsonProperty("switch_relay_index_switch")]
        public string? SwitchRelayIndexSwitch { get; set; }

        [JsonProperty("switch_relay_mode_switch")]
        public OXTSwitchRelayMode? SwitchRelayModeSwitch { get; set; }

        [JsonProperty("update")]
        public Z2MUpdate? Update { get; set; }
    }


}