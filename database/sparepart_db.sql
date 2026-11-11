-- Create the Sparepart database
CREATE DATABASE SparepartDB;
GO

USE SparepartDB;
GO

-- Create Users table
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    Role NVARCHAR(20) NOT NULL, -- Admin, Kasir, Gudang, etc.
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    LastLogin DATETIME NULL
);

INSERT INTO Users (Username, Password, FullName, Email, Phone, Role, IsActive)
VALUES (
    'demo_user',              -- Username
    'ChangeMe123!',              -- Password (disarankan disimpan dalam bentuk hash)
    'Demo User',            -- Full Name
    'demo@example.com',       -- Email
    '080000000001',             -- Phone
    'Admin',                    -- Role
    1                           -- IsActive
);

INSERT INTO Users (Username, Password, FullName, Email, Phone, Role, IsActive)
VALUES (
    'demo_staff',              -- Username
    'ChangeMe123!',              -- Password (disarankan disimpan dalam bentuk hash)
    'Demo Staff',            -- Full Name
    'staff@example.com',       -- Email
    '080000000002',             -- Phone
    'Admin',                    -- Role
    1                           -- IsActive
);

-- Create Suppliers table
CREATE TABLE Suppliers (
    SupplierID INT PRIMARY KEY IDENTITY(1,1),
    SupplierName NVARCHAR(100) NOT NULL,
    ContactPerson NVARCHAR(100),
    Phone NVARCHAR(20) NOT NULL,
    Email NVARCHAR(100),
    Address NVARCHAR(255),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);

-- Create Categories table
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255)
);

-- Create Products table
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductCode NVARCHAR(20) NOT NULL UNIQUE,
    ProductName NVARCHAR(100) NOT NULL,
    CategoryID INT REFERENCES Categories(CategoryID),
    Description NVARCHAR(255),
    PurchasePrice DECIMAL(18,2) NOT NULL,
    SellingPrice DECIMAL(18,2) NOT NULL,
    StockQuantity INT DEFAULT 0,
    MinimumStock INT DEFAULT 5,
    SupplierID INT REFERENCES Suppliers(SupplierID),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);

-- Create Customers table
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    CustomerName NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20),
    Email NVARCHAR(100),
    Address NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Create PaymentMethods table
CREATE TABLE PaymentMethods (
    PaymentMethodID INT PRIMARY KEY IDENTITY(1,1),
    MethodName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255)
);

-- Create Transactions table (Sales)
CREATE TABLE Transactions (
    TransactionID INT PRIMARY KEY IDENTITY(1,1),
    TransactionDate DATETIME DEFAULT GETDATE(),
    CustomerID INT REFERENCES Customers(CustomerID),
    UserID INT REFERENCES Users(UserID),
    PaymentMethodID INT REFERENCES PaymentMethods(PaymentMethodID),
    TotalAmount DECIMAL(18,2) NOT NULL,
    PaidAmount DECIMAL(18,2) NOT NULL,
    ChangeAmount DECIMAL(18,2) NOT NULL,
    Notes NVARCHAR(255),
    InvoiceNumber NVARCHAR(20) UNIQUE
);

-- Create TransactionDetails table
CREATE TABLE TransactionDetails (
    TransactionDetailID INT PRIMARY KEY IDENTITY(1,1),
    TransactionID INT REFERENCES Transactions(TransactionID),
    ProductID INT REFERENCES Products(ProductID),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    Subtotal DECIMAL(18,2) NOT NULL
);

-- Create PurchaseOrders table
CREATE TABLE PurchaseOrders (
    PurchaseOrderID INT PRIMARY KEY IDENTITY(1,1),
    OrderDate DATETIME DEFAULT GETDATE(),
    SupplierID INT REFERENCES Suppliers(SupplierID),
    UserID INT REFERENCES Users(UserID),
    TotalAmount DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(20) DEFAULT 'Pending', -- Pending, Received, Cancelled
    Notes NVARCHAR(255),
    PONumber NVARCHAR(20) UNIQUE
);

-- Create PurchaseOrderDetails table
CREATE TABLE PurchaseOrderDetails (
    PurchaseOrderDetailID INT PRIMARY KEY IDENTITY(1,1),
    PurchaseOrderID INT REFERENCES PurchaseOrders(PurchaseOrderID),
    ProductID INT REFERENCES Products(ProductID),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    Subtotal DECIMAL(18,2) NOT NULL,
    ReceivedQuantity INT DEFAULT 0
);

