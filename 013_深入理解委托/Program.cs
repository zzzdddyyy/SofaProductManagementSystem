using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _013_深入理解委托
{
    class Program
    {
        //首先定义一个问候的委托
        public delegate void GreetLanguge(string name);
        static void Main(string[] args)
        {
            GreetLanguge gt1, gt2;
            gt1 = EnglishGreet;
            gt2 = ChineseGreet;
            //GreetPeople("Lily");
            //GreetPeople("张思德");
            GreetPeople(",Frank", gt1);
            GreetPeople(",王二虎", gt2);
            Console.ReadKey();
        }
        public static void GreetPeople(string name,GreetLanguge makeGreet)//把委托当成方法的一个参数传递引用,makeGreet的类型是委托GreetLanguage。
        {
            //EnglishGreet(name);
            //ChineseGreet(name);
            //如果全球的语言都在这里，要依次调用n个函数来执行XXX语言GReet，
            //现在想用一个方法，代表各种语言的问候，-----委托一个方法，让它实现此功能
            //首先定义一个问候的委托，委托是类级别的，定时与类同级
            
            makeGreet(name);//makeGreet本身也是一个方法，现在需要实例化-->调用本方法的参数作为委托的参数
        }
        public static void EnglishGreet(string name)
        {
            Console.WriteLine("Good Morning," + name);
        }
        public static void ChineseGreet(string name)
        {
            Console.WriteLine("早上好," + name);
        }
    }
}
