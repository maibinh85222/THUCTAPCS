using DevExpress.XtraBars.Ribbon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHIENDICHMUAHE
{
    public partial class Dangnhap : Form
    {
        private Boolean isSinhVien = false;

        public static SqlConnection conn_publisher = new SqlConnection();
       // DataTable dt = new DataTable();

        public Dangnhap()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Dangnhap_Load(object sender, EventArgs e)
        {
            
            this.WindowState = FormWindowState.Maximized;
            //Lay_DSKHOA("select *from KHOA");
           /* if (ketnoi_maychu()==0) return;
            LayDSKHOA("select *from KHOA");
            comboBox1.SelectedIndex = 0;*/
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

      /*  private void LayDSKHOA(String cmd)
        {
            DataTable dt = new DataTable();
            if (conn_publisher.State == ConnectionState.Closed) conn_publisher.Open();
        
            SqlDataAdapter da = new SqlDataAdapter(cmd, conn_publisher);
            da.Fill(dt);
            conn_publisher.Close();

            Program.bds_dspm.DataSource = dt;
            comboBox1.DataSource = Program.bds_dspm;
            *//*comboBox1.DisplayMember= "MaKhoa";
            comboBox1.ValueMember = "TenKhoa";*//*

            comboBox1.DisplayMember = "TenKhoa"; // hiện thị
            comboBox1.ValueMember = "MaKhoa";   // giá trị

        }*/

        public static int ketnoi_maychu()
        {
            if (conn_publisher != null && conn_publisher.State == ConnectionState.Open)
                conn_publisher.Close();
            try
            {
                conn_publisher.ConnectionString = Program.connstr_Publisher;
                conn_publisher.Open();
                return 1;
            }

            catch (Exception e)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu gốc. " + e.Message, "", MessageBoxButtons.OK);
                return 0;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Trim()==""|| textBox2.Text.Trim()=="")
            { 
                MessageBox.Show("tài khoản và mật khẩu không được để trống!","",MessageBoxButtons.OK);
                return;
            }
            Program.mlogin = textBox1.Text.Trim ();
            Program.password = textBox2.Text.Trim ();

            if (Program.KetNoi() == 0)
            {
                return;
            }
            Program.mloginDN = Program.mlogin;
            Program.passwordDN = Program.password;

            if (isSinhVien)
            {
                String strleng = "exec SP_LayThongTinSinhVien '" + Program.mloginDN + "'";
                Program.myReader = Program.ExecSqlDataReader(strleng);

                if (Program.myReader == null) return;
                Program.myReader.Read();

                Program.username = Program.myReader.GetString(0);
                if (Convert.IsDBNull(Program.username))
                {
                    MessageBox.Show("Login khong co quyen truy cap!");
                    return;
                }
                Program.mHoten = Program.myReader.GetString(1);
                Program.mGroup = Program.myReader.GetString(2);

                Program.myReader.Close();
                Program.conn.Close();
                Program.frmChinh.HienThiMenuSinhVien();
                Program.frmChinh.HienThiThongTin();
            }
            else
            {
                String strleng = "exec SP_LayThongTinGiaoVien '" + Program.mloginDN + "'";
                Program.myReader = Program.ExecSqlDataReader(strleng);

                if (Program.myReader == null) return;
                Program.myReader.Read();

                Program.username = Program.myReader.GetString(0);
                if (Convert.IsDBNull(Program.username))
                {
                    MessageBox.Show("Login khong co quyen truy cap!");
                    return;
                }
                Program.mHoten = Program.myReader.GetString(1);
                Program.mGroup = Program.myReader.GetString(2);

                Program.myReader.Close();
                Program.conn.Close();

                if (Program.mGroup == "TRUONG")
                {
                    Program.frmChinh.HienThiMenuAdmin();
                }

                if (Program.mGroup == "GIANGVIEN")
                {

                    Program.frmChinh.HienThiMenuGiangVien();

                }
                Program.frmChinh.HienThiThongTin();
            }

            Program.conn.Close();
            if (Program.mGroup == "GIANGVIEN" || Program.mGroup == "TRUONG") 
            this.Close();
        }

      
        private void ckbSinhVien_CheckedChanged(object sender, EventArgs e)
        {
            if (!isSinhVien)
            {
                isSinhVien = true;
                label1.Text = "Mã sinh viên: ";
            }
            else
            {
                isSinhVien = false;
                label1.Text = "Tài khoản: ";
            }
        }
    }
}
