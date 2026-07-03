/*
 Navicat Premium Data Transfer

 Source Server         : sqlserver
 Source Server Type    : SQL Server
 Source Server Version : 17001000
 Source Host           : DESKTOP-592USLC\SQLEXPRESS:1433
 Source Catalog        : cernamic_shop
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 17001000
 File Encoding         : 65001

 Date: 04/07/2026 02:54:42
*/


-- ----------------------------
-- Table structure for accounts
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[accounts]') AND type IN ('U'))
	DROP TABLE [dbo].[accounts]
GO

CREATE TABLE [dbo].[accounts] (
  [id] int  IDENTITY(1,1) NOT NULL,
  [uuid] char(36) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [username] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [password] varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [full_name] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [active] bit DEFAULT 1 NOT NULL,
  [created_at] datetime DEFAULT getdate() NOT NULL,
  [is_enable] bit DEFAULT 1 NOT NULL,
  [email] varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [phone] varchar(15) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL
)
GO

ALTER TABLE [dbo].[accounts] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of accounts
-- ----------------------------
SET IDENTITY_INSERT [dbo].[accounts] ON
GO

INSERT INTO [dbo].[accounts] ([id], [uuid], [username], [password], [full_name], [active], [created_at], [is_enable], [email], [phone]) VALUES (N'3', N'3aa84fe2-9854-4aeb-abf8-37d7dee43065', N'dev01', N'123123', N'toilatoi', N'1', N'2026-07-04 00:36:59.687', N'1', N'dev01@gmail.com', N'0334583920')
GO

SET IDENTITY_INSERT [dbo].[accounts] OFF
GO


-- ----------------------------
-- Table structure for categories
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[categories]') AND type IN ('U'))
	DROP TABLE [dbo].[categories]
GO

CREATE TABLE [dbo].[categories] (
  [id] int  IDENTITY(1,1) NOT NULL,
  [name] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [created_at] datetime DEFAULT getdate() NULL,
  [is_enable] bit DEFAULT 1 NULL
)
GO

ALTER TABLE [dbo].[categories] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of categories
-- ----------------------------
SET IDENTITY_INSERT [dbo].[categories] ON
GO

INSERT INTO [dbo].[categories] ([id], [name], [created_at], [is_enable]) VALUES (N'1', N'Bình gốm', N'2026-07-04 00:38:40.017', N'1')
GO

INSERT INTO [dbo].[categories] ([id], [name], [created_at], [is_enable]) VALUES (N'2', N'Bát cổ', N'2026-07-04 00:38:59.760', N'1')
GO

SET IDENTITY_INSERT [dbo].[categories] OFF
GO


-- ----------------------------
-- Table structure for images
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[images]') AND type IN ('U'))
	DROP TABLE [dbo].[images]
GO

CREATE TABLE [dbo].[images] (
  [id] int  IDENTITY(1,1) NOT NULL,
  [path] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [owner] nchar(36) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [uuid] nchar(36) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [created_at] datetime DEFAULT getdate() NOT NULL,
  [is_enable] bit DEFAULT 1 NOT NULL
)
GO

ALTER TABLE [dbo].[images] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of images
-- ----------------------------
SET IDENTITY_INSERT [dbo].[images] ON
GO

INSERT INTO [dbo].[images] ([id], [path], [owner], [uuid], [created_at], [is_enable]) VALUES (N'1', N'Resources/2026/07/39844511-e0c7-4750-b866-2fecdee4ddfe.png', N'product-1                           ', N'7fe95b12-5112-472f-898b-41eeb32c6cad', N'2026-07-04 01:24:21.923', N'1')
GO

SET IDENTITY_INSERT [dbo].[images] OFF
GO


-- ----------------------------
-- Table structure for order_detail
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[order_detail]') AND type IN ('U'))
	DROP TABLE [dbo].[order_detail]
GO

CREATE TABLE [dbo].[order_detail] (
  [id] int  IDENTITY(1,1) NOT NULL,
  [order_uuid] char(36) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [slug_product] varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [amount] decimal(12,2)  NOT NULL,
  [quantity] int  NOT NULL,
  [uuid] char(36) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [is_enable] bit DEFAULT 1 NOT NULL,
  [created_at] datetime DEFAULT getdate() NOT NULL
)
GO

