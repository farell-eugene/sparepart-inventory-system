using System.Drawing;
using System.Windows.Forms;
using System;

namespace Sparepart
{
    public partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem1,
            this.menuItem2,
            this.menuItem3,
            this.menuItem4,
            this.menuItem5});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 0;
            // 
            // menuItem1
            // 
            this.menuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subMenuItem2});
            this.menuItem1.Name = "menuItem1";
            this.menuItem1.Size = new System.Drawing.Size(50, 20);
            this.menuItem1.Text = "Menu";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click_1);
            // 
            // subMenuItem2
            // 
            this.subMenuItem2.Name = "subMenuItem2";
            this.subMenuItem2.Size = new System.Drawing.Size(93, 22);
            this.subMenuItem2.Text = "Exit";
            this.subMenuItem2.Click += new System.EventHandler(this.subMenuItem2_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subMenuItem3,
            this.subMenuItem4,
            this.subMenuItem7,
            this.subMenuItem8,
            this.subMenuItem9});
            this.menuItem2.Name = "menuItem2";
            this.menuItem2.Size = new System.Drawing.Size(82, 20);
            this.menuItem2.Text = "Manajemen";
            // 
            // subMenuItem3
            // 
            this.subMenuItem3.Name = "subMenuItem3";
            this.subMenuItem3.Size = new System.Drawing.Size(164, 22);
            this.subMenuItem3.Text = "Kelola Pengguna";
            this.subMenuItem3.Click += new System.EventHandler(this.subMenuItem3_Click);
            // 
            // subMenuItem4
            // 
            this.subMenuItem4.Name = "subMenuItem4";
            this.subMenuItem4.Size = new System.Drawing.Size(164, 22);
            this.subMenuItem4.Text = "Kelola Produk";
            this.subMenuItem4.Click += new System.EventHandler(this.subMenuItem4_Click);
            // 
            // subMenuItem7
            // 
            this.subMenuItem7.Name = "subMenuItem7";
            this.subMenuItem7.Size = new System.Drawing.Size(164, 22);
            this.subMenuItem7.Text = "Kelola Supplier";
            this.subMenuItem7.Click += new System.EventHandler(this.subMenuItem7_Click);
            // 
            // subMenuItem8
            // 
            this.subMenuItem8.Name = "subMenuItem8";
            this.subMenuItem8.Size = new System.Drawing.Size(164, 22);
            this.subMenuItem8.Text = "Purchase Order";
            this.subMenuItem8.Click += new System.EventHandler(this.subMenuItem8_Click);
            // 
            // subMenuItem9
            // 
            this.subMenuItem9.Name = "subMenuItem9";
            this.subMenuItem9.Size = new System.Drawing.Size(164, 22);
            this.subMenuItem9.Text = "Stock Movement";
            this.subMenuItem9.Click += new System.EventHandler(this.subMenuItem9_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subMenuItem5});
            this.menuItem3.Name = "menuItem3";
            this.menuItem3.Size = new System.Drawing.Size(66, 20);
            this.menuItem3.Text = "Transaksi";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click_1);
            // 
            // subMenuItem5
            // 
            this.subMenuItem5.Name = "subMenuItem5";
            this.subMenuItem5.Size = new System.Drawing.Size(179, 22);
            this.subMenuItem5.Text = "Catat Transaksi Baru";
            // 
            // menuItem4
            // 
            this.menuItem4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subMenuItem6});
            this.menuItem4.Name = "menuItem4";
            this.menuItem4.Size = new System.Drawing.Size(62, 20);
            this.menuItem4.Text = "Laporan";
            // 
            // subMenuItem6
            // 
            this.subMenuItem6.Name = "subMenuItem6";
            this.subMenuItem6.Size = new System.Drawing.Size(173, 22);
            this.subMenuItem6.Text = "Laporan Keuangan";
            this.subMenuItem6.Click += new System.EventHandler(this.subMenuItem6_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subMenuItem10});
            this.menuItem5.Name = "menuItem5";
            this.menuItem5.Size = new System.Drawing.Size(57, 20);
            this.menuItem5.Text = "Produk";
            // 
            // subMenuItem10
            // 
            this.subMenuItem10.Name = "subMenuItem10";
            this.subMenuItem10.Size = new System.Drawing.Size(147, 22);
            this.subMenuItem10.Text = "Daftar Produk";
            this.subMenuItem10.Click += new System.EventHandler(this.SubMenuItem10_Click);
            // 
            // MainForm
            // 
            this.BackColor = System.Drawing.Color.Orange;
            this.BackgroundImage = global::Sparepart.Properties.Resources.b6f88eed846fe251408b764c8eb5a285;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainForm";
            this.Text = "Sistem Manajemen Toko";
            this.Load += new System.EventHandler(this.MainForm_Load_1);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
