using _02.CaterBLL;
using _04.CaterModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _01.CaterUI
{
    public partial class FormLogin : Form
    {
        private ManagerInfoBll miBll = new ManagerInfoBll();// creates business logic layer object

        public FormLogin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// closes form
        /// </summary>
        /// <param name="sender">btnClose</param>
        /// <param name="e">data for executing event</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// logs in system
        /// </summary>
        /// <param name="sender">btnLogin</param>
        /// <param name="e">data for executing event</param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // collects user info
            string username = txtName.Text;
            string password = txtPassword.Text;

            // calls method to log in
            LoginState state = miBll.Login("Cater", username, password, out int type);

            switch (state)
            {
                case LoginState.OK:
                    FormMain fMain = new FormMain();
                    fMain.Tag = type;// passes employee type
                    fMain.Show();
                    this.Hide();
                    break;
                case LoginState.NameError:
                    MessageBox.Show("用户名错误");
                    break;
                case LoginState.PasswordError:
                    MessageBox.Show("密码错误");
                    break;
            }
        }
    }
}
