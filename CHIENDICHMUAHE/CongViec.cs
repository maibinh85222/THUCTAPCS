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
    public partial class CongViec : Form
    {
        SqlConnection conn_publisher = new SqlConnection();
        DataTable dt = new DataTable();

        //Đánh dấu đang là lưu khóa sửa hay thêm
        string flag = "";

        //Tạo biến tạm để lưu dữ liệu quay lại;
        string tempMaCV = "", tempTenCV = "", tempMaAp = "";

        //HIỆN THỊ BẢNG DỮ LIỆU:
        private void HienThiDuLieu()
        {
            String strlenh = "select * from CONGVIEC";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Công Việc";
            dataGridView1.Columns[1].HeaderText = "Tên Công Việc";
            dataGridView1.Columns[2].HeaderText = "Mã Ấp";
            dataGridView1.Columns[3].HeaderText = "Công";
            dataGridView1.Columns[4].HeaderText = "Ngày bắt đầu";
            dataGridView1.Columns[5].HeaderText = "Ngày kết thúc";
            conn_publisher.Close();
        }

        //LẤY DANH SACH AP
        private void LayDSAP()
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaAp from AP";
            dt = Program.ExecSqlDataTable(strlenh);

            comboBox1.DataSource = dt;
            //comboBox1.ValueMember = "MaKhoa";
            comboBox1.DisplayMember = "MaAp";
            //comboBox1.SelectedIndex = 0;
            conn_publisher.Close();
        }
        public CongViec()
        {
            InitializeComponent();
        }

        //Thoát
        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void HienThiMenuAdmin_CongViec()
        {
            if(Program.mGroup=="TRUONG") menuStrip1.Enabled = panel1.Enabled = true;
            if (Program.mGroup == "SINHVIEN") button1.Visible = true;
        }
        private void CongViec_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            menuStrip1.Enabled = panel1.Enabled = false;
            textBox1.Enabled = textBox2.Enabled = comboBox1.Enabled = false;
            numericUpDown1.Enabled = dateTimePicker1.Enabled = dateTimePicker2.Enabled = false;
            // phân quyền
            HienThiMenuAdmin_CongViec();

            // hiện thị dữ liệu gridview
            HienThiDuLieu();
            LayDSAP();
        }

        //Thêm
        private void thêmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = textBox2.Enabled = comboBox1.Enabled = true;
            numericUpDown1.Enabled = dateTimePicker1.Enabled = dateTimePicker2.Enabled = true;
            textBox1.Text = textBox2.Text = comboBox1.Text = "";
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
            textBox2.Enabled = true;
            xóaToolStripMenuItem.Enabled = thêmToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
            numericUpDown1.Enabled = dateTimePicker2.Enabled = true;
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
        private bool Check_Trung(String MACV)
        {
            foreach (DataRow row in dt.Rows)
            {
                String maKhoaGridView = row["MaCV"].ToString();
                if (maKhoaGridView.Trim() == MACV.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        //Kiểm tra công việc đã được phân công chưa
        private bool KiemTraCvTrongNhomThucHien(string MACV)
        {
            DataTable dt = new DataTable();
            String strlenh = "select *from NHOMTHUCHIEN where MaCV = '"+MACV+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            if(dt.Rows.Count > 0)
            {
                return true; // đã có trong phân công not delete
            }
            return false;

        }
        //Lưu
        private void lưuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tempMaCV = textBox1.Text.Trim();
            tempTenCV = textBox2.Text.Trim();
            tempMaAp = comboBox1.Text.Trim();
            
            //Kiểm tra dữ liệu nhập vào:
            if (Check_NULL(textBox1, "Mã công việc không được để trống!")) return;
            if (Check_NULL(textBox2, "Tên công việc không được để trống!")) return;
            if (Check_NULL_CBX(comboBox1, "Mã ấp không được để trống!")) return;
            if (textBox1.Text.Trim().Length != 5)
            {
                MessageBox.Show("Mã công việc phải đúng 5 ký tự", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBox1.Text.Contains(" "))
            {
                MessageBox.Show("Mã công việc không được chứa khoảng trống", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Xử lý
            //Thêm
            if (flag == "add")
            {
                if (Check_Trung(textBox1.Text.Trim()) && flag == "add")
                {
                    MessageBox.Show("Mã công việc đã tồn tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (dateTimePicker1.Value < DateTime.Now)
                {
                    MessageBox.Show("Ngày bắt đầu phải từ ngày nhập trở đi!");
                    return;
                }
                if (dateTimePicker1.Value >= dateTimePicker2.Value)
                {
                    MessageBox.Show("Ngày bắt đầu phải nhỏ hơn ngày kết thúc!");
                    return;
                }

                String strLenh = "sp_AddCV";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MACV", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@TENCV", textBox2.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@MAAP", comboBox1.Text.ToString().Trim()));

                sqlCommand.Parameters.Add(new SqlParameter("@CONG", numericUpDown1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@NGAYBD", dateTimePicker1.Value));
                sqlCommand.Parameters.Add(new SqlParameter("@NGAYKT", dateTimePicker2.Value));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Thêm công việc thành công!");
                HienThiDuLieu();

            }

            //Xóa
            else if (flag == "delete")
            {
                if (KiemTraCvTrongNhomThucHien(tempMaCV))
                {
                    MessageBox.Show("Công việc đã được phân công! Not delete!");
                    return;
                }
                String strLenh = "sp_DeleteCV";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MACV", textBox1.Text.ToString().Trim()));
                //sqlCommand.Parameters.Add(new SqlParameter("@TENKHOA", textBox2.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Xóa công việc thành công!");
                HienThiDuLieu();
            }

            //sửa
            else if (flag == "edit")
            {
               
                if (dateTimePicker1.Value >= dateTimePicker2.Value)
                {
                    MessageBox.Show("Ngày bắt đầu phải nhỏ hơn ngày kết thúc!");
                    return;
                }

                String strLenh = "sp_EditCV";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MACV", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@TENCV", textBox2.Text.ToString().Trim()));
                //sqlCommand.Parameters.Add(new SqlParameter("@MAAP", comboBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@CONG", numericUpDown1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@NGAYBD", dateTimePicker1.Value));
                sqlCommand.Parameters.Add(new SqlParameter("@NGAYKT", dateTimePicker2.Value));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Sửa công việc thành công!");
                HienThiDuLieu();
            }
        }

        //sự kiện gridview
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            DataTable dt = (DataTable)dataGridView1.DataSource;
            if (dt.Rows.Count > 0)
            {
                textBox1.Text = dataGridView1.Rows[index].Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.Rows[index].Cells[1].Value.ToString();
                comboBox1.Text = dataGridView1.Rows[index].Cells[2].Value.ToString();

                numericUpDown1.Text = dataGridView1.Rows[index].Cells[3].Value.ToString();
                dateTimePicker1.Text = dataGridView1.Rows[index].Cells[4].Value.ToString();
                dateTimePicker2.Text = dataGridView1.Rows[index].Cells[5].Value.ToString();

            }
        }

        //Hiện thị cá nhân sinh viên
        private void button1_Click(object sender, EventArgs e)
        {
            String strlenh = "select * from LayDS_CV_NHOM('" + Program.username+"')";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Công Việc";
            dataGridView1.Columns[1].HeaderText = "Tên Công Việc";
            dataGridView1.Columns[2].HeaderText = "Mã Ấp";
            dataGridView1.Columns[3].HeaderText = "Công";
            dataGridView1.Columns[4].HeaderText = "Ngày bắt đầu";
            dataGridView1.Columns[5].HeaderText = "Ngày kết thúc";

            conn_publisher.Close();
        }

        //Tìm theo mã công việc
        private void button3_Click(object sender, EventArgs e)
        {
            String strlenh = "select * from CONGVIEC where MaCV = '"+textBox3.Text.ToString().Trim()+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Công Việc";
            dataGridView1.Columns[1].HeaderText = "Tên Công Việc";
            dataGridView1.Columns[2].HeaderText = "Mã Ấp";
            dataGridView1.Columns[3].HeaderText = "Công";
            dataGridView1.Columns[4].HeaderText = "Ngày bắt đầu";
            dataGridView1.Columns[5].HeaderText = "Ngày kết thúc";

            conn_publisher.Close();
        }

        //Tìm theo ấp
        private void button2_Click(object sender, EventArgs e)
        {
            String strlenh = "select * from CONGVIEC where MaAp = '" + textBox4.Text.ToString().Trim() + "'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Công Việc";
            dataGridView1.Columns[1].HeaderText = "Tên Công Việc";
            dataGridView1.Columns[2].HeaderText = "Mã Ấp";
            dataGridView1.Columns[3].HeaderText = "Công";
            dataGridView1.Columns[4].HeaderText = "Ngày bắt đầu";
            dataGridView1.Columns[5].HeaderText = "Ngày kết thúc";

            conn_publisher.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        //Undo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = tempMaCV;
            textBox2.Text = tempTenCV;
            comboBox1.Text = tempMaAp;
        }

        //refresh
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            HienThiMenuAdmin_CongViec();
            HienThiDuLieu();
            textBox1.Enabled = textBox2.Enabled = comboBox1.Enabled = false;
            numericUpDown1.Enabled = dateTimePicker1.Enabled = dateTimePicker2.Enabled = false;
            sửaToolStripMenuItem.Enabled = xóaToolStripMenuItem.Enabled = thêmToolStripMenuItem.Enabled = undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            textBox3.Text = textBox4.Text = "";
        }
    }
}
