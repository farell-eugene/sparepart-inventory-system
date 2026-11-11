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
    public partial class SupplierManagementForm : Form
    {
        private DataGridView dataGridView1;
        private Button button1; // Tambah
        private Button button2; // Edit
        private Button button3; // Hapus
        private TextBox textBox1; // Nama Supplier
        private TextBox textBox2; // Kontak Person
        private TextBox textBox3; // No. Telepon
        private TextBox textBox4; // Email
        private TextBox textBox5; // Alamat
        private int selectedSupplierId = 0;

        public SupplierManagementForm()
        {
            InitializeComponent();
            SetupDataGridView();
            LoadSupplierData();

            // Add event handler for selection changed
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        private void SetupDataGridView()
        {
            // Clear existing columns first
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("SupplierID", "ID");
            dataGridView1.Columns.Add("SupplierName", "Nama Supplier");
            dataGridView1.Columns.Add("ContactPerson", "Kontak Person");
            dataGridView1.Columns.Add("Phone", "No. Telepon");
            dataGridView1.Columns.Add("Email", "Email");
            dataGridView1.Columns.Add("Address", "Alamat");
            dataGridView1.Columns.Add("IsActive", "Status");

            // Hide the ID column
            dataGridView1.Columns["SupplierID"].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadSupplierData()
        {
            try
            {
                // Clear existing rows
                dataGridView1.Rows.Clear();

                // Get suppliers from database
                string query = "SELECT * FROM Suppliers WHERE IsActive = 1 ORDER BY SupplierName";
                DataTable dataTable = DatabaseHelper.ExecuteQuery(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    dataGridView1.Rows.Add(
                        row["SupplierID"],
                        row["SupplierName"],
                        row["ContactPerson"] == DBNull.Value ? "" : row["ContactPerson"],
                        row["Phone"],
                        row["Email"] == DBNull.Value ? "" : row["Email"],
                        row["Address"] == DBNull.Value ? "" : row["Address"],
                        Convert.ToBoolean(row["IsActive"]) ? "Aktif" : "Tidak Aktif"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading supplier data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                selectedSupplierId = Convert.ToInt32(selectedRow.Cells["SupplierID"].Value);

                textBox1.Text = selectedRow.Cells["SupplierName"].Value.ToString();
                textBox2.Text = selectedRow.Cells["ContactPerson"].Value.ToString();
                textBox3.Text = selectedRow.Cells["Phone"].Value.ToString();
                textBox4.Text = selectedRow.Cells["Email"].Value.ToString();
                textBox5.Text = selectedRow.Cells["Address"].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    var supplier = new Supplier
                    {
                        SupplierName = textBox1.Text,
                        ContactPerson = textBox2.Text,
                        Phone = textBox3.Text,
                        Email = textBox4.Text,
                        Address = textBox5.Text,
                        IsActive = true
                    };

                    // Save supplier to database
                    string query = @"
                INSERT INTO Suppliers (SupplierName, ContactPerson, Phone, Email, Address, IsActive, CreatedAt)
                VALUES (@SupplierName, @ContactPerson, @Phone, @Email, @Address, @IsActive, GETDATE());
                SELECT SCOPE_IDENTITY();";

                    SqlParameter[] parameters = new SqlParameter[]
                    {
                new SqlParameter("@SupplierName", supplier.SupplierName),
                new SqlParameter("@ContactPerson", string.IsNullOrEmpty(supplier.ContactPerson) ? (object)DBNull.Value : supplier.ContactPerson),
                new SqlParameter("@Phone", supplier.Phone),
                new SqlParameter("@Email", string.IsNullOrEmpty(supplier.Email) ? (object)DBNull.Value : supplier.Email),
                new SqlParameter("@Address", string.IsNullOrEmpty(supplier.Address) ? (object)DBNull.Value : supplier.Address),
                new SqlParameter("@IsActive", supplier.IsActive)
                    };

                    object result = DatabaseHelper.ExecuteScalar(query, parameters);
                    if (result != null)
                    {
                        MessageBox.Show("Supplier berhasil ditambahkan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                        LoadSupplierData();
                    }
                    else
                    {
                        MessageBox.Show("Gagal menambahkan supplier", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && ValidateInput())
            {
                try
                {
                    // Update supplier in database
                    string query = @"
                UPDATE Suppliers SET
                    SupplierName = @SupplierName,
                    ContactPerson = @ContactPerson,
                    Phone = @Phone,
                    Email = @Email,
                    Address = @Address,
                    UpdatedAt = GETDATE()
                WHERE SupplierID = @SupplierID";

                    SqlParameter[] parameters = new SqlParameter[]
                    {
                new SqlParameter("@SupplierID", selectedSupplierId),
                new SqlParameter("@SupplierName", textBox1.Text),
                new SqlParameter("@ContactPerson", string.IsNullOrEmpty(textBox2.Text) ? (object)DBNull.Value : textBox2.Text),
                new SqlParameter("@Phone", textBox3.Text),
                new SqlParameter("@Email", string.IsNullOrEmpty(textBox4.Text) ? (object)DBNull.Value : textBox4.Text),
                new SqlParameter("@Address", string.IsNullOrEmpty(textBox5.Text) ? (object)DBNull.Value : textBox5.Text)
                    };

                    int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Supplier berhasil diubah", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                        LoadSupplierData();
                    }
                    else
                    {
                        MessageBox.Show("Gagal mengubah supplier", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih supplier yang ingin diubah", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    string supplierName = dataGridView1.SelectedRows[0].Cells["SupplierName"].Value.ToString();

                    if (MessageBox.Show($"Apakah Anda yakin ingin menghapus supplier '{supplierName}'?", "Konfirmasi",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // Soft delete - set IsActive to 0
                        string query = @"
                            UPDATE Suppliers SET 
                                IsActive = 0, 
                                UpdatedAt = GETDATE() 
                            WHERE SupplierID = @SupplierID";

                        SqlParameter[] parameters = new SqlParameter[]
                        {
                            new SqlParameter("@SupplierID", selectedSupplierId)
                        };

                        int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Supplier berhasil dihapus", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearForm();
                            LoadSupplierData();
                        }
                        else
                        {
                            MessageBox.Show("Gagal menghapus supplier", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Pilih supplier yang ingin dihapus", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Nama Supplier harus diisi", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("No. Telepon harus diisi", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            selectedSupplierId = 0;
            dataGridView1.ClearSelection();
        }
    }
}