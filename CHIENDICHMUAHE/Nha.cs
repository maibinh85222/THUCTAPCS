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
    public partial class Nha : Form
    {
        SqlConnection conn_publisher = new SqlConnection();
        DataTable dt = new DataTable();

        //Đánh dấu đang là lưu khóa sửa hay thêm
        string flag = "";

        //Tạo biến tạm để lưu dữ liệu quay lại;
        string tempMaNha = "", tempTenNha = "", tempMaAp = "", tempMaNhom = "";

        //HIỆN THỊ BẢNG DỮ LIỆU:
        private void HienThiDuLieu()
        {
            String strlenh = "select MaNha, TenNha, MaAp, MaNhom from NHA";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Nhà";
            dataGridView1.Columns[1].HeaderText = "Tên Nhà";
            dataGridView1.Columns[2].HeaderText = "Mã Âp";
            dataGridView1.Columns[3].HeaderText = "Mã nhóm";

            conn_publisher.Close();
        }

        //LẤY DANH SACH nhà
        private void LayDSNha()
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
        public Nha()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //Thoát
        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void HienThiMenuAdmin_Nha()
        {
            if(Program.mGroup=="TRUONG") menuStrip1.Enabled = panel1.Enabled = true;

        }
        private void Nha_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            // phân quyền
            menuStrip1.Enabled = panel1.Enabled = false;
            textBox1.Enabled = textBox2.Enabled = comboBox1.Enabled = comboBox2.Enabled = false;
            HienThiMenuAdmin_Nha();

            // hiện thị dữ liệu gridview
            HienThiDuLieu();
            LayDSAP();
            LayDSNha();
        }

        //Thêm
        private void thêmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = textBox2.Enabled = comboBox1.Enabled = comboBox2.Enabled = true;
            textBox1.Text = textBox2.Text = comboBox1.Text = "";
            xóaToolStripMenuItem.Enabled = sửaToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
            flag = "add";
        }

        //XÓa
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
        private bool Check_Trung(String MANHA)
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

        //Kiểm tra mã nhóm đã  tồn tại chưa
        private bool Check_Trung_Nhom(String MANHOM)
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

        //Kiểm tra nhà đã có nhóm chưa
        private bool KiemTraNhomTrongNha(string MANHA)
        {
            DataTable dt = new DataTable();
            String strlenh = "select * from NHA where MaNha = '"+MANHA+ "' and MaNhom is null";
            dt = Program.ExecSqlDataTable(strlenh);
            if(dt.Rows.Count > 0)
            {
                return false; // không có nhóm trong nhà
            }
            return true; // ngược lại
        }


        //Lưu
        private void lưuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tempMaNha = textBox1.Text.Trim();
            tempTenNha = textBox2.Text.Trim();
            tempMaAp = comboBox1.Text.Trim();
            tempMaNhom = comboBox2.Text.Trim();
            //Kiểm tra dữ liệu nhập vào:
            if (Check_NULL(textBox1, "Mã nhà không được để trống!")) return;
            if (Check_NULL(textBox2, "Tên nhà không được để trống!")) return;
            if (Check_NULL_CBX(comboBox1, "Mã ấp không được để trống!")) return;
            if (textBox1.Text.Trim().Length != 5)
            {
                MessageBox.Show("Mã nhà phải đúng 5 ký tự", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBox1.Text.Contains(" "))
            {
                MessageBox.Show("Mã nhà không được chứa khoảng trống", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            

            //Xử lý
            //Thêm
            if (flag == "add")
            {
                if (Check_Trung(textBox1.Text.Trim()))
                {
                    MessageBox.Show("Mã nhà đã tồn tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (Check_Trung_Nhom(tempMaNhom))
                {
                    MessageBox.Show("Mã nhóm đã thuộc nhà vui lòng chọn nhóm khác!");
                    return;
                }

                String strLenh = "sp_AddNha";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MANHA", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@TENNHA", textBox2.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@MAAP", comboBox1.Text.ToString().Trim()));

                if (comboBox2.Text.ToString().Trim() == "")
                {

                    sqlCommand.Parameters.Add(new SqlParameter("@MANHOM", DBNull.Value));

                }
                else if (comboBox2.Text.ToString().Trim() != "")
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@MANHOM", comboBox2.Text.ToString().Trim()));
                }

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Thêm nhà thành công!");
                HienThiDuLieu();

            }

            //Xóa
            else if (flag == "delete")
            {
                if (KiemTraNhomTrongNha(tempMaNha))
                {
                    MessageBox.Show("Nhà đã có nhóm không thể xóa!");
                    return;
                }
                String strLenh = "sp_DeleteNha";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MANHA", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@MAAP", comboBox1.Text.ToString().Trim()));
                //sqlCommand.Parameters.Add(new SqlParameter("@TENKHOA", textBox2.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Xóa nhà thành công!");
                HienThiDuLieu();
            }

            //sửa
            else if (flag == "edit")
            {
                if (Check_Trung_Nhom(tempMaNhom) && comboBox2.Text.Trim()!="")
                {
                    MessageBox.Show("Mã nhóm đã thuộc nhà vui lòng chọn nhóm khác!");
                    return;
                }

                String strLenh = "sp_UpdateNha";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MANHA", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@TENNHA", textBox2.Text.ToString().Trim()));
                if (comboBox2.Text.ToString().Trim() == "")
                {

                    sqlCommand.Parameters.Add(new SqlParameter("@MANHOM", DBNull.Value));

                }
                else if (comboBox2.Text.ToString().Trim() != "")
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@MANHOM", comboBox2.Text.ToString().Trim()));
                }



                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Sửa nhà thành công!");
                HienThiDuLieu();
            }
        }

        //Tạo sự kiện gridview
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

        //tìm kiếm nha
        private void button1_Click(object sender, EventArgs e)
        {
            String strlenh = "select MaNha, TenNha, MaAp, MaNhom from NHA where MaNha = '"+textBox3.Text.ToString().Trim()+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Nhà";
            dataGridView1.Columns[1].HeaderText = "Tên Nhà";
            dataGridView1.Columns[2].HeaderText = "Mã Âp";
            dataGridView1.Columns[3].HeaderText = "Mã Nhóm";


            conn_publisher.Close();
        }

        //Tìm kiếm âp
        private void button2_Click(object sender, EventArgs e)
        {
            String strlenh = "select MaNha, TenNha, MaAp, MaNhom from NHA where MaAp = '" + textBox4.Text.ToString().Trim() + "'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Nhà";
            dataGridView1.Columns[1].HeaderText = "Tên Nhà";
            dataGridView1.Columns[2].HeaderText = "Mã Âp";
            dataGridView1.Columns[3].HeaderText = "Mã nhóm";


            conn_publisher.Close();
        }

        //Undo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = tempMaNha;
            textBox2.Text = tempTenNha;
            comboBox1.Text = tempMaAp;
            comboBox2.Text = tempMaNhom;
        }

        //Refresh
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            HienThiMenuAdmin_Nha();
            HienThiDuLieu();
            textBox1.Enabled = textBox2.Enabled = comboBox1.Enabled = false;
            sửaToolStripMenuItem.Enabled = xóaToolStripMenuItem.Enabled = thêmToolStripMenuItem.Enabled = undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            textBox3.Text = textBox4.Text = "";
        }
    
    }
}
