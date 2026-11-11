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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Sistem Manajemen Toko - Landing Page";
            SetupForm();
        }

        private void SetupForm()
        {
            if (progressTimer == null || progressBar == null || loadingLabel == null)
            {
                MessageBox.Show("Komponen form tidak diinisialisasi dengan benar", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            this.WindowState = FormWindowState.Maximized;
            this.Text = "Sistem Manajemen Toko - Landing Page";
            this.progressTimer.Tick += ProgressTimer_Tick;
            this.progressTimer.Start();
        }

        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (progressBar.Value < 100)
                {
                    progressBar.Value += 2;
                    loadingLabel.Text = $"Loading: {progressBar.Value}%";
                }
                else
                {
                    progressTimer.Stop();
                    OpenLoginForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
        private void OpenLoginForm()
        {
            try
            {
                LoginForm loginForm = new LoginForm();
                loginForm.FormClosed += (s, args) => this.Close(); // Close the current form when LoginForm is closed
                this.Hide();
                loginForm.ShowDialog(); // Use ShowDialog instead of Show
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open login form: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        // Call this method when you want to open the LoginForm
        private void SomeEventHandler(object sender, EventArgs e)
        {
            OpenLoginForm();
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
