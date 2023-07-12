using DevExpress.XtraEditors.Mask.Design;
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
    public partial class Nhom : Form
    {
        SqlConnection conn_publisher = new SqlConnection();
        DataTable dt = new DataTable();

        //Đánh dấu đang là lưu khóa sửa hay thêm
        string flag = "";

        //Tạo biến tạm để lưu dữ liệu quay lại;
        string tempMaNhom = "", tempTenNhom = "", tempTruongNhom = "", tempMaNha ="", tempSoLuong ="";
        string tempTruongNhom_s = ""; int tempsl_s = 0;
        string tempMaNha_s = "";

        //HIỆN THỊ BẢNG DỮ LIỆU:
        private void HienThiDuLieu()
        {
            String strlenh = "select MaNhom, TenNhom, SoLuongSV, MaTruongNhom, MaNha from NHOM";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Nhóm";
            dataGridView1.Columns[1].HeaderText = "Tên Nhóm";
            dataGridView1.Columns[2].HeaderText = "Số Lượng Sinh Viên";
            dataGridView1.Columns[3].HeaderText = "Mã Trưởng Nhóm";
            dataGridView1.Columns[4].HeaderText = "Mã Nhà";
            conn_publisher.Close();
        }

        //LẤY DANH Nha
        private void LayDSNHA()
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaNha from NHA";
            dt = Program.ExecSqlDataTable(strlenh);

            comboBox2.DataSource = dt;
            //comboBox1.ValueMember = "MaKhoa";
            comboBox2.DisplayMember = "MaNha";
            //comboBox1.SelectedIndex = 0;
            conn_publisher.Close();
        }

        //LẤY DANH Nha
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
        public Nhom()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        //Thoát
        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        // PHÂN QUYỀN
        public void HienThiMenuAdmin_Nhom()
        {
            if(Program.mGroup == "TRUONG" || Program.mGroup == "GIANGVIEN")
                menuStrip1.Enabled = panel1.Enabled = true;
            if(Program.mGroup =="SINHVIEN") button1.Visible = true;
        }

        private void Nhom_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            panel1.Enabled = menuStrip1.Enabled = false;
            textBox1.Enabled = textBox2.Enabled = comboBox1.Enabled = comboBox2.Enabled = comboBox3.Enabled = false;
            HienThiMenuAdmin_Nhom();
            //Hiện thị lên bảng GridView
            HienThiDuLieu();
            LayDSNHA();
            LayDSSV();
            HienThiSoLuongSv();
        }

        //Kiểm tra nhóm có được xóa không
        private bool KiemTraDeleteNhom(string MANHOM)
        {
            String strlenh = "select MaSV from SINHVIEN where MaNhom = '" + MANHOM + "'";
            dt = Program.ExecSqlDataTable(strlenh);
            if (dt.Rows.Count > 0)
            {
                return false;
            }

            String strlenh1 = "select MaNha from NHA where MaNhom = '" + MANHOM + "'";
            dt = Program.ExecSqlDataTable(strlenh1);
            if (dt.Rows.Count > 0)
            {
                return false;
            }

            return true;
        }

        //Thêm
        private void theToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = textBox2.Enabled = comboBox1.Enabled = comboBox2.Enabled = comboBox3.Enabled = true;
            textBox1.Text = textBox2.Text = comboBox1.Text = comboBox2.Text = comboBox3.Text = "";
            xóaToolStripMenuItem.Enabled = sửaToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
            flag = "add";
        }

        //Xóa
        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = "delete";

            theToolStripMenuItem.Enabled = sửaToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
        }

        //Sửa
        private void sửaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = "edit";
            textBox2.Enabled = true; comboBox1.Enabled =comboBox3.Enabled = comboBox2.Enabled = true;
            xóaToolStripMenuItem.Enabled = theToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
            tempTruongNhom_s = comboBox1.Text.ToString().Trim();
            tempsl_s = int.Parse(comboBox3.Text.ToString().Trim());
            dataGridView1.Enabled = false;
            tempMaNha_s = comboBox2.Text.ToString().Trim();
            dataGridView1.Enabled = false;
        }

        //Undo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = tempMaNhom;
            textBox2.Text = tempTenNhom;
            comboBox1.Text = tempTruongNhom;
            comboBox2.Text = tempMaNha;
            comboBox3.Text = tempSoLuong;
        }

        //refresh
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            HienThiMenuAdmin_Nhom();
            HienThiDuLieu();
            textBox1.Enabled = textBox2.Enabled = comboBox1.Enabled = comboBox2.Enabled = comboBox3.Enabled = false;
            sửaToolStripMenuItem.Enabled = xóaToolStripMenuItem.Enabled = theToolStripMenuItem.Enabled = undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            textBox3.Text = "";
            dataGridView1.Enabled = true;
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

        //Sự kiện gridview
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            DataTable dt = (DataTable)dataGridView1.DataSource;
            if (dt.Rows.Count > 0)
            {
                textBox1.Text = dataGridView1.Rows[index].Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.Rows[index].Cells[1].Value.ToString();
                comboBox3.Text = dataGridView1.Rows[index].Cells[2].Value.ToString();
                comboBox1.Text = dataGridView1.Rows[index].Cells[3].Value.ToString();
                comboBox2.Text = dataGridView1.Rows[index].Cells[4].Value.ToString();
            }
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
        private bool Check_Trung(String MANHOM)
        {
            foreach (DataRow row in dt.Rows)
            {
                String maKhoaGridView = row["MaNhom"].ToString();
                if (maKhoaGridView.Trim() == MANHOM.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        //Lấy tất cả các sinh viên thuộc nhóm
        private bool Check_SV_Nhom(String MANHOM, String MASV)
        {
            DataTable dt1 = new DataTable();
            String strlenh1 = "select MaSV from SINHVIEN where MaNhom = '"+ MANHOM+"'";
            dt1 = Program.ExecSqlDataTable(strlenh1);

            foreach (DataRow row in dt1.Rows)
            {
                String maKhoaGridView = row["MaSV"].ToString();
                if (maKhoaGridView.Trim() == MASV.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        //Nút cá nhân quyền sinh viên
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt1 = new DataTable();
            String strlenh1 = "select MaNhom from SINHVIEN where MaSV = '" + Program.username + "'";
            dt1 = Program.ExecSqlDataTable(strlenh1);
            string NHOM = "";

            foreach (DataRow row in dt1.Rows)
            {
                String maKhoaGridView = row["MaNhom"].ToString();
                if (maKhoaGridView.Trim() != "")
                {
                    NHOM = maKhoaGridView.Trim();
                    break;
                }
            }

            String strlenh = "select MaNhom, TenNhom, SoLuongSV, MaTruongNhom, MaNha from NHOM where MaNhom = '"+NHOM+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Nhóm";
            dataGridView1.Columns[1].HeaderText = "Tên Nhóm";
            dataGridView1.Columns[2].HeaderText = "Số Lượng Sinh Viên";
            dataGridView1.Columns[3].HeaderText = "Mã Trưởng Nhóm";
            dataGridView1.Columns[4].HeaderText = "Mã Nhà";
            conn_publisher.Close();
        }

        //Tìm kiếm nhóm
        private void button2_Click(object sender, EventArgs e)
        {
            String strlenh = "select MaNhom, TenNhom, SoLuongSV, MaTruongNhom, MaNha from NHOM where MaNhom = '"+textBox3.Text.ToString().Trim()+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Nhóm";
            dataGridView1.Columns[1].HeaderText = "Tên Nhóm";
            dataGridView1.Columns[2].HeaderText = "Số Lượng Sinh Viên";
            dataGridView1.Columns[3].HeaderText = "Mã Trưởng Nhóm";
            dataGridView1.Columns[4].HeaderText = "Mã Nhà";
            conn_publisher.Close();
        }

        //Lấy tất cả các sinh viên thuộc nhóm
        private bool Check_SV(String MASV)
        {
            DataTable dt1 = new DataTable();
            String strlenh1 = "select MaSV from SINHVIEN where MaSV = '"+ MASV+ "' and MaNhom IS NULL";
            dt1 = Program.ExecSqlDataTable(strlenh1);
            if(dt1.Rows.Count > 0) { return true; }
            return false;
        }

        
        //Kiểm tra nhà đã thuộc nhóm chưa
        private bool Check_Nha(String MANHA)
        {
            foreach (DataRow row in dt.Rows)
            {
                String maKhoaGridView = row["MaNha"].ToString();
                if (maKhoaGridView.Trim() == MANHA.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        //Kiểm tra sinh viên nhóm nếu nhóm đã đủ thì không cho giảm số lượng thành viên
        private bool KIEMTRASOLUONGSV(string MANHOM, int sl)
        {
            DataTable dt1 = new DataTable();
            String strlenh1 = "select MaSV from SINHVIEN where MaNhom = '"+ MANHOM+"'";
            dt1 = Program.ExecSqlDataTable(strlenh1);
            if (dt1.Rows.Count <= sl) {
                return true; }
            return false;
        }

        //Kiểm tra nhà đã tồn tại:
        private bool KiemTraNhaTrongNhom(string MANHA)
        {
            DataTable dt1 = new DataTable();
            String strlenh1 = "select MaNha from NHA where MaNha = '" + MANHA + "' and MaNhom is null";
            dt1 = Program.ExecSqlDataTable(strlenh1);
            if (dt1.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        //Lưu
        private void lưuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tempMaNhom = textBox1.Text.Trim();
            tempTenNhom = textBox2.Text.Trim();
            tempTruongNhom = comboBox1.Text.Trim();
            tempMaNha = comboBox2.Text.Trim();
            tempSoLuong = comboBox3.Text.Trim();

            tempsl_s = int.Parse(comboBox3.Text.ToString().Trim());

            //Kiểm tra dữ liệu nhập vào:
            if (Check_NULL(textBox1, "Mã nhóm không được để trống!")) return;
            if (Check_NULL(textBox2, "Tên nhóm không được để trống!")) return;
            if (Check_NULL_CBX(comboBox1, "Mã trưởng nhóm không được để trống!")) return;
            if (Check_NULL_CBX(comboBox2, "Mã nhà không được để trống!")) return;
            if (Check_NULL_CBX(comboBox3, "số lượng không được để trống!")) return;

            if (textBox1.Text.Trim().Length != 5)
            {
                MessageBox.Show("Mã nhóm phải đúng 5 ký tự", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBox1.Text.Contains(" "))
            {
                MessageBox.Show("Mã nhóm không được chứa khoảng trống", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Xử lý
            //Thêm
            if (flag == "add")
            {
                if (Check_Trung(textBox1.Text.Trim()))
                {
                    MessageBox.Show("Mã nhóm đã tồn tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (Check_SV(tempTruongNhom)==false)
                {
                    MessageBox.Show("Vui lòng chọn sinh viên chưa thuộc nhóm để làm nhóm trưởng!");
                    return;
                }

                if (Check_Nha(tempMaNha))
                {
                    MessageBox.Show("Mã nhà đã có nhóm. Vui lòng chọn nhà khác!");
                    return;
                }

                String strLenh = "sp_AddNhom";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MANHOM", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@TENNHOM", textBox2.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@SOLUONG", comboBox3.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@MATRUONGNHOM", comboBox1.Text.ToString().Trim()));

                if (comboBox2.Text.ToString().Trim() == "")
                {

                    sqlCommand.Parameters.Add(new SqlParameter("@MANHA", DBNull.Value));

                }
                else if (comboBox2.Text.ToString().Trim() != "")
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@MANHA", comboBox2.Text.ToString().Trim()));
                }

                //sqlCommand.Parameters.Add(new SqlParameter("@MANHA", comboBox2.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Thêm nhóm thành công!");
                HienThiDuLieu();

            }

            //Xóa
            else if (flag == "delete")
            {
                if (KiemTraDeleteNhom(tempMaNhom) == false)
                {
                    MessageBox.Show("Không thể xóa.Nhóm đã có sinh viên hoặc đã có nhà!");
                    return;
                }
                String strLenh = "sp_DeleteNhom";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MANHOM", textBox1.Text.ToString().Trim()));
                //sqlCommand.Parameters.Add(new SqlParameter("@TENKHOA", textBox2.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Xóa nhóm thành công!");
                HienThiDuLieu();
            }

            //sửa
            else if (flag == "edit")
            {

                if (KiemTraNhaTrongNhom(tempMaNha) == false && tempMaNha_s != tempMaNha)
                {
                    MessageBox.Show("Mã nhà thuộc nhóm khác!");
                    return;
                }
                if (KIEMTRASOLUONGSV(tempMaNhom, int.Parse(comboBox3.Text.ToString().Trim())) ==false )
                {
                    MessageBox.Show("Số lượng không hợp lệ vì thành viên trong nhóm nhiều hơn số lượng hiện tại");
                    return;
                }
                if (Check_SV_Nhom(tempMaNhom, tempTruongNhom)==false && tempTruongNhom_s != tempTruongNhom)
                {
                    MessageBox.Show("Vui lòng chọn sinh viên thuộc nhóm để làm nhóm trưởng!");
                    return;
                }

                /*if (Check_Nha(tempMaNha) && tempMaNha != "")
                {
                    MessageBox.Show("Mã nhà đã có nhóm. Vui lòng chọn nhà khác!");
                    return;
                }*/

                String strLenh = "sp_EditNhom";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MANHOM", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@TENNHOM", textBox2.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@SOLUONG", comboBox3.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@MATRUONGNHOM", comboBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@MANHA", comboBox2.Text.ToString().Trim()));
                /* if (comboBox2.Text.ToString().Trim() == "")
                 {

                     sqlCommand.Parameters.Add(new SqlParameter("@MANHA", DBNull.Value));

                 }
                 else if (comboBox2.Text.ToString().Trim() != "")
                 {
                     sqlCommand.Parameters.Add(new SqlParameter("@MANHA", comboBox2.Text.ToString().Trim()));
                 }*/

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Sửa nhóm thành công!");
                HienThiDuLieu();
            }
        }

        //Sử lý chế độ số lượng.
        private void HienThiSoLuongSv()
        {
            comboBox3.Items.Add("3");
            comboBox3.Items.Add("4");
            comboBox3.Items.Add("5");
        }
    }
}
