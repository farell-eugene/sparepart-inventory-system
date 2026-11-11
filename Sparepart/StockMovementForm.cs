using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Linq;

namespace Sparepart
{
    public partial class StockMovementForm : Form
    {
        private int selectedPurchaseOrderID = 0;
        private int currentUserId = 1; // This should come from your login system

        public StockMovementForm()
        {
            InitializeComponent();
            InitCustomSetup();
        }

        private void InitCustomSetup()
        {
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(new string[] { "Semua", "In", "Out", "Purchase Order" });
            comboBox2.SelectedIndex = 0;

            LoadProducts();
            LoadMovementData();
        }

        private void LoadProducts()
        {
            try
            {
                string query = "SELECT ProductID, ProductName FROM Products WHERE IsActive = 1";
                DataTable productTable = DatabaseHelper.ExecuteQuery(query);

                DataRow newRow = productTable.NewRow();
                newRow["ProductID"] = 0;
                newRow["ProductName"] = "Semua Produk";
                productTable.Rows.InsertAt(newRow, 0);

                comboBox1.DataSource = productTable;
                comboBox1.DisplayMember = "ProductName";
                comboBox1.ValueMember = "ProductID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
            }
        }

        private DataGridView GetDataGridView1()
        {
            return dataGridView1;
        }

