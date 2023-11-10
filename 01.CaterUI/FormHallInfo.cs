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
    public partial class FormHallInfo : Form
    {
        private HallInfoBll hiBll = new HallInfoBll();// creates business logic layer object
        private static FormHallInfo _form = null;// singleton
        public event Action HallUpdated;

        private FormHallInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// form singleton
        /// </summary>
        /// <returns>singleton form</returns>
        public static FormHallInfo Create()
        {
            if (_form == null)
            {
                _form = new FormHallInfo();
            }
            return _form;
        }

        /// <summary>
        /// resets singleton _form
        /// </summary>
        /// <param name="sender">FormHallInfo</param>
        /// <param name="e">data for executing event</param>
        private void FormHallInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            // When form is being closed, Dispose() is called to release form by Close()
            _form = null;
        }

        /// <summary>
        /// loads undeleted hall list
        /// </summary>
        /// <param name="sender">FormHallInfo</param>
        /// <param name="e">data for executing event</param>
        private void FormHallInfo_Load(object sender, EventArgs e)
        {
            LoadList();
        }

        /// <summary>
        /// loads undeleted hall list
        /// </summary>
        private void LoadList()
        {
            // forbids dgvList to auto generate columns
            dgvList.AutoGenerateColumns = false;
            // calls method and binds data
            dgvList.DataSource = hiBll.GetList("Cater");
        }

        /// <summary>
        /// adds hall
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

            // collects hall info
            HallInfo hi = new HallInfo()
            {
                HTitle = txtTitle.Text,
                HIsDelete = false
            };

            if (txtId.Text.Equals("添加时无编号"))
            {
                #region add
                // calls method to add hall
                if (hiBll.Add("Cater", hi))
                {
                    // if succeeds, reloads hall list
                    LoadList();
                    HallUpdated();
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
                hi.HId = int.Parse(txtId.Text);

                // calls method to edit hall
                if (hiBll.Edit("Cater", hi))
                {
                    // if succeeds, reloads hall list
                    LoadList();
                    HallUpdated();
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
            txtTitle.Clear();
            btnSave.Text = "添加";
        }

        /// <summary>
        /// cancels currently editing hall
        /// </summary>
        /// <param name="sender">btnCancel</param>
        /// <param name="e">data for executing event</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Restore();
        }

        /// <summary>
        /// edits hall
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
        /// removes hall according to hall id
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
                    // gets hall id
                    int[] ids = new int[rows.Count];
                    for (int i = 0; i < rows.Count; i++)
                    {
                        ids[i] = Convert.ToInt32(rows[i].Cells[0].Value);
                    }

                    // removes hall according to hall id
                    if (hiBll.Remove("Cater", ids))
                    {
                        LoadList();
                        HallUpdated();
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
    }
}
