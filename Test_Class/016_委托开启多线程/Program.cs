using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _016_委托开启多线程
{
    class Program
    {
        static int Test(int i,string str)
        {
            Console.WriteLine("test,"+i+str);
            Thread.Sleep(500);//让当前线程休眠（中断）0.1S
            return 100;
        }
        static void Main(string[] args)
        {
            //在main()线程中执行，一个线程里面的语句是从上到下的
            //1、通过委托，开启一个线程
            Func<int, string, int> a = Test;
            IAsyncResult ar = a.BeginInvoke(199, "张三丰", null, null);//开启一个新的线程去执行a 所引用的方法
            //IAsyncResult可以    取得当前线程的状态
            //可以认为线程是同时执行的（异步执行）
            Console.WriteLine("Main");

            //检测线程结束方法一
            //while (ar.IsCompleted==false)//如果当前线程没有执行完毕
            //{
            //    Console.WriteLine("*——*");
            //    Thread.Sleep(100);//主线程休眠0.01s，再去查询a线程的执行状态；控制子线程的检测频率
            //}
            //int res=a.EndInvoke(ar);//取得test异步线程的返回值
            //Console.WriteLine(res);

            //检测线程结束方法二
            //bool isEnd=ar.AsyncWaitHandle.WaitOne(1000);//1S表示超时时间，如果等待了1S线程还没有结束，就会返回false，若果1s内结束了，这个方法会返回true
            //if (isEnd)
            //{
            //    int res = a.EndInvoke(ar);
            //    Console.WriteLine("*——*");
            //    Console.WriteLine(res);
            //}
            //else
            //{
            //    Console.WriteLine("超时");
            //}

            //检测线程结束方法三：通过回调，检测线程结束
            Func<int, string, int> aa= Test;
            //倒数第二个参数是一个委托类型的参数，表示回调函数，当线程结束时就会调用这个委托指向的方法
            //倒数第一个参数用来给回调函数传递参数
            IAsyncResult ar1 = a.BeginInvoke(199, "张三丰", OnCallBack, aa);//开启一个新的线程去执行a 所引用的方法
            Console.WriteLine("结束");
            Console.ReadKey();
        }
        static void OnCallBack(IAsyncResult ar1)
        {
            Func<int, string, int> aa = ar1.AsyncState as Func<int, string, int>;
            int res = aa.EndInvoke(ar1);
            Console.WriteLine(res + "在回调函数中取得结果");
        }
    }
}
