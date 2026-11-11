# Sparepart Inventory System (C# WinForms)

A desktop inventory and sales management system for an auto spare-parts store. It handles users and roles, products and categories, suppliers, customer transactions, purchase orders, stock movements and **Crystal Reports** transaction reports. Visual Programming course project.

## Features

| Module | Forms | Description |
|---|---|---|
| Authentication | `LoginForm`, `RegisterForm`, `UserManagementForm` | Login, registration and user administration |
| Main menu | `MainForm` | Navigation to all modules |
| Products | `ProductManagementForm`, `ProductForm`, `ProductViewerForm`, `ProductSelectionForm` | Product catalog with categories, prices and stock |
| Suppliers | `SupplierManagementForm` | Supplier master data |
| Sales | `TransactionForm` | Customer transactions with line items and payment methods |
| Purchasing | `PurchaseOrderForm` | Purchase orders to suppliers |
| Inventory | `StockMovementForm` | Stock-in and stock-out history |
| Reporting | `Laporan`, `ReportTransaction.rpt` | Transaction reports with Crystal Reports |

## Database

SQL Server database `SparepartDB` (script: [`database/sparepart_db.sql`](database/sparepart_db.sql)) with 11 tables:

`Users` · `Suppliers` · `Categories` · `Products` · `Customers` · `PaymentMethods` · `Transactions` · `TransactionDetails` · `PurchaseOrders` · `PurchaseOrderDetails` · `StockMovements`

## Getting started

**Requirements:** Windows, Visual Studio 2019+ with the *.NET desktop development* workload, .NET Framework 4.7.2, SQL Server Express, and SAP Crystal Reports runtime for Visual Studio.

1. Run `database/sparepart_db.sql` in SQL Server Management Studio to create `SparepartDB`.
2. Open `Sparepart.sln` in Visual Studio.
3. Check the connection string in `Sparepart/App.config` and `Sparepart/DatabaseHelper.cs`. It defaults to `Data Source=.\SQLEXPRESS;Initial Catalog=SparepartDB;Integrated Security=True`.
4. Build and run (F5).

## Project structure

```
Sparepart.sln
Sparepart/
├── Program.cs
├── DatabaseHelper.cs                  # shared connection + query helpers
├── *Form.cs                           # WinForms screens
├── Product.cs, Supplier.cs, Transaction.cs, PurchaseOrder.cs, StockMovement.cs, User.cs   # domain models
├── ReportTransaction.rpt              # Crystal Reports template
└── SparepartDBDataSet.xsd             # typed dataset
database/sparepart_db.sql              # database schema
```

## Tech stack

C# · .NET Framework 4.7.2 · Windows Forms · ADO.NET · SQL Server · Crystal Reports
