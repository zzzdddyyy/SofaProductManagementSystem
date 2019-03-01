using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _004委托
{
    class Program
    {
        public delegate int Expression(int a,int b);//自定义一个委托(与类同级，参数和返回值类型必须与调用委托的方法类型相同)
        static void Main(string[] args)
        {
            //调用委托,实例化
            Caculate(Div, 3, 4);
            Console.ReadKey();
        }
        static int Add(int a,int b)
        {
            return a + b;
        }
        static int Sub(int a, int b)
        {
            return a - b;
        }
        static int Mul(int a, int b)
        {
            return a * b;
        }
        static int Div(int a, int b)
        {
            return a / b;
        }
        static void Caculate(Expression ex,int a,int b)
        {
            Console.WriteLine(ex(a, b));
        }
    }
}
