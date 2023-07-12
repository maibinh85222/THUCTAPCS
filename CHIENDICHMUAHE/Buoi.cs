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
    public partial class Buoi : Form
    {
        SqlConnection conn_publisher = new SqlConnection();
        DataTable dt = new DataTable();
        //Đánh dấu đang là lưu khóa sửa hay thêm
        string flag = "";

        //Tạo biến tạm để lưu dữ liệu quay lại;
        string tempBuoiNgay = "", tempBuoi = "";
        DateTime tempNgay;

        //HIỆN THỊ BẢNG DỮ LIỆU:
        private void HienThiDuLieu()
        {
            String strlenh = "select * from BUOI";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Buổi Ngày";
            dataGridView1.Columns[1].HeaderText = "Buổi";
            dataGridView1.Columns[2].HeaderText = "Ngày";
            conn_publisher.Close();
        }
        public Buoi()
        {
            InitializeComponent();
        }

        //Thoát
        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void HienThiMenuAdmin_Buoi()
        {
            if(Program.mGroup=="TRUONG") menuStrip1.Enabled = panel1.Enabled = true;
            if(Program.mGroup=="SINHVIEN") button1.Visible = true;

        }

        private void Buoi_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            menuStrip1.Enabled = panel1.Enabled = false;
            textBox1.Enabled = cbxbuoi.Enabled = dateTimePicker1.Enabled = false;

            // phân quyền
            HienThiMenuAdmin_Buoi();

            // hiện thị dữ liệu:
            HienThiDuLieu();
            HienThiBuoi();

        }
        //Thêm
        private void thêmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = cbxbuoi.Enabled = dateTimePicker1.Enabled = true;
            textBox1.Text = cbxbuoi.Text = "";
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
            flag = "edit";
            cbxbuoi.Enabled = dateTimePicker1.Enabled = true;
            xóaToolStripMenuItem.Enabled = thêmToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
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
        private bool Check_Trung(String BUOINGAY)
        {
            foreach (DataRow row in dt.Rows)
            {
                String maKhoaGridView = row["BuoiNgay"].ToString();
                if (maKhoaGridView.Trim() == BUOINGAY.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        //kiểm tra trùng buổi
        private bool Check_Trung_Buoi(String BUOI, String NGAY)
        {
            String strlenh = "exec sp_KTBuoiTonTai '" +BUOI+"' , '"+NGAY+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            if (dt.Rows.Count == 0)
            {
                return false;
            }
            return true;
      
        }

        //Kiêm tra buôi đã được phân công
        private bool KiemTraBuoiDaDuocPhan(string BUOINGAY)
        {
            String strlenh = "select *from NHOMTHUCHIEN where BuoiNgay = '" + BUOINGAY + "'";
            dt = Program.ExecSqlDataTable(strlenh);
            if(dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        //Lưu
        private void lưuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime thoigian = dateTimePicker1.Value;

            tempBuoiNgay = textBox1.Text.Trim();
            tempBuoi = cbxbuoi.Text.Trim();
            tempNgay = dateTimePicker1.Value;
            //Kiểm tra dữ liệu nhập vào:
            if (Check_NULL(textBox1, "Buổi ngày không được để trống!")) return;
            if (Check_NULL_CBX(cbxbuoi, "Buổi không được để trống!")) return;
         
            if (textBox1.Text.Trim().Length < 5 || 30 <= textBox1.Text.Trim().Length)
            {
                MessageBox.Show("Buổi ngày phải đúng nằm trong khoảng (5,30) ký tự", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBox1.Text.Contains(" "))
            {
                MessageBox.Show("Buổi ngày không được chứa khoảng trống", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Xử lý
            //Thêm
            if (flag == "add")
            {
                if (Check_Trung(textBox1.Text.Trim()) && flag == "add")
                {
                    MessageBox.Show("Buổi ngày đã tồn tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (Check_Trung_Buoi(cbxbuoi.Text.ToString().Trim(), thoigian.ToString("yyyy-MM-dd")))
                {
                    MessageBox.Show("Ngày và buổi đã tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                } 
                String strLenh = "sp_AddBuoi";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@BUOINGAY", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@BUOI", cbxbuoi.Text.ToString().Trim()));

                SqlParameter parameter1 = new SqlParameter("@NGAY", SqlDbType.Char);
                parameter1.Value = thoigian.ToString("yyyy-MM-dd");
                sqlCommand.Parameters.Add(parameter1);

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Thêm Buổi thành công!");
                HienThiDuLieu();

            }

            //Xóa
            else if (flag == "delete")
            {
                if (KiemTraBuoiDaDuocPhan(tempBuoiNgay))
                {
                    MessageBox.Show("Buổi đã được phân công. Not delete!");
                    return;
                }
                String strLenh = "sp_DeleteBuoi";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@BUOINGAY", textBox1.Text.ToString().Trim()));
                //sqlCommand.Parameters.Add(new SqlParameter("@TENKHOA", textBox2.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Xóa buổi thành công!");
                HienThiDuLieu();
            }

            //sửa
            else if (flag == "edit")
            {
                String strLenh = "sp_UpdateBuoi";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@BUOINGAY", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@BUOI", cbxbuoi.Text.ToString().Trim()));

                SqlParameter parameter1 = new SqlParameter("@NGAY", SqlDbType.Char);
                parameter1.Value = thoigian.ToString("yyyy-MM-dd");
                sqlCommand.Parameters.Add(parameter1);

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Sửa buổi thành công!");
                HienThiDuLieu();
            }
        }

        //Undo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = tempBuoiNgay;
            cbxbuoi.Text = tempBuoi;
            dateTimePicker1.Value = tempNgay;
        }

        //Tạo sự kiện gridview
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            DataTable dt = (DataTable)dataGridView1.DataSource;
            if (dt.Rows.Count > 0)
            {
                textBox1.Text = dataGridView1.Rows[index].Cells[0].Value.ToString();
                cbxbuoi.Text = dataGridView1.Rows[index].Cells[1].Value.ToString();
                dateTimePicker1.Text = dataGridView1.Rows[index].Cells[2].Value.ToString();
            }
        }

        //Refresh
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            HienThiMenuAdmin_Buoi();
            HienThiDuLieu();
            textBox1.Enabled = cbxbuoi.Enabled = dateTimePicker1.Enabled = false;
            sửaToolStripMenuItem.Enabled = xóaToolStripMenuItem.Enabled = thêmToolStripMenuItem.Enabled = undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            comboBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String strlenh = "select *from LayDS_Buoi_NHOM('"+Program.username+"')";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Buổi Ngày";
            dataGridView1.Columns[1].HeaderText = "Buổi";
            dataGridView1.Columns[2].HeaderText = "Ngày";
            conn_publisher.Close();
        }

        //Tìm theo buổi
        private void button4_Click(object sender, EventArgs e)
        {
            String strlenh = "select * from BUOI where Buoi = '"+comboBox1.Text.ToString().Trim()+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Buổi Ngày";
            dataGridView1.Columns[1].HeaderText = "Buổi";
            dataGridView1.Columns[2].HeaderText = "Ngày";
            conn_publisher.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime NgayDau = dateTimePicker2.Value;
            DateTime NgaySau = dateTimePicker3.Value;

            string Dau = NgayDau.ToString("yyyy-MM-dd");
            string Sau = NgaySau.ToString("yyyy-MM-dd");

            if (NgayDau > NgaySau)
            {
                MessageBox.Show("Ngày bắt đầu phải trước ngày kết thúc!");
                return;
            }

            String strlenh = "select * from BUOI where Ngay >= '" + Dau + "' and Ngay <= '" + Sau + "'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Buổi Ngày";
            dataGridView1.Columns[1].HeaderText = "Buổi";
            dataGridView1.Columns[2].HeaderText = "Ngày";
            conn_publisher.Close();

        }

        //Lấy danh sách 3 buổi sáng chiều và tối
        private void HienThiBuoi()
        {       
            cbxbuoi.Items.Add("SANG");
            cbxbuoi.Items.Add("CHIEU");
            cbxbuoi.Items.Add("TOI");

            comboBox1.Items.Add("SANG");
            comboBox1.Items.Add("CHIEU");
            comboBox1.Items.Add("TOI");
        }
    }
}
