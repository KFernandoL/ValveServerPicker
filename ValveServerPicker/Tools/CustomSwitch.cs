using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using ValveServerPicker.Helpers;
using ValveServerPicker.Models.Web;

namespace ValveServerPicker.Tools
{
    public class CustomSwitch
    {
        public static async Task<ToggleSwitch> SwitchServerAsync(string gameName, string gamePath, string serverName, List<Relay> servers, bool isOn)
        {
            Style customStyle = new Style(typeof(ToggleSwitch), (Style)Application.Current.Resources["MahApps.Metro.Styles.ToggleSwitch.Win10"]);
            ToggleSwitch toggleSwitch = new ToggleSwitch
            {
                MinWidth = 0,
                IsOn = isOn,
                OffContent = "",
                OnContent = "",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,

            };
            toggleSwitch.Toggled += (sender, e) => SwitchToggleEvent(sender, e, gameName, gamePath, serverName, servers).GetAwaiter();

            await Task.Delay(5);
            return toggleSwitch;
        }

        private static async Task SwitchToggleEvent(object sender, RoutedEventArgs e, string gameName, string gamePath, string serverName, List<Relay> servers)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    FirewallHelper.CreateRule(gameName, gamePath, serverName, servers);
                }
                else
                {
                    FirewallHelper.DeleteRule(gameName, serverName);
                }
            }
            await Task.Delay(5);

        }
    }
}
