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
    public partial class RegForm : Form
    {
        public RegForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string zc = this.textBox1.Text.Trim();
            if (zc.Equals(BeanUtil.zcm))
            {
                List<Other> list = OtherDao.read();
                if (list == null || list.Count == 0)
                {
                    OtherDao.add(new Other(1, "", BeanUtil.zcm));
                }
                else
                {
                    OtherDao.update(new Other(1, "", BeanUtil.zcm));
                }
                BeanUtil.update = true;
                MessageBox.Show("注册成功！", "感谢您的支持！");
                this.Close();
            }
            else {
                MessageBox.Show("注册码错误！","提示");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MoneyForm mf = new MoneyForm();
            mf.Show();
        }
    }
}
