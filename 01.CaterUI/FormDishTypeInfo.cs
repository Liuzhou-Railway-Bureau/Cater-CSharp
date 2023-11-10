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
    public partial class FormDishTypeInfo : Form
    {
        private DishTypeInfoBll dtiBll = new DishTypeInfoBll();// creates business logic layer object
        private static FormDishTypeInfo _form = null;// singleton
        private DialogResult result = DialogResult.Cancel;

        private FormDishTypeInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// form singleton
        /// </summary>
        /// <returns>singleton form</returns>
        public static FormDishTypeInfo Create()
        {
            if (_form == null)
            {
                _form = new FormDishTypeInfo();
            }
            return _form;
        }

        /// <summary>
        /// resets singleton _form
        /// </summary>
        /// <param name="sender">FormDishTypeInfo</param>
        /// <param name="e">data for executing event</param>
        private void FormDishTypeInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            // When form is being closed, Dispose() is called to release form by Close()
            _form = null;
            this.DialogResult = result;
        }

        /// <summary>
        /// loads undeleted dish type list
        /// </summary>
        /// <param name="sender">FormDishTypeInfo</param>
        /// <param name="e">data for executing event</param>
        private void FormDishTypeInfo_Load(object sender, EventArgs e)
        {
            LoadList();
        }

        /// <summary>
        /// loads undeleted dish type list
        /// </summary>
        private void LoadList()
        {
            // forbids dgvList to auto generate columns
            dgvList.AutoGenerateColumns = false;
            // calls method and binds data
            dgvList.DataSource = dtiBll.GetList("Cater");
        }

        /// <summary>
        /// adds dish type
        /// </summary>
        /// <param name="sender">btnSave</param>
        /// <param name="e">data for executing event</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            #region validation
            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                MessageBox.Show("请输入标题");
                txtTitle.Focus();
                return;
            }
            #endregion

            // collects dish type info
            DishTypeInfo dti = new DishTypeInfo()
            {
                DTitle = txtTitle.Text,
                DIsDelete = false
            };

            if (txtId.Text.Equals("添加时无编号"))
            {
                #region add
                // calls method to add dish type
                if (dtiBll.Add("Cater", dti))
                {
                    // if succeeds, reloads dish type list
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
                dti.DId = int.Parse(txtId.Text);

                // calls method to edit dish type
                if (dtiBll.Edit("Cater", dti))
                {
                    // if succeeds, reloads dish type list
                    LoadList();
                }
                else
                {
                    // otherwise pops up message
                    MessageBox.Show("修改失败，请稍后重试！");
                }
                #endregion
            }

            result = DialogResult.OK;

            // restores text boxes
            Restore();
        }

        /// <summary>
        /// resets text boxes
        /// </summary>
        private void Restore()
        {
            txtId.Text = "添加时无编号";
            txtTitle.Clear();
            btnSave.Text = "添加";
        }

        /// <summary>
        /// cancels currently editing dish type
        /// </summary>
        /// <param name="sender">btnCancel</param>
        /// <param name="e">data for executing event</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Restore();
        }

        /// <summary>
        /// edits dish type
        /// </summary>
        /// <param name="sender">dgvList</param>
        /// <param name="e">data for executing event</param>
        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // According to what was double clicked, finds row and columns to assign to text boxes
            DataGridViewRow row = dgvList.Rows[e.RowIndex];
            txtId.Text = row.Cells[0].Value.ToString();
            txtTitle.Text = row.Cells[1].Value.ToString();
            btnSave.Text = "修改";
        }

        /// <summary>
        /// removes dish type according to dish type id
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
                    // gets dish type id
                    int[] ids = new int[rows.Count];
                    for (int i = 0; i < rows.Count; i++)
                    {
                        ids[i] = Convert.ToInt32(rows[i].Cells[0].Value);
                    }

                    // removes dish type according to dish type id
                    if (dtiBll.Remove("Cater", ids))
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

            result = DialogResult.OK;
        }
    }
}
