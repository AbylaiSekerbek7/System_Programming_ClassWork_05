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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ClassWork_05
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            ThreadPool.QueueUserWorkItem(ThreadProcedure, this);
        }

        public ManualResetEvent GlobalEvent = null; // Ссылка на глобальное событие для всех потоков
        public ManualResetEvent Event = new ManualResetEvent(false); // Объект события для потока этого окна

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Event.Reset(); // Event <== False
        }

        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            Event.Set(); // Event <== True
        }

        private void ThreadProcedure(object param)
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            Window1 form = param as Window1;
            form.Dispatcher.Invoke(new Action(() =>
            {
                txtOut.Text = ""; // очистить окно вывода
                txtOut.Text = $"Thread with id: {Thread.CurrentThread.ManagedThreadId} is started\n"; // очистить окно вывода
            }));
            while (form.GlobalEvent == null)
            {
                Thread.Sleep(0);
            }
            while (true)
            {   // Разрешения на выполнение работы потока 
                form.GlobalEvent.WaitOne();
                form.Event.WaitOne();
                int temp = rnd.Next(1, 100);
                form.Dispatcher.Invoke(new Action(() =>
                {
                     txtOut.Text += temp.ToString();
                }));
                Thread.Sleep(200);
            }
        }
    }
}
