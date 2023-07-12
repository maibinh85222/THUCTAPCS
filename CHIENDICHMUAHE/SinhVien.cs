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
    public partial class SinhVien : Form
    {
        SqlConnection conn_publisher = new SqlConnection();
        DataTable dt = new DataTable();

        //Đánh dấu đang là lưu khóa sửa hay thêm
        string flag = "";

        //Tạo biến tạm để lưu dữ liệu quay lại;
        string tempMaSV = "", tempMaNhom = "", tempMaKhoa = "", tempTenSV = "";
        string tempMaNhom_S = "";
        public SinhVien()
        {
            InitializeComponent();
        }

        // phân quyền
        public void HienThiMenuAdminSinhVien()
        {
            if(Program.mGroup == "TRUONG") menuStrip1.Enabled = panel1.Enabled = true;
            if(Program.mGroup == "SINHVIEN") button1.Visible = true;


        }

        // Hiện thị bảng dữ liệu
        private void HienThiDuLieu()
        {
            String strlenh = "select MaSV, TenSV, MaKhoa, MaNhom, ChucVu, MaDoiGiamSat from SINHVIEN";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Sinh Viên";
            dataGridView1.Columns[1].HeaderText = "Tên Sinh Viên";
            dataGridView1.Columns[2].HeaderText = "Mã Khoa";
            dataGridView1.Columns[3].HeaderText = "Mã Nhóm";
            dataGridView1.Columns[4].HeaderText = "Chức vụ";
            dataGridView1.Columns[5].HeaderText = "Mã đội giám sát";
            conn_publisher.Close();
        }

        //LẤY DANH SACH KHOA
        private void LayDSKHOA()
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaKhoa, TenKhoa from KHOA";
            dt = Program.ExecSqlDataTable(strlenh);

            comboBox1.DataSource = dt;
            //comboBox1.ValueMember = "MaKhoa";
            comboBox1.DisplayMember = "MaKhoa";
            //comboBox1.SelectedIndex = 0;
            conn_publisher.Close();
        }

        //LẤY DANH SACH NHOM
        private void LayDSNHOM()
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaNhom from NHOM";
            dt = Program.ExecSqlDataTable(strlenh);

            comboBox2.DataSource = dt;
            comboBox2.ValueMember = "MaNhom";
            //comboBox1.DisplayMember = "TenKhoa";
            //comboBox1.SelectedIndex = 0;
            conn_publisher.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void SinhVien_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            menuStrip1.Enabled = panel1.Enabled = false;
            textBox1.Enabled = textBox2.Enabled = comboBox1.Enabled = comboBox2.Enabled = false;
            HienThiMenuAdminSinhVien();// phân quyền
            HienThiDuLieu();
            LayDSKHOA();
            LayDSNHOM();
        }

        //Thoat
        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Them
        private void thêmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = textBox2.Enabled = comboBox1.Enabled = comboBox2.Enabled = true;
            textBox1.Text = textBox2.Text = comboBox1.Text = comboBox2.Text = "";
            xóaToolStripMenuItem.Enabled = sửaToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = refrshToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
            flag = "add";
        }

        //Xoa
        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = "delete";

            thêmToolStripMenuItem.Enabled = sửaToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = refrshToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
        }

        //Sua
        private void sửaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = "edit";
            textBox2.Enabled = comboBox2.Enabled = true;
            xóaToolStripMenuItem.Enabled = thêmToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = refrshToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
            tempMaNhom_S = comboBox2.Text.ToString().Trim();
            dataGridView1.Enabled = false;
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

        //Kiểm tra text có rỗng không
        private bool Check_NULL_CBX(ComboBox tb, string str)
        {
            if (tb.Text.Trim().Equals(""))
            {
                MessageBox.Show(str, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tb.Focus();
                return true;
            }
            return false;

        }

        //Kiểm tra mã có trùng không
        private bool Check_Trung(String MASV)
        {
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

        //Kiểm tra số lượng sinh viên của nhóm đã đầy chưa
        private bool Check_sv_full_nhom(String MANHOM)
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaNhom, SoLuongSV from NHOM";
            dt = Program.ExecSqlDataTable(strlenh);
            int Soluong = 0;
            foreach (DataRow row in dt.Rows)
            {
                String maKhoaGridView = row["MaNhom"].ToString();
                if (maKhoaGridView.Trim() == MANHOM.Trim())
                {
                    Soluong = int.Parse(row["SoLuongSV"].ToString());
                    break;
                }
            }
            conn_publisher.Close();



            DataTable dt1 = new DataTable();
            String strlenh1 = "select MaNhom from SINHVIEN";
            dt1 = Program.ExecSqlDataTable(strlenh1);
            int dem = 0;
            foreach (DataRow row in dt1.Rows)
            {
                String maKhoaGridView = row["MaNhom"].ToString();
                if (maKhoaGridView.Trim() == MANHOM.Trim())
                {
                    dem++;
                }
            }
            conn_publisher.Close();
            if(dem== Soluong) return true;
            else return false;
        }

        //Sự kiện gridview
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            DataTable dt = (DataTable)dataGridView1.DataSource;
            if (dt.Rows.Count > 0)
            {
                textBox1.Text = dataGridView1.Rows[index].Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.Rows[index].Cells[1].Value.ToString();
                comboBox1.Text = dataGridView1.Rows[index].Cells[2].Value.ToString();
                comboBox2.Text = dataGridView1.Rows[index].Cells[3].Value.ToString();
                label7.Text = dataGridView1.Rows[index].Cells[4].Value.ToString();
                label9.Text = dataGridView1.Rows[index].Cells[5].Value.ToString();
            }
        }

        //Nút cá nhân sinh viên
        private void button1_Click(object sender, EventArgs e)
        {
            String strlenh = "select MaSV, TenSV, MaKhoa, MaNhom, ChucVu, MaDoiGiamSat from SINHVIEN where MaSV = '"+Program.username+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Sinh Viên";
            dataGridView1.Columns[1].HeaderText = "Tên Sinh Viên";
            dataGridView1.Columns[2].HeaderText = "Mã Khoa";
            dataGridView1.Columns[3].HeaderText = "Mã Nhóm";
            dataGridView1.Columns[4].HeaderText = "Chức vụ";
            dataGridView1.Columns[5].HeaderText = "Mã đội giám sát";
            conn_publisher.Close();
        }

        //Tìm kiếm sinh viên
        private void button2_Click(object sender, EventArgs e)
        {
            String strlenh = "select MaSV, TenSV, MaKhoa, MaNhom, ChucVu, MaDoiGiamSat from SINHVIEN where MaSV = '"+textBox3.Text.ToString().Trim()+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Sinh Viên";
            dataGridView1.Columns[1].HeaderText = "Tên Sinh Viên";
            dataGridView1.Columns[2].HeaderText = "Mã Khoa";
            dataGridView1.Columns[3].HeaderText = "Mã Nhóm";
            dataGridView1.Columns[4].HeaderText = "Chức vụ";
            dataGridView1.Columns[5].HeaderText = "Mã đội giám sát";
            conn_publisher.Close();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        //Kiểm tra sinh viên có được xóa không
        private bool KiemTraDeleteSV(string MASV)
        {
            String strlenh = "select MaSV from SINHVIEN where MaDoiGiamSat is null and MaSV = '"+MASV+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            if (dt.Rows.Count == 0)
            {
                return false;
            }

            String strlenh1 = "select MaNhom from NHOM where MaTruongNhom = '"+MASV+"'";
            dt = Program.ExecSqlDataTable(strlenh1);
            if (dt.Rows.Count > 0)
            {
                return false;
            }

            String strlenh2 = "select *from SV_KT where MaSV = '" + MASV + "'";
            dt = Program.ExecSqlDataTable(strlenh2);
            if (dt.Rows.Count == 1)
            {
                return false; // không cho xóa
            }

            return true;
        }

        //Kiểm tra sinh viên có thuộc đội trưởng
        private bool KiemTraDoiTruong(string MASV)
        {
            String strlenh = "select * from NHOM where MaTruongNhom = '" + MASV + "'";
            dt = Program.ExecSqlDataTable(strlenh);
            if (dt.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        //Luu
        private void lưuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tempMaSV = textBox1.Text.Trim();
            tempTenSV = textBox2.Text.Trim();
            tempMaNhom = comboBox2.Text.Trim();    
            tempMaKhoa = comboBox1.Text.Trim();

          
            //Kiểm tra dữ liệu nhập vào:
            if (Check_NULL(textBox1, "Mã sinh viên không được để trống!")) return;
            if (Check_NULL(textBox2, "Tên sinh viên không được để trống!")) return;
            //if (Check_NULL_CBX(comboBox2, "Mã nhóm không được để trống!")) return;
            if (Check_NULL_CBX(comboBox1, "Mã khoa không được để trống!")) return;
            if (textBox1.Text.Trim().Length != 5)
            {
                MessageBox.Show("Mã sinh viên phải đúng 5 ký tự", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBox1.Text.Contains(" "))
            {
                MessageBox.Show("Mã sinh viên không được chứa khoảng trống", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //Xử lý
            //Thêm
            if (flag == "add")
            {
                if (Check_Trung(textBox1.Text.Trim()))
                {
                    MessageBox.Show("Mã sinh viên đã tồn tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (Check_sv_full_nhom(tempMaNhom))
                {
                    MessageBox.Show("Số lượng sinh viên trong nhóm đã full!");
                    return;
                }
                String strLenh = "AddSinhVien";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MASV", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@TENSV", textBox2.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@MAKHOA", comboBox1.Text.ToString().Trim()));

                if (comboBox2.Text.ToString().Trim() == "")
                {

                    sqlCommand.Parameters.Add(new SqlParameter("@MANHOM", DBNull.Value));

                }
                else if (comboBox2.Text.ToString().Trim() != "")
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@MANHOM", comboBox2.Text.ToString().Trim()));
                }

                sqlCommand.Parameters.Add(new SqlParameter("@CHUCVU", "Sinh Viên"));

                sqlCommand.Parameters.Add(new SqlParameter("@MADOIGIAMSAT", DBNull.Value));

                //sqlCommand.Parameters.Add(new SqlParameter("@MANHOM", comboBox2.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Thêm sinh viên thành công!");
                HienThiDuLieu();

            }

            //Xóa
            else if (flag == "delete")
            {
                if (KiemTraDeleteSV(tempMaSV) == false)
                {
                    MessageBox.Show("Không thể xóa vì đã thuộc đội giám sát hoặc trưởng nhóm hoặc đã nằm trong danh sách khen thưởng");
                    return;
                }
                String strLenh = "DeleteSinhVien";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MASV", textBox1.Text.ToString().Trim()));
                //sqlCommand.Parameters.Add(new SqlParameter("@TENKHOA", textBox2.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Xóa sinh viên thành công!");
                HienThiDuLieu();
            }

            //sửa
            else if (flag == "edit")
            {
                if (KiemTraDoiTruong(tempMaSV) && tempMaNhom_S != tempMaNhom)
                {
                    MessageBox.Show("Không thể sửa vì sinh viên là nhóm trưởng");
                    return;
                }
                if (Check_sv_full_nhom(tempMaNhom) && tempMaNhom != "")
                {
                    MessageBox.Show("Số lượng sinh viên trong nhóm đã full!");
                    return;
                }

                if (label9.Text.ToString().Trim()!="")
                {
                    MessageBox.Show("Sinh viên thuộc đội giám sát nên không thuộc nhóm!");
                    return;
                }

                String strLenh = "EditSinhVien";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MASV", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@TENSV", textBox2.Text.ToString().Trim()));
               
                if (comboBox2.Text.ToString().Trim() == "")
                {

                    sqlCommand.Parameters.Add(new SqlParameter("@MANHOM", DBNull.Value));

                }
                else if (comboBox2.Text.ToString().Trim() != "")
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@MANHOM", comboBox2.Text.ToString().Trim()));
                }

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Sửa sinh viên thành công!");
                HienThiDuLieu();
            }
        }

        //Undo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = tempMaSV;
            textBox2.Text = tempTenSV;
            comboBox2.Text = tempMaNhom;
            comboBox1.Text = tempMaKhoa;
        }

        //Refresh
        private void refrshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            HienThiMenuAdminSinhVien();
            HienThiDuLieu();
            textBox1.Enabled = textBox2.Enabled = comboBox1.Enabled = comboBox2.Enabled = false;
            sửaToolStripMenuItem.Enabled = xóaToolStripMenuItem.Enabled = thêmToolStripMenuItem.Enabled = undoToolStripMenuItem.Enabled = refrshToolStripMenuItem.Enabled = true;
            textBox3.Text = "";
            dataGridView1.Enabled = true;
        }
    }
}
