using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sparepart
{
    public class Supplier
    {
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Get all suppliers
        public static List<Supplier> GetAllSuppliers()
        {
            string query = "SELECT * FROM Suppliers WHERE IsActive = 1 ORDER BY SupplierName";
            DataTable dataTable = DatabaseHelper.ExecuteQuery(query);
            List<Supplier> suppliers = new List<Supplier>();

            foreach (DataRow row in dataTable.Rows)
            {
                suppliers.Add(new Supplier
                {
                    SupplierID = Convert.ToInt32(row["SupplierID"]),
                    SupplierName = row["SupplierName"].ToString(),
                    ContactPerson = row["ContactPerson"] != DBNull.Value ? row["ContactPerson"].ToString() : "",
                    Phone = row["Phone"].ToString(),
                    Email = row["Email"] != DBNull.Value ? row["Email"].ToString() : "",
                    Address = row["Address"] != DBNull.Value ? row["Address"].ToString() : "",
                    IsActive = Convert.ToBoolean(row["IsActive"]),
                    CreatedAt = Convert.ToDateTime(row["CreatedAt"]),
                    UpdatedAt = row["UpdatedAt"] != DBNull.Value ? Convert.ToDateTime(row["UpdatedAt"]) : (DateTime?)null
                });
            }

            return suppliers;
        }

        // Get supplier by ID
        public static Supplier GetSupplierByID(int supplierID)
        {
            string query = "SELECT * FROM Suppliers WHERE SupplierID = @SupplierID";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@SupplierID", supplierID)
            };

            DataTable dataTable = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                return new Supplier
                {
                    SupplierID = Convert.ToInt32(row["SupplierID"]),
                    SupplierName = row["SupplierName"].ToString(),
                    ContactPerson = row["ContactPerson"] != DBNull.Value ? row["ContactPerson"].ToString() : "",
                    Phone = row["Phone"].ToString(),
                    Email = row["Email"] != DBNull.Value ? row["Email"].ToString() : "",
                    Address = row["Address"] != DBNull.Value ? row["Address"].ToString() : "",
                    IsActive = Convert.ToBoolean(row["IsActive"]),
                    CreatedAt = Convert.ToDateTime(row["CreatedAt"]),
                    UpdatedAt = row["UpdatedAt"] != DBNull.Value ? Convert.ToDateTime(row["UpdatedAt"]) : (DateTime?)null
                };
            }

            return null;
        }

        // Insert new supplier
        public bool Insert()
        {
            string query = @"
                INSERT INTO Suppliers (
                    SupplierName, ContactPerson, Phone, Email, Address, IsActive, CreatedAt
                ) VALUES (
                    @SupplierName, @ContactPerson, @Phone, @Email, @Address, @IsActive, GETDATE()
                );
                SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@SupplierName", SupplierName),
                new SqlParameter("@ContactPerson", string.IsNullOrEmpty(ContactPerson) ? (object)DBNull.Value : ContactPerson),
                new SqlParameter("@Phone", Phone),
                new SqlParameter("@Email", string.IsNullOrEmpty(Email) ? (object)DBNull.Value : Email),
                new SqlParameter("@Address", string.IsNullOrEmpty(Address) ? (object)DBNull.Value : Address),
                new SqlParameter("@IsActive", IsActive)
            };

            object result = DatabaseHelper.ExecuteScalar(query, parameters);
            if (result != null)
            {
                SupplierID = Convert.ToInt32(result);
                return true;
            }

            return false;
        }

        // Update existing supplier
        public bool Update()
        {
            string query = @"
                UPDATE Suppliers SET
                    SupplierName = @SupplierName,
                    ContactPerson = @ContactPerson,
                    Phone = @Phone,
                    Email = @Email,
                    Address = @Address,
                    IsActive = @IsActive,
                    UpdatedAt = GETDATE()
                WHERE SupplierID = @SupplierID";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@SupplierID", SupplierID),
                new SqlParameter("@SupplierName", SupplierName),
                new SqlParameter("@ContactPerson", string.IsNullOrEmpty(ContactPerson) ? (object)DBNull.Value : ContactPerson),
                new SqlParameter("@Phone", Phone),
                new SqlParameter("@Email", string.IsNullOrEmpty(Email) ? (object)DBNull.Value : Email),
                new SqlParameter("@Address", string.IsNullOrEmpty(Address) ? (object)DBNull.Value : Address),
                new SqlParameter("@IsActive", IsActive)
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        // Delete supplier (soft delete)
        public bool Delete()
        {
            string query = "UPDATE Suppliers SET IsActive = 0, UpdatedAt = GETDATE() WHERE SupplierID = @SupplierID";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@SupplierID", SupplierID)
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }
    }
}