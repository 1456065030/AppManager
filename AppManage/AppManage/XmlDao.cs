using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace AppManage
{
    public class XmlDao
    {
        //加载xml
        private static XmlDocument load()
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.Load(BeanUtil.XmlFilePath);
            }
            catch(Exception e){
                MessageBox.Show(BeanUtil.NullXmlFileErr(e), "错误！");
                return null;
            }
            return xd;
        }
        //保存
        private static bool save(XmlDocument xd) {
            try
            {
                xd.Save(BeanUtil.XmlFilePath);
            }
            catch (Exception e)
            {
                MessageBox.Show(BeanUtil.NullXmlFileErr(e), "错误！");
                return false;
            }
            return true;
        }

        //读取全部
        public static List<object[]> read(String nodeName)
        {
            List<object[]> list = new List<object[]>();

            XmlDocument xd = load();
            if (xd == null) return null;

            XmlNode node = xd.DocumentElement;//根节点 AppManage

            XmlNodeList nodeList1 = node.ChildNodes;

            foreach (XmlNode item1 in nodeList1) //遍历根节点的子节点
            {
                String text = item1.Name.Trim(); //获取子节点名称

                if (nodeName.Equals(text)) //查找到匹配的子节点  MyApp
                {
                    XmlNodeList nodeList2 = item1.ChildNodes;

                    foreach (XmlNode item2 in nodeList2) //App
                    {
                        XmlNodeList nodeList3 = item2.ChildNodes;

                        object[] obj = new object[nodeList3.Count];

                        for (int i = 0; i < nodeList3.Count; i++)
                        {
                            XmlNode item3 = nodeList3[i];
                            obj[i] = item3.InnerText;
                        }

                        list.Add(obj);
                    }

                }
            }

            return list;
        }

        //增加
        public static bool add(String nodeName, String name, Dictionary<string, string> dic)
        {
            XmlDocument xd = load();
            if (xd == null) return false;

            XmlNode gnode = xd.DocumentElement;

            //创建节点
            XmlNode node = xd.CreateElement(name);
            foreach (string item in dic.Keys)
            {
                string value = dic[item];
                XmlNode n = xd.CreateElement(item);
                n.InnerText = value;
                node.AppendChild(n);
            }

            //往匹配节点添加新创建的节点
            XmlNodeList nodeList1 = gnode.ChildNodes;
            foreach (XmlNode item1 in nodeList1) 
            {
                String text = item1.Name.Trim();

                if (nodeName.Equals(text)) 
                {
                    item1.AppendChild(node);
                }
            }
            //保存
            return save(xd);
        }

        //创建跟节点
        public static bool createBootNode(string name) {
            XmlDocument xd = load();
            if (xd == null) return false;

            XmlNode gnode = xd.DocumentElement;
            XmlNodeList list = gnode.ChildNodes;
            foreach (XmlNode item in list)
            {
                String text = item.Name.Trim();
                if (name.Equals(text))
                {
                    return false;
                }
            }
            //创建节点
            XmlNode node = xd.CreateElement(name);
            gnode.AppendChild(node);
            //保存
            return save(xd);
        }

        //删除根节点
        public static bool deleteBootNode(string name) {
            XmlDocument xd = load();
            if (xd == null) return false;
            bool notdel = true;
            XmlNode gnode = xd.DocumentElement;
            XmlNodeList list = gnode.ChildNodes;
            foreach (XmlNode item in list)
            {
                String text = item.Name.Trim();
                if (name.Equals(text))
                {
                    item.ParentNode.RemoveChild(item);
                    notdel = false;
                    break;
                }
            }
            if (notdel)
                return false;
            //保存
            return save(xd);
        }

        //修改
        public static bool update(String nodeName, int id, Dictionary<string, string> dic)
        {
            XmlDocument xd = load();
            if (xd == null) return false;

            XmlNode gnode = xd.DocumentElement;
            XmlNodeList nodeList1 = gnode.ChildNodes;
            foreach (XmlNode item1 in nodeList1)
            {
                String text = item1.Name.Trim();

                if (nodeName.Equals(text))
                {
                    XmlNodeList nodeList2 = item1.ChildNodes;
                    foreach (XmlNode item2 in nodeList2)
                    {
                        XmlNodeList nodeList3 = item2.ChildNodes;//App
                        string idstr=nodeList3[0].InnerText;
                        try
                        {
                            if (int.Parse(idstr) == id)
                            {
                                foreach (string key in dic.Keys)
                                {
                                    string value = dic[key];

                                    foreach (XmlNode item3 in nodeList3)
                                    {
                                        if (item3.Name.Trim().Equals(key)) {
                                            item3.InnerText = value; break;
                                        }
                                    }
                                }
                                break;
                            }
                            else continue;
                        }
                        catch { continue; }
                    }
                }
            }

            return save(xd);
        }

        //修改，如果没有找到匹配的子元素就创建
        public static bool updateAndCreate(String nodeName, int id, Dictionary<string, string> dic)
        {
            XmlDocument xd = load();
            if (xd == null) return false;

            XmlNode gnode = xd.DocumentElement;
            XmlNodeList nodeList1 = gnode.ChildNodes;
            foreach (XmlNode item1 in nodeList1)
            {
                String text = item1.Name.Trim();

                if (nodeName.Equals(text))
                {
                    XmlNodeList nodeList2 = item1.ChildNodes;
                    foreach (XmlNode item2 in nodeList2)
                    {
                        XmlNodeList nodeList3 = item2.ChildNodes;
                        string idstr = nodeList3[0].InnerText;
                        try
                        {
                            if (int.Parse(idstr) == id)
                            {
                                foreach (string key in dic.Keys)
                                {
                                    string value = dic[key];

                                    for (int i = 0; i < nodeList3.Count; i++)
                                    {
                                        XmlNode item3 = nodeList3[i];
                                        if (item3.Name.Trim().Equals(key))
                                        {
                                            //自增
                                            if (value.Equals("++")) {
                                                try
                                                {
                                                    value = (int.Parse(item3.InnerText.Trim()) + 1).ToString();
                                                }
                                                catch { value = "1"; }
                                            }
                                            item3.InnerText = value; 
                                            break;
                                        }
                                        else if (i >= nodeList3.Count- 1) {
                                            XmlNode node = xd.CreateElement(key);
                                            if (value.Equals("++"))
                                                node.InnerText = "1";
                                            else
                                                node.InnerText = value;
                                            item2.AppendChild(node);
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                            else continue;
                        }
                        catch { continue; }
                    }
                }
            }
            return save(xd);
        }

        //删除
        public static bool delete(String nodeName, int id)
        {
            XmlDocument xd = load();
            if (xd == null) return false;

            XmlNode gnode = xd.DocumentElement;
            XmlNodeList nodeList1 = gnode.ChildNodes;
            foreach (XmlNode item1 in nodeList1)
            {
                String text = item1.Name.Trim();

                if (nodeName.Equals(text))
                {
                    XmlNodeList nodeList2 = item1.ChildNodes;
                    foreach (XmlNode item2 in nodeList2)
                    {
                        XmlNodeList nodeList3 = item2.ChildNodes;//App
                        string idstr = nodeList3[0].InnerText;
                        try
                        {
                            if (int.Parse(idstr) == id)
                            {
                                item2.ParentNode.RemoveChild(item2);
                                break;
                            }
                            else continue;
                        }
                        catch { continue; }
                    }
                }
            }

            return save(xd);
        }
        //获取最大id
        public static int getMaxId(String nodeName)
        {
            XmlDocument xd = load();
            if (xd == null) return 0;
            int max = 0;
            XmlNode gnode = xd.DocumentElement;
            XmlNodeList nodeList1 = gnode.ChildNodes;
            foreach (XmlNode item1 in nodeList1)
            {
                String text = item1.Name.Trim();

                if (nodeName.Equals(text))
                {
                    XmlNodeList nodeList2 = item1.ChildNodes;
                    foreach (XmlNode item2 in nodeList2)
                    {
                        XmlNodeList nodeList3 = item2.ChildNodes;//App
                        string idstr = nodeList3[0].InnerText;
                        try
                        {
                            int id = int.Parse(idstr);
                            if (id > max) max = id;
                        }
                        catch { continue; }
                    }
                }
            }
            return max;
        }
    }
}

/*
 
XML规定结构：
 
 <AppManage>

	<MyApp>
		<App>
		  <id></id>
		  <name></name>
		  <type></type>
		  <path></path>
		  <pwd></pwd>
		</App>
	</MyApp>

	<AppType>
		<Type>
			<id></id>
			<name></name>
		</Type>
	</AppType>
    
    <xxx>
        <xxx>
            <xxx>...</xxx>
            <xxx>...</xxx>
                .....
        </xxx>
    </xxx>
</AppManage>
 
 */
