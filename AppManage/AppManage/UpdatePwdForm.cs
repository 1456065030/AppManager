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
    public partial class UpdatePwdForm : Form
    {
        public UpdatePwdForm()
        {
            InitializeComponent();
        }

        private void UpdatePwdForm_Load(object sender, EventArgs e)
        {
            this.textBox1.Select();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pwd = this.textBox1.Text.Trim();
            if (pwd == null || pwd == "")
            {
                MessageBox.Show("请输入密码！", "提示");
                return;
            }
            if (pwd != this.textBox2.Text.Trim())
            {
                MessageBox.Show("密码不一致！", "警告！");
                return;
            }
            BeanUtil.filepwd = pwd;
            this.Close();
        }

    }
}
