using System;
using System.Windows.Forms;

namespace PuzzleGame
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Запускаємо нашу форму
            Application.Run(new MainForm());
        }
    }
}