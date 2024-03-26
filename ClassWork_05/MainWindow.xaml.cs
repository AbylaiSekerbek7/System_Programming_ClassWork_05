using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ClassWork_05
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
        }

        public ManualResetEvent GlobalEvent = new ManualResetEvent(false);
        public int ThreadCount = 0;

        private void NewWindow_Click(object sender, RoutedEventArgs e)
        {
            Thread th = new Thread(Start_Window1);
            ThreadCount++;
            th.IsBackground = true; // Thread is background
            th.Name = $"Thread N {ThreadCount}";
            th.SetApartmentState (ApartmentState.STA);
            th.Start(this);
            // th.Start(GlobalEvent);
        }

        private void Start_Window1(object param)
        {
            MainWindow form = param as MainWindow;
            // form.GlobalEvent;
            Window1 wind = new Window1();
            wind.GlobalEvent = form.GlobalEvent;
            wind.Title = $"Window 1 : {Thread.CurrentThread.Name}";
            wind.ShowDialog();
        }

        private void StartAllThread_Click(object sender, RoutedEventArgs e)
        {
            GlobalEvent.Set(); // GlobalEvent <== True - Start
        }

        private void StopAllThread_Click(object sender, RoutedEventArgs e)
        {
            GlobalEvent.Reset(); // GlobalEvent <== False - Stop
        }
    }
}
