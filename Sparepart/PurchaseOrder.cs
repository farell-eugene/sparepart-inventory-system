using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparepart
{
    public class PurchaseOrder
    {
        public int PurchaseID { get; set; }
        public int SupplierID { get; set; }
        public DateTime OrderDate { get; set; }
        public int UserID { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string PurchaseOrderNumber { get; set; } // Tambahkan properti ini
        public List<PurchaseOrderDetail> Details { get; set; }
    }
}
