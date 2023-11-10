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
    public partial class FormTableInfo : Form
    {
        private TableInfoBll tiBll = new TableInfoBll();// creates business logic layer object
        private static FormTableInfo _form = null;// singleton
        public event Action TableUpdated;

        private FormTableInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// form singleton
        /// </summary>
        /// <returns>singleton form</returns>
        public static FormTableInfo Create()
        {
            if (_form == null)
            {
                _form = new FormTableInfo();
            }
            return _form;
        }

        /// <summary>
        /// resets singleton _form
        /// </summary>
        /// <param name="sender">FormTableInfo</param>
        /// <param name="e">data for executing event</param>
        private void FormTableInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            // When form is being closed, Dispose() is called to release form by Close()
            _form = null;
        }

        /// <summary>
        /// loads undeleted table list and hall list
        /// </summary>
        /// <param name="sender">FormTableInfo</param>
        /// <param name="e">data for executing event</param>
        private void FormTableInfo_Load(object sender, EventArgs e)
        {
            LoadList();
            LoadSearchList();
        }

        /// <summary>
        /// loads undeleted table list
        /// </summary>
        private void LoadList()
        {
            // creates a dictionary to save condition
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (ddlHallSearch.SelectedIndex > 0)
            {
                dic.Add("THallId", ddlHallSearch.SelectedValue.ToString());
            }
            if (ddlFreeSearch.SelectedIndex > 0)
            {
                dic.Add("TIsFree", ddlFreeSearch.SelectedValue.ToString());
            }

            // forbids dgvList to auto generate columns
            dgvList.AutoGenerateColumns = false;
            // calls method and binds data
            dgvList.DataSource = tiBll.GetList("Cater", dic);
        }

        /// <summary>
        /// loads search list
        /// </summary>
        private void LoadSearchList()
        {
            // gets hall list
            HallInfoBll hiBll = new HallInfoBll();

            #region ddlHallSearch
            List<HallInfo> listHallSearch = hiBll.GetList("Cater");
            listHallSearch.Insert(0, new HallInfo()
            {
                HId = 0,
                HTitle = "全部",
                HIsDelete = false,
            });
            // binds data
            ddlHallSearch.DataSource = listHallSearch;
            // sets display hall
            ddlHallSearch.DisplayMember = "HTitle";
            // sets actually-used value hall
            ddlHallSearch.ValueMember = "HId";
            // <select><option value="id">title</option></select>
            // unselects all by default
            ddlHallSearch.SelectedIndex = -1;
            #endregion

            #region ddlFreeSearch
            List<DdlFreeSearchModel> listFreeSearch = new List<DdlFreeSearchModel>()
            {
                new DdlFreeSearchModel("-1", "全部"),
                new DdlFreeSearchModel("1", "空闲"),
                new DdlFreeSearchModel("0", "使用中")
            };
            // binds data
            ddlFreeSearch.DataSource = listFreeSearch;
            // sets display table
            ddlFreeSearch.DisplayMember = "Title";
            // sets actually-used value table
            ddlFreeSearch.ValueMember = "Id";
            // <select><option value="id">title</option></select>
            // unselects all by default
            ddlFreeSearch.SelectedIndex = -1;
            #endregion

            #region ddlHallAdd
            List<HallInfo> listHallAdd = hiBll.GetList("Cater");
            // binds data
            ddlHallAdd.DataSource = listHallAdd;
            // sets display hall
            ddlHallAdd.DisplayMember = "HTitle";
            // sets actually-used value hall
            ddlHallAdd.ValueMember = "HId";
            // <select><option value="id">title</option></select>
            // unselects all by default
            ddlHallAdd.SelectedIndex = -1;
            #endregion
        }

        /// <summary>
        /// finds by hall
        /// </summary>
        /// <param name="sender">ddlHallSearch</param>
        /// <param name="e">data for executing event</param>
        private void ddlHallSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadList();
        }

        /// <summary>
        /// finds by isFree
        /// </summary>
        /// <param name="sender">ddlFreeSearch</param>
        /// <param name="e">data for executing event</param>
        private void ddlFreeSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadList();
        }

        /// <summary>
        /// shows all tables
        /// </summary>
        /// <param name="sender">btnSearchAll</param>
        /// <param name="e">data for executing event</param>
        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            ddlHallSearch.SelectedIndex = -1;
            ddlFreeSearch.SelectedIndex = -1;
            LoadList();
        }

        /// <summary>
        /// adds table
        /// </summary>
        /// <param name="sender">btnSave</param>
        /// <param name="e">data for executing event</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            #region validation
            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                MessageBox.Show("请输入名称");
                txtTitle.Focus();
                return;
            }
            if (ddlHallAdd.SelectedIndex < 0)
            {
                MessageBox.Show("请选择厅包");
                ddlHallAdd.Focus();
                return;
            }
            #endregion

            // collects table info
            TableInfo ti = new TableInfo()
            {
                TTitle = txtTitle.Text,
                THallId = Convert.ToInt32(ddlHallAdd.SelectedValue),
                TIsFree = rbFree.Checked ? true : false,
                TIsDelete = false
            };

            if (txtId.Text.Equals("添加时无编号"))
            {
                #region add
                // calls method to add table
                if (tiBll.Add("Cater", ti))
                {
                    // if succeeds, reloads table list
                    LoadList();
                    TableUpdated();
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
                ti.TId = int.Parse(txtId.Text);

                // calls method to edit table
                if (tiBll.Edit("Cater", ti))
                {
                    // if succeeds, reloads table list
                    LoadList();
                    TableUpdated();
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
            ddlHallAdd.SelectedIndex = -1;
            rbFree.Checked = true;
            btnSave.Text = "添加";
        }

        /// <summary>
        /// cancels currently editing table
        /// </summary>
        /// <param name="sender">btnCancel</param>
        /// <param name="e">data for executing event</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Restore();
        }

        /// <summary>
        /// formats IsFree
        /// </summary>
        /// <param name="sender">dgvList</param>
        /// <param name="e">data for executing event</param>
        private void dgvList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                e.Value = Convert.ToBoolean(e.Value) ? "空闲" : "使用中";
            }
        }

        /// <summary>
        /// edits table
        /// </summary>
        /// <param name="sender">dgvList</param>
        /// <param name="e">data for executing event</param>
        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // According to what was double clicked, finds row and columns to assign to text boxes
            DataGridViewRow row = dgvList.Rows[e.RowIndex];
            txtId.Text = row.Cells[0].Value.ToString();
            txtTitle.Text = row.Cells[1].Value.ToString();
            ddlHallAdd.Text = row.Cells[2].Value.ToString();
            if (Convert.ToBoolean(row.Cells[3].Value))
            {
                rbFree.Checked = true;
                rbUnFree.Checked = false;
            }
            else
            {
                rbUnFree.Checked = true;
                rbFree.Checked = false;
            }
            btnSave.Text = "修改";
        }

        /// <summary>
        /// removes table according to table id
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
                    // gets table id
                    int[] ids = new int[rows.Count];
                    for (int i = 0; i < rows.Count; i++)
                    {
                        ids[i] = Convert.ToInt32(rows[i].Cells[0].Value);
                    }

                    // removes table according to table id
                    if (tiBll.Remove("Cater", ids))
                    {
                        LoadList();
                        TableUpdated();
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
        /// manages hall
        /// </summary>
        /// <param name="sender">btnAddHall</param>
        /// <param name="e">data for executing event</param>
        private void btnAddHall_Click(object sender, EventArgs e)
        {
            FormHallInfo fHall = FormHallInfo.Create();
            fHall.HallUpdated += LoadList;
            fHall.HallUpdated += LoadSearchList;
            fHall.ShowDialog();
        }
    }
}
