using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sparepart
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int? CategoryID { get; set; }
        public string CategoryName { get; set; } // Add this property
        public string Description { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int StockQuantity { get; set; }
        public int MinimumStock { get; set; }
        public int? SupplierID { get; set; }
        public string SupplierName { get; set; } // Add this property
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int Id
        {
            get { return ProductID; }
            set { ProductID = value; }
        }

        public decimal Price
        {
            get { return SellingPrice; }
            set { SellingPrice = value; }
        }

        // Get all products
        public static List<Product> GetAllProducts()
        {
            string query = @"
                SELECT p.*, c.CategoryName, s.SupplierName
                FROM Products p
                LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
                LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID
                WHERE p.IsActive = 1
                ORDER BY p.ProductName";

            DataTable dataTable = DatabaseHelper.ExecuteQuery(query);
            List<Product> products = new List<Product>();

            foreach (DataRow row in dataTable.Rows)
            {
                products.Add(new Product
                {
                    ProductID = Convert.ToInt32(row["ProductID"]),
                    ProductCode = row["ProductCode"].ToString(),
                    ProductName = row["ProductName"].ToString(),
                    CategoryID = row["CategoryID"] != DBNull.Value ? Convert.ToInt32(row["CategoryID"]) : 0,
                    CategoryName = row["CategoryName"] != DBNull.Value ? row["CategoryName"].ToString() : "",
                    Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : "",
                    PurchasePrice = Convert.ToDecimal(row["PurchasePrice"]),
                    SellingPrice = Convert.ToDecimal(row["SellingPrice"]),
                    StockQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MinimumStock = Convert.ToInt32(row["MinimumStock"]),
                    SupplierID = row["SupplierID"] != DBNull.Value ? Convert.ToInt32(row["SupplierID"]) : 0,
                    SupplierName = row["SupplierName"] != DBNull.Value ? row["SupplierName"].ToString() : "",
                    IsActive = Convert.ToBoolean(row["IsActive"]),
                    CreatedAt = Convert.ToDateTime(row["CreatedAt"]),
                    UpdatedAt = row["UpdatedAt"] != DBNull.Value ? Convert.ToDateTime(row["UpdatedAt"]) : (DateTime?)null
                });
            }

            return products;
        }

        // Get product by ID
        public static Product GetProductByID(int productID)
        {
            string query = @"
                SELECT p.*, c.CategoryName, s.SupplierName
                FROM Products p
                LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
                LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID
                WHERE p.ProductID = @ProductID";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductID", productID)
            };

            DataTable dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                return new Product
                {
                    ProductID = Convert.ToInt32(row["ProductID"]),
                    ProductCode = row["ProductCode"].ToString(),
                    ProductName = row["ProductName"].ToString(),
                    CategoryID = row["CategoryID"] != DBNull.Value ? Convert.ToInt32(row["CategoryID"]) : 0,
                    CategoryName = row["CategoryName"] != DBNull.Value ? row["CategoryName"].ToString() : "",
                    Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : "",
                    PurchasePrice = Convert.ToDecimal(row["PurchasePrice"]),
                    SellingPrice = Convert.ToDecimal(row["SellingPrice"]),
                    StockQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MinimumStock = Convert.ToInt32(row["MinimumStock"]),
                    SupplierID = row["SupplierID"] != DBNull.Value ? Convert.ToInt32(row["SupplierID"]) : 0,
                    SupplierName = row["SupplierName"] != DBNull.Value ? row["SupplierName"].ToString() : "",
                    IsActive = Convert.ToBoolean(row["IsActive"]),
                    CreatedAt = Convert.ToDateTime(row["CreatedAt"]),
                    UpdatedAt = row["UpdatedAt"] != DBNull.Value ? Convert.ToDateTime(row["UpdatedAt"]) : (DateTime?)null
                };
            }

            return null;
        }

        // Insert new product
        public bool Insert()
        {
            string query = @"
                INSERT INTO Products (
                    ProductCode, ProductName, CategoryID, Description, 
                    PurchasePrice, SellingPrice, StockQuantity, MinimumStock, 
                    SupplierID, IsActive, CreatedAt
                ) VALUES (
                    @ProductCode, @ProductName, @CategoryID, @Description, 
                    @PurchasePrice, @SellingPrice, @StockQuantity, @MinimumStock, 
                    @SupplierID, @IsActive, GETDATE()
                );
                SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductCode", ProductCode),
                new SqlParameter("@ProductName", ProductName),
                new SqlParameter("@CategoryID", CategoryID == 0 ? (object)DBNull.Value : CategoryID),
                new SqlParameter("@Description", string.IsNullOrEmpty(Description) ? (object)DBNull.Value : Description),
                new SqlParameter("@PurchasePrice", PurchasePrice),
                new SqlParameter("@SellingPrice", SellingPrice),
                new SqlParameter("@StockQuantity", StockQuantity),
                new SqlParameter("@MinimumStock", MinimumStock),
                new SqlParameter("@SupplierID", SupplierID == 0 ? (object)DBNull.Value : SupplierID),
                new SqlParameter("@IsActive", IsActive)
            };

            object result = DatabaseHelper.ExecuteScalar(query, parameters);
            if (result != null)
            {
                ProductID = Convert.ToInt32(result);
                return true;
            }

            return false;
        }

        // Update existing product
        public bool Update()
        {
            string query = @"
                UPDATE Products SET
                    ProductCode = @ProductCode,
                    ProductName = @ProductName,
                    CategoryID = @CategoryID,
                    Description = @Description,
                    PurchasePrice = @PurchasePrice,
                    SellingPrice = @SellingPrice,
                    StockQuantity = @StockQuantity,
                    MinimumStock = @MinimumStock,
                    SupplierID = @SupplierID,
                    IsActive = @IsActive,
                    UpdatedAt = GETDATE()
                WHERE ProductID = @ProductID";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductID", ProductID),
                new SqlParameter("@ProductCode", ProductCode),
                new SqlParameter("@ProductName", ProductName),
                new SqlParameter("@CategoryID", CategoryID == 0 ? (object)DBNull.Value : CategoryID),
                new SqlParameter("@Description", string.IsNullOrEmpty(Description) ? (object)DBNull.Value : Description),
                new SqlParameter("@PurchasePrice", PurchasePrice),
                new SqlParameter("@SellingPrice", SellingPrice),
                new SqlParameter("@StockQuantity", StockQuantity),
                new SqlParameter("@MinimumStock", MinimumStock),
                new SqlParameter("@SupplierID", SupplierID == 0 ? (object)DBNull.Value : SupplierID),
                new SqlParameter("@IsActive", IsActive)
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        // Delete product (soft delete)
        public bool Delete()
        {
            string query = "UPDATE Products SET IsActive = 0, UpdatedAt = GETDATE() WHERE ProductID = @ProductID";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductID", ProductID)
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }
    }
}