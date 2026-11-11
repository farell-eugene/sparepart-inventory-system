using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparepart
{
    public class StockMovement
    {
        public int MovementID { get; set; }
        public int ProductID { get; set; }
        public string MovementType { get; set; }
        public int Quantity { get; set; }
        public int ReferenceID { get; set; }
        public DateTime MovementDate { get; set; }
        public int UserID { get; set; }
        public string Notes { get; set; }
    }
}
