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
        private static Uri ServerCSGO = new Uri("https://api.steampowered.com/ISteamApps/GetSDRConfig/v1?appid=730");

        public static async Task<RootObject> getTF2Servers()
        {

            var jsonTF2Server = await getJsonFromUrlAsync(ServersTF2);

            return JsonSerializer.Deserialize<RootObject>(jsonTF2Server);
        }

        public static async Task<RootObject> getCSGOServer()
        {
            var jsonCSGOServer = await getJsonFromUrlAsync(ServerCSGO);

            return JsonSerializer.Deserialize<RootObject>(jsonCSGOServer);
        }

        private static async Task<string> getJsonFromUrlAsync(Uri url)
        {
            using HttpClient client = new HttpClient();

            try
            {
                // Send Get request
                HttpResponseMessage response = await client.GetAsync(ServersTF2);

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
