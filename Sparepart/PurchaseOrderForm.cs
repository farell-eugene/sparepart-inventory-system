using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sparepart
{
    public partial class PurchaseOrderForm : Form
    {
        public PurchaseOrderForm()
        {
            InitializeComponent();
        }

        private void PurchaseOrderForm_Load(object sender, EventArgs e)
        {
            // TODO: Load data dari database ke comboBox Supplier
            this.suppliersTableAdapter.Fill(this.sparepartDBDataSet.Suppliers);

            // Contoh isi comboBox2 (Status)
            comboBox2.Items.Clear();
            comboBox2.Items.Add("Pending");
            comboBox2.Items.Add("Approved");
            comboBox2.Items.Add("Rejected");
            comboBox2.SelectedIndex = 0;

            // Inisialisasi DataGridView, misalnya kolom untuk Item, Qty, Harga
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("ItemID", "Item ID");
            dataGridView1.Columns.Add("ItemName", "Item Name");
            dataGridView1.Columns.Add("Quantity", "Quantity");
            dataGridView1.Columns.Add("Price", "Price");
            dataGridView1.Columns.Add("Total", "Total");

            dataGridView1.Columns["ItemID"].ReadOnly = true;
            dataGridView1.Columns["ItemName"].ReadOnly = true;
            dataGridView1.Columns["Total"].ReadOnly = true;

            UpdateTotalLabel();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var productSelectionForm = new ProductSelectionForm())
            {
                var result = productSelectionForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    var detail = productSelectionForm.SelectedDetail;
                    if (detail != null)
                    {
                        // Ambil nama produk dari database berdasarkan ProductID
                        string productName = GetProductNameById(detail.ProductID);

                        int newRowIndex = dataGridView1.Rows.Add();
                        var row = dataGridView1.Rows[newRowIndex];

                        row.Cells["ItemID"].Value = detail.ProductID;
                        row.Cells["ItemName"].Value = productName;
                        row.Cells["Quantity"].Value = detail.Quantity;
                        row.Cells["Price"].Value = detail.UnitPrice;
                        row.Cells["Total"].Value = detail.Subtotal;

                        UpdateTotalLabel();
                    }
                }
            }
        }

        // Method untuk ambil nama produk dari ProductID
        private string GetProductNameById(int productId)
        {
            string query = $"SELECT ProductName FROM Products WHERE ProductID = {productId}";
            var dt = DatabaseHelper.ExecuteQuery(query);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["ProductName"].ToString();
            }
            return "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Hapus baris yang dipilih di DataGridView
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (!row.IsNewRow)
                {
                    dataGridView1.Rows.Remove(row);
                }
            }
            UpdateTotalLabel();
        }
        private string GeneratePONumber(SqlConnection conn, SqlTransaction transaction)
        {
            // Contoh: Buat nomor PO dengan format "POyyyyMMddNNN"
            // NNN = angka urut berdasarkan tanggal hari ini

            string datePart = DateTime.Now.ToString("yyyyMMdd");
            string prefix = "PO" + datePart;

            // Query untuk cek nomor PO terbesar hari ini yang sudah ada
            string query = @"
        SELECT MAX(PONumber) 
        FROM PurchaseOrders 
        WHERE PONumber LIKE @Prefix + '%'";

            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@Prefix", prefix);

                object result = cmd.ExecuteScalar();

                if (result == DBNull.Value || result == null)
                {
                    // Belum ada PO untuk hari ini, mulai dari 001
                    return prefix + "001";
                }
                else
                {
                    string lastPONumber = (string)result;
                    // Ambil 3 digit terakhir untuk increment
                    string lastNumberStr = lastPONumber.Substring(lastPONumber.Length - 3);
                    if (int.TryParse(lastNumberStr, out int lastNumber))
                    {
                        int nextNumber = lastNumber + 1;
                        return prefix + nextNumber.ToString("D3"); // format 3 digit dengan leading zero
                    }
                    else
                    {
                        // Kalau gagal parse, mulai dari 001
                        return prefix + "001";
                    }
                }
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();

                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Panggil method GeneratePONumber untuk buat nomor PO unik
                            string poNumber = GeneratePONumber(conn, transaction);

                            // Hitung total amount dari detail di DataGridView
                            decimal totalAmount = 0;
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                if (row.IsNewRow) continue;
                                if (row.Cells["Total"].Value != null)
                                {
                                    totalAmount += Convert.ToDecimal(row.Cells["Total"].Value);
                                }
                            }

                            // Insert header PO dengan PONumber dan PurchaseOrderNumber yang sudah di-generate
                            string insertHeader = @"INSERT INTO PurchaseOrders 
                                    (PONumber, SupplierID, Status, Notes, CreatedDate, TotalAmount, PurchaseOrderNumber) 
                                    VALUES 
                                    (@PONumber, @SupplierID, @Status, @Notes, @CreatedDate, @TotalAmount, @PurchaseOrderNumber); 
                                    SELECT SCOPE_IDENTITY();";

                            SqlCommand cmdHeader = new SqlCommand(insertHeader, conn, transaction);
                            cmdHeader.Parameters.AddWithValue("@PONumber", poNumber);
                            cmdHeader.Parameters.AddWithValue("@SupplierID", (int)comboBox1.SelectedValue);
                            cmdHeader.Parameters.AddWithValue("@Status", comboBox2.SelectedItem.ToString());
                            cmdHeader.Parameters.AddWithValue("@Notes", textBox1.Text);
                            cmdHeader.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                            cmdHeader.Parameters.AddWithValue("@TotalAmount", totalAmount);
                            cmdHeader.Parameters.AddWithValue("@PurchaseOrderNumber", poNumber); // Tambahkan parameter untuk PurchaseOrderNumber

                            int poID = Convert.ToInt32(cmdHeader.ExecuteScalar());

                            // Insert detail PO
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                if (row.IsNewRow) continue;

                                string insertDetail = @"INSERT INTO PurchaseOrderDetails 
                                        (PurchaseOrderID, ProductID, Quantity, UnitPrice, Subtotal) 
                                        VALUES 
                                        (@POID, @ProductID, @Quantity, @UnitPrice, @Subtotal)";

                                SqlCommand cmdDetail = new SqlCommand(insertDetail, conn, transaction);
                                cmdDetail.Parameters.AddWithValue("@POID", poID);
                                cmdDetail.Parameters.AddWithValue("@ProductID", Convert.ToInt32(row.Cells["ItemID"].Value));
                                cmdDetail.Parameters.AddWithValue("@Quantity", Convert.ToInt32(row.Cells["Quantity"].Value));
                                cmdDetail.Parameters.AddWithValue("@UnitPrice", Convert.ToDecimal(row.Cells["Price"].Value));
                                cmdDetail.Parameters.AddWithValue("@Subtotal", Convert.ToDecimal(row.Cells["Total"].Value));

                                cmdDetail.ExecuteNonQuery();
                            }

                            transaction.Commit();

                            MessageBox.Show("Data Purchase Order berhasil disimpan.");

                            // Clear form
                            comboBox1.SelectedIndex = -1;
                            comboBox2.SelectedIndex = 0;
                            textBox1.Clear();
                            dataGridView1.Rows.Clear();
                            label4.Text = "Total: 0";
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("Terjadi kesalahan saat menyimpan data: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan koneksi: " + ex.Message);
            }
        }

        private void UpdateTotalLabel()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Total"].Value != null)
                {
                    decimal rowTotal;
                    if (decimal.TryParse(row.Cells["Total"].Value.ToString(), out rowTotal))
                    {
                        total += rowTotal;
                    }
                }
            }
            label4.Text = $"Total: {total:N0}";
        }
    }
}
