using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using ValveServerPicker.Helpers;
using ValveServerPicker.Models.Web;

namespace ValveServerPicker.Tools
{
    public class CustomSwitch
    {
        public static ToggleSwitch SwitchServer(string gameName, string serverName, List<Relay> servers, bool isOn)
        {
            ToggleSwitch toggleSwitch = new ToggleSwitch
            {
                MinWidth = 0,
                IsOn = isOn,
                OffContent = "",
                OnContent = "",
                ContentDirection = FlowDirection.LeftToRight,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            toggleSwitch.Toggled += (sender, e) => SwitchToggleEvent(sender, e, gameName, serverName, servers);

            return toggleSwitch;
        }

        private static void SwitchToggleEvent(object sender, RoutedEventArgs e, string gameName, string serverName, List<Relay> servers)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    FirewallHelper.CreateRule(gameName, serverName, servers);
                }
                else
                {
                    FirewallHelper.DeleteRule(gameName, serverName);
                }
            }

        }
    }
}
