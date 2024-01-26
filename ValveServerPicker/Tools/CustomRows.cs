using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using ValveServerPicker.Helpers;
using ValveServerPicker.Models.Web;
using System.Windows.Media;

namespace ValveServerPicker.Tools
{
    public class CustomRows
    {
        public static async Task<Grid> CreateRowAsync(string game, string gamePath, string nameServer, List<Relay>? server)
        {
            var ip = server != null ? server[0].IPv4 : "127.0.0.1";

            Grid grid = new Grid();

            // Crea las columnas de manera asíncrona
            var colDef1Task = CreateColumnDefinitionAsync(1.5, GridUnitType.Star);
            var colDef2Task = CreateColumnDefinitionAsync(0.5, GridUnitType.Star);
            var colDef3Task = CreateColumnDefinitionAsync(0.5, GridUnitType.Star);

            // Espera a que se completen las tareas de creación de columnas
            var colDef1 = await colDef1Task;
            var colDef2 = await colDef2Task;
            var colDef3 = await colDef3Task;

            // Agrega las columnas al grid
            grid.ColumnDefinitions.Add(colDef1);
            grid.ColumnDefinitions.Add(colDef2);
            grid.ColumnDefinitions.Add(colDef3);

            var col1Task = CustomLabels.LabelNameAsync(nameServer);
            var col2Task = CustomLabels.LabelPingAsync();
            var col3Task = CustomSwitch.SwitchServerAsync(game, gamePath, nameServer, server, await FirewallHelper.RuleExistAsync(game, nameServer));

            var col1 = await col1Task;
            var col2 = await col2Task;
            var col3 = await col3Task;

            await new AutoUpdatePing().AutoPingAsync(col2, ip);

            // Agregar bordes blancos a las dos primeras columnas
            var border1 = new Border
            {
                BorderBrush = Brushes.White,
                BorderThickness = new Thickness(0, 0, 1, 0),
                Child = col1
            };

            var border2 = new Border
            {
                BorderBrush = Brushes.White,
                BorderThickness = new Thickness(0, 0, 1, 0),
                Child = col2
            };


            Grid.SetColumn(border1, 0);
            Grid.SetColumn(border2, 1);
            Grid.SetColumn(col3, 2);

            grid.Children.Add(border1);
            grid.Children.Add(border2);
            grid.Children.Add(col3);

            return grid;
        }

        private static Task<ColumnDefinition> CreateColumnDefinitionAsync(double value, GridUnitType type)
        {
            return Task.FromResult(new ColumnDefinition { Width = new GridLength(value, type) });
        }


    }
}
