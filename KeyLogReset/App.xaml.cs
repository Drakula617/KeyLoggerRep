using KeyLogReset.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KeyLogReset
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ApplicationContext db = new ApplicationContext();
        //public static MouseHook mouseHook = new MouseHook();
    }
}
