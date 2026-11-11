namespace Sparepart
{
    partial class StockMovementForm
    {
        private System.ComponentModel.IContainer components = null;

        // Filter Controls
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button button1; // Filter
        private System.Windows.Forms.Button button2; // Reset
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1; // From Date
        private System.Windows.Forms.Label label2; // To Date
        private System.Windows.Forms.Label label3; // Product
        private System.Windows.Forms.Label label4; // Type

        // Purchase Order Controls
        private System.Windows.Forms.Button btnGetFromPO;
        private System.Windows.Forms.DataGridView dgvPurchaseOrderDetails;
        private System.Windows.Forms.Button btnAddToProducts;
        private System.Windows.Forms.Button btnProcessStockIn;
        private System.Windows.Forms.CheckBox chkSelectAll;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.btnGetFromPO = new System.Windows.Forms.Button();
            this.dgvPurchaseOrderDetails = new System.Windows.Forms.DataGridView();
            this.btnAddToProducts = new System.Windows.Forms.Button();
            this.btnProcessStockIn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchaseOrderDetails)).BeginInit();
            this.SuspendLayout();

            // 
            // label1 (From Date)
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Dari Tanggal:";

            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(12, 31);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(120, 20);
            this.dateTimePicker1.TabIndex = 1;

            // 
            // label2 (To Date)
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(138, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Sampai Tanggal:";

            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Location = new System.Drawing.Point(138, 31);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(120, 20);
            this.dateTimePicker2.TabIndex = 3;

            // 
            // label3 (Product)
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(264, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Produk:";

            // 
            // comboBox1 (Product)
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Location = new System.Drawing.Point(264, 31);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(150, 21);
            this.comboBox1.TabIndex = 5;

            // 
            // label4 (Type)
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(420, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Tipe:";

            // 
            // comboBox2 (Type)
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.Location = new System.Drawing.Point(420, 31);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(120, 21);
            this.comboBox2.TabIndex = 7;

            // 
            // button1 (Filter)
            // 
            this.button1.BackColor = System.Drawing.Color.DodgerBlue;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(550, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Filter";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);

            // 
            // button2 (Reset)
            // 
            this.button2.BackColor = System.Drawing.Color.Gray;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(631, 29);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Reset";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);

            // 
            // dataGridView1 (Stock Movement Data)
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(12, 65);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(776, 200);
            this.dataGridView1.TabIndex = 10;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);

            // 
            // btnGetFromPO
            // 
            this.btnGetFromPO.BackColor = System.Drawing.Color.Orange;
            this.btnGetFromPO.ForeColor = System.Drawing.Color.White;
            this.btnGetFromPO.Location = new System.Drawing.Point(12, 280);
            this.btnGetFromPO.Name = "btnGetFromPO";
            this.btnGetFromPO.Size = new System.Drawing.Size(180, 35);
            this.btnGetFromPO.TabIndex = 11;
            this.btnGetFromPO.Text = "Ambil dari Purchase Order";
            this.btnGetFromPO.UseVisualStyleBackColor = false;
            this.btnGetFromPO.Click += new System.EventHandler(this.btnGetFromPO_Click);

            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(12, 325);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(78, 17);
            this.chkSelectAll.TabIndex = 12;
            this.chkSelectAll.Text = "Pilih Semua";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.Visible = false;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);

            // 
            // btnAddToProducts
            // 
            this.btnAddToProducts.BackColor = System.Drawing.Color.Green;
            this.btnAddToProducts.Enabled = false;
            this.btnAddToProducts.ForeColor = System.Drawing.Color.White;
            this.btnAddToProducts.Location = new System.Drawing.Point(200, 320);
            this.btnAddToProducts.Name = "btnAddToProducts";
            this.btnAddToProducts.Size = new System.Drawing.Size(180, 30);
            this.btnAddToProducts.TabIndex = 13;
            this.btnAddToProducts.Text = "Tambahkan ke Produk";
            this.btnAddToProducts.UseVisualStyleBackColor = false;
            this.btnAddToProducts.Visible = false;
            this.btnAddToProducts.Click += new System.EventHandler(this.btnAddToProducts_Click);

            // 
            // btnProcessStockIn
            // 
            this.btnProcessStockIn.BackColor = System.Drawing.Color.DarkGreen;
            this.btnProcessStockIn.Enabled = false;
            this.btnProcessStockIn.ForeColor = System.Drawing.Color.White;
            this.btnProcessStockIn.Location = new System.Drawing.Point(400, 320);
            this.btnProcessStockIn.Name = "btnProcessStockIn";
            this.btnProcessStockIn.Size = new System.Drawing.Size(180, 30);
            this.btnProcessStockIn.TabIndex = 14;
            this.btnProcessStockIn.Text = "Proses Stock In dari PO";
            this.btnProcessStockIn.UseVisualStyleBackColor = false;
            this.btnProcessStockIn.Visible = false;
            this.btnProcessStockIn.Click += new System.EventHandler(this.btnProcessStockIn_Click);

            // 
            // dgvPurchaseOrderDetails
            // 
            this.dgvPurchaseOrderDetails.AllowUserToAddRows = false;
            this.dgvPurchaseOrderDetails.AllowUserToDeleteRows = false;
            this.dgvPurchaseOrderDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPurchaseOrderDetails.Location = new System.Drawing.Point(12, 360);
            this.dgvPurchaseOrderDetails.MultiSelect = false;
            this.dgvPurchaseOrderDetails.Name = "dgvPurchaseOrderDetails";
            this.dgvPurchaseOrderDetails.ReadOnly = true;
            this.dgvPurchaseOrderDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPurchaseOrderDetails.Size = new System.Drawing.Size(776, 120);
            this.dgvPurchaseOrderDetails.TabIndex = 15;
            this.dgvPurchaseOrderDetails.Visible = false;
            this.dgvPurchaseOrderDetails.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurchaseOrderDetails_CellContentClick);

            // 
            // StockMovementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.dgvPurchaseOrderDetails);
            this.Controls.Add(this.btnProcessStockIn);
            this.Controls.Add(this.btnAddToProducts);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.btnGetFromPO);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label1);
            this.Name = "StockMovementForm";
            this.Text = "Stock Movement Management";
            this.Load += new System.EventHandler(this.StockMovementForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchaseOrderDetails)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}