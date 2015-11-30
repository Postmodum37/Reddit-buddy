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
        Subreddit bottom_left_subreddit;
        Subreddit bottom_right_subreddit;

        List<Post> default_subreddit_posts = new List<Post>();
        List<Post> bottom_left_posts = new List<Post>();
        List<Post> bottom_right_posts = new List<Post>();


        public Form2(Reddit reddit)
        {
            InitializeComponent();
            default_subredit = reddit.RSlashAll;
            label1.Text = "/r/" + default_subredit.Name;
            listView1.View = View.Details;
            listView1.Columns.Add("Title", - 2, HorizontalAlignment.Left);
            listView1.Columns.Add("Subreddit", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Score", -2, HorizontalAlignment.Left);
            listView1.Columns[0].Width = (listView1.Width / 7) * 6;
            listView1.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            listView1.Columns[2].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            foreach (var post in default_subredit.Hot.Take(15))
            {
                default_subreddit_posts.Add(post);
                listView1.Items.Add(new ListViewItem(new[] { post.Title, post.SubredditName, post.Score.ToString() }));
            }

            bottom_left_subreddit = reddit.GetSubreddit("/r/programming");
            listView2.View = View.Details;
            listView2.Columns.Add("Title", -2, HorizontalAlignment.Left);
            listView2.Columns.Add("Score", -2, HorizontalAlignment.Left);
            listView2.Columns[0].Width = (listView2.Width / 9) * 8;
            listView2.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            foreach (var post in bottom_left_subreddit.Hot.Take(15))
            {
                bottom_left_posts.Add(post);
                listView2.Items.Add(new ListViewItem(new[] { post.Title, post.Score.ToString() }));
            }

            bottom_right_subreddit = reddit.GetSubreddit("/r/linux");
            listView3.View = View.Details;
            listView3.Columns.Add("Title", -2, HorizontalAlignment.Left);
            listView3.Columns.Add("Score", -2, HorizontalAlignment.Left);
            listView3.Columns[0].Width = (listView3.Width / 9) * 8;
            listView3.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            foreach (var post in bottom_right_subreddit.Hot.Take(15))
            {
                bottom_right_posts.Add(post);
                listView3.Items.Add(new ListViewItem(new[] { post.Title, post.Score.ToString() }));
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
                        Process.Start(post.Url.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Link opening failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listView2_Click(object sender, EventArgs e)
        {
            try
            {
                var firstSelectedItem = listView1.SelectedItems[0].Text;
                foreach (var post in bottom_left_posts)
                {
                    if (post.Title == firstSelectedItem)
                    {
                        Process.Start(post.Url.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Link opening failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listView3_Click(object sender, EventArgs e)
        {
            try
            {
                var firstSelectedItem = listView1.SelectedItems[0].Text;
                foreach (var post in bottom_right_posts)
                {
                    if (post.Title == firstSelectedItem)
                    {
                        Process.Start(post.Url.ToString());
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
