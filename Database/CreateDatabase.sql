CREATE DATABASE ShopLocalDb;
GO

USE ShopLocalDb;
GO

CREATE TABLE Users (
    UserNum INT IDENTITY(1,1) NOT NULL,
    Id AS ('TK' + RIGHT('000' + CAST(UserNum AS VARCHAR(10)), 3)) PERSISTED PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NOT NULL
);
GO

CREATE TABLE Products (
    ProductNum INT IDENTITY(1,1) NOT NULL,
    Id AS ('HH' + RIGHT('000' + CAST(ProductNum AS VARCHAR(10)), 3)) PERSISTED PRIMARY KEY,
    ProductName NVARCHAR(50) NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    Price INT NOT NULL,
    Stock INT NOT NULL,
);
GO

INSERT INTO dbo.Products (ProductName, Category, Price, Stock)
VALUES
    (N'Áo thun basic', N'Thời trang', 189000, 42),
    (N'Quần jeans slim', N'Thời trang', 349000, 8),
    (N'Bình nước thể thao', N'Phụ kiện', 129000, 0),
    (N'Túi tote canvas', N'Phụ kiện', 159000, 16),
    (N'Sổ tay da', N'Văn phòng phẩm', 99000, 7),
    (N'Bút gel đen', N'Văn phòng phẩm', 25000, 85),
    (N'Mũ lưỡi trai', N'Thời trang', 149000, 24),
    (N'Ví da mini', N'Phụ kiện', 219000, 5);
GO