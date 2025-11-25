using System;
using System.Windows.Forms;

namespace FullSearch
{
    static class Program
    {
        [STAThread] 
        static void Main()
        {
            Application.Run(new MainForm()); // Start the application with MainForm as the main window
        }
    }
    /// Short Program class as all classes are sperated and in their own files. The Main method starts the application but 
    /// transfers to MainForm for the UI and logic. The MainFrom file contains the bulk of the code for the UI and logic.

}