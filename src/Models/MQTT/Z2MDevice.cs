using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace src.Models.MQTT
{
    public class Target
    {

        [JsonProperty("endpoint")]
        public int Endpoint { get; set; }

        [JsonProperty("ieee_address")]
        public string? IeeeAddress { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("id")]
        public int? Id { get; set; }
    }

    public class Binding
    {

        [JsonProperty("cluster")]
        public string? Cluster { get; set; }

        [JsonProperty("target")]
        public Target? Target { get; set; }
    }

    public class Clusters
    {

        [JsonProperty("input")]
        public IList<string> Input { get; set; } = new List<string>();

        [JsonProperty("output")]
        public IList<string> Output { get; set; } = new List<string>();
    }

    public class Endpoint
    {

        [JsonProperty("bindings")]
        public IList<Binding> Bindings { get; set; } = new List<Binding>();

        [JsonProperty("clusters")]
        public Clusters? Clusters { get; set; }

        [JsonProperty("configured_reportings")]
        public IList<ConfiguredReporting> ConfiguredReportings { get; set; } = new List<ConfiguredReporting>();

        [JsonProperty("scenes")]
        public IList<object> Scenes { get; set; } = new List<object>();

        [JsonProperty("name")]
        public string? Name { get; set; }
    }

    public class ConfiguredReporting
    {

        [JsonProperty("attribute")]
        public string? Attribute { get; set; }

        [JsonProperty("cluster")]
        public string? Cluster { get; set; }

        [JsonProperty("maximum_report_interval")]
        public int MaximumReportInterval { get; set; }

        [JsonProperty("minimum_report_interval")]
        public int MinimumReportInterval { get; set; }

        [JsonProperty("reportable_change")]
        public int ReportableChange { get; set; }
    }

    public class Feature
    {

        [JsonProperty("access")]
        public int Access { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("endpoint")]
        public string? Endpoint { get; set; }

        [JsonProperty("label")]
        public string? Label { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("property")]
        public string? Property { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("value_off")]
        public string? ValueOff { get; set; }

        [JsonProperty("value_on")]
        public string? ValueOn { get; set; }

        [JsonProperty("value_toggle")]
        public string? ValueToggle { get; set; }

        [JsonProperty("unit")]
        public string? Unit { get; set; }

        [JsonProperty("value_max")]
        public int? ValueMax { get; set; }

        [JsonProperty("value_min")]
        public int? ValueMin { get; set; }

        [JsonProperty("value_step")]
        public double? ValueStep { get; set; }

        [JsonProperty("values")]
        public IList<string> Values { get; set; } = new List<string>();
    }

    public class Expos
    {

        [JsonProperty("access")]
        public int Access { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("endpoint")]
        public string? Endpoint { get; set; }

        [JsonProperty("label")]
        public string? Label { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("property")]
        public string? Property { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("value_off")]
        public object? ValueOff { get; set; }

        [JsonProperty("value_on")]
        public object? ValueOn { get; set; }

        [JsonProperty("features")]
        public IList<Feature> Features { get; set; } = new List<Feature>();

        [JsonProperty("category")]
        public string? Category { get; set; }

        [JsonProperty("values")]
        public IList<string> Values { get; set; } = new List<string>();

        [JsonProperty("value_max")]
        public double? ValueMax { get; set; }

        [JsonProperty("value_min")]
        public double? ValueMin { get; set; }

        [JsonProperty("unit")]
        public string? Unit { get; set; }

        [JsonProperty("value_step")]
        public double? ValueStep { get; set; }
    }

    public class Option
    {

        [JsonProperty("access")]
        public int Access { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("label")]
        public string? Label { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("property")]
        public string? Property { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("value_off")]
        public bool ValueOff { get; set; }

        [JsonProperty("value_on")]
        public bool ValueOn { get; set; }

        [JsonProperty("value_step")]
        public double? ValueStep { get; set; }

        [JsonProperty("value_max")]
        public int? ValueMax { get; set; }

        [JsonProperty("value_min")]
        public int? ValueMin { get; set; }

        [JsonProperty("features")]
        public IList<Feature> Features { get; set; } = new List<Feature>();

        [JsonProperty("values")]
        public IList<string> Values { get; set; } = new List<string>();
    }

    public class Definition
    {

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("exposes")]
        public IList<Expos> Exposes { get; set; } = new List<Expos>();

        [JsonProperty("model")]
        public string? Model { get; set; }

        [JsonProperty("options")]
        public IList<Option> Options { get; set; } = new List<Option>();

        [JsonProperty("source")]
        public string? Source { get; set; }

        [JsonProperty("supports_ota")]
        public bool SupportsOta { get; set; }

        [JsonProperty("vendor")]
        public string? Vendor { get; set; }
    }

    public class Z2MDevice
    {

        [JsonProperty("disabled")]
        public bool Disabled { get; set; }

        [JsonProperty("endpoints")]
        public Dictionary<string, Endpoint> Endpoints { get; set; } = new Dictionary<string, Endpoint>();

        [JsonProperty("friendly_name")]
        public string? FriendlyName { get; set; }

        [JsonProperty("ieee_address")]
        public string? IeeeAddress { get; set; }

        [JsonProperty("interview_completed")]
        public bool InterviewCompleted { get; set; }

        [JsonProperty("interview_state")]
        public string? InterviewState { get; set; }

        [JsonProperty("interviewing")]
        public bool Interviewing { get; set; }

        [JsonProperty("network_address")]
        public int NetworkAddress { get; set; }

        [JsonProperty("supported")]
        public bool Supported { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("date_code")]
        public string? DateCode { get; set; }

        [JsonProperty("definition")]
        public Definition? Definition { get; set; }

        [JsonProperty("manufacturer")]
        public string? Manufacturer { get; set; }

        [JsonProperty("model_id")]
        public string? ModelId { get; set; }

        [JsonProperty("power_source")]
        public string? PowerSource { get; set; }

        [JsonProperty("software_build_id")]
        public string? SoftwareBuildId { get; set; }
    }


}