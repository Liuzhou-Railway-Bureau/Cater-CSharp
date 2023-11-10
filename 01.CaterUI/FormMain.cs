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
    public partial class FormMain : Form
    {
        private OrderInfoBll oiBll = new OrderInfoBll();// creates business logic layer object
        private int orderId = -1;

        public FormMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// exits program
        /// </summary>
        /// <param name="sender">FormMain</param>
        /// <param name="e">data for executing event</param>
        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();// exits instead of just closing
        }

        /// <summary>
        /// exits program
        /// </summary>
        /// <param name="sender">menuQuit</param>
        /// <param name="e">data for executing event</param>
        private void menuQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();// exits instead of just closing
        }

        /// <summary>
        /// manages privilege
        /// </summary>
        /// <param name="sender">FormMain</param>
        /// <param name="e">data for executing event</param>
        private void FormMain_Load(object sender, EventArgs e)
        {
            int type = Convert.ToInt32(this.Tag);

            if (type == 0)
            {
                menuManagerInfo.Visible = false;// doesn't display manager for clerk
            }

            // loads all halls
            LoadHallInfo();
        }

        /// <summary>
        /// loads hall info
        /// </summary>
        private void LoadHallInfo()
        {
            // clears tab pages
            tcHallInfo.TabPages.Clear();
            // gets all halls
            HallInfoBll hiBll = new HallInfoBll();
            TableInfoBll tiBll = new TableInfoBll();
            List<HallInfo> hallList = hiBll.GetList("Cater");
            // adds info to tab pages
            foreach (HallInfo hi in hallList)
            {
                // creates tab page according to hall
                TabPage tp = new TabPage(hi.HTitle);
                // gets all tables
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("THallId", hi.HId.ToString());
                List<TableInfo> tableList = tiBll.GetList("Cater", dic);
                // dynamically creates list view and adds it to tab page
                ListView lvTableInfo = new ListView();
                lvTableInfo.DoubleClick += lvTableInfo_DoublClick;// registers double click event
                lvTableInfo.LargeImageList = imageList1;// List view uses imageList1
                lvTableInfo.Dock = DockStyle.Fill;
                tp.Controls.Add(lvTableInfo);
                foreach (TableInfo ti in tableList)
                {
                    ListViewItem lvi = new ListViewItem(ti.TTitle, ti.TIsFree ? 0 : 1);
                    lvi.Tag = ti.TId;
                    lvTableInfo.Items.Add(lvi);
                }
                // adds tab page to container
                tcHallInfo.TabPages.Add(tp);
            }
        }

        /// <summary>
        /// places order
        /// </summary>
        /// <param name="sender">lvTableInfo</param>
        /// <param name="e">data for executing event</param>
        private void lvTableInfo_DoublClick(object sender, EventArgs e)
        {
            ListView lv = sender as ListView;
            ListViewItem lvi = lv.SelectedItems[0];
            int tableId = Convert.ToInt32(lvi.Tag);

            if (lvi.ImageIndex == 0)
            {
                #region If free, places order
                // places order
                orderId = oiBll.PlaceOrder("Cater", tableId);
                // refreshes icon
                lv.SelectedItems[0].ImageIndex = 1;
                #endregion
            }
            else
            {
                #region If in use, gets table's order id
                orderId = oiBll.GetOrderId("Cater", tableId);
                #endregion
            }

            FormOrderDish fOrder = FormOrderDish.Create();
            fOrder.Tag = orderId;
            fOrder.ShowDialog();
        }

        /// <summary>
        /// shows manager info form
        /// </summary>
        /// <param name="sender">menuManagerInfo</param>
        /// <param name="e">data for executing event</param>
        private void menuManagerInfo_Click(object sender, EventArgs e)
        {
            FormManagerInfo fManager = FormManagerInfo.Create();
            fManager.Show();
            fManager.Focus();
        }

        /// <summary>
        /// shows member info form
        /// </summary>
        /// <param name="sender">menuMemberInfo</param>
        /// <param name="e">data for executing event</param>
        private void menuMemberInfo_Click(object sender, EventArgs e)
        {
            FormMemberInfo fMember = FormMemberInfo.Create();
            fMember.Show();
            fMember.Focus();
        }

        /// <summary>
        /// shows dish info form
        /// </summary>
        /// <param name="sender">menuDishInfo</param>
        /// <param name="e">data for executing event</param>
        private void menuDishInfo_Click(object sender, EventArgs e)
        {
            FormDishInfo fDish = FormDishInfo.Create();
            fDish.Show();
            fDish.Focus();
        }

        /// <summary>
        /// shows table info form
        /// </summary>
        /// <param name="sender">menuTableInfo</param>
        /// <param name="e">data for executing event</param>
        private void menuTableInfo_Click(object sender, EventArgs e)
        {
            FormTableInfo fTable = FormTableInfo.Create();
            fTable.TableUpdated += LoadHallInfo;
            fTable.Show();
            fTable.Focus();
        }

        /// <summary>
        /// shows order pay form
        /// </summary>
        /// <param name="sender">menuOrder</param>
        /// <param name="e">data for executing event</param>
        private void menuOrder_Click(object sender, EventArgs e)
        {
            ListView lv = tcHallInfo.SelectedTab.Controls[0] as ListView;
            ListViewItem lvTable = lv.SelectedItems[0];
            if (lvTable.ImageIndex == 1)
            {
                int tableId = Convert.ToInt32(lvTable.Tag);
                orderId = oiBll.GetOrderId("Cater", tableId);
                FormOrderPay fPay = FormOrderPay.Create();
                fPay.Tag = orderId;
                fPay.Refresh += LoadHallInfo;
                fPay.ShowDialog();
                fPay.Focus();
            }
            else
            {
                MessageBox.Show("餐桌未被使用，无法结账！");
            }
        }
    }
}
