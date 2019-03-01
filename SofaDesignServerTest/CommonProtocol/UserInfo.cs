using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonProtocol
{

    [Serializable]//可序列化
    public class UserInfo
    {
        public string Name { get; set; }
        public int JobID { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Pwd { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
    }
}
