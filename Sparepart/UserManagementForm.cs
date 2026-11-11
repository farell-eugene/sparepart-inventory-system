using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Sparepart
{
    public partial class UserManagementForm : Form
    {
        private DataGridView dataGridView1;
        private Button button1; // Tambah
        private Button button2; // Edit
        private Button button3; // Hapus
        private List<User> users;

        public UserManagementForm()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            try
            {
                string query = @"
            SELECT 
                UserID, Username, Password, FullName, Email, Phone, 
                Role, IsActive, CreatedAt, LastLogin 
            FROM Users 
            WHERE IsActive = 1";

                DataTable dataTable = DatabaseHelper.ExecuteQuery(query);

                users = new List<User>();
                foreach (DataRow row in dataTable.Rows)
                {
                    users.Add(new User
                    {
                        UserID = Convert.ToInt32(row["UserID"]),
                        Username = row["Username"].ToString(),
                        Password = row["Password"].ToString(),
                        FullName = row["FullName"].ToString(),
                        Email = row["Email"].ToString(),
                        Phone = row["Phone"].ToString(),
                        Role = row["Role"].ToString(),
                        IsActive = Convert.ToBoolean(row["IsActive"]),
                        CreatedAt = Convert.ToDateTime(row["CreatedAt"]),
                        LastLogin = row["LastLogin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["LastLogin"])
                    });
                }

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = users;

                // Sembunyikan kolom Password untuk keamanan tampilan
                if (dataGridView1.Columns.Contains("Password"))
                    dataGridView1.Columns["Password"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = Microsoft.VisualBasic.Interaction.InputBox("Masukkan username:", "Tambah User");
            string password = Microsoft.VisualBasic.Interaction.InputBox("Masukkan password:", "Tambah User");
            string fullName = Microsoft.VisualBasic.Interaction.InputBox("Masukkan nama lengkap:", "Tambah User");
            string email = Microsoft.VisualBasic.Interaction.InputBox("Masukkan email:", "Tambah User");
            string phone = Microsoft.VisualBasic.Interaction.InputBox("Masukkan nomor telepon:", "Tambah User");
            string role = Microsoft.VisualBasic.Interaction.InputBox("Masukkan role (Admin/Karyawan):", "Tambah User");

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Semua field harus diisi!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string insertQuery = @"
        INSERT INTO Users (Username, Password, FullName, Email, Phone, Role, IsActive, CreatedAt)
        VALUES (@Username, @Password, @FullName, @Email, @Phone, @Role, @IsActive, @CreatedAt)";

            List<SqlParameter> insertParams = new List<SqlParameter>
    {
        new SqlParameter("@Username", username),
        new SqlParameter("@Password", password), // Untuk aplikasi nyata, jangan simpan password plaintext!
        new SqlParameter("@FullName", fullName),
        new SqlParameter("@Email", email),
        new SqlParameter("@Phone", phone),
        new SqlParameter("@Role", role),
        new SqlParameter("@IsActive", true),
        new SqlParameter("@CreatedAt", DateTime.Now)
    };

            try
            {
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(insertQuery, insertParams.ToArray());
                if (rowsAffected > 0)
                {
                    LoadUserData();
                    MessageBox.Show("User berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Gagal menambahkan user.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat menambahkan user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                User selectedUser = (User)dataGridView1.SelectedRows[0].DataBoundItem;

                string newUsername = Microsoft.VisualBasic.Interaction.InputBox("Masukkan username baru:", "Edit User", selectedUser.Username);
                string newPassword = Microsoft.VisualBasic.Interaction.InputBox("Masukkan password baru:", "Edit User", selectedUser.Password);
                string newFullName = Microsoft.VisualBasic.Interaction.InputBox("Masukkan nama lengkap baru:", "Edit User", selectedUser.FullName);
                string newEmail = Microsoft.VisualBasic.Interaction.InputBox("Masukkan email baru:", "Edit User", selectedUser.Email);
                string newPhone = Microsoft.VisualBasic.Interaction.InputBox("Masukkan nomor telepon baru:", "Edit User", selectedUser.Phone);
                string newRole = Microsoft.VisualBasic.Interaction.InputBox("Masukkan role baru (Admin/Karyawan):", "Edit User", selectedUser.Role);

                if (!string.IsNullOrWhiteSpace(newUsername) &&
                    !string.IsNullOrWhiteSpace(newPassword) &&
                    !string.IsNullOrWhiteSpace(newFullName) &&
                    !string.IsNullOrWhiteSpace(newEmail) &&
                    !string.IsNullOrWhiteSpace(newPhone) &&
                    !string.IsNullOrWhiteSpace(newRole))
                {
                    string updateQuery = @"
                UPDATE Users 
                SET 
                    Username = @Username, 
                    Password = @Password, 
                    FullName = @FullName, 
                    Email = @Email, 
                    Phone = @Phone, 
                    Role = @Role 
                WHERE UserID = @UserID";

                    List<SqlParameter> updateParams = new List<SqlParameter>
            {
                new SqlParameter("@Username", newUsername),
                new SqlParameter("@Password", newPassword), // Reminder: jangan simpan password plaintext di aplikasi produksi
                new SqlParameter("@FullName", newFullName),
                new SqlParameter("@Email", newEmail),
                new SqlParameter("@Phone", newPhone),
                new SqlParameter("@Role", newRole),
                new SqlParameter("@UserID", selectedUser.UserID)
            };

                    try
                    {
                        int rowsAffected = DatabaseHelper.ExecuteNonQuery(updateQuery, updateParams.ToArray());
                        if (rowsAffected > 0)
                        {
                            LoadUserData();
                            MessageBox.Show("User berhasil diupdate!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Gagal mengupdate user. Mungkin UserID tidak ditemukan.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Terjadi kesalahan saat update user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Semua field harus diisi!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Pilih satu baris user yang ingin diedit.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                User selectedUser = (User)dataGridView1.SelectedRows[0].DataBoundItem;

                if (MessageBox.Show("Apakah Anda yakin ingin menghapus user ini?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string deleteQuery = "UPDATE Users SET IsActive = 0 WHERE UserID = @UserID";
                    SqlParameter deleteParam = new SqlParameter("@UserID", selectedUser.UserID);

                    int rowsAffected = DatabaseHelper.ExecuteNonQuery(deleteQuery, new SqlParameter[] { deleteParam });

                    if (rowsAffected > 0)
                    {
                        LoadUserData();
                        MessageBox.Show("User berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Gagal menghapus user. Mungkin UserID tidak ditemukan.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

