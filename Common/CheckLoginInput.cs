using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common
{
    public class CheckLoginInput
    {
        //判断工号有效性
        public static bool IsEmployeeNum(string str)
        {
            //9位数字
            Regex objRegex = new Regex(@"^[0-9]{9}$");
            return objRegex.IsMatch(str);
        }
        //判断用户名有效性(汉字)
        public static bool IsUserName(string str)
        {
            Regex objRegex = new Regex(@"^[\u4e00-\u9fa5]{0,}$");
            return objRegex.IsMatch(str);
        }
        public static bool IsPrize(string str)
        {
            //带两位小数
            Regex objRegex = new Regex(@"^([1-9]\d{0,9}|0)(\.\d{1,2})?$");
            return objRegex.IsMatch(str);
        }
        //判断邮箱：
        public static bool IsEmail(string str)
        {
            Regex reg = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            return reg.IsMatch(str);
        }
        //判断由字母和数字组成的密码
        public static bool IsPwd(string str)
        {
            Regex reg = new Regex(@"^[A-Za-z0-9]+$");
            return reg.IsMatch(str);
        }
    }
}
