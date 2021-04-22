using QuanLyQuanCafe.DTO;
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

namespace QuanLyQuanCafe
{
    public partial class fAccountProfile : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); tt(loginAccount); }
        }
        public fAccountProfile(Account acc)
        {
            InitializeComponent();
            LoginAccount = acc;
        }
        void ChangeAccount(Account acc)
        {
            txtbUserName.Text = LoginAccount.UserName;
            txtbDisplayName.Text = LoginAccount.DisplayName;
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void tt(Account acc)
        {
            if(LoginAccount.Type == 1)
            {
                txtbQuyen.Text = " Quản lý";
            }
            else if(LoginAccount.Type == 0)
            {

                txtbQuyen.Text = " Nhân viên";
            }    
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thay đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK);
                 
            
        }
    }
}
