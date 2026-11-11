using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sparepart
{
    public partial class TransactionForm : Form
    {
        private DataGridView dataGridView1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label labelTotal;
        private TextBox textBox1; // Nama Pelanggan
        private TextBox textBox2; // Notes
        private ComboBox comboBox1; // Produk
        private ComboBox comboBox2; // Payment Method
        private NumericUpDown numericUpDown1; // Jumlah
        private Button button1; // Tambah ke Keranjang
        private Button button2; // Proses Transaksi

        private List<TransactionDetail> cartItems = new List<TransactionDetail>();
        private decimal total = 0;

        public TransactionForm()
        {
            InitializeComponent();

            // Remove duplicate calls
            LoadProductData();
            SetupPaymentMethods();
            numericUpDown1.Minimum = 1;
            numericUpDown1.Value = 1;
            InitializePlaceholder();
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;

        }
        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Pastikan user klik baris yang valid
            if (e.RowIndex >= 0 && e.RowIndex < cartItems.Count)
            {
                // Konfirmasi hapus
                var result = MessageBox.Show("Hapus item ini dari keranjang?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Hapus item dari list cartItems sesuai baris yang di-double click
                    cartItems.RemoveAt(e.RowIndex);
                    UpdateCart();
                }
            }
        }

        private void InitializePlaceholder()
        {
            textBox2.Text = "Catatan Transaksi";
            textBox2.ForeColor = Color.Gray;

            textBox2.GotFocus += RemovePlaceholderText;
            textBox2.LostFocus += AddPlaceholderText;
        }

        private void RemovePlaceholderText(object sender, EventArgs e)
        {
            if (textBox2.Text == "Catatan Transaksi")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

        private void AddPlaceholderText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = "Catatan Transaksi";
                textBox2.ForeColor = Color.Gray;
            }
        }

        private void SetupPaymentMethods()
        {
            comboBox2.Items.AddRange(new string[] { "Cash", "Debit", "Credit" });
            comboBox2.SelectedIndex = 0;
        }

        private void LoadProductData()
        {
            try
            {
                string query = "SELECT ProductID, ProductCode, ProductName, SellingPrice, StockQuantity FROM Products WHERE IsActive = 1";
                DataTable dataTable = DatabaseHelper.ExecuteQuery(query);

                // Create a binding source
                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = dataTable;

                comboBox1.DataSource = bindingSource;
                comboBox1.DisplayMember = "ProductName";
                comboBox1.ValueMember = "ProductID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading product data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateCart()
        {
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = cartItems;
            dataGridView1.DataSource = bindingSource;

            total = cartItems.Sum(x => x.Subtotal);
            labelTotal.Text = total.ToString("C");

            // Optional: Set kolom agar tampil lengkap
            if (dataGridView1.Columns["TransactionDetailId"] != null)
                dataGridView1.Columns["TransactionDetailId"].HeaderText = "Detail ID";

            if (dataGridView1.Columns["TransactionId"] != null)
                dataGridView1.Columns["TransactionId"].HeaderText = "Transaction ID";
        }


        private int SaveTransaction(Transaction transaction)
        {
            int transactionId = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    connection.Open();
                    using (SqlTransaction sqlTransaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Dapatkan CustomerID dan PaymentMethodID (insert jika belum ada)
                            int customerId = GetCustomerId(transaction.CustomerName);
                            int paymentMethodId = GetPaymentMethodId(transaction.PaymentMethod);

                            // Generate nomor invoice
                            string invoiceNumber = GenerateInvoiceNumber();

                            // Insert ke tabel Transactions
                            string transQuery = @"
                        INSERT INTO Transactions 
                            (TransactionDate, CustomerID, PaymentMethodID, TotalAmount, PaidAmount, Notes, InvoiceNumber) 
                        VALUES 
                            (@Date, @CustomerID, @PaymentMethodID, @Total, @PaidAmount, @Notes, @InvoiceNumber);
                        SELECT SCOPE_IDENTITY();";

                            using (SqlCommand cmd = new SqlCommand(transQuery, connection, sqlTransaction))
                            {
                                cmd.Parameters.AddWithValue("@Date", transaction.TransactionDate);
                                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                                cmd.Parameters.AddWithValue("@PaymentMethodID", paymentMethodId);
                                cmd.Parameters.AddWithValue("@Total", transaction.TotalAmount);
                                cmd.Parameters.AddWithValue("@PaidAmount", transaction.TotalAmount); // Asumsi lunas
                                cmd.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(transaction.Notes) ? (object)DBNull.Value : transaction.Notes);
                                cmd.Parameters.AddWithValue("@InvoiceNumber", invoiceNumber);

                                transactionId = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            // Insert detail transaksi dan update stok
                            foreach (var detail in transaction.Details)
                            {
                                string detailQuery = @"
                            INSERT INTO TransactionDetails (TransactionID, ProductID, Quantity, UnitPrice, Subtotal)
                            VALUES (@TransactionID, @ProductID, @Quantity, @UnitPrice, @Subtotal);
                            SELECT SCOPE_IDENTITY();";

                                using (SqlCommand detailCmd = new SqlCommand(detailQuery, connection, sqlTransaction))
                                {
                                    detailCmd.Parameters.AddWithValue("@TransactionID", transactionId);
                                    detailCmd.Parameters.AddWithValue("@ProductID", detail.ProductID);
                                    detailCmd.Parameters.AddWithValue("@Quantity", detail.Quantity);
                                    detailCmd.Parameters.AddWithValue("@UnitPrice", detail.UnitPrice);
                                    detailCmd.Parameters.AddWithValue("@Subtotal", detail.Subtotal);

                                    // Simpan TransactionDetailId ke objek
                                    detail.TransactionDetailID = Convert.ToInt32(detailCmd.ExecuteScalar());
                                    detail.TransactionID = transactionId;
                                }

                                // Update stok produk
                                string updateStockQuery = @"
                            UPDATE Products 
                            SET StockQuantity = StockQuantity - @Quantity 
                            WHERE ProductID = @ProductID AND StockQuantity >= @Quantity";

                                using (SqlCommand updateStockCmd = new SqlCommand(updateStockQuery, connection, sqlTransaction))
                                {
                                    updateStockCmd.Parameters.AddWithValue("@Quantity", detail.Quantity);
                                    updateStockCmd.Parameters.AddWithValue("@ProductID", detail.ProductID);

                                    int rowsAffected = updateStockCmd.ExecuteNonQuery();
                                    if (rowsAffected == 0)
                                    {
                                        throw new Exception($"Stok produk dengan ID {detail.ProductID} tidak mencukupi.");
                                    }
                                }
                            }

                            // Commit transaksi jika semua sukses
                            sqlTransaction.Commit();
                        }
                        catch
                        {
                            sqlTransaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save transaction: " + ex.Message);
            }
            return transactionId;
        }




        private void ClearForm()
        {
            textBox1.Clear();
            textBox2.Clear();
            numericUpDown1.Value = 1;
            cartItems.Clear();
            UpdateCart();
            InitializePlaceholder();
        }


        private int GetCustomerId(string customerName)
        {
            int customerId = 0;
            string query = "SELECT CustomerID FROM Customers WHERE CustomerName = @Name";

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Name", customerName);
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null)
                    customerId = Convert.ToInt32(result);
                else
                {
                    // Insert customer baru jika tidak ditemukan, atau berikan error
                    // Contoh insert customer baru:
                    string insertQuery = "INSERT INTO Customers (CustomerName) VALUES (@Name); SELECT SCOPE_IDENTITY();";
                    cmd.CommandText = insertQuery;
                    customerId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return customerId;
        }

        private int GetPaymentMethodId(string paymentMethodName)
        {
            int paymentMethodId = 0;
            string query = "SELECT PaymentMethodID FROM PaymentMethods WHERE MethodName = @Name";

            using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Name", paymentMethodName);
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null)
                    paymentMethodId = Convert.ToInt32(result);
                else
                {
                    // Insert payment method baru atau beri error
                    string insertQuery = "INSERT INTO PaymentMethods (MethodName) VALUES (@Name); SELECT SCOPE_IDENTITY();";
                    cmd.CommandText = insertQuery;
                    paymentMethodId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return paymentMethodId;
        }

        private string GenerateInvoiceNumber()
        {
            string prefix = "INV";
            string datePart = DateTime.Now.ToString("yyyyMMdd");

            int nextNumber = 1;

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT 
                    MAX(CAST(RIGHT(InvoiceNumber, 4) AS INT)) 
                FROM Transactions 
                WHERE InvoiceNumber LIKE @InvoicePattern";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@InvoicePattern", prefix + "-" + datePart + "-%");

                    var result = cmd.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                    {
                        nextNumber = Convert.ToInt32(result) + 1;
                    }
                }
            }
            catch
            {
                // Kalau gagal, biar tetap jalan dengan nomor 1
                nextNumber = 1;
            }

            // Format nomor dengan leading zero 4 digit, misal 0001
            string numberPart = nextNumber.ToString("D4");

            return $"{prefix}-{datePart}-{numberPart}";
        }


        private void TransactionForm_Load(object sender, EventArgs e)
        {
            // Remove these lines as we're handling data loading in LoadProductData and SetupPaymentMethods
            // this.paymentMethodsTableAdapter.Fill(this.sparepartDBDataSet.PaymentMethods);
            // this.productsTableAdapter.Fill(this.sparepartDBDataSet.Products);
            // this.suppliersTableAdapter.Fill(this.sparepartDBDataSet.Suppliers);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is DataRowView selectedRow)
            {
                int stockQuantity = Convert.ToInt32(selectedRow["StockQuantity"]);
                decimal price = Convert.ToDecimal(selectedRow["SellingPrice"]);
                int productId = Convert.ToInt32(selectedRow["ProductID"]);

                if (numericUpDown1.Value <= 0)
                {
                    MessageBox.Show("Jumlah harus lebih dari 0", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (numericUpDown1.Value > stockQuantity)
                {
                    MessageBox.Show("Stok tidak mencukupi", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var detail = new TransactionDetail
                {
                    ProductID = productId,
                    Quantity = (int)numericUpDown1.Value,
                    UnitPrice = price,
                    Subtotal = price * (int)numericUpDown1.Value
                };

                cartItems.Add(detail);
                UpdateCart();
            }
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            if (cartItems.Count <= 0)
            {
                MessageBox.Show("Keranjang belanja kosong", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Nama pelanggan harus diisi", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var transaction = new Transaction
            {
                TransactionDate = DateTime.Now,
                CustomerName = textBox1.Text,
                PaymentMethod = comboBox2.SelectedItem.ToString(),
                Notes = textBox2.Text == "Catatan Transaksi" ? "" : textBox2.Text,
                TotalAmount = total,
                Details = new List<TransactionDetail>(cartItems)
            };

            try
            {
                // Simpan transaksi dan dapatkan transactionId
                int transactionId = SaveTransaction(transaction);

                // Update TransactionId dan TransactionDetailId ke objek cartItems agar bisa ditampilkan
                foreach (var detail in cartItems)
                {
                    detail.TransactionID = transactionId;
                    // TransactionDetailId sudah diassign di SaveTransaction
                }

                UpdateCart();  // Refresh DataGridView untuk tampilkan ID

                MessageBox.Show("Transaksi Berhasil", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal menyimpan transaksi: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
