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
    public partial class FormMemberInfo : Form
    {
        private MemberInfoBll miBll = new MemberInfoBll();// creates business logic layer object
        private static FormMemberInfo _form = null;// singleton

        private FormMemberInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// form singleton
        /// </summary>
        /// <returns>singleton form</returns>
        public static FormMemberInfo Create()
        {
            if (_form == null)
            {
                _form = new FormMemberInfo();
            }
            return _form;
        }

        /// <summary>
        /// resets singleton _form
        /// </summary>
        /// <param name="sender">FormMemberInfo</param>
        /// <param name="e">data for executing event</param>
        private void FormMemberInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            // When form is being closed, Dispose() is called to release form by Close()
            _form = null;
        }

        /// <summary>
        /// loads undeleted member list and member type list
        /// </summary>
        /// <param name="sender">FormMemberInfo</param>
        /// <param name="e">data for executing event</param>
        private void FormMemberInfo_Load(object sender, EventArgs e)
        {
            LoadList();
            LoadTypeList();
        }

        /// <summary>
        /// loads undeleted member list
        /// </summary>
        private void LoadList()
        {
            // creates a dictionary to save condition
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(txtNameSearch.Text))
            {
                dic.Add("MName", txtNameSearch.Text);
            }
            if (!string.IsNullOrEmpty(txtPhoneSearch.Text))
            {
                dic.Add("MPhone", txtPhoneSearch.Text);
            }

            // forbids dgvList to auto generate columns
            dgvList.AutoGenerateColumns = false;
            // calls method and binds data
            dgvList.DataSource = miBll.GetList("Cater", dic);
        }

        /// <summary>
        /// loads member type list
        /// </summary>
        private void LoadTypeList()
        {
            // gets member type list
            MemberTypeInfoBll mtiBll = new MemberTypeInfoBll();
            List<MemberTypeInfo> list = mtiBll.GetList("Cater");
            // binds data
            ddlType.DataSource = list;
            // sets display member
            ddlType.DisplayMember = "MTitle";
            // sets actually-used value member
            ddlType.ValueMember = "MId";
            // <select><option value="id">title</option></select>
            // unselects all by default
            ddlType.SelectedIndex = -1;
        }

        /// <summary>
        /// finds by name
        /// </summary>
        /// <param name="sender">txtNameSearch</param>
        /// <param name="e">data for executing event</param>
        private void txtNameSearch_TextChanged(object sender, EventArgs e)
        {
            LoadList();
        }

        /// <summary>
        /// finds by phone
        /// </summary>
        /// <param name="sender">txtPhoneSearch</param>
        /// <param name="e">data for executing event</param>
        private void txtPhoneSearch_TextChanged(object sender, EventArgs e)
        {
            LoadList();
        }

        /// <summary>
        /// shows all members
        /// </summary>
        /// <param name="sender">btnSearchAll</param>
        /// <param name="e">data for executing event</param>
        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            txtNameSearch.Clear();
            txtPhoneSearch.Clear();
            LoadList();
        }

        /// <summary>
        /// adds member
        /// </summary>
        /// <param name="sender">btnSave</param>
        /// <param name="e">data for executing event</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            #region validation
            if (string.IsNullOrEmpty(txtNameAdd.Text))
            {
                MessageBox.Show("请输入姓名");
                txtNameAdd.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtPhoneAdd.Text))
            {
                MessageBox.Show("请输入手机号");
                txtPhoneAdd.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtMoney.Text))
            {
                MessageBox.Show("请输入余额");
                txtMoney.Focus();
                return;
            }
            #endregion

            // collects member info
            MemberInfo mi = new MemberInfo()
            {
                MName = txtNameAdd.Text,
                MTypeId = Convert.ToInt32(ddlType.SelectedValue),
                MPhone = txtPhoneAdd.Text,
                MMoney = Convert.ToDecimal(txtMoney.Text),
                MIsDelete = false
            };

            if (txtId.Text.Equals("添加时无编号"))
            {
                #region add
                // calls method to add member
                if (miBll.Add("Cater", mi))
                {
                    // if succeeds, reloads member list
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

                // calls method to edit member
                if (miBll.Edit("Cater", mi))
                {
                    // if succeeds, reloads member list
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
            txtNameAdd.Clear();
            ddlType.SelectedIndex = -1;
            txtPhoneAdd.Clear();
            txtMoney.Clear();
            btnSave.Text = "添加";
        }

        /// <summary>
        /// cancels currently editing member
        /// </summary>
        /// <param name="sender">btnCancel</param>
        /// <param name="e">data for executing event</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Restore();
        }

        /// <summary>
        /// edits member
        /// </summary>
        /// <param name="sender">dgvList</param>
        /// <param name="e">data for executing event</param>
        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // According to what was double clicked, finds row and columns to assign to text boxes
            DataGridViewRow row = dgvList.Rows[e.RowIndex];
            txtId.Text = row.Cells[0].Value.ToString();
            txtNameAdd.Text = row.Cells[1].Value.ToString();
            ddlType.Text = row.Cells[2].Value.ToString();
            txtPhoneAdd.Text = row.Cells[3].Value.ToString();
            txtMoney.Text = row.Cells[4].Value.ToString();
            btnSave.Text = "修改";
        }

        /// <summary>
        /// removes member according to member id
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
                    // gets member id
                    int[] ids = new int[rows.Count];
                    for (int i = 0; i < rows.Count; i++)
                    {
                        ids[i] = Convert.ToInt32(rows[i].Cells[0].Value);
                    }

                    // removes member according to member id
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
        /// manages member type
        /// </summary>
        /// <param name="sender">btnAddType</param>
        /// <param name="e">data for executing event</param>
        private void btnAddType_Click(object sender, EventArgs e)
        {
            FormMemberTypeInfo fMemberType = FormMemberTypeInfo.Create();
            DialogResult result = fMemberType.ShowDialog();
            if (result == DialogResult.OK)
            {
                LoadTypeList();
                LoadList();
            }
        }
    }
}
