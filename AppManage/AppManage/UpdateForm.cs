using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace AppManage
{
    public partial class UpdateForm : Form
    {
        public UpdateForm()
        {
            InitializeComponent();
        }

        string path = null; bool jiami = true; int stop = 0;
        private void UpdateForm_Load(object sender, EventArgs e)
        {
            BeanUtil.filepwd = null;
            MyApp app = BeanUtil.app;
            if (app == null)
            {
                MessageBox.Show("程序出现错误！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                BeanUtil.app = null;
                this.Close();
            }
            List<AppType> list = BeanUtil.typeList;
            string[] types = new string[list.Count];
            int tid = 0;
            for (int i = 0; i < list.Count; i++)
            {
                types[i] = list[i].Name;
                if (list[i].Name.Equals(app.Type)) {
                    tid = i;
                }
            }
            this.cobtype.Items.AddRange(types);
            this.cobtype.SelectedIndex = tid;
            this.txtname.Text = app.Name;
            path = app.Path;
            this.txtpath.Text = app.Path;
            if (!BeanUtil.isNull(app.Pwd))
            {
                jiami = true;
                BeanUtil.filepwd = app.Pwd;
                this.txtpath.Text = "***加密路径***";
            }
            else jiami = false;
            if (jiami) this.linkLabel2.Text = "解除加密";
            else this.linkLabel2.Text = "文件加密";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string name = this.txtname.Text;
            if (path.Trim() == null || path.Trim() == "")
            {
                MessageBox.Show("文件路径有误！", "警告"); return;
            }
            if (name.Trim() == null || name.Trim() == "")
            {
                MessageBox.Show("应用名不能为空！");
                return;
            }
            MyApp app = new MyApp();
            app.Name = name;
            app.Path = path;
            app.Id = BeanUtil.app.Id;
            app.Type = this.cobtype.SelectedItem.ToString();
            app.Pwd = BeanUtil.filepwd;
            BeanUtil.app = app;
            stop = 1;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                path = fbd.SelectedPath;
                if (jiami)
                {
                    this.txtpath.Text = "***加密路径***";
                }
                else
                {
                    this.txtpath.Text = fbd.SelectedPath;
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                path = ofd.FileName;
                if (jiami)
                {
                    this.txtpath.Text = "***加密路径***";
                }
                else
                {
                    this.txtpath.Text = ofd.FileName;
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (jiami)
            {
                MessageBox.Show("文件已加密，不能访问！", "提示");
                return;
            }
            int i = path.LastIndexOf("\\");
            try
            {

                BeanUtil.OpenAndSetWindow(path.Substring(0, i));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误！");
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!jiami)
            {
                UpdatePwdForm f = new UpdatePwdForm();
                f.ShowDialog();
                if (!BeanUtil.isNull(BeanUtil.filepwd))
                {
                    jiami = true;
                    this.linkLabel2.Text = "解除加密";
                    this.txtpath.Text = "***加密路径***";
                }
                else jiami = false;
            }
            else
            {
                if (BeanUtil.isNull(BeanUtil.filepwd))
                    return;
                PassForm f = new PassForm();
                f.ShowDialog();
                if (BeanUtil.truepwd)
                {
                   BeanUtil.filepwd = null;
                    this.linkLabel2.Text = "文件加密";
                    this.txtpath.Text = path;
                    jiami = false;
                }
                BeanUtil.truepwd = false;
            }


        }

        private void UpdateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (stop == 0) BeanUtil.app = null;
        }
    }
}
