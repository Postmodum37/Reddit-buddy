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

        Reddit reddit;


        public Form2(Reddit reddit)
        {
            InitializeComponent();
            this.reddit = reddit;
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

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    bottom_left_posts.Clear();
                    listView2.Items.Clear();
                    bottom_left_subreddit = reddit.GetSubreddit("/r/" + textBox1.Text);
                    foreach (var post in bottom_left_subreddit.Hot.Take(15))
                    {
                        bottom_left_posts.Add(post);
                        listView2.Items.Add(new ListViewItem(new[] { post.Title, post.Score.ToString() }));
                    }
                    label1.Text = "/r/" + bottom_left_subreddit.Name;
                    textBox1.Clear();
                }
                catch (Exception ex)
                {
                    bottom_left_posts.Clear();
                    listView2.Items.Clear();
                    bottom_left_subreddit = reddit.GetSubreddit("/r/programming");
                    foreach (var post in bottom_left_subreddit.Hot.Take(15))
                    {
                        bottom_left_posts.Add(post);
                        listView2.Items.Add(new ListViewItem(new[] { post.Title, post.Score.ToString() }));
                    }
                    label1.Text = "/r/" + bottom_left_subreddit.Name;
                    textBox1.Clear();

                    MessageBox.Show(ex.ToString(), "List update failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }              
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    bottom_right_posts.Clear();
                    listView3.Items.Clear();
                    bottom_right_subreddit = reddit.GetSubreddit("/r/" + textBox2.Text);
                    foreach (var post in bottom_right_subreddit.Hot.Take(15))
                    {
                        bottom_right_posts.Add(post);
                        listView3.Items.Add(new ListViewItem(new[] { post.Title, post.Score.ToString() }));
                    }
                    label2.Text = "/r/" + bottom_right_subreddit.Name;
                    textBox2.Clear();

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                catch (Exception ex)
                {
                    bottom_right_posts.Clear();
                    listView3.Items.Clear();
                    bottom_right_subreddit = reddit.GetSubreddit("/r/linux");
                    foreach (var post in bottom_right_subreddit.Hot.Take(15))
                    {
                        bottom_right_posts.Add(post);
                        listView3.Items.Add(new ListViewItem(new[] { post.Title, post.Score.ToString() }));
                    }
                    label2.Text = "/r/" + bottom_right_subreddit.Name;
                    textBox2.Clear();

                    MessageBox.Show(ex.ToString(), "List update failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
        }
    }
}
