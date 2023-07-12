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
    public partial class Ap : Form
    {
        SqlConnection conn_publisher = new SqlConnection();
        DataTable dt = new DataTable();

        //Đánh dấu đang là lưu khóa sửa hay thêm
        string flag = "";

        //Tạo biến tạm để lưu dữ liệu quay lại;
        string tempMaAp = "", tempTenAp = "", tempMaXa = "";
        //HIỆN THỊ BẢNG DỮ LIỆU:
        private void HienThiDuLieu()
        {
            String strlenh = "select MaAp, TenAp, MaXa from AP";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Ấp";
            dataGridView1.Columns[1].HeaderText = "Tên Ấp";
            dataGridView1.Columns[2].HeaderText = "Mã Xã";
            conn_publisher.Close();
        }

        //LẤY DANH SACH XA
        private void LayDSXA()
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaXa from XA";
            dt = Program.ExecSqlDataTable(strlenh);

            comboBox1.DataSource = dt;
            //comboBox1.ValueMember = "MaKhoa";
            comboBox1.DisplayMember = "MaXa";
            //comboBox1.SelectedIndex = 0;
            conn_publisher.Close();
        }

        //Kiểm tra ấp có thuộc nhà không
        private bool KiemTraApTrongNha(string MAAP)
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaNha from NHA where MaAp = '"+MAAP+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;

        }
        public Ap()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        //Thoat
        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void HienThiMenuAdmin_Ap()
        {
            if(Program.mGroup=="TRUONG")    menuStrip1.Enabled = panel1.Enabled = true;

        }
        private void Ap_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            // phân quyền
            menuStrip1.Enabled = panel1.Enabled = false;
            textBox1.Enabled = textBox2.Enabled = comboBox1.Enabled = false;
            HienThiMenuAdmin_Ap();

            // hiện thị dữ liệu gridview
            HienThiDuLieu();
            LayDSXA();
        }

        //Them
        private void thêmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = textBox2.Enabled = comboBox1.Enabled = true;
            textBox1.Text = textBox2.Text = comboBox1.Text = "";
            xóaToolStripMenuItem.Enabled = sửaToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
            flag = "add";
        }

        //XOa
        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = "delete";

            thêmToolStripMenuItem.Enabled = sửaToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
        }

        //Sua
        private void sửaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = "edit";
            textBox2.Enabled = true;
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
        //Luu
        private void lưuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tempMaAp = textBox1.Text.Trim();
            tempTenAp = textBox2.Text.Trim();
            tempMaXa = comboBox1.Text.Trim();
            //Kiểm tra dữ liệu nhập vào:
            if (Check_NULL(textBox1, "Mã ấp không được để trống!")) return;
            if (Check_NULL(textBox2, "Tên ấp không được để trống!")) return;
            if (Check_NULL_CBX(comboBox1, "Mã xã không được để trống!")) return;
            if (textBox1.Text.Trim().Length != 5)
            {
                MessageBox.Show("Mã ấp phải đúng 5 ký tự", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBox1.Text.Contains(" "))
            {
                MessageBox.Show("Mã ấp không được chứa khoảng trống", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            

            //Xử lý
            //Thêm
            if (flag == "add")
            {
                if (Check_Trung(textBox1.Text.Trim()))
                {
                    MessageBox.Show("Mã ấp đã tồn tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                String strLenh = "sp_AddAp";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MAAP", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@TENAP", textBox2.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@MAXA", comboBox1.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Thêm ấp thành công!");
                HienThiDuLieu();

            }

            //Xóa
            else if (flag == "delete")
            {
                if (KiemTraApTrongNha(tempMaAp) == true)
                {
                    MessageBox.Show("Mã ấp đã có trong nhà. Không thể xóa!");
                    return;
                }
                String strLenh = "sp_DeleteAp";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MAAP", textBox1.Text.ToString().Trim()));
                //sqlCommand.Parameters.Add(new SqlParameter("@TENKHOA", textBox2.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Xóa ấp thành công!");
                HienThiDuLieu();
            }

            //sửa
            else if (flag == "edit")
            {
                String strLenh = "sp_UpdateAp";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MAAP", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@TENAP", textBox2.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@MAXA", comboBox1.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Sửa ấp thành công!");
                HienThiDuLieu();
            }

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
            }
        }

        //Tìm kiếm ấp
        private void button1_Click(object sender, EventArgs e)
        {
            String strlenh = "select MaAp, TenAp, MaXa from AP where MaAp = '"+textBox3.Text.ToString().Trim()+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Ấp";
            dataGridView1.Columns[1].HeaderText = "Tên Ấp";
            dataGridView1.Columns[2].HeaderText = "Mã Xã";
            conn_publisher.Close();
        }

        //Undo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = tempMaAp;
            textBox2.Text = tempTenAp;
            comboBox1.Text = tempMaXa;
        }

        //Refresh
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            HienThiMenuAdmin_Ap();
            HienThiDuLieu();
            textBox1.Enabled = textBox2.Enabled = comboBox1.Enabled = false;
            sửaToolStripMenuItem.Enabled = xóaToolStripMenuItem.Enabled = thêmToolStripMenuItem.Enabled = undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            textBox3.Text = "";
        }
    }
}
