using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test0707
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLogin());
        }
        //定义全局变量
        //【1】登录是否成功[
        public static bool isLogin = false;
        //【2】当前登录者
        public static string currentUser = null;
        //【3】定义沙发的所有信息明细,用于从数据库接收产品数据
        public static List<Sofa> objListSofa = null;
        //【4】定义靠背零部件信息
        public static List<Sofa> objListKB = null;
    }
}
