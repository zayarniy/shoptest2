using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopVik
{

    
    public partial class Form4 : Form
    {
        DataTable cart = new DataTable();

        BindingSource bsCart=new BindingSource();
        public Form4()
        {
            InitializeComponent();
            cart.Columns.Add("Name");
            cart.Columns.Add("Count");
            bsCart.DataSource = cart;
            dgvCart.DataSource = bsCart;
            bnCart.BindingSource = bsCart;
           // cart.RowChanged += Cart_RowChanged;
           // bsCart.CurrentItemChanged += BsCart_CurrentItemChanged;
        }

        private void BsCart_CurrentItemChanged(object sender, EventArgs e)
        {
        }


        private void productsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.productsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.shopDataSet);

        }

        private void Form4_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "shopDataSet.Products". При необходимости она может быть перемещена или удалена.
            this.productsTableAdapter.Fill(this.shopDataSet.Products);

        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            string[] item = new string[2];
            item[0] = cbProducts.Text;
            item[1] = cbCount.Text;
            cart.Rows.Add(item);
            int s = 0;

            foreach (DataRow row in cart.Rows)
                if (row != null)
                    s = s + Convert.ToInt32(row.ItemArray[1]);
            lblSum.Text = s.ToString();

        }
    }
}
