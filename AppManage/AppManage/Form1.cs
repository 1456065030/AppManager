using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;
using IWshRuntimeLibrary;

namespace AppManage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region 全局变量
        //所有MyApp数据集合
        List<MyApp> appList = new List<MyApp>();
       //过滤后的MyApp数据集合
        List<MyApp> filterList = new List<MyApp>();
        //显示(绑定)dataGridView的MyApp数据集合
        BindingList<MyApp> showList = new BindingList<MyApp>();
        //所有AppType的数据集合
        List<AppType> typeList = new List<AppType>();
        //所有Applications的数据集合
        List<Applications> yingyongList = new List<Applications>();
        //扩展panel集合
        List<Panel> panelList = new List<Panel>();
        //排序方式
        int sort = 1;
        #endregion

        #region 加载数据

        private void loadData() {
            typeList = AppTypeDao.read();
            appList = MyAppDao.read();
            if(appList == null)
                appList= new List<MyApp>();
            this.cobType.Items.Clear();
            this.cobType.Items.Add("全部应用");

            //给MyApp的type赋值和绑定cobType
            if (typeList != null && appList != null)
            {
                int[] type = new int[MyAppDao.getMaxId()+AppTypeDao.getMaxId()];
                for (int i = 0; i < typeList.Count; i++)
                {
                    AppType at = typeList[i];
                    try
                    {
                        type[at.Id] = i;
                    }
                    catch { }
                    this.cobType.Items.Add(at.Name);
                }
                for (int i = 0; i < appList.Count; i++)
                {
                    appList[i].Count = i + 1;
                    //路径加密
                    if (BeanUtil.isNull(appList[i].Pwd))
                        appList[i].ShowPath = appList[i].Path;
                    else
                        appList[i].ShowPath = "Files in encrypted";
                    int tid = appList[i].TypeId;
                    try
                    {
                        appList[i].Type = typeList[type[tid]].Name;
                    }
                    catch { appList[i].Type = "未知"; }
                }
            }
            //排序
            this.ListSort(appList,sort,true);
            this.cobType.SelectedIndex = 0;
            showList = new BindingList<MyApp>(appList);
            this.MyDataGridView.DataSource = showList;
        }
        #endregion

        #region 按条件过滤,检索

        private void query() {
            filterList.Clear();
            string name = this.txtName.Text.Trim().ToLower();
            string type = this.cobType.SelectedItem.ToString();
            int tid = this.cobType.SelectedIndex;
            if (tid == 0 && BeanUtil.isNull(name)) {
                foreach (MyApp item in appList)
                {
                    filterList.Add(item);
                }
                showList = new BindingList<MyApp>(filterList);
                this.MyDataGridView.DataSource = showList;
                return;
            }
            foreach (MyApp item in appList)
            {
                try
                {
                    if ((!Regex.IsMatch(item.Name.Trim().ToLower(), name)) && (!Regex.IsMatch(BeanUtil.GetChineseSpell(item.Name.Trim().ToLower()), name)))
                    {
                        continue;
                    }
                }
                catch {
                    string mess = @"注意：不能含有非法字符！（ +[(?)\* 等）";
                    MessageBox.Show(mess,"警告");
                    showList = new BindingList<MyApp>(filterList);
                    this.MyDataGridView.DataSource = showList;
                    return;
                }
                if (tid != 0)
                {
                    if (!Regex.IsMatch(item.Type.Trim().ToLower(), type))
                    {
                        continue;
                    }
                }
                try
                {
                    this.filterList.Add(item);
                }
                catch (Exception ex) { MessageBox.Show("错误信息：\n"+ex.Message,"出现未知错误！--119", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            this.ListSort(filterList,sort,false);
            showList = new BindingList<MyApp>(filterList);
            this.MyDataGridView.DataSource = showList;
        }
        #endregion

        #region 事件方法

        private void Form1_Load(object sender, EventArgs e)
        {
            Jianpan_close();
            //this.AllowDrop=true;
            string tempfilepwd = ""; bool firstopen = false;
            PwdClass.read(ref tempfilepwd, ref firstopen);
            //判断是否要open Pwd
            if (firstopen) {
                PwdClass.deletePwdNode();
                Open f = new Open();
                f.ShowDialog();
            }else if (tempfilepwd.Trim() != "-1"){
                Open f = new Open();
                f.ShowDialog();
            }

            List<Other> list = OtherDao.read();
            if (list == null || list.Count == 0)
            {
                DateTime dt = DateTime.Now;
                string yyr = "" + dt.Year.ToString().Substring(2, 2) + ":" + dt.Month + ":" + dt.Day;
                Other o = new Other();
                o.Zc = ""; o.Yyr = yyr; o.Id = 1;
                OtherDao.add(o);
            }
            else {
                //排序方式
                sort = list[0].Sort;
                if (Other.isGuoQiAndNoZc(list, 3)) {
                    this.linkLabelZhuce.Visible = true;
                    //您的软件即将过试用期！请尽快注册。
                }
                if (Other.isGuoQiAndNoZc(list, 5)) {
                    this.timer1.Enabled = true;
                }
            }
            this.MyDataGridView.AutoGenerateColumns = false;
            this.loadData();
            this.loadApplications();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;

            MessageBox.Show("您的软件已过试用期！", "温馨提示");
            BeanUtil.update = false;
            RegForm reg = new RegForm();
            reg.ShowDialog();
            if (!BeanUtil.update)
                this.Close();
        }

        private void cobType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.query();
            }
            catch (Exception ex) { MessageBox.Show("错误信息：\n"+ex.Message,"出现未知错误！--185",MessageBoxButtons.OK,MessageBoxIcon.Error); }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtName.SelectionStart = txtName.Text.Length;
                this.query();
            }
            catch (Exception ex) { MessageBox.Show("错误信息：\n"+ex.Message,"出现未知错误！--195", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.query();
            }
            catch (Exception ex) { MessageBox.Show("错误信息：\n"+ex.Message,"出现未知错误！--203", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void MyDataGridView_DoubleClick(object sender, EventArgs e)
        {
            this.打开该应用ToolStripMenuItem_Click(null,null);
        }

        #endregion

        #region 快捷菜单click事件

        private void 打开该应用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyApp app = null;
            try
            {
                app = this.MyDataGridView.CurrentRow.DataBoundItem as MyApp;
            }
            catch { MessageBox.Show("你还未选中任何应用！", "提示"); return; }
            if (!BeanUtil.isNull(app.Pwd))
            {
                PassForm f = new PassForm();
                BeanUtil.filepwd = app.Pwd;
                f.ShowDialog();
                if (!BeanUtil.truepwd) return;
            }
           
            try
            {
                MyAppDao.click(app.Id);
                app.Click++;
            }
            catch { }
            BeanUtil.OpenAndSetWindow(app.Path);

        }

        private void 查看所在文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyApp app = null;
            try
            {
                app = this.MyDataGridView.CurrentRow.DataBoundItem as MyApp;
            }
            catch { MessageBox.Show("你还未选中任何应用！", "提示"); return; }
            if (!BeanUtil.isNull(app.Pwd))
            {
                PassForm f = new PassForm();
                BeanUtil.filepwd = app.Pwd;
                f.ShowDialog();
                if (!BeanUtil.truepwd) return;
            }

            //int i = app.Path.LastIndexOf("\\");
            //BeanUtil.OpenAndSetWindow(app.Path.Substring(0, i));

            System.Diagnostics.Process.Start("Explorer.exe", @"/select," + app.Path);
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //代替是否高级添加
            BeanUtil.update = false;
            InsertForm addForm = new InsertForm();
            BeanUtil.typeList = this.typeList;
            BeanUtil.app = null;
            addForm.ShowDialog();
            MyApp app = BeanUtil.app;
            if (app == null) {
                if (BeanUtil.update)
                    this.loadData();
                return;
            }
            app.TypeId = getTidByName(app.Type);
            if (MyAppDao.add(app))
                MessageBox.Show("添加成功！");
            else
                MessageBox.Show("添加失败！");
            this.loadData();
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyApp app = null;
            try
            {
                app = this.MyDataGridView.CurrentRow.DataBoundItem as MyApp;
            }
            catch { MessageBox.Show("你还未选中任何应用！", "提示"); return; }
            UpdateForm uf = new UpdateForm();
            BeanUtil.typeList = this.typeList;
            BeanUtil.app = app;
            uf.ShowDialog();
            app = BeanUtil.app;
            if (app == null) return;
            app.TypeId = getTidByName(app.Type);
            if (MyAppDao.update(app))
                MessageBox.Show("修改成功！");
            else
                MessageBox.Show("修改失败！");
            BeanUtil.filepwd = null;
            this.loadData();
        }

        private void 属性ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyApp app = null;
            try
            {
                app = this.MyDataGridView.CurrentRow.DataBoundItem as MyApp;
            }
            catch { MessageBox.Show("你还未选中任何应用！", "提示"); return; }
            if (!BeanUtil.isNull(app.Pwd))
            {
                PassForm f = new PassForm();
                BeanUtil.filepwd = app.Pwd;
                f.ShowDialog();
                if (!BeanUtil.truepwd) return;
            }
            FileInfo fi = new FileInfo(app.Path);
            string mess = null;
            mess += "文件名：" + fi.Name + "\n";
            mess += "上次访问时间：" + fi.LastAccessTime.ToString("yyyy-MM-dd") + "\n";
            mess += "是否只读：" + fi.IsReadOnly.ToString() + "\n";
            try
            {
                mess += "文件大小：";
                long size = fi.Length;
                string sizeStr = "";
                if (size < 1024) {
                    sizeStr = size+"(字节)";
                }
                else if (size < 1024 * 1024) {
                    sizeStr = Math.Round((double)size / 1024, 2) + "(KB)";
                }
                else if (size < 1024 * 1024 * 1024)
                {
                    sizeStr =  Math.Round((double)size / (1024 * 1024), 2) + "MB";
                }
                else {
                    sizeStr = Math.Round((double)size / (1024 * 1024 * 1024), 2) + "G";
                }
                mess +=sizeStr + "\n";
            }
            catch {
                mess += "未知\n";
            }
           
            string pathAll = fi.FullName;
            string path = "";
            while (pathAll.Length > 25) {
                path+=pathAll.Substring(0, 25)+"\n";
                pathAll = pathAll.Substring(25);
            }
            path += pathAll;

            mess += "文件路径：\n【" + path + "】\n";
            try
            {
                mess += "扩展名：" + fi.Extension + "\n";
            }
            catch { }
            mess += "创建时间：" + fi.CreationTime.ToString("yyyy-MM-dd") + "\n";
            mess += "文件或目录特性：" + fi.Attributes+"\n";
            mess += "运行次数: "+app.Click+"次";
            MessageBox.Show(mess, "文件详情");
        }

        private void 删除单个ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyApp app = null;
            try
            {
                app = this.MyDataGridView.CurrentRow.DataBoundItem as MyApp;
            }
            catch { MessageBox.Show("你还未选中任何应用！", "提示"); return; }
            if (MessageBox.Show("确认删除”" + app.Name + "“吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
                return;
            if (!BeanUtil.isNull(app.Pwd))
            {
                PassForm f = new PassForm();
                BeanUtil.filepwd = app.Pwd;
                f.ShowDialog();
                if (!BeanUtil.truepwd) return;
            }
            if (MyAppDao.delete(app.Id))
                MessageBox.Show("删除成功！");
            else
                MessageBox.Show("删除失败！");
            this.loadData();
        }

        private void 自定义删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeanUtil.update = false;
            BeanUtil.appList = this.appList;
            DeleteForm df = new DeleteForm();
            df.ShowDialog();
            if (BeanUtil.update)
                this.loadData();
        }

        private void 编辑类型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeanUtil.update = false;
            UpdateTypeForm tf = new UpdateTypeForm();
            tf.ShowDialog();
            if (BeanUtil.update)
                this.loadData();
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeanUtil.appList = this.appList;
            GengDuoForm gdf = new GengDuoForm();
            BeanUtil.sort = this.sort;
            gdf.ShowDialog();
            if (BeanUtil.sort != this.sort) {
                this.sort = BeanUtil.sort;
                ListSort(filterList, sort, true);
                ListSort(appList, sort, true);
                showList = new BindingList<MyApp>(filterList);
                this.MyDataGridView.DataSource = showList;
            }
           
        }

        #endregion

        #region 其它方法

        //根据类型获取类型id
        private int getTidByName(string type) {
            int tid = 0;
            foreach (AppType item in typeList)
            {
                if (item.Name.Equals(type)) {
                    tid = item.Id; break;
                }
            }
            return tid;
        }
        //软件注册
        private void linkLabelZhuce_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            BeanUtil.update = false;
            RegForm reg = new RegForm();
            reg.ShowDialog();
            this.linkLabelZhuce.Visible = !BeanUtil.update;
        }

        #endregion

        #region 扩展中心

        private void loadApplications() {
            yingyongList.Clear();
            panelList.Clear();
            page = 1;
            yingyongList = ApplicationsDao.read();
            foreach (Applications item in yingyongList)
            {
                Panel p = new Panel();
                p.BorderStyle = BorderStyle.Fixed3D;
                p.Width = this.panel2.Width-10; 
                p.Height = 100;
                if (!BeanUtil.isNull(item.Image)) {
                    try
                    {
                        p.BackgroundImage = Image.FromFile(item.Image);
                    }
                    catch { }
                }
                Label lab = new Label();
                if (!BeanUtil.isNull(item.Path))
                    lab.Cursor = Cursors.Hand;
                lab.Name = item.Id.ToString();//id
                lab.Text = item.Name; lab.Width = p.Width;
                lab.Height = p.Height;
                lab.BackColor = Color.Transparent;
                lab.TextAlign = ContentAlignment.MiddleCenter;
                lab.Click += new System.EventHandler(this.MyLabel_Click);
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
                panelList.Add(p);
            }
            showApplications(0);
        }

        int page = 1;//页数
        private void showApplications(int status) {
            int maxPage = (panelList.Count - 1) / 3 + 1;
            if (page + status <= 0 || page + status > maxPage)
                return;
            page = page + status;
            this.panel2.Controls.Clear();
            if (panelList == null) return;
            int countp = 1; 
            int count = panelList.Count;
            if (count % 3 == 0) countp = count / 3;
            else countp = count / 3 + 1;
            this.label4.Text = page + "/" + countp;
            int i = (page - 1) * 3;
            for (int j = 0; i < panelList.Count; i++, j++)
            {
                Panel p = panelList[i];
                p.Location = new Point(5, p.Height*j+14 * (j+1));//14 130
                this.panel2.Controls.Add(p);
                if (j == 2) break;
            }

        }

        private void MyLabel_Click(object sender, EventArgs e)
        {
            Label lab = (Label)sender;
            try
            {
                Applications app = getYingyongById(int.Parse(lab.Name));
                if (BeanUtil.isNull(app.Path)) return;
                BeanUtil.OpenAndSetWindow(app.Path);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "错误"); }
        }

        private Applications getYingyongById(int id) {
            foreach (Applications item in yingyongList)
            {
                if (id == item.Id) {
                    return item;
                }
            }
            return null;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //上页
            showApplications(-1);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //下页
            showApplications(1);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //编辑
            BeanUtil.update = false;
            EditApplicationsForm eaf = new EditApplicationsForm();
            eaf.ShowDialog();
            if (BeanUtil.update)
                this.loadApplications();
        }

        #endregion

        #region 实现可拖文件功能
        string[]files = null;
        private void MyDataGridView_DragEnter(object sender, DragEventArgs e)
        {
            
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if(files!=null)
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (Path.GetExtension(files[i]).ToLower().Equals(".lnk"))
                        {
                            WshShell shell = new WshShell();
                            IWshShortcut ff = (IWshShortcut)shell.CreateShortcut(files[i]);
                            files[i] = ff.TargetPath;
                        }
                    }
            }
        }
        
        private void MyDataGridView_DragLeave(object sender, EventArgs e)
        {
            if (files == null) return;
            if (files.Length < 1) return;
            BeanUtil.filepath = files;
            if (files.Length == 1)
            {
                try
                {
                    this.添加ToolStripMenuItem_Click(null, null);
                }
                catch { MessageBox.Show("读取文件错误！","错误！"); }
            }
            else {
                BeanUtil.update = false;
                BeanUtil.typeList = this.typeList;
                InsertMultiForm infs = new InsertMultiForm();
                infs.ShowDialog();
                if (BeanUtil.update)
                    this.loadData();
            }
            BeanUtil.filepath = null;
            files = null;
        }
        #endregion

        #region 虚拟键盘
        private void JianPan_Click(object sender, EventArgs e)
        {
            try
            {
                Label lab = (Label)sender;
                this.txtName.Text += lab.Text.Trim();
            }
            catch { }
        }

        private void JianPan_Back_Click(object sender, EventArgs e)
        {
            try
            {
                Label lab = (Label)sender;
                string txt = lab.Text;
                string str = this.txtName.Text;
                if (txt.Trim().ToLower().Equals("x"))
                    str = "";
                else
                    str = str.Length > 0 ? str.Substring(0, str.Length - 1) : "";
                this.txtName.Text = str;
            }
            catch { }
        }

        private void JianPan_MouseEnter(object sender, EventArgs e)
        {
            Label lab = (Label)sender;
            lab.BorderStyle = BorderStyle.FixedSingle;
            lab.BackColor = BeanUtil.RandomColor();
        }

        private void JianPan_MouseLeave(object sender, EventArgs e)
        {
            Label lab = (Label)sender;
            lab.BorderStyle = BorderStyle.None;
            lab.BackColor = Color.LightGray;
        }

        private void p_jianpan_MouseEnter(object sender, EventArgs e)
        {
            Panel p = (Panel)sender;
            foreach (var item in p.Controls)
            {
                if (item is Label)
                {
                    Label lab = item as Label;
                    if (lab.BorderStyle != BorderStyle.None)
                    {
                        lab.BorderStyle = BorderStyle.None;
                        lab.BackColor = Color.LightGray;
                    }
                }
            }
        }
        Color jiancolor = Color.LightGray;
        private void JianPan_MouseDown(object sender, MouseEventArgs e)
        {
            Label lab = (Label)sender;
            jiancolor = lab.BackColor;
            lab.BackColor = BeanUtil.colorHx16toRGB("#293955");
        }

        private void JianPan_MouseUp(object sender, MouseEventArgs e)
        {
            Label lab = (Label)sender;
            lab.BackColor = jiancolor;
        }
        //收展键盘
        private void Jianpan_close()
        {
            p_jianpan.Top = p_jianpan.Parent.Height+10;
        }
        private void Jianpan_open()
        {
            p_jianpan.Top = p_jianpan.Parent.Height - p_jianpan.Height;
        }

        private void tabPage1_MouseClick(object sender, MouseEventArgs e)
        {
            Jianpan_close();
        }

        private void txtName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (isOpenJianpan()) 
                Jianpan_close();
            else 
                Jianpan_open();
        }

        private void MyDataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            Jianpan_close();
        }

        private bool isOpenJianpan() {
            return p_jianpan.Top < p_jianpan.Parent.Height;
        }

        #endregion

        #region 排序方法
        private void MyDataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn sortColumn = (DataGridViewColumn)MyDataGridView.Columns[e.ColumnIndex];
            if (sortColumn.DataPropertyName.Equals("count"))
            {
                //排序切换123
                sort = (sort + 1) > 3 ? 1 : (sort + 1);
                string[] stxt = {"添加顺序","常用软件","应用名"};
                sortColumn.ToolTipText = "当前排序规则：\n-->" + stxt[sort - 1] + "\n下一个排序规则:\n-->" + stxt[(sort + 1) > 3 ? 0 : sort];
                ListSort(filterList, sort, true);
                ListSort(appList,sort,true);
                showList = new BindingList<MyApp>(filterList);
                this.MyDataGridView.DataSource = showList;
            }
        }

        //List<MyApp>排序方法
        private void ListSort(List<MyApp> list, int option, bool reset)
        {
            if (option != 1 && option != 2 && option != 3) 
                option = 1;
            //排序
            int a = 0, b = -1, c = 1;
            if (option == 1)
            {    //方式一 按id
                //先后增加顺序
                list.Sort((x, y) =>
                {
                    return x.Id > y.Id ? c : b;
                });
            }
            else if(option==2) {
                //方式二 按click
                list.Sort((x, y) =>
                {
                    if (x.Click == y.Click)
                    {
                        return x.Id > y.Id ? b : c;
                    }
                    else if (x.Click > y.Click)
                        return b;
                    else return c;
                });
            }
            else if (option == 3) {
                //方式三 按应用名
                list.Sort((x, y) =>
                {
                    //伟神算法
                    return BeanUtil.getMySortStr(x.Name) > BeanUtil.getMySortStr(y.Name) ? a : b;
                });
            }
            //序号重置
            if (reset)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Count = i + 1;
                }
            }
        }
        #endregion

    }
}

//----伟神