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
        private Subreddit default_subredit;
        private Subreddit bottom_left_subreddit;
        private Subreddit bottom_right_subreddit;

        private List<Post> default_subreddit_posts = new List<Post>();
        private List<Post> bottom_left_posts = new List<Post>();
        private List<Post> bottom_right_posts = new List<Post>();

        private List<Post> default_subreddit_posts_sorted = new List<Post>();
        private List<Post> bottom_left_posts_sorted = new List<Post>();
        private List<Post> bottom_right_posts_sorted = new List<Post>();


        private int numberOfLinks;

        Reddit reddit;


        public Form2(Reddit reddit)
        {
            InitializeComponent();
            radioButton1.Checked = true;
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
                updateListviewWithNewSubreddit(listView2, bottom_left_subreddit, bottom_left_posts, textBox1, label1, defaultSub: "programming");

                e.Handled = true;
                e.SuppressKeyPress = true;        
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                updateListviewWithNewSubreddit(listView3, bottom_right_subreddit, bottom_right_posts, textBox2, label2, defaultSub: "linux");

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void updateListviewWithRandomSubreddit(ListView view, Subreddit sub, List<Post> posts, TextBox textBox, Label label)
        {
            posts.Clear();
            view.Items.Clear();
            sub = reddit.GetSubreddit("/r/random");
            foreach (var post in sub.Hot.Take(15))
            {
                posts.Add(post);
                view.Items.Add(new ListViewItem(new[] { post.Title, post.Score.ToString() }));
            }
            label.Text = "/r/" + sub.Name;
            textBox.Clear();
        }


        private void updateListviewWithNewSubreddit(ListView view, Subreddit sub, List<Post> posts, TextBox textBox, Label label, string defaultSub = "programming")
        {
            try
            {
                posts.Clear();
                view.Items.Clear();
                sub = reddit.GetSubreddit("/r/" + textBox.Text);
                foreach (var post in sub.Hot.Take(15))
                {
                    posts.Add(post);
                    view.Items.Add(new ListViewItem(new[] { post.Title, post.Score.ToString() }));
                }
                label.Text = "/r/" + sub.Name;
                textBox.Clear();

            }
            catch (Exception ex)
            {
                posts.Clear();
                view.Items.Clear();
                sub = reddit.GetSubreddit("/r/" + defaultSub);
                foreach (var post in sub.Hot.Take(15))
                {
                    posts.Add(post);
                    view.Items.Add(new ListViewItem(new[] { post.Title, post.Score.ToString() }));
                }
                label.Text = "/r/" + sub.Name;
                textBox.Clear();

                MessageBox.Show(ex.ToString(), "List update failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            updateListviewWithRandomSubreddit(listView2, bottom_left_subreddit, bottom_left_posts, textBox1, label1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            updateListviewWithRandomSubreddit(listView3, bottom_right_subreddit, bottom_right_posts, textBox2, label2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked || radioButton2.Checked)
                {
                    if (checkBox1.Checked || checkBox2.Checked || checkBox3.Checked)
                    {
                        if (int.TryParse(textBox3.Text, out numberOfLinks))
                        {
                            if (numberOfLinks > 2 && numberOfLinks < 8)
                            {
                                openLinks(numberOfLinks);
                            }
                        }
                        else
                        {
                            throw new Exception("Couldn't prase entered number. Please enter a number between [3, 7].");
                        }
                    }
                    else
                    {
                        throw new Exception("Please select at least one list.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Link opening failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }          
        }

        private void openLinks(int numberOfLinks)
        {
            if (radioButton1.Checked)
            {
                if (checkBox1.Checked)
                {
                    for (int i = 0; i < numberOfLinks; i++)
                    {
                        Process.Start(default_subreddit_posts[i].Url.ToString());
                    }
                }
                if (checkBox2.Checked)
                {
                    for (int i = 0; i < numberOfLinks; i++)
                    {
                        Process.Start(bottom_left_posts[i].Url.ToString());
                    }
                }
                if (checkBox3.Checked)
                {
                    for (int i = 0; i < numberOfLinks; i++)
                    {
                        Process.Start(bottom_right_posts[i].Url.ToString());
                    }
                }
            }
            else if (radioButton2.Checked)
            {
                if (checkBox1.Checked)
                {
                    default_subreddit_posts_sorted = default_subreddit_posts.OrderByDescending(o => o.Score).ToList();
                    for (int i = 0; i < numberOfLinks; i++)
                    {
                        Process.Start(default_subreddit_posts_sorted[i].Url.ToString());
                    }
                }
                if (checkBox2.Checked)
                {
                    bottom_left_posts_sorted = bottom_left_posts.OrderByDescending(o => o.Score).ToList();

                    for (int i = 0; i < numberOfLinks; i++)
                    {
                        Process.Start(bottom_left_posts[i].Url.ToString());
                    }
                }
                if (checkBox3.Checked)
                {
                    bottom_right_posts_sorted = bottom_right_posts.OrderByDescending(o => o.Score).ToList();
                    for (int i = 0; i < numberOfLinks; i++)
                    {
                        Process.Start(bottom_right_posts_sorted[i].Url.ToString());
                    }
                }
            }
        }
    }
}
