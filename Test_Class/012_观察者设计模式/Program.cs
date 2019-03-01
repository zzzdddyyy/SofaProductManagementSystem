using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _012_观察者设计模式
{
    class Cat
    {
        private string name;
        private string color;
        public Cat(string name,string color)//构造方法
        {
            this.name = name;
            this.color = color;
        }
        public void CatComing()
        {
            Console.WriteLine(color + "的猫" + name + "来抓老鼠了，喵喵喵。。。");
            //mouse1.MouseRun();
            //mouse2.MouseRun();
            if (catCome != null)
            {
                catCome();//事件不能在类的外部调用
            }
        }
        public event Action catCome;//声明委托,发布事件,
    }//猫类---被观察者类
    class Mouse
    {
        private string name;
        private string color;
        public Mouse(string name,string color,Cat cat)
        {
            this.name = name;
            this.color = color;
            cat.catCome += this.MouseRun;//把自身的逃跑方法注册进猫的委托内
        }
        public  void MouseRun()
        {
            Console.WriteLine("猫来了！" + color + "的老鼠" + name + "跑了。。。");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Cat cat = new Cat("加菲", "brown");
            Mouse mouse1 = new Mouse("米其", "black",cat);
            //cat.catCome += mouse1.MouseRun;
            Mouse mouse2 = new Mouse("缺耳朵", "pink",cat);
            //cat.catCome += mouse1.MouseRun;
            //cat.CatComing(mouse1,mouse2);//被观察者状态发生改变
            cat.CatComing();
            Console.ReadKey();
        }
    }
}
