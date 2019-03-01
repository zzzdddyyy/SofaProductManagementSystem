using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Models;
using DAL;
using Common;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Test0707
{
    public partial class frmProductDetail : Form
    {
        private ProductSearch productSearch = new ProductSearch();//实例化产品查询类
        private Sofa objSofa = new Sofa();
        private DBConnect dbConnect = new DBConnect();//实例化数据库操作类
        private ProductServices ProductServices = new ProductServices();//实例化产品数据操作类
        private int actionFlag = 0;//识别是修改还是添加操作的标志，如果是1---》添加  2---》修改；
        private string engiDrawPath = string.Empty;//定义工程图路径的变量，加载时为空
        private string renderPath = string.Empty;//定义渲染图路径的变量，加载时为空
        private bool isChooseEngi = false;//判断是否选择了工程图
        private bool isChooseRender = false;//判断是否选择了渲染图

        //构造函数
        public frmProductDetail()//无参构造方法
        {
            InitializeComponent();
        }
        public frmProductDetail(Sofa objSofa):this()//带一个参数的构造方法
        {
            //禁用控件
            this.DisableBtn();
            //展示数据
            txtCode.Text = objSofa.Pcode;
            txtName.Text = objSofa.Pname;
            txtPrize.Text = Convert.ToString(objSofa.Pprize);
            cmbFrame.Text = objSofa.Frame;
            cmboxFillter.Text = objSofa.Fillter;
            cmboxHandrail.Text = objSofa.Handrail;
            cmboxFoot.Text = objSofa.Footrest;
            cmboxBackrest.Text = objSofa.Backrest;
            cmboxCorner.Text = objSofa.Corner;
            cmboxLay.Text = objSofa.Lay;
            cmboxMid.Text = objSofa.Mid;
            cmboxSeatbox.Text = objSofa.Seatbox;
            cmboxStyle.Text = objSofa.Pstyle;
            cmboxTea.Text = objSofa.Teatable;
            cmboxWrapper.Text = objSofa.Wrapper;
            txtEngiPath.Text = objSofa.Engi_draw;
            txtRenderPath.Text = objSofa.Rendergraph;
            if (string.IsNullOrWhiteSpace(objSofa.Engi_draw))
            {
                pBoxEngi_draw.BackgroundImage = Image.FromFile(".\\image\\engi_draw001.gif");
            }
            else pBoxEngi_draw.BackgroundImage = Image.FromFile(objSofa.Engi_draw);
            if (string.IsNullOrWhiteSpace(objSofa.Rendergraph))
            {
                pBoxRendergraph.BackgroundImage = Image.FromFile(".\\image\\sofa_8150.jpg");
            }
            else pBoxRendergraph.BackgroundImage = Image.FromFile(objSofa.Rendergraph);
            //this.ReLoadInfo();
        }//带一个参数的构造，继承自Form主窗口
        private void frmProductDetail_Load(object sender, EventArgs e)
        {
            txtEngiPath.Visible = false;
            txtRenderPath.Visible = false;
        }

        //控件事件
        //【01】取消操作
        private void btnCancel_Click(object sender, EventArgs e)
        {
            switch (actionFlag)
            {
                case 1://新建
                    this.Close();
                    break;
                case 2://修改
                    this.DisableBtn();
                    break;
                default:
                    break;
            }
        }

        //【02】关闭窗口
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //【03】修改(做准备)
        private void btnModify_Click(object sender, EventArgs e)
        {
            //启用控件
            gBoxDetail.Enabled = true;
            txtCode.Enabled = false;
            btnCancel.Enabled = true;
            btnSave.Enabled = true;
            btnSubmit.Enabled = true;
            btnRendergraph.Enabled = true;
            btnEngiDraw.Enabled = true;
            //修改标志
            actionFlag = 2;
        }
        //【04】删除
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //删除当前项，需要管理员权限
            if (Program.currentUser != "000000000")
            {
                MessageBox.Show("您没有权限删除该产品的信息。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string info = "您是否要删除编号为【" + txtCode.Text + "】的信息吗？";
            DialogResult re = MessageBox.Show(info, "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (re == DialogResult.Yes)
            {
                string sqlDel = string.Format("DELETE FROM funsofa where pcode='{0}'", txtCode.Text.Trim());
                try
                {
                    dbConnect.Delete(sqlDel);
                    MessageBox.Show("删除成功,即将关闭此窗口，请刷新结果！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        //【05】新建（做准备）
        private void bntNew_Click(object sender, EventArgs e)
        {
            //启用控件
            EnableBtn();
            //禁用产品编号
            txtCode.Enabled = false;
            //清空内容
            this.ClearContent();
            //修改标志
            actionFlag = 1;
            //生成产品编号
            txtCode.Text = ProductServices.GenerateCode();
        }
        //【06】提交修改/新建
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //验证输入是否有效
            if (this.IsVaildInput())
            {
                
                switch (actionFlag)
                {
                    case 1://New
                           //把所有的信息封装到Sofa类中
                        Sofa objNewSofa = new Sofa
                        {
                            //初始化
                            Pcode = txtCode.Text,
                            Pname = txtName.Text,
                            Pprize = Convert.ToDouble(txtPrize.Text),
                            Pstyle = cmboxStyle.Text,
                            Fillter = cmboxFillter.Text,
                            Frame = cmbFrame.Text,
                            Footrest = cmboxFoot.Text,
                            Backrest = cmboxBackrest.Text,
                            Engi_draw = null,
                            Corner = cmboxCorner.Text,
                            Handrail = cmboxHandrail.Text,
                            Lay = cmboxLay.Text,
                            Mid = cmboxMid.Text,
                            Rendergraph = null,
                            Seatbox = cmboxSeatbox.Text,
                            Wrapper = cmboxWrapper.Text,
                            Teatable = cmboxTea.Text,
                        };
                        if (pBoxEngi_draw.BackgroundImage != null)
                        {
                            objNewSofa.Engi_draw = EngiDrawNameSave(engiDrawPath, pBoxEngi_draw);
                        }
                        if (pBoxRendergraph.BackgroundImage != null)
                        {
                            objNewSofa.Rendergraph = RenderGraphNameSave(renderPath, pBoxRendergraph);
                        }
                        try
                        {
                            ProductServices.NewProduct(objNewSofa);
                            MessageBox.Show("产品【"+txtName.Text+"】已成功导入数据库！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        break;
                    case 2://Modify
                           //把所有的信息封装到Sofa类中
                        Sofa objModifySofa = new Sofa
                        {
                            //初始化
                            Pcode = txtCode.Text,
                            Pname = txtName.Text,
                            Pprize = Convert.ToDouble(txtPrize.Text),
                            Pstyle = cmboxStyle.Text,
                            Fillter = cmboxFillter.Text,
                            Frame = cmbFrame.Text,
                            Footrest = cmboxFoot.Text,
                            Backrest = cmboxBackrest.Text,
                            Engi_draw = txtEngiPath.Text,
                            Corner = cmboxCorner.Text,
                            Handrail = cmboxHandrail.Text,
                            Lay = cmboxLay.Text,
                            Mid = cmboxMid.Text,
                            Rendergraph = txtRenderPath.Text,
                            Seatbox = cmboxSeatbox.Text,
                            Wrapper = cmboxWrapper.Text,
                            Teatable = cmboxTea.Text,
                        };
                        if (isChooseEngi)
                        {
                            objModifySofa.Engi_draw = EngiDrawNameSave(engiDrawPath, pBoxEngi_draw);
                        }
                        if (isChooseRender)
                        {
                            objModifySofa.Rendergraph = RenderGraphNameSave(renderPath, pBoxRendergraph);
                        }
                        DialogResult re = MessageBox.Show("您确定要修改【" + txtName.Text + "】的信息吗？该操作不可逆！！！", "系统提示", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);
                        if(re == DialogResult.Yes)
                        {
                            //执行修改
                            try
                            {
                                ProductServices.ModifyProduct(txtCode.Text, objModifySofa);
                                MessageBox.Show("产品【" + txtName.Text + "】修改完成！", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        break;
                    default:
                        break;
                }

            }
        }
        //【07】选择工程图
        private void btnEngiDraw_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "picture|*.png;*.jpg;*.bmp;*.gif;*.*";
            switch (actionFlag)
            {
                case 2:
                    
                    if (openFile.ShowDialog() == DialogResult.OK)
                    {
                        //修改了工程图
                        isChooseEngi = true;
                        engiDrawPath = openFile.FileName;
                        //把照片展示在pictureBox中
                        pBoxEngi_draw.BackgroundImage = Image.FromFile(engiDrawPath);
                    }
                    break;
                default:
                    if (openFile.ShowDialog() == DialogResult.OK)
                    {
                        engiDrawPath = openFile.FileName;
                        //把照片展示在pictureBox中
                        pBoxEngi_draw.BackgroundImage = Image.FromFile(engiDrawPath);
                    }
                    break;
            }
            
        }
        //【08】选择渲染图
        private void btnRendergraph_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "picture|*.png;*.jpg;*.bmp;*.*";
            switch (actionFlag)
            {
                case 2:
                    if (openFile.ShowDialog() == DialogResult.OK)
                    {
                        //修改了渲染图
                        isChooseRender = true;
                        renderPath = openFile.FileName;
                        //把照片展示在pictureBox中
                        pBoxRendergraph.BackgroundImage = Image.FromFile(renderPath);
                    }
                    break;
                default:
                    if (openFile.ShowDialog() == DialogResult.OK)
                    {
                        renderPath = openFile.FileName;
                        //把照片展示在pictureBox中
                        pBoxRendergraph.BackgroundImage = Image.FromFile(renderPath);
                    }
                    break;
            }
           
        }
        //【09】保存，禁用控件
        private void btnSave_Click(object sender, EventArgs e)
        {
            DisableBtn();
            MessageBox.Show("保存成功！");
        }


        //用户自定义方法：
        //【01】展示选中行的数据
        private void LoadDgvToDetail()
        {
            
        }
        //【02】启用控件
        private void EnableBtn()
        {
            //启用控件
            //gBoxDetail.Enabled = true;
            txtCode.Enabled = false;
            btnCancel.Enabled = true;
            btnSave.Enabled = true;
            btnSubmit.Enabled = true;
            btnRendergraph.Enabled = true;
            btnEngiDraw.Enabled = true;
        }
        //【03】禁用控件
        private void DisableBtn()
        {
            //禁用控件
            //gBoxDetail.Enabled = false;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            btnSubmit.Enabled = false;
            btnRendergraph.Enabled = false;
            btnEngiDraw.Enabled = false;
        }
        //【04】清空内容
        private void ClearContent()
        {
            txtCode.Text = string.Empty;
            txtName.Text = string.Empty;
            txtPrize.Text = string.Empty;
            cmbFrame.Text = string.Empty;
            cmboxFillter.Text = string.Empty;
            cmboxHandrail.Text = string.Empty;
            cmboxFoot.Text = string.Empty;
            cmboxBackrest.Text = string.Empty;
            cmboxCorner.Text = string.Empty;
            cmboxLay.Text = string.Empty;
            cmboxMid.Text = string.Empty;
            cmboxSeatbox.Text = string.Empty;
            cmboxStyle.Text = string.Empty;
            cmboxTea.Text = string.Empty;
            cmboxWrapper.Text = string.Empty;
            pBoxEngi_draw.BackgroundImage = null;
            pBoxRendergraph.BackgroundImage = null;
        }
        //【05】记录当前传入的信息，以便取消后重载
        private void ReLoadInfo()
        {
            Sofa objSofa = new Sofa()
            {
                Pcode = txtCode.Text,
                Pname = txtName.Text,
                Pprize = Convert.ToDouble(txtPrize.Text),
                Pstyle = cmboxStyle.Text,
                Fillter = cmboxFillter.Text,
                Frame = cmbFrame.Text,
                Footrest = cmboxFoot.Text,
                Backrest = cmboxBackrest.Text,
                Engi_draw = txtEngiPath.Text,
                Corner = cmboxCorner.Text,
                Handrail = cmboxHandrail.Text,
                Lay = cmboxLay.Text,
                Mid= cmboxMid.Text,
                Rendergraph=txtRenderPath.Text,
                Seatbox= cmboxSeatbox.Text,
                Wrapper= cmboxWrapper.Text,
                Teatable= cmboxTea.Text,
            };
        }
        //【06】验证输入是否有效
        private bool IsVaildInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("产品名称不能为空。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(cmboxStyle.Text))
            {
                MessageBox.Show("产品风格不能为空。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmboxStyle.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPrize.Name))
            {
                MessageBox.Show("产品单价不能为空。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPrize.Focus();
                return false;
            }
            //价格必须是两位小数
            if (!CheckLoginInput.IsPrize(txtPrize.Text))
            {
                MessageBox.Show("产品单件为0.00~999999999.99之间的数字。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPrize.Focus();
                return false;
            }
            if (pBoxEngi_draw.BackgroundImage == null)
            {
                MessageBox.Show("工程图不能为空。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnEngiDraw.Focus();
                return false;
            }
            if (pBoxRendergraph.BackgroundImage == null)
            {
                MessageBox.Show("产品渲染图不能为空。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnRendergraph.Focus();
                return false;
            }
            return true;
        }
        //【07】//生成工程图片名称和路径
        private string EngiDrawNameSave(string currentPicturePath,PictureBox pictureBox)
        {
            //生成图片名
            string photoName = BuilName.BuildPhotoName(currentPicturePath);
            //生成图片路径
            photoName = ".\\engineer\\" + photoName;
            //存储图片
            Bitmap objBitmap = new Bitmap(pictureBox.BackgroundImage);
            objBitmap.Save(photoName, pictureBox.BackgroundImage.RawFormat);
            objBitmap.Dispose();
            //返回路径
            return photoName;
        }//生成工程图片名称和路径
        //【08】//生成渲染图片名称和路径
        private string RenderGraphNameSave(string currentPicturePath, PictureBox pictureBox)
        {
            //生成图片名
            string photoName = BuilName.BuildPhotoName(currentPicturePath);
            //生成图片路径
            photoName = ".\\render\\" + photoName;
            //存储图片
            Bitmap objBitmap = new Bitmap(pictureBox.BackgroundImage);
            objBitmap.Save(photoName, pictureBox.BackgroundImage.RawFormat);
            objBitmap.Dispose();
            //返回路径
            return photoName;
        }//生成渲染图片名称和路径

        private void label11_Click(object sender, EventArgs e)
        {
            
        }

        private void lblFS_Click(object sender, EventArgs e)
        {
            frmPartsDetail frmParts = new frmPartsDetail();
            frmParts.Show();
        }
    }
}
