using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Models
{
    class Program
    {
        static int count1 = 0;
        static int count2 = 0;
        static void Main(string[] args)
        {
            int[] sortArray = new int[] { 3,1,2,9, 6, 89, 4, 66, 43, 12, 32, 56, 6, 0 };
            //Console.Write("do..while排序。。。。。。。。。\n");
            //Console.Write("排序前。。。。。。。。。\n");
            //foreach (int item in sortArray)
            //{
            //    Console.Write(item + ",");
            //}
            //Console.Write("\n");
            //Sorted(sortArray);
            //Console.Write("排序后。。。。。。。。。\n");
            //Console.WriteLine(count1);
            //foreach (int item in sortArray)
            //{
            //    Console.Write(item + ",");
            //}
            //Console.ReadKey();
            //Console.Write("冒泡排序。。。。。。。。。\n");
            //MaoPaoSort(sortArray);
            //Console.WriteLine(count2);
            //foreach (int item in sortArray)
            //{
            //    Console.Write(item + "*");
            //}
            //实例化
            EmployeeServers[] employee = new EmployeeServers[]
            {
  
            };
            
            
            Console.ReadKey();
        }
        static void Sorted(int[] sortArray)
        {
            bool swapped = true;
            do
            {
                swapped = false;
                for (int i = 0; i < sortArray.Length - 1; i++)
                {
                    if (sortArray[i] > sortArray[i + 1])
                    {
                        int temp = sortArray[i];
                        sortArray[i] = sortArray[i + 1];
                        sortArray[i + 1] = temp;
                        swapped = true;//如果每次遍历发生了交换，swapped就变为true
                        count1++;
                    }
                }
                } while (swapped);//当
            }
        static void MaoPaoSort(int[] Array)
        {
            for(int i = 0; i< Array.Length - 1; i++)
            {
                for(int j=0; j < Array.Length - 1 - i; j++)
                {
                    if (Array[j] > Array[j + 1])
                    {
                        Array[j] = Array[j] + Array[j + 1];
                        Array[j + 1] = Array[j] - Array[j + 1];
                        Array[j] = Array[j] - Array[j + 1];
                        count2++;
                    }
                }
            }
        }
        static void CommenSort<T>(T[] sortArray,Func<T,T,bool> compareMethod)
        {
            bool swapped = true;
            do
            {
                swapped = false;
                for (int i = 0; i < sortArray.Length - 1; i++)
                {
                    if (compareMethod(sortArray[i],sortArray[i+1]))
                    {
                        T temp = sortArray[i];
                        sortArray[i] = sortArray[i + 1];
                        sortArray[i + 1] = temp;
                        swapped = true;//如果每次遍历发生了交换，swapped就变为true
                        count1++;
                    }
                }
            } while (swapped);//当
        }
        }
    }
