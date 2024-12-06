using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonnelManagementSystem
{
    public partial class LoginForm : Form
    {
        private DataAccess db = new DataAccess();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text; // 实际中应先进行哈希

            if (db.ValidateUser(username, password, out string role, out bool isActive))
            {
                if (!isActive)
                {
                    MessageBox.Show("账户已被禁用！");
                    return;
                }

                // 登录成功，跳转到主界面
                MainForm mainForm = new MainForm(username, role);
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("用户名或密码不正确！");
            }
        }
    }
}