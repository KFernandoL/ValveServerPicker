using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media;
using static ValveServerPicker.MainWindow;

namespace ValveServerPicker.Helpers
{
    public class AutoUpdatePing
    {

        public async Task AutoPingAsync(Label pingLabel, string ip, Game game)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1200);

            // Usamos una función lambda asincrónica para el controlador de eventos Tick
            timer.Tick += async (sender, e) => await TimerEventProcessor(sender, e, pingLabel, ip, game);

            timer.Start();
        }


        private async Task TimerEventProcessor(object sender, EventArgs e, Label pingLabel, string ip, Game game)
        {
            if(game == menuSelected)
            {
                long latencia = await Ping(ip);
                if (latencia <= 0)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        pingLabel.Foreground = Brushes.Black;
                        pingLabel.Content = latencia;
                    }));
                }
                else if (latencia < 80)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        pingLabel.Foreground = Brushes.GreenYellow;
                        pingLabel.Content = latencia;
                    }));

                }
                else if (latencia >= 81 && latencia <= 120)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        pingLabel.Foreground = Brushes.Orange;
                        pingLabel.Content = latencia;
                    }));
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        pingLabel.Foreground = Brushes.Red;
                        pingLabel.Content = latencia;
                    }));
                }
            }
        }

        private async Task<long> Ping(string ip)
        {
            Ping myPing = new Ping();
            PingReply reply = await myPing.SendPingAsync(ip, 1000);
            return reply.RoundtripTime;
        }
    }
}
