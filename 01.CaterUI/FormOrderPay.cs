using _02.CaterBLL;
using _04.CaterModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _01.CaterUI
{
    public partial class FormOrderPay : Form
    {
        private OrderInfoBll oiBll = new OrderInfoBll();
        private static FormOrderPay _form = null;// singleton
        public new event Action Refresh;
        private int orderId = -1;

        private FormOrderPay()
        {
            InitializeComponent();
        }

        /// <summary>
        /// form singleton
        /// </summary>
        /// <returns>singleton form</returns>
        public static FormOrderPay Create()
        {
            if (_form == null)
            {
                _form = new FormOrderPay();
            }
            return _form;
        }

        /// <summary>
        /// resets singleton _form
        /// </summary>
        /// <param name="sender">FormOrderPay</param>
        /// <param name="e">data for executing event</param>
        private void FormOrderPay_FormClosing(object sender, FormClosingEventArgs e)
        {
            // When form is being closed, Dispose() is called to release form by Close()
            _form = null;
        }

        /// <summary>
        /// shows total money
        /// </summary>
        /// <param name="sender">FormOrderPay</param>
        /// <param name="e">data for executing event</param>
        private void FormOrderPay_Load(object sender, EventArgs e)
        {
            gbMember.Enabled = false;
            // gets order id
            orderId = Convert.ToInt32(this.Tag);
            // gets total money
            GetMoney();
        }

        /// <summary>
        /// gets total money
        /// </summary>
        private void GetMoney()
        {
            decimal sum = oiBll.GetTotalPrice("Cater", orderId);
            lblPayMoney.Text = sum.ToString();
            lblPayMoneyDiscount.Text = lblPayMoney.Text;
        }

        /// <summary>
        /// enables or disables gbMember
        /// </summary>
        /// <param name="sender">cbkMember</param>
        /// <param name="e">data for executing event</param>
        private void cbkMember_CheckedChanged(object sender, EventArgs e)
        {
            gbMember.Enabled = cbkMember.Checked;
        }

        /// <summary>
        /// loads undeleted member
        /// </summary>
        private void LoadMember()
        {
            // queries member according to member ID or phone number
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(txtId.Text))
            {
                dic.Add("MId", txtId.Text);
            }
            if (!string.IsNullOrEmpty(txtPhone.Text))
            {
                dic.Add("MPhone", txtPhone.Text);
            }

            MemberInfoBll miBll = new MemberInfoBll();
            List<MemberInfo> list = miBll.GetList("Cater", dic);
            if (list.Count > 0)
            {
                #region found
                // shows member info
                MemberInfo mi = list[0];
                lblMoney.Text = mi.MMoney.ToString();
                lblTypeTitle.Text = mi.MTypeTitle;
                lblDiscount.Text = mi.MDiscount.ToString();
                lblPayMoneyDiscount.Text = (Convert.ToDecimal(lblPayMoney.Text) * Convert.ToDecimal(lblDiscount.Text)).ToString();
                #endregion
            }
            else
            {
                #region not found
                // pops up message
                MessageBox.Show("会员信息有误！");
                #endregion
            }
        }

        /// <summary>
        /// finds by id
        /// </summary>
        /// <param name="sender">txtId</param>
        /// <param name="e">data for executing event</param>
        private void txtId_Leave(object sender, EventArgs e)
        {
            LoadMember();
        }

        /// <summary>
        /// finds by phone
        /// </summary>
        /// <param name="sender">txtPhone</param>
        /// <param name="e">data for executing event</param>
        private void txtPhone_Leave(object sender, EventArgs e)
        {
            LoadMember();
        }

        /// <summary>
        /// pays for bill
        /// </summary>
        /// <param name="sender">btnOrderPay</param>
        /// <param name="e">data for executing event</param>
        private void btnOrderPay_Click(object sender, EventArgs e)
        {
            if (cbkMember.Checked)
            {
                #region is member
                if (cbkMoney.Checked)
                {
                    #region uses member balance
                    if (Convert.ToDecimal(lblMoney.Text) >= Convert.ToDecimal(lblPayMoneyDiscount.Text))
                    {
                        Pay(true);
                    }
                    else
                    {
                        MessageBox.Show("账户余额不足，无法结账！");
                    }
                    #endregion
                }
                else
                {
                    #region doesn't use member balance
                    Pay(true);
                    #endregion
                }
                #endregion
            }
            else
            {
                #region is not member
                Pay(false);
                #endregion
            }
        }

        /// <summary>
        /// pays for bill
        /// </summary>
        /// <param name="isMember">whether pays as member</param>
        private void Pay(bool isMember)
        {
            if (isMember)
            {
                #region pays as member
                if (oiBll.PayForBill("Cater", cbkMoney.Checked, Convert.ToDecimal(lblPayMoneyDiscount
                                        .Text), Convert.ToInt32(txtId.Text), orderId, Convert.ToDecimal(lblDiscount.Text)))
                {
                    // if succeeds, prompts OK and closes form
                    MessageBox.Show("结账完成");
                    Refresh();
                    this.Close();
                }
                else
                {
                    // otherwise prompts failure
                    MessageBox.Show("结账失败，请重试！");
                }
                #endregion
            }
            else
            {
                #region pays as not member
                if (oiBll.PayForBill("Cater", orderId))
                {
                    // if succeeds, prompts OK and closes form
                    MessageBox.Show("结账完成");
                    Refresh();
                    this.Close();
                }
                else
                {
                    // otherwise prompts failure
                    MessageBox.Show("结账失败，请重试！");
                }
                #endregion
            }
        }

        /// <summary>
        /// cancels payment
        /// </summary>
        /// <param name="sender">btnCancel</param>
        /// <param name="e">data for executing event</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // closes form
            this.Close();
        }
    }
}
