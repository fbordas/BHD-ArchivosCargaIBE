using System;
using System.Windows.Forms;

namespace MassTemplateGenerator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool forcefirstrun = Array.Exists(args, arg => arg == "/forcefirstrun");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (Array.Exists(args, arg => arg == "/debugPrefs"))
            { Application.Run(new WndPrefs()); }
            else { Application.Run(new WndMain(forcefirstrun)); }
        }
    }
}
