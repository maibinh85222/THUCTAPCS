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
    public partial class KhenThuong : Form
    {
        SqlConnection conn_publisher = new SqlConnection();
        DataTable dt = new DataTable();
        string flag = "";
        string tempMaDiaBan = "";
        string tempTenDiaBan = "";
        //HIỆN THỊ BẢNG DỮ LIỆU:
        private void HienThiDuLieu()
        {
            String strlenh = "select MaKT, NoiDungKT from KHENTHUONG";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Khen Thưởng";
            dataGridView1.Columns[1].HeaderText = "Nội Dung Khen Thưởng";

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
        private bool Check_Trung(String MAKT)
        {
            foreach (DataRow row in dt.Rows)
            {
                String maDBGridView = row["MaKT"].ToString();
                if (maDBGridView.Trim() == MAKT.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        public KhenThuong()
        {
            InitializeComponent();
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void HienThiMenuAdmin_KhenThuong()
        {
            if (Program.mGroup == "TRUONG") 
            {
                menuStrip1.Enabled = panel1.Enabled = true;
                textBox1.Enabled = textBox2.Enabled = false;
                lưuToolStripMenuItem.Enabled = false;
            }      

        }

        //-----------------load dữ liệu khi vào from
        private void KhenThuong_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            menuStrip1.Enabled = panel1.Enabled = false;
            // phân quyền
            HienThiMenuAdmin_KhenThuong();
            //hiện thị dữ liệu:
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

        //Sửa
        private void sửaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = "edit";
            textBox2.Enabled = true;
            xóaToolStripMenuItem.Enabled = thêmToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
        }

        //Xóa
        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flag = "delete";

            thêmToolStripMenuItem.Enabled = sửaToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
        }

        //Kiểm tra mã khen thưởng đã thuộc sinh viên nào chưa
        private bool KiemTraKhenThuongTrongSv(string MAKHENTHUONG)
        {
            String strlenh = "select *from SV_KT where MaKT = '"+MAKHENTHUONG+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            if(dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        //Lưu
        private void lưuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tempMaDiaBan = textBox1.Text.Trim();
            tempTenDiaBan = textBox2.Text.Trim();
            //Kiểm tra dữ liệu nhập vào:
            if (Check_NULL(textBox1, "Mã khen thưởng bàn không được để trống!")) return;
            if (Check_NULL(textBox2, "Tên khen thưởng không được để trống!")) return;
            if (textBox1.Text.Trim().Length != 5)
            {
                MessageBox.Show("Mã khen thưởng phải đúng 5 ký tự", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBox1.Text.Contains(" "))
            {
                MessageBox.Show("Mã khen thưởng không được chứa khoảng trống", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } 

            //Xử lý
            //Thêm
            if (flag == "add")
            {
                if (Check_Trung(textBox1.Text.Trim()) && flag == "add")
                {
                    MessageBox.Show("Mã khen thưởng đã tồn tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                String strLenh = "sp_InsertKT";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MAKT", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@NOIDUNG", textBox2.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Thêm khen thưởng thành công!");
                HienThiDuLieu();
            }

            //Xóa
            else if (flag == "delete")
            {
                if (KiemTraKhenThuongTrongSv(tempMaDiaBan))
                {
                    MessageBox.Show("Mã khen thưởng đã có trong sinh viên khen thưởng! Not dalete");
                    return;
                }
                String strLenh = "sp_DeleteKT";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MAKT", textBox1.Text.ToString().Trim()));
                //sqlCommand.Parameters.Add(new SqlParameter("@TENKHOA", textBox2.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Xóa khen thưởng thành công!");
                HienThiDuLieu();
            }

            //sửa
            else if (flag == "edit")
            {
                String strLenh = "sp_UpdateKT";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MAKT", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@NOIDUNG", textBox2.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Sửa khen thưởng thành công!");
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
            HienThiMenuAdmin_KhenThuong();
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

        //tìm
        private void button1_Click(object sender, EventArgs e)
        {
            String strlenh = "select MaKT, NoiDungKT from KHENTHUONG where MaKT = '"+textBox3.Text.ToString().Trim()+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Khen Thưởng";
            dataGridView1.Columns[1].HeaderText = "Nội Dung Khen Thưởng";

            conn_publisher.Close();
        }
    }
}
