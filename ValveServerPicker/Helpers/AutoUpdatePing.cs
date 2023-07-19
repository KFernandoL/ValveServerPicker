using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media;

namespace ValveServerPicker.Helpers
{
    public class AutoUpdatePing
    {
        public void AutoPing(Label pingLabel, string ip)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += (sender, e) => TimerEventProcessor(sender, e, pingLabel, ip).GetAwaiter();
            timer.Start();
        }

        private async Task TimerEventProcessor(object sender, EventArgs e, Label pingLabel, string ip)
        {

            long latencia = await Ping(ip);
            if (latencia < 80)
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

        private async Task<long> Ping(string ip)
        {
            Ping myPing = new Ping();
            PingReply reply = await myPing.SendPingAsync(ip, 1000);
            return reply.RoundtripTime;
        }
    }
}
