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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CHIENDICHMUAHE
{
    public partial class sv_kt : Form
    {
        SqlConnection conn_publisher = new SqlConnection();
        DataTable dt = new DataTable();

        //Đánh dấu đang là lưu khóa sửa hay thêm
        string flag = "";

        //Tạo biến tạm để lưu dữ liệu quay lại;
        string tempMaKT = "", tempMaSV = "";
        DateTime tempNgay;

        //HIỆN THỊ BẢNG DỮ LIỆU:
        private void HienThiDuLieu()
        {
            String strlenh = "select MaKT, MaSV, Ngay from SV_KT";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Khen Thưởng";
            dataGridView1.Columns[1].HeaderText = "Mã Sinh Viên";
            dataGridView1.Columns[2].HeaderText = "Ngày";

            conn_publisher.Close();
        }

        //LẤY DANH SACH SINH VIEN
        private void LayDSSV()
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaSV from SINHVIEN";
            dt = Program.ExecSqlDataTable(strlenh);

            comboBox1.DataSource = dt;
            //comboBox1.ValueMember = "MaKhoa";
            comboBox1.DisplayMember = "MaSV";
            //comboBox1.SelectedIndex = 0;
            conn_publisher.Close();
        }

        //LẤY DANH SACH KHEN THƯỞNG
        private void LayDS_KT()
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaKT from KHENTHUONG";
            dt = Program.ExecSqlDataTable(strlenh);

            comboBox2.DataSource = dt;
            //comboBox1.ValueMember = "MaKhoa";
            comboBox2.DisplayMember = "MaKT";
            //comboBox1.SelectedIndex = 0;
            conn_publisher.Close();
        }
        public sv_kt()
        {
            InitializeComponent();
        }

        //Thoát
        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void HienThiMenuAdmin_sv_kt()
        {
            if(Program.mGroup=="TRUONG")  menuStrip1.Enabled = panel1.Enabled = true;

        }
        private void sv_kt_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            menuStrip1.Enabled = panel1.Enabled = false;
            comboBox2.Enabled = comboBox1.Enabled = dateTimePicker1.Enabled = false;
            // phân quyền
            HienThiMenuAdmin_sv_kt();

            // hiện thị dữ liệu lên gridview 
            HienThiDuLieu();
            LayDSSV();
            LayDS_KT();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //Thêm
        private void thêmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comboBox2.Enabled = comboBox1.Enabled = dateTimePicker1.Enabled = true;
            comboBox2.Text = comboBox1.Text = "";
            xóaToolStripMenuItem.Enabled = sửaToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
            flag = "add";
        }

        //Xóa
        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = "delete";

            thêmToolStripMenuItem.Enabled = sửaToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
        }

        //Sửa
        private void sửaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Không được sửa sinh viên khen thưởng!");
        }

        //Kiểm tra text có rỗng không
        private bool Check_NULL_CBX(System.Windows.Forms.ComboBox tb, string str)
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
        private bool Check_Trung(String MAKT, String MASV, String NGAY)
        {
            foreach (DataRow row in dt.Rows)
            {
                String maKhoaGridView = row["MaKT"].ToString();
                String maKhoaGridView1 = row["MaSV"].ToString();
                String maKhoaGridView2 = row["Ngay"].ToString();
                if (maKhoaGridView.Trim() == MAKT.Trim() && maKhoaGridView1.Trim() == MASV.Trim() && maKhoaGridView2.Trim() == NGAY.Trim())
                {
                    return true;
                }
            }
            return false;
        }
        //Lưu
        private void lưuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime thoigian = dateTimePicker1.Value;

            tempMaSV = comboBox1.Text.Trim();
            tempMaKT = comboBox2.Text.Trim();
            tempNgay = dateTimePicker1.Value;
            //Kiểm tra dữ liệu nhập vào:
            if (Check_NULL_CBX(comboBox1, "Mã sinh viên không được để trống!")) return;
            if (Check_NULL_CBX(comboBox2, "Mã khen thưởng không được để trống!")) return;
         
            else if (Check_Trung(comboBox2.Text.Trim(), comboBox1.Text.Trim(), dateTimePicker1.Text.Trim()) && flag == "add")
            {
                MessageBox.Show("Sinh viên đã thuộc khen thưởng trong ngày!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Xử lý
            //Thêm
            if (flag == "add")
            {
                String strLenh = "sp_Add_SV_KT";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MAKT", comboBox2.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@MASV", comboBox1.Text.ToString().Trim()));

                SqlParameter parameter1 = new SqlParameter("@NGAY", SqlDbType.Char);
                parameter1.Value = thoigian.ToString("yyyy-MM-dd");
                sqlCommand.Parameters.Add(parameter1);

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Thêm sinh viên khen thưởng thành công!");
                HienThiDuLieu();

            }

            //Xóa
            else if (flag == "delete")
            {
                String strLenh = "sp_Delete_SV_KT";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MAKT", comboBox2.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@MASV", comboBox1.Text.ToString().Trim()));

                SqlParameter parameter1 = new SqlParameter("@NGAY", SqlDbType.Char);
                parameter1.Value = thoigian.ToString("yyyy-MM-dd");
                sqlCommand.Parameters.Add(parameter1);

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Xóa sinh viên khen thưởng thành công!");
                HienThiDuLieu();
            }

            //sửa
            else if (flag == "edit")
            {
               
            }
        }

        //Undo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comboBox1.Text = tempMaSV;
            comboBox2.Text = tempMaKT;
            if (tempNgay != null) { dateTimePicker1.Value = tempNgay; }
                
        }

        //Sự kiện gridview
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            DataTable dt = (DataTable)dataGridView1.DataSource;
            if (dt.Rows.Count > 0)
            {
                comboBox2.Text = dataGridView1.Rows[index].Cells[0].Value.ToString();
                comboBox1.Text = dataGridView1.Rows[index].Cells[1].Value.ToString();
                dateTimePicker1.Text = dataGridView1.Rows[index].Cells[2].Value.ToString();
            }
        }

        //Tìm theo mã khen thưởng
        private void button2_Click(object sender, EventArgs e)
        {
            String strlenh = "select MaKT, MaSV, Ngay from SV_KT where MaKT = '"+textBox1.Text.ToString().Trim()+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Khen Thưởng";
            dataGridView1.Columns[1].HeaderText = "Mã Sinh Viên";
            dataGridView1.Columns[2].HeaderText = "Ngày";

            conn_publisher.Close();
        }

        //Tìm theo mã sinh viên
        private void button3_Click(object sender, EventArgs e)
        {

            String strlenh = "select MaKT, MaSV, Ngay from SV_KT where MaSV = '" + textBox2.Text.ToString().Trim() + "'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Khen Thưởng";
            dataGridView1.Columns[1].HeaderText = "Mã Sinh Viên";
            dataGridView1.Columns[2].HeaderText = "Ngày";

            conn_publisher.Close();
        }

        //Tìm theo ngày tới ngày
        private void button1_Click(object sender, EventArgs e)
        {
            DateTime NgayDau = dateTimePicker2.Value;
            DateTime NgaySau = dateTimePicker3.Value;

            string Dau = NgayDau.ToString("yyyy-MM-dd");
            string Sau = NgaySau.ToString("yyyy-MM-dd");

            if(NgayDau > NgaySau)
            {
                MessageBox.Show("Ngày bắt đầu phải trước ngày kết thúc!");
                return;
            }

            String strlenh = "select MaKT, MaSV, Ngay from SV_KT where Ngay >= '"+Dau+"' and Ngay <= '"+Sau+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Khen Thưởng";
            dataGridView1.Columns[1].HeaderText = "Mã Sinh Viên";
            dataGridView1.Columns[2].HeaderText = "Ngày";

            conn_publisher.Close();
        }

        //Refresh
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            HienThiMenuAdmin_sv_kt();
            HienThiDuLieu();
            comboBox1.Enabled = comboBox2.Enabled = dateTimePicker1.Enabled = false;
            sửaToolStripMenuItem.Enabled = xóaToolStripMenuItem.Enabled = thêmToolStripMenuItem.Enabled = undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            textBox1.Text = textBox2.Text = "";
        }
    }
}
