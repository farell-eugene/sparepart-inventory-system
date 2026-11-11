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
    public partial class Laporan : Form
    {
        public Laporan()
        {
            InitializeComponent();
        }

       
        

        private void crystalReportViewer1_Load_1(object sender, EventArgs e)
        {
            try
            {
                List<Transaction> transactionList = Transaction.GetTransactions();

                var ds = new DataSet1(); // ganti dengan nama dataset .xsd kamu
                var table = ds.Transactions;

                if (transactionList != null && table != null)
                {
                    foreach (var trx in transactionList)
                    {
                        var row = table.NewRow();

                        // Jika kamu punya TransactionID di DataTable dan ingin abaikan, gunakan DBNull
                        // row["TransactionID"] = DBNull.Value;

                        row["TransactionDate"] = trx.TransactionDate;
                        row["CustomerName"] = trx.CustomerName;
                        row["InvoiceNumber"] = trx.InvoiceNumber;
                        row["TotalAmount"] = trx.TotalAmount;
                        row["PaidAmount"] = trx.PaidAmount;

                        table.Rows.Add(row);
                    }

                    var report = new ReportTransaction();
                    report.SetDataSource(ds);

                    crystalReportViewer1.ReportSource = report;
                    crystalReportViewer1.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
