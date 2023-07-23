using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using WindowsFirewallHelper.FirewallRules;
using WindowsFirewallHelper;
using System.Data;
using System.Net;
using WindowsFirewallHelper.Addresses;
using ValveServerPicker.Models.Web;

namespace ValveServerPicker.Helpers
{
    public class FirewallHelper
    {
        public static void CreateRule(string gameName, string gamePath, string servername, List<Relay> relays)
        {
            List<ushort> ports = new List<ushort>();
            List<IAddress> IPs = new List<IAddress>();
            foreach (var relay in relays)
            {
                var firstPort = (ushort)relay.PortRange.First();
                var lastPort = (ushort)relay.PortRange.Last();
                var portsList = PortsList(firstPort, lastPort);
                ports.AddRange(portsList);

                IPs.Add(new SingleIP(IPAddress.Parse(relay.IPv4)));
            }
            ushort[] portsNoDuplicate = ports.Distinct().ToArray();

            AddRule(gameName, gamePath, servername, IPs.ToArray(), portsNoDuplicate);


        }

        public static void AddRule(string gameName, string gamePath, string serverName, IAddress[] IPs, ushort[] ports)
        {
            var rule2 = FirewallManager.Instance.CreateApplicationRule($"Block Server {gameName} - {serverName}", gamePath);
            rule2.IsEnable = true;
            rule2.Protocol = FirewallProtocol.UDP;
            rule2.Direction = FirewallDirection.Outbound;
            rule2.Action = FirewallAction.Block;
            rule2.Scope = FirewallScope.All;
            rule2.RemotePorts = ports;
            rule2.RemoteAddresses = IPs;

            FirewallManager.Instance.Rules.Add(rule2);
        }

        public static List<ushort> PortsList(ushort firstPort, ushort lastPort)
        {
            int totalPortsInRange = PortsInRange(firstPort, lastPort);
            List<ushort> portsList = new List<ushort>();

            for (int i = 0; i < totalPortsInRange; i++)
            {
                portsList.Add((ushort)(firstPort + i));
            }
            return portsList;
        }

        public static int PortsInRange(ushort firstPort, ushort lastPort)
        {
            return (lastPort - firstPort) + 1;
        }

        public static async Task<bool> RuleExistAsync(string gameName, string serverName)
        {
            string ruleName = $"Block Server {gameName} - {serverName}";

            // Utilizamos un método asincrónico específico para buscar la regla
            var rule = await Task.Run(() => FindRule(ruleName));

            // Verificamos si la regla existe
            return rule != null;
        }

        // Método de búsqueda de la regla específico que se ejecutará en otro hilo
        private static IFirewallRule FindRule(string ruleName)
        {
            // Obtenemos todas las reglas de manera síncrona
            var allRules = FirewallManager.Instance.Rules;

            // Buscamos la regla utilizando LINQ (síncrono)
            var rule = allRules.FirstOrDefault(x => x.Name == ruleName);

            return rule;
        }


        public static void DeleteRule(string gameName, string serverName)
        {
            string ruleName = $"Block Server {gameName} - {serverName}";
            var myRule = FirewallManager.Instance.Rules.SingleOrDefault(r => r.Name == ruleName);
            if (myRule != null)
            {
                FirewallManager.Instance.Rules.Remove(myRule);
            }
        }
    }
}
