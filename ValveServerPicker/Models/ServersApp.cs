using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ValveServerPicker.Models
{
    public class ServersApp
    {
        public Label Name { get; set; }
        public Label Ping { get; set; }
        public ToggleSwitch TF2 { get; set; }
        public ToggleSwitch CSGO { get; set; }
    }
}
