namespace Sparepart
{
    partial class ProductSelectionForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label labelProductName;
        private System.Windows.Forms.Label labelQty;
        private System.Windows.Forms.Label labelPrice;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support — do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBoxQty = new System.Windows.Forms.TextBox();
            this.textBoxPrice = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelProductName = new System.Windows.Forms.Label();
            this.labelQty = new System.Windows.Forms.Label();
            this.labelPrice = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.Location = new System.Drawing.Point(120, 17);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(220, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // textBoxQty
            // 
            this.textBoxQty.Location = new System.Drawing.Point(120, 57);
            this.textBoxQty.Name = "textBoxQty";
            this.textBoxQty.Size = new System.Drawing.Size(220, 20);
            this.textBoxQty.TabIndex = 3;
            // 
            // textBoxPrice
            // 
            this.textBoxPrice.Location = new System.Drawing.Point(120, 97);
            this.textBoxPrice.Name = "textBoxPrice";
            this.textBoxPrice.Size = new System.Drawing.Size(220, 20);
            this.textBoxPrice.TabIndex = 5;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(265, 135);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 30);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelProductName
            // 
            this.labelProductName.AutoSize = true;
            this.labelProductName.Location = new System.Drawing.Point(20, 20);
            this.labelProductName.Name = "labelProductName";
            this.labelProductName.Size = new System.Drawing.Size(75, 13);
            this.labelProductName.TabIndex = 0;
            this.labelProductName.Text = "Nama Produk:";
            // 
            // labelQty
            // 
            this.labelQty.AutoSize = true;
            this.labelQty.Location = new System.Drawing.Point(20, 60);
            this.labelQty.Name = "labelQty";
            this.labelQty.Size = new System.Drawing.Size(54, 13);
            this.labelQty.TabIndex = 2;
            this.labelQty.Text = "Kuantitas:";
            // 
            // labelPrice
            // 
            this.labelPrice.AutoSize = true;
            this.labelPrice.Location = new System.Drawing.Point(20, 100);
            this.labelPrice.Name = "labelPrice";
            this.labelPrice.Size = new System.Drawing.Size(76, 13);
            this.labelPrice.TabIndex = 4;
            this.labelPrice.Text = "Harga Satuan:";
            // 
            // ProductSelectionForm
            // 
            this.ClientSize = new System.Drawing.Size(370, 180);
            this.Controls.Add(this.labelProductName);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.labelQty);
            this.Controls.Add(this.textBoxQty);
            this.Controls.Add(this.labelPrice);
            this.Controls.Add(this.textBoxPrice);
            this.Controls.Add(this.buttonOK);
            this.Name = "ProductSelectionForm";
            this.Text = "Pilih Produk";
            this.Load += new System.EventHandler(this.ProductSelectionForm_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}