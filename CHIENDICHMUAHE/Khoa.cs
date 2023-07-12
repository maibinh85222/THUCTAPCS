using DevExpress.ClipboardSource.SpreadsheetML;
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
    public partial class Khoa : Form
    {
        SqlConnection conn_publisher = new SqlConnection();
        DataTable dt = new DataTable();

        //Đánh dấu đang là lưu khóa sửa hay thêm
        string flag = "";

        //Tạo biến tạm để lưu dữ liệu quay lại;
        string tempMaKhoa = "", tempTenKhoa = "";

        private void HienThiDuLieuKhoa()
        {
            String strlenh = "select MaKhoa, TenKhoa from KHOA";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1_Khoa.DataSource = dt;
            dataGridView1_Khoa.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1_Khoa.Columns[0].HeaderText = "Mã Khoa";
            dataGridView1_Khoa.Columns[1].HeaderText = "Tên Khoa";
            conn_publisher.Close();
        }

        public Khoa()
        {
            InitializeComponent();
        }

        //Hiện thị Quyền admin
        public void HienThiMenuAdmin_Khoa()
        {
            if(Program.mGroup == "TRUONG")
            {
                menuStrip1.Enabled = panel1.Enabled = true;
                textBox1.Enabled = textBox2.Enabled = false;
                lưuToolStripMenuItem.Enabled = false;
            }

            if (Program.mGroup == "GIANGVIEN")
            {
                menuStrip1.Enabled = panel1.Enabled = false;
                textBox1.Enabled = textBox2.Enabled = false;
                lưuToolStripMenuItem.Enabled = false;
            }
        }

        //Kiểm tra text có rỗng không
        private bool Check_NULL(TextBox tb, string str)
        {
            if(tb.Text.Trim().Equals(""))
            {
                MessageBox.Show(str,"Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Error);
                tb.Focus();
                return true;
            } 
            return false;

        } 

        //Kiểm tra mã có trùng không
        private bool Check_Trung(String MAKHOA)
        {
            foreach(DataRow row in dt.Rows)
            {
                String maKhoaGridView = row["MaKhoa"].ToString();
                if (maKhoaGridView.Trim() == MAKHOA.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //-----------------------------------------------------MÀN HÌNH LOAD
        private void Khoa_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            menuStrip1.Enabled = panel1.Enabled = false;
            //this.WindowState = FormWindowState.Maximized;
            HienThiMenuAdmin_Khoa();
            HienThiDuLieuKhoa();

        }

        private void thoatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Sự kiện nhấn chuột vào gridview sẽ hiện lên textbox
        private void dataGridView1_Khoa_SelectionChanged(object sender, EventArgs e)
        {
            int index = dataGridView1_Khoa.CurrentCell.RowIndex;
            DataTable dt = (DataTable)dataGridView1_Khoa.DataSource;
            if(dt.Rows.Count > 0)
            {
                textBox1.Text = dataGridView1_Khoa.Rows[index].Cells[0].Value.ToString();
                textBox2.Text = dataGridView1_Khoa.Rows[index].Cells[1].Value.ToString();
            }    
        }

        //Thêm
        private void theeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = textBox2.Enabled = true;
            textBox1.Text = textBox2.Text = "";
            xóaToolStripMenuItem.Enabled = sửaToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = reToolStripMenuItem.Enabled = true; 
            lưuToolStripMenuItem.Enabled = true;
            flag = "add";

        }

        //Xóa
        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = "delete";

            theeToolStripMenuItem.Enabled = sửaToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = reToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;

        }

        //Sửa
        private void sửaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = "edit";
            textBox2.Enabled = true;
            xóaToolStripMenuItem.Enabled = theeToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = reToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
        }

        //Kiểm tra khoa có xóa được không.
        private bool KiemTraDeleteKhoa(string MAKHOA)
        {
            String strlenh = "select MaGV from GIANGVIEN where MaKhoa = '"+MAKHOA+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            if(dt.Rows.Count > 0 )
            {
                return false;
            }

            String strlenh1 = "select MaSV from SINHVIEN where MaKhoa = '" + MAKHOA + "'";
            dt = Program.ExecSqlDataTable(strlenh1);
            if (dt.Rows.Count > 0)
            {
                return false;
            }

            return true;
        }
        //Lưu
        private void lưuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tempMaKhoa = textBox1.Text.Trim();
            tempTenKhoa = textBox2.Text.Trim();
            //Kiểm tra dữ liệu nhập vào:
            if (Check_NULL(textBox1, "Mã khoa không được để trống!")) return;
            if (Check_NULL(textBox2, "Tên khoa không được để trống!")) return;
            if (textBox1.Text.Trim().Length != 5)
            {
                MessageBox.Show("Mã khoa phải đúng 5 ký tự", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBox1.Text.Contains(" "))
            {
                MessageBox.Show("Mã khoa không được chứa khoảng trống", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

           
            //Xử lý
            //Thêm
            if (flag == "add")
            {
                if (Check_Trung(textBox1.Text.ToString().Trim()))
                {
                    MessageBox.Show("Mã khoa đã tồn tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                String strLenh = "sp_InsertKhoa";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MAKHOA", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@TENKHOA", textBox2.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Thêm khoa thành công!");
                HienThiDuLieuKhoa();

            }

            //Xóa
            else if (flag == "delete")
            {
                if (KiemTraDeleteKhoa(tempMaKhoa) == false)
                {
                    MessageBox.Show("Không thể xóa khoa này");
                    return;
                }
                String strLenh = "sp_DeleteKhoa";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MAKHOA", textBox1.Text.ToString().Trim()));
                //sqlCommand.Parameters.Add(new SqlParameter("@TENKHOA", textBox2.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Xóa khoa thành công!");
                HienThiDuLieuKhoa();
            }

            //sửa
            else if (flag == "edit")
            {
                String strLenh = "sp_UpdateKhoa";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MAKHOA", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@TENKHOA", textBox2.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Sửa khoa thành công!");
                HienThiDuLieuKhoa();
            }
            

        }

        //Undo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = tempMaKhoa;
            textBox2.Text = tempTenKhoa;
        }

        //Refresh
        private void reToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1_Khoa.Refresh();
            HienThiMenuAdmin_Khoa();
            HienThiDuLieuKhoa();
            textBox1.Enabled = textBox2.Enabled = false;
            sửaToolStripMenuItem.Enabled = xóaToolStripMenuItem.Enabled = theeToolStripMenuItem.Enabled = undoToolStripMenuItem.Enabled = reToolStripMenuItem.Enabled = true;


        }
    }
}
