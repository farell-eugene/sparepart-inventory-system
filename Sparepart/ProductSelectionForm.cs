using System;
using System.Data;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Sparepart
{
    public partial class ProductSelectionForm : Form
    {
        private System.Windows.Forms.TextBox textBoxQty;
        private System.Windows.Forms.TextBox textBoxPrice;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button buttonOK;

        public PurchaseOrderDetail SelectedDetail { get; private set; }

        public ProductSelectionForm()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            string query = "SELECT ProductID, ProductName, SellingPrice FROM Products WHERE IsActive = 1";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "ProductName";
            comboBox1.ValueMember = "ProductID";
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null &&
                int.TryParse(textBoxQty.Text, out int qty) &&
                decimal.TryParse(textBoxPrice.Text, out decimal price))
            {
                DataRowView selectedRow = (DataRowView)comboBox1.SelectedItem;

                SelectedDetail = new PurchaseOrderDetail
                {
                    ProductID = Convert.ToInt32(selectedRow["ProductID"]),
                    Quantity = qty,
                    UnitPrice = price,
                    Subtotal = qty * price
                };

                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Harap isi data dengan benar.");
            }
        }

        private void ProductSelectionForm_Load(object sender, EventArgs e)
        {

        }

        private void ProductSelectionForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}