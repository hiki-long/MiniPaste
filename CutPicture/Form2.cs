using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CutPicture
{
    public partial class Form2 : Form
    {
        private Point mouseOff;//鼠标移动位置变量
        private bool leftFlag;//标记是否为左键
        private Bitmap back;//窗口背景
        private PictureBox background;//绘制背景
        private Rectangle edge;//边界
        private int group = 0;
        private string record;
        private double now_opacity = 1.0;//当前窗口透明度，在0~1之间
        private double now_size = 1.0;//当前窗口缩放比例
        private int origin_width;//初始窗口宽度
        private int origin_height;//初始窗口高度

        public delegate void UpdateDataDelegate(Form2 from);//声明一个委托

        public UpdateDataDelegate UpdateTextBox; //定义委托

        public Form2()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
        }
        public Form2(Bitmap img, Rectangle rec, int gr)
        {
            InitializeComponent();
            group = gr;
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.origin_height = rec.Height;
            this.origin_width = rec.Width;

            edge = new Rectangle(rec.Location, rec.Size);
            this.FormBorderStyle = FormBorderStyle.None;
            this.KeyPreview = true;
            this.MouseWheel += new MouseEventHandler(Form2_Mousewheel);


            Graphics g = this.CreateGraphics();
            Pen p = new Pen(Color.White, 1);      //设置画笔颜色和宽度
            g.DrawRectangle(p, edge);
            g.Dispose();
            p.Dispose();
            this.back = (Bitmap)img.Clone();
            this.background = new PictureBox();
            this.background.Location = new Point(2, 2);
            this.background.MouseDown += new MouseEventHandler(picbox_MouseDown);
            this.background.MouseUp += new MouseEventHandler(picbox_MouseUp);
            this.background.MouseMove += new MouseEventHandler(picbox_MouseMove);
            this.background.MouseClick += new MouseEventHandler(picbox_MouseClick);
            background.ClientSize = new Size(rec.Width, rec.Height);
            background.Image = (Bitmap)this.back.Clone();

            this.ClientSize = new Size(rec.Width + 4, rec.Height + 4);
            this.Controls.Add(background);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = (Point)new Size(rec.X - 2, rec.Y - 2);

            this.Show();
        }

        public Form2(Bitmap img, Rectangle rec, int gr, string str)
        {
            InitializeComponent();
            group = gr;
            record = str;
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.origin_height = rec.Height;
            this.origin_width = rec.Width;

            edge = new Rectangle(rec.Location, rec.Size);
            this.FormBorderStyle = FormBorderStyle.None;
            this.KeyPreview = true;
            this.MouseWheel += new MouseEventHandler(Form2_Mousewheel);


            Graphics g = this.CreateGraphics();
            Pen p = new Pen(Color.White, 1);      //设置画笔颜色和宽度
            g.DrawRectangle(p, edge);
            g.Dispose();
            p.Dispose();
            this.back = (Bitmap)img.Clone();
            this.background = new PictureBox();
            this.background.Location = new Point(2, 2);
            this.background.MouseDown += new MouseEventHandler(picbox_MouseDown);
            this.background.MouseUp += new MouseEventHandler(picbox_MouseUp);
            this.background.MouseMove += new MouseEventHandler(picbox_MouseMove);
            this.background.MouseClick += new MouseEventHandler(picbox_MouseClick);
            background.ClientSize = new Size(rec.Width, rec.Height);
            background.Image = (Bitmap)this.back.Clone();

            this.ClientSize = new Size(rec.Width + 4, rec.Height + 4);
            this.Controls.Add(background);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = (Point)new Size(rec.X - 2, rec.Y - 2);

            this.Show();
        }

        private void Spin(bool time_order)
        {
            Bitmap bmp = (Bitmap)this.background.Image.Clone();//加载图像
            if (time_order)//顺时针旋转
                bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            else
                bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);//逆时针旋转
            this.background.Image = bmp;
            this.background.Width = this.background.Image.Width;
            this.background.Height = this.background.Image.Height;
            this.Size = new Size(this.background.Width + 4, this.background.Height + 4);
        }

        private void Mirror(bool horizon)
        {
            Bitmap bmp = (Bitmap)this.background.Image.Clone();//加载图像
            if (horizon)//顺时针旋转
                bmp.RotateFlip(RotateFlipType.Rotate180FlipY);
            else
                bmp.RotateFlip(RotateFlipType.Rotate180FlipX);//逆时针旋转
            this.background.Image = bmp;
            this.background.Width = this.background.Image.Width;
            this.background.Height = this.background.Image.Height;
            this.Size = new Size(this.background.Width + 4, this.background.Height + 4);
        }
        private void Restore()
        {
            now_opacity = 1.0;
            now_size = 1.0;
            //恢复原本窗口状态
            int iNewWidth = this.origin_width;
            int iNewHeight = this.origin_height;

            //定义新的Bitmap；
            Bitmap BitNewImg = new Bitmap(iNewWidth, iNewHeight);
            //定义对BitNewImg的绘制方法；
            Graphics graph = Graphics.FromImage(BitNewImg);
            graph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            //用原始图像绘制新图像；
            graph.DrawImage(this.back, new Rectangle(0, 0, origin_width, origin_height),
                new Rectangle(0, 0, origin_width, origin_height), GraphicsUnit.Pixel);
            graph.Dispose();
            this.background.Image = BitNewImg;
            this.background.Width = this.background.Image.Width;
            this.background.Height = this.background.Image.Height;
            //缩放后的滚轮的位置。
            this.Size = new Size(this.background.Width + 4, this.background.Height + 4);
            this.Opacity = this.now_opacity;
            this.Refresh();

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

        private void picbox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.Close();
                this.Dispose();
            }
            else if (e.Button == MouseButtons.Middle)
            {
                /*SaveImage();*/
                Restore();
            }

        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                leftFlag = true;                  //点击左键按下时标注为true;
            }
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }

        private void SaveImage()
        {
            //保存图片
            Image image = (Image)back.Clone();
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Filter = "Jpg 图片|*.jpg|Bmp 图片|*.bmp|Gif 图片|*.gif|Png 图片|*.png|Wmf  图片|*.wmf";
            savedialog.FilterIndex = 0;
            savedialog.RestoreDirectory = true;//保存对话框是否记忆上次打开的目录
            savedialog.CheckPathExists = true;//检查目录
            savedialog.FileName = System.DateTime.Now.ToString("yyyyMMddHHmmss") + "-"; ;//设置默认文件名
            if (savedialog.ShowDialog() == DialogResult.OK)
            {
                image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);// image为要保存的图片
                //MessageBox.Show(this, "图片保存成功！", "信息提示");
            }
        }

        private void Form2_Mousewheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                if (e.Delta > 0)
                {
                    /*MessageBox.Show("滚轮向前滚动");*/
                    if (this.now_opacity > 0.2)
                    {
                        this.now_opacity -= 0.1;
                        this.Opacity = this.now_opacity;
                        this.Refresh();
                    }
                    else
                    {
                        this.now_opacity = 0.1;
                    }
                }
                else
                {
                    /*MessageBox.Show("滚轮向后滚动");*/
                    if (this.now_opacity < 1.0)
                    {
                        this.now_opacity += 0.1;
                        this.Opacity = this.now_opacity;
                        this.Refresh();
                    }
                    else
                    {
                        this.now_opacity = 1.0;
                    }
                }
            }
            else
            {
                if (e.Delta > 0)
                {
                    if (now_size < 3.0)
                    {
                        now_size = (now_size + 0.1 < 3.0) ? (now_size + 0.1) : 3.0;
                    }
                    else
                    {
                        now_size = 3.0;
                    }
                    //缩放后的picture box的大小。
                    int iNewWidth = (int)(now_size * this.origin_width);
                    int iNewHeight = (int)(now_size * this.origin_height);

                    //定义新的Bitmap；
                    Bitmap BitNewImg = new Bitmap(iNewWidth, iNewHeight);
                    //定义对BitNewImg的绘制方法；
                    Graphics graph = Graphics.FromImage(BitNewImg);
                    graph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                    //用原始图像绘制新图像；
                    graph.DrawImage(this.back, new Rectangle(0, 0, iNewWidth, iNewHeight),
                        new Rectangle(0, 0, origin_width, origin_height), GraphicsUnit.Pixel);
                    //缩放后的中心点距离picture box左上角的距离。
                    graph.Dispose();
                    this.background.Image = BitNewImg;
                    this.background.Width = this.background.Image.Width;
                    this.background.Height = this.background.Image.Height;
                    //缩放后的滚轮的位置。
                    this.Size = new Size(this.background.Width + 4, this.background.Height + 4);


                }
                else
                {
                    if (now_size > 0.3)
                    {
                        now_size = (now_size - 0.1 > 0.3) ? (now_size - 0.1) : 0.3;
                    }
                    else
                    {
                        now_size = 0.3;
                    }
                    int iNewWidth = (int)(now_size * this.origin_width);
                    int iNewHeight = (int)(now_size * this.origin_height);

                    Bitmap BitNewImg = new Bitmap(iNewWidth, iNewHeight);
                    //定义对BitNewImg的绘制方法；
                    Graphics graph = Graphics.FromImage(BitNewImg);
                    graph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                    //用原始图像绘制新图像；
                    graph.DrawImage(this.back, new Rectangle(0, 0, iNewWidth, iNewHeight),
                        new Rectangle(0, 0, origin_width, origin_height), GraphicsUnit.Pixel);
                    graph.Dispose();
                    this.background.Image = BitNewImg;
                    this.background.Width = this.background.Image.Width;
                    this.background.Height = this.background.Image.Height;
                    this.Size = new Size(this.background.Width + 4, this.background.Height + 4);

                }
            }

        }

        private void ChangeOpacity(double opacity)
        {
            this.now_opacity = opacity;
            this.Opacity = this.now_opacity;
            this.Refresh();
        }

        private void ChangeSize(double size)
        {
            this.now_size = size;
            int iNewWidth = (int)(now_size * this.origin_width);
            int iNewHeight = (int)(now_size * this.origin_height);

            //定义新的Bitmap；
            Bitmap BitNewImg = new Bitmap(iNewWidth, iNewHeight);
            //定义对BitNewImg的绘制方法；
            Graphics graph = Graphics.FromImage(BitNewImg);
            graph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            //用原始图像绘制新图像；
            graph.DrawImage(this.back, new Rectangle(0, 0, iNewWidth, iNewHeight),
                new Rectangle(0, 0, origin_width, origin_height), GraphicsUnit.Pixel);
            //缩放后的中心点距离picture box左上角的距离。
            graph.Dispose();
            this.background.Image = BitNewImg;
            this.background.Width = this.background.Image.Width;
            this.background.Height = this.background.Image.Height;
            //缩放后的滚轮的位置。
            this.Size = new Size(this.background.Width + 4, this.background.Height + 4);
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void CopyStripMenuItem1_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(back);
        }

        private void SaveStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveImage();
        }

        private void CloseStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void DestroyStripMenuItem1_Click(object sender, EventArgs e)
        {
            switch (group)
            {
                case 0:
                    Delrecord(0);
                    break;
                case 1:
                    Delrecord(1);
                    break;
                case 2:
                    Delrecord(2);
                    break;
                case 3:
                    Delrecord(3);
                    break;
            }
            this.Close();
            this.Dispose();
        }

        private void ScalingStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void S10StripMenuItem2_Click(object sender, EventArgs e)
        {
            ChangeSize(0.1);
        }

        private void S30StripMenuItem3_Click(object sender, EventArgs e)
        {
            ChangeSize(0.3);
        }


        private void S50StripMenuItem4_Click(object sender, EventArgs e)
        {
            ChangeSize(0.5);
        }

        private void S100StripMenuItem5_Click(object sender, EventArgs e)
        {
            ChangeSize(1.0);
        }

        private void S200StripMenuItem6_Click(object sender, EventArgs e)
        {
            ChangeSize(2.0);
        }

        private void D10StripMenuItem7_Click(object sender, EventArgs e)
        {
            ChangeOpacity(0.1);
        }

        private void D30StripMenuItem8_Click(object sender, EventArgs e)
        {
            ChangeOpacity(0.3);
        }

        private void D50StripMenuItem9_Click(object sender, EventArgs e)
        {
            ChangeOpacity(0.5);
        }

        private void D100tripMenuItem10_Click(object sender, EventArgs e)
        {
            ChangeOpacity(1.0);
        }

        public Bitmap GetBitmap()
        {
            return back;
        }

        public int GetGroup()
        {
            return group;
        }

        private void MyFormClosed(object sender, EventArgs e)
        {
            UpdateTextBox(this);

        }

        private void MoveStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (group != 1)
            {
                Firstgr(group, 1);
                group = 1;
            }
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (group != 2)
            {
                Firstgr(group, 2);
                group = 2;
            }
        }

        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (group != 3)
            {
                Firstgr(group, 3);
                group = 3;
            }
        }
        private void Firstgr(int src, int gr)
        {
            switch (src)
            {
                case 0:
                    StreamWriter sw = new StreamWriter(@"/cutimage/分组" + gr + ".txt", true);
                    string nowtime = DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_"
                                   + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + "_" + DateTime.Now.Millisecond.ToString();
                    string pathname = "/cutimage/截图" + nowtime + ".png";
                    Bitmap bitmap = (Bitmap)back.Clone();
                    bitmap.Save(pathname, System.Drawing.Imaging.ImageFormat.Png);
                    record = pathname + this.Location + this.Size;
                    sw.WriteLine(record);
                    sw.Close();
                    sw.Dispose();
                    break;
                default:
                    Delrecord(src);
                    MoveGroup(gr);
                    break;
            }
        }

        private void Delrecord(int src)
        {
            StreamReader read = new StreamReader("/cutimage/分组" + src + ".txt");
            List<string> strlines = new List<string>();
            while (!read.EndOfStream)
            {
                string str = read.ReadLine();
                if (str.Equals(record))
                    continue;
                else
                    strlines.Add(str);

            }
            read.Close();
            read.Dispose();
            StreamWriter sw = new StreamWriter(@"/cutimage/分组" + src + ".txt", false);
            sw.WriteLine("");
            foreach (var str in strlines)
            {
                sw.WriteLine(str);
            }
            sw.Close();
            sw.Dispose();
        }
        private void MoveGroup(int gr)
        {
            StreamWriter sw1 = new StreamWriter(@"/cutimage/分组" + gr + ".txt", true);
            record = record.Substring(0, record.IndexOf('{')) + this.Location + this.Size;
            Console.WriteLine(record);
            sw1.WriteLine(record);
            sw1.Close();
            sw1.Dispose();
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D1)
            {
                Spin(true);
            }
            else if (e.KeyCode == Keys.D2)
            {
                Spin(false);
            }
            else if (e.KeyCode == Keys.D3)
            {
                Mirror(true);
            }
            else if (e.KeyCode == Keys.D4)
            {
                Mirror(false);
            }
            else if (e.Control && e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }

        }

        private int ReadGroupNum(int i)
        {
            int n = 0;
            StreamReader read = new StreamReader("/cutimage/分组" + i + ".txt");
            List<string> strlines = new List<string>();
            while (!read.EndOfStream)
            {
                string str = read.ReadLine();
                if (str.Length > 1)
                    n++;
            }
            read.Close();
            read.Dispose();
            return n;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            this.ToolStripMenuItem1.Text = "分组1" + "      " + ReadGroupNum(1);
            this.ToolStripMenuItem2.Text = "分组2" + "      " + ReadGroupNum(2);
            this.ToolStripMenuItem3.Text = "分组3" + "      " + ReadGroupNum(3);

            switch (group)
            {
                case 0:
                    this.ToolStripMenuItem1.Checked = false;
                    this.ToolStripMenuItem2.Checked = false;
                    this.ToolStripMenuItem3.Checked = false;
                    break;
                case 1:
                    this.ToolStripMenuItem1.Checked = true;
                    this.ToolStripMenuItem2.Checked = false;
                    this.ToolStripMenuItem3.Checked = false;
                    break;
                case 2:
                    this.ToolStripMenuItem1.Checked = false;
                    this.ToolStripMenuItem2.Checked = true;
                    this.ToolStripMenuItem3.Checked = false;
                    break;
                case 3:
                    this.ToolStripMenuItem1.Checked = false;
                    this.ToolStripMenuItem2.Checked = false;
                    this.ToolStripMenuItem3.Checked = true;
                    break;
            }
        }
    }
}