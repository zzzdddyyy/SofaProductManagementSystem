using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _010_事件
{
    class Program
    {
        public delegate void MyDelegate();
        //public MyDelegate myDelegate;//声明一个委托类型的变量，作为类的成员
        public event MyDelegate myDelegate;//声明一个委托类型的变量，作为类的成员,加Event是声明一个事件
        static void Main(string[] args)
        {
            Program pro = new Program();
        }
    }
}