-- Create StockMovements table
CREATE TABLE StockMovements (
    MovementID INT PRIMARY KEY IDENTITY(1,1),
    MovementDate DATETIME DEFAULT GETDATE(),
    ProductID INT REFERENCES Products(ProductID),
    Quantity INT NOT NULL, -- Positive for incoming, negative for outgoing
    ReferenceType NVARCHAR(20) NOT NULL, -- PO, Sale, Adjustment, Return
    ReferenceID INT NOT NULL, -- ID of the related transaction
    Notes NVARCHAR(255),
    UserID INT REFERENCES Users(UserID)
);

-- Insert default payment methods
INSERT INTO PaymentMethods (MethodName, Description)
VALUES 
('Cash', 'Pembayaran tunai'),
('Debit Card', 'Pembayaran dengan kartu debit'),
('Credit Card', 'Pembayaran dengan kartu kredit'),
('Transfer', 'Pembayaran melalui transfer bank'),
('QRIS', 'Pembayaran melalui QRIS');

-- Insert default categories
INSERT INTO Categories (CategoryName, Description)
VALUES 
('Engine Parts', 'Suku cadang mesin'),
('Electrical', 'Komponen kelistrikan'),
('Body Parts', 'Suku cadang body kendaraan'),
('Suspension', 'Sistem suspensi'),
('Brake System', 'Sistem pengereman'),
('Transmission', 'Sistem transmisi'),
('Accessories', 'Aksesoris kendaraan');

-- Insert default admin user
INSERT INTO Users (Username, Password, FullName, Role, IsActive)
VALUES ('admin', 'admin123', 'Administrator', 'Admin', 1);

-- Create a view for product stock information
CREATE VIEW vw_ProductStock AS
SELECT 
    p.ProductID,
    p.ProductCode,
    p.ProductName,
    c.CategoryName,
    p.StockQuantity,
    p.MinimumStock,
    CASE 
        WHEN p.StockQuantity <= p.MinimumStock THEN 'Low Stock'
        ELSE 'Normal'
    END AS StockStatus,
    s.SupplierName
FROM 
    Products p
    LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
    LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID;

-- Create a view for transaction summary
CREATE VIEW vw_TransactionSummary AS
SELECT 
    t.TransactionID,
    t.TransactionDate,
    t.InvoiceNumber,
    c.CustomerName,
    u.Username AS CashierName,
    pm.MethodName AS PaymentMethod,
    t.TotalAmount,
    COUNT(td.TransactionDetailID) AS TotalItems,
    SUM(td.Quantity) AS TotalQuantity
FROM 
    Transactions t
    LEFT JOIN Customers c ON t.CustomerID = c.CustomerID
    LEFT JOIN Users u ON t.UserID = u.UserID
    LEFT JOIN PaymentMethods pm ON t.PaymentMethodID = pm.PaymentMethodID
    LEFT JOIN TransactionDetails td ON t.TransactionID = td.TransactionID
GROUP BY 
    t.TransactionID, t.TransactionDate, t.InvoiceNumber, c.CustomerName, 
    u.Username, pm.MethodName, t.TotalAmount;

-- Create a view for purchase order summary
CREATE VIEW vw_PurchaseOrderSummary AS
SELECT 
    po.PurchaseOrderID,
    po.OrderDate,
    po.PONumber,
    s.SupplierName,
    u.Username AS OrderedBy,
    po.TotalAmount,
    po.Status,
    COUNT(pod.PurchaseOrderDetailID) AS TotalItems,
    SUM(pod.Quantity) AS TotalQuantity,
    SUM(pod.ReceivedQuantity) AS TotalReceivedQuantity
FROM 
    PurchaseOrders po
    LEFT JOIN Suppliers s ON po.SupplierID = s.SupplierID
    LEFT JOIN Users u ON po.UserID = u.UserID
    LEFT JOIN PurchaseOrderDetails pod ON po.PurchaseOrderID = pod.PurchaseOrderID
GROUP BY 
    po.PurchaseOrderID, po.OrderDate, po.PONumber, s.SupplierName, 
    u.Username, po.TotalAmount, po.Status;

-- Create a stored procedure to update product stock
CREATE PROCEDURE sp_UpdateProductStock
    @ProductID INT,
    @Quantity INT,
    @ReferenceType NVARCHAR(20),
    @ReferenceID INT,
    @Notes NVARCHAR(255),
    @UserID INT
