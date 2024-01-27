using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ValveServerPicker.Models.Web;

namespace ValveServerPicker.Servicesasd
{

    class SteamServersServices
    {
        private static Uri ServersTF2 = new Uri("https://api.steampowered.com/ISteamApps/GetSDRConfig/v1?appid=440");
        private static Uri ServerCS2 = new Uri("https://api.steampowered.com/ISteamApps/GetSDRConfig/v1?appid=730");
        private static Uri ServerL4D2 = new Uri("https://api.steampowered.com/IGameServersService/GetServerList/v1/?&limit=50000&filter=\\gamedir\\left4dead2\\appid\\550\\dedicated\\1\\secure\\1");

        public static async Task<RootObject> getTF2ServersAsync()
        {

            var jsonTF2Server = await getJsonFromUrlAsync(ServersTF2);
            var serializeServers = JsonSerializer.Deserialize<RootObject>(jsonTF2Server);
            var orderRelays = serializeServers.Pops.Values.OrderBy(x => x.Relays.Count);

            return serializeServers;
        }

        public static async Task<RootObject> getCS2ServerAsync()
        {
            var jsonCS2Server = await getJsonFromUrlAsync(ServerCS2);

            return JsonSerializer.Deserialize<RootObject>(jsonCS2Server);
        }

        public static async Task<SteamBrowserServer> getL4D2ServerAsync()
        {
            var jsonL4D2Server = await getJsonFromUrlAsync(ServerL4D2);
            var serializeServers = JsonSerializer.Deserialize<SteamBrowserServer>(jsonL4D2Server);
            serializeServers.response.servers = serializeServers.response.servers.Where(x => x.name.StartsWith("Valve Left4Dead 2 ")).ToList();

            return serializeServers;
        }

        private static async Task<string> getJsonFromUrlAsync(Uri url)
        {
            using HttpClient client = new HttpClient();

            try
            {
                // Send Get request
                HttpResponseMessage response = await client.GetAsync(url);

                //Http OK
                if (response.IsSuccessStatusCode)
                {
                    // Read response
                    string json = await response.Content.ReadAsStringAsync();
                    return json;
                }

                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }
}
