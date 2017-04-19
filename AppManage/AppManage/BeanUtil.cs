using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace AppManage
{
    public class BeanUtil
    {

        #region 静态变量
        public static string XmlFilePath = "MyAppFile.xml";

        public static List<MyApp> appList = new List<MyApp>();

        //窗体传值对象
        public static MyApp app = null;
        public static List<AppType> typeList = new List<AppType>();
        //密码传值
        public static string filepwd;
        public static bool truepwd = false;
        //是否进行操作
        public static bool update = false;
        //拖文件传值
        public static string[] filepath = null;
        //修改自定义删除为其他
        public static string zidingyiName=null;
        //排序
        public static int sort = 1;
        public static string zcm = "1456065030";
        #endregion

        #region 打开应用的方法

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "MoveWindow")]
        public static extern bool MoveWindow(System.IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        public static void OpenAndSetWindow(String fileName)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = fileName;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                if (fileName != null && fileName.IndexOf(":\\") != -1 && fileName.IndexOf(".") != -1)
                {
                    try
                    {
                        p.StartInfo.WorkingDirectory = fileName.Substring(0, fileName.LastIndexOf("\\"));
                    }
                    catch { }
                }
                p.Start();
            }
            catch(Exception e) {
                System.Windows.Forms.MessageBox.Show(e.Message,"错误");
            }
        }

        public string OpenCMD(string name, string input, bool v)
        {
            Process p = new Process();
            p.StartInfo.FileName = name;// "cmd.exe";//要执行的程序名称 
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;//可能接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.CreateNoWindow = v;//不显示程序窗口 
            p.Start();//启动程序 
            //向CMD窗口发送输入信息： 
            p.StandardInput.WriteLine(input);
            //获取CMD窗口的输出信息： 
            string output = p.StandardOutput.ReadToEnd();
            p.Close();
            return output;
        }
        #endregion

        #region [颜色：16进制转成RGB]
        /// <summary>
        /// [颜色：16进制转成RGB]
        /// </summary>
        /// <param name="strColor">设置16进制颜色 [返回RGB]</param>
        /// <returns></returns>
        public static Color colorHx16toRGB(string strHxColor)
        {
            try
            {
                if (strHxColor.Length == 0)
                {//如果为空
                    return Color.FromArgb(0, 0, 0);//设为黑色
                }
                else
                {//转换颜色
                    return Color.FromArgb(System.Int32.Parse(strHxColor.Substring(1, 2), System.Globalization.NumberStyles.AllowHexSpecifier), System.Int32.Parse(strHxColor.Substring(3, 2), System.Globalization.NumberStyles.AllowHexSpecifier), System.Int32.Parse(strHxColor.Substring(5, 2), System.Globalization.NumberStyles.AllowHexSpecifier));
                }
            }
            catch
            {//设为黑色
                return Color.FromArgb(0, 0, 0);
            }
        }
        #endregion

        #region [颜色：RGB转成16进制]
        /// <summary>
        /// [颜色：RGB转成16进制]
        /// </summary>
        /// <param name="R">红 int</param>
        /// <param name="G">绿 int</param>
        /// <param name="B">蓝 int</param>
        /// <returns></returns>
        public static string colorRGBtoHx16(int R, int G, int B)
        {
            return ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(R, G, B));
        }
        #endregion

        #region 根据路径获取文件类型
        public static string getFileTypeName(string filepath)
        {
            string txt = null;
            string type = "应用";
            try
            {
                txt = "(游戏|英雄联盟|game|qq飞车|tgp|client.exe|desktoptips.exe|腾讯游戏|qq游戏|cstrike.exe|地下|穿越|战地|侠盗|单机|网游|页游)";
                if (Regex.IsMatch(filepath.Trim().ToLower(), txt))
                {
                    type = "游戏";
                }

                txt = ".(pptx|mp4|jpg|png|txt|xls|doc|mp3|ppt|视频)$";
                if (Regex.IsMatch(filepath.Trim().ToLower(), txt))
                {
                    type = "其他";
                }
                txt = "(学习|学习资料|语文|数学|英语|教学)";
                if (Regex.IsMatch(filepath.Trim().ToLower(), txt))
                {
                    type = "学习";
                }
                
            }
            catch { }
            return type;
        }
        #endregion

        #region 获取首字母-(小写)

        public static string GetChineseSpell(string strText)
        {
            int len = strText.Length;
            string myStr = "";
            for (int i = 0; i < len; i++)
            {
                myStr += getSpell(strText.Substring(i, 1));
            }
            return myStr;
        }

        private static string getSpell(string cnChar)
        {
            byte[] arrCN = Encoding.Default.GetBytes(cnChar);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(97 + i) });
                    }
                }
                return "*";
            }
            else return cnChar;
        }
        #endregion

        #region 随机颜色
        static string[] fontColor = { "#8CCCFA", "#07BAA4", "#FF0000", "#00FF00", "#275566", "#9FCEFE", "#841010", "#56AC20", "#4B1590", "#175CA1", "#A0BF00", "#FF0000", "#00FF00", "#7636F9", "#7C200F", "#4A3E6A", "#9B7F9C", "#037725", "#827218", "#0E299A", "#144E35", "#E46B36", "#28542C", "#ED0D6B", "#EF61DB", "#CD040F", "#2602E8", "#4F548B", "#3A1F04", "#150603", "#1C378F", "#405075", "#E0565C", "#123300", "#325006", "#41752B", "#485F75", "#FF0000", "#00FF00", "#00FA00", "#BACE01", "#FD142B" };
        public static Color RandomColor()
        {
            Random ra = new Random();
            int index = ra.Next(fontColor.Length - 1);
            return colorHx16toRGB(fontColor[index]);
        }
        #endregion

        #region other

        //伟神字符串排序算法
        public static double getMySortStr(string strText)
        {
            int len = strText.Length;
            double myCount = 0;
            for (int i = 0; i < len; i++)
            {
                try
                {
                    myCount += ((int)getSpell(strText.Substring(i, 1))[0]) * 1000 / Math.Pow(10000, i);
                }
                catch { continue; }
            }
            return myCount;
        }

        //字符串是否为空
        public static bool isNull(String name)
        {
            if (name == null) return true;
            if (name.Trim().Equals("")) return true;
            return false;
        }

        //xml加载错误信息
        public static string NullXmlFileErr(Exception e) {
            string mess = "";
            mess += "错误来源（文件丢失？）：\n1>." + e.Message + "\n2>.找不到" + BeanUtil.XmlFilePath + "文件或" + BeanUtil.XmlFilePath + "和该应用不在同一个文件夹";
            mess += "\n\n解决方案：\n";
            mess += "请把" + BeanUtil.XmlFilePath + "文件和该应用放到同一个文件夹\n";
            mess += "如果文件丢失请重新下载或联系QQ：1456065030\n";
            mess += "\n           ---Eminent伟";
            return mess;
        }
        #endregion

    }
}