ALTER TABLE [dbo].[order_detail] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'số tiền/1 sản phẩm ',
'SCHEMA', N'dbo',
'TABLE', N'order_detail',
'COLUMN', N'amount'
GO


-- ----------------------------
-- Records of order_detail
-- ----------------------------
SET IDENTITY_INSERT [dbo].[order_detail] ON
GO

INSERT INTO [dbo].[order_detail] ([id], [order_uuid], [slug_product], [amount], [quantity], [uuid], [is_enable], [created_at]) VALUES (N'1', N'9f837680-e198-4b47-95b4-8802c4accaa3', N'dia-gom-trang', N'120000.00', N'2', N'af9d0879-3cb1-47cb-9407-80e7823f6acb', N'1', N'2026-07-04 01:35:42.457')
GO

INSERT INTO [dbo].[order_detail] ([id], [order_uuid], [slug_product], [amount], [quantity], [uuid], [is_enable], [created_at]) VALUES (N'2', N'8ba86120-86a3-4cd7-8d49-0283af3da36d', N'dia-gom-trang', N'120000.00', N'2', N'97097ffa-8b2f-4734-a137-832121334a2f', N'1', N'2026-07-04 01:35:59.523')
GO

INSERT INTO [dbo].[order_detail] ([id], [order_uuid], [slug_product], [amount], [quantity], [uuid], [is_enable], [created_at]) VALUES (N'3', N'4604c514-29c0-482f-b280-068abb4d9b80', N'dia-gom-trang', N'120000.00', N'2', N'545ce524-d67c-4bef-af5f-04fba4271c79', N'1', N'2026-07-04 01:37:24.907')
GO

INSERT INTO [dbo].[order_detail] ([id], [order_uuid], [slug_product], [amount], [quantity], [uuid], [is_enable], [created_at]) VALUES (N'4', N'f6db040e-7114-484d-bc53-b727e2a12b50', N'dia-gom-trang', N'120000.00', N'2', N'5f32f93c-b84e-4798-9ec1-52f573bbbf23', N'1', N'2026-07-04 01:40:58.077')
GO

INSERT INTO [dbo].[order_detail] ([id], [order_uuid], [slug_product], [amount], [quantity], [uuid], [is_enable], [created_at]) VALUES (N'5', N'f6db040e-7114-484d-bc53-b727e2a12b50', N'product-1', N'100000.00', N'3', N'80be62ff-9bd7-47fa-9a7a-48d992264867', N'1', N'2026-07-04 01:40:58.083')
GO

INSERT INTO [dbo].[order_detail] ([id], [order_uuid], [slug_product], [amount], [quantity], [uuid], [is_enable], [created_at]) VALUES (N'6', N'8a777f43-8ca2-4117-b703-e4c37cc245be', N'dia-gom-trang', N'120000.00', N'2', N'2d3ef829-ecae-4237-8bf4-4139bd0ef073', N'1', N'2026-07-04 01:41:12.913')
GO

INSERT INTO [dbo].[order_detail] ([id], [order_uuid], [slug_product], [amount], [quantity], [uuid], [is_enable], [created_at]) VALUES (N'7', N'8a777f43-8ca2-4117-b703-e4c37cc245be', N'product-1', N'100000.00', N'3', N'bcf84b9b-4d1a-46b1-97ca-b2b494589c94', N'1', N'2026-07-04 01:41:12.920')
GO

SET IDENTITY_INSERT [dbo].[order_detail] OFF
GO


-- ----------------------------
-- Table structure for orders
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[orders]') AND type IN ('U'))
	DROP TABLE [dbo].[orders]
GO

CREATE TABLE [dbo].[orders] (
  [id] int  IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
  [uuid] char(36) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [total_amount] decimal(12,2) DEFAULT 0 NOT NULL,
  [state] tinyint DEFAULT 1 NOT NULL,
  [is_enable] bit DEFAULT 1 NOT NULL,
  [created_at] datetime DEFAULT getdate() NOT NULL,
  [phone_number] varchar(15) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [full_name] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [created_by] char(36) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL
)
GO

ALTER TABLE [dbo].[orders] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'1 chờ thanh toán, 2 đã thanh toán',
'SCHEMA', N'dbo',
'TABLE', N'orders',
'COLUMN', N'state'
GO


