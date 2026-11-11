using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sparepart
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string CustomerName { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public string Notes { get; set; }
        public List<TransactionDetail> Details { get; set; }

        public static List<Transaction> GetTransactions()
        {
            var transactions = new List<Transaction>();

            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=SparepartDB;Integrated Security=True";

            string query = @"
        SELECT 
            t.TransactionDate,
            t.InvoiceNumber,
            c.CustomerName,
            t.TotalAmount,
            t.PaidAmount
        FROM Transactions t
        LEFT JOIN Customers c ON t.CustomerID = c.CustomerID
        ORDER BY t.TransactionDate DESC";

            using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
            using (var command = new System.Data.SqlClient.SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            transactions.Add(new Transaction
                            {
                                TransactionDate = reader.GetDateTime(0),
                                InvoiceNumber = reader.GetString(1),
                                CustomerName = reader.GetString(2),
                                TotalAmount = reader.GetDecimal(3),
                                PaidAmount = reader.GetDecimal(4)
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal mengambil data transaksi: " + ex.Message);
                }
            }

            return transactions;
        }

    }
}
