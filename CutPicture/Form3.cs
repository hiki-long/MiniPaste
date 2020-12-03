using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CutPicture
{
    public partial class Form3 : Form
    {
        private PictureBox pic;
        private Point mouseOff;
        private bool leftFlag;
        public Form3()
        {
            InitializeComponent();
            this.Show();
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            pic = new PictureBox();
            pic.Size = new Size(this.Width, 50);
            Bitmap mp = new Bitmap(pic.Width, pic.Height);
            Graphics g = Graphics.FromImage(mp);
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, this.Width, 50));
            this.Controls.Add(pic);
            pic.Image = mp;
        }

        public Form3(int a, int b, int c)
        {
            InitializeComponent();
            this.Show();
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            pic = new PictureBox();
            this.MouseDown += new MouseEventHandler(this.picbox_MouseDown);
            this.MouseMove += new MouseEventHandler(this.picbox_MouseMove);
            this.MouseUp += new MouseEventHandler(this.picbox_MouseUp);
            pic.MouseDown += new MouseEventHandler(this.picbox_MouseDown);
            pic.MouseMove += new MouseEventHandler(this.picbox_MouseMove);
            pic.MouseUp += new MouseEventHandler(this.picbox_MouseUp);
            pic.Size = new Size(this.Width, 50);
            Bitmap mp = new Bitmap(pic.Width, pic.Height);
            Graphics g = Graphics.FromImage(mp);
            g.FillRectangle(new SolidBrush(Color.FromArgb(255,a,b,c)),new Rectangle(0,0, this.Width, 50));
            g.DrawRectangle(new Pen(new SolidBrush(Color.White)), new Rectangle(0, 0, this.Width-1, 50-1));
            this.Controls.Add(pic);
            pic.Image = mp;
            label6.Text = Convert.ToString(a);
            label7.Text = Convert.ToString(b);
            label8.Text = Convert.ToString(c);
            double d = Math.Round(((double)a / 255), 2);
            double e = Math.Round(((double) b / 255), 2);
            double f = Math.Round(((double)b / 255), 2);
            label10.Text = Convert.ToString(d);
            label11.Text = Convert.ToString(e);
            label12.Text = Convert.ToString(f);
            g.Dispose();

        }

        private void Form3_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                this.Close();
            }
        }

        private void picbox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                leftFlag = true;                  //点击左键按下时标注为true;
            }
        }

        private void picbox_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }

        private void picbox_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }
    }
}
