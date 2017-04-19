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
    public partial class PassForm : Form
    {
        public PassForm()
        {
            InitializeComponent();
        }
        string newfilepwd = null;
        private void PassForm_Load(object sender, EventArgs e)
        {
            BeanUtil.truepwd = false;
            if (BeanUtil.filepwd == null || BeanUtil.filepwd == "")
            {
                MessageBox.Show("密码读取出错！", "系统出错！");
                this.Close();
            }
            newfilepwd = BeanUtil.filepwd;
        }
      
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Trim() == newfilepwd)
            {
                BeanUtil.truepwd = true;
                this.Close();
            }
        }
    }
}
