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
    public partial class GiangVien : Form
    {
        SqlConnection conn_publisher = new SqlConnection();
        DataTable dt = new DataTable();
        string flag = "";

        //Tạo biến tạm để lưu dữ liệu quay lại;
        string tempMaGV = "", tempTenGV = "", tempMaKhoa = "", tempMaDoiGiamSat = "";
        public GiangVien()
        {
            InitializeComponent();
        }

        // phan quyen:
        public void HienThiMenuAdmin_GiangVien()
        {
            if (Program.mGroup == "TRUONG")
            {
                textBox1.Enabled = textBox2.Enabled = comboBox1.Enabled = comboBox2.Enabled = false;
                menuStrip1.Enabled = panel1.Enabled = true;
                lưuToolStripMenuItem.Enabled = false;
            }

            if (Program.mGroup == "GIANGVIEN")
            {
                menuStrip1.Enabled = panel1.Enabled = false;
                //textBox1.Enabled = textBox2.Enabled = false;
                //lưuToolStripMenuItem.Enabled = false;
            }

        }

        // Hien thi du lieu:
        private void HienThiDuLieuGiangVien()
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaGV, TenGV, MaKhoa, MaDoiGiamSat from GIANGVIEN";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Giảng Viên";
            dataGridView1.Columns[1].HeaderText = "Tên Giảng Viên";
            dataGridView1.Columns[2].HeaderText = "Mã Khoa";
            dataGridView1.Columns[3].HeaderText = "Mã Đội Giám Sát";
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

        //LẤY DANH SACH KHOA
        private void LayDS_MDGS()
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaDoiGiamSat from DOIGIAMSAT";
            dt = Program.ExecSqlDataTable(strlenh);

            comboBox2.DataSource = dt;
            comboBox2.ValueMember = "MaDoiGiamSat";
            
            //comboBox1.SelectedIndex = 0;
            conn_publisher.Close();
        }

        //Kiểm tra text có rỗng không textBox
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

        //Kiểm tra text có rỗng không comboBox
        private bool Check_NULL_cBox(ComboBox cb, string str)
        {
            if (cb.Text.Trim().Equals(""))
            {
                MessageBox.Show(str, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cb.Focus();
                return true;
            }
            return false;

        }

        //Kiểm tra mã có trùng không
        private bool Check_Trung(String MAGV)
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaGV from GIANGVIEN";
            dt = Program.ExecSqlDataTable(strlenh);

            foreach (DataRow row in dt.Rows)
            {
                String maDBGridView = row["MaGV"].ToString();
                if (maDBGridView.Trim() == MAGV.Trim())
                {
                    conn_publisher.Close();
                    return true;    
                }
            }

            conn_publisher.Close();
            return false;
           
        }

        //Kiểm tra mã đội giám sát có trùng không
        private bool Check_full_DGS(String MADGS)
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaGV, MaDoiGiamSat from GIANGVIEN";
            dt = Program.ExecSqlDataTable(strlenh);
            int dem = 0;
            foreach (DataRow row in dt.Rows)
            {
                String maDBGridView = row["MaDoiGiamSat"].ToString();
                if (maDBGridView.Trim() == MADGS.Trim())
                {
                    dem++;
                    if (dem == 2)
                    {
                        conn_publisher.Close();
                        return true;
                    }
                    
                }
            }

            conn_publisher.Close();
            return false;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        //Thoat
        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        //-------------------------------------Man Hinh load hien thi du lieu 
        private void GiangVien_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            menuStrip1.Enabled = panel1.Enabled = false;
            HienThiMenuAdmin_GiangVien();
            //Hien thi du lieu bang gridview
            HienThiDuLieuGiangVien();
            LayDSKHOA();
            LayDS_MDGS();
            //HIỆN THỊ KHOA
            //if (Program.KetNoi() == 0) return;
            //LayDSKHOA("select *from KHOA");


        }

        //Them
        private void thêmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = textBox2.Enabled = comboBox1.Enabled = comboBox2.Enabled = true;
            textBox1.Text = textBox2.Text = comboBox1.Text = comboBox2.Text = "";
            xóaToolStripMenuItem.Enabled = sửaToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = reToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
            flag = "add";
        }

        //Sua
        private void sửaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = "edit";
            textBox2.Enabled = comboBox2.Enabled = true;
            xóaToolStripMenuItem.Enabled = theeToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = reToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
        }

        //Xoa
        private void suaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = "delete";
            theeToolStripMenuItem.Enabled = sửaToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = reToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
        }

        //Refresh
        private void refrheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            HienThiDuLieuGiangVien();
            textBox1.Enabled = textBox2.Enabled = comboBox1.Enabled = comboBox2.Enabled = false;
            sửaToolStripMenuItem.Enabled = xóaToolStripMenuItem.Enabled = theeToolStripMenuItem.Enabled = undoToolStripMenuItem.Enabled = reToolStripMenuItem.Enabled = true;
            textBox3.Text = "";
        }

        //Tìm kiếm giảng viên:
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaGV, TenGV, MaKhoa, MaDoiGiamSat from GIANGVIEN where MaGV = '"+ textBox3.Text.ToString().Trim()+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Giảng Viên";
            dataGridView1.Columns[1].HeaderText = "Tên Giảng Viên";
            dataGridView1.Columns[2].HeaderText = "Mã Khoa";
            dataGridView1.Columns[3].HeaderText = "Mã Đội Giám Sát";
            conn_publisher.Close();
        }

        //Kiểm tra gv có thuộc đổi giám sát không
        private bool KiemTraDeleteKhoa(string MAGV)
        {
            String strlenh = "select MaGV from GIANGVIEN where MaDoiGiamSat is null and MaGV = '"+MAGV+"'";
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
            tempMaGV = textBox1.Text.Trim();
            tempTenGV = textBox2.Text.Trim();
            tempMaKhoa = comboBox1.Text.Trim();
            tempMaDoiGiamSat = comboBox2.Text.Trim();
            //Kiểm tra dữ liệu nhập vào:
            if (Check_NULL(textBox1, "Mã giảng viên không được để trống!")) return;
            if (Check_NULL_cBox(comboBox1, "Mã khoa không được trống!")) return;
            if (Check_NULL(textBox2, "Tên giảng viên không được để trống!")) return;
            if (textBox1.Text.Trim().Length != 5)
            {
                MessageBox.Show("Mã giảng viên phải đúng 5 ký tự", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBox1.Text.Contains(" "))
            {
                MessageBox.Show("Mã giảng viên không được chứa khoảng trống", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else if (Check_Trung(textBox1.Text.Trim()) && flag == "add")
            {
                MessageBox.Show("Mã giảng viên đã tồn tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Xử lý
            //Thêm
            if (flag == "add")
            {
                if (Check_full_DGS(tempMaDoiGiamSat))
                {
                    MessageBox.Show("Đội giám sát đã đủ số lượng giảng viên!"); return;
                }
                String strLenh = "sp_InsertGiangVien";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MAGV", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@TENGV", textBox2.Text.ToString().Trim()));

                sqlCommand.Parameters.Add(new SqlParameter("@MAKHOA", comboBox1.Text.ToString().Trim()));

                if (comboBox2.Text.ToString().Trim() == "")
                {

                    sqlCommand.Parameters.Add(new SqlParameter("@MADOIGIAMSAT", DBNull.Value));

                }
                else if (comboBox2.Text.ToString().Trim() != "")
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@MADOIGIAMSAT", comboBox2.Text.ToString().Trim()));
                }

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Thêm giảng viên thành công!");
                HienThiDuLieuGiangVien();

            }

            //Xóa
            else if (flag == "delete")
            {
                if (KiemTraDeleteKhoa(tempMaGV) == false)
                {
                    MessageBox.Show("Không thể xóa giảng viên! Giảng viên đã thuộc đội giám sát!");
                    return;
                }
                String strLenh = "sp_DeleteGiangVien";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MAGV", textBox1.Text.ToString().Trim()));
                //sqlCommand.Parameters.Add(new SqlParameter("@TENKHOA", textBox2.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Xóa giảng viên thành công!");
                HienThiDuLieuGiangVien();
            }

            //sửa
            else if (flag == "edit")
            {
                if (Check_full_DGS(tempMaDoiGiamSat)&& tempMaDoiGiamSat!="")
                {
                    MessageBox.Show("Đội giám sát đã đủ số lượng giảng viên!"); return;
                }

                String strLenh = "sp_UpdateGiangVien";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MAGV", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@TENGV", textBox2.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@MAKHOA", comboBox1.Text.ToString().Trim()));
                if (comboBox2.Text.ToString().Trim() == "")
                {

                    sqlCommand.Parameters.Add(new SqlParameter("@MADOIGIAMSAT", DBNull.Value));

                }
                else if (comboBox2.Text.ToString().Trim() != "")
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@MADOIGIAMSAT", comboBox2.Text.ToString().Trim()));
                }

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Sửa giảng viên thành công!");
                HienThiDuLieuGiangVien();
            }
        }

        //Undo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = tempMaGV;
            textBox2.Text = tempTenGV;
            comboBox1.Text = tempMaKhoa;
            comboBox2.Text = tempMaDoiGiamSat;
        }

        //Sự kiện nhấn chuột vào gridview sẽ hiện lên textbox
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
            }
        }
    }
}
