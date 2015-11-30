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
            createMainList(listView1, default_subredit, default_subreddit_posts);

            bottom_left_subreddit = reddit.GetSubreddit("/r/programming");
            createSideList(listView2, bottom_left_subreddit, bottom_left_posts, label1);

            bottom_right_subreddit = reddit.GetSubreddit("/r/linux");
            createSideList(listView3, bottom_right_subreddit, bottom_right_posts, label2);

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
                var firstSelectedItem = listView2.SelectedItems[0].Text;
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
                var firstSelectedItem = listView3.SelectedItems[0].Text;
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

        private void createSideList(ListView list, Subreddit sub, List<Post> posts, Label label)
        {
            list.View = View.Details;
            label.Text = "/r/" + sub.Name;
            list.Columns.Add("Title", -2, HorizontalAlignment.Left);
            list.Columns.Add("Score", -2, HorizontalAlignment.Left);
            list.Columns[0].Width = (list.Width / 9) * 8;
            list.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            foreach (var post in sub.Hot.Take(15))
            {
                posts.Add(post);
                list.Items.Add(new ListViewItem(new[] { post.Title, post.Score.ToString() }));
            }
        }

        private void createMainList(ListView list, Subreddit sub, List<Post> posts)
        {
            list.View = View.Details;
            list.Columns.Add("Title", -2, HorizontalAlignment.Left);
            list.Columns.Add("Subreddit", -2, HorizontalAlignment.Left);
            list.Columns.Add("Score", -2, HorizontalAlignment.Left);
            list.Columns[0].Width = (list.Width / 7) * 6;
            list.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            list.Columns[2].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            foreach (var post in sub.Hot.Take(15))
            {
                posts.Add(post);
                list.Items.Add(new ListViewItem(new[] { post.Title, post.SubredditName, post.Score.ToString() }));
            }
        }

        private void Form2_ResizeEnd(Object sender, EventArgs e)
        {
            try
            {
                listView1.Columns[0].Width = (listView1.Width / 10) * 8;
                listView1.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                listView1.Columns[2].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);

                listView2.Columns[0].Width = (listView2.Width / 9) * 8;
                listView2.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);

                listView3.Columns[0].Width = (listView3.Width / 9) * 8;
                listView3.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
