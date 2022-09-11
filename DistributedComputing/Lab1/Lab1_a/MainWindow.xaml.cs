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

        private Thread? _thread1 = null;
        private Thread? _thread2 = null;

        private string? _priority;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void startButton_Click(object sender, RoutedEventArgs e)
        {
            _thread1 = new Thread(() => SetSliderValue(10)) { Name = "Thread1", IsBackground = true };
            if (_priority == "Thread1")
                _thread1.Priority = ThreadPriority.Highest;

            _thread2 = new Thread(() => SetSliderValue(90)) { Name = "Thread2", IsBackground = true };
            if (_priority == "Thread2")
                _thread2.Priority = ThreadPriority.Highest;

            _thread1?.Start();
            _thread2?.Start();
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
}