        private void LoadMovementData()
        {
            try
            {
                string query = @"
            SELECT 
                sm.MovementID as ID,
                sm.MovementDate as Tanggal,
                p.ProductName as Produk,
                sm.ReferenceType as Tipe,
                sm.Quantity as Jumlah,
                sm.ReferenceID as Referensi,
                u.Username as [User],
                sm.Notes as Catatan,
                CASE 
                    WHEN sm.ReferenceType = 'In' THEN '+' + CAST(sm.Quantity AS VARCHAR)
                    WHEN sm.ReferenceType = 'Out' THEN '-' + CAST(sm.Quantity AS VARCHAR)
                    ELSE CAST(sm.Quantity AS VARCHAR)
                END as [+/-]
            FROM StockMovements sm
            INNER JOIN Products p ON sm.ProductID = p.ProductID
            INNER JOIN Users u ON sm.UserID = u.UserID
            WHERE 1 = 1";

                List<SqlParameter> parameters = new List<SqlParameter>();

                // Filter tanggal
                if (dateTimePicker1.Value != null)
                {
                    query += " AND sm.MovementDate >= @StartDate";
                    parameters.Add(new SqlParameter("@StartDate", dateTimePicker1.Value.Date));
                }

                if (dateTimePicker2.Value != null)
                {
                    query += " AND sm.MovementDate <= @EndDate";
                    parameters.Add(new SqlParameter("@EndDate", dateTimePicker2.Value.Date.AddDays(1).AddSeconds(-1)));
                }

                // Filter produk
                if (comboBox1.SelectedValue != null
                    && int.TryParse(comboBox1.SelectedValue.ToString(), out int productId)
                    && productId != 0)
                {
                    query += " AND sm.ProductID = @ProductID";
                    parameters.Add(new SqlParameter("@ProductID", productId));
                }

                // Filter tipe movement
                if (comboBox2.SelectedIndex > 0)
                {
                    string movementType = comboBox2.SelectedItem.ToString();
                    if (movementType == "Purchase Order")
                    {
                        query += " AND sm.ReferenceType = 'Purchase Order'";
                    }
                    else if (movementType != "Semua")
                    {
                        query += " AND sm.ReferenceType = @MovementType";
                        parameters.Add(new SqlParameter("@MovementType", movementType));
                    }
                }

                query += " ORDER BY sm.MovementDate DESC";

                // Eksekusi query dan set DataSource
                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());

                // Periksa apakah data ditemukan
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Data Stock Movement tidak ditemukan untuk filter yang dipilih",
                                  "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Set DataSource dan format tampilan
                dataGridView1.DataSource = null; // Clear existing data source
                dataGridView1.DataSource = dt;

                // Format tampilan grid dengan pengecekan null yang lebih aman
                if (dataGridView1?.Columns != null && dataGridView1.Columns.Count > 0)
                {
                    try
                    {
                        // Simpan pengaturan AutoSizeMode saat ini
                        var currentMode = dataGridView1.AutoSizeColumnsMode;
                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                        // Atur lebar kolom
                        foreach (DataGridViewColumn col in dataGridView1.Columns)
                        {
                            if (col == null) continue;

                            switch (col.Name)
                            {
                                case "ID":
                                    col.Width = 50;
                                    break;
                                case "Tanggal":
                                    col.Width = 90;
                                    break;
                                case "Produk":
                                    col.Width = 200;
                                    break;
                                case "Tipe":
                                    col.Width = 100;
                                    break;
                                case "Jumlah":
                                    col.Width = 80;
                                    break;
                                case "Referensi":
                                    col.Width = 150;
                                    break;
                                case "User":
                                    col.Width = 100;
                                    break;
                                case "+/-":
                                    col.Width = 60;
                                    break;
                            }
                        }

                        // Kembalikan pengaturan AutoSizeMode
                        dataGridView1.AutoSizeColumnsMode = currentMode;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saat mengatur lebar kolom: {ex.Message}",
                                      "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saat memuat data: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGetFromPO_Click(object sender, EventArgs e)
        {
            using (var form = new Form())
            {
                form.Text = "Pilih Purchase Order";
                form.Size = new System.Drawing.Size(900, 500);
                form.StartPosition = FormStartPosition.CenterParent;

                var dgvPurchaseOrders = new DataGridView();
                dgvPurchaseOrders.Dock = DockStyle.Fill;
                dgvPurchaseOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvPurchaseOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvPurchaseOrders.MultiSelect = false;
                dgvPurchaseOrders.AllowUserToAddRows = false;
                dgvPurchaseOrders.AllowUserToDeleteRows = false;
                dgvPurchaseOrders.ReadOnly = true;

                string query = @"
                    SELECT 
                        po.PurchaseOrderID, 
                        po.PurchaseOrderNumber,
                        po.PONumber, 
                        po.OrderDate, 
                        s.SupplierName, 
                        po.TotalAmount, 
                        po.Status,
                        COUNT(pod.PurchaseOrderDetailID) as [Total Items]
                    FROM PurchaseOrders po
                    INNER JOIN Suppliers s ON po.SupplierID = s.SupplierID
                    LEFT JOIN PurchaseOrderDetails pod ON po.PurchaseOrderID = pod.PurchaseOrderID
                    WHERE po.Status IN ('Approved', 'Received')
                    GROUP BY po.PurchaseOrderID, po.PurchaseOrderNumber, po.PONumber, 
                             po.OrderDate, s.SupplierName, po.TotalAmount, po.Status
                    ORDER BY po.OrderDate DESC";

                DataTable dt = DatabaseHelper.ExecuteQuery(query);
                dgvPurchaseOrders.DataSource = dt;

                var btnSelect = new Button();
                btnSelect.Text = "Pilih PO";
                btnSelect.Dock = DockStyle.Bottom;
                btnSelect.Height = 40;
                btnSelect.BackColor = System.Drawing.Color.DodgerBlue;
                btnSelect.ForeColor = System.Drawing.Color.White;
                btnSelect.Click += (s, args) =>
                {
                    if (dgvPurchaseOrders.SelectedRows.Count > 0)
                    {
                        selectedPurchaseOrderID = Convert.ToInt32(dgvPurchaseOrders.SelectedRows[0].Cells["PurchaseOrderID"].Value);
                        form.DialogResult = DialogResult.OK;
                        form.Close();
                    }
                    else
                    {
                        MessageBox.Show("Silakan pilih Purchase Order terlebih dahulu", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                form.Controls.Add(dgvPurchaseOrders);
                form.Controls.Add(btnSelect);

                if (form.ShowDialog() == DialogResult.OK && selectedPurchaseOrderID > 0)
                {
                    LoadPurchaseOrderDetails(selectedPurchaseOrderID);
                }
            }
        }

        private void LoadPurchaseOrderDetails(int purchaseOrderID)
        {
            try
            {
                string query = @"
                    SELECT 
                        pod.PurchaseOrderDetailID,
                        pod.ProductID,
                        COALESCE(p.ProductName, 'Product Not Found') as ProductName,
                        COALESCE(p.ProductCode, '') as ProductCode,
                        pod.Quantity,
                        pod.UnitPrice,
                        pod.Subtotal,
                        CASE WHEN p.ProductID IS NULL THEN 0 ELSE 1 END AS ProductExists,
                        CASE WHEN p.ProductID IS NULL THEN 'Tidak Ada' ELSE 'Ada' END AS [Status Produk]
                    FROM PurchaseOrderDetails pod
                    LEFT JOIN Products p ON pod.ProductID = p.ProductID
                    WHERE pod.PurchaseOrderID = @PurchaseOrderID
                    ORDER BY pod.PurchaseOrderDetailID";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@PurchaseOrderID", purchaseOrderID)
                };

                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

                // Clear existing columns first
                dgvPurchaseOrderDetails.Columns.Clear();

                // Add checkbox column for selection
                DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
                checkColumn.Name = "Select";
                checkColumn.HeaderText = "Pilih";
                checkColumn.Width = 50;
                dgvPurchaseOrderDetails.Columns.Add(checkColumn);

                dgvPurchaseOrderDetails.DataSource = dt;
                dgvPurchaseOrderDetails.Visible = true;

                // Show related controls
                chkSelectAll.Visible = true;
                btnAddToProducts.Visible = true;
                btnProcessStockIn.Visible = true;

                // Enable buttons
                btnAddToProducts.Enabled = dt.Rows.Count > 0;
                btnProcessStockIn.Enabled = dt.Rows.Count > 0;

                // Format kolom - dengan pengecekan yang lebih aman
                if (dgvPurchaseOrderDetails.Columns.Count > 0)
                {
                    // Hide unnecessary columns
                    if (dgvPurchaseOrderDetails.Columns.Contains("PurchaseOrderDetailID"))
                        dgvPurchaseOrderDetails.Columns["PurchaseOrderDetailID"].Visible = false;
                    if (dgvPurchaseOrderDetails.Columns.Contains("ProductID"))
                        dgvPurchaseOrderDetails.Columns["ProductID"].Visible = false;
                    if (dgvPurchaseOrderDetails.Columns.Contains("ProductExists"))
                        dgvPurchaseOrderDetails.Columns["ProductExists"].Visible = false;

                    // Set column headers yang lebih user-friendly
                    if (dgvPurchaseOrderDetails.Columns.Contains("ProductName"))
                    {
                        dgvPurchaseOrderDetails.Columns["ProductName"].HeaderText = "Nama Produk";
                        dgvPurchaseOrderDetails.Columns["ProductName"].Width = 150;
                    }
                    if (dgvPurchaseOrderDetails.Columns.Contains("ProductCode"))
                    {
                        dgvPurchaseOrderDetails.Columns["ProductCode"].HeaderText = "Kode Produk";
                        dgvPurchaseOrderDetails.Columns["ProductCode"].Width = 100;
                    }
                    if (dgvPurchaseOrderDetails.Columns.Contains("Quantity"))
                    {
                        dgvPurchaseOrderDetails.Columns["Quantity"].HeaderText = "Jumlah";
                        dgvPurchaseOrderDetails.Columns["Quantity"].Width = 80;
                    }
                    if (dgvPurchaseOrderDetails.Columns.Contains("UnitPrice"))
                    {
                        dgvPurchaseOrderDetails.Columns["UnitPrice"].HeaderText = "Harga Satuan";
                        dgvPurchaseOrderDetails.Columns["UnitPrice"].Width = 100;
                    }
                    if (dgvPurchaseOrderDetails.Columns.Contains("Subtotal"))
                    {
                        dgvPurchaseOrderDetails.Columns["Subtotal"].HeaderText = "Total";
                        dgvPurchaseOrderDetails.Columns["Subtotal"].Width = 100;
                    }
                    if (dgvPurchaseOrderDetails.Columns.Contains("Status Produk"))
                    {
                        dgvPurchaseOrderDetails.Columns["Status Produk"].Width = 100;
                    }
                }

                MessageBox.Show($"Loaded {dt.Rows.Count} items from Purchase Order", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading purchase order details: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkSelectAll = sender as CheckBox;
            if (dgvPurchaseOrderDetails.Visible && dgvPurchaseOrderDetails.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvPurchaseOrderDetails.Rows)
                {
                    if (!row.IsNewRow && row.Cells["Select"] != null)
                    {
                        row.Cells["Select"].Value = chkSelectAll.Checked;
                    }
                }
            }
        }
        private void btnAddToProducts_Click(object sender, EventArgs e)
        {
            // Check if any items are selected
            var selectedRows = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dgvPurchaseOrderDetails.Rows)
            {
                if (!row.IsNewRow && row.Cells["Select"] != null && Convert.ToBoolean(row.Cells["Select"].Value ?? false))
                {
                    selectedRows.Add(row);
                }
            }

            if (selectedRows.Count == 0)
            {
                MessageBox.Show("Silakan pilih produk yang ingin ditambahkan dengan mencentang checkbox", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int addedCount = 0;
            int skippedCount = 0;
            List<string> addedProducts = new List<string>();
            List<string> errorMessages = new List<string>();

            foreach (var row in selectedRows)
            {
                try
                {
                    bool productExists = Convert.ToBoolean(row.Cells["ProductExists"].Value);

                    if (productExists)
                    {
                        skippedCount++;
                        continue;
                    }

                    string productCode = row.Cells["ProductCode"].Value?.ToString() ?? "";
                    string productName = row.Cells["ProductName"].Value?.ToString() ?? "";
                    decimal unitPrice = Convert.ToDecimal(row.Cells["UnitPrice"].Value ?? 0);
                    int quantity = Convert.ToInt32(row.Cells["Quantity"].Value ?? 0);

                    // Validate data
                    if (string.IsNullOrWhiteSpace(productName))
                    {
                        errorMessages.Add("Product name is empty for one of the selected items");
                        continue;
                    }

                    if (unitPrice <= 0)
                    {
                        errorMessages.Add($"Invalid price for product: {productName}");
                        continue;
                    }

                    // Add product to database
                    AddProductToDatabase(productCode, productName, unitPrice, quantity);
                    addedCount++;
                    addedProducts.Add(productName);
                }
                catch (Exception ex)
                {
                    string productName = row.Cells["ProductName"].Value?.ToString() ?? "Unknown Product";
                    errorMessages.Add($"Error adding {productName}: {ex.Message}");
                }
            }

            // Show summary
            string message = $"Berhasil menambahkan {addedCount} produk ke database";
            if (skippedCount > 0)
            {
                message += $"\nDilewati {skippedCount} produk (sudah ada dalam database)";
            }
            if (addedProducts.Count > 0)
            {
                message += "\n\nProduk yang berhasil ditambahkan:";
                foreach (string product in addedProducts)
                {
                    message += $"\n• {product}";
                }
            }
            if (errorMessages.Count > 0)
            {
                message += "\n\nError yang terjadi:";
                foreach (string error in errorMessages)
                {
                    message += $"\n• {error}";
                }
            }

            MessageBox.Show(message, "Hasil Penambahan Produk", MessageBoxButtons.OK,
                addedCount > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (addedCount > 0)
            {
                // Refresh data
                LoadProducts();
                LoadPurchaseOrderDetails(selectedPurchaseOrderID);

                // Uncheck processed items
                foreach (var row in selectedRows)
                {
                    if (row.Cells["Select"] != null)
                    {
                        row.Cells["Select"].Value = false;
                    }
                }
                chkSelectAll.Checked = false;
            }
        }

        private void AddProductToDatabase(string productCode, string productName, decimal unitPrice, int initialStock)
        {
            try
            {
                // Check if product code already exists
                if (!string.IsNullOrWhiteSpace(productCode))
                {
                    string checkQuery = "SELECT COUNT(*) FROM Products WHERE ProductCode = @ProductCode AND IsActive = 1";
                    SqlParameter[] checkParams = new SqlParameter[]
                    {
                        new SqlParameter("@ProductCode", productCode)
                    };

                    object result = DatabaseHelper.ExecuteScalar(checkQuery, checkParams);
                    if (Convert.ToInt32(result) > 0)
                    {
                        throw new Exception($"Product with code '{productCode}' already exists");
                    }
                }

                // Generate unique product code if empty
                if (string.IsNullOrWhiteSpace(productCode))
                {
                    productCode = GenerateProductCode(productName);
                }

                // Check if Products table has Stock column
                string checkColumnQuery = @"
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'Products' AND COLUMN_NAME = 'Stock'";

                bool hasStockColumn = false;
                object columnResult = DatabaseHelper.ExecuteScalar(checkColumnQuery);
                hasStockColumn = Convert.ToInt32(columnResult) > 0;

                string insertQuery;
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (hasStockColumn)
                {
                    insertQuery = @"
                        INSERT INTO Products (ProductCode, ProductName, PurchasePrice, SellingPrice, Stock, IsActive, CreatedAt)
                        VALUES (@ProductCode, @ProductName, @PurchasePrice, @SellingPrice, @Stock, 1, GETDATE())";

                    parameters.Add(new SqlParameter("@Stock", initialStock));
                }
                else
                {
                    insertQuery = @"
                        INSERT INTO Products (ProductCode, ProductName, PurchasePrice, SellingPrice, IsActive, CreatedAt)
                        VALUES (@ProductCode, @ProductName, @PurchasePrice, @SellingPrice, 1, GETDATE())";
                }

                parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("@ProductCode", productCode),
                    new SqlParameter("@ProductName", productName),
                    new SqlParameter("@PurchasePrice", unitPrice),
                    new SqlParameter("@SellingPrice", unitPrice * 1.2m) // 20% markup
                });

                int rowsAffected = DatabaseHelper.ExecuteNonQuery(insertQuery, parameters.ToArray());

                if (rowsAffected == 0)
                {
                    throw new Exception("Failed to insert product into database");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to add product to database: {ex.Message}");
            }
        }

        private string GenerateProductCode(string productName)
        {
            try
            {
                // Generate code from first letters of words + timestamp
                string[] words = productName.Split(new char[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
                string codePrefix = "";

                foreach (string word in words.Take(3)) // Take first 3 words
                {
                    if (word.Length > 0)
                    {
                        codePrefix += word.Substring(0, 1).ToUpper();
                    }
                }

                if (string.IsNullOrWhiteSpace(codePrefix))
                {
                    codePrefix = "PRD";
                }

                // Add timestamp to make it unique
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                return $"{codePrefix}{timestamp}";
            }
            catch
            {
                return "PRD" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }
        }

        private void btnProcessStockIn_Click(object sender, EventArgs e)
        {
            // Check if any items are selected
            var selectedRows = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dgvPurchaseOrderDetails.Rows)
            {
                if (!row.IsNewRow && row.Cells["Select"] != null && Convert.ToBoolean(row.Cells["Select"].Value ?? false))
                {
                    selectedRows.Add(row);
                }
            }

            if (selectedRows.Count == 0)
            {
                MessageBox.Show("Silakan pilih item yang ingin diproses untuk Stock In", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Apakah Anda yakin ingin memproses Stock In untuk {selectedRows.Count} item yang dipilih?",
                "Konfirmasi",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            int processedCount = 0;

                            foreach (var row in selectedRows)
                            {
                                bool productExists = Convert.ToBoolean(row.Cells["ProductExists"].Value);
                                if (!productExists) continue; // Skip products that don't exist

                                int productId = Convert.ToInt32(row.Cells["ProductID"].Value);
                                int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                                string productName = row.Cells["ProductName"].Value?.ToString() ?? "";

                                // Check if Products table has Stock column, if not skip stock update
                                string checkColumnQuery = @"
                                    SELECT COUNT(*) 
                                    FROM INFORMATION_SCHEMA.COLUMNS 
                                    WHERE TABLE_NAME = 'Products' AND COLUMN_NAME = 'Stock'";

                                bool hasStockColumn = false;
                                using (SqlCommand checkCmd = new SqlCommand(checkColumnQuery, conn, transaction))
                                {
                                    int columnCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                                    hasStockColumn = columnCount > 0;
                                }

                                // Update stock in Products table only if Stock column exists
                                if (hasStockColumn)
                                {
                                    string updateStockQuery = @"
                                        UPDATE Products 
                                        SET Stock = ISNULL(Stock, 0) + @Quantity
                                        WHERE ProductID = @ProductID";

                                    using (SqlCommand cmd = new SqlCommand(updateStockQuery, conn, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                                        cmd.Parameters.AddWithValue("@ProductID", productId);
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                // Insert stock movement record
                                string insertMovementQuery = @"
                                    INSERT INTO StockMovements 
                                    (ProductID, ReferenceType, Quantity, ReferenceID, MovementDate, UserID, Notes)
                                    VALUES 
                                    (@ProductID, @ReferenceType, @Quantity, @ReferenceID, @MovementDate, @UserID, @Notes)";

                                using (SqlCommand cmd = new SqlCommand(insertMovementQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@ProductID", productId);
                                    cmd.Parameters.AddWithValue("@ReferenceType", "Purchase Order");
                                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                                    cmd.Parameters.AddWithValue("@ReferenceID", selectedPurchaseOrderID);
                                    cmd.Parameters.AddWithValue("@MovementDate", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@UserID", currentUserId);
                                    cmd.Parameters.AddWithValue("@Notes", $"Stock In dari PO - {productName}");
                                    cmd.ExecuteNonQuery();
                                }

                                processedCount++;
                            }

                            // Update Purchase Order status to Received if not already
                            string updatePOQuery = @"
                                UPDATE PurchaseOrders 
                                SET Status = 'Received'
                                WHERE PurchaseOrderID = @PurchaseOrderID 
                                AND Status != 'Received'";

                            using (SqlCommand cmd = new SqlCommand(updatePOQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@PurchaseOrderID", selectedPurchaseOrderID);
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();

                            MessageBox.Show($"Stock In berhasil diproses untuk {processedCount} item yang dipilih",
                                          "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Uncheck all items after processing
                            foreach (DataGridViewRow row in dgvPurchaseOrderDetails.Rows)
                            {
                                if (!row.IsNewRow && row.Cells["Select"] != null)
                                {
                                    row.Cells["Select"].Value = false;
                                }
                            }

                            // Refresh data
                            LoadMovementData();
                            LoadPurchaseOrderDetails(selectedPurchaseOrderID);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing stock in: " + ex.Message,
                              "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadMovementData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            dateTimePicker1.Value = DateTime.Today.AddDays(-30);
            dateTimePicker2.Value = DateTime.Today;
            LoadMovementData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void dgvPurchaseOrderDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle checkbox clicks
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridView dgv = sender as DataGridView;
                if (dgv.Columns[e.ColumnIndex].Name == "Select" && dgv.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn)
                {
                    // Toggle the checkbox value
                    bool currentValue = Convert.ToBoolean(dgv.Rows[e.RowIndex].Cells["Select"].Value ?? false);
                    dgv.Rows[e.RowIndex].Cells["Select"].Value = !currentValue;

                    // Update "Select All" checkbox status
                    UpdateSelectAllCheckbox();
                }
            }
        }

        private void UpdateSelectAllCheckbox()
        {
            if (dgvPurchaseOrderDetails.Rows.Count == 0)
            {
                chkSelectAll.Checked = false;
                return;
            }

            bool allSelected = true;
            bool anySelected = false;

            foreach (DataGridViewRow row in dgvPurchaseOrderDetails.Rows)
            {
                if (!row.IsNewRow && row.Cells["Select"] != null)
                {
                    bool isSelected = Convert.ToBoolean(row.Cells["Select"].Value ?? false);
                    if (isSelected)
                    {
                        anySelected = true;
                    }
                    else
                    {
                        allSelected = false;
                    }
                }
            }

            // Update checkbox without triggering event
            chkSelectAll.CheckedChanged -= chkSelectAll_CheckedChanged;
            chkSelectAll.Checked = allSelected && anySelected;
            chkSelectAll.CheckedChanged += chkSelectAll_CheckedChanged;
        }

        private void StockMovementForm_Load(object sender, EventArgs e)
        {
            // Set default date range
            dateTimePicker1.Value = DateTime.Today.AddDays(-30);
            dateTimePicker2.Value = DateTime.Today;
        }
    }
}