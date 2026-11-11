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
    public partial class MainForm : Form
    {
        private MenuStrip menuStrip1;
        private ToolStripMenuItem menuItem1;
        private ToolStripMenuItem menuItem2;
        private ToolStripMenuItem menuItem3;
        private ToolStripMenuItem menuItem4;
        private ToolStripMenuItem menuItem5; // <--- Lihat
        private ToolStripMenuItem subMenuItem2;
        private ToolStripMenuItem subMenuItem3;
        private ToolStripMenuItem subMenuItem4;
        private ToolStripMenuItem subMenuItem5;
        private ToolStripMenuItem subMenuItem6;
        private ToolStripMenuItem subMenuItem7;
        private ToolStripMenuItem subMenuItem8;
        private ToolStripMenuItem subMenuItem9;
        private ToolStripMenuItem subMenuItem10;
        private User currentUser;

        public MainForm(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User tidak boleh null");
            }
            currentUser = user;
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            SetupMenuAccess();
        }

        private void MainForm_Load(object sender, EventArgs e) { }

        private void SetupMenuAccess()
        {
            try
            {
                if (menuItem2 == null || menuItem4 == null)
                {
                    MessageBox.Show("Menu belum diinisialisasi dengan benar", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Jika user bukan Admin, sembunyikan menu khusus Admin
                if (!string.Equals(currentUser?.Role, "Admin", StringComparison.OrdinalIgnoreCase))
                {
                    menuItem2.Visible = false; // Menu Manajemen
                    menuItem4.Visible = false; // Menu Laporan
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan saat mengatur menu: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Apakah Anda yakin ingin keluar dari aplikasi?", "Konfirmasi Keluar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void menuItem1_Click_1(object sender, EventArgs e) { }

        private void MainForm_Load_1(object sender, EventArgs e) { }

        private void subMenuItem2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Apakah Anda yakin ingin keluar dari aplikasi?", "Konfirmasi Keluar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void subMenuItem3_Click(object sender, EventArgs e)
        {
            try
            {
                UserManagementForm userForm = new UserManagementForm();
                userForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void subMenuItem4_Click(object sender, EventArgs e)
        {
            try
            {
                ProductManagementForm productForm = new ProductManagementForm();
                productForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void subMenuItem5_Click(object sender, EventArgs e)
        {
            try
            {
                TransactionForm transactionForm = new TransactionForm();
                transactionForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan saat membuka form transaksi: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void subMenuItem6_Click(object sender, EventArgs e)
        {
            try
            {
                Laporan reportForm = new Laporan();
                reportForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal membuka laporan: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void subMenuItem7_Click(object sender, EventArgs e)
        {
            try
            {
                SupplierManagementForm supplierForm = new SupplierManagementForm();
                supplierForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void subMenuItem8_Click(object sender, EventArgs e)
        {
            try
            {
                PurchaseOrderForm purchaseForm = new PurchaseOrderForm();
                purchaseForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void subMenuItem9_Click(object sender, EventArgs e)
        {
            try
            {
                StockMovementForm stockForm = new StockMovementForm();
                stockForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Untuk Daftar Produk
        private void SubMenuItem10_Click(object sender, EventArgs e)
        {
            try
            {
                ProductViewerForm form = new ProductViewerForm(); // Ganti dengan nama form milikmu
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal membuka daftar produk: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void menuItem3_Click_1(object sender, EventArgs e)
        {
            subMenuItem5_Click(sender, e); // jalankan submenu langsung saat klik menu
        }
    }
}
