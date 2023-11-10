using _02.CaterBLL;
using _04.CaterModel;
using _05.CaterCommon;
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
    public partial class FormDishInfo : Form
    {
        private DishInfoBll diBll = new DishInfoBll();// creates business logic layer object
        private static FormDishInfo _form = null;// singleton

        private FormDishInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// form singleton
        /// </summary>
        /// <returns>singleton form</returns>
        public static FormDishInfo Create()
        {
            if (_form == null)
            {
                _form = new FormDishInfo();
            }
            return _form;
        }

        /// <summary>
        /// resets singleton _form
        /// </summary>
        /// <param name="sender">FormDishInfo</param>
        /// <param name="e">data for executing event</param>
        private void FormDishInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            // When form is being closed, Dispose() is called to release form by Close()
            _form = null;
        }

        /// <summary>
        /// loads undeleted dish list and dish type list
        /// </summary>
        /// <param name="sender">FormDishInfo</param>
        /// <param name="e">data for executing event</param>
        private void FormDishInfo_Load(object sender, EventArgs e)
        {
            LoadList();
            LoadTypeList();
        }

        /// <summary>
        /// loads undeleted dish list
        /// </summary>
        private void LoadList()
        {
            // creates a dictionary to save condition
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(txtTitleSearch.Text))
            {
                dic.Add("DTitle", txtTitleSearch.Text);
            }
            if (ddlTypeSearch.SelectedIndex > 0)
            {
                dic.Add("DTypeId", ddlTypeSearch.SelectedValue.ToString());
            }

            // forbids dgvList to auto generate columns
            dgvList.AutoGenerateColumns = false;
            // calls method and binds data
            dgvList.DataSource = diBll.GetList("Cater", dic);
        }

        /// <summary>
        /// loads dish type list
        /// </summary>
        private void LoadTypeList()
        {
            // gets dish type list
            DishTypeInfoBll dtiBll = new DishTypeInfoBll();

            #region ddlTypeSearch
            List<DishTypeInfo> listSearch = dtiBll.GetList("Cater");
            listSearch.Insert(0, new DishTypeInfo()
            {
                DId = 0,
                DTitle = "全部",
                DIsDelete = false,
            });
            // binds data
            ddlTypeSearch.DataSource = listSearch;
            // sets display dish
            ddlTypeSearch.DisplayMember = "DTitle";
            // sets actually-used value dish
            ddlTypeSearch.ValueMember = "DId";
            // <select><option value="id">title</option></select>
            // unselects all by default
            ddlTypeSearch.SelectedIndex = -1;
            #endregion

            #region ddlTypeAdd
            List<DishTypeInfo> listAdd = dtiBll.GetList("Cater");
            // binds data
            ddlTypeAdd.DataSource = listAdd;
            // sets display dish
            ddlTypeAdd.DisplayMember = "DTitle";
            // sets actually-used value dish
            ddlTypeAdd.ValueMember = "DId";
            // <select><option value="id">title</option></select>
            // unselects all by default
            ddlTypeAdd.SelectedIndex = -1;
            #endregion
        }

        /// <summary>
        /// finds by title
        /// </summary>
        /// <param name="sender">txtTitleSearch</param>
        /// <param name="e">data for executing event</param>
        private void txtTitleSearch_TextChanged(object sender, EventArgs e)
        {
            LoadList();
        }

        /// <summary>
        /// finds by type
        /// </summary>
        /// <param name="sender">ddlTypeSearch</param>
        /// <param name="e">data for executing event</param>
        private void ddlTypeSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadList();
        }

        /// <summary>
        /// shows all dishes
        /// </summary>
        /// <param name="sender">btnSearchAll</param>
        /// <param name="e">data for executing event</param>
        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            txtTitleSearch.Clear();
            ddlTypeSearch.SelectedIndex = -1;
            LoadList();
        }

        /// <summary>
        /// adds dish
        /// </summary>
        /// <param name="sender">btnSave</param>
        /// <param name="e">data for executing event</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            #region validation
            if (string.IsNullOrEmpty(txtTitleAdd.Text))
            {
                MessageBox.Show("请输入名称");
                txtTitleAdd.Focus();
                return;
            }
            if (ddlTypeAdd.SelectedIndex < 0)
            {
                MessageBox.Show("请选择分类");
                ddlTypeAdd.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtPrice.Text))
            {
                MessageBox.Show("请输入价格");
                txtPrice.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtChar.Text))
            {
                MessageBox.Show("请输入拼音");
                txtChar.Focus();
                return;
            }
            #endregion

            // collects dish info
            DishInfo di = new DishInfo()
            {
                DTitle = txtTitleAdd.Text,
                DTypeId = Convert.ToInt32(ddlTypeAdd.SelectedValue),
                DPrice = Convert.ToDecimal(txtPrice.Text),
                DChar = txtChar.Text,
                DIsDelete = false
            };

            if (txtId.Text.Equals("添加时无编号"))
            {
                #region add
                // calls method to add dish
                if (diBll.Add("Cater", di))
                {
                    // if succeeds, reloads dish list
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
                di.DId = int.Parse(txtId.Text);

                // calls method to edit dish
                if (diBll.Edit("Cater", di))
                {
                    // if succeeds, reloads dish list
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
        /// gets each char's first Pinyin letter
        /// </summary>
        /// <param name="sender">txtTitleAdd</param>
        /// <param name="e">data for executing event</param>
        private void txtTitleAdd_Leave(object sender, EventArgs e)
        {
            txtChar.Text = PinyinHelper.GetPinyin(txtTitleAdd.Text);
        }

        /// <summary>
        /// resets text boxes
        /// </summary>
        private void Restore()
        {
            txtId.Text = "添加时无编号";
            txtTitleAdd.Clear();
            ddlTypeAdd.SelectedIndex = -1;
            txtPrice.Clear();
            txtChar.Clear();
            btnSave.Text = "添加";
        }

        /// <summary>
        /// cancels currently editing dish
        /// </summary>
        /// <param name="sender">btnCancel</param>
        /// <param name="e">data for executing event</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Restore();
        }

        /// <summary>
        /// edits dish
        /// </summary>
        /// <param name="sender">dgvList</param>
        /// <param name="e">data for executing event</param>
        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // According to what was double clicked, finds row and columns to assign to text boxes
            DataGridViewRow row = dgvList.Rows[e.RowIndex];
            txtId.Text = row.Cells[0].Value.ToString();
            txtTitleAdd.Text = row.Cells[1].Value.ToString();
            ddlTypeAdd.Text = row.Cells[2].Value.ToString();
            txtPrice.Text = row.Cells[3].Value.ToString();
            txtChar.Text = row.Cells[4].Value.ToString();
            btnSave.Text = "修改";
        }

        /// <summary>
        /// removes dish according to dish id
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
                    // gets dish id
                    int[] ids = new int[rows.Count];
                    for (int i = 0; i < rows.Count; i++)
                    {
                        ids[i] = Convert.ToInt32(rows[i].Cells[0].Value);
                    }

                    // removes dish according to dish id
                    if (diBll.Remove("Cater", ids))
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
        /// manages dish type
        /// </summary>
        /// <param name="sender">btnAddType</param>
        /// <param name="e">data for executing event</param>
        private void btnAddType_Click(object sender, EventArgs e)
        {
            FormDishTypeInfo fDishType = FormDishTypeInfo.Create();
            DialogResult result = fDishType.ShowDialog();
            if (result == DialogResult.OK)
            {
                LoadTypeList();
                LoadList();
            }
        }
    }
}
