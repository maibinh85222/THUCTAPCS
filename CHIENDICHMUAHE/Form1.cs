using DevExpress.Drawing.Internal.Fonts;
using DevExpress.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CHIENDICHMUAHE
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {     
            this.WindowState = FormWindowState.Maximized;
            /*  if (Program.mGroup == "SINHVIEN" || Program.mGroup == "TRUONG" || Program.mGroup == "GIANGVIEN") 
              {
                  statusStrip1.Visible = true;
                  HienThiThongTin();
              }*/
            
        }

        //Hiện thị thanh thông tin ở dưới stripview
        public void HienThiThongTin()
        {
            //hien thi nut dang ky
            if(Program.mGroup=="TRUONG" || Program.mGroup == "GIANGVIEN")
                ribbonPageGroup10.Visible = true;
            //------------------------------------
            statusStrip1.Visible = true;
            toolStripStatusLabel1.Text = "Mã: "+Program.username;
            toolStripStatusLabel2.Text = "Họ tên: "+Program.mHoten;
            toolStripStatusLabel3.Text = "Nhóm: "+Program.mGroup;
        }

        private Form IsExists(Type type)
        {
            foreach (Form f in this.MdiChildren)
            {
                if(f.GetType()==type)  return f; 
            }
            return null;
        }

        //Phân quyền: quyền admin SA toàn quyền
        public void HienThiMenuAdmin()
        {
            ribbonPage2.Visible = true;
            ribbonPageGroup2.Visible = ribbonPageGroup3.Visible = ribbonPageGroup4.Visible = ribbonPageGroup5.Visible = ribbonPageGroup6.Visible = ribbonPageGroup7.Visible = ribbonPageGroup8.Visible = ribbonPageGroup9.Visible = true;
        }

        //Phân quyền giảng viên:Thêm xóa sửa NHÓM, NHÓM THỰC HIỆN, xem ALL
        public void HienThiMenuGiangVien()
        {
            ribbonPage2.Visible = true;
            ribbonPageGroup2.Visible = ribbonPageGroup3.Visible = ribbonPageGroup4.Visible = ribbonPageGroup5.Visible = ribbonPageGroup6.Visible = ribbonPageGroup7.Visible = ribbonPageGroup8.Visible = ribbonPageGroup9.Visible = true;
        }

        //Phân quyền sinh viên:Chỉ được xem 1 số from: SINHVIEN, NHOM, CONGVIEC, DOIGIAMSAT, NHOMTHUCHIEN, BUOI
        public void HienThiMenuSinhVien()
        {
            ribbonPage2.Visible = true;
            ribbonPageGroup4.Visible = ribbonPageGroup5.Visible = ribbonPageGroup8.Visible = ribbonPageGroup9.Visible = true;
            ribbonPageGroup2.Visible = ribbonPageGroup3.Visible = ribbonPageGroup6.Visible = false;
            ribbonPageGroup7.Visible = false;
        }

        //Tat hien thi khi dang xuat
        public void DangXuat()
        {
            ribbonPage2.Visible = false;
            statusStrip1.Visible = false;
            ribbonPageGroup10.Visible= false;
        }

        //Kieemr tra from co ton tai k
        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }

        //Mở from
        public void showForm(Type frmType)
        {
            Form frm = this.CheckExists(frmType);
            if (frm != null) frm.Activate();
            else
            {
                Form f = (Form)Activator.CreateInstance(frmType);

                //gán cha của formDangNhap là form hiện tại và show lên
                f.MdiParent = this;
                f.Show();
            }
        }

        //frm đăng nhập
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Program.mloginDN == null|| Program.mloginDN == "")
            {
                Form ftm = this.IsExists(typeof(Dangnhap));
                if (ftm != null)
                {
                    ftm.Activate();
                }
                else
                {
                    Dangnhap f = new Dangnhap();
                    f.MdiParent = this;
                    f.Show();
                }
            }
            else MessageBox.Show("Vui lòng đăng xuất trước khi đăng nhập lại!");
            
        }

        //frm GiangVien
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form ftm = this.IsExists(typeof(GiangVien));
            if (ftm != null)
            {
                ftm.Activate();
            }
            else
            {
                GiangVien f = new GiangVien();
                f.MdiParent = this;
                f.Show();
            }
        }

        //frm sinh vien
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form ftm = this.IsExists(typeof(SinhVien));
            if (ftm != null)
            {
                ftm.Activate();
            }
            else
            {
                SinhVien f = new SinhVien();
                f.MdiParent = this;
                f.Show();
            }
        }

        //frm nhóm
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form ftm = this.IsExists(typeof(Nhom));
            if (ftm != null)
            {
                ftm.Activate();
            }
            else
            {
                Nhom f = new Nhom();
                f.MdiParent = this;
                f.Show();
            }
        }

        //frm địa bàn
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form ftm = this.IsExists(typeof(DiaBan));
            if (ftm != null)
            {
                ftm.Activate();
            }
            else
            {
                DiaBan f = new DiaBan();
                f.MdiParent = this;
                f.Show();
            }
        }

        //frm ấp
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form ftm = this.IsExists(typeof(Ap));
            if (ftm != null)
            {
                ftm.Activate();
            }
            else
            {
                Ap f = new Ap();
                f.MdiParent = this;
                f.Show();
            }
        }

        //frm xã
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form ftm = this.IsExists(typeof(Xa));
            if (ftm != null)
            {
                ftm.Activate();
            }
            else
            {
                Xa f = new Xa();
                f.MdiParent = this;
                f.Show();
            }
        }

        //frm nhà
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form ftm = this.IsExists(typeof(Nha));
            if (ftm != null)
            {
                ftm.Activate();
            }
            else
            {
                Nha f = new Nha();
                f.MdiParent = this;
                f.Show();
            }
        }

        //frm khen thưởng
        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form ftm = this.IsExists(typeof(KhenThuong));
            if (ftm != null)
            {
                ftm.Activate();
            }
            else
            {
                KhenThuong f = new KhenThuong();
                f.MdiParent = this;
                f.Show();
            }
        }

        //frm công việc
        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form ftm = this.IsExists(typeof(CongViec));
            if (ftm != null)
            {
                ftm.Activate();
            }
            else
            {
                CongViec f = new CongViec();
                f.MdiParent = this;
                f.Show();
            }
        }

        //frm đội giám sát
        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form ftm = this.IsExists(typeof(DoiGiamSat));
            if (ftm != null)
            {
                ftm.Activate();
            }
            else
            {
                DoiGiamSat f = new DoiGiamSat();
                f.MdiParent = this;
                f.Show();
            }
        }

        //frm Khoa
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Form ftm = this.IsExists(typeof(Khoa));
            if (ftm != null)
            {
                ftm.Activate();
            }
            else
            {
                Khoa f = new Khoa();
                // f.MdiParent = this;
                f.Show();
            }*/
            showForm(typeof(Khoa));

        }

        //Thoát
        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        //frm sinh viên khen thưởng
        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form ftm = this.IsExists(typeof(sv_kt));
            if (ftm != null)
            {
                ftm.Activate();
            }
            else
            {
                sv_kt f = new sv_kt();
                f.MdiParent = this;
                f.Show();
            }

        }

        //frm nhóm thực hiện
        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form ftm = this.IsExists(typeof(NhomThucHien));
            if (ftm != null)
            {
                ftm.Activate();
            }
            else
            {
                NhomThucHien f = new NhomThucHien();
                f.MdiParent = this;
                f.Show();
            }
        }

        //frm Buổi
        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form ftm = this.IsExists(typeof(Buoi));
            if (ftm != null)
            {
                ftm.Activate();
            }
            else
            {
                Buoi f = new Buoi();
                f.MdiParent = this;
                f.Show();
            }
        }

        //đăng xuất
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Program.mGroup != null & Program.mGroup != "")
            {
                Program.mloginDN = null;
                Program.mlogin = Program.mGroup = Program.mHoten = Program.mloginDN;
                DangXuat();
                MessageBox.Show("Đăng xuất thành công!");
            }
            else MessageBox.Show("Bạn chưa đăng nhập!");
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        //đăng ký
        private void barButtonItem20_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dangky f = new dangky();
            f.MdiParent = this;
            f.Show();
        }
    }
}
