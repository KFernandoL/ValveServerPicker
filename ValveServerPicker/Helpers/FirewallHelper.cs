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
        public static void CreateRule(string gameName, string servername, List<Relay> relays)
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

            AddRule(gameName, servername, IPs.ToArray(), portsNoDuplicate);


        }

        public static void AddRule(string gameName, string serverName, IAddress[] IPs, ushort[] ports)
        {
            var rule2 = FirewallManager.Instance.CreateApplicationRule($"Block Server {gameName} - {serverName}", "tf2.exe");
            rule2.IsEnable = true;
            rule2.Protocol = FirewallProtocol.UDP;
            rule2.Direction = FirewallDirection.Outbound;
            rule2.Action = FirewallAction.Block;
            rule2.Scope = FirewallScope.All;
            rule2.RemotePorts = ports;
            rule2.RemoteAddresses = IPs;

            FirewallManager.Instance.Rules.Add(rule2);

            /* var rule = new FirewallWASRule(
                 "Hola",
                 FirewallAction.Block,
                 FirewallDirection.Outbound,
                 FirewallProfiles.Public);*/
            /*{
                 Name = "MiReglaFirewall",
                 Action = FirewallAction.Block,
                 Direction = FirewallDirection.Outbound
             };*/

            //firewallManager.Rules.Add(rule);

            /*Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);
            // Let's create a new rule
            INetFwRule2 inboundRule = (INetFwRule2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
            inboundRule.Enabled = true;
            //Allow through firewall
            inboundRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
            inboundRule.Protocol = 17; // UDP
            inboundRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
            inboundRule.RemoteAddresses = await IpRangetoString(Servers[indice].Server);
            inboundRule.RemotePorts = await PortsRangetoString(Servers[indice].Server);
            inboundRule.Name = $"Block Server {MainWindow.gameNameSelect[MainWindow.menuOpcionSelect]} - {Servers[indice].ServerName}";*/
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

        public static bool RuleExist(string gameName, string serverName)
        {
            string ruleName = $"Block Server {gameName} - {serverName}";
            var allRules = FirewallManager.Instance.Rules.ToList();
            var rule = allRules.FirstOrDefault(x => x.Name == ruleName);
            if (rule != null)
            {
                return true;
            }
            else
            {
                return false;
            }
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
