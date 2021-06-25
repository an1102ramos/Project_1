using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource accountList = new BindingSource();
        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            Load();
        }

        #region Phương thức
        void Load()
        {
            dtgvFood.DataSource = foodList;
            dtgvAccount.DataSource = accountList;
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListFood();
            LoadAccount();
            LoadCategoryIntoCombobox(cbFoodCategory);
            AddFoodBinding();
            AddAccountBinding();
        }
        void AddAccountBinding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            numericUpDown1.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }
        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }
        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }
        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }

            LoadAccount();
        }
        void EditAccount(string userName, string displayName, int type)
        {
            
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Vui lòng đăng xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();

            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại");
            }

            LoadAccount();
        }
        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Vui lòng không xóa tài khoản đang đăng nhập!","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            LoadAccount();
        }
        void ResetPass(string userName, string pass)
        {
            if (AccountDAO.Instance.ResetPassword(userName,pass))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
        }
        void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }
        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }
        #endregion

        #region Sự kiện
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }
        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }       
        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            if (dtgvFood.SelectedCells.Count > 0)
            {
                int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;

                Category cateogory = CategoryDAO.Instance.GetCategoryByID(id);

                cbFoodCategory.SelectedItem = cateogory;

                int index = -1;
                int i = 0;
                foreach (Category item in cbFoodCategory.Items)
                {
                    if (item.ID == cateogory.ID)
                    {
                        index = i;
                        break;
                    }
                    i++;
                }

                cbFoodCategory.SelectedIndex = index;
            }
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            try
            {

            
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm đồ uống");
            }
            }
            catch
            {
                MessageBox.Show("Có lỗi khi thêm đồ uống");
            }
        }
        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa thức ăn");
            }
        }
        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa thức ăn");
            }
        }
        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }
        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }
        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txbFoodName.Text = "";
            cbFoodCategory.Text = "";
            nmFoodPrice.Value = 0;
            txbFoodID.Text = "";
        }
        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = txbUserName.Text;
                string displayName = txbDisplayName.Text;
                int type = (int)numericUpDown1.Value;

                AddAccount(userName, displayName, type);
            }
            catch
            {
                MessageBox.Show("Trùng tên đăng nhập!","Thông báo!",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;

            DeleteAccount(userName);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {

            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)numericUpDown1.Value;

            EditAccount(userName, displayName, type);
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string pass = txbreset.Text;
            if (txbreset.Text == "")
            {
                MessageBox.Show("Nhập mật khẩu cần đặt lại!");
            }
            else
            {
                ResetPass(userName, pass);
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            txbUserName.Text = "";
            txbDisplayName.Text = "";
            numericUpDown1.Value = 0;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wk = app.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel._Worksheet sht = null;
            
            
                sht = wk.Sheets["Sheet1"];
                sht = wk.ActiveSheet;
                app.Visible = true;
                sht.Cells[1, 1] = "DANH SÁCH HÓA  ĐƠN";
                sht.Cells[2, 1] = "STT";
                sht.Cells[2, 2] = "Tên bàn ";
                sht.Cells[2, 3] = "Tổng tiền";
                sht.Cells[2, 4] = "Ngày vào";           
                sht.Cells[2, 5] = "Ngày ra";
                sht.Cells[2, 6] = "Giảm giá";

                for (int i = 0; i < dtgvBill.RowCount - 1; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        sht.Cells[i + 3, 1] = i + 1;
                        sht.Cells[i + 3, j + 2] = dtgvBill.Rows[i].Cells[j].Value;
                    }
                }
            
            
        }
        #endregion
    }
}
