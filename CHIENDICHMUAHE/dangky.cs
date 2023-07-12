using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHIENDICHMUAHE
{
    public partial class dangky : Form
    {
        SqlConnection conn_publisher = new SqlConnection();
        DataTable dt = new DataTable();

        string QUYEN = "";
        public dangky()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        //trường
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label2.Text = "Login name:";
            label2.Visible = label3.Visible = label4.Visible = true;
            textBox1.Visible = textBox2.Visible = textBox3.Visible = true;
            button1.Visible = true;

            QUYEN = "TRUONG";
        }

        //Giảng viên
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            label2.Text = "Login name:";
            label2.Visible = label3.Visible = label4.Visible = true;
            textBox1.Visible = textBox2.Visible = textBox3.Visible = true;
            button1.Visible = true;

            QUYEN = "GIANGVIEN";
        }

        //Sinh viên
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            label2.Text = "Mã sinh viên: ";
            label2.Visible = label4.Visible = true;
            textBox1.Visible = textBox3.Visible = true;
            button1.Visible = true;
            textBox2.Visible = false; label3.Visible = false;

            QUYEN = "SINHVIEN";
        }

        private void dangky_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            if (Program.mGroup == "TRUONG") radioButton1.Visible = radioButton3.Visible = radioButton4.Visible = true;
            if (Program.mGroup == "GIANGVIEN") radioButton4.Visible = true;
        }

        //Kiểm tra text có rỗng không
        private bool Check_NULL(TextBox tb, string str)
        {
            if (tb.Text.Trim().Equals(""))
            {
                MessageBox.Show(str, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tb.Focus();
                return true;
            }
            return false;

        }

        //Kiểm tra giảng viên có tồn tại
        private bool Check_GV(String MAGV)
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaGV from GIANGVIEN";
            dt = Program.ExecSqlDataTable(strlenh);
            foreach (DataRow row in dt.Rows)
            {
                String maKhoaGridView = row["MaGV"].ToString();
                if (maKhoaGridView.Trim() == MAGV.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        //Kiểm tra sinh viên có tồn tại
        private bool Check_SV(String MASV)
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaSV from SINHVIEN";
            dt = Program.ExecSqlDataTable(strlenh);
            foreach (DataRow row in dt.Rows)
            {
                String maKhoaGridView = row["MaSV"].ToString();
                if (maKhoaGridView.Trim() == MASV.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        //Nhấn vào đăng ký
        private void button1_Click(object sender, EventArgs e)
        {

            if (QUYEN == "SINHVIEN")
            {
                if (Check_NULL(textBox1, "Mã sinh viên không được để trống!")) return;
                if (Check_NULL(textBox3, "Mật khẩu không được để trống!")) return;

                else if (textBox1.Text.Contains(" "))
                {
                    MessageBox.Show("Mã sinh viên không được chứa khoảng trống", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                else if (textBox3.Text.Contains(" "))
                {
                    MessageBox.Show("Mật khẩu không được chứa khoảng trống", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (Check_SV(textBox1.Text.Trim())==false){
                    MessageBox.Show("Mã sinh viên không tồn tại!"); return;
                }

                String strLenh = "sp_LogIn";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@LoginName", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@Password", textBox3.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@UserName", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@Role", QUYEN));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Đăng ký tài khoản sinh viên thành công!");
            }
            else 
            {
                if (Check_NULL(textBox1, "Tên đăng nhập không được để trống!")) return;
                if (Check_NULL(textBox2, "Mã giảng viên không được để trống!")) return;
                if (Check_NULL(textBox3, "Mật khẩu không được để trống!")) return;

                else if (textBox1.Text.Contains(" "))
                {
                    MessageBox.Show("Mã tên đăng nhập không được chứa khoảng trống", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                else if (textBox2.Text.Contains(" "))
                {
                    MessageBox.Show("Mã giảng viên không được chứa khoảng trống", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                else if (textBox3.Text.Contains(" "))
                {
                    MessageBox.Show("Mật khẩu không được chứa khoảng trống", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (Check_GV(textBox2.Text.Trim()) == false)
                {
                    MessageBox.Show("Mã giảng viên không tồn tại!"); return;
                }

                String strLenh = "sp_LogIn";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@LoginName", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@Password", textBox3.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@UserName", textBox2.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@Role", QUYEN));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Đăng ký tài khoản thành công!");
            }
           
        }
    }
}
