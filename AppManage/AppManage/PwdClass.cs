using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace AppManage
{
    public class PwdClass
    {
        private string pwd;

        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }

        public static void read(ref string tempfilepwd,ref bool firstopen)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.Load(BeanUtil.XmlFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(BeanUtil.NullXmlFileErr(ex), "警告！");
                Application.ExitThread();
            }
            XmlNode xn = xd.DocumentElement;
            XmlNodeList xnl = xn.ChildNodes;
            XmlNode temp; XmlNodeList templist;
            foreach (XmlNode item1 in xnl) //遍历根节点的子节点
            {
                String text = item1.Name.Trim(); //获取子节点名称

                if ("Temp".Equals(text)) //查找到匹配的子节点  MyApp
                {
                    temp = item1;
                    templist = temp.ChildNodes;
                    XmlNode xnpwd = templist[0];
                    tempfilepwd = xnpwd.InnerText;
                    return;
                }
            }
            temp = xd.CreateElement("Temp");
            XmlNode tpwd = xd.CreateElement("pwd");
            tpwd.InnerText = "";
            temp.AppendChild(tpwd);
            xn.AppendChild(temp);
            xd.Save(BeanUtil.XmlFilePath);
            firstopen = true;
        }

        public static bool update(string pwd)
        {
            try
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(BeanUtil.XmlFilePath);
                XmlNode xn = xd.DocumentElement;
                XmlNodeList xnl = xn.ChildNodes;
                XmlNode temp; XmlNodeList templist;
                foreach (XmlNode item1 in xnl)
                {
                    String text = item1.Name.Trim(); 

                    if ("Temp".Equals(text)) //查找到匹配的子节点  MyApp
                    {
                        temp = item1;
                        templist = temp.ChildNodes;
                        XmlNode xnpwd = templist[0];
                        xnpwd.InnerText = pwd;
                        xd.Save(BeanUtil.XmlFilePath);
                        return true;
                    }
                }
                return false;
            }
            catch {
                return false;
            }
        }

        public static bool deletePwdNode() {
           return XmlDao.deleteBootNode("Temp");
        }

    }
}
