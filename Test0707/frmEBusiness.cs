using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using IronPython.Hosting;//导入IronPython库文件
using Microsoft.Scripting.Hosting;//导入微软脚本解释库文件


namespace Test0707
{
    public partial class frmEBusiness : Form
    {
        MySqlDataAdapter daTaoBao;
        DataSet dsTaoBao;
        public frmEBusiness()//无参构造方法
        {
            InitializeComponent();
        }
        //【01】初始化，加载数据库中数据
        private void frmEBusiness_Load(object sender, EventArgs e)
        {
            string connStr = "server=localhost;user=root;database=product;port=3306;password=";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                string sql = "SELECT * FROM tbsofa";
                daTaoBao = new MySqlDataAdapter(sql, conn);
                MySqlCommandBuilder cb = new MySqlCommandBuilder(daTaoBao);
                dsTaoBao = new DataSet();
                //从表中填充数据
                daTaoBao.Fill(dsTaoBao,"tbsofa");
                dgvEBusiness.DataSource = dsTaoBao;//将数据集中的数据展示
                dgvEBusiness.DataMember = "tbsofa";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        //【02】关闭
        private void btnClose_Click(object sender, EventArgs e)
        {
            frmEBusiness frmEBusiness = null;
            this.Close();
        }
        /// <summary>
        /// 调用python脚本进行数据分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDataAnsys_Click(object sender, EventArgs e)
        {
        //    ScriptEngine pyEngine = Python.CreateEngine();//创建Python解释器对象
        //    dynamic py = pyEngine.ExecuteFile(@"SofaDataAnsys.py");//读取脚本文件
        //    //string dd = py.main(textBox1.Lines);//调用脚本文件中对应的函数
        }
    }
}
