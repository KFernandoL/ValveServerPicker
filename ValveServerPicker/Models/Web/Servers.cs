using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xaml.Schema;

namespace ValveServerPicker.Models.Web
{
    public class Relay
    {
        [JsonPropertyName("ipv4")]
        public string IPv4 { get; set; }
        [JsonPropertyName("port_range")]
        public List<int> PortRange { get; set; }
    }

    public class Location
    {
        [JsonPropertyName("desc")]
        public string Desc { get; set; }
        [JsonPropertyName("geo")]
        public List<double> Geo { get; set; }
        [JsonPropertyName("partners")]
        public int Partners { get; set; }
        [JsonPropertyName("tier")]
        public int Tier { get; set; }
        [JsonPropertyName("relays")]
        public List<Relay> Relays { get; set; }
    }

    public class RootObject
    {
        [JsonPropertyName("revision")]
        public int Revision { get; set; }
        [JsonPropertyName("pops")]
        public Dictionary<string, Location> Pops { get; set; }
    }



}
