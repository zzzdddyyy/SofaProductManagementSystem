using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel= Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Reflection;

namespace DAL
{
    public class ProductSearch
    {
        /// <summary>
        /// 按照产品编号查询产品
        /// </summary>
        /// <param name="code"></param>
        /// <param name="objListSofa"></param>
        /// <returns>符合产品编号的产品</returns>
        public List<Sofa> GetAllProductByCode(string code,List<Sofa> objListSofa)
        {
            List<Sofa> objListQuery = new List<Sofa>();
            foreach(Sofa item in objListSofa)
            {
                if (item.Pcode.StartsWith(code))
                {
                    objListQuery.Add
                        (new Sofa
                        {
                            Pcode = item.Pcode,
                            Id = item.Id,
                            Pname = item.Pname,
                            Pprize = item.Pprize,
                            Pstyle = item.Pstyle,
                            Fillter = item.Fillter,
                            Footrest = item.Footrest,
                            Frame = item.Frame,
                            Handrail = item.Handrail,
                            Seatbox = item.Seatbox,
                            Backrest=item.Backrest,
                            Corner=item.Corner,
                            Lay=item.Lay,
                            Mid=item.Mid,
                            Teatable=item.Teatable,
                            Wrapper=item.Wrapper,
                            Engi_draw=item.Engi_draw,
                            Rendergraph=item.Rendergraph,
                         }
                        );
                }
            }
            return objListQuery;
        }
        /// <summary>
        /// 按照产品名称查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="objListSofa"></param>
        /// <returns></returns>
        public List<Sofa> GetAllProductByName(string name, List<Sofa> objListSofa)
        {
            List<Sofa> objListQuery = new List<Sofa>();
            foreach (Sofa item in objListSofa)
            {
                if (item.Pname.StartsWith(name))
                {
                    objListQuery.Add
                        (new Sofa
                        {
                            Pcode = item.Pcode,
                            Id = item.Id,
                            Pname = item.Pname,
                            Pprize = item.Pprize,
                            Pstyle = item.Pstyle,
                            Fillter = item.Fillter,
                            Footrest = item.Footrest,
                            Frame = item.Frame,
                            Handrail = item.Handrail,
                            Seatbox = item.Seatbox,
                            Backrest = item.Backrest,
                            Corner = item.Corner,
                            Lay = item.Lay,
                            Mid = item.Mid,
                            Teatable = item.Teatable,
                            Wrapper = item.Wrapper,
                            Engi_draw = item.Engi_draw,
                            Rendergraph = item.Rendergraph,
                        }
                        );
                }
            }
            return objListQuery;
        }
        /// <summary>
        /// 按照产品风格查询
        /// </summary>
        /// <param name="style"></param>
        /// <param name="objListSofa"></param>
        /// <returns></returns>
        public List<Sofa> GetAllProductByStyle(string style, List<Sofa> objListSofa)
        {
            List<Sofa> objListQuery = new List<Sofa>();
            foreach (Sofa item in objListSofa)
            {
                if (item.Pstyle.StartsWith(style))
                {
                    objListQuery.Add
                        (new Sofa
                        {
                            Pcode = item.Pcode,
                            Id = item.Id,
                            Pname = item.Pname,
                            Pprize = item.Pprize,
                            Pstyle = item.Pstyle,
                            Fillter = item.Fillter,
                            Footrest = item.Footrest,
                            Frame = item.Frame,
                            Handrail = item.Handrail,
                            Seatbox = item.Seatbox,
                            Backrest = item.Backrest,
                            Corner = item.Corner,
                            Lay = item.Lay,
                            Mid = item.Mid,
                            Teatable = item.Teatable,
                            Wrapper = item.Wrapper,
                            Engi_draw = item.Engi_draw,
                            Rendergraph = item.Rendergraph,
                        }
                        );
                }
            }
            return objListQuery;
        }
        public List<Sofa> GetAllProductByUnion(string[] UnionSelect,List<Sofa> objListSofa)
        {
            List<Sofa> objListQuery = new List<Sofa>();
            //获取Sofa类有多少种属性
            PropertyInfo[] pis = typeof(Sofa).GetProperties();
            //定义接收Sofa属性的字符串
            string proParams = null;
            foreach (Sofa item in objListSofa)
            {
                proParams   = item.Pcode+ item.Pname+ item.Pstyle + item.Fillter + item.Footrest + item.Frame+ item.Handrail + item.Seatbox+
                   item.Backrest+ item.Corner+ item.Lay+ item.Mid+ item.Teatable+ item.Wrapper;
                if (IsContainAllString(UnionSelect, proParams))
                {
                    objListQuery.Add
                         (new Sofa
                         {
                             Pcode = item.Pcode,
                             Id = item.Id,
                             Pname = item.Pname,
                             Pprize = item.Pprize,
                             Pstyle = item.Pstyle,
                             Fillter = item.Fillter,
                             Footrest = item.Footrest,
                             Frame = item.Frame,
                             Handrail = item.Handrail,
                             Seatbox = item.Seatbox,
                             Backrest = item.Backrest,
                             Corner = item.Corner,
                             Lay = item.Lay,
                             Mid = item.Mid,
                             Teatable = item.Teatable,
                             Wrapper = item.Wrapper,
                             Engi_draw = item.Engi_draw,
                             Rendergraph = item.Rendergraph,
                         }
                         );
                }
            }
            return objListQuery;
        }
        /// <summary>
        /// 按照产品编号获取该编号的产品信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="objListSofa"></param>
        /// <returns></returns>
        public Sofa GetProductByCode(string code,List<Sofa> objListSofa)
        {
            Sofa objSofa = new Sofa();
            foreach(Sofa item in objListSofa)
            {
                if (item.Pcode.Equals(code))
                {
                    objSofa = new Sofa
                    {
                        Pcode = item.Pcode,
                        Id = item.Id,
                        Pname = item.Pname,
                        Pprize = item.Pprize,
                        Pstyle = item.Pstyle,
                        Fillter = item.Fillter,
                        Footrest = item.Footrest,
                        Frame = item.Frame,
                        Handrail = item.Handrail,
                        Seatbox = item.Seatbox,
                        Backrest = item.Backrest,
                        Corner = item.Corner,
                        Lay = item.Lay,
                        Mid = item.Mid,
                        Teatable = item.Teatable,
                        Wrapper = item.Wrapper,
                        Engi_draw = item.Engi_draw,
                        Rendergraph = item.Rendergraph,
                    };
                    break;
                }
            }
            return objSofa;
        }
        /// <summary>
        ///判断字符串中是否包含数组元素
        /// </summary>
        /// <param name="Selects"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool IsContainAllString(string[] Selects,string str)
        {
            foreach (string item in Selects)
            {
                if (!str.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
