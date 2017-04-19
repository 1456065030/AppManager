using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using System.IO;

namespace AppManage
{
    public partial class DeleteForm : Form
    {
        public DeleteForm()
        {
            InitializeComponent();
        }

        List<int> list = new List<int>();
        List<MyApp> appList = null;

        private void DeleteForm_Load(object sender, EventArgs e)
        {
            appList = BeanUtil.appList;
            if (appList == null) {
                MessageBox.Show("读取错误或无软件列表！","提示");
                this.Close();
            }
            foreach (MyApp item in appList)
            {
                this.checkedListBox1.Items.Add(item.Id + ">" + item.Name);
            }

            //其他操作
            if (!BeanUtil.isNull(BeanUtil.zidingyiName)) {
                string formtext = this.Text;
                string formlabel1 = this.label1.Text;
                string buttontext = this.button1.Text;
                if (BeanUtil.zidingyiName.Equals("快捷方式")) {
                    formtext = "自定义创建快捷方式";
                    formlabel1 = "请选中将要创建快捷方式的应用：";
                    buttontext = "立即创建";
                }
                this.Text = formtext;
                this.label1.Text = formlabel1;
                this.button1.Text = buttontext;
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ChedckChang();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "出现未预知的错误！");
                this.Close();
            }
            if (this.checkedListBox1.CheckedItems.Count != this.checkedListBox1.Items.Count)
                this.checkBox1.Checked = false;
            else
                this.checkBox1.Checked = true;
        }

        private void ChedckChang()
        {
            this.listBox1.Items.Clear(); list.Clear();
            if (this.checkedListBox1.CheckedItems.Count <= 0) return;

            foreach (string item in this.checkedListBox1.CheckedItems)
            {
                if (int.Parse(item.Split('>')[0]) < 10)
                    this.listBox1.Items.Add(item.Split('>')[0] + ".  " + item.Split('>')[1]);
                else
                    this.listBox1.Items.Add(item.Split('>')[0] + ". " + item.Split('>')[1]);
                list.Add(int.Parse(item.Split('>')[0]));
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (list.Count <= 0) {
                MessageBox.Show("你还没有选中任何选项！","提示");
                return;
            }
            if (BeanUtil.isNull(BeanUtil.zidingyiName))
                this.delete();
            else {
                if (BeanUtil.zidingyiName.Equals("快捷方式")) {
                    this.createKjfs();
                }
            }
        }

        public void delete() {
            if (MessageBox.Show("共" + list.Count + "个应用，确定要删除吗？", "提示", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;
            foreach (int item in list)
            {
                try
                {
                    MyAppDao.delete(item);
                }
                catch { }
            }
            BeanUtil.update = true;
            this.Close();
        }

        //创建快捷方式
        public void createKjfs() {
            foreach (int i in list)
            {
                foreach (MyApp item in appList)
                {
                    if (item.Id == i)
                    {
                        try
                        {
                            WshShell shell = new WshShell();
                            //通过该对象的 CreateShortcut 方法来创建 IWshShortcut 接口的实例对象
                            string DirPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "//AppManager快捷方式";
                            Directory.CreateDirectory(DirPath);
                            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(DirPath + "//" + item.Name + ".lnk");
                            //设置快捷方式的目标所在的位置(源程序完整路径)
                            shortcut.TargetPath = item.Path;
                            //应用程序的工作目录   
                            //当用户没有指定一个具体的目录时，快捷方式的目标应用程序将使用该属性所指定的目录来装载或保存文件。   
                            shortcut.WorkingDirectory = System.Environment.CurrentDirectory;
                            //目标应用程序窗口类型(1.Normal window普通窗口,3.Maximized最大化窗口,7.Minimized最小化)   
                            shortcut.WindowStyle = 1;

                            //快捷方式的描述   
                            shortcut.Description = "AppManager-快捷方式";
                            //保存快捷方式   
                            shortcut.Save();
                            break;
                        }
                        catch (Exception e) { MessageBox.Show(e.Message,"错误！"); break; }
                    }
                }
            }
            MessageBox.Show("生成在桌面的“AppManager快捷方式”文件夹下。", "创建完成！");
            this.Close();
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
            {
                if (this.checkBox1.Checked)
                    checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                else
                    checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
                //checkedListBox1.SetItemChecked(i, true);
            }
            try
            {
                ChedckChang();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "出现未预知的错误！");
                this.Close();
            }
        }


    }
}
