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
    public partial class Form1 : Form
    {

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private int record_x, record_y;
        private PictureBox pic;//展示的画布
        private UserRect usr;//选取的矩形
        private Form2 frm;//临时测试
        private int zoomwidth = 160;//放大镜宽度
        private int zoomheight = 120;//放大镜高度
        private Bitmap screenImage;//背景图缓存
        private Form zoomwin;//放大镜窗口
        PictureBox pc;//小型画布
        private List<Form2> frmlist;
        private List<Form2> frmgroup;
        private int group = -1;

        private string path = "/cutimage";
        private string filepath = "/cutimage/分组0.txt";

        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }
        public Form1()
        {
            InitializeComponent();
            cutPicture(false);
            cutState = eCutStatus.None;
            //zoom窗口
            zoomwin = new Form();
            zoomwin.ClientSize = new Size(zoomwidth + 2, zoomheight + 2);
            zoomwin.FormBorderStyle = FormBorderStyle.None;
            zoomwin.StartPosition = FormStartPosition.Manual;
            zoomwin.TopMost = true;
            zoomwin.ShowInTaskbar = false;
            Bitmap bmp = new Bitmap(zoomwin.Width, zoomwin.Height);
            zoomwin.BackgroundImage = bmp;
            zoomwin.Location = new Point(MousePosition.X + 15, MousePosition.Y + 22);
            this.AddOwnedForm(zoomwin);
            pc = new PictureBox();
            pc.Location = new Point(1, 1);
            pc.Size = new Size(zoomwin.Width, zoomwin.Height);
            zoomwin.Controls.Add(pc);
            zoomwin.Hide();
            frmlist = new List<Form2>();
            frmgroup = new List<Form2>();
            this.Hide();
        }

        int hotkey = (int)(KeyModifier.Control);
        private void Form1_Load(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Hide();
            //注册F1热键，此时只有该热键监听全局事件，其他的不进行注册，防止与windows快捷键进行冲突
            RegisterHotKey(this.Handle, 10000, (int)KeyModifier.None, (int)Keys.F1);
            RegisterHotKey(this.Handle, 10006, (int)KeyModifier.None, (int)Keys.F3);

            if (!Directory.Exists(@"/cutimage"))
            {
                System.IO.Directory.CreateDirectory(@"/cutimage");//不存在就创建目录
            }
            if (!File.Exists(path + "/分组0.txt"))
                File.Create(path + "/分组0.txt");
            if (!File.Exists(path + "/分组1.txt"))
                File.Create(path + "/分组1.txt");
            if (!File.Exists(path + "/分组2.txt"))
                File.Create(path + "/分组2.txt");
            if (!File.Exists(path + "/分组3.txt"))
                File.Create(path + "/分组3.txt");

            ReadFile(filepath, 0);
        }

        private void ValidHotKey()
        {
            RegisterHotKey(this.Handle, 10001, hotkey, (int)Keys.T);
            RegisterHotKey(this.Handle, 10002, hotkey, (int)Keys.C);
            RegisterHotKey(this.Handle, 10003, hotkey, (int)Keys.S);
            RegisterHotKey(this.Handle, 10004, hotkey, (int)Keys.H);
            RegisterHotKey(this.Handle, 10005, (int)KeyModifier.Shift, (int)Keys.S);
            RegisterHotKey(this.Handle, 10007, (int)KeyModifier.None, (int)Keys.C);
        }

        private void InValidHotKey(bool exit)
        {
            if (exit)
            {
                UnregisterHotKey(this.Handle, 10000);
                UnregisterHotKey(this.Handle, 10001);
                UnregisterHotKey(this.Handle, 10006);
            }
            UnregisterHotKey(this.Handle, 10002);
            UnregisterHotKey(this.Handle, 10003);
            UnregisterHotKey(this.Handle, 10004);
            UnregisterHotKey(this.Handle, 10005);
            UnregisterHotKey(this.Handle, 10007);
        }

        private void Delfrm(Form2 form)
        {
            frmlist.Remove(form);
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            InValidHotKey(true);
        }

        protected override void WndProc(ref Message m)
        {

            const int WM_HOTKEY = 0x0312;
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    if (m.WParam.ToString() == "10000")
                    {
                        cutPicture(true);
                    }
                    else if (m.WParam.ToString() == "10001")
                    {
                        if (cutState == eCutStatus.End)
                            PasteImg(usr.GetnowRect());
                    }
                    else if (m.WParam.ToString() == "10002")
                    {
                        if (cutState == eCutStatus.End)
                            CopyClip();
                    }
                    else if (m.WParam.ToString() == "10003")
                    {
                        if (cutState == eCutStatus.End)
                            SaveImage();
                    }
                    else if (m.WParam.ToString() == "10004")
                    {
                        foreach (var form in frmlist)
                        {
                            if (form.Visible)
                                form.Hide();
                            else
                                form.Show();
                        }
                    }
                    else if (m.WParam.ToString() == "10005")
                    {
                        Save();
                    }
                    else if (m.WParam.ToString() == "10006")
                    {
                        group = (group + 1) % 3;
                        switch (group)
                        {
                            case 0:
                                FrmClear();
                                ReadFile(path + "/分组1.txt", 1);
                                break;
                            case 1:
                                FrmClear();
                                ReadFile(path + "/分组2.txt", 2);
                                break;
                            case 2:
                                FrmClear();
                                ReadFile(path + "/分组3.txt", 3);
                                break;
                        }

                    }
                    else if (m.WParam.ToString() == "10007")
                    {
                        if (cutState == eCutStatus.End && usr.GetnowRect().Contains(MousePosition.X, MousePosition.Y))
                            GetPix(MousePosition.X, MousePosition.Y);
                    }
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);
        }

        private void cutPicture(bool show)
        {
            if (cutState == eCutStatus.None)
            {
                ValidHotKey();
                cutState = eCutStatus.Ready;
                Bitmap catchScreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                Graphics g = Graphics.FromImage(catchScreen);
                g.CopyFromScreen(new Point(0, 0), new Point(0, 0), Screen.PrimaryScreen.Bounds.Size);
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
                this.BackgroundImage = catchScreen;
                this.screenImage = catchScreen;
                this.TopMost = true;
                if (show)
                    this.Show();
            }

        }

        enum eCutStatus
        {
            None, Ready, Begin, End
        }

        eCutStatus cutState = eCutStatus.None;
        Rectangle cutRect = new Rectangle();
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (cutState != eCutStatus.Ready) return;
            if (e.Button == MouseButtons.Left)
            {
                record_x = e.X;
                record_y = e.Y;
                cutRect.X = e.X;
                cutRect.Y = e.Y;
                cutRect.Width = 0;
                cutRect.Height = 0;
                cutState = eCutStatus.Begin;
            }
        }

        private void PasteImg(Rectangle temprec)
        {
            /*            if (Cursor.Position.X > temprec.Left && Cursor.Position.X < temprec.Right && Cursor.Position.Y > temprec.Top && Cursor.Position.Y < temprec.Bottom)
                        {
                            Bitmap cutPic = new Bitmap(temprec.Width, temprec.Height);
                            Graphics g = Graphics.FromImage(cutPic);
                            g.DrawImage(this.BackgroundImage, new Rectangle(0, 0, temprec.Width, temprec.Height), temprec, GraphicsUnit.Pixel);
                            frm = new Form2(cutPic, temprec,0);
                            frm.UpdateTextBox += Delfrm; //添加事件队列 (观察者模式)
                            frmlist.Add(frm);
                            g.Dispose();
                            cutPic.Dispose();
                        }*/

            Bitmap cutPic = new Bitmap(temprec.Width, temprec.Height);
            Graphics g = Graphics.FromImage(cutPic);
            g.DrawImage(this.BackgroundImage, new Rectangle(0, 0, temprec.Width, temprec.Height), temprec, GraphicsUnit.Pixel);
            frm = new Form2(cutPic, temprec, 0);
            frm.UpdateTextBox += Delfrm; //添加事件队列 (观察者模式)
            frmlist.Add(frm);
            g.Dispose();
            cutPic.Dispose();
            EndCut();
        }
        private void SaveImage()
        {
            if (this.Visible && this.usr.GetnowRect().Size != Size.Empty)
            {
                Rectangle temprec = usr.GetnowRect();
                Bitmap cutPic = new Bitmap(temprec.Width, temprec.Height);
                Graphics g = Graphics.FromImage(cutPic);
                g.DrawImage(this.BackgroundImage, new Rectangle(0, 0, temprec.Width, temprec.Height), temprec, GraphicsUnit.Pixel);
                Image image = (Image)cutPic.Clone();
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Filter = "Jpg 图片|*.jpg|Bmp 图片|*.bmp|Gif 图片|*.gif|Png 图片|*.png|Wmf  图片|*.wmf";
                savedialog.FilterIndex = 0;
                savedialog.RestoreDirectory = true;//保存对话框是否记忆上次打开的目录
                savedialog.CheckPathExists = true;//检查目录
                savedialog.FileName = System.DateTime.Now.ToString("yyyyMMddHHmmss") + "-"; ;//设置默认文件名
                if (savedialog.ShowDialog() == DialogResult.OK)
                {
                    image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);// image为要保存的图片
                }
                g.Dispose();
                cutPic.Dispose();
                EndCut();
            }
        }

        private void GetPix(int x, int y)
        {
            Color c = screenImage.GetPixel(x, y);
            Form3 frm3 = new Form3(c.R, c.G, c.B);
            EndCut();
        }

        private void EndCut()
        {
            //截图结束处理函数
            InValidHotKey(false);
            usr.Reset();
            pic.Hide();
            cutState = eCutStatus.None;
            cutRect.X = 0;
            cutRect.Y = 0;
            cutRect.Width = 0;
            cutRect.Height = 0;
            record_x = 0;
            record_y = 0;
            zoomwin.Hide();
            this.Hide();

        }

        private void CopyClip()
        {//复制到剪切版
            if (this.Visible && this.usr.GetnowRect().Size != Size.Empty)
            {
                Rectangle temprec = usr.GetnowRect();
                Bitmap cutPic = new Bitmap(temprec.Width, temprec.Height);
                Graphics g = Graphics.FromImage(cutPic);
                g.DrawImage(this.BackgroundImage, new Rectangle(0, 0, temprec.Width, temprec.Height), temprec, GraphicsUnit.Pixel);
                Clipboard.SetImage(cutPic);
                g.Dispose();
                cutPic.Dispose();
                EndCut();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (cutState != eCutStatus.Begin) return;
            if (e.Button == MouseButtons.Left)
            {
                cutRect.Width = Math.Abs(record_x - cutRect.X);
                cutRect.Height = Math.Abs(record_y - cutRect.Y);
                int temp;
                if (record_x < cutRect.X)
                {
                    temp = cutRect.X;
                    cutRect.X = record_x;
                    record_x = temp;
                }
                if (record_y < cutRect.Y)
                {
                    temp = cutRect.Y;
                    cutRect.Y = record_y;
                    record_y = temp;
                }
                cutState = eCutStatus.End;

                pic = new PictureBox();
                pic.SetBounds(0, 0, 1920, 1080);
                Bitmap tempbit = (Bitmap)this.BackgroundImage.Clone();
                using (Graphics g = Graphics.FromImage(tempbit))
                {
                    using (SolidBrush sb = new SolidBrush(Color.FromArgb(155, 0, 0, 0)))
                    {
                        g.FillRectangle(sb, 0, 0, pic.Width, pic.Height);
                    }
                }
                pic.BackgroundImage = tempbit;
                usr = new UserRect(new Rectangle(cutRect.Location, cutRect.Size));
                usr.SetPictureBox(pic);
                Bitmap cutPic = new Bitmap(cutRect.Width, cutRect.Height);
                pic.MouseClick += new MouseEventHandler(this.pic_MouseClick);
                pic.MouseDoubleClick += new MouseEventHandler(this.pic_OnMouseDoubleClick);
                pic.MouseMove += new MouseEventHandler(this.pic_OnMouseMove);
                usr.SetBitmap((Bitmap)this.BackgroundImage);
                this.Controls.Add(pic);

            }
        }

        private void pic_MouseClick(object sender, MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Right)
            {
                EndCut();
            }
        }

        private void pic_OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            Rectangle temprec = usr.GetnowRect();
            PasteImg(temprec);

        }

        private void pic_OnMouseMove(object sender, MouseEventArgs e)
        {
            zoomwin.Location = new Point(MousePosition.X + 15, MousePosition.Y + 22);
            Graphics g = pc.CreateGraphics();
            Rectangle rec = usr.GetnowRect();
            if (rec.Contains(new Point(MousePosition.X, MousePosition.Y)))
            {
                zoomwin.Show();
                Point zoomLocation = new Point(MousePosition.X + 15, MousePosition.Y + 22);
                if (zoomLocation.Y + this.zoomheight > this.Height)
                {
                    if (zoomLocation.X + this.zoomwidth > this.Width)
                    {
                        zoomLocation = new Point(MousePosition.X - this.zoomwidth - 10, MousePosition.Y - this.zoomheight - 10);
                    }
                    else
                    {
                        zoomLocation = new Point(MousePosition.X + 15, MousePosition.Y - this.zoomheight - 15);
                    }
                }
                else
                {
                    if (zoomLocation.X + this.zoomwidth > this.Width)
                    {
                        zoomLocation = new Point(MousePosition.X - this.zoomwidth - 15, MousePosition.Y);
                    }
                }
                //绘制放大镜中的图像
                Size srcSize = new System.Drawing.Size((int)(this.zoomwidth / 10), (int)(this.zoomheight / 10));
                Point srcPoint;
                if (MousePosition.X >= 5 && MousePosition.Y >= 4 && MousePosition.X + srcSize.Width <= this.Width && MousePosition.Y + srcSize.Height <= this.Height)
                {//超出范围的暂时不做处理
                    srcPoint = new Point(MousePosition.X - 5, MousePosition.Y - 4);
                    Rectangle zoomRect = new Rectangle(0, 0, 10, 10);
                    Brush zoomBrush;
                    for (int row = 0; row < srcSize.Height; row++)
                    {
                        for (int col = 0; col < srcSize.Width; col++)
                        {
                            Color penc = screenImage.GetPixel(srcPoint.X + col, srcPoint.Y + row);
                            zoomBrush = new SolidBrush(penc);
                            g.FillRectangle(zoomBrush, zoomRect);
                            zoomRect.Offset(10, 0);
                        }
                        zoomRect.X = 0;
                        zoomRect.Offset(0, 10);
                    }

                    SolidBrush brush = new SolidBrush(Color.FromArgb(155, 220, 20, 60));
                    Pen pen = new Pen(brush, 2.0F);
                    g.DrawLine(pen, new Point(0, zoomheight / 2 + 1), new Point(zoomwidth + 2, zoomheight / 2 + 1)); //加5解决放大镜准星和实际鼠标位置对不上的问题；
                    g.DrawLine(pen, new Point(zoomwidth / 2 + 1, 0), new Point(zoomwidth / 2 + 1, zoomheight + 2));
                    g.DrawRectangle(new Pen(new SolidBrush(Color.White)), new Rectangle(0, 0, zoomwidth + 2, zoomheight + 2));
                    brush.Dispose();
                    pen.Dispose();
                    /* zoomwin.Show();*/
                }
                else
                {
                    zoomwin.Hide();
                }
            }
            else
            {
                zoomwin.Hide();
            }
            g.Dispose();
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (e.X > cutRect.Left && e.X < cutRect.Right && e.Y > cutRect.Top && e.Y < cutRect.Bottom)
            {
                Bitmap cutPic = new Bitmap(cutRect.Width, cutRect.Height);
                Graphics g = Graphics.FromImage(cutPic);
                g.DrawImage(this.BackgroundImage, new Rectangle(0, 0, cutRect.Width, cutRect.Height), cutRect, GraphicsUnit.Pixel);
                Clipboard.SetImage(cutPic);
                g.Dispose();
                cutPic.Dispose();

                pic.Dispose();
                cutState = eCutStatus.None;
                cutRect.X = 0;
                cutRect.Y = 0;
                cutRect.Width = 0;
                cutRect.Height = 0;
                record_x = 0;
                record_y = 0;
                this.Hide();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Right)
            {
                this.Hide();
                cutState = eCutStatus.None;
                cutRect.X = 0;
                cutRect.Y = 0;
                cutRect.Width = 0;
                cutRect.Height = 0;
                record_x = 0;
                record_y = 0;

            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (cutState == eCutStatus.Begin)
            {
                record_x = e.X;
                record_y = e.Y;
                Invalidate();
            }

        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            Rectangle temprec = new Rectangle();
            temprec.X = (record_x > cutRect.X) ? cutRect.X : record_x;
            temprec.Y = (record_y > cutRect.Y) ? cutRect.Y : record_y;
            temprec.Width = Math.Abs(record_x - cutRect.X);
            temprec.Height = Math.Abs(record_y - cutRect.Y);
            if (cutState != eCutStatus.None)
            {
                Color backcolor = Color.FromArgb(155, Color.Black);
                List<Rectangle> rects = new List<Rectangle>();
                rects.Add(new Rectangle(0, 0, Screen.PrimaryScreen.Bounds.Width, temprec.Y));
                rects.Add(new Rectangle(0, temprec.Y, temprec.X, temprec.Height));
                rects.Add(new Rectangle(temprec.Right, temprec.Top, Screen.PrimaryScreen.Bounds.Width - temprec.Right, temprec.Height));
                rects.Add(new Rectangle(0, temprec.Bottom, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height - temprec.Bottom));
                g.FillRectangles(new SolidBrush(backcolor), rects.ToArray());
            }
            if (cutState == eCutStatus.Begin || cutState == eCutStatus.End)
            {
                g.DrawRectangle(Pens.Red, temprec);

                Point zoomLocation = new Point(MousePosition.X + 15, MousePosition.Y + 22);
                if (zoomLocation.Y + this.zoomheight > this.Height)
                {
                    if (zoomLocation.X + this.zoomwidth > this.Width)
                    {
                        zoomLocation = new Point(MousePosition.X - this.zoomwidth - 10, MousePosition.Y - this.zoomheight - 10);
                    }
                    else
                    {
                        zoomLocation = new Point(MousePosition.X + 15, MousePosition.Y - this.zoomheight - 15);
                    }
                }
                else
                {
                    if (zoomLocation.X + this.zoomwidth > this.Width)
                    {
                        zoomLocation = new Point(MousePosition.X - this.zoomwidth - 15, MousePosition.Y);
                    }
                }
                //绘制放大镜中的图像
                Size srcSize = new System.Drawing.Size((int)(this.zoomwidth / 10), (int)(this.zoomheight / 10));
                Point srcPoint;
                if (MousePosition.X >= 5 && MousePosition.Y >= 4 && MousePosition.X + srcSize.Width <= this.Width && MousePosition.Y + srcSize.Height <= this.Height)
                {//超出范围的暂时不做处理
                    srcPoint = new Point(MousePosition.X - 5, MousePosition.Y - 4);
                    Rectangle zoomRect = new Rectangle(0, 0, 10, 10);
                    Brush zoomBrush;
                    zoomRect.Location = zoomLocation;
                    for (int row = 0; row < srcSize.Height; row++)
                    {
                        for (int col = 0; col < srcSize.Width; col++)
                        {
                            Color pc = screenImage.GetPixel(srcPoint.X + col, srcPoint.Y + row);
                            zoomBrush = new SolidBrush(pc);
                            e.Graphics.FillRectangle(zoomBrush, zoomRect);
                            //g1.FillRectangle(zoomBrush, zoomRect);

                            zoomRect.Offset(10, 0);
                        }
                        zoomRect.Offset(0, 10);
                        //zoomRect.X = 0;
                        zoomRect.X = zoomLocation.X;
                        /*zoomRect.Y = zoomLocation.Y;*/
                    }

                    SolidBrush brush = new SolidBrush(Color.FromArgb(155, 220, 20, 60));
                    Pen pen = new Pen(brush, 2.0F);
                    e.Graphics.DrawLine(pen, new Point(zoomLocation.X, zoomLocation.Y + zoomheight / 2), new Point(zoomLocation.X + zoomwidth, zoomLocation.Y + zoomheight / 2)); //加5解决放大镜准星和实际鼠标位置对不上的问题；
                    e.Graphics.DrawLine(pen, new Point(zoomLocation.X + zoomwidth / 2, zoomLocation.Y), new Point(zoomLocation.X + zoomwidth / 2, zoomLocation.Y + zoomheight));
                    e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.White)), new Rectangle(zoomLocation.X - 1, zoomLocation.Y - 1, zoomwidth + 2, zoomheight + 2));
                    brush.Dispose();
                    pen.Dispose();
                }

            }
        }

        private void FrmClear()
        {
            for (int i = 0; i < frmlist.Count;)
            {
                if (frmlist[i].GetGroup() != 0)
                {
                    frmlist[i].Close();
                    i = 0;
                }
                else
                    i++;

            }
            frmgroup.Clear();
        }

        private void ReadFile(string file, int gr)
        {
            if (File.Exists(file))
            {
                StreamReader read = new StreamReader(file);
                while (!read.EndOfStream)
                {
                    string str = read.ReadLine();
                    if (str.Length > 1)
                    {
                        int nid = str.IndexOf('{');
                        string filename = str.Substring(0, nid);
                        if (File.Exists(filename))
                        {
                            string a = str.Substring(nid + 3);
                            nid = a.IndexOf(',');
                            int x = int.Parse(a.Substring(0, nid));
                            string b = a.Substring(nid + 3);
                            nid = b.IndexOf('}');
                            int y = int.Parse(b.Substring(0, nid));
                            string c = b.Substring(nid + 8);
                            nid = c.IndexOf(',');
                            int width = int.Parse(c.Substring(0, nid));
                            string d = c.Substring(nid + 9);
                            nid = d.IndexOf('}');
                            int height = int.Parse(d.Substring(0, nid));
                            Bitmap bitmap = new Bitmap(filename);
                            Rectangle rec = new Rectangle(x, y, width, height);
                            frm = new Form2(bitmap, rec, gr, str);
                            frm.UpdateTextBox += Delfrm; //添加事件队列 (观察者模式)
                            frmlist.Add(frm);
                            frmgroup.Add(frm);
                        }
                    }
                }
                read.Close();
                read.Dispose();
            }
            else
                File.Create(file);
        }

        private void Save()
        {
            StreamWriter sw = new StreamWriter(@"/cutimage/分组0.txt", false);
            foreach (var form in frmlist)
            {
                string nowtime = DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_"
                               + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + "_" + DateTime.Now.Millisecond.ToString();
                string pathname = path + "/截图" + nowtime + ".png";
                Bitmap bitmap = (Bitmap)form.GetBitmap().Clone();
                bitmap.Save(pathname, System.Drawing.Imaging.ImageFormat.Png);
                sw.WriteLine(pathname + form.Location + form.Size);
            }
            sw.Close();
            sw.Dispose();
        }



        #region 托盘

        private void GoClip(object sender, EventArgs e)
        {
            cutPicture(true);
        }

        private void AppExit(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        #endregion
    }
}