-- ----------------------------
-- Records of orders
-- ----------------------------
SET IDENTITY_INSERT [dbo].[orders] ON
GO

INSERT INTO [dbo].[orders] ([id], [uuid], [total_amount], [state], [is_enable], [created_at], [phone_number], [full_name], [created_by]) VALUES (N'1', N'936b1775-8c65-4234-a8bb-c9aa4dd7cb95', N'240000.00', N'2', N'1', N'2026-07-04 00:44:39.907', N'0987654321', N'Khách Hàng Test 1', N'3aa84fe2-9854-4aeb-abf8-37d7dee43065')
GO

INSERT INTO [dbo].[orders] ([id], [uuid], [total_amount], [state], [is_enable], [created_at], [phone_number], [full_name], [created_by]) VALUES (N'2', N'9f837680-e198-4b47-95b4-8802c4accaa3', N'240000.00', N'2', N'1', N'2026-07-04 01:35:42.210', N'0987654321', N'Khách Hàng Test 1', N'3aa84fe2-9854-4aeb-abf8-37d7dee43065')
GO

INSERT INTO [dbo].[orders] ([id], [uuid], [total_amount], [state], [is_enable], [created_at], [phone_number], [full_name], [created_by]) VALUES (N'3', N'8ba86120-86a3-4cd7-8d49-0283af3da36d', N'240000.00', N'2', N'1', N'2026-07-04 01:35:59.520', N'0987654321', N'Khách Hàng Test 1', N'3aa84fe2-9854-4aeb-abf8-37d7dee43065')
GO

INSERT INTO [dbo].[orders] ([id], [uuid], [total_amount], [state], [is_enable], [created_at], [phone_number], [full_name], [created_by]) VALUES (N'4', N'4604c514-29c0-482f-b280-068abb4d9b80', N'240000.00', N'2', N'1', N'2026-07-04 01:37:24.653', N'0333333333', N'Khách Hàng Test 1 - Updated', N'3aa84fe2-9854-4aeb-abf8-37d7dee43065')
GO

INSERT INTO [dbo].[orders] ([id], [uuid], [total_amount], [state], [is_enable], [created_at], [phone_number], [full_name], [created_by]) VALUES (N'5', N'f6db040e-7114-484d-bc53-b727e2a12b50', N'540000.00', N'2', N'1', N'2026-07-04 01:40:57.660', N'0334583920', N'Nguy?n Van Tr?ng', N'3aa84fe2-9854-4aeb-abf8-37d7dee43065')
GO

INSERT INTO [dbo].[orders] ([id], [uuid], [total_amount], [state], [is_enable], [created_at], [phone_number], [full_name], [created_by]) VALUES (N'6', N'8a777f43-8ca2-4117-b703-e4c37cc245be', N'540000.00', N'2', N'1', N'2026-07-04 01:41:12.903', N'0334583920', N'Nguy?n Van Tr?ng', N'3aa84fe2-9854-4aeb-abf8-37d7dee43065')
GO

SET IDENTITY_INSERT [dbo].[orders] OFF
GO


-- ----------------------------
-- Table structure for products
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[products]') AND type IN ('U'))
	DROP TABLE [dbo].[products]
GO

CREATE TABLE [dbo].[products] (
  [id] int  IDENTITY(1,1) NOT NULL,
  [slug] varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [name] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [categories_id] int  NOT NULL,
  [quantity] int DEFAULT 0 NOT NULL,
  [price] decimal(12,2) DEFAULT 0 NOT NULL,
  [is_enable] bit DEFAULT 1 NOT NULL,
  [created_at] datetime DEFAULT getdate() NOT NULL
)
GO

ALTER TABLE [dbo].[products] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of products
-- ----------------------------
SET IDENTITY_INSERT [dbo].[products] ON
GO

INSERT INTO [dbo].[products] ([id], [slug], [name], [categories_id], [quantity], [price], [is_enable], [created_at]) VALUES (N'1', N'dia-gom-trang', N'Đĩa gốm trắng', N'1', N'38', N'120000.00', N'1', N'2026-07-04 00:43:54.827')
GO

INSERT INTO [dbo].[products] ([id], [slug], [name], [categories_id], [quantity], [price], [is_enable], [created_at]) VALUES (N'2', N'product-1', N'Sản phẩm 1', N'2', N'4', N'100000.00', N'1', N'2026-07-04 01:28:55.287')
GO

