using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class EmployeeServers
    {
        public string Sname { get; set; }
        public int Salary { get; set; }
        public void Employee(string name,int salary)
        {
            this.Sname = name;
            this.Salary = salary;
        }
        public static bool Compare(EmployeeServers e1,EmployeeServers e2)
        {
            if (e1.Salary > e2.Salary) return true;
            else return false;
        }
    }
}
