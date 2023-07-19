using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using ValveServerPicker.Helpers;

namespace ValveServerPicker.Tools
{
    public class CustomRows
    {
        public static Grid CreateRow(string game, string serverName, string ip, int indice, bool status)
        {
            Grid grid = new Grid();

            // Crea las columnas
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();

            // Establece el tamaño
            colDef1.Width = new GridLength(1.5, GridUnitType.Star);
            colDef2.Width = new GridLength(0.5, GridUnitType.Star);
            colDef3.Width = new GridLength(0.5, GridUnitType.Star);

            // Agrega las columnas al grid
            grid.ColumnDefinitions.Add(colDef1);
            grid.ColumnDefinitions.Add(colDef2);
            grid.ColumnDefinitions.Add(colDef3);

            var col1 = CustomLabels.LabelName(serverName);
            Grid.SetColumn(col1, 0);

            var col2 = CustomLabels.LabelPing();
            Grid.SetColumn(col2, 1);
            new AutoUpdatePing().AutoPing(col2, ip);

            var col3 = CustomSwitch.SwitchServer(game, indice, status);
            Grid.SetColumn(col3, 2);
            col3.Toggled += (sender, args) => { Console.WriteLine("togle"); };

            grid.Children.Add(col1);
            grid.Children.Add(col2);
            grid.Children.Add(col3);

            return grid;
        }
    }
}
