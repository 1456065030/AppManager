using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace AppManage
{
    public partial class InsertForm : Form
    {
        public InsertForm()
        {
            InitializeComponent();
        }

        bool filejia = false;

        private void InsertForm_Load(object sender, EventArgs e)
        {
            List<AppType> list= BeanUtil.typeList;
            string[] types = new string[list.Count];

            if (list==null || list.Count < 1) {
                types=new string[]{"未知"};
            }
            
            for (int i = 0; i < list.Count; i++)
            {
                types[i]=list[i].Name;
            }
            this.cobtype.Items.AddRange(types);
            this.cobtype.SelectedIndex = 0;
            //拖文件添加
            try
            {
                if (BeanUtil.filepath == null) return;
                string path= BeanUtil.filepath[0];
                this.txtname.Text= path.Substring(path.LastIndexOf('\\') + 1).Split('.')[0];
                this.txtpath.Text = path;
                string typeName = BeanUtil.getFileTypeName(path);
                if (types != null)
                {
                    for (int i = 0; i < types.Length; i++)
                    {
                        if (typeName.Equals(types[i]))
                        {
                            cobtype.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message,"拖文件读取错误"); }
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.txtpath.Text = fbd.SelectedPath;
                filejia = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string DesktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);//得到桌面文件夹 

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = DesktopPath;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.txtpath.Text = ofd.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string name = this.txtname.Text;
            string path = this.txtpath.Text;
            if (path.Trim() == null || path.Trim() == "")
            {
                MessageBox.Show("文件路径有误！", "警告"); return;
            }
            if (name.Trim() == null || name.Trim() == "")
            {
                MessageBox.Show("应用名不能为空！");
                return;
            }
            if (!File.Exists(path.Trim()) && !filejia)
            {
                if(MessageBox.Show("检测目标不是文件或不存在！是否继续导入？", "提示！",MessageBoxButtons.OKCancel)==DialogResult.Cancel)
                    return;
            }
            MyApp app = new MyApp();
            app.Name = name;
            app.Path = path;
            app.Type = this.cobtype.SelectedItem.ToString();
            BeanUtil.app = app;
            this.Close();
        }


        private void txtpath_TextChanged(object sender, EventArgs e)
        {
            filejia = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            BeanUtil.update = false;
            InsertMultiForm iuf = new InsertMultiForm();
            iuf.ShowDialog();
            if (BeanUtil.update) {
                this.Close();
            }
        }
    }
}
