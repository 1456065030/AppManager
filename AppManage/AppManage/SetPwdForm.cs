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
    public partial class SetPwdForm : Form
    {
        public SetPwdForm()
        {
            InitializeComponent();
        }
        string pwd = "";
        private void SetPwdForm_Load(object sender, EventArgs e)
        {
            bool first = false;
            PwdClass.read(ref pwd,ref first);
            try
            {
                if (!first && !pwd.Equals("-1"))
                {
                    string[] rou = { "r", "o", "u", "s", "e", "v" };
                    string fp = "";
                    foreach (var item in pwd.Trim())
                    {
                        fp += rou[int.Parse(item.ToString())];
                    }
                    PassForm f = new PassForm();
                    BeanUtil.filepwd = fp;
                    MessageBox.Show("请输入软件启动密码以rousev的方式输入！","提示");
                    f.ShowDialog();
                    if (!BeanUtil.truepwd)
                        this.Close();
                }
            }
            catch { }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar) == 8) return;
            string key = e.KeyChar.ToString().ToLower();
            if (key != "r" && key != "o" && key != "u" && key != "s" && key != "e" && key != "v")
            {
                e.Handled = true;
                this.labelrousev.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fpwd = null;
            string[] rou = { "r", "o", "u", "s", "e", "v" };
            foreach (var item in this.txtpwd.Text.Trim().ToLower())
            {
                for (int i = 0; i < rou.Length; i++)
                {
                    if (item.ToString() == rou[i])
                    {
                        fpwd += i.ToString(); break;
                    }
                }
            }
            if (BeanUtil.isNull(fpwd))
            {
                PwdClass.update("-1");
                MessageBox.Show("关闭软件启动密码成功！", "消息");
            }
            else
            {
                PwdClass.update(fpwd);
                MessageBox.Show("修改软件启动密码成功！", "消息");
            }
            this.Close();
        }
    }
}
