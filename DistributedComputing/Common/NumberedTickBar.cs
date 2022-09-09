using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Common
{
    public class NumberedTickBar : TickBar
    {
        protected override void OnRender(DrawingContext dc)
        {

            var size = new Size(ActualWidth, ActualHeight);
            var tickCount = (int)((Maximum - Minimum) / TickFrequency) + 1;
            if ((Maximum - Minimum) % TickFrequency == 0)
                tickCount -= 1;
            // Calculate tick's setting
            var tickFrequencySize = (size.Width * TickFrequency / (Maximum - Minimum));

            // Draw each tick text
            for (var i = 0; i <= tickCount; i++)
            {
                var text = Convert.ToString(Convert.ToInt32(Minimum + TickFrequency * i), 10);

                var formattedText = new FormattedText(text, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 8, Brushes.Black);
                dc.DrawText(formattedText, new Point((tickFrequencySize * i), 30));

            }
        }
    }
}
