using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sparepart
{
    public partial class ProductViewerForm : Form
    {
        private DataGridView dataGridViewProducts;

        public ProductViewerForm()
        {
            InitializeComponent();
            InitializeControls();
            LoadProductData();
        }

        private void InitializeControls()
        {
            this.Text = "Lihat Daftar Produk";
            this.Size = new System.Drawing.Size(800, 500);

            dataGridViewProducts = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 400,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.Add(dataGridViewProducts);
            
        }

        private void LoadProductData()
        {
            try
            {
                string query = @"
            SELECT 
                ProductID AS [ID Produk],
                ProductName AS [Nama Produk],
                StockQuantity AS [Stok],
                SellingPrice AS [Harga],
                CASE 
                    WHEN StockQuantity = 1 THEN 'Aktif'
                    WHEN IsActive = 1 THEN 'Aktif'
                    ELSE 'Nonaktif'
                END AS [Status]
            FROM Products;";

                // Eksekusi query dan tampilkan ke DataGridView
                DataTable dt = DatabaseHelper.ExecuteQuery(query);
                dataGridViewProducts.DataSource = dt;

                // Opsional: Otomatis atur ukuran kolom agar muat
                dataGridViewProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data produk: " + ex.Message, "Kesalahan",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ProductViewerForm_Load(object sender, EventArgs e)
        {

        }

    

        private void dataGridViewProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
