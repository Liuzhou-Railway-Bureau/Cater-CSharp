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
    public partial class FormOrderDish : Form
    {
        private OrderInfoBll oiBll = new OrderInfoBll();// creates business logic layer object
        private static FormOrderDish _form = null;// singleton

        private FormOrderDish()
        {
            InitializeComponent();
        }

        /// <summary>
        /// form singleton
        /// </summary>
        /// <returns>singleton form</returns>
        public static FormOrderDish Create()
        {
            if (_form == null)
            {
                _form = new FormOrderDish();
            }
            return _form;
        }

        /// <summary>
        /// resets singleton _form
        /// </summary>
        /// <param name="sender">FormOrderDish</param>
        /// <param name="e">data for executing event</param>
        private void FormOrderDish_FormClosing(object sender, FormClosingEventArgs e)
        {
            // When form is being closed, Dispose() is called to release form by Close()
            _form = null;
        }

        /// <summary>
        /// loads undeleted dish list and dish type list
        /// </summary>
        /// <param name="sender">FormOrderDish</param>
        /// <param name="e">data for executing event</param>
        private void FormOrderDish_Load(object sender, EventArgs e)
        {
            LoadDishList();
            LoadDishTypeList();
            LoadDetailList();
        }

        /// <summary>
        /// loads undeleted dish list
        /// </summary>
        private void LoadDishList()
        {
            // creates DishInfoBll
            DishInfoBll diBll = new DishInfoBll();
            // creates a dictionary to save condition
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(txtTitle.Text))
            {
                dic.Add("DChar", txtTitle.Text);
            }
            if (ddlType.SelectedIndex > 0)
            {
                dic.Add("DTypeId", ddlType.SelectedValue.ToString());
            }

            // forbids dgvAllDish to auto generate columns
            dgvAllDish.AutoGenerateColumns = false;
            // calls method and binds data
            dgvAllDish.DataSource = diBll.GetList("Cater", dic);
        }

        /// <summary>
        /// loads dish type list
        /// </summary>
        private void LoadDishTypeList()
        {
            // gets dish type list
            DishTypeInfoBll dtiBll = new DishTypeInfoBll();
            List<DishTypeInfo> list = dtiBll.GetList("Cater");
            list.Insert(0, new DishTypeInfo()
            {
                DId = 0,
                DTitle = "全部",
                DIsDelete = false,
            });
            // binds data
            ddlType.DataSource = list;
            // sets display dish
            ddlType.DisplayMember = "DTitle";
            // sets actually-used value dish
            ddlType.ValueMember = "DId";
            // <select><option value="id">title</option></select>
            // unselects all by default
            ddlType.SelectedIndex = -1;
        }

        /// <summary>
        /// loads order detail list
        /// </summary>
        private void LoadDetailList()
        {
            // gets order id
            int orderId = Convert.ToInt32(this.Tag);

            // forbids dgvOrderDetail to auto generate columns
            dgvOrderDetail.AutoGenerateColumns = false;
            // calls method and binds data
            dgvOrderDetail.DataSource = oiBll.GetDetailList("Cater", orderId);

            // calculates total price
            GetSum();
        }

        /// <summary>
        /// gets total price
        /// </summary>
        private void GetSum()
        {
            // gets order id
            int orderId = Convert.ToInt32(this.Tag);
            // gets total price
            decimal sum = oiBll.GetTotalPrice("Cater", orderId);
            if (sum > 0)
            {
                lblMoney.Text = sum.ToString();
            }
        }

        /// <summary>
        /// finds by title
        /// </summary>
        /// <param name="sender">txtTitle</param>
        /// <param name="e">data for executing event</param>
        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            LoadDishList();
        }

        /// <summary>
        /// finds by type
        /// </summary>
        /// <param name="sender">ddlType</param>
        /// <param name="e">data for executing event</param>
        private void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDishList();
        }

        /// <summary>
        /// orders dish
        /// </summary>
        /// <param name="sender">dgvAllDish</param>
        /// <param name="e">data for executing event</param>
        private void dgvAllDish_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // gets order id and dish id
            int orderId = Convert.ToInt32(this.Tag);
            int dishId = Convert.ToInt32(dgvAllDish.Rows[e.RowIndex].Cells[0].Value);
            // orders dish
            if (oiBll.OrderDish("Cater", orderId, dishId))
            {
                // if succeeds, loads order detail list
                LoadDetailList();
            }
            else
            {
                // otherwise pops up message
                MessageBox.Show("点菜失败，请重试！");
            }
        }

        /// <summary>
        /// updates dish count
        /// </summary>
        /// <param name="sender">dgvOrderDetail</param>
        /// <param name="e">data for executing event</param>
        private void dgvOrderDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                // gets updated row
                DataGridViewRow row = dgvOrderDetail.Rows[e.RowIndex];
                // gets order id and dish count
                int orderId = Convert.ToInt32(row.Cells[0].Value);
                int count = Convert.ToInt32(row.Cells[2].Value);
                // updates dish count
                if (oiBll.SetCount("Cater", orderId, count))
                {
                    // if succeeds, calculates total price
                    GetSum();
                }
                else
                {
                    // if fails, pops up message
                    MessageBox.Show("点菜失败，请重试！");
                }
            }
        }

        /// <summary>
        /// places order
        /// </summary>
        /// <param name="sender">btnOrder</param>
        /// <param name="e">data for executing event</param>
        private void btnOrder_Click(object sender, EventArgs e)
        {
            // gets order id and total price
            int orderId = Convert.ToInt32(this.Tag);
            decimal sum = Convert.ToDecimal(lblMoney.Text);
            // places order
            if (oiBll.PlaceOrder("Cater", orderId, sum))
            {
                // if succeeds, prompts OK
                MessageBox.Show("下单成功");
            }
            else
            {
                // otherwise prompts failure
                MessageBox.Show("点菜失败，请重试！");
            }
        }

        /// <summary>
        /// removes detail
        /// </summary>
        /// <param name="sender">btnRemove</param>
        /// <param name="e">data for executing event</param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            // gets selected rows
            DataGridViewSelectedRowCollection rows = dgvOrderDetail.SelectedRows;

            if (rows.Count > 0)
            {
                // asks whether to remove
                DialogResult result = MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes)
                {
                    // gets detail id
                    int[] ids = new int[rows.Count];
                    for (int i = 0; i < rows.Count; i++)
                    {
                        ids[i] = Convert.ToInt32(rows[i].Cells[0].Value);
                    }

                    // removes detail according to detail id
                    if (oiBll.RemoveDetail("Cater", ids))
                    {
                        LoadDetailList();
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
