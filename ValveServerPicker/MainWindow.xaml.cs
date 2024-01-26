using MahApps.Metro.Controls;
using Microsoft.Win32;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
        public static int menuSelected = 0;

        //Games Routes
        private string TF2Path = SteamGameFinder.FindExeAsync("Team Fortress 2", "hl2.exe").GetAwaiter().GetResult();
        private string CS2Path = SteamGameFinder.FindExeAsync("Counter-Strike Global Offensive", "cs2.exe").GetAwaiter().GetResult();

        public RootObject serversTF2;
        public RootObject serversCS2;

        public MainWindow()
        {
            InitializeComponent();
            LoadServers("TF2").GetAwaiter();
        }

        public async Task LoadServers(string gameContainer)
        {
            try
            {
                serversTF2 = await SteamServersServices.getTF2ServersAsync();
                serversCS2 = await SteamServersServices.getCS2ServerAsync();

                //var Rows = await Task.WhenAll(CreateRowsAsync(serversTF2, "TF2"), CreateRowsAsync(serversCS2, "CS2"));

                var RowsTF2 = await CreateRowsAsync(serversTF2, "TF2", TF2Path);
                var RowsCS2 = await CreateRowsAsync(serversCS2, "CS2", CS2Path);

                foreach (var row in RowsTF2)
                {
                    Dispatcher.InvokeAsync(() => ServersTF2Contenedor.Children.Add(row));
                }
                foreach (var row in RowsCS2)
                {
                    Dispatcher.InvokeAsync(() => ServersCS2Contenedor.Children.Add(row));
                }
                //
                ServerTF2ScrollView.Visibility = Visibility.Visible;
                Loading.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error LoadServers");
            }
        }

        private async Task<List<Grid>> CreateRowsAsync(RootObject servers, string gameServersName, string gamePath)
        {
            var tasks = servers.Pops.Values.Select(server => CustomRows.CreateRowAsync(gameServersName, gamePath, server.Desc, server.Relays));
            var grids = await Task.WhenAll(tasks);
            return grids.ToList();
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

        #region efectos de hover y onclick en las opciones del menu
        //Click izquierdo al icono de tf2 en el menu
        private void MenuTF2_MouseClick(object sender, RoutedEventArgs e)
        {

            if (menuSelected == 1)
            {
                menuSelected = 0;
                btnTF2Hover.Visibility = Visibility.Visible;
                btnCS2Hover.Visibility = Visibility.Hidden;
                ServerTF2ScrollView.Visibility = Visibility.Visible;
                ServerCS2ScrollView.Visibility = Visibility.Hidden;
            }
        }


        //Mouse encima del icono de tf2 en el menu
        private void MenuTF2_MouseEnter(object sender, MouseEventArgs e) { btnTF2Hover.Visibility = Visibility.Visible; }

        //Mouse sale del icono de tf2 en el menu
        private void MenuTF2_MouseLeave(object sender, MouseEventArgs e)
        {
            btnTF2Hover.Visibility = menuSelected == 0 ? Visibility.Visible : Visibility.Hidden;
        }

        //Click izquierdo al icono de cs2 en el menu
        private void MenuCS2_MouseClick(object sender, RoutedEventArgs e)
        {
            if (menuSelected == 0)
            {
                menuSelected = 1;
                btnTF2Hover.Visibility = Visibility.Hidden;
                btnCS2Hover.Visibility = Visibility.Visible;
                ServerTF2ScrollView.Visibility = Visibility.Hidden;
                ServerCS2ScrollView.Visibility = Visibility.Visible;
            }
        }


        //Mouse encima del icono de cs2 en el menu
        private void MenuCS2_MouseEnter(object sender, MouseEventArgs e)
        {

            if (menuSelected == 0)
            {
                btnCS2Hover.Visibility = Visibility.Visible;
            }
            else
            {
                btnCS2Hover.Visibility = Visibility.Hidden;
            }
        }

        //Mouse sale del icono de cs2 en el menu
        private void MenuCS2_MouseLeave(object sender, MouseEventArgs e)
        {
            if (menuSelected == 1)
            {
                btnCS2Hover.Visibility = Visibility.Visible;
            }
            else
            {
                btnCS2Hover.Visibility = Visibility.Hidden;
            }
        }

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

        #endregion

    }
}
