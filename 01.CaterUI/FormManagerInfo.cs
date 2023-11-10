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
    public partial class FormManagerInfo : Form
    {
        private ManagerInfoBll miBll = new ManagerInfoBll();// creates business logic layer object
        private static FormManagerInfo _form = null;// singleton

        private FormManagerInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// form singleton
        /// </summary>
        /// <returns>singleton form</returns>
        public static FormManagerInfo Create()
        {
            if (_form == null)
            {
                _form = new FormManagerInfo();
            }
            return _form;
        }

        /// <summary>
        /// loads user list
        /// </summary>
        /// <param name="sender">FormManagerInfo</param>
        /// <param name="e">data for executing event</param>
        private void FormManagerInfo_Load(object sender, EventArgs e)
        {
            LoadList();
        }

        /// <summary>
        /// loads user list
        /// </summary>
        private void LoadList()
        {
            // forbids dgvList to auto generate columns
            dgvList.AutoGenerateColumns = false;
            // calls method and binds data
            dgvList.DataSource = miBll.GetList("Cater");
        }

        /// <summary>
        /// adds user
        /// </summary>
        /// <param name="sender">btnSave</param>
        /// <param name="e">data for executing event</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            #region validation
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("请输入用户名");
                txtName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtPwd.Text))
            {
                MessageBox.Show("请输入密码");
                txtPwd.Focus();
                return;
            }
            #endregion

            // collects user info
            ManagerInfo mi = new ManagerInfo()
            {
                MName = txtName.Text,
                MPwd = txtPwd.Text,
                MType = rb1.Checked ? 1 : 0
            };

            if (txtId.Text.Equals("添加时无编号"))
            {
                #region add
                // calls method to add user
                if (miBll.Add("Cater", mi))
                {
                    // if succeeds, reloads user list
                    LoadList();
                }
                else
                {
                    // otherwise pops up message
                    MessageBox.Show("添加失败，请稍后重试！");
                }
                #endregion
            }
            else
            {
                #region edit
                mi.MId = int.Parse(txtId.Text);

                // calls method to edit user
                if (miBll.Edit("Cater", mi))
                {
                    // if succeeds, reloads user list
                    LoadList();
                }
                else
                {
                    // otherwise pops up message
                    MessageBox.Show("修改失败，请稍后重试！");
                }
                #endregion
            }

            // restores text boxes
            Restore();
        }

        /// <summary>
        /// resets text boxes
        /// </summary>
        private void Restore()
        {
            txtId.Text = "添加时无编号";
            txtName.Clear();
            txtPwd.Clear();
            rb2.Checked = true;
            btnSave.Text = "添加";
        }

        /// <summary>
        /// formats Type
        /// </summary>
        /// <param name="sender">dgvList</param>
        /// <param name="e">data for executing event</param>
        private void dgvList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                e.Value = Convert.ToInt32(e.Value) == 1 ? "经理" : "店员";
            }
        }

        /// <summary>
        /// edits user
        /// </summary>
        /// <param name="sender">dgvList</param>
        /// <param name="e">data for executing event</param>
        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // According to what was double clicked, finds row and columns to assign to text boxes
            DataGridViewRow row = dgvList.Rows[e.RowIndex];
            txtId.Text = row.Cells[0].Value.ToString();
            txtName.Text = row.Cells[1].Value.ToString();
            if (row.Cells[2].Value.ToString().Equals("1"))
            {
                rb1.Checked = true;
                rb2.Checked = false;
            }
            else
            {
                rb2.Checked = true;
                rb1.Checked = false;
            }
            txtPwd.Text = "旧密码";
            btnSave.Text = "修改";
        }

        /// <summary>
        /// cancels currently editing user
        /// </summary>
        /// <param name="sender">btnCancel</param>
        /// <param name="e">data for executing event</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Restore();
        }

        /// <summary>
        /// removes user according to user id
        /// </summary>
        /// <param name="sender">btnRemove</param>
        /// <param name="e">data for executing event</param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            // gets selected rows
            DataGridViewSelectedRowCollection rows = dgvList.SelectedRows;

            if (rows.Count > 0)
            {
                // asks whether to remove
                DialogResult result = MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes)
                {
                    // gets user id
                    int[] ids = new int[rows.Count];
                    for (int i = 0; i < rows.Count; i++)
                    {
                        ids[i] = Convert.ToInt32(rows[i].Cells[0].Value);
                    }

                    // removes user according to user id
                    if (miBll.Remove("Cater", ids))
                    {
                        LoadList();
                    }
                    else
                    {
                        MessageBox.Show("删除失败，请稍后重试！");
                    }
                }
            }
            else
            {
                MessageBox.Show("请先选择要删除的行！");
            }
        }

        /// <summary>
        /// resets singleton _form
        /// </summary>
        /// <param name="sender">FormManagerInfo</param>
        /// <param name="e">data for executing event</param>
        private void FormManagerInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            // When form is being closed, Dispose() is called to release form by Close()
            _form = null;
        }
    }
}
