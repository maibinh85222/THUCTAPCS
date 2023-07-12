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
    public partial class NhomThucHien : Form
    {
        SqlConnection conn_publisher = new SqlConnection();
        DataTable dt = new DataTable();

        //Đánh dấu đang là lưu khóa sửa hay thêm
        string flag = "";

        //Tạo biến tạm để lưu dữ liệu quay lại;
        string tempBuoi = "", tempNhom = "", tempCongViec = "";

        //HIỆN THỊ BẢNG DỮ LIỆU:
        private void HienThiDuLieu()
        {
            String strlenh = "select * from NHOMTHUCHIEN";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Buổi Ngày";
            dataGridView1.Columns[1].HeaderText = "Nhóm";
            dataGridView1.Columns[2].HeaderText = "Mã Công Việc";
            conn_publisher.Close();
        }

        //LẤY DANH SACH BUOI
        private void LayDSBUOI()
        {
            DataTable dt = new DataTable();
            String strlenh = "select BuoiNgay from BUOI";
            dt = Program.ExecSqlDataTable(strlenh);

            comboBox1.DataSource = dt;
            //comboBox1.ValueMember = "MaKhoa";
            comboBox1.DisplayMember = "BuoiNgay";
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
            //comboBox1.ValueMember = "MaKhoa";
            comboBox2.DisplayMember = "MaNhom";
            //comboBox1.SelectedIndex = 0;
            conn_publisher.Close();
        }

        //LẤY DANH SACH CONG VIEC
        private void LayDSCONGVIEC()
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaCV from CONGVIEC";
            dt = Program.ExecSqlDataTable(strlenh);

            comboBox3.DataSource = dt;
            //comboBox1.ValueMember = "MaKhoa";
            comboBox3.DisplayMember = "MaCV";
            //comboBox1.SelectedIndex = 0;
            conn_publisher.Close();
        }
        public NhomThucHien()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        //Thoát
        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void HienThiMenuAdmin_NhomThucHien()
        {
            if(Program.mGroup=="TRUONG"||Program.mGroup=="GIANGVIEN") menuStrip1.Enabled = panel1.Enabled = true;
            if(Program.mGroup=="SINHVIEN") button1.Visible = true;

        }

        //Kiểm tra ngày trong buổi ngày có hợp lệ
        private bool KIEMTRABUOINGAY(string BUOINGAY, string macv)
        {
            DataTable dt = new DataTable();
            String strlenh = "select *from LAY_CV_THUOC_BUOI('"+BUOINGAY+"','"+macv+"')";
            dt = Program.ExecSqlDataTable(strlenh);
            
            if(dt.Rows.Count > 0)
            {
                return true; // trả về tồn tại
            }
          

            return false; // trả về không tồn tại
        }

        private void NhomThucHien_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            menuStrip1.Enabled = panel1.Enabled = false;
            comboBox1.Enabled = comboBox2.Enabled = comboBox3.Enabled = false;
            // phân quyền
            HienThiMenuAdmin_NhomThucHien();


            // Hiện thị dữ liệu
            HienThiDuLieu();
            LayDSNHOM();
            LayDSCONGVIEC();
            LayDSBUOI();

        }

        //Hiện thi dữ liệu gridview
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            DataTable dt = (DataTable)dataGridView1.DataSource;
            if (dt.Rows.Count > 0)
            {
                comboBox1.Text = dataGridView1.Rows[index].Cells[0].Value.ToString();
                comboBox2.Text = dataGridView1.Rows[index].Cells[1].Value.ToString();
                comboBox3.Text = dataGridView1.Rows[index].Cells[2].Value.ToString();
            }
        }

        //Thêm
        private void thêmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comboBox1.Enabled = comboBox2.Enabled = comboBox3.Enabled = true;
            comboBox1.Text = comboBox2.Text = comboBox3.Text = "";
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
            MessageBox.Show("Không thể sửa nhóm thực hiện. Chỉ thêm và xóa!");
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
        private bool Check_Trung(String MAAP)
        {
            foreach (DataRow row in dt.Rows)
            {
                String maKhoaGridView = row["MaAp"].ToString();
                if (maKhoaGridView.Trim() == MAAP.Trim())
                {
                    return true;
                }
            }
            return false;
        }
        //Lưu
        private void lưuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tempBuoi = comboBox1.Text.Trim();
            tempNhom = comboBox2.Text.Trim();
            tempCongViec = comboBox3.Text.Trim();
            //Kiểm tra dữ liệu nhập vào:
            if (Check_NULL_CBX(comboBox1, "Buổi không được để trống!")) return;
            if (Check_NULL_CBX(comboBox2, "Nhóm không được để trống!")) return;
            if (Check_NULL_CBX(comboBox3, "Công việc không được để trống!")) return;
            
          
           /* else if (Check_Trung(textBox1.Text.Trim()) && flag == "add")
            {
                MessageBox.Show("Mã ấp đã tồn tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }*/

            //Xử lý
            //Thêm
            if (flag == "add")
            {
                if (KIEMTRABUOINGAY(tempBuoi, tempCongViec) ==false)
                {
                    MessageBox.Show("Ngày trong buổi ngày không thuộc khoảng thời gian của công việc!");
                    return;
                }
                String strLenh = "sp_AddNhomThucHien";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@BUOINGAY", comboBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@NHOM", comboBox2.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@MACV", comboBox3.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Thêm nhóm thực hiện thành công!");
                HienThiDuLieu();

            }

            //Xóa
            else if (flag == "delete")
            {
                String strLenh = "sp_XoaThucHien";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@BUOINGAY", comboBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@NHOM", comboBox2.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@MACV", comboBox3.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Thêm nhóm thực hiện thành công!");
                HienThiDuLieu();
            }

            //sửa
            else if (flag == "edit")
            {
               
            }
        }

        //Nút hiện thị cá nhân
        private void button1_Click(object sender, EventArgs e)
        {
            String strlenh = "select *from LayDS_NHOMTHUCHIEN_NHOM('"+Program.username+"')";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Buổi Ngày";
            dataGridView1.Columns[1].HeaderText = "Nhóm";
            dataGridView1.Columns[2].HeaderText = "Mã Công Việc";
            conn_publisher.Close();
        }

        //Tìm theo buổi ngày
        private void button3_Click(object sender, EventArgs e)
        {
            String strlenh = "select * from NHOMTHUCHIEN where BuoiNgay = '"+textBox3.Text.ToString().Trim()+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Buổi Ngày";
            dataGridView1.Columns[1].HeaderText = "Nhóm";
            dataGridView1.Columns[2].HeaderText = "Mã Công Việc";
            conn_publisher.Close();
        }

        //Tìm theo nhóm
        private void button4_Click(object sender, EventArgs e)
        {
            String strlenh = "select * from NHOMTHUCHIEN where Nhom = '"+ textBox2.Text.ToString().Trim()+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Buổi Ngày";
            dataGridView1.Columns[1].HeaderText = "Nhóm";
            dataGridView1.Columns[2].HeaderText = "Mã Công Việc";
            conn_publisher.Close();
        }

        //Tìm theo công việc
        private void button2_Click(object sender, EventArgs e)
        {
            String strlenh = "select * from NHOMTHUCHIEN where MaCV = '" + textBox4.Text.ToString().Trim() + "'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Buổi Ngày";
            dataGridView1.Columns[1].HeaderText = "Nhóm";
            dataGridView1.Columns[2].HeaderText = "Mã Công Việc";
            conn_publisher.Close();
        }

        //Undo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comboBox1.Text = tempBuoi;
            comboBox2.Text = tempNhom;
            comboBox3.Text = tempCongViec;
        }

        //Refresh
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            HienThiMenuAdmin_NhomThucHien();
            HienThiDuLieu();
            comboBox1.Enabled = comboBox2.Enabled = comboBox3.Enabled = false;
            sửaToolStripMenuItem.Enabled = xóaToolStripMenuItem.Enabled = thêmToolStripMenuItem.Enabled = undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            textBox3.Text = textBox2.Text = textBox4.Text = "";
        }
    }
}
