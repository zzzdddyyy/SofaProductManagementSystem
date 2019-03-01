using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _014_委托之烧水事件
{
    public class BoilWater
    {
        private int tempreature;
        public delegate void BoilHandler(int param);//声明委托，一个int参数，返回值为空
        public event BoilHandler boilHandlerEvent;//声明事件
        public void HotWater()
        {
            for (int i = 0; i < 101; i++)
            {
                tempreature = i;
                if (i > 95)
                {
                    if (boilHandlerEvent != null)//如果给事件注册了方法，就引用委托
                    {
                        boilHandlerEvent(tempreature);//引用委托,让烧水器可以使用报警器和显示器的方法，可以用来显示，或者报警
                    }
                }
            }
        }
    }
    public class ArmClock
    {
        public void Arm(int param)
        {
            Console.WriteLine("滴滴滴，水烧开了！！！/n");
        }
    }
    public class DisplayMsg
    {
        public  void Show(int param)
        {
            Console.WriteLine("现在的水温是【{0}】°", param);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //实例化一个热水器
            BoilWater hot = new BoilWater();
            //实例化一个报警器，一个显示器
            ArmClock armClock = new ArmClock();
            DisplayMsg displayMsg = new DisplayMsg();
            //为委托注册方法
            hot.boilHandlerEvent += armClock.Arm;
            hot.boilHandlerEvent += displayMsg.Show;
            hot.HotWater();
            Console.ReadKey();
        }
    }
}
