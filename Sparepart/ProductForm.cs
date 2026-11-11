using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sparepart
{
    public partial class ProductForm : Form, IDisposable
    {
        private Label lblProductCode;
        private TextBox txtProductCode;
        private Label lblProductName;
        private TextBox txtProductName;
        private Label lblCategory;
        private ComboBox cmbCategory;
        private Label lblDescription;
        private TextBox txtDescription;
        private Label lblPurchasePrice;
        private NumericUpDown numPurchasePrice;
        private Label lblSellingPrice;
        private NumericUpDown numSellingPrice;
        private Label lblStock;
        private NumericUpDown numStock;
        private Label lblMinStock;
        private NumericUpDown numMinStock;
        private Label lblSupplier;
        private ComboBox cmbSupplier;
        private Button btnSave;
        private Button btnCancel;
        private SparepartDBDataSet sparepartDBDataSet;
        private BindingSource categoriesBindingSource;
        private IContainer components;
        private SparepartDBDataSetTableAdapters.CategoriesTableAdapter categoriesTableAdapter;
        private BindingSource suppliersBindingSource;
        private SparepartDBDataSetTableAdapters.SuppliersTableAdapter suppliersTableAdapter;
        private int _productId = 0;
        // Remove the unused _product field or use it in the code
        // Change this line:
        
        // To this:
        // No need for this field if it's not being used
        public ProductForm()
        {
            InitializeComponent();
            this.Text = "Tambah Produk Baru";
            LoadCategories();
            LoadSuppliers();
        }

        // Constructor for editing an existing product
        public ProductForm(int productId)
        {
            InitializeComponent();
            _productId = productId;
            this.Text = "Edit Produk";
            LoadCategories();
            LoadSuppliers();
            LoadProductData();
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblProductCode = new System.Windows.Forms.Label();
            this.txtProductCode = new System.Windows.Forms.TextBox();
            this.lblProductName = new System.Windows.Forms.Label();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.categoriesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sparepartDBDataSet = new Sparepart.SparepartDBDataSet();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblPurchasePrice = new System.Windows.Forms.Label();
            this.numPurchasePrice = new System.Windows.Forms.NumericUpDown();
            this.lblSellingPrice = new System.Windows.Forms.Label();
            this.numSellingPrice = new System.Windows.Forms.NumericUpDown();
            this.lblStock = new System.Windows.Forms.Label();
            this.numStock = new System.Windows.Forms.NumericUpDown();
            this.lblMinStock = new System.Windows.Forms.Label();
            this.numMinStock = new System.Windows.Forms.NumericUpDown();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.cmbSupplier = new System.Windows.Forms.ComboBox();
            this.suppliersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.categoriesTableAdapter = new Sparepart.SparepartDBDataSetTableAdapters.CategoriesTableAdapter();
            this.suppliersTableAdapter = new Sparepart.SparepartDBDataSetTableAdapters.SuppliersTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.categoriesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sparepartDBDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPurchasePrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSellingPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.suppliersBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // lblProductCode
            // 
            this.lblProductCode.Location = new System.Drawing.Point(20, 20);
            this.lblProductCode.Name = "lblProductCode";
            this.lblProductCode.Size = new System.Drawing.Size(120, 20);
            this.lblProductCode.TabIndex = 0;
            this.lblProductCode.Text = "Kode Produk:";
            // 
            // txtProductCode
            // 
            this.txtProductCode.Location = new System.Drawing.Point(150, 20);
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.Size = new System.Drawing.Size(200, 20);
            this.txtProductCode.TabIndex = 1;
            // 
            // lblProductName
            // 
            this.lblProductName.Location = new System.Drawing.Point(20, 50);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(120, 20);
            this.lblProductName.TabIndex = 2;
            this.lblProductName.Text = "Nama Produk:";
            // 
            // txtProductName
            // 
            this.txtProductName.Location = new System.Drawing.Point(150, 50);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(200, 20);
            this.txtProductName.TabIndex = 3;
            // 
            // lblCategory
            // 
            this.lblCategory.Location = new System.Drawing.Point(20, 80);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(120, 20);
            this.lblCategory.TabIndex = 4;
            this.lblCategory.Text = "Kategori:";
            // 
            // cmbCategory
            // 
            this.cmbCategory.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.categoriesBindingSource, "CategoryID", true));
            this.cmbCategory.DataSource = this.categoriesBindingSource;
            this.cmbCategory.DisplayMember = "CategoryName";
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Location = new System.Drawing.Point(150, 80);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(200, 21);
            this.cmbCategory.TabIndex = 5;
            this.cmbCategory.ValueMember = "CategoryID";
            // 
            // categoriesBindingSource
            // 
            this.categoriesBindingSource.DataMember = "Categories";
            this.categoriesBindingSource.DataSource = this.sparepartDBDataSet;
            // 
            // sparepartDBDataSet
            // 
            this.sparepartDBDataSet.DataSetName = "SparepartDBDataSet";
            this.sparepartDBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(20, 110);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(120, 20);
            this.lblDescription.TabIndex = 6;
            this.lblDescription.Text = "Deskripsi:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(150, 110);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(200, 60);
            this.txtDescription.TabIndex = 7;
            // 
            // lblPurchasePrice
            // 
            this.lblPurchasePrice.Location = new System.Drawing.Point(20, 180);
            this.lblPurchasePrice.Name = "lblPurchasePrice";
            this.lblPurchasePrice.Size = new System.Drawing.Size(120, 20);
            this.lblPurchasePrice.TabIndex = 8;
            this.lblPurchasePrice.Text = "Harga Beli:";
            // 
            // numPurchasePrice
            // 
            this.numPurchasePrice.Location = new System.Drawing.Point(150, 180);
            this.numPurchasePrice.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numPurchasePrice.Name = "numPurchasePrice";
            this.numPurchasePrice.Size = new System.Drawing.Size(200, 20);
            this.numPurchasePrice.TabIndex = 9;
            this.numPurchasePrice.ThousandsSeparator = true;
            // 
            // lblSellingPrice
            // 
            this.lblSellingPrice.Location = new System.Drawing.Point(20, 210);
            this.lblSellingPrice.Name = "lblSellingPrice";
            this.lblSellingPrice.Size = new System.Drawing.Size(120, 20);
            this.lblSellingPrice.TabIndex = 10;
            this.lblSellingPrice.Text = "Harga Jual:";
            // 
            // numSellingPrice
            // 
            this.numSellingPrice.Location = new System.Drawing.Point(150, 210);
            this.numSellingPrice.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numSellingPrice.Name = "numSellingPrice";
            this.numSellingPrice.Size = new System.Drawing.Size(200, 20);
            this.numSellingPrice.TabIndex = 11;
            this.numSellingPrice.ThousandsSeparator = true;
            // 
            // lblStock
            // 
            this.lblStock.Location = new System.Drawing.Point(20, 240);
            this.lblStock.Name = "lblStock";
            this.lblStock.Size = new System.Drawing.Size(120, 20);
            this.lblStock.TabIndex = 12;
            this.lblStock.Text = "Stok:";
            // 
            // numStock
            // 
            this.numStock.Location = new System.Drawing.Point(150, 240);
            this.numStock.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numStock.Name = "numStock";
            this.numStock.Size = new System.Drawing.Size(200, 20);
            this.numStock.TabIndex = 13;
            // 
            // lblMinStock
            // 
            this.lblMinStock.Location = new System.Drawing.Point(20, 270);
            this.lblMinStock.Name = "lblMinStock";
            this.lblMinStock.Size = new System.Drawing.Size(120, 20);
            this.lblMinStock.TabIndex = 14;
            this.lblMinStock.Text = "Stok Minimum:";
            // 
            // numMinStock
            // 
            this.numMinStock.Location = new System.Drawing.Point(150, 270);
            this.numMinStock.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numMinStock.Name = "numMinStock";
            this.numMinStock.Size = new System.Drawing.Size(200, 20);
            this.numMinStock.TabIndex = 15;
            // 
            // lblSupplier
            // 
            this.lblSupplier.Location = new System.Drawing.Point(20, 300);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(120, 20);
            this.lblSupplier.TabIndex = 16;
            this.lblSupplier.Text = "Supplier:";
            // 
            // cmbSupplier
            // 
            this.cmbSupplier.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.suppliersBindingSource, "SupplierName", true));
            this.cmbSupplier.DataSource = this.suppliersBindingSource;
            this.cmbSupplier.DisplayMember = "SupplierName";
            this.cmbSupplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSupplier.Location = new System.Drawing.Point(150, 300);
            this.cmbSupplier.Name = "cmbSupplier";
            this.cmbSupplier.Size = new System.Drawing.Size(200, 21);
            this.cmbSupplier.TabIndex = 17;
            this.cmbSupplier.ValueMember = "SupplierID";
            // 
            // suppliersBindingSource
            // 
            this.suppliersBindingSource.DataMember = "Suppliers";
            this.suppliersBindingSource.DataSource = this.sparepartDBDataSet;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnSave.Location = new System.Drawing.Point(150, 340);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 30);
            this.btnSave.TabIndex = 18;
            this.btnSave.Text = "Simpan";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnCancel.Location = new System.Drawing.Point(260, 340);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Batal";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // categoriesTableAdapter
            // 
            this.categoriesTableAdapter.ClearBeforeFill = true;
            // 
            // suppliersTableAdapter
            // 
            this.suppliersTableAdapter.ClearBeforeFill = true;
            // 
            // ProductForm
            // 
            this.BackColor = System.Drawing.Color.Orange;
            this.ClientSize = new System.Drawing.Size(600, 500);
            this.Controls.Add(this.lblProductCode);
            this.Controls.Add(this.txtProductCode);
            this.Controls.Add(this.lblProductName);
            this.Controls.Add(this.txtProductName);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblPurchasePrice);
            this.Controls.Add(this.numPurchasePrice);
            this.Controls.Add(this.lblSellingPrice);
            this.Controls.Add(this.numSellingPrice);
            this.Controls.Add(this.lblStock);
            this.Controls.Add(this.numStock);
            this.Controls.Add(this.lblMinStock);
            this.Controls.Add(this.numMinStock);
            this.Controls.Add(this.lblSupplier);
            this.Controls.Add(this.cmbSupplier);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Name = "ProductForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Product Form";
            this.Load += new System.EventHandler(this.ProductForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.categoriesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sparepartDBDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPurchasePrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSellingPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.suppliersBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void LoadCategories()
        {
            try
            {
                ComboBox cmbCategory = (ComboBox)this.Controls["cmbCategory"];
                cmbCategory.Items.Clear();

                // Add default item
                cmbCategory.Items.Add(new { CategoryID = 0, CategoryName = "-- Pilih Kategori --" });

                // Get categories from database
                string query = "SELECT CategoryID, CategoryName FROM Categories WHERE IsActive = 1 ORDER BY CategoryName";
                DataTable dataTable = DatabaseHelper.ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    cmbCategory.Items.Add(new
                    {
                        CategoryID = Convert.ToInt32(row["CategoryID"]),
                        CategoryName = row["CategoryName"].ToString()
                    });
                }

                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CategoryID";
                cmbCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSuppliers()
        {
            try
            {
                ComboBox cmbSupplier = (ComboBox)this.Controls["cmbSupplier"];
                cmbSupplier.Items.Clear();

                // Add default item
                cmbSupplier.Items.Add(new { SupplierID = 0, SupplierName = "-- Pilih Supplier --" });

                // Get suppliers from database
                string query = "SELECT SupplierID, SupplierName FROM Suppliers WHERE IsActive = 1 ORDER BY SupplierName";
                DataTable dataTable = DatabaseHelper.ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    cmbSupplier.Items.Add(new
                    {
                        SupplierID = Convert.ToInt32(row["SupplierID"]),
                        SupplierName = row["SupplierName"].ToString()
                    });
                }

                cmbSupplier.DisplayMember = "SupplierName";
                cmbSupplier.ValueMember = "SupplierID";
                cmbSupplier.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading suppliers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProductData()
        {
            try
            {
                if (_productId <= 0) return;

                string query = @"
                    SELECT * FROM Products 
                    WHERE ProductID = @ProductID";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ProductID", _productId)
                };

                DataTable dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];

                    // Populate form fields
                    ((TextBox)this.Controls["txtProductCode"]).Text = row["ProductCode"].ToString();
                    ((TextBox)this.Controls["txtProductName"]).Text = row["ProductName"].ToString();
                    ((TextBox)this.Controls["txtDescription"]).Text = row["Description"] != DBNull.Value ? row["Description"].ToString() : "";

                    ((NumericUpDown)this.Controls["numPurchasePrice"]).Value = Convert.ToDecimal(row["PurchasePrice"]);
                    ((NumericUpDown)this.Controls["numSellingPrice"]).Value = Convert.ToDecimal(row["SellingPrice"]);
                    ((NumericUpDown)this.Controls["numStock"]).Value = Convert.ToInt32(row["StockQuantity"]);
                    ((NumericUpDown)this.Controls["numMinStock"]).Value = Convert.ToInt32(row["MinimumStock"]);

                    // Set category
                    ComboBox cmbCategory = (ComboBox)this.Controls["cmbCategory"];
                    if (row["CategoryID"] != DBNull.Value)
                    {
                        int categoryId = Convert.ToInt32(row["CategoryID"]);
                        for (int i = 0; i < cmbCategory.Items.Count; i++)
                        {
                            dynamic item = cmbCategory.Items[i];
                            if (item.CategoryID == categoryId)
                            {
                                cmbCategory.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    // Set supplier
                    ComboBox cmbSupplier = (ComboBox)this.Controls["cmbSupplier"];
                    if (row["SupplierID"] != DBNull.Value)
                    {
                        int supplierId = Convert.ToInt32(row["SupplierID"]);
                        for (int i = 0; i < cmbSupplier.Items.Count; i++)
                        {
                            dynamic item = cmbSupplier.Items[i];
                            if (item.SupplierID == supplierId)
                            {
                                cmbSupplier.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Product not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading product data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    string productCode = ((TextBox)this.Controls["txtProductCode"]).Text;
                    string productName = ((TextBox)this.Controls["txtProductName"]).Text;
                    string description = ((TextBox)this.Controls["txtDescription"]).Text;

                    decimal purchasePrice = ((NumericUpDown)this.Controls["numPurchasePrice"]).Value;
                    decimal sellingPrice = ((NumericUpDown)this.Controls["numSellingPrice"]).Value;
                    int stockQuantity = (int)((NumericUpDown)this.Controls["numStock"]).Value;
                    int minimumStock = (int)((NumericUpDown)this.Controls["numMinStock"]).Value;

                    ComboBox cmbCategory = (ComboBox)this.Controls["cmbCategory"];
                    int categoryId = cmbCategory.SelectedIndex > 0 ? ((dynamic)cmbCategory.SelectedItem).CategoryID : 0;

                    ComboBox cmbSupplier = (ComboBox)this.Controls["cmbSupplier"];
                    int supplierId = cmbSupplier.SelectedIndex > 0 ? ((dynamic)cmbSupplier.SelectedItem).SupplierID : 0;

                    if (_productId > 0)
                    {
                        // Update existing product
                        string query = @"
                            UPDATE Products SET
                                ProductCode = @ProductCode,
                                ProductName = @ProductName,
                                CategoryID = @CategoryID,
                                Description = @Description,
                                PurchasePrice = @PurchasePrice,
                                SellingPrice = @SellingPrice,
                                StockQuantity = @StockQuantity,
                                MinimumStock = @MinimumStock,
                                SupplierID = @SupplierID,
                                UpdatedAt = GETDATE()
                            WHERE ProductID = @ProductID";

                        SqlParameter[] parameters = new SqlParameter[]
                        {
                            new SqlParameter("@ProductID", _productId),
                            new SqlParameter("@ProductCode", productCode),
                            new SqlParameter("@ProductName", productName),
                            new SqlParameter("@CategoryID", categoryId == 0 ? (object)DBNull.Value : categoryId),
                            new SqlParameter("@Description", string.IsNullOrEmpty(description) ? (object)DBNull.Value : description),
                            new SqlParameter("@PurchasePrice", purchasePrice),
                            new SqlParameter("@SellingPrice", sellingPrice),
                            new SqlParameter("@StockQuantity", stockQuantity),
                            new SqlParameter("@MinimumStock", minimumStock),
                            new SqlParameter("@SupplierID", supplierId == 0 ? (object)DBNull.Value : supplierId)
                        };

                        int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Produk berhasil diperbarui", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Gagal memperbarui produk", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        // Insert new product
                        string query = @"
                            INSERT INTO Products (
                                ProductCode, ProductName, CategoryID, Description, 
                                PurchasePrice, SellingPrice, StockQuantity, MinimumStock, 
                                SupplierID, IsActive, CreatedAt
                            ) VALUES (
                                @ProductCode, @ProductName, @CategoryID, @Description, 
                                @PurchasePrice, @SellingPrice, @StockQuantity, @MinimumStock, 
                                @SupplierID, 1, GETDATE()
                            );
                            SELECT SCOPE_IDENTITY();";

                        SqlParameter[] parameters = new SqlParameter[]
                        {
                            new SqlParameter("@ProductCode", productCode),
                            new SqlParameter("@ProductName", productName),
                            new SqlParameter("@CategoryID", categoryId == 0 ? (object)DBNull.Value : categoryId),
                            new SqlParameter("@Description", string.IsNullOrEmpty(description) ? (object)DBNull.Value : description),
                            new SqlParameter("@PurchasePrice", purchasePrice),
                            new SqlParameter("@SellingPrice", sellingPrice),
                            new SqlParameter("@StockQuantity", stockQuantity),
                            new SqlParameter("@MinimumStock", minimumStock),
                            new SqlParameter("@SupplierID", supplierId == 0 ? (object)DBNull.Value : supplierId)
                        };

                        object result = DatabaseHelper.ExecuteScalar(query, parameters);
                        if (result != null)
                        {
                            MessageBox.Show("Produk berhasil ditambahkan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Gagal menambahkan produk", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(((TextBox)this.Controls["txtProductCode"]).Text))
            {
                MessageBox.Show("Kode produk tidak boleh kosong", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Controls["txtProductCode"].Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(((TextBox)this.Controls["txtProductName"]).Text))
            {
                MessageBox.Show("Nama produk tidak boleh kosong", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Controls["txtProductName"].Focus();
                return false;
            }

            if (((NumericUpDown)this.Controls["numPurchasePrice"]).Value <= 0)
            {
                MessageBox.Show("Harga beli harus lebih dari 0", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Controls["numPurchasePrice"].Focus();
                return false;
            }

            if (((NumericUpDown)this.Controls["numSellingPrice"]).Value <= 0)
            {
                MessageBox.Show("Harga jual harus lebih dari 0", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Controls["numSellingPrice"].Focus();
                return false;
            }

            return true;
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'sparepartDBDataSet.Suppliers' table. You can move, or remove it, as needed.
            this.suppliersTableAdapter.Fill(this.sparepartDBDataSet.Suppliers);
            // TODO: This line of code loads data into the 'sparepartDBDataSet.Categories' table. You can move, or remove it, as needed.
            this.categoriesTableAdapter.Fill(this.sparepartDBDataSet.Categories);

        }
    }
}