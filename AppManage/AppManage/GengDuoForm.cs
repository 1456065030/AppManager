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
    public partial class GengDuoForm : Form
    {
        public GengDuoForm()
        {
            InitializeComponent();
        }

        bool change = false;
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string mess = "欢迎使用！";
            MessageBox.Show(mess,"系统--伟神");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetPwdForm spf = new SetPwdForm();
            spf.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BeanUtil.zidingyiName = "快捷方式";
            DeleteForm df = new DeleteForm();
            df.ShowDialog();
            BeanUtil.zidingyiName = null;
        }

        private void rdo_Click(object sender, EventArgs e)
        {
            change = true;
        }

        private void GengDuoForm_Load(object sender, EventArgs e)
        {
            int sort = BeanUtil.sort;
            if (sort == 3)
                this.rdoYym.Checked = true;
            else if (sort == 2)
                this.rdoYxcs.Checked = true;
            else 
                this.rdoTjsx.Checked = true;
        }

        private void GengDuoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (change)
            {
                int sort = 1;
                if (this.rdoYym.Checked)
                    sort = 3;
                else if (this.rdoYxcs.Checked)
                    sort = 2;
                else
                    sort = 1;
                try
                {
                    OtherDao.sortUpdate(1, sort);
                    BeanUtil.sort = sort;
                }
                catch { MessageBox.Show("修改排序规则失败！","错误"); }
            }
        }
    }
}
