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
    public partial class UpdateTypeForm : Form
    {
        public UpdateTypeForm()
        {
            InitializeComponent();
        }


        List<AppType> saveList = new List<AppType>();
        List<AppType> typeList = new List<AppType>();
        List<Panel> list = new List<Panel>();
        List<int> delList=new List<int>();
        int count = 0; int maxId = 1;
        private void UpdateTypeForm_Load(object sender, EventArgs e)
        {
            saveList=AppTypeDao.read();
            foreach (AppType item in saveList)
            {
                typeList.Add(item);
            }
            maxId = AppTypeDao.getMaxId();
            show();
        }



        private void createPanel(AppType type) {
            Panel p = new Panel();
            p.Width = 240;
            p.Height = 42;
            p.Left = 10;
            p.Top = p.Height * count + 12*(count+1);
            p.BorderStyle = BorderStyle.FixedSingle;

            Label la = new Label();
            la.Text = type.Id.ToString();
            la.Top = 15; la.Left = 15;
            la.AutoSize = true;
            p.Controls.Add(la);

            TextBox tname = new TextBox();
            tname.Text = type.Name;
            tname.TextAlign = HorizontalAlignment.Center;
            tname.Width = 100;
            tname.Height = 21;
            tname.MaxLength = 10;
            tname.Top = 11; tname.Left = 47;
            p.Controls.Add(tname);

            Button b = new Button();
            b.Text = "删除";
            b.Name = type.Id.ToString();
            b.Width = 51;
            b.Height = 23;
            b.Top = 10; b.Left = 171;
            b.Click += new System.EventHandler(this.MyButton_Click);
            p.Controls.Add(b);

            list.Add(p);

            count++;
            
        }

        private void show() {
            this.panel1.Controls.Clear();
            list.Clear();
            count = 0;
            foreach (AppType item in typeList)
            {
                this.createPanel(item);
            }

            foreach (Panel item in list)
            {
                this.panel1.Controls.Add(item);
            }
            LinkLabel lab = new LinkLabel();
            lab.Text = "增加一个"; lab.Left = 95;
            lab.Width = 53;lab.Height=12;
            lab.Click += new System.EventHandler(this.MyLinkLabel_Click);
            lab.Top =42 * count + 12 * (count + 1);
            this.panel1.Controls.Add(lab);

            Button b = new Button();
            b.Text = "保存";
            b.Width = 75; b.Height = 23;
            b.Top = 50 * count + 12 * (count + 1);
            if (b.Top < 257) b.Top = 257;
            b.Left = 159;
            b.Click += new System.EventHandler(this.SaveButton_Click);
            this.panel1.Controls.Add(b);

        }

        private void MyButton_Click(object sender, EventArgs e) {
            Update();
            Button b = (Button)sender;
            int id=int.Parse(b.Name);
            foreach (AppType item in typeList)
            {
                if (item.Id == id) {
                    delList.Add(id);
                    saveList.Remove(item);
                    typeList.Remove(item);
                    break;
                }
            }
            show();
        }

        private void Update() {
            foreach (var item1 in this.panel1.Controls)
            {
                if (item1 is Panel) {
                    Panel pan=item1 as Panel;
                    AppType type=new AppType();
                    foreach (var item2 in pan.Controls )
                    {
                        try
                        {
                            if (item2 is Label)
                            {
                                type.Id = int.Parse((item2 as Label).Text);
                            }
                            if (item2 is TextBox)
                            {
                                type.Name = (item2 as TextBox).Text;
                            }
                        }
                        catch { type = null; break; }
                    }
                    if (type == null) continue;
                    foreach (AppType item in typeList)
                    {
                        if (type.Id == item.Id)
                        {
                            item.Name = type.Name; break;
                        }
                    }
                }
            }
        }

        private void MyLinkLabel_Click(object sender, EventArgs e)
        {
            Update();
            typeList.Add(new AppType(++maxId,"新添加"));
            show();
        }


        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认保存吗？", "提示", MessageBoxButtons.OKCancel) == DialogResult.Cancel) {
                BeanUtil.update = false;
                this.Close();
                return;
            }
            this.Update();
            BeanUtil.update = true;
            try
            {
                foreach (int item in delList)
                {
                    AppTypeDao.delete(item);
                }

                foreach (AppType item1 in typeList)
                {
                    int i = 0; 
                    foreach (AppType item2 in saveList)
                    {
                        if (item1.Id == item2.Id)
                        {
                            AppTypeDao.update(item1);
                            i = 1;
                            break;
                        }
                    }
                    if (i == 0)
                    {
                        item1.Id = 0;
                        AppTypeDao.add(item1);
                    }
                }
            }
            catch { MessageBox.Show("修改成功,但有错误！"); this.Close(); }
            MessageBox.Show("保存成功！"); this.Close();
        }


    }
}
