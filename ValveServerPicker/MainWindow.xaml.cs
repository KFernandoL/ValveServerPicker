using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ValveServerPicker.Helpers;
using ValveServerPicker.Models;
using ValveServerPicker.Models.Web;
using ValveServerPicker.Servicesasd;
using ValveServerPicker.Tools;

namespace ValveServerPicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public enum Game
        {
            TF2 = 0,
            CS2 = 1,
            L4D2 = 2
        }

        public static Game menuSelected = Game.TF2;

        //Games Routes
        private string TF2Path = SteamGameFinder.FindExeAsync("Team Fortress 2", "hl2.exe").GetAwaiter().GetResult();
        private string CS2Path = SteamGameFinder.FindExeAsync("Counter-Strike Global Offensive", "cs2.exe").GetAwaiter().GetResult();
        //private string L4D2Path = SteamGameFinder.FindExeAsync("Left 4 Dead 2", "left4dead2.exe").GetAwaiter().GetResult();

        public RootObject serversTF2;
        public RootObject serversCS2;
        //public SteamBrowserServer serversL4D2;



        public MainWindow()
        {
            InitializeComponent();
            LoadServers().GetAwaiter();
            //disable left 4 dead 2
            MenuL4D2.Visibility = Visibility.Hidden;
            ServerL4D2ScrollView.Visibility = Visibility.Hidden;
            ServersL4D2Contenedor.Visibility = Visibility.Hidden;
        }

        public async Task LoadServers()
        {
            try
            {
                serversTF2 = await SteamServersServices.getTF2ServersAsync();
                serversCS2 = await SteamServersServices.getCS2ServerAsync();
                //serversL4D2 = await SteamServersServices.getL4D2ServerAsync();

                var RowsTF2 = await CreateRowsAsync(serversTF2, Game.TF2, "TF2", TF2Path);
                var RowsCS2 = await CreateRowsAsync(serversCS2, Game.CS2, "CS2", CS2Path);
                //var RowsL4D2 = await CreateRowsAsync(serversL4D2, Game.L4D2, "L4D2", L4D2Path);

                foreach (var row in RowsTF2)
                {
                    Dispatcher.InvokeAsync(() => ServersTF2Contenedor.Children.Add(row));
                }
                foreach (var row in RowsCS2)
                {
                    Dispatcher.InvokeAsync(() => ServersCS2Contenedor.Children.Add(row));
                }
                /*foreach (var row in RowsL4D2)
                {
                    Dispatcher.InvokeAsync(() => ServersL4D2Contenedor.Children.Add(row));
                }*/
                //Initial Values
                ServerTF2ScrollView.Visibility = Visibility.Visible;
                Loading.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                
            }
        }

        private async Task<List<Grid>> CreateRowsAsync(RootObject servers, Game game, string gameServersName, string gamePath)
        {
            var tasks = servers.Pops.Values.Select(server => CustomRows.CreateRowAsync(game, gameServersName, gamePath, server.Desc, server.Relays));
            var grids = await Task.WhenAll(tasks);
            return grids.ToList();
        }

        private async Task<List<Grid>> CreateRowsAsync(SteamBrowserServer servers, Game game, string gameServersName, string gamePath)
        {
            List<Grid> grids = new List<Grid>();
            //regex remove unnecesary data
            var regex = new Regex(@"\s*\([^)]*\)");
            var regexRemovePort = new Regex(@":[0-9]+");
            var regexRemoveValveName = new Regex(@"Valve Left4Dead 2 ");
            var regexRemoveServerText = new Regex(@" Server");
            //remove unnecesary data
            servers.response.servers.ForEach(x => x.name = regex.Replace(x.name, ""));
            servers.response.servers.ForEach(x => x.name = regexRemoveValveName.Replace(x.name, ""));
            servers.response.servers.ForEach(x => x.name = regexRemoveServerText.Replace(x.name, ""));
            servers.response.servers.ForEach(x => x.addr = regexRemovePort.Replace(x.addr, ""));
            //group by name server
            var regions = servers.response.servers.GroupBy(x => x.name).Select(x => x.First()).ToList();

            //Create Rows by Regios
            foreach (var region in regions)
            {
                var serverByRegion = servers.response.servers.Where(x => x.name.StartsWith(region.name)).ToList();
                var groupIpByRegion = serverByRegion.GroupBy(x => x.addr).Select(x => x.First()).ToList();

                //Create relays list
                List<Relay> relays = new List<Relay>();
                foreach (var serverByIp in groupIpByRegion)
                {
                    relays.Add(new Relay
                    {
                        IPv4 = serverByIp.addr,
                        PortRange = serverByRegion.Where(x => x.addr.StartsWith(serverByIp.addr)).OrderBy(x => x.gameport).DistinctBy(x => x.gameport).Select(x => x.gameport).ToList(),
                    });
                }

                var row = await CustomRows.CreateRowAsync(game, gameServersName, gamePath, region.name, relays);
                grids.Add(row);
            }

            return grids;
        }


        #region Custom titleBar actions
        private void btnCerrar_MouseClick(object sender, RoutedEventArgs e)
        { Close(); }
        private void btnMinimizar_MouseClick(object sender, RoutedEventArgs e) { WindowState = WindowState.Minimized; }
        private void titleBar_MouseLeftButtonDown(object sender, MouseEventArgs e) { DragMove(); }
        #endregion

        #region Custom titleBar Hover
        //Hover effect button Min
        private void btnMinimizar_MouseEnter(object sender, MouseEventArgs e) { btnMinimizar.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#b8aa02")); }
        private void btnMinimizar_MouseLeave(object sender, MouseEventArgs e) { btnMinimizar.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FAE800")); }
        //Hover effect btn Closed
        private void btnCerrar_MouseEnter(object sender, MouseEventArgs e) { btnCerrar.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#96262a")); }

        private void btnCerrar_MouseLeave(object sender, MouseEventArgs e) { btnCerrar.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FC4850")); }
        #endregion

        #region Button Menu TF2 Events
        //Click izquierdo al icono de tf2 en el menu
        private void MenuTF2_MouseClick(object sender, RoutedEventArgs e)
        {

            if (menuSelected != Game.TF2)
            {
                menuSelected = Game.TF2;
                Grid.SetColumn(btnHoverEffect, (int)Game.TF2);
                ServerTF2ScrollView.Visibility = Visibility.Visible;
                ServerCS2ScrollView.Visibility = Visibility.Hidden;
                ServerL4D2ScrollView.Visibility = Visibility.Hidden;
            }
        }

        private void MenuTF2_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid.SetColumn(btnHoverEffect, (int)Game.TF2);
        }

        private void MenuTF2_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid.SetColumn(btnHoverEffect, (int)menuSelected);
        }
        #endregion

        #region Button Menu CS2 Events
        private void MenuCS2_MouseClick(object sender, RoutedEventArgs e)
        {
            if (menuSelected != Game.CS2)
            {
                menuSelected = Game.CS2;
                Grid.SetColumn(btnHoverEffect, (int)Game.CS2);
                ServerTF2ScrollView.Visibility = Visibility.Hidden;
                ServerCS2ScrollView.Visibility = Visibility.Visible;
                ServerL4D2ScrollView.Visibility = Visibility.Hidden;
            }
        }

        private void MenuCS2_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid.SetColumn(btnHoverEffect, 1);

        }

        private void MenuCS2_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid.SetColumn(btnHoverEffect, (int)Game.CS2);
        }
        #endregion

        #region Button Menu L4D2 Events
        private void MenuL4D2_Click(object sender, RoutedEventArgs e)
        {
            if (menuSelected != Game.L4D2)
            {
                menuSelected = Game.L4D2;
                Grid.SetColumn(btnHoverEffect, (int)Game.L4D2);
                ServerTF2ScrollView.Visibility = Visibility.Hidden;
                ServerCS2ScrollView.Visibility = Visibility.Hidden;
                ServerL4D2ScrollView.Visibility = Visibility.Visible;

            }
        }
        private void MenuL4D2_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid.SetColumn(btnHoverEffect, (int)Game.L4D2);
        }

        private void MenuL4D2_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid.SetColumn(btnHoverEffect, (int)menuSelected);
        }
        #endregion
        private void MainWindows_Loaded(object sender, RoutedEventArgs e)
        {
            // Obtener el tamaño de la pantalla actual
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;

            // Establecer tamaños en porcentaje
            double widthPercentage = 0.225; // 80% del ancho de la pantalla
            double heightPercentage = 0.70; // 60% del alto de la pantalla

            // Establecer los tamaños de la ventana en porcentaje de la pantalla
            this.Width = screenWidth * widthPercentage;
            this.Height = screenHeight * heightPercentage;

            // Opcional: Centrar la ventana en la pantalla
            this.Left = (screenWidth - this.Width) / 2;
            this.Top = (screenHeight - this.Height) / 2;
        }

        private void AddCustomServer_Click(object sender, RoutedEventArgs e)
        {
            new ValveCustomServerBlocker.MainWindow().Show();
        }
    }
}
