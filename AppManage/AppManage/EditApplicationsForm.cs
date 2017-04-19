using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AppManage
{
    public partial class EditApplicationsForm : Form
    {
        public EditApplicationsForm()
        {
            InitializeComponent();
        }

        Applications app = null;
        List<Applications> list = new List<Applications>();
        bool change = false;
        private void EditApplicationsForm_Load(object sender, EventArgs e)
        {
            loadApplications();
        }

        private void loadApplications()
        {
            list.Clear();
            listBox1.Items.Clear();
            list = ApplicationsDao.read();
            
            if (list != null)
            {
                string[] names = new string[list.Count];
                int i = 0;
                foreach (Applications item in list)
                {
                    string name = item.Name;
                    if (BeanUtil.isNull(name))
                        name = "[空标签]";
                    names[i++] = name;
                }
                listBox1.Items.AddRange(names);
                try
                {
                    listBox1.SelectedIndex = 0;
                    showApplications(0);
                }
                catch { }
            }
            showApplications(0);
        }

        private void createAndShowPanel(Applications item)
        {
            Panel p = new Panel();
            p.BorderStyle = BorderStyle.Fixed3D;
            p.Width = 660; p.Height = 100;
            if (!BeanUtil.isNull(item.Image))
            {
                try
                {
                    p.BackgroundImage = Image.FromFile(item.Image);
                }
                catch { }
            }

            Label lab = new Label();
            lab.Name = item.Id.ToString();//id
            lab.Text = item.Name;
            lab.Width = p.Width; lab.Height = p.Height;
            lab.BackColor = Color.Transparent;
            lab.TextAlign = ContentAlignment.MiddleCenter;
            try
            {
                lab.Font = new Font(item.Style, item.Size);
            }
            catch
            {
                lab.Font = new Font("楷体", 20F);
            }
            lab.ForeColor = Color.Black;
            try
            {
                lab.ForeColor = BeanUtil.colorHx16toRGB(item.Color);
            }
            catch
            {
                lab.ForeColor = Color.Black;
            }
            p.Controls.Add(lab);
            this.panel1.Controls.Clear();
            this.panel1.Controls.Add(p);
            app = item;
        }



        private void showApplications(int index) {
            app = null;
            try
            {
                Applications a = list[index];
                Applications newApp = new Applications(a.Id,a.Name,a.Path,a.Image,a.Style,a.Size,a.Color);
                app = null;
                this.textBox1.Text = a.Name;
                this.createAndShowPanel(newApp);
            }
            catch { MessageBox.Show("加载失败！","提示"); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.listBox1.ClearSelected(); 
            //新建
            if (change)
                if (MessageBox.Show("对象已更改是否保存", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    this.save();
                else
                    change = false;
            app = null;
            app = new Applications(); app.Name = "新创建"; app.Id = 0;
            this.textBox1.Text = app.Name;
            change = true;
            this.createAndShowPanel(app);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (change)
                if (MessageBox.Show("对象已更改是否保存", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    this.save();
                else
                    change = false;
            try
            {
                int i = this.listBox1.SelectedIndex;
                if (i < 0) return;
            }
            catch { return; }
            try
            {
                showApplications(this.listBox1.SelectedIndex);
            }
            catch { }
            
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //字体颜色
            ColorDialog dialog = new ColorDialog();
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                //ColorTranslator.FromHtml("Red");
                var color = dialog.Color;
                app.Color=BeanUtil.colorRGBtoHx16(color.R,color.G,color.B);
                change = true;
            }
            this.createAndShowPanel(app);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //背景图片
            if (app == null)
            {
                app = new Applications(); app.Name = "新创建"; app.Id = 0;
            }

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            string fpath = fbd.SelectedPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = fpath;
            ofd.Filter = "图片(jpg格式);图片(png格式);图片(gif格式);图片(jpeg格式)|*.jpg;*.png;*.gif;*.jpeg";
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                app.Image = ofd.FileName;
                change = true;
            }
            this.createAndShowPanel(app);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.save();
        }
        private void save() {
            //save
            if (app.Id != 0)
                MessageBox.Show(ApplicationsDao.update(app) ? "保存成功！" : "保存失败！");
            else
                MessageBox.Show(ApplicationsDao.add(app) ? "新建成功！" : "新建失败！");
            change = false;
            this.loadApplications();
            BeanUtil.update = true;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //字体风格
            FontDialog fd = new FontDialog();
            try
            {
                fd.Font = new Font(app.Style, app.Size);
            }
            catch { }
            if (fd.ShowDialog() == DialogResult.OK) {
                Font f = fd.Font;
                app.Style = f.FontFamily.Name;
                app.Size = f.Size;
                change = true;
            }
            this.createAndShowPanel(app);
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //绑定应用
            string DesktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = DesktopPath;
            try
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    app.Path = ofd.FileName;
                    change = true;
                }
                MessageBox.Show("绑定成功！");
            }
            catch { MessageBox.Show("绑定失败！"); }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (app == null)
                return;
            //name
            app.Name = this.textBox1.Text;
            change = true;
            this.createAndShowPanel(app);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;
            //删除
            if (app == null) 
                this.loadApplications();
            if (app.Id == 0)
            {
                try
                {
                    this.listBox1.SelectedIndex = 0;
                    change = false;
                }
                catch { }
                MessageBox.Show("删除成功！");
            }
            else {
                if (ApplicationsDao.delete(app.Id))
                {
                    MessageBox.Show("删除成功！");
                    BeanUtil.update = true;
                }
                else {
                    MessageBox.Show("删除失败！");
                }
                this.loadApplications();
            }
            change = false;
        }
    }
}
