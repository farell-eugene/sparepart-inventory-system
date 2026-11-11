using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;

namespace Sparepart
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Check if there are any users in the database
            if (!AnyUsersExist())
            {
                // If no users exist, show a message and open the registration form
                MessageBox.Show("No users found in the system. Please create an admin account.",
                    "First Time Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);

                using (var registerForm = new RegisterForm(true))
                {
                    this.Hide();
                    var result = registerForm.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        Application.Exit();
                        return;
                    }
                    this.Show();
                }
            }
        }

        private bool AnyUsersExist()
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Users";
                object result = DatabaseHelper.ExecuteScalar(query);
                return Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking users: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

      
        private void UpdateLastLogin(int userId)
        {
            try
            {
                string query = "UPDATE Users SET LastLogin = GETDATE() WHERE UserID = @UserID";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@UserID", userId)
                };
                DatabaseHelper.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                // Just log the error, don't interrupt the login process
                Console.WriteLine("Error updating last login: " + ex.Message);
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var registerForm = new RegisterForm(false))
            {
                this.Hide();
                registerForm.ShowDialog();
                this.Show();
            }
        }

        
        private void button3_Click_1(object sender, EventArgs e)
        {
             
            string username = textBox1.Text.Trim();
            string password = textBox2.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password", "Login Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Hash the password for comparison
                string hashedPassword = HashPassword(password);

                // Check credentials
                string query = @"
                    SELECT UserID, Username, FullName, Role 
                    FROM Users 
                    WHERE Username = @Username AND Password = @Password AND IsActive = 1";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Username", username),
                    new SqlParameter("@Password", hashedPassword)
                };

                DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);

                if (result.Rows.Count > 0)
                {
                    // Login successful
                    DataRow userRow = result.Rows[0];
                    int userId = Convert.ToInt32(userRow["UserID"]);
                    string fullName = userRow["FullName"].ToString();
                    string role = userRow["Role"].ToString();

                    // Update last login time
                    UpdateLastLogin(userId);

                    // Create User object
                    User currentUser = new User
                    {
                        UserID = userId,
                        Username = username,
                        FullName = fullName,
                        Role = role
                    };

                    // Store user info in a global session
                    UserSession.UserID = userId;
                    UserSession.Username = username;
                    UserSession.FullName = fullName;
                    UserSession.Role = role;

                    MessageBox.Show($"Welcome, {fullName}!", "Login Successful",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Open the main form with the user object
                    this.Hide();
                    using (var mainForm = new MainForm(currentUser))
                    {
                        mainForm.ShowDialog();
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password", "Login Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login error: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            
        }
    }

}
    }

    // Static class to store user session information
    public static class UserSession
    {
        public static int UserID { get; set; }
        public static string Username { get; set; }
        public static string FullName { get; set; }
        public static string Role { get; set; }
    }
