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
    public partial class Xa : Form
    {
        SqlConnection conn_publisher = new SqlConnection();
        DataTable dt = new DataTable();

        //Đánh dấu đang là lưu khóa sửa hay thêm
        string flag = "";

        //Tạo biến tạm để lưu dữ liệu quay lại;
        string tempMaXa = "", tempTenXa = "", tempMaDiaBan = "", tempMaDoiGiamSat = "";

        //HIỆN THỊ BẢNG DỮ LIỆU:
        private void HienThiDuLieu()
        {
            String strlenh = "select MaXa, TenXa, MaDiaBan, MaDoiGiamSat from XA";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Xã";
            dataGridView1.Columns[1].HeaderText = "Tên Xã";
            dataGridView1.Columns[2].HeaderText = "Mã Địa Bàn";
            dataGridView1.Columns[3].HeaderText = "Mã Đội Giám Sát";
            conn_publisher.Close();
        }

        //LẤY DANH SACH DIA BAN
        private void LayDSDIABAN()
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaDiaBan from DIABAN";
            dt = Program.ExecSqlDataTable(strlenh);

            comboBox1.DataSource = dt;
            //comboBox1.ValueMember = "MaKhoa";
            comboBox1.DisplayMember = "MaDiaBan";
            //comboBox1.SelectedIndex = 0;
            conn_publisher.Close();
        }

        //LẤY DANH SACH DOI GIAM SAT
        private void LayDSDGS()
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaDoiGiamSat from DOIGIAMSAT";
            dt = Program.ExecSqlDataTable(strlenh);

            comboBox2.DataSource = dt;
            //comboBox1.ValueMember = "MaKhoa";
            comboBox2.DisplayMember = "MaDoiGiamSat";
            //comboBox1.SelectedIndex = 0;
            conn_publisher.Close();
        }
        public Xa()
        {
            InitializeComponent();
        }

        //Thoat
        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void HienThiMenuAdmin_Xa()
        {
            if(Program.mGroup=="TRUONG") menuStrip1.Enabled = panel1.Enabled = true;

        }

        private void Xa_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            menuStrip1.Enabled = panel1.Enabled = false;
            textBox1.Enabled = textBox2.Enabled = comboBox1.Enabled = comboBox2.Enabled = false;
            // phân quyền
            HienThiMenuAdmin_Xa();

            // hiện thị dữ liệu gridview
            HienThiDuLieu();
            LayDSDIABAN();
            LayDSDGS();
        }

        //Them
        private void thêmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = textBox2.Enabled = comboBox1.Enabled = comboBox2.Enabled = true;
            textBox1.Text = textBox2.Text = comboBox1.Text = comboBox2.Text = "";
            xóaToolStripMenuItem.Enabled = sửaToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
            flag = "add";
        }
        
        //Xoa
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
            textBox1.Enabled = comboBox2.Enabled = true;
            xóaToolStripMenuItem.Enabled = thêmToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
        }

        //Undo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox2.Text = tempMaXa;
            textBox1.Text = tempTenXa;
            comboBox1.Text = tempMaDiaBan;
            comboBox2.Text = tempMaDoiGiamSat;
        }

        //Refresh
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            HienThiMenuAdmin_Xa();
            HienThiDuLieu();
            textBox1.Enabled = textBox2.Enabled = comboBox1.Enabled = comboBox2.Enabled = false;
            sửaToolStripMenuItem.Enabled = xóaToolStripMenuItem.Enabled = thêmToolStripMenuItem.Enabled = undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            textBox3.Text = "";
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
        private bool Check_Trung(String MAXA)
        {
            foreach (DataRow row in dt.Rows)
            {
                String maKhoaGridView = row["MaXa"].ToString();
                if (maKhoaGridView.Trim() == MAXA.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        //Tìm kiếm
        private void button1_Click(object sender, EventArgs e)
        {
            String strlenh = "select MaXa, TenXa, MaDiaBan, MaDoiGiamSat from XA where MaXa = '"+textBox3.Text.ToString().Trim()+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Xã";
            dataGridView1.Columns[1].HeaderText = "Tên Xã";
            dataGridView1.Columns[2].HeaderText = "Mã Địa Bàn";
            dataGridView1.Columns[3].HeaderText = "Mã Đội Giám Sát";
            conn_publisher.Close();
        }

        //Kiểm tra xã đã thuộc ấp chưa
        private bool KiemTraXaThuocAp(string MAXA)
        {
            String strlenh = "select MaAp from AP where MaXa = '" + MAXA + "'";
            dt = Program.ExecSqlDataTable(strlenh);
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }

        //Kiểm tra mã có trùng không
        private bool Check_Trung_DGS(String MADGS)
        {
            foreach (DataRow row in dt.Rows)
            {
                String maKhoaGridView = row["MaDoiGiamSat"].ToString();
                if (maKhoaGridView.Trim() == MADGS.Trim())
                {
                    return true;
                }
            }
            return false;
        }
        //Luu
        private void lưuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tempMaXa = textBox2.Text.Trim();
            tempTenXa = textBox1.Text.Trim();
            tempMaDiaBan = comboBox1.Text.Trim();
            tempMaDoiGiamSat = comboBox2.Text.Trim();

            //Kiểm tra dữ liệu nhập vào:
            if (Check_NULL(textBox2, "Mã xã không được để trống!")) return;
            if (Check_NULL(textBox1, "Tên xã không được để trống!")) return;
            if (Check_NULL_CBX(comboBox1, "Mã địa bàn không được để trống!")) return;
            //if (Check_NULL_CBX(comboBox2, "Mã đội giám sát không được để trống!")) return;
            if (textBox2.Text.Trim().Length != 5)
            {
                MessageBox.Show("Mã xã phải đúng 5 ký tự", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBox2.Text.Contains(" "))
            {
                MessageBox.Show("Mã xã không được chứa khoảng trống", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            

            //Xử lý
            //Thêm
            if (flag == "add")
            {
                if (Check_Trung(textBox2.Text.Trim()))
                {
                    MessageBox.Show("Mã xã đã tồn tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (Check_Trung_DGS(tempMaDoiGiamSat))
                {
                    MessageBox.Show("Đội giám sát đã thuộc xã khác. Chọn đội khác!");
                    return;
                }
                String strLenh = "sp_AddXa";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MAXA", textBox2.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@TENXA", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@MADIABAN", comboBox1.Text.ToString().Trim()));
                //sqlCommand.Parameters.Add(new SqlParameter("@MADGS", comboBox2.Text.ToString().Trim()));

                if (comboBox2.Text.ToString().Trim() == "")
                {

                    sqlCommand.Parameters.Add(new SqlParameter("@MADGS", DBNull.Value));

                }
                else if (comboBox2.Text.ToString().Trim() != "")
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@MADGS", comboBox2.Text.ToString().Trim()));
                }


                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Thêm xã thành công!");
                HienThiDuLieu();

            }

            //Xóa
            else if (flag == "delete")
            {
                if (KiemTraXaThuocAp(tempMaXa) == false)
                {
                    MessageBox.Show("Không thể xóa xã, xã đã có trong ấp");
                    return;
                }
                String strLenh = "sp_DeleteXa";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MAXA", textBox2.Text.ToString().Trim()));
                //sqlCommand.Parameters.Add(new SqlParameter("@TENKHOA", textBox2.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Xóa xã thành công!");
                HienThiDuLieu();
            }

            //sửa
            else if (flag == "edit")
            {
                if (Check_Trung_DGS(tempMaDoiGiamSat) && tempMaDoiGiamSat!="")
                {
                    MessageBox.Show("Đội giám sát đã thuộc xã khác. Chọn đội khác!");
                    return;
                }

                String strLenh = "sp_EditXa";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MAXA", textBox2.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@TENXA", textBox1.Text.ToString().Trim()));
                if (comboBox2.Text.ToString().Trim() == "")
                {

                    sqlCommand.Parameters.Add(new SqlParameter("@MADGS", DBNull.Value));

                }
                else if (comboBox2.Text.ToString().Trim() != "")
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@MADGS", comboBox2.Text.ToString().Trim()));
                }
                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Sửa xã thành công!");
                HienThiDuLieu();
            }
        }

        //tao su kien griview
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            DataTable dt = (DataTable)dataGridView1.DataSource;
            if (dt.Rows.Count > 0)
            {
                textBox2.Text = dataGridView1.Rows[index].Cells[0].Value.ToString();
                textBox1.Text = dataGridView1.Rows[index].Cells[1].Value.ToString();
                comboBox1.Text = dataGridView1.Rows[index].Cells[2].Value.ToString();
                comboBox2.Text = dataGridView1.Rows[index].Cells[3].Value.ToString();
            }
        }
    }
    
}
