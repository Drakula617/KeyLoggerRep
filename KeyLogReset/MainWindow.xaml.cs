using Hooks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyLogReset
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool startStop = false;
        
        public MainWindow()
        {
            InitializeComponent();
            //MessageBox.Show(App.db.Database.Connection.ConnectionString);
            //App.db.Key_Events.RemoveRange(App.db.Key_Events);
            //App.db.SaveChanges();
            ProgCMB.ItemsSource = App.db.Key_Events.ToList().Select(c => c.ProgramName).Distinct().ToList();

            StartStopBtn.Content = "Старт";

            Hooks.KBDHook.LocalHook = false;
            Hooks.KBDHook.KeyDown += new KBDHook.HookKeyPress(KBDHook_KeyDownAsync);
            Hooks.KBDHook.InstallHook();


            //MouseHook mouseHook = new MouseHook();
            //App.mouseHook.LeftButtonDown += MouseHook_LeftButtonDown;
            //App.mouseHook.Install();
        }

        //private void MouseHook_LeftButtonDown(MouseHook.MSLLHOOKSTRUCT mouseStruct)
        //{
        //    System.Windows.Forms.MessageBox.Show($"прикол");
        //}

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


        private async void KBDHook_KeyDownAsync(LLKHEventArgs e)
        {
            
            try
            {
                if (startStop == true)
                {
                    string keyStr = e.Keys.ToString();
                    IntPtr hWnd = GetForegroundWindow();
                    uint procId = 0;
                    GetWindowThreadProcessId(hWnd, out procId);
                    var proc = Process.GetProcessById((int)procId);
                    App.db.Key_Events.Add(new Model.Key_Event()
                    {
                        KeyName = keyStr,
                        ProgramName = proc.ProcessName.ToString(),
                        DatetimeEvent = DateTime.Now
                    });
                    await App.db.SaveChangesAsync();
                }
            }
            catch
            {
                return;
            }
            
        }

        private void StartStopBtn_Click(object sender, RoutedEventArgs e)
        {
            if (startStop == false)
            {
                startStop = true;
                StartStopBtn.Content = "Стоп";
            }
            else
            {
                startStop = false;
                StartStopBtn.Content = "Старт";
            }
        }

        public async Task UpdateData()
        {
            EventGrid.ItemsSource = null; 
            if (ProgCMB.SelectedItem != null)
                EventGrid.ItemsSource = App.db.Key_Events.ToList().
                Where(x => x.ProgramName == ProgCMB.SelectedItem.ToString()).
                ToList().GroupBy(c=> c.KeyName).ToList().Select(x => new {Name = x.Key, Count = x.Count() }).ToList();
                await Task.Delay(0);
        }

        private async void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            ProgCMB.ItemsSource = App.db.Key_Events.ToList().Select(c => c.ProgramName).Distinct().ToList();
            await UpdateData();
        }

        private async void ProgCMB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await UpdateData();
        }

        private async void RemoveBTN_Click(object sender, RoutedEventArgs e)
        {
            if (ProgCMB.SelectedItem == null)
                return;
            DialogResult dialogResult = System.Windows.Forms.MessageBox.Show($"Вы дейсвительно хотите удалить логи к программме {ProgCMB.SelectedItem}?","Предупреждение!", MessageBoxButtons.YesNo);
            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                App.db.Key_Events.RemoveRange(App.db.Key_Events.ToList().Where(c=> c.ProgramName == ProgCMB.SelectedItem.ToString()));
                App.db.SaveChanges();
                ProgCMB.ItemsSource = App.db.Key_Events.ToList().Select(c => c.ProgramName).Distinct().ToList();
                await UpdateData();
            }
            else
            { 
                
            }
        }
    }
}
