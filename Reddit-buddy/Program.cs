using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RedditSharp;

namespace Reddit_buddy
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 f1 = new Form1();
            DialogResult dr = f1.ShowDialog();
            var reddit = f1.reddit;
            if (dr == DialogResult.OK)
            {
                Application.Run(new Form2(reddit));
            }
            else
            {
                Application.Exit();
            }
        }
    }
}
