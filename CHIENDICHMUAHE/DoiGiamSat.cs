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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CHIENDICHMUAHE
{
    public partial class DoiGiamSat : Form
    {
        SqlConnection conn_publisher = new SqlConnection();
        DataTable dt = new DataTable();

        //Đánh dấu đang là lưu khóa sửa hay thêm
        string flag = "";

        //Tạo biến tạm để lưu dữ liệu quay lại;
        string tempMaDGS = "", tempGV1 = "", tempGV2 = "", tempDoiTruong = "", tempDoiPho = "", tempTenDGS = "";

        //Lấy giá trị để sửa dữ liệu
        string tempMaDGS_S = "", tempGV1_S = "", tempGV2_S = "", tempDoiTruong_S = "", tempDoiPho_S = "", tempTenDGS_S = "";


        //HIỆN THỊ BẢNG DỮ LIỆU:
        private void HienThiDuLieu()
        {
            String strlenh = "select * from DOIGIAMSAT";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Đội Giám Sát";
            //dataGridView1.Columns[1].HeaderText = "Mã Giảng Viên 1";
            //dataGridView1.Columns[2].HeaderText = "Mã Giảng Viên 2";
            dataGridView1.Columns[1].HeaderText = "Mã Đội Trưởng";
            dataGridView1.Columns[2].HeaderText = "Mã Đội Phó";
            dataGridView1.Columns[3].HeaderText = "Tên đội giám sát";
            conn_publisher.Close();
        }

        //LẤY DANH SACH GIANG VIEN
       /* private void LayDSGV1()
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaGV from GIANGVIEN";
            dt = Program.ExecSqlDataTable(strlenh);

            comboBox1.DataSource = dt;
            //comboBox3.DataSource = dt;
            //comboBox1.ValueMember = "MaKhoa";
            comboBox1.DisplayMember = "MaGV";
            //comboBox3.DisplayMember = "MaGV";
            //comboBox1.SelectedIndex = 0;
            conn_publisher.Close();
        }*/
        /*private void LayDSGV2()
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaGV from GIANGVIEN";
            dt = Program.ExecSqlDataTable(strlenh);

            //comboBox1.DataSource = dt;
            comboBox3.DataSource = dt;
            //comboBox1.ValueMember = "MaKhoa";
            //comboBox1.DisplayMember = "MaGV";
            comboBox3.DisplayMember = "MaGV";
            //comboBox1.SelectedIndex = 0;
            conn_publisher.Close();
        }*/

        //Kiểm tra sv trong đội giám sát có là đội trưởng không.
        private bool KIEMTRANHOMTRUONG(String MASV)
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaTruongNhom from NHOM where MaTruongNhom = '"+MASV+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        //LẤY DANH SACH SINH VIEN
        private void LayDSSV1()
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaSV from SINHVIEN";
            dt = Program.ExecSqlDataTable(strlenh);

            comboBox2.DataSource = dt;
            //comboBox4.DataSource = dt;
            //comboBox1.ValueMember = "MaKhoa";
            comboBox2.DisplayMember = "MaSV";
            //comboBox4.DisplayMember = "MaSV";
            //comboBox1.SelectedIndex = 0;
            conn_publisher.Close();
        }
        private void LayDSSV2()
        {
            DataTable dt = new DataTable();
            String strlenh = "select MaSV from SINHVIEN";
            dt = Program.ExecSqlDataTable(strlenh);

            //comboBox2.DataSource = dt;
            comboBox4.DataSource = dt;
            //comboBox1.ValueMember = "MaKhoa";
            //comboBox2.DisplayMember = "MaSV";
            comboBox4.DisplayMember = "MaSV";
            //comboBox1.SelectedIndex = 0;
            conn_publisher.Close();
        }
        public DoiGiamSat()
        {
            InitializeComponent();
        }

        //Thoát
        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void HienThiMenuAdmin_DoiNguGiamSat()
        {
            if(Program.mGroup=="TRUONG") menuStrip1.Enabled = panel1.Enabled = true;
            if(Program.mGroup=="SINHVIEN") button1.Visible = true;

        }
        private void DoiGiamSat_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            menuStrip1.Enabled = panel1.Enabled = false;
            textBox1.Enabled = textBox5.Enabled = comboBox2.Enabled = comboBox4.Enabled = false;
            // phân quyền::
            HienThiMenuAdmin_DoiNguGiamSat();

            //hiện thị dữ liệu 
            HienThiDuLieu();
            //LayDSGV1();
            //LayDSGV2();
            LayDSSV1();
            LayDSSV2();
        }

        //Thêm
        private void thêmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Enabled =  comboBox2.Enabled = textBox5.Enabled = comboBox4.Enabled = true;
            textBox1.Text = textBox5.Text = comboBox2.Text = comboBox4.Text = "";
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
            comboBox2.Enabled = textBox5.Enabled = comboBox4.Enabled = true;
            xóaToolStripMenuItem.Enabled = thêmToolStripMenuItem.Enabled = false; undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            lưuToolStripMenuItem.Enabled = true;
            dataGridView1.Enabled = false;

            tempMaDGS_S = textBox1.Text.Trim();
            //tempGV1_S = comboBox1.Text.Trim();
            //tempGV2_S = comboBox3.Text.Trim();
            tempTenDGS_S = textBox5.Text.Trim();
            tempDoiTruong_S = comboBox2.Text.Trim();
            tempDoiPho_S = comboBox4.Text.Trim();
        }

        //Undo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = tempMaDGS;
            //comboBox1.Text = tempGV1;
            //comboBox3.Text = tempGV2;
            textBox5.Text = tempTenDGS;
            comboBox2 .Text = tempDoiTruong;
            comboBox4 .Text = tempDoiPho;
        }

        //Cá nhân sinh viên
        private void button1_Click(object sender, EventArgs e)
        {
            String strlenh = "select *from LayDS_DGS_NHOM('"+Program.username+"')";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Đội Giám Sát";
            //dataGridView1.Columns[1].HeaderText = "Mã Giảng Viên 1";
            //dataGridView1.Columns[2].HeaderText = "Mã Giảng Viên 2";
            dataGridView1.Columns[1].HeaderText = "Mã Đội Trưởng";
            dataGridView1.Columns[2].HeaderText = "Mã Đội Phó";
            dataGridView1.Columns[3].HeaderText = "Tên đội giám sát";
            conn_publisher.Close();
        }

        //Tìm theo mã gs
        private void button3_Click(object sender, EventArgs e)
        {
            String strlenh = "select * from DOIGIAMSAT where MaDoiGiamSat = '"+textBox3.Text.ToString().Trim()+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Đội Giám Sát";
            //dataGridView1.Columns[1].HeaderText = "Mã Giảng Viên 1";
            //dataGridView1.Columns[2].HeaderText = "Mã Giảng Viên 2";
            dataGridView1.Columns[1].HeaderText = "Mã Đội Trưởng";
            dataGridView1.Columns[2].HeaderText = "Mã Đội Phó";
            dataGridView1.Columns[3].HeaderText = "Tên đội giám sát";
            conn_publisher.Close();
        }

        //Tìm theo mã GV
        private void button4_Click(object sender, EventArgs e)
        {
            /*String strlenh = "select * from DOIGIAMSAT where MaGV1 = '"+textBox2.Text.ToString().Trim()+"' or MaGV2 = '"+textBox2.Text.ToString().Trim()+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Đội Giám Sát";
            dataGridView1.Columns[1].HeaderText = "Mã Giảng Viên 1";
            dataGridView1.Columns[2].HeaderText = "Mã Giảng Viên 2";
            dataGridView1.Columns[3].HeaderText = "Mã Đội Trưởng";
            dataGridView1.Columns[4].HeaderText = "Mã Đội Phó";
            conn_publisher.Close();*/
            MessageBox.Show("Đã loại bỏ GV nên tạm dùng nút này");
        }

        //Tìm theo mã sinh viên
        private void button2_Click(object sender, EventArgs e)
        {
            String strlenh = "select * from DOIGIAMSAT where MaDoiTruong = '" + textBox4.Text.ToString().Trim() + "' or MaDoiPho = '" + textBox4.Text.ToString().Trim() + "'";
            dt = Program.ExecSqlDataTable(strlenh);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;// phu het bang
            dataGridView1.Columns[0].HeaderText = "Mã Đội Giám Sát";
            //dataGridView1.Columns[1].HeaderText = "Mã Giảng Viên 1";
            //dataGridView1.Columns[2].HeaderText = "Mã Giảng Viên 2";
            dataGridView1.Columns[1].HeaderText = "Mã Đội Trưởng";
            dataGridView1.Columns[2].HeaderText = "Mã Đội Phó";
            dataGridView1.Columns[3].HeaderText = "Tên đội giám sát";
            conn_publisher.Close();
        }

        //refresh
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            HienThiMenuAdmin_DoiNguGiamSat();
            HienThiDuLieu();
            dataGridView1.Enabled = true;
            textBox1.Enabled = textBox5.Enabled = comboBox2.Enabled =  comboBox4.Enabled = false;
            sửaToolStripMenuItem.Enabled = xóaToolStripMenuItem.Enabled = thêmToolStripMenuItem.Enabled = undoToolStripMenuItem.Enabled = refreshToolStripMenuItem.Enabled = true;
            //textBox2.Text = textBox3.Text = textBox4.Text = "";
        }

        //Tạo sự kiện gridview
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            DataTable dt = (DataTable)dataGridView1.DataSource;
            if (dt.Rows.Count > 0)
            {
                textBox1.Text = dataGridView1.Rows[index].Cells[0].Value.ToString();
                //comboBox1.Text = dataGridView1.Rows[index].Cells[1].Value.ToString();
                //comboBox3.Text = dataGridView1.Rows[index].Cells[2].Value.ToString();
                comboBox2.Text = dataGridView1.Rows[index].Cells[1].Value.ToString();
                comboBox4.Text = dataGridView1.Rows[index].Cells[2].Value.ToString();
                textBox5.Text = dataGridView1.Rows[index].Cells[3].Value.ToString();
            }
        }


        //Kiểm tra text có rỗng không
        private bool Check_NULL(System.Windows.Forms.TextBox tb, string str)
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
        private bool Check_Trung(String MADGS)
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

        //Kiểm tra mã có trùng mã giảng viên
        /*private bool Check_Trung_GV(String MAGV)
        {
            foreach (DataRow row in dt.Rows)
            {
                String maKhoaGridView = row["MaGV1"].ToString();
                if (maKhoaGridView.Trim() == MAGV.Trim())
                {
                    if (maKhoaGridView == "")
                        break;
                    return true;
                }
            }

            foreach (DataRow row in dt.Rows)
            {
                String maKhoaGridView = row["MaGV2"].ToString();
                if (maKhoaGridView.Trim() == MAGV.Trim())
                {
                    if (maKhoaGridView == "")
                        break;
                    return true;
                }
            }
            return false;
        }*/

        private bool Check_Trung_SV(String MASV)
        {
            String strlenh = "select * from SINHVIEN where MaSV = '"+MASV+"' and MaDoiGiamSat is null";
            dt = Program.ExecSqlDataTable(strlenh);
            if(dt.Rows.Count > 0)
                return false;
            return true;
        }

        //Kiểm tra sinh viên đã thuộc nhóm chưa
        private bool KiemtraSinhVienDaThuocNhom(String MASV)
        {
            String strlenh = "select MaSV from SINHVIEN where MaSV = '"+MASV+"' and MaNhom is null";
            dt = Program.ExecSqlDataTable(strlenh);
            if(dt.Rows.Count > 0)
            {
                return false;// được thêm
            }
            return true; // không được
        }

        //Kiểm tra đội giám sát đã thuộc giảng viên và xã chưa nếu thuộc không cho xóa
        private bool KiemTraGVXa(string MADGS)
        {
            String strlenh = "select * from GIANGVIEN where MaDoiGiamSat = '"+MADGS+"'";
            dt = Program.ExecSqlDataTable(strlenh);
            if(dt.Rows.Count > 0)
            {
                //không được xóa
                return true;
            }

            String strlenh1 = "select * from XA where MaDoiGiamSat = '" + MADGS + "'";
            dt = Program.ExecSqlDataTable(strlenh1);
            if (dt.Rows.Count > 0)
            {
                //không được xóa
                return true;
            }
            return false;
        }
        //Lưu
        private void lưuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tempMaDGS = textBox1.Text.Trim();
            //tempGV1 = comboBox1.Text.Trim();
            //tempGV2 = comboBox3.Text.Trim();
            tempDoiTruong = comboBox2.Text.Trim();
            tempDoiPho = comboBox4.Text.Trim();
            tempTenDGS = textBox5.Text.Trim();

            //Kiểm tra dữ liệu nhập vào:
            if (Check_NULL(textBox1, "Mã đội giám sát không được để trống!")) return;
            /*if (Check_NULL_CBX(comboBox1, "Mã GV1 không được để trống!")) return;
            if (Check_NULL_CBX(comboBox3, "Mã GV2 không được để trống!")) return;*/
            if (Check_NULL_CBX(comboBox2, "Mã đội trưởng không được để trống!")) return;
            if (Check_NULL_CBX(comboBox4, "Mã đội phó không được để trống!")) return;
            if (Check_NULL(textBox5, "Tên đội giám sát không được để trống!")) return;

            if (textBox1.Text.Trim().Length != 5)
            {
                MessageBox.Show("Mã đội giám sát phải đúng 5 ký tự", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBox1.Text.Contains(" "))
            {
                MessageBox.Show("Mã đội giám sát không được chứa khoảng trống", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Xử lý
            //Thêm
            if (flag == "add")
            {
                if (Check_Trung(textBox1.Text.Trim()))
                {
                    MessageBox.Show("Mã đội giám sát đã tồn tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                /*if (Check_Trung_GV(tempGV1))
                {
                    MessageBox.Show("Mã giảng viên 1 đã thuộc đội giám sát. Chọn Giảng viên mới!"); return;
                }*/

                /*if (Check_Trung_GV(tempGV2))
                {
                    MessageBox.Show("Mã giảng viên 2 đã thuộc đội giám sát. Chọn Giảng viên mới!"); return;
                }*/

                if (Check_Trung_SV(tempDoiTruong))
                {
                    MessageBox.Show("Mã đội trưởng đã thuộc đội giám sát. Chọn sinh viên mới!"); return;
                }

                if (Check_Trung_SV(tempDoiPho))
                {
                    MessageBox.Show("Mã đội phó đã thuộc đội giám sát. Chọn sinh viên mới!"); return;
                }

                if (KIEMTRANHOMTRUONG(tempDoiTruong))
                {
                    MessageBox.Show("Mã đội trưởng đã thuộc nhóm trưởng vui lòng chọn sinh viên khác!"); return;
                }

                if (KIEMTRANHOMTRUONG(tempDoiPho))
                {
                    MessageBox.Show("Mã đội phó đã thuộc nhóm trưởng vui lòng chọn sinh viên khác!"); return;
                }

                if (KiemtraSinhVienDaThuocNhom(tempDoiTruong))
                {
                    MessageBox.Show("Sinh viên đã thuộc nhóm. Không thể thêm!");
                    return;
                }

                if (KiemtraSinhVienDaThuocNhom(tempDoiPho))
                {
                    MessageBox.Show("Sinh viên đã thuộc nhóm. Không thể thêm!");
                    return;
                }


                /*if ((tempGV1 == tempGV2) && tempGV1 != "" && tempGV2 != "")
                {
                    MessageBox.Show("Mã giảng viên 1 và 2 không được trùng nhau!"); return;
                }*/

                if ((tempDoiTruong == tempDoiPho) && tempDoiTruong != "" && tempDoiPho != "")
                {
                    MessageBox.Show("Mã sinh viên 1 và 2 không được trùng nhau!"); return;
                }

                String strLenh = "sp_AddDoiGiamSat";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MADGS", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@MADOITRUONG", comboBox2.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@MADOIPHO", comboBox4.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@TENDOIGIAMSAT", textBox5.Text.ToString().Trim()));


                /*if (comboBox1.Text.ToString().Trim() == "")
                {

                    sqlCommand.Parameters.Add(new SqlParameter("@MAGV1", DBNull.Value));

                }
                else if (comboBox1.Text.ToString().Trim() != "")
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@MAGV1", comboBox1.Text.ToString().Trim()));
                }
*/
                /*if (comboBox3.Text.ToString().Trim() == "")
                {

                    sqlCommand.Parameters.Add(new SqlParameter("@MAGV2", DBNull.Value));

                }
                else if (comboBox3.Text.ToString().Trim() != "")
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@MAGV2", comboBox3.Text.ToString().Trim()));
                }*/

                /*//-------
                if (comboBox2.Text.ToString().Trim() == "")
                {

                    sqlCommand.Parameters.Add(new SqlParameter("@MADOITRUONG", DBNull.Value));

                }
                else if (comboBox2.Text.ToString().Trim() != "")
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@MADOITRUONG", comboBox2.Text.ToString().Trim()));
                }

                if (comboBox4.Text.ToString().Trim() == "")
                {

                    sqlCommand.Parameters.Add(new SqlParameter("@MADOIPHO", DBNull.Value));

                }
                else if (comboBox4.Text.ToString().Trim() != "")
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@MADOIPHO", comboBox4.Text.ToString().Trim()));
                }

                sqlCommand.Parameters.Add(new SqlParameter("@TENDOIGIAMSAT", textBox5.Text.ToString().Trim()));
                //-------*/

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Thêm đội giám sát thành công!");
                HienThiDuLieu();

            }

            //Xóa
            else if (flag == "delete")
            {
                if (KiemTraGVXa(tempMaDGS))
                {
                    MessageBox.Show("Đội giám sát đã thuộc xã hoặc giảng viên! Not delete");
                    return;
                }
                String strLenh = "sp_DeleteDoiGiamSat";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MADGS", textBox1.Text.ToString().Trim()));
                //sqlCommand.Parameters.Add(new SqlParameter("@TENKHOA", textBox2.Text.ToString().Trim()));

                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Xóa đội giám sát thành công!");
                HienThiDuLieu();
            }

            //sửa
            else if (flag == "edit")
            {
                /*if (tempGV1_S != "" && tempGV1_S == tempGV1)
                {

                }
                else 
                {
                    if (Check_Trung_GV(tempGV1))
                    {
                        MessageBox.Show("Mã giảng viên 1 đã thuộc đội giám sát. Chọn Giảng viên mới!"); return;
                    }
                }*/

                /* if (tempGV2_S != "" && tempGV2_S == tempGV2)
                 {

                 }
                 else
                 {
                     if (Check_Trung_GV(tempGV2))
                     {
                         MessageBox.Show("Mã giảng viên 2 đã thuộc đội giám sát. Chọn Giảng viên mới!"); return;
                     }
                 }*/


                if (tempDoiTruong_S != "" && tempDoiTruong_S == tempDoiTruong)
                {

                }
                else
                {
                    if (Check_Trung_SV(tempDoiTruong))
                    {
                        MessageBox.Show("Mã đội trưởng đã thuộc đội giám sát. Chọn sinh viên mới!"); return;
                    }

                    if (KIEMTRANHOMTRUONG(tempDoiTruong))
                    {
                        MessageBox.Show("Mã đội trưởng đã thuộc nhóm trưởng vui lòng chọn sinh viên khác!"); return;
                    }

                    
                }

                if (KiemtraSinhVienDaThuocNhom(tempDoiTruong))
                {
                    MessageBox.Show("Sinh viên đã thuộc nhóm. Không thể thêm!");
                    return;
                }

                if (tempDoiPho_S != "" && tempDoiPho_S == tempDoiPho)
                {

                }
                else
                {
                    if (Check_Trung_SV(tempDoiPho))
                    {
                        MessageBox.Show("Mã đội phó đã thuộc đội giám sát. Chọn sinh viên mới!"); return;
                    }

                    if (KIEMTRANHOMTRUONG(tempDoiPho))
                    {
                        MessageBox.Show("Mã đội phó đã thuộc nhóm trưởng vui lòng chọn sinh viên khác!"); return;
                    }
                   
                }

                if (KiemtraSinhVienDaThuocNhom(tempDoiPho))
                {
                    MessageBox.Show("Sinh viên đã thuộc nhóm. Không thể thêm!");
                    return;
                }

                /*if ((tempGV1 == tempGV2) && tempGV1 != "" && tempGV2 != "")
                {
                    MessageBox.Show("Mã giảng viên 1 và 2 không được trùng nhau!"); return;
                }*/

                if ((tempDoiTruong == tempDoiPho) && tempDoiTruong != "" && tempDoiPho != "")
                {
                    MessageBox.Show("Mã sinh viên 1 và 2 không được trùng nhau!"); return;
                }

                String strLenh = "sp_EditDoiGiamSat";
                SqlCommand sqlCommand = new SqlCommand(strLenh, Program.conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 600;

                sqlCommand.Parameters.Add(new SqlParameter("@MADGS", textBox1.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@MADOITRUONG", comboBox2.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@MADOIPHO", comboBox4.Text.ToString().Trim()));
                sqlCommand.Parameters.Add(new SqlParameter("@TENDOIGIAMSAT", textBox5.Text.ToString().Trim()));

                /*if (comboBox1.Text.ToString().Trim() == "")
                {

                    sqlCommand.Parameters.Add(new SqlParameter("@MAGV1", DBNull.Value));

                }
                else if (comboBox1.Text.ToString().Trim() != "")
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@MAGV1", comboBox1.Text.ToString().Trim()));
                }*/

                /*if (comboBox3.Text.ToString().Trim() == "")
                {

                    sqlCommand.Parameters.Add(new SqlParameter("@MAGV2", DBNull.Value));

                }
                else if (comboBox3.Text.ToString().Trim() != "")
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@MAGV2", comboBox3.Text.ToString().Trim()));
                }*/

                /*if (comboBox2.Text.ToString().Trim() == "")
                {

                    sqlCommand.Parameters.Add(new SqlParameter("@MADOITRUONG", DBNull.Value));

                }
                else if (comboBox2.Text.ToString().Trim() != "")
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@MADOITRUONG", comboBox2.Text.ToString().Trim()));
                }

                if (comboBox4.Text.ToString().Trim() == "")
                {

                    sqlCommand.Parameters.Add(new SqlParameter("@MADOIPHO", DBNull.Value));

                }
                else if (comboBox4.Text.ToString().Trim() != "")
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@MADOIPHO", comboBox4.Text.ToString().Trim()));
                }*/



                Program.ExecSQLCommand(sqlCommand, conn_publisher);
                MessageBox.Show("Sửa đội giám sát thành công!");
                HienThiDuLieu();
            }
        }
    }
}
