using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonProtocol
{
    [Serializable]//可序列化
    public class SofaProtocal
    {
        //模块
        public int model;
        //操作
        public int operate;
        //数据
        public object data;
    }
}
