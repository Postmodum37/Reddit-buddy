using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RedditSharp;

namespace Reddit_buddy
{
    public partial class Form1 : Form
    {
        public Reddit reddit = new Reddit();
        public Form1()
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';
            textBox2.MaxLength = 20;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            textBox1.AppendText("scriverry");
            textBox2.AppendText("postmodum3647");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text == "") || (textBox2.Text == ""))
            {
                MessageBox.Show("One or more input fields are empty.", "Wrong credentials",  MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    var user = reddit.LogIn(textBox1.Text, textBox2.Text);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
