using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _010_lambda表达式
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<int, int, int> func = (arg1, arg2) =>
              {
                  return arg1 + arg2;
              };
            Func<int, int> test1 = a => a + 1;//如果lambda表达式只有一个参数，在方法快内就可以不适用花括号，也可以不用返回值，
            //如果需要返回值，C#会自动return
            int res1 = test1(4);
            int res = func(1, 2);
            Console.WriteLine(res);
            Console.WriteLine(res1);
            Console.ReadKey();
        }
    }
}
