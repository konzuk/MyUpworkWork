using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace JobUpwork5
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BS());
        }
    }
}
