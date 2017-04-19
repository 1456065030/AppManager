using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace AppManage
{
    public partial class InsertMultiForm : Form
    {
        public InsertMultiForm()
        {
            InitializeComponent();
        }

        int xb = 0; int oladheight = 60;
        List<MyApp> list = new List<MyApp>();
        List<AppType> typeList = new List<AppType>();
        List<GroupBox> group = new List<GroupBox>();
        string[] types = new string[0];

        private void InsertMultiForm_Load(object sender, EventArgs e)
        {
            typeList = BeanUtil.typeList;
            if (typeList == null) {
                typeList = new List<AppType>();
                MessageBox.Show("加载类型失败！");
            }


            types = new string[typeList.Count];
            if (typeList == null || typeList.Count<1)
            {
                types =new string[]{"未知"};
            }
            for (int i = 0; i < typeList.Count; i++)
            {
                types[i] = typeList[i].Name;
            }
            oladheight = this.Top;

            //加载拖进的文件
            try
            {
                if (BeanUtil.filepath == null) return;
                if (BeanUtil.filepath.Length < 1) return;
                loadFileCreateObj(BeanUtil.filepath);
                Show();
                this.Top = 40;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "拖文件读取错误"); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            list.Clear(); ConClear(); this.Height = 140;
            this.Top = oladheight;
            OpenFileDialog ope = new OpenFileDialog();
            ope.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            ope.Multiselect = true;
            ope.AutoUpgradeEnabled = true;
            ope.Title = "高级添加应用 --Eminent伟";
            DialogResult dia = ope.ShowDialog();
            if (dia == System.Windows.Forms.DialogResult.OK)
            {
                //ope.ValidateNames = true;
                loadFileCreateObj(ope.FileNames);
            }
            Show();
            this.Top = 40;

        }

        private void loadFileCreateObj(string[] Fullpath)
        {
            int count = 1;
            foreach (var item in Fullpath)
            {
                if (count > 8)
                {
                    label1.ForeColor = Color.Red;
                    break;
                }
                label1.ForeColor = Color.Gray;
                MyApp app = new MyApp();
                app.Path = item;
                app.Name = item.Substring(item.LastIndexOf('\\') + 1).Split('.')[0];
                app.Type = "未知";

                app.Type = BeanUtil.getFileTypeName(item);

                list.Add(app);
                count++;
            }
        }

        private void Show()
        {

            xb = 0; group.Clear();

            foreach (var item in list)
            {
                TianJia(item.Name, item.Type, item.Path);
            }
        }

        private void ConClear()
        {
            for (int i = 0; i < 6; i++)
            {
                foreach (var item in this.Controls)
                {
                    if (item is GroupBox)
                    {
                        GroupBox gb = item as GroupBox;
                        this.Controls.Remove(gb);
                    }
                }

            }

        }

        public void TianJia(string name, string temp, string temppath)
        {
            GroupBox g = new GroupBox();
            g.Text = "应用" + (xb + 1);
            g.Width = 400; g.Height = 86;
            g.Left = 25;
            g.Top = g.Height * xb + 20;

            Label la = new Label();
            la.Text = "应用名：";
            //size 53, 12  top 29 left 10
            la.Top = 29; la.Left = 10;
            la.Width = 53; la.Height = 12;
            g.Controls.Add(la);

            TextBox tname = new TextBox();
            tname.Text = name;
            //size 100, 21 len 50  locat 65, 26
            tname.Width = 100;
            tname.Height = 21;
            tname.MaxLength = 50;
            tname.Top = 26; tname.Left = 65;
            g.Controls.Add(tname);

            Label latype = new Label();
            latype.Text = "类型：";
            latype.Top = 29; latype.Left = 170;
            latype.AutoSize = true;
            g.Controls.Add(latype);

            ComboBox ctype = new ComboBox();
            //size 105, 20 loc 218, 26 DropDownList
            ctype.Width = 105; ctype.Height = 20;
            ctype.Left = 218; ctype.Top = 26;
            ctype.DropDownStyle = ComboBoxStyle.DropDownList;
            ctype.Items.AddRange(types);
            try
            {
                ctype.SelectedIndex = 0;
            }
            catch { }
            if (types != null)
            {
                for (int i = 0; i < types.Length; i++)
                {
                    if (temp.Equals(types[i])) {
                        ctype.SelectedIndex = i;
                        break;
                    }
                }
            }
            g.Controls.Add(ctype);

            Label la1 = new Label();
            la1.Text = "路径：";
            //loc 14, 59  size 41, 12  路径：
            la1.Top = 59; la1.Left = 14;
            la1.Width = 41; la1.Height = 12;
            g.Controls.Add(la1);

            Label tpath = new Label();
            tpath.Text = temppath;
            //loc 66, 59   路径  AutoSize true
            tpath.Top = 59; tpath.Left = 66;
            tpath.AutoSize = true;
            g.Controls.Add(tpath);

            group.Add(g);
            this.Controls.Add(g);
            this.Height = 250 + g.Height * xb + 24;
            if (this.Height > 600) this.Height = 600;
            xb++;


        }

        private void button3_Click(object sender, EventArgs e)
        {
            list.Clear(); int count = 0;
            foreach (GroupBox item in group)
            {
                MyApp app = new MyApp();
                foreach (var item1 in item.Controls)
                {
                    if (item1 is TextBox)
                    {
                        TextBox txt = item1 as TextBox;
                        app.Name = txt.Text;
                    }
                    if (item1 is ComboBox)
                    {
                        ComboBox cbo = item1 as ComboBox;
                        app.Type = cbo.SelectedItem.ToString();
                        app.TypeId = getTidByName(app.Type);
                    }
                    if (item1 is Label)
                    {
                        Label la = item1 as Label;
                        if (la.Text == "应用名：" || la.Text == "路径：" || la.Text == "类型：")
                            continue;
                        app.Path = la.Text;
                    }
                }

                bool cg=MyAppDao.add(app);
                if(cg)count++;
            }
            string mess = "共添加" + group.Count+"个，";
            if (count == group.Count) mess += "全部成功！";
            else mess += count+"个成功!"+(group.Count-count)+"个失败！";
            MessageBox.Show(mess);
            BeanUtil.update = true;
            this.Close();
        }

        //根据类型获取类型id
        private int getTidByName(string type)
        {
            int tid = 0;
            foreach (AppType item in typeList)
            {
                if (item.Name.Equals(type))
                {
                    tid = item.Id; break;
                }
            }
            return tid;
        }
    }
}
