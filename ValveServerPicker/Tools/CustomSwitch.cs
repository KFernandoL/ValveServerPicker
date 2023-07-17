using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace ValveServerPicker.Tools
{
    public class CustomSwitch
    {
        public static ToggleSwitch SwitchServer(bool isOn)
        {
            return new ToggleSwitch
            {
                MinWidth = 0,
                IsOn = isOn,
                OffContent = "",
                OnContent = "",
                ContentDirection = FlowDirection.LeftToRight,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
        }
    }
}
