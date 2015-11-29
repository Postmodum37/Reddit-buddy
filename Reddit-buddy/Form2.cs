using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedditSharp;
using RedditSharp.Things;
using System.Windows.Forms;
using System.Diagnostics;

namespace Reddit_buddy
{
    public partial class Form2 : Form
    {
        Subreddit default_subredit;
        List<Post> default_subreddit_posts = new List<Post>();

        public Form2(Reddit reddit)
        {
            InitializeComponent();
            default_subredit = reddit.GetSubreddit("/r/programming");
            Debug.WriteLine(default_subredit.GetType());
            label1.Text = "/r/" + default_subredit.Name;
            listView1.View = View.Details;
            listView1.Columns.Add("Title", - 2, HorizontalAlignment.Left);
            listView1.Columns.Add("Score", -2, HorizontalAlignment.Left);
            foreach (var post in default_subredit.Hot.Take(15))
            {
                default_subreddit_posts.Add(post);
                listView1.Items.Add(new ListViewItem(new[] { post.Title, post.Score.ToString() }));
            }
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            try
            {
                var firstSelectedItem = listView1.SelectedItems[0].Text;
                foreach (var post in default_subreddit_posts)
                {
                    if (post.Title == firstSelectedItem)
                    {
                        System.Diagnostics.Process.Start(post.Url.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Link opening failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
