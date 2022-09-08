using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lab1_a.Annotations;
using ThreadState = System.Threading.ThreadState;

namespace Lab1_a
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public int SliderValue { get; private set; } = 0;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        private readonly Thread _thread1;
        private readonly Thread _thread2;

        private string _priority;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _thread1 = new Thread(() => SetSliderValue(10)) { Name = "Thread1", IsBackground = true };
            _thread2 = new Thread(() => SetSliderValue(90)) { Name = "Thread2", IsBackground = true };
        }

        public void startButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (_thread1.ThreadState.HasFlag(ThreadState.Unstarted))
            {
                if (_priority == "Thread1")
                    _thread1.Priority = ThreadPriority.Highest;
                _thread1.Start();
            }

            if (_thread2.ThreadState.HasFlag(ThreadState.Unstarted))
            {
                if (_priority == "Thread2")
                    _thread2.Priority = ThreadPriority.Highest;
                _thread2.Start();
            }
        }

        private void SetSliderValue(int value)
        {
            try
            {
                _semaphore.Wait();
                SliderValue = value;
                OnPropertyChanged("SliderValue");
                MessageBox.Show($"{Thread.CurrentThread.Name}, value={value}");
            }
            finally
            {
                _semaphore.Release(1);
            }
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var pressed = (RadioButton)sender;
            _priority = pressed.Content.ToString();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

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