AS
BEGIN
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Update product stock
        UPDATE Products
        SET StockQuantity = StockQuantity + @Quantity,
            UpdatedAt = GETDATE()
        WHERE ProductID = @ProductID;
        
        -- Record stock movement
        INSERT INTO StockMovements (
            MovementDate, ProductID, Quantity, 
            ReferenceType, ReferenceID, Notes, UserID
        )
        VALUES (
            GETDATE(), @ProductID, @Quantity,
            @ReferenceType, @ReferenceID, @Notes, @UserID
        );
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Create a stored procedure to process a transaction
CREATE PROCEDURE sp_ProcessTransaction
    @CustomerID INT,
    @UserID INT,
    @PaymentMethodID INT,
    @TotalAmount DECIMAL(18,2),
    @PaidAmount DECIMAL(18,2),
    @Notes NVARCHAR(255),
    @TransactionID INT OUTPUT
AS
BEGIN
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Generate invoice number (format: INV-YYYYMMDD-XXXX)
        DECLARE @InvoiceNumber NVARCHAR(20);
        DECLARE @CurrentDate NVARCHAR(8) = FORMAT(GETDATE(), 'yyyyMMdd');
        DECLARE @Counter INT;
        
        SELECT @Counter = COUNT(*) + 1 
        FROM Transactions 
        WHERE CONVERT(DATE, TransactionDate) = CONVERT(DATE, GETDATE());
        
        SET @InvoiceNumber = 'INV-' + @CurrentDate + '-' + RIGHT('0000' + CAST(@Counter AS NVARCHAR(4)), 4);
        
        -- Insert transaction
        INSERT INTO Transactions (
            TransactionDate, CustomerID, UserID, PaymentMethodID,
            TotalAmount, PaidAmount, ChangeAmount, Notes, InvoiceNumber
        )
        VALUES (
            GETDATE(), @CustomerID, @UserID, @PaymentMethodID,
            @TotalAmount, @PaidAmount, @PaidAmount - @TotalAmount, @Notes, @InvoiceNumber
        );
        
        SET @TransactionID = SCOPE_IDENTITY();
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Create a stored procedure to add transaction detail and update stock
CREATE PROCEDURE sp_AddTransactionDetail
    @TransactionID INT,
    @ProductID INT,
    @Quantity INT,
    @UnitPrice DECIMAL(18,2),
    @UserID INT
AS
BEGIN
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Calculate subtotal
        DECLARE @Subtotal DECIMAL(18,2) = @Quantity * @UnitPrice;
        
        -- Insert transaction detail
        INSERT INTO TransactionDetails (
            TransactionID, ProductID, Quantity, UnitPrice, Subtotal
        )
        VALUES (
            @TransactionID, @ProductID, @Quantity, @UnitPrice, @Subtotal
        );
        
        -- Update product stock (negative quantity for sales)
        EXEC sp_UpdateProductStock 
            @ProductID = @ProductID,
            @Quantity = @Quantity,
            @ReferenceType = 'Sale',
            @ReferenceID = @TransactionID,
            @Notes = 'Sale transaction',
            @UserID = @UserID;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Create a stored procedure to create purchase order
CREATE PROCEDURE sp_CreatePurchaseOrder
    @SupplierID INT,
    @UserID INT,
    @TotalAmount DECIMAL(18,2),
    @Notes NVARCHAR(255),
    @PurchaseOrderID INT OUTPUT
AS
BEGIN
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Generate PO number (format: PO-YYYYMMDD-XXXX)
        DECLARE @PONumber NVARCHAR(20);
        DECLARE @CurrentDate NVARCHAR(8) = FORMAT(GETDATE(), 'yyyyMMdd');
        DECLARE @Counter INT;
        
        SELECT @Counter = COUNT(*) + 1 
        FROM PurchaseOrders 
        WHERE CONVERT(DATE, OrderDate) = CONVERT(DATE, GETDATE());
        
        SET @PONumber = 'PO-' + @CurrentDate + '-' + RIGHT('0000' + CAST(@Counter AS NVARCHAR(4)), 4);
        
        -- Insert purchase order
        INSERT INTO PurchaseOrders (
            OrderDate, SupplierID, UserID, TotalAmount, Status, Notes, PONumber
        )
        VALUES (
            GETDATE(), @SupplierID, @UserID, @TotalAmount, 'Pending', @Notes, @PONumber
        );
        
        SET @PurchaseOrderID = SCOPE_IDENTITY();
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Create a stored procedure to receive purchase order
CREATE PROCEDURE sp_ReceivePurchaseOrder
    @PurchaseOrderID INT,
    @UserID INT
