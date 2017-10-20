using System;
using System.Windows.Forms;

namespace Horn_War_II
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            using (var game = new HornWarII())
                game.Run();
        }
    }
#endif
}
