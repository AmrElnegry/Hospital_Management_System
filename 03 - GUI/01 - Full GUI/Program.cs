using System;
using System.Windows.Forms;

namespace Project_Hospital
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            DB.Load();
            Application.ApplicationExit += delegate { DB.Save(); };
            AppDomain.CurrentDomain.ProcessExit += delegate { DB.Save(); };
            AppDomain.CurrentDomain.UnhandledException += delegate { DB.Save(); };
            Application.Run(new MainForm());
        }
    }
}
