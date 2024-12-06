using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonnelManagementSystem
{
    public partial class MainForm : Form
    {
        private string currentUsername;
        private string currentRole;
        private DataAccess db = new DataAccess();

        public MainForm(string username, string role)
        {
            InitializeComponent();
            currentUsername = username;
            currentRole = role;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = "欢迎：" + currentUsername + " [" + currentRole + "]";

            // 根据角色和权限动态控制按钮显示
            // 超级管理员一定有用户管理权限
            if (currentRole == "SUPER_ADMIN")
            {
                btnUserManagement.Visible = true;
            }

            // 如果当前用户在EntryUsers中则显示录入界面入口
            if (db.IsInEntryUsers(currentUsername))
            {
                btnDataEntry.Visible = true;
            }

            // 如果当前用户在ViewUsers中则显示数据查询入口
            if (db.IsInViewUsers(currentUsername))
            {
                btnDataView.Visible = true;
            }
        }

        private void btnUserManagement_Click(object sender, EventArgs e)
        {
            // 超级管理员的用户管理界面
            AdminForm af = new AdminForm();
            af.ShowDialog();
        }

        private void btnDataEntry_Click(object sender, EventArgs e)
        {
            // 数据录入界面（只有在EntryUsers中才会显示）
            EntryForm ef = new EntryForm(currentUsername);
            ef.ShowDialog();
        }

        private void btnDataView_Click(object sender, EventArgs e)
        {
            // 数据查看界面（只有在ViewUsers中才会显示）
            ViewForm vf = new ViewForm();
            vf.ShowDialog();
        }
    }
}