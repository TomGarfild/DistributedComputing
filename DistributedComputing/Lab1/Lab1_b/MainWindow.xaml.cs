using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Lab1_a.Annotations;
using ThreadState = System.Threading.ThreadState;

namespace Lab1_b
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public int SliderValue { get; private set; } = 0;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        private Thread? _thread1;
        private Thread? _thread2;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void start1Button_Click(object sender, RoutedEventArgs e)
        {
            _thread1 = new Thread(() => SetSliderValue(10))
            {
                Name = "Thread1",
                IsBackground = true,
                Priority = ThreadPriority.Highest
            };

            _thread1.Start();
        }

        public void start2Button_Click(object sender, RoutedEventArgs e)
        {
            _thread2 = new Thread(() => SetSliderValue(90))
            {
                Name = "Thread2",
                IsBackground = true,
                Priority = ThreadPriority.Highest
            };

            _thread2.Start();
        }

        public void stop1Button_Click(object sender, RoutedEventArgs e)
        {
            if (_thread1 != null && !_thread1.ThreadState.HasFlag(ThreadState.WaitSleepJoin))
            {
                _semaphore.Release(1);
                _thread1 = null;
                MessageBox.Show($"Thread 1 disposed and released semaphore");
            }
        }

        public void stop2Button_Click(object sender, RoutedEventArgs e)
        {
            if (_thread2 != null && !_thread2.ThreadState.HasFlag(ThreadState.WaitSleepJoin))
            {
                _semaphore.Release(1);
                _thread2 = null;
                MessageBox.Show($"Thread 2 was disposed and released semaphore");
            }
        }

        private void SetSliderValue(int value)
        {
            _semaphore.Wait();
            SliderValue = value;
            OnPropertyChanged("SliderValue");
            MessageBox.Show($"{Thread.CurrentThread.Name}, value={value}");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
