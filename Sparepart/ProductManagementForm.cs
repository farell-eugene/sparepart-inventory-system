using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Sparepart
{
    public partial class ProductManagementForm : Form
    {
        private DataGridView dataGridView1;
        private Button button1; // Tambah
        private Button button2; // Edit
        private Button button3; // Hapus

        public ProductManagementForm()
        {
            InitializeComponent();
            LoadProductData();

            // Add event handlers for buttons
            button1.Click += button1_Click;
            button2.Click += button2_Click;
            button3.Click += button3_Click;
        }

        private void LoadProductData()
        {
            try
            {
                // Clear existing columns and rows
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = null;

                // Set up columns
                dataGridView1.Columns.Add("ProductID", "ID");
                dataGridView1.Columns.Add("ProductCode", "Kode");
                dataGridView1.Columns.Add("ProductName", "Nama Produk");
                dataGridView1.Columns.Add("CategoryName", "Kategori");
                dataGridView1.Columns.Add("SellingPrice", "Harga Jual");
                dataGridView1.Columns.Add("StockQuantity", "Stok");
                dataGridView1.Columns.Add("SupplierName", "Supplier");

                // Get products from database
                List<Product> products = Product.GetAllProducts();

                // Add rows to DataGridView
                foreach (var product in products)
                {
                    dataGridView1.Rows.Add(
                        product.ProductID,
                        product.ProductCode,
                        product.ProductName,
                        product.CategoryName,
                        string.Format("Rp {0:N0}", product.SellingPrice),
                        product.StockQuantity,
                        product.SupplierName
                    );
                }

                // Format the DataGridView
                dataGridView1.Columns["ProductID"].Visible = false;
                dataGridView1.Columns["ProductCode"].Width = 80;
                dataGridView1.Columns["ProductName"].Width = 200;
                dataGridView1.Columns["CategoryName"].Width = 100;
                dataGridView1.Columns["SellingPrice"].Width = 100;
                dataGridView1.Columns["StockQuantity"].Width = 80;
                dataGridView1.Columns["SupplierName"].Width = 150;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading product data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Add the missing event handlers:
        
        private void button1_Click(object sender, EventArgs e)
        {
            // Open form to add a new product
            using (var productForm = new ProductForm())
            {
                if (productForm.ShowDialog() == DialogResult.OK)
                {
                    LoadProductData(); // Refresh the product list
                }
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            // Edit selected product
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int productId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ProductID"].Value);
                using (var productForm = new ProductForm(productId))
                {
                    if (productForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadProductData(); // Refresh the product list
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a product to edit", "Information", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            // Delete selected product
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int productId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ProductID"].Value);
                string productName = dataGridView1.SelectedRows[0].Cells["ProductName"].Value.ToString();
                
                if (MessageBox.Show($"Are you sure you want to delete product '{productName}'?", 
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        // Soft delete - set IsActive to 0
                        string query = "UPDATE Products SET IsActive = 0, UpdatedAt = GETDATE() WHERE ProductID = @ProductID";
                        System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[]
                        {
                            new System.Data.SqlClient.SqlParameter("@ProductID", productId)
                        };
                        
                        int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
                        if (result > 0)
                        {
                            MessageBox.Show("Product deleted successfully", "Success", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadProductData(); // Refresh the product list
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete product", "Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a product to delete", "Information", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Not implemented
        }
    }
}