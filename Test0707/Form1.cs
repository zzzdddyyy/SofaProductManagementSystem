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
using MySQLDriverCS;
using DAL;
using Models;
using Excel= Microsoft.Office.Interop.Excel;
using System.IO;
using System.Reflection;

namespace Test0707
{
    public partial class Form1 : Form
    {
        List<Sofa> objListQuery = new List<Sofa>();//存放查询结果
        private DBConnect dbConnect = new DBConnect();//实例化数据库操作类
        private ProductSearch productSearch = new ProductSearch();//实例化产品查询类
        private Sofa sofa = new Sofa();
        //MySqlDataAdapter daFunSofa;
        //DataSet dsFunSofa;
        public Form1()
        {
            InitializeComponent();
            //判断用户是否为管理员,可操作用户管理
            if (Program.currentUser == "000000000")
            {
                UserManage.Enabled = true;
            }
            else UserManage.Enabled = false;
        }


        //【控件方法】
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        //【1】结束所有进程
        private void button2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“zdy_productDataSet.funsofa”中。您可以根据需要移动或删除它。
            //this.funsofaTableAdapter.Fill(this.zdy_productDataSet.funsofa);
            //初始化使用者
            if (Program.currentUser == null)
            {
                lblUser.Text = null;
            }
            else
            {
                lblUser.Text = Program.currentUser;
            }
            //lblInfo.Text = "请先从数据库中导入数据......";
            //设置DataGridView属性(第一行，第一列可操作)
            DataGridViewReadOnlyFalse(dataGridView1, 0);
            DataGridViewRowReadOnlyFalse(dataGridView1, 0);
            tbcCompones.Visible = false;
        }
        //【2】TreeView选择控件
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //【01】显示功能沙发资料
            if (treeView1.SelectedNode.Text =="功能沙发")
            {
                tbcCompones.Visible = false;
                string sql = "SELECT * FROM funsofa ";
                try
                {
                   //把数据绑定到全局变量沙发信息上
                    Program.objListSofa = dbConnect.Select(sql);
                    dataGridView1.DataSource = null;
                    dataGridView1.AutoGenerateColumns = false;
                    dataGridView1.DataSource = Program.objListSofa;
                    lblChooseInfo.Text = "导入数据成功！当前数据库中一共有【" + dataGridView1.Rows.Count + "】条产品信息。";
                }
                catch (Exception ex)
                {

                   MessageBox.Show(ex.Message);
                }
            }
            //【02】显示木质框架资料
            else if (treeView1.SelectedNode.Text == "木质框架")
            {
                tbcCompones.Visible = true;
                string sql = "SELECT * FROM funsofa ";
                try
                {
                    //把数据绑定到全局变量沙发信息上
                    Program.objListSofa = dbConnect.Select(sql);
                    dgvZJ.DataSource = null;
                    dgvZJ.AutoGenerateColumns = false;
                    dgvZJ.DataSource = Program.objListSofa;
                    lblChooseInfo.Text = "导入数据成功！当前数据库中一共有【" + dataGridView1.Rows.Count + "】条产品信息。";
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            //【03】显示功能沙发资料
            //【04】显示功能沙发资料
        }
        //【3】全选
        private void btnChooseAll_Click(object sender, EventArgs e)
        {
            int count = dataGridView1.Rows.Count;
            //如果按钮==“全选”，全选
            if (btnChooseAll.Text=="全  选")
            {
                for (int i = 0; i < count; i++)
                {
                    DataGridViewCheckBoxCell dgvCheckBox = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells[0];
                    Boolean flag = Convert.ToBoolean(dgvCheckBox.Value);
                    if (!flag)
                    {
                        dgvCheckBox.Value = true;
                    }
                    else continue;
                    btnChooseAll.Text = "取消全选";
                    lblChooseInfo.Text = "一共选中了【" + dataGridView1.Rows.Count + "】条信息。。。";
                }
            }
            //若果选中，取消全选
            else
            {
                for (int i = 0; i < count; i++)
                {
                    DataGridViewCheckBoxCell dgvCheckBox = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells[0];
                    dgvCheckBox.Value = false;
                }
                btnChooseAll.Text = "全  选";
                lblChooseInfo.Text = "当前未选中任何产品。。。。";
            }

        }
        //【4】反选
        private void btnChooseRest_Click(object sender, EventArgs e)
        {
            int count = dataGridView1.Rows.Count;
            for(int i=0; i < count; i++)
            {
                DataGridViewCheckBoxCell dgvCheckBox = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells[0];
                Boolean flag = Convert.ToBoolean(dgvCheckBox.Value);
                if (!flag)
                {
                    //选中取消
                    dgvCheckBox.Value = true;
                }
                //没选中，选中
                else dgvCheckBox.Value = false;
            }
        }
        //【5】批量删除--------只有管理员才有此权限
        private void btnChooseDel_Click(object sender, EventArgs e)
        {
            //定义objListChoosed用接收被选中行的编号
            List<string> objListChoosed = new List<string>();
            //判断当前用户是不是管理员
            if (Program.currentUser != "000000000")
            {
                MessageBox.Show("您没有权限进行此操作", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                //执行删除操作
                objListChoosed = GetChoosedRows(dataGridView1);
                if (objListChoosed.Count==0)
                {
                    MessageBox.Show("请选择要删除的行！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    string info = "您是否要删除编号为【" + objListChoosed[0] + "】等" + objListChoosed.Count + "条信息吗？";
                    DialogResult result= MessageBox.Show(info, "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        for (int i = 0; i < objListChoosed.Count; i++)
                        {
                            string sqlDel = string.Format("DELETE FROM funsofa where pcode='{0}'", objListChoosed[i]);
                            try
                            {
                                dbConnect.Delete(sqlDel);
                                MessageBox.Show("删除成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //刷新数据源
                                string sqlSelect = "SELECT * FROM funsofa ";
                                try
                                {
                                    //把数据绑定到全局变量沙发信息上
                                    Program.objListSofa = dbConnect.Select(sqlSelect);
                                    dataGridView1.DataSource = null;
                                    dataGridView1.AutoGenerateColumns = false;
                                    dataGridView1.DataSource = Program.objListSofa;
                                    lblChooseInfo.Text = "更新数据成功！当前数据库中一共有【" + dataGridView1.Rows.Count + "】条产品信息。";
                                }
                                catch (Exception ex)
                                {

                                    MessageBox.Show(ex.Message);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                    else return;
                }
            }
        }
        //【6】查询啊查询
        private void txtQueryContent_TextChanged(object sender, EventArgs e)
        {
            switch (cmboxQueryWay.Text)
            {
                case "产品编号":
                    lblInfo.Text = "按照【产品编号】进行查询。。。";
                    //清空objListQuery
                    objListQuery = null;
                    objListQuery = productSearch.GetAllProductByCode(txtQueryContent.Text.Trim(), Program.objListSofa);
                    //绑定到datagridview上
                    dataGridView1.DataSource = null;
                    dataGridView1.AutoGenerateColumns = false;
                    dataGridView1.DataSource = objListQuery;
                    break;
                case "产品名称":
                    lblInfo.Text = "按照【产品名称】进行查询。。。";
                    //清空objListQuery
                    objListQuery = null;
                    objListQuery = productSearch.GetAllProductByName(txtQueryContent.Text.Trim(), Program.objListSofa);
                    //绑定到datagridview上
                    dataGridView1.DataSource = null;
                    dataGridView1.AutoGenerateColumns = false;
                    dataGridView1.DataSource = objListQuery;
                    break;
                case "风格类型":
                    lblInfo.Text = "按照【风格类型】进行查询。。。";
                    //清空objListQuery
                    objListQuery = null;
                    objListQuery = productSearch.GetAllProductByStyle(txtQueryContent.Text.Trim(), Program.objListSofa);
                    //绑定到datagridview上
                    dataGridView1.DataSource = null;
                    dataGridView1.AutoGenerateColumns = false;
                    dataGridView1.DataSource = objListQuery;
                    break;
                case "组合查询":
                    lblInfo.Text = "按照【自定义方式】进行查询。。。";
                    //清空objListQuery
                    //objListQuery = null;
                    //TODO:点击查询按钮
                    break;
                default:
                    break;
            }
        }
        //【7】切换用户
        private void btnChange_Click(object sender, EventArgs e)
        {
            //打开登录窗口
            DialogResult re = MessageBox.Show("您确定要切换其他用户吗？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (re == DialogResult.Yes)
            {
                this.Close();
                frmLogin frmLogin = new frmLogin();
                frmLogin.Show();
            }
            else return;
        }
        //【8】选择行发生变化
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0) return;
            else if (dataGridView1.CurrentRow.Selected == false) return;
            else
            {
                Sofa objSofa = productSearch.GetProductByCode(dataGridView1.CurrentRow.Cells[1].Value.ToString(), Program.objListSofa);
                //显示当前选择行信息明细
                string info = "产品编号：" + objSofa.Pcode + "，" + "产品名称：" + objSofa.Pname+"产品风格："+objSofa.Pstyle+"。";
                lblChooseInfo.Text = info;
            }
        }
        //【9】双击某一行
        public static frmProductDetail frmProductDetail = null;
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            Sofa objSofa = new Sofa();
            objSofa = productSearch.GetProductByCode(dataGridView1.CurrentRow.Cells[1].Value.ToString(), Program.objListSofa);
            //打开新窗口，如果已经打开，就展示
             frmProductDetail = new frmProductDetail(objSofa);
             frmProductDetail.Show();
        }

        //【10】把选中的行导出到Excel
        private void btnChooseOutput_Click(object sender, EventArgs e)
        {
            //定义objListChoosed用接收被选中行的编号
            List<string> objListChoosed = new List<string>();
            objListChoosed = GetChoosedRows(dataGridView1);
            if (objListChoosed.Count==0)
            {
                MessageBox.Show("请选择要导出的行！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                 //文件导出
                  this.DgvToExcel(dataGridView1);
            }  
        }
        //【11】刷新数据源
        private void btnRe_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM funsofa ";
            try
            {
                //把数据绑定到全局变量沙发信息上
                Program.objListSofa = dbConnect.Select(sql);
                dataGridView1.DataSource = null;
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = Program.objListSofa;
                lblChooseInfo.Text = "刷新数据成功！当前数据库中一共有【" + dataGridView1.Rows.Count + "】条产品信息。";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        //【12】点击搜索
        private void button1_Click(object sender, EventArgs e)
        {
            //整理用户输入数据格式
            string query = txtQueryContent.Text.Trim();
            string[] queryArray = query.Split(' ');
            objListQuery = productSearch.GetAllProductByUnion(queryArray, Program.objListSofa);
            //绑定到datagridview上
            dataGridView1.DataSource = null;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = objListQuery;
        }
        //【13】电商平台大数据展示
        public static frmEBusiness frmEBusiness = null;
        private void tsmTaoBaoData_Click(object sender, EventArgs e)
        {
            if (frmEBusiness==null)
            {
                frmEBusiness = new frmEBusiness();
                frmEBusiness.Show();
            }
            else
            {
                frmEBusiness.Activate();
            }
        }

        //【自定义方法】
        //【1】设置某一列的属性为可编辑，其他为只读
        private void DataGridViewReadOnlyFalse(DataGridView dataGridView, int columnIndex)
        {
            dataGridView.ReadOnly = false;
            dataGridView.Columns[columnIndex].ReadOnly = false;
            for (int i = 0; i < dataGridView.Columns.Count - 1; i++)
            {
                dataGridView.Columns[i + 1].ReadOnly = true;
            }
        }
        //【2】设置某一行的属性为可编辑，其他为只读
        private void DataGridViewRowReadOnlyFalse(DataGridView dataGridView, int rowIndex)
        {
            dataGridView.ReadOnly = false;
            dataGridView.Rows[rowIndex].ReadOnly = false;
            for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
            {
                dataGridView.Rows[i + 1].ReadOnly = true;
            }
        }
        //【3】判断是否选中行数据,并返回选中行的产品编号
        private List<string> GetChoosedRows(DataGridView dgv)
        {
            List<string> objList = new List<string>();//实例化
            //objList = null;
            int count = dgv.Rows.Count;
            for(int i = 0; i < count; i++)
            {
                DataGridViewCheckBoxCell dgvCheckBox = (DataGridViewCheckBoxCell)dgv.Rows[i].Cells[0];
                Boolean flag = Convert.ToBoolean(dgvCheckBox.Value);
                DataGridViewTextBoxCell dgvCodeBox = (DataGridViewTextBoxCell)dgv.Rows[i].Cells[1];
                string pcode = Convert.ToString(dgvCodeBox.Value);
                if (flag)
                {
                    objList.Add(pcode);
                }
            }
            return objList;
        }
        //【4】信息导出Excel
        private void DgvToExcel(DataGridView dgv)
        {
            //申明保存对话框   
            SaveFileDialog dlg = new SaveFileDialog();
            //默然文件后缀   
            dlg.DefaultExt = "xls ";
            //文件后缀列表   
            dlg.Filter = "EXCEL文件(*.XLS)|*.xls ";
            //默然路径是系统当前路径   
            dlg.InitialDirectory = Directory.GetCurrentDirectory();
            //打开保存对话框   
            if (dlg.ShowDialog() == DialogResult.Cancel) return;
            //返回文件路径   
            string fileNameString = dlg.FileName;
            //验证strFileName是否为空或值无效   
            if (fileNameString.Trim() == " ")
            { return; }
            //定义表格内数据的行数和列数   
            int colscount = dgv.Columns.Count;
            List<string> objList = new List<string>();//实例化
            objList = this.GetChoosedRows(dgv);
            int rowscount = objList.Count;//行数等于被选中行的个数，机智如我
            //判断行列选择是否满足条件
            if (!IsRoeOrCol(rowscount, colscount)) return;
            //验证以fileNameString命名的文件是否存在，如果存在删除它   
            FileInfo file = new FileInfo(fileNameString);
            if (file.Exists)
            {
                try
                {
                    file.Delete();
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "删除失败 ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            Excel.Application objExcel = null;
            Excel.Workbook objWorkbook = null;
            Excel.Worksheet objsheet = null;
            try
            {
                //申明对象   
                objExcel = new Microsoft.Office.Interop.Excel.Application();
                objWorkbook = objExcel.Workbooks.Add(Missing.Value);
                objsheet = (Excel.Worksheet)objWorkbook.ActiveSheet;
                //设置EXCEL不可见   
                objExcel.Visible = false;

                //向Excel中写入表格的表头   
                int displayColumnsCount = 1;
                for (int i = 0; i <= dgv.ColumnCount - 1; i++)
                {
                    objExcel.Cells[1, displayColumnsCount] = dgv.Columns[i].HeaderText.Trim();
                    displayColumnsCount++;
                }
                //设置进度条   
                tempProgressBar.Refresh();   
                tempProgressBar.Visible   =   true;   
                tempProgressBar.Minimum=1;   
                tempProgressBar.Maximum=rowscount;   
                tempProgressBar.Step=1;   
                //向Excel中逐行逐列写入表格中的数据   
                for (int row = 0; row <= rowscount - 1; row++)
                {
                    tempProgressBar.PerformStep();   
                    displayColumnsCount = 1;
                    for (int col = 0; col < colscount; col++)
                    {
                        try
                        {
                            objExcel.Cells[row + 2, displayColumnsCount] = dgv.Rows[row].Cells[col].Value.ToString().Trim();
                            displayColumnsCount++;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
                //隐藏进度条   
                tempProgressBar.Visible   =   false;  
                //保存文件   
                objWorkbook.SaveAs(fileNameString, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                        Missing.Value, Excel.XlSaveAsAccessMode.xlShared, Missing.Value, Missing.Value, Missing.Value,
                        Missing.Value, Missing.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "警告 ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            finally
            {
                //关闭Excel应用   
                if (objWorkbook != null) objWorkbook.Close(Missing.Value, Missing.Value, Missing.Value);
                if (objExcel.Workbooks != null) objExcel.Workbooks.Close();
                if (objExcel != null) objExcel.Quit();
                objsheet = null;
                objWorkbook = null;
                objExcel = null;
            }
            MessageBox.Show(fileNameString + "/n/n导出完毕! ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //【5】判断行列选择是否满足条件
        private bool IsRoeOrCol(int rowscount, int colscount)
        {
            //行数必须大于0   
            if (rowscount <= 0)
            {
                MessageBox.Show("没有数据可供保存 ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //列数必须大于0   
            if (colscount <= 0)
            {
                MessageBox.Show("没有数据可供保存 ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //行数不可以大于65536   
            if (rowscount > 65536)
            {
                MessageBox.Show("数据记录数太多(最多不能超过65536条)，不能保存 ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //列数不可以大于255   
            if (colscount > 255)
            {
                MessageBox.Show("数据记录行数太多，不能保存 ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }


    }


}
