using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class BuilName
    {
        public static string BuildPhotoName(string fileName)
        {
            //产生名称
            string photoName = DateTime.Now.ToString("yyyyMMddHHmmss");
            //生成最后两位的随机数
            Random objrandom = new Random();
            photoName += objrandom.Next(0, 99).ToString("00");
            //添加原先上传文件的类型
            photoName += fileName.Substring(fileName.Length - 4);
            return photoName;
        }
    }
}
