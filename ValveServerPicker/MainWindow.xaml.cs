using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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

namespace ValveServerPicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Custom titleBar actions
        private void btnCerrar_MouseClick(object sender, RoutedEventArgs e) { Close(); }
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

    }
}
