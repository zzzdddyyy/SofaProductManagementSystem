using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Common;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class ProductServices
    {
        private DBConnect dbConnect = new DBConnect();//实例化数据库操作类
        //产品编码生成器
        public string GenerateCode()
        {
            string str1 = "SF10";
            string[] seatArr = { "01", "02", "03", "ZH" };
            int seatIndex = new Random().Next(seatArr.Length);
            string str2 = seatArr[seatIndex];
            string style = Convert.ToString(new Random().Next(99));
            string[] cornerArr = { "LZ", "RZ", "--"};
            string[] teaArr = { "MC","LC", "RC", "--" };
            string[] layArr = { "LT", "RT", "MT", "--" };
            string pcode = str1 + str2 + style + teaArr[new Random().Next(teaArr.Length)] + layArr[new Random().Next(layArr.Length)] +
                cornerArr[new Random().Next(cornerArr.Length)];
            return pcode;
        }
        /// <summary>
        /// 新建产品
        /// </summary>
        /// <param name="objSofa"></param>
        public void NewProduct(Sofa objSofa)
        {
            //连接数据库
            string insertSql = string.Format("INSERT INTO funsofa(pcode,pname,pstyle,pprize,frame,fillter,wrapper,backrest,handrail,footrest,seatbox,teatable," +
                "mid,lay,corner,engi_draw,rendergraph) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}')",
                objSofa.Pcode,objSofa.Pname,objSofa.Pstyle,objSofa.Pprize,objSofa.Frame,objSofa.Fillter,objSofa.Wrapper,objSofa.Backrest,objSofa.Handrail,
                objSofa.Footrest,objSofa.Seatbox,objSofa.Teatable,objSofa.Mid,objSofa.Lay,objSofa.Corner,objSofa.Engi_draw.Replace("\\","\\\\"),
                objSofa.Rendergraph.Replace("\\","\\\\"));
            try
            {
                dbConnect.Insert(insertSql);
            }
            catch (Exception ex)
            {
                throw(ex);
            }
        }
        /// <summary>
        /// 修改产品
        /// </summary>
        /// <param name="code"></param>
        /// <param name="objSofa"></param>
        public void ModifyProduct(string code,Sofa objSofa)
        {
            //删除原来的
            string deleteSql = string.Format("delete from funsofa where pcode ='{0}'", code);
            try
            {
                dbConnect.Delete(deleteSql);
            }
            catch (Exception ex)
            {
                throw(ex);
            }
            //传入现在值
            string alterSql = string.Format("INSERT INTO funsofa(pcode,pname,pstyle,pprize,frame,fillter,wrapper,backrest,handrail,footrest,seatbox,teatable," +
                "mid,lay,corner,engi_draw,rendergraph) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}')",
                objSofa.Pcode, objSofa.Pname, objSofa.Pstyle, objSofa.Pprize, objSofa.Frame, objSofa.Fillter, objSofa.Wrapper, objSofa.Backrest, objSofa.Handrail,
                objSofa.Footrest, objSofa.Seatbox, objSofa.Teatable, objSofa.Mid, objSofa.Lay, objSofa.Corner, objSofa.Engi_draw.Replace("\\","\\\\"),
                objSofa.Rendergraph.Replace("\\","\\\\"));
            try
            {
                dbConnect.Insert(alterSql);
            }
            catch (Exception ex)
            {
                throw(ex);
            }
        }
    }
}
