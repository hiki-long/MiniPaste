using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CutPicture
{
    public class UserRect
    {
        private PictureBox mPictureBox;
        public Rectangle rect;
        public bool allowDeformingDuringMovement = false;
        private bool mIsClick = false;
        private bool hityedge = false;
        private bool hitxedge = false;
        private bool mMove = false;
        private int oldX;
        private int oldY;
        private int sizeNodeRect = 5;
        private Bitmap mBmp = null;
        private PosSizableRect nodeSelected = PosSizableRect.None;
        private int angle = 30;

        private enum PosSizableRect
        {
            UpMiddle,
            LeftMiddle,
            LeftBottom,
            LeftUp,
            RightUp,
            RightMiddle,
            RightBottom,
            BottomMiddle,
            None

        };

        public UserRect(Rectangle r)
        {
            rect = r;
            mIsClick = false;
        }

        public Rectangle GetnowRect()
        {
          /*  Console.WriteLine(rect.X);
            Console.WriteLine(rect.Y);*/
            return this.rect;
        }

        public void Reset()
        {
            this.rect.X = 0;
            this.rect.Y = 0;
            this.rect.Width = 0;
            this.rect.Height = 0;
        }

        public void Draw(Graphics g)
        {//这里是绘制具体形状的矩形
            if(this.rect.Size != Size.Empty)
            {
                Rectangle temprec = new Rectangle(rect.X - 1, rect.Y - 1, rect.Width + 1, rect.Height + 1);
                /*            g.DrawRectangle(new Pen(Color.Red), rect);*/
                g.DrawRectangle(new Pen(Color.Red), temprec);

                foreach (PosSizableRect pos in Enum.GetValues(typeof(PosSizableRect)))
                {
                    g.DrawRectangle(new Pen(Color.Red), GetRect(pos));
                }

                if (this.mBmp != null)
                {
                    g.DrawImage(mBmp, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height), rect, GraphicsUnit.Pixel);
                }
            }
            

        }

        public void SetBitmapFile(string filename)
        {
            this.mBmp = new Bitmap(filename);
        }

        public void SetBitmap(Bitmap bmp)
        {
            this.mBmp = bmp;
        }

        public void SetPictureBox(PictureBox p)
        {
            this.mPictureBox = p;
            mPictureBox.MouseDown += new MouseEventHandler(mPictureBox_MouseDown);
            mPictureBox.MouseUp += new MouseEventHandler(mPictureBox_MouseUp);
            mPictureBox.MouseMove += new MouseEventHandler(mPictureBox_MouseMove);
            mPictureBox.Paint += new PaintEventHandler(mPictureBox_Paint);
/*            mPictureBox.MouseClick += new MouseEventHandler(mPictureBox_MouseClick);*/
        }

        private void mPictureBox_Paint(object sender, PaintEventArgs e)
        {

            try
            {
                Draw(e.Graphics);
            }
            catch (Exception exp)
            {
                System.Console.WriteLine(exp.Message);
            }

        }

        private void mPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            mIsClick = true;

            nodeSelected = PosSizableRect.None;
            nodeSelected = GetNodeSelectable(e.Location);

            if (rect.Contains(new Point(e.X, e.Y)))
            {
                mMove = true;
            }
            oldX = e.X;
            oldY = e.Y;
        }

        private void mPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            mIsClick = false;
            mMove = false;
            
        }

        private void mPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                mPictureBox.Dispose();
        }

        private void mPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            ChangeCursor(e.Location);
            if (mIsClick == false)
            {
                if (hitxedge && hityedge)
                {
                    if(mMove)
                    {
                        if (e.X < oldX)
                        {
                            rect.X = rect.X + e.X - oldX;
                            oldX = e.X;
                            hitxedge = false;
                        }
                        if (e.Y < oldY)
                        {
                            rect.Y = rect.Y + e.Y - oldY;
                            oldY = e.Y;
                            hityedge = false;
                        }
                        TestIfRectInsideArea();
                        mPictureBox.Invalidate();
                    }
                }
                else
                {
                    if (hitxedge)
                    {
                        /*                    hitxedge = false;*/
                        if (mMove)
                        {
                            int temp = rect.Y + e.Y - oldY;
                            if(temp + rect.Height > mPictureBox.Height)
                            {
                                rect.Y = mPictureBox.Height - rect.Height - 1;
                                oldY = rect.Y;
                                hityedge = true;
                            }
                            else
                            {
                                rect.Y = rect.Y + e.Y - oldY;
                                oldY = e.Y;
                                hityedge = false;
                            }
                            if (e.X < oldX)
                            {
                                rect.X = rect.X + e.X - oldX;
                                oldX = e.X;
                                hitxedge = false;
                            }
                        }
                        TestIfRectInsideArea();
                        mPictureBox.Invalidate();

                    }
                    if (hityedge)
                    {
                        /* hityedge = false;*/
                        if (mMove)
                        {
                            int temp = rect.X + e.X - oldX;
                            if (temp + rect.Width > mPictureBox.Width)
                            {
                                rect.X = mPictureBox.Width - rect.Width - 1;
                                oldX = rect.X;
                                hitxedge = true;
                            }
                            else 
                            {
                                rect.X = rect.X + e.X - oldX;
                                oldX = e.X;
                                hitxedge = false;
                            }   
                            if (e.Y < oldY)
                            {
                                rect.Y = rect.Y + e.Y - oldY;
                                oldY = e.Y;
                                hityedge = false;
                            }
                        }
                        TestIfRectInsideArea();
                        mPictureBox.Invalidate();

                    }
                }
                return;
            }

            Rectangle backupRect = rect;

            switch (nodeSelected)
            {
                case PosSizableRect.LeftUp:
                    rect.X += e.X - oldX;
                    rect.Width -= e.X - oldX;
                    rect.Y += e.Y - oldY;
                    rect.Height -= e.Y - oldY;
                    break;
                case PosSizableRect.LeftMiddle:
                    rect.X += e.X - oldX;
                    rect.Width -= e.X - oldX;
                    break;
                case PosSizableRect.LeftBottom:
                    rect.Width -= e.X - oldX;
                    rect.X += e.X - oldX;
                    rect.Height += e.Y - oldY;
                    break;
                case PosSizableRect.BottomMiddle:
                    rect.Height += e.Y - oldY;
                    break;
                case PosSizableRect.RightUp:
                    rect.Width += e.X - oldX;
                    rect.Y += e.Y - oldY;
                    rect.Height -= e.Y - oldY;
                    break;
                case PosSizableRect.RightBottom:
                    rect.Width += e.X - oldX;
                    rect.Height += e.Y - oldY;
                    break;
                case PosSizableRect.RightMiddle:
                    rect.Width += e.X - oldX;
                    break;

                case PosSizableRect.UpMiddle:
                    rect.Y += e.Y - oldY;
                    rect.Height -= e.Y - oldY;
                    break;

                default:
                    if (mMove)
                    {
                        rect.X = rect.X + e.X - oldX;
                        rect.Y = rect.Y + e.Y - oldY;
                    }
                    break;
            }
            oldX = e.X;
            oldY = e.Y;

            if (rect.Width < 5 || rect.Height < 5)
            {
                rect = backupRect;
            }

            TestIfRectInsideArea();

            mPictureBox.Invalidate();
        }

        private void TestIfRectInsideArea()
        {
            // Test if rectangle still inside the area.
            if (rect.X < 0) rect.X = 0;
            if (rect.Y < 0) rect.Y = 0;
            if (rect.Width <= 0) rect.Width = 1;
            if (rect.Height <= 0) rect.Height = 1;
/*            if(hitxedge && hityedge)
            {
                mIsClick = false;
                return;
            }*/
            if (rect.X + rect.Width > mPictureBox.Width)
            {
                rect.Width = mPictureBox.Width - rect.X - 1; // -1 to be still show 
                if (allowDeformingDuringMovement == false)
                {
                    mIsClick = false;
                    hitxedge = true;
                }
            }
            if (rect.Y + rect.Height > mPictureBox.Height)
            {
                rect.Height = mPictureBox.Height - rect.Y - 1;// -1 to be still show 
                if (allowDeformingDuringMovement == false)
                {
                    mIsClick = false;
                    hityedge = true;
                }
            }
            if(!hitxedge && !hityedge)
            {
                mIsClick = true;
            }
        }


        private Rectangle CreateRectSizableNode(int x, int y)
        {
            return new Rectangle(x - sizeNodeRect / 2, y - sizeNodeRect / 2, sizeNodeRect, sizeNodeRect);
        }

        private Rectangle GetRect(PosSizableRect p)
        {
            switch (p)
            {
                case PosSizableRect.LeftUp:
                    return CreateRectSizableNode(rect.X, rect.Y);

                case PosSizableRect.LeftMiddle:
                    return CreateRectSizableNode(rect.X, rect.Y + +rect.Height / 2);

                case PosSizableRect.LeftBottom:
                    return CreateRectSizableNode(rect.X, rect.Y + rect.Height);

                case PosSizableRect.BottomMiddle:
                    return CreateRectSizableNode(rect.X + rect.Width / 2, rect.Y + rect.Height);

                case PosSizableRect.RightUp:
                    return CreateRectSizableNode(rect.X + rect.Width, rect.Y);

                case PosSizableRect.RightBottom:
                    return CreateRectSizableNode(rect.X + rect.Width, rect.Y + rect.Height);

                case PosSizableRect.RightMiddle:
                    return CreateRectSizableNode(rect.X + rect.Width, rect.Y + rect.Height / 2);

                case PosSizableRect.UpMiddle:
                    return CreateRectSizableNode(rect.X + rect.Width / 2, rect.Y);
                default:
                    return new Rectangle();
            }
        }

        private PosSizableRect GetNodeSelectable(Point p)
        {
            foreach (PosSizableRect r in Enum.GetValues(typeof(PosSizableRect)))
            {
                if (GetRect(r).Contains(p))
                {
                    return r;
                }
            }
            return PosSizableRect.None;
        }

        private void ChangeCursor(Point p)
        {
            mPictureBox.Cursor = GetCursor(GetNodeSelectable(p));
        }

        /// <summary>
        /// Get cursor for the handle
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Cursor GetCursor(PosSizableRect p)
        {
            switch (p)
            {
                case PosSizableRect.LeftUp:
                    return Cursors.SizeNWSE;

                case PosSizableRect.LeftMiddle:
                    return Cursors.SizeWE;

                case PosSizableRect.LeftBottom:
                    return Cursors.SizeNESW;

                case PosSizableRect.BottomMiddle:
                    return Cursors.SizeNS;

                case PosSizableRect.RightUp:
                    return Cursors.SizeNESW;

                case PosSizableRect.RightBottom:
                    return Cursors.SizeNWSE;

                case PosSizableRect.RightMiddle:
                    return Cursors.SizeWE;

                case PosSizableRect.UpMiddle:
                    return Cursors.SizeNS;
                default:
                    return Cursors.Default;
            }
        }

    }
}