AS
BEGIN
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Update purchase order status
        UPDATE PurchaseOrders
        SET Status = 'Received'
        WHERE PurchaseOrderID = @PurchaseOrderID;
        
        -- Get all purchase order details
        DECLARE @PurchaseOrderDetails TABLE (
            PurchaseOrderDetailID INT,
            ProductID INT,
            ReceivedQuantity INT
        );
        
        INSERT INTO @PurchaseOrderDetails
        SELECT PurchaseOrderDetailID, ProductID, ReceivedQuantity
        FROM PurchaseOrderDetails
        WHERE PurchaseOrderID = @PurchaseOrderID;
        
        -- Update stock for each product
        DECLARE @DetailID INT, @ProductID INT, @ReceivedQuantity INT;
        
        DECLARE detail_cursor CURSOR FOR
        SELECT PurchaseOrderDetailID, ProductID, ReceivedQuantity
        FROM @PurchaseOrderDetails;
        
        OPEN detail_cursor;
        FETCH NEXT FROM detail_cursor INTO @DetailID, @ProductID, @ReceivedQuantity;
        
        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Update product stock
            EXEC sp_UpdateProductStock 
                @ProductID = @ProductID,
                @Quantity = @ReceivedQuantity,
                @ReferenceType = 'PO',
                @ReferenceID = @PurchaseOrderID,
                @Notes = 'Purchase order received',
                @UserID = @UserID;
            
            FETCH NEXT FROM detail_cursor INTO @DetailID, @ProductID, @ReceivedQuantity;
        END
        
        CLOSE detail_cursor;
        DEALLOCATE detail_cursor;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO


CREATE PROCEDURE sp_AddPurchaseOrder
    @SupplierID INT,
    @OrderDate DATETIME,
    @Status NVARCHAR(50),
    @Notes NVARCHAR(MAX),
    @TotalAmount DECIMAL(18, 2),
    @NewPOID INT OUTPUT
AS
BEGIN
    INSERT INTO PurchaseOrders (SupplierID, OrderDate, Status, Notes, TotalAmount)
    VALUES (@SupplierID, @OrderDate, @Status, @Notes, @TotalAmount);

    SET @NewPOID = SCOPE_IDENTITY(); -- Ambil ID dari baris yang baru saja dimasukkan
END;

CREATE PROCEDURE sp_AddPurchaseOrderDetail
    @POID INT,
    @ProductID INT,
    @Quantity INT,
    @UnitPrice DECIMAL(18, 2)
AS
BEGIN
    DECLARE @Subtotal DECIMAL(18, 2)
    SET @Subtotal = @Quantity * @UnitPrice

    INSERT INTO PurchaseOrderDetails (POID, ProductID, Quantity, UnitPrice, Subtotal)
    VALUES (@POID, @ProductID, @Quantity, @UnitPrice, @Subtotal);
END;

ALTER TABLE Transactions ALTER COLUMN PaidAmount DECIMAL(18,2) NULL;
ALTER TABLE Transactions ALTER COLUMN ChangeAmount DECIMAL(18,2) NULL;
ALTER TABLE PurchaseOrders ADD CreatedDate DATETIME DEFAULT GETDATE();
ALTER TABLE PurchaseOrders
ADD PurchaseOrderNumber VARCHAR(50) NULL;
UPDATE PurchaseOrders
SET PurchaseOrderNumber = 'PO-' + CAST(PurchaseOrderID AS VARCHAR(10));
ALTER TABLE PurchaseOrders
ALTER COLUMN PurchaseOrderNumber VARCHAR(50) NOT NULL;

ALTER TABLE PurchaseOrders
ADD CONSTRAINT UQ_PurchaseOrderNumber UNIQUE (PurchaseOrderNumber);
-- Tambahkan kolom Stock jika belum ada
ALTER TABLE Products ADD Stock INT DEFAULT 0;

-- Opsional: tambahkan kolom untuk tracking update
ALTER TABLE Products ADD LastUpdated DATETIME DEFAULT GETDATE();
SELECT * FROM Transactions; 
ALTER TABLE Transactions
ADD CustomerName NVARCHAR(100) NULL;
UPDATE Transactions SET CustomerName = 'Alice' WHERE TransactionID = 4;
UPDATE Transactions SET CustomerName = 'Bob' WHERE TransactionID = 5;
UPDATE Transactions SET CustomerName = 'Charlie' WHERE TransactionID = 6;
UPDATE Transactions SET CustomerName = 'Diana' WHERE TransactionID = 7;
