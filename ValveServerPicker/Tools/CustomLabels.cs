using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ValveServerPicker.Tools
{
    public class CustomLabels
    {

        public static async Task<Label> LabelNameAsync(string content)
        {
            Label newLabel = new Label
            {
                Content = content,
                Foreground = Brushes.White,
                FontSize = 18,
                Padding = new Thickness(25, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            await Task.Delay(5);
            return newLabel;
        }

        public static async Task<Label> LabelPingAsync()
        {
            Label newLabel = new Label
            {
                Content = "0",
                Foreground = Brushes.Black,
                FontSize = 16,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            await Task.Delay(5);
            return newLabel;
        }

        public static Label LabelPing(int ping)
        {
            SolidColorBrush? ForegoundColor = (ping < 60 ? Brushes.GreenYellow : (ping > 60 && ping < 120) ? Brushes.Yellow : Brushes.Red);
            return new Label
            {
                Content = $"{ping}",
                Foreground = ForegoundColor,
                FontSize = 16,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
        }

    }
}
