using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Xml;
using System.Text.RegularExpressions;

namespace AppManage
{
    public partial class Open : Form
    {
        Point mouseOff;//鼠标移动位置变量
        bool leftFlag;//标签是否为左键
        int[] imgArr = new int[7];
        Brush b; Graphics g;
        Font f; bool firstopen=false;
        string Rousev=null;
        string tempfilepwd ="";
        public Open()
        {
            InitializeComponent();
        }
       
        private void imgdown_MouseEnter(object sender, EventArgs e)
        {
            this.imgdown.Image=imageList1.Images[3];
        }

        private void imgClose_MouseEnter(object sender, EventArgs e)
        {
            this.imgClose.Image = imageList1.Images[1];
        }

        private void imgdown_MouseLeave(object sender, EventArgs e)
        {
            this.imgdown.Image = imageList1.Images[2];
        }

        private void imgClose_MouseLeave(object sender, EventArgs e)
        {
            this.imgClose.Image = imageList1.Images[0];
        }

        private void imgClose_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private void imgdown_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Open_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                leftFlag = true;                  //点击左键按下时标注为true;
            }
        }

        private void Open_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition; 
                int y = mouseSet.Y - Location.Y;
                if (y > 25) return;
                this.Cursor = Cursors.SizeAll;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }

        private void Open_MouseUp(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Default;
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }

        private void ImageCursor()
        {
            Point mouseSet = Control.MousePosition;
            int x = mouseSet.X - Location.X; 
            int y = mouseSet.Y - Location.Y; 
            if (x <= 321 && x >= 321 - 50 && y >= 154 && y <= 154 + 50) { 
                //左下 PanNE
                this.pictureBox1.Cursor = Cursors.PanNE;
            }
            else if (x <= 321 && x >= 321 - 50 && y <= 154 && y >= 154 - 50) {
                //左up PanEast
                this.pictureBox1.Cursor = Cursors.PanEast;
            }
            else if (x >= 321 && x <= 321 + 50 && y >= 154 && y <= 154 + 50) {
                //右下 PanSouth
                this.pictureBox1.Cursor = Cursors.PanSouth;
            }
            else if (x >= 321 && x <= 321 + 50 && y <= 154 && y >= 154 - 50)
            {
                //PanSE
                this.pictureBox1.Cursor = Cursors.PanSE;
            }
            else {
                //Hand
                this.pictureBox1.Cursor = Cursors.Hand;
            }
           
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ImageCursor();
        }

        #region 图片旋转函数
         
        // XuanZhuan(1);  0顺时针 1逆时针 旋转45°
        public void XuanZhuan(int jd) {
            try
            {
                Bitmap a = new Bitmap(pictureBox1.Image);//得到图片框中的图片
                pictureBox1.Image = Rotate(a, jd);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                //pictureBox1.Location = panel1.Location;
                pictureBox1.Refresh();//最后刷新图片框
            }
            catch { }
        }

         public Bitmap Rotate(Bitmap b, int angle)
        {
            angle = angle % 360;
 
            //弧度转换
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);
 
            //原图的宽和高
            int w = b.Width;
            int h = b.Height;
            int W = w;//(int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            int H = h;//(int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));
 
            //目标位图
            Bitmap dsImage = new Bitmap(W, H);
            Graphics g = Graphics.FromImage(dsImage);            
 
            g.InterpolationMode = InterpolationMode.Bilinear;
 
            g.SmoothingMode = SmoothingMode.HighQuality;
 
            //计算偏移量
            Point Offset = new Point((W - w) / 2, (H - h) / 2);
 
            //构造图像显示区域：让图像的中心与窗口的中心点一致
            Rectangle rect = new Rectangle(Offset.X, Offset.Y, w, h);
            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(360 - angle);
 
            //恢复图像在水平和垂直方向的平移
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawImage(b, rect);
 
            //重至绘图的所有变换
            g.ResetTransform();
 
            g.Save();
            g.Dispose();
            return dsImage;
        }
        #endregion

         #region 旋转方法
         public void XZyun(int img,int zf) {
             if (zf == 1)
             {
                 for (int i = 0; i <= img; i++)
                 {
                     ZhuanLeftRight(imgArr[i], -5, imgArr[i + 1]);
                 }
             }
             else {
                 for (int i = img+1; i >0; i--)
                 {
                     if(i!=1)
                         ZhuanLeftRight(imgArr[i], 5, imgArr[i-1]);
                     else
                         ZhuanLeftRight(imgArr[i], 5, 13);
                 }
             }
         }

         public void ZhuanLeftRight(int imgxb1, int xz, int imgxb2)
         {
             this.pictureBox1.Image = imageList1.Images[imgxb1];
             pictureBox1.Refresh();
             for (int i = 0; i < 45; i+=5)  // for (int i = 0; i < 45; i++) 
             {
                 XuanZhuan(xz);
             }
             this.pictureBox1.Image = imageList1.Images[imgxb2];
             pictureBox1.Refresh();
         }

         public void XZstar(int imgxb) {

             XZyun(imgxb, 1);
             Thread.Sleep(500);
             XZyun(imgxb, 0);
             Rousev += imgxb.ToString();
         }

        #endregion

         private void Open_Load(object sender, EventArgs e)
         {
             g = this.panel1.CreateGraphics();
             b = Brushes.Green;
             f = new Font("楷体", 10);
             imgArr = new int[] {12,5,6,7,8,9,10 };
             xmlTempFile();
             if (tempfilepwd == "-1")
             {
                 this.Close();
             }
             this.timerjiance.Enabled = true;
         }
         int dianji = 0;
         private void img0_Click(object sender, EventArgs e)
         {
             PictureBox p = (PictureBox)sender;
             string img = p.Name;
             try
             {
                 XZstar(int.Parse(img.Substring(3)));
             }
             catch { }
             if (firstopen)
             {
                 if (Rousev == null || Rousev.Trim() == "") { return; }
                 ShowPwd();
             }
             else
             {
                  dianji++;
                  string txt = tempfilepwd.Trim();
                  if (Regex.IsMatch(Rousev.Trim(), txt))
                  {
                      this.Close(); return;
                  }
                  if (dianji >= 6) {
                      b = Brushes.Red; panel1.Refresh();
                      f = new Font("微软雅黑", 10);
                      g.DrawString("警告：你的输入已超过"+dianji+"次", f, b, 10, 10);
                  }
                  if (dianji > 10) {
                      MessageBox.Show("由于你的密码次数输入过多，系统将强制关闭应用！\n\n忘记密码请联系QQ：1456065030","警告！");
                      Application.ExitThread(); return;
                  }
             }
            
         }

         private void ShowPwd()
         {
             string fpwd = null;
             string[] rou = { "R", "o", "u", "s", "e", "v" };
             foreach (var item in Rousev)
             {
                 fpwd += rou[int.Parse(item.ToString())] + " ";
             }
             b = Brushes.Red; panel1.Refresh();
             f = new Font("微软雅黑", 10);
             g.DrawString("你的密码：" + fpwd, f, b, 10, 10);
         }
         int enter = 0;
         private void Open_MouseEnter(object sender, EventArgs e)
         {
             if (firstopen) return;
             if (enter == 0 || enter == 1)
             {
                 b = Brushes.Black;
                 g.DrawString("欢迎使用MyApp管理应用程序！", f, b, 10, 10);
             }
             else if (enter == 2) {
                 b = Brushes.Green; panel1.Refresh();
                 f = new Font("微软雅黑",10);
                 g.DrawString("请点密码旋转密令进入系统！", f, b, 10, 10);
             }
             enter++;
         }

        private void xmlTempFile(){
            PwdClass.read(ref tempfilepwd,ref firstopen);
        }

        private void qxpwdset(string txt) {
            PwdClass.update(txt);
        }

        private void timerjiance_Tick(object sender, EventArgs e)
        {
            this.timerjiance.Enabled = false;
            if (tempfilepwd == "-1")
            {
                Close();
            }
            if (firstopen)
            {
                DialogResult dr = MessageBox.Show("检测到你是第一次使用，请设密码！", "系统", MessageBoxButtons.OKCancel);
                if (dr == DialogResult.Cancel) {
                    if (MessageBox.Show("确定取消设置密码吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        qxpwdset("-1");
                        this.Close(); return;
                    }
                }
                b = Brushes.Red; panel1.Refresh();
                f = new Font("微软雅黑", 10);
                g.DrawString("设置密码开始，请点击Rousev字母开始设置！", f, b, 0, 10);
                this.panel2.Left = 457; 
                this.panel2.Top = 172;
                Rousev ="";
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Rousev == null || Rousev.Trim().Equals("")) {
                if (MessageBox.Show("确认不设置密码吗？", "提示", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }
                else
                {
                    qxpwdset("-1");
                    this.Close(); return;
                }
            }
            qxpwdset(Rousev);
            string fpwd = null;
            string[] rou = { "R", "o", "u", "s", "e", "v" };
            foreach (var item in Rousev)
            {
                fpwd += rou[int.Parse(item.ToString())] + " ";
            }
            MessageBox.Show("请牢记你的密令："+fpwd,"温馨提示");
            this.Close(); return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Rousev = "";
            ShowPwd();
        }



    }
}
