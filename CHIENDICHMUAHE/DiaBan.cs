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
    public partial class DiaBan : Form
    {
        SqlConnection conn_publisher = new SqlConnection();
        DataTable dt = new DataTable();
        string flag = "";
        string tempMaDiaBan = "";
        string tempTenDiaBan = "";
        //HIỆN THỊ BẢNG DỮ LIỆU:
        private void HienThiDuLieu()
        {
            String strlenh = "select MaDiaBan, TenDiaBan from DIABAN";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Địa Bàn";
            dataGridView1.Columns[1].HeaderText = "Tên Địa Bàn";
            conn_publisher.Close();
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

        //Kiểm tra mã có trùng không
        private bool Check_Trung(String MADIABAN)
        {
            foreach (DataRow row in dt.Rows)
            {
                String maDBGridView = row["MaDiaBan"].ToString();
                if (maDBGridView.Trim() == MADIABAN.Trim())
                {
                    return true;
                }
            }
            return false;
        }
        
        public DiaBan()
        {
            InitializeComponent();
        }

        //Thoát
        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void HienThiMenuAdmin_DiaBan()
        {
            if (Program.mGroup == "TRUONG")
            {
                menuStrip1.Enabled = panel1.Enabled = true;
                textBox1.Enabled = textBox2.Enabled = false;
                lưuToolStripMenuItem.Enabled = false;
            }

        }
        private void DiaBan_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            menuStrip1.Enabled = panel1.Enabled = false;
            // phân quyền
            HienThiMenuAdmin_DiaBan();
           
            // xuất dữ liệu:
            HienThiDuLieu();
        }

        //Thêm
        private void thêmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = textBox2.Enabled = true;
            textBox1.Text = textBox2.Text = "";
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

        }

        //Kiểm tra địa bàn đã tồn tại trong xã chưa
        private bool KiemTraDiaBan(string MADIABAN)
        {
            String strlenh = "select MaXa from XA where MaDiaBan = '"+MADIABAN+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            if (dt.Rows.Count > 1)
            {
                return false;
            }
            return true;
        }

        //Lưu
        private void lưuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tempMaDiaBan = textBox1.Text.Trim();
            tempTenDiaBan = textBox2.Text.Trim();
            //Kiểm tra dữ liệu nhập vào:
            if (Check_NULL(textBox1, "Mã địa bàn không được để trống!")) return;
            if (Check_NULL(textBox2, "Tên địa bàn không được để trống!")) return;
            if (textBox1.Text.Trim().Length != 5)
            {
                MessageBox.Show("Mã địa bàn phải đúng 5 ký tự", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBox1.Text.Contains(" "))
            {
                MessageBox.Show("Mã địa bàn không được chứa khoảng trống", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            

            //Xử lý
            //Thêm
            if (flag == "add")
            {
                if (Check_Trung(textBox1.Text.Trim()))
                {
                    MessageBox.Show("Mã địa bàn đã tồn tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                String strLenh = "sp_InsertDiaBan";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MADIABAN", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@TENDIABAN", textBox2.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Thêm khoa thành công!");
                HienThiDuLieu();

            }

            //Xóa
            else if (flag == "delete")
            {
                if (KiemTraDiaBan(tempMaDiaBan) == false)
                {
                    MessageBox.Show("Địa bàn đã có trong xã! Không thể xóa!");
                    return;
                }
                String strLenh = "sp_DeleteDiaBan";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MADIABAN", textBox1.Text.ToString().Trim()));
                //sqlCommand.Parameters.Add(new SqlParameter("@TENKHOA", textBox2.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Xóa địa bàn thành công!");
                HienThiDuLieu();
            }

            //sửa
            else if (flag == "edit")
            {
                String strLenh = "sp_UpdateDiaBan";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MADIABAN", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@TENDIABAN", textBox2.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Sửa địa bàn thành công!");
                HienThiDuLieu();
            }

        }

        //Undo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = tempMaDiaBan;
            textBox2.Text = tempTenDiaBan;
        }

        //Refresh
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            HienThiMenuAdmin_DiaBan();
            HienThiDuLieu();
            textBox1.Enabled = textBox2.Enabled = false;
            sửaToolStripMenuItem.Enabled = xóaToolStripMenuItem.Enabled = thêmToolStripMenuItem.Enabled = undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            textBox3.Text = "";
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
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String strlenh = "select MaDiaBan, TenDiaBan from DIABAN where MaDiaBan = '"+textBox3.Text.ToString().Trim()+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Địa Bàn";
            dataGridView1.Columns[1].HeaderText = "Tên Địa Bàn";
            conn_publisher.Close();
        }
    }
}