SET IDENTITY_INSERT [dbo].[products] OFF
GO


-- ----------------------------
-- Auto increment value for accounts
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[accounts]', RESEED, 3)
GO


-- ----------------------------
-- Indexes structure for table accounts
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_accounts_username]
ON [dbo].[accounts] (
  [username] ASC
)
GO


-- ----------------------------
-- Uniques structure for table accounts
-- ----------------------------
ALTER TABLE [dbo].[accounts] ADD CONSTRAINT [UQ__accounts__7F4279316C17559F] UNIQUE NONCLUSTERED ([uuid] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table accounts
-- ----------------------------
ALTER TABLE [dbo].[accounts] ADD CONSTRAINT [PK__accounts__3213E83F5C943CF1] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for categories
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[categories]', RESEED, 2)
GO


-- ----------------------------
-- Primary Key structure for table categories
-- ----------------------------
ALTER TABLE [dbo].[categories] ADD CONSTRAINT [PK__categori__3213E83F54A0BD46] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for images
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[images]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table images
-- ----------------------------
ALTER TABLE [dbo].[images] ADD CONSTRAINT [PK_images] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for order_detail
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[order_detail]', RESEED, 7)
GO


-- ----------------------------
-- Indexes structure for table order_detail
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [idx_uq_uuid]
ON [dbo].[order_detail] (
  [uuid] ASC
)
GO

CREATE NONCLUSTERED INDEX [idx_created_at]
ON [dbo].[order_detail] (
  [created_at] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table order_detail
-- ----------------------------
ALTER TABLE [dbo].[order_detail] ADD CONSTRAINT [PK_order_detail] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for orders
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[orders]', RESEED, 6)
GO


-- ----------------------------
-- Indexes structure for table orders
-- ----------------------------
CREATE NONCLUSTERED INDEX [idx_created_at]
ON [dbo].[orders] (
  [created_at] ASC
)
GO

CREATE NONCLUSTERED INDEX [idx_state_isenable]
ON [dbo].[orders] (
  [state] ASC,
  [is_enable] ASC
)
GO

CREATE NONCLUSTERED INDEX [idx_phone]
ON [dbo].[orders] (
  [phone_number] ASC
)
GO


-- ----------------------------
-- Uniques structure for table orders
-- ----------------------------
ALTER TABLE [dbo].[orders] ADD CONSTRAINT [UQ__orders__7F427931C54DF2D1] UNIQUE NONCLUSTERED ([uuid] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table orders
-- ----------------------------
ALTER TABLE [dbo].[orders] ADD CONSTRAINT [PK__orders__3213E83F19E2405A] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for products
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[products]', RESEED, 2)
GO


-- ----------------------------
-- Indexes structure for table products
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_Products_Slug]
ON [dbo].[products] (
  [slug] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table products
-- ----------------------------
ALTER TABLE [dbo].[products] ADD CONSTRAINT [PK__products__3213E83F9247EF76] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Foreign Keys structure for table order_detail
-- ----------------------------
ALTER TABLE [dbo].[order_detail] ADD CONSTRAINT [FK__order_det__order__4D5F7D71] FOREIGN KEY ([order_uuid]) REFERENCES [dbo].[orders] ([uuid]) ON DELETE CASCADE ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[order_detail] ADD CONSTRAINT [FK__order_det__slug___4E53A1AA] FOREIGN KEY ([slug_product]) REFERENCES [dbo].[products] ([slug]) ON DELETE CASCADE ON UPDATE CASCADE
GO


-- ----------------------------
-- Foreign Keys structure for table orders
-- ----------------------------
ALTER TABLE [dbo].[orders] ADD CONSTRAINT [FK__orders__created___2739D489] FOREIGN KEY ([created_by]) REFERENCES [dbo].[accounts] ([uuid]) ON DELETE CASCADE ON UPDATE CASCADE
GO


-- ----------------------------
-- Foreign Keys structure for table products
-- ----------------------------
ALTER TABLE [dbo].[products] ADD CONSTRAINT [FK_Products_Categories] FOREIGN KEY ([categories_id]) REFERENCES [dbo].[categories] ([id]) ON DELETE CASCADE ON UPDATE NO ACTION
GO

