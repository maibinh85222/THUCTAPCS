USE [master]
GO
/****** Object:  Database [CHIENDICHMUAHE]    Script Date: 7/10/2023 1:38:09 PM ******/
CREATE DATABASE [CHIENDICHMUAHE]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CHIENDICHMUAHE', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.THANHBINH\MSSQL\DATA\CHIENDICHMUAHE.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CHIENDICHMUAHE_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.THANHBINH\MSSQL\DATA\CHIENDICHMUAHE_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [CHIENDICHMUAHE] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CHIENDICHMUAHE].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CHIENDICHMUAHE] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET ARITHABORT OFF 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET RECOVERY FULL 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET  MULTI_USER 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CHIENDICHMUAHE] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CHIENDICHMUAHE] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'CHIENDICHMUAHE', N'ON'
GO
ALTER DATABASE [CHIENDICHMUAHE] SET QUERY_STORE = ON
GO
ALTER DATABASE [CHIENDICHMUAHE] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [CHIENDICHMUAHE]
GO
/****** Object:  User [SV002]    Script Date: 7/10/2023 1:38:09 PM ******/
CREATE USER [SV002] FOR LOGIN [SV002] WITH DEFAULT_SCHEMA=[SV002]
GO
/****** Object:  User [SV001]    Script Date: 7/10/2023 1:38:09 PM ******/
CREATE USER [SV001] FOR LOGIN [SV001] WITH DEFAULT_SCHEMA=[SV001]
GO
/****** Object:  User [sds]    Script Date: 7/10/2023 1:38:09 PM ******/
CREATE USER [sds] FOR LOGIN [sadsd] WITH DEFAULT_SCHEMA=[sds]
GO
/****** Object:  User [GV003]    Script Date: 7/10/2023 1:38:09 PM ******/
CREATE USER [GV003] FOR LOGIN [gv3] WITH DEFAULT_SCHEMA=[GV003]
GO
/****** Object:  User [GV002]    Script Date: 7/10/2023 1:38:09 PM ******/
CREATE USER [GV002] FOR LOGIN [gv2] WITH DEFAULT_SCHEMA=[GV002]
GO
/****** Object:  DatabaseRole [TRUONG]    Script Date: 7/10/2023 1:38:09 PM ******/
CREATE ROLE [TRUONG]
GO
/****** Object:  DatabaseRole [SINHVIEN]    Script Date: 7/10/2023 1:38:09 PM ******/
CREATE ROLE [SINHVIEN]
GO
/****** Object:  DatabaseRole [GIANGVIEN]    Script Date: 7/10/2023 1:38:09 PM ******/
CREATE ROLE [GIANGVIEN]
GO
ALTER ROLE [SINHVIEN] ADD MEMBER [SV002]
GO
ALTER ROLE [db_owner] ADD MEMBER [SV002]
GO
ALTER ROLE [SINHVIEN] ADD MEMBER [SV001]
GO
ALTER ROLE [db_owner] ADD MEMBER [SV001]
GO
ALTER ROLE [TRUONG] ADD MEMBER [sds]
GO
ALTER ROLE [db_owner] ADD MEMBER [sds]
GO
ALTER ROLE [GIANGVIEN] ADD MEMBER [GV003]
GO
ALTER ROLE [db_owner] ADD MEMBER [GV003]
GO
ALTER ROLE [TRUONG] ADD MEMBER [GV002]
GO
ALTER ROLE [db_owner] ADD MEMBER [GV002]
GO
ALTER ROLE [db_owner] ADD MEMBER [TRUONG]
GO
ALTER ROLE [db_owner] ADD MEMBER [SINHVIEN]
GO
ALTER ROLE [db_owner] ADD MEMBER [GIANGVIEN]
GO
/****** Object:  Schema [GV001]    Script Date: 7/10/2023 1:38:10 PM ******/
CREATE SCHEMA [GV001]
GO
/****** Object:  Schema [GV002]    Script Date: 7/10/2023 1:38:10 PM ******/
CREATE SCHEMA [GV002]
GO
/****** Object:  Schema [GV003]    Script Date: 7/10/2023 1:38:10 PM ******/
CREATE SCHEMA [GV003]
GO
/****** Object:  Schema [sds]    Script Date: 7/10/2023 1:38:10 PM ******/
CREATE SCHEMA [sds]
GO
/****** Object:  Schema [SV001]    Script Date: 7/10/2023 1:38:10 PM ******/
CREATE SCHEMA [SV001]
GO
/****** Object:  Schema [SV002]    Script Date: 7/10/2023 1:38:10 PM ******/
CREATE SCHEMA [SV002]
GO
/****** Object:  Table [dbo].[NHOMTHUCHIEN]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NHOMTHUCHIEN](
	[BuoiNgay] [varchar](30) NOT NULL,
	[Nhom] [varchar](5) NOT NULL,
	[MaCV] [varchar](5) NOT NULL,
 CONSTRAINT [PK_NHOMTHUCHIEN] PRIMARY KEY CLUSTERED 
(
	[BuoiNgay] ASC,
	[Nhom] ASC,
	[MaCV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CONGVIEC]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CONGVIEC](
	[MaCV] [varchar](5) NOT NULL,
	[TenCV] [nvarchar](50) NOT NULL,
	[MaAp] [varchar](5) NOT NULL,
	[Cong] [int] NOT NULL,
	[NgayBd] [date] NOT NULL,
	[NgayKt] [date] NOT NULL,
 CONSTRAINT [PK_CONGVIEC] PRIMARY KEY CLUSTERED 
(
	[MaCV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SINHVIEN]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SINHVIEN](
	[MaSV] [varchar](5) NOT NULL,
	[TenSV] [nvarchar](50) NOT NULL,
	[MaKhoa] [varchar](5) NOT NULL,
	[MaNhom] [varchar](5) NULL,
	[ChucVu] [nvarchar](10) NULL,
	[MaDoiGiamSat] [varchar](5) NULL,
 CONSTRAINT [PK_SINHVIEN] PRIMARY KEY CLUSTERED 
(
	[MaSV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[LayDS_CV_NHOM]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[LayDS_CV_NHOM] (@MASV varchar(5))
RETURNS TABLE
AS
RETURN
    SELECT CONGVIEC.MaCV, TenCV, MaAp, CONGVIEC.Cong, CONGVIEC.NgayBd, CONGVIEC.NgayKt
    FROM NHOMTHUCHIEN, CONGVIEC
    WHERE Nhom = (select MaNhom from SINHVIEN where MaSV = @MASV) and NHOMTHUCHIEN.MaCV = CONGVIEC.MaCV
GO
/****** Object:  Table [dbo].[NHOM]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NHOM](
	[MaNhom] [varchar](5) NOT NULL,
	[TenNhom] [nvarchar](50) NOT NULL,
	[SoLuongSV] [int] NOT NULL,
	[MaTruongNhom] [varchar](5) NULL,
	[MaNha] [varchar](5) NULL,
 CONSTRAINT [PK_NHOM] PRIMARY KEY CLUSTERED 
(
	[MaNhom] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AP]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AP](
	[MaAp] [varchar](5) NOT NULL,
	[TenAp] [nvarchar](50) NOT NULL,
	[MaXa] [varchar](5) NOT NULL,
 CONSTRAINT [PK_AP] PRIMARY KEY CLUSTERED 
(
	[MaAp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NHA]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NHA](
	[MaNha] [varchar](5) NOT NULL,
	[TenNha] [nvarchar](50) NOT NULL,
	[MaAp] [varchar](5) NOT NULL,
	[MaNhom] [varchar](5) NULL,
 CONSTRAINT [PK_NHA] PRIMARY KEY CLUSTERED 
(
	[MaNha] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DOIGIAMSAT]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DOIGIAMSAT](
	[MaDoiGiamSat] [varchar](5) NOT NULL,
	[MaDoiTruong] [varchar](5) NULL,
	[MaDoiPho] [varchar](5) NULL,
	[TenDoiGiamSat] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_DOIGIAMSAT] PRIMARY KEY CLUSTERED 
(
	[MaDoiGiamSat] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[XA]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[XA](
	[MaXa] [varchar](5) NOT NULL,
	[TenXa] [nchar](50) NOT NULL,
	[MaDiaBan] [varchar](5) NOT NULL,
	[MaDoiGiamSat] [varchar](5) NULL,
 CONSTRAINT [PK_XA] PRIMARY KEY CLUSTERED 
(
	[MaXa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[LayDS_DGS_NHOM]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[LayDS_DGS_NHOM] (@MASV varchar(5))
RETURNS TABLE
AS
RETURN
    select * from DOIGIAMSAT where MaDoiGiamSat = 
	(select MaDoiGiamSat from XA where MaXa = 
	(select MaXa from AP where MaAp = 
	(select MaAp from NHA where MaNha = 
	(select MaNha from NHOM where MaNhom = 
	(select MaNhom from SINHVIEN where MaSV = @MASV)))))
GO
/****** Object:  UserDefinedFunction [dbo].[LayDS_NHOMTHUCHIEN_NHOM]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[LayDS_NHOMTHUCHIEN_NHOM] (@MASV varchar(5))
RETURNS TABLE
AS
RETURN
    select *from NHOMTHUCHIEN where Nhom = (select MaNhom from SINHVIEN where MaSV = @MASV)
GO
/****** Object:  Table [dbo].[BUOI]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BUOI](
	[BuoiNgay] [varchar](30) NOT NULL,
	[Buoi] [varchar](10) NOT NULL,
	[Ngay] [date] NOT NULL,
 CONSTRAINT [PK_BUOI_1] PRIMARY KEY CLUSTERED 
(
	[BuoiNgay] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[LayDS_Buoi_NHOM]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[LayDS_Buoi_NHOM] (@MASV varchar(5))
RETURNS TABLE
AS
RETURN
	select Buoi.BuoiNgay, Buoi.Buoi, Buoi.Ngay
	from NHOMTHUCHIEN, BUOI
	where NHOMTHUCHIEN.Nhom = (select MaNhom from SINHVIEN where MaSV = @MASV) and NHOMTHUCHIEN.BuoiNgay = BUOI.BuoiNgay
GO
/****** Object:  UserDefinedFunction [dbo].[LAY_CV_THUOC_BUOI]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[LAY_CV_THUOC_BUOI](@BUOINGAY varchar(30), @MACV varchar(5))
RETURNS TABLE
AS
RETURN
	select Ngay , CV.NgayBd  , CV.NgayKt
	from BUOI as  B, CONGVIEC as CV
	where BuoiNgay = @BUOINGAY and CV.MaCV = @MACV and B.Ngay >= CV.NgayBd and B.Ngay <= CV.NgayKt
GO
/****** Object:  Table [dbo].[DIABAN]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DIABAN](
	[MaDiaBan] [varchar](5) NOT NULL,
	[TenDiaBan] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_DIABAN] PRIMARY KEY CLUSTERED 
(
	[MaDiaBan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GIANGVIEN]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GIANGVIEN](
	[MaGV] [varchar](5) NOT NULL,
	[TenGV] [nvarchar](50) NOT NULL,
	[MaKhoa] [varchar](5) NOT NULL,
	[MaDoiGiamSat] [varchar](5) NULL,
 CONSTRAINT [PK_GIANGVIEN] PRIMARY KEY CLUSTERED 
(
	[MaGV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KHENTHUONG]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KHENTHUONG](
	[MaKT] [varchar](5) NOT NULL,
	[NoiDungKT] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_KHENTHUONG] PRIMARY KEY CLUSTERED 
(
	[MaKT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KHOA]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KHOA](
	[MaKhoa] [varchar](5) NOT NULL,
	[TenKhoa] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_KHOA] PRIMARY KEY CLUSTERED 
(
	[MaKhoa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SV_KT]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SV_KT](
	[MaKT] [varchar](5) NOT NULL,
	[MaSV] [varchar](5) NOT NULL,
	[Ngay] [date] NOT NULL,
 CONSTRAINT [PK_SV_KT] PRIMARY KEY CLUSTERED 
(
	[MaKT] ASC,
	[MaSV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AP]  WITH CHECK ADD  CONSTRAINT [FK_AP_XA] FOREIGN KEY([MaXa])
REFERENCES [dbo].[XA] ([MaXa])
GO
ALTER TABLE [dbo].[AP] CHECK CONSTRAINT [FK_AP_XA]
GO
ALTER TABLE [dbo].[CONGVIEC]  WITH CHECK ADD  CONSTRAINT [FK_CONGVIEC_AP] FOREIGN KEY([MaAp])
REFERENCES [dbo].[AP] ([MaAp])
GO
ALTER TABLE [dbo].[CONGVIEC] CHECK CONSTRAINT [FK_CONGVIEC_AP]
GO
ALTER TABLE [dbo].[DOIGIAMSAT]  WITH CHECK ADD  CONSTRAINT [FK_DOIGIAMSAT_SINHVIEN] FOREIGN KEY([MaDoiTruong])
REFERENCES [dbo].[SINHVIEN] ([MaSV])
GO
ALTER TABLE [dbo].[DOIGIAMSAT] CHECK CONSTRAINT [FK_DOIGIAMSAT_SINHVIEN]
GO
ALTER TABLE [dbo].[DOIGIAMSAT]  WITH CHECK ADD  CONSTRAINT [FK_DOIGIAMSAT_SINHVIEN1] FOREIGN KEY([MaDoiPho])
REFERENCES [dbo].[SINHVIEN] ([MaSV])
GO
ALTER TABLE [dbo].[DOIGIAMSAT] CHECK CONSTRAINT [FK_DOIGIAMSAT_SINHVIEN1]
GO
ALTER TABLE [dbo].[GIANGVIEN]  WITH CHECK ADD  CONSTRAINT [FK_GIANGVIEN_DOIGIAMSAT] FOREIGN KEY([MaDoiGiamSat])
REFERENCES [dbo].[DOIGIAMSAT] ([MaDoiGiamSat])
GO
ALTER TABLE [dbo].[GIANGVIEN] CHECK CONSTRAINT [FK_GIANGVIEN_DOIGIAMSAT]
GO
ALTER TABLE [dbo].[GIANGVIEN]  WITH CHECK ADD  CONSTRAINT [FK_GIANGVIEN_KHOA] FOREIGN KEY([MaKhoa])
REFERENCES [dbo].[KHOA] ([MaKhoa])
GO
ALTER TABLE [dbo].[GIANGVIEN] CHECK CONSTRAINT [FK_GIANGVIEN_KHOA]
GO
ALTER TABLE [dbo].[NHA]  WITH CHECK ADD  CONSTRAINT [FK_NHA_AP] FOREIGN KEY([MaAp])
REFERENCES [dbo].[AP] ([MaAp])
GO
ALTER TABLE [dbo].[NHA] CHECK CONSTRAINT [FK_NHA_AP]
GO
ALTER TABLE [dbo].[NHOM]  WITH CHECK ADD  CONSTRAINT [FK_NHOM_NHA] FOREIGN KEY([MaNha])
REFERENCES [dbo].[NHA] ([MaNha])
GO
ALTER TABLE [dbo].[NHOM] CHECK CONSTRAINT [FK_NHOM_NHA]
GO
ALTER TABLE [dbo].[NHOM]  WITH CHECK ADD  CONSTRAINT [FK_NHOM_SINHVIEN] FOREIGN KEY([MaTruongNhom])
REFERENCES [dbo].[SINHVIEN] ([MaSV])
GO
ALTER TABLE [dbo].[NHOM] CHECK CONSTRAINT [FK_NHOM_SINHVIEN]
GO
ALTER TABLE [dbo].[NHOMTHUCHIEN]  WITH CHECK ADD  CONSTRAINT [FK_NHOMTHUCHIEN_BUOI] FOREIGN KEY([BuoiNgay])
REFERENCES [dbo].[BUOI] ([BuoiNgay])
GO
ALTER TABLE [dbo].[NHOMTHUCHIEN] CHECK CONSTRAINT [FK_NHOMTHUCHIEN_BUOI]
GO
ALTER TABLE [dbo].[NHOMTHUCHIEN]  WITH CHECK ADD  CONSTRAINT [FK_NHOMTHUCHIEN_CONGVIEC] FOREIGN KEY([MaCV])
REFERENCES [dbo].[CONGVIEC] ([MaCV])
GO
ALTER TABLE [dbo].[NHOMTHUCHIEN] CHECK CONSTRAINT [FK_NHOMTHUCHIEN_CONGVIEC]
GO
ALTER TABLE [dbo].[NHOMTHUCHIEN]  WITH CHECK ADD  CONSTRAINT [FK_NHOMTHUCHIEN_NHOM] FOREIGN KEY([Nhom])
REFERENCES [dbo].[NHOM] ([MaNhom])
GO
ALTER TABLE [dbo].[NHOMTHUCHIEN] CHECK CONSTRAINT [FK_NHOMTHUCHIEN_NHOM]
GO
ALTER TABLE [dbo].[SINHVIEN]  WITH CHECK ADD  CONSTRAINT [FK_SINHVIEN_DOIGIAMSAT] FOREIGN KEY([MaDoiGiamSat])
REFERENCES [dbo].[DOIGIAMSAT] ([MaDoiGiamSat])
GO
ALTER TABLE [dbo].[SINHVIEN] CHECK CONSTRAINT [FK_SINHVIEN_DOIGIAMSAT]
GO
ALTER TABLE [dbo].[SINHVIEN]  WITH CHECK ADD  CONSTRAINT [FK_SINHVIEN_KHOA] FOREIGN KEY([MaKhoa])
REFERENCES [dbo].[KHOA] ([MaKhoa])
GO
ALTER TABLE [dbo].[SINHVIEN] CHECK CONSTRAINT [FK_SINHVIEN_KHOA]
GO
ALTER TABLE [dbo].[SINHVIEN]  WITH CHECK ADD  CONSTRAINT [FK_SINHVIEN_NHOM] FOREIGN KEY([MaNhom])
REFERENCES [dbo].[NHOM] ([MaNhom])
GO
ALTER TABLE [dbo].[SINHVIEN] CHECK CONSTRAINT [FK_SINHVIEN_NHOM]
GO
ALTER TABLE [dbo].[SV_KT]  WITH CHECK ADD  CONSTRAINT [FK_SV_KT_KHENTHUONG] FOREIGN KEY([MaKT])
REFERENCES [dbo].[KHENTHUONG] ([MaKT])
GO
ALTER TABLE [dbo].[SV_KT] CHECK CONSTRAINT [FK_SV_KT_KHENTHUONG]
GO
ALTER TABLE [dbo].[SV_KT]  WITH CHECK ADD  CONSTRAINT [FK_SV_KT_SINHVIEN] FOREIGN KEY([MaSV])
REFERENCES [dbo].[SINHVIEN] ([MaSV])
GO
ALTER TABLE [dbo].[SV_KT] CHECK CONSTRAINT [FK_SV_KT_SINHVIEN]
GO
ALTER TABLE [dbo].[XA]  WITH CHECK ADD  CONSTRAINT [FK_XA_DIABAN] FOREIGN KEY([MaDiaBan])
REFERENCES [dbo].[DIABAN] ([MaDiaBan])
GO
ALTER TABLE [dbo].[XA] CHECK CONSTRAINT [FK_XA_DIABAN]
GO
ALTER TABLE [dbo].[XA]  WITH CHECK ADD  CONSTRAINT [FK_XA_DOIGIAMSAT] FOREIGN KEY([MaDoiGiamSat])
REFERENCES [dbo].[DOIGIAMSAT] ([MaDoiGiamSat])
GO
ALTER TABLE [dbo].[XA] CHECK CONSTRAINT [FK_XA_DOIGIAMSAT]
GO
/****** Object:  StoredProcedure [dbo].[AddSinhVien]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[AddSinhVien]
	@MASV varchar(5),
	@TENSV nvarchar(50),
	@MAKHOA varchar(5),
	@MANHOM varchar(5),
	@CHUCVU nvarchar(10),
	@MADOIGIAMSAT varchar(5)
as
	insert into SINHVIEN(MaSV,TenSV,MaKhoa,MaNhom,ChucVu,MaDoiGiamSat)
	values(@MASV,@TENSV,@MAKHOA,@MANHOM,@CHUCVU,@MADOIGIAMSAT)
GO
/****** Object:  StoredProcedure [dbo].[DeleteSinhVien]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[DeleteSinhVien]
	@MASV varchar(5)
as
	delete SINHVIEN
	where MaSV = @MASV
GO
/****** Object:  StoredProcedure [dbo].[EditSinhVien]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[EditSinhVien]
	@MASV varchar(5),
	@TENSV nvarchar(50),
	@MANHOM varchar(5)
as
	update SINHVIEN
	set TenSV = @TENSV, MaNhom = @MANHOM
	where MaSV = @MASV
GO
/****** Object:  StoredProcedure [dbo].[Lay_Thong_Tin_Tu_Log_In]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[Lay_Thong_Tin_Tu_Log_In]
	@TenLogIn varchar(100)
as
	declare @UID int
	declare @MaNV char(5)

	select @UID = uid, @MaNV =name
	from sys.sysusers 
	where sid=SUSER_ID(@TenLogIn)

	select MaNV = @MaNV, HOTEN = (select TenGV from GIANGVIEN where MaGV = @MaNV), TENNHOM = name
	from sys.sysusers
	where uid = (select groupuid from sys.sysmembers where memberuid = @UID)
GO
/****** Object:  StoredProcedure [dbo].[sp_Add_SV_KT]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_Add_SV_KT]
	@MAKT varchar(5),
	@MASV varchar(5),
	@NGAY date
as
	insert into SV_KT(MaKT,MaSV,Ngay)
	values(@MAKT,@MASV,@NGAY)
GO
/****** Object:  StoredProcedure [dbo].[sp_AddAp]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_AddAp]
	@MAAP varchar(5),
	@TENAP nvarchar(50),
	@MAXA varchar(5)
as
	insert into AP(MaAp,TenAp,MaXa)
	values(@MAAP,@TENAP,@MAXA)
GO
/****** Object:  StoredProcedure [dbo].[sp_AddBuoi]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_AddBuoi]
	@BUOINGAY varchar(30),
	@BUOI varchar(10),
	@NGAY date
as
	insert into BUOI(BuoiNgay,Buoi,Ngay)
	values(@BUOINGAY,@BUOI,@NGAY)
GO
/****** Object:  StoredProcedure [dbo].[sp_AddCV]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_AddCV]
	@MACV varchar(5),
	@TENCV nvarchar(50),
	@MAAP varchar(5),
	@CONG int,
	@NGAYBD date,
	@NGAYKT date
as
	insert into CONGVIEC(MaCV,TenCV,MaAp,Cong,NgayBd,NgayKt)
	values(@MACV,@TENCV,@MAAP,@CONG,@NGAYBD,@NGAYKT)
GO
/****** Object:  StoredProcedure [dbo].[sp_AddDoiGiamSat]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_AddDoiGiamSat]
	@MADGS varchar(5),
	@MADOITRUONG varchar(5),
	@MADOIPHO varchar(5),
	@TENDOIGIAMSAT nvarchar(50)
as
	insert into DOIGIAMSAT(MaDoiGiamSat,MaDoiTruong,MaDoiPho,TenDoiGiamSat)
	values(@MADGS,@MADOITRUONG,@MADOIPHO,@TENDOIGIAMSAT)
GO
/****** Object:  StoredProcedure [dbo].[sp_AddNha]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_AddNha]
	@MANHA varchar(5),
	@TENNHA nvarchar(50),
	@MAAP varchar(5),
	@MANHOM varchar(5)

as
	insert into NHA(MaNha,TenNha,MaAp,MaNhom)
	values(@MANHA,@TENNHA,@MAAP,@MANHOM)
GO
/****** Object:  StoredProcedure [dbo].[sp_AddNhom]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_AddNhom]
	@MANHOM varchar(5),
	@TENNHOM nvarchar(50),
	@SOLUONG int,
	@MATRUONGNHOM varchar(5),
	@MANHA varchar(5)
as
	insert into NHOM(MaNhom,TenNhom,SoLuongSV,MaTruongNhom,MaNha)
	values(@MANHOM,@TENNHOM,@SOLUONG,@MATRUONGNHOM,@MANHA)
GO
/****** Object:  StoredProcedure [dbo].[sp_AddNhomThucHien]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_AddNhomThucHien]
	@BUOINGAY varchar(30),
	@NHOM varchar(5),
	@MACV varchar(5)
as
	insert into NHOMTHUCHIEN(BuoiNgay,Nhom,MaCV)
	values(@BUOINGAY,@NHOM,@MACV)
GO
/****** Object:  StoredProcedure [dbo].[sp_AddXa]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_AddXa]
	@MAXA VARCHAR(5),
	@TENXA NCHAR(50),
	@MADIABAN VARCHAR(5),
	@MADGS VARCHAR(5)
AS
	INSERT INTO XA(MaXa,TenXa,MaDiaBan,MaDoiGiamSat)
	VALUES (@MAXA,@TENXA,@MADIABAN,@MADGS)
GO
/****** Object:  StoredProcedure [dbo].[sp_Delete_SV_KT]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_Delete_SV_KT]
	@MAKT varchar(5),
	@MASV varchar(5),
	@NGAY date
as
	delete SV_KT
	where MaKT= @MAKT and MaSV = @MASV and Ngay = @NGAY
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteAp]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_DeleteAp]
	@MAAP varchar(5)
as
	delete AP
	where MaAp = @MAAP
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteBuoi]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_DeleteBuoi]
	@BUOINGAY varchar(30)
as
	delete BUOI
	where BuoiNgay = @BUOINGAY
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteCV]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_DeleteCV]
	@MACV varchar(5)
as
	delete CONGVIEC
	where MaCV = @MACV
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteDiaBan]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_DeleteDiaBan]
	@MADIABAN varchar(5)
as
	delete from DIABAN
	where MaDiaBan = @MADIABAN
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteDoiGiamSat]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_DeleteDoiGiamSat]
	@MADGS varchar(5)
as
	delete DOIGIAMSAT
	where MaDoiGiamSat = @MADGS 
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteGiangVien]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create proc [dbo].[sp_DeleteGiangVien]
	@MAGV varchar(5)
as
	delete from GIANGVIEN
	where MaGV = @MAGV
	
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteKhoa]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_DeleteKhoa]
	@MAKHOA varchar(5)
as
	delete from KHOA
	where MaKhoa = @MAKHOA
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteKT]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_DeleteKT]
	@MAKT varchar(5)
as
	delete from KHENTHUONG
	where MaKT = @MAKT
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteNha]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_DeleteNha]
	@MANHA varchar(5),
	@MAAP varchar(5)
as
	delete NHA
	where MaNha= @MANHA and MaAp = @MAAP
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteNhom]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_DeleteNhom]
	@MANHOM varchar(5)
as
	delete NHOM
	where MaNhom = @MANHOM
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteXa]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create proc [dbo].[sp_DeleteXa]
	@MAXA VARCHAR(5)
	--@MADIABAN VARCHAR(5),
	--@MADGS VARCHAR(5)
AS
	delete XA
	where MaXa = @MAXA
GO
/****** Object:  StoredProcedure [dbo].[sp_EditCV]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_EditCV]
	@MACV varchar(5),
	@TENCV nvarchar(50),
	@CONG int,
	@NGAYBD date,
	@NGAYKT date
as
	update CONGVIEC
	set TenCV = @TENCV, Cong = @CONG, NgayBd = @NGAYBD, NgayKt = @NGAYKT
	where MaCV = @MACV
GO
/****** Object:  StoredProcedure [dbo].[sp_EditDoiGiamSat]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_EditDoiGiamSat]
	@MADGS varchar(5),
	@MADOITRUONG varchar(5),
	@MADOIPHO varchar(5),
	@TENDOIGIAMSAT nvarchar(50)
as
	update DOIGIAMSAT
	set MaDoiTruong = @MADOITRUONG, MaDoiPho = @MADOIPHO, TenDoiGiamSat = @TENDOIGIAMSAT
	where MaDoiGiamSat = @MADGS
GO
/****** Object:  StoredProcedure [dbo].[sp_EditNhom]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_EditNhom]
	@MANHOM varchar(5),
	@TENNHOM nvarchar(50),
	@SOLUONG int,
	@MATRUONGNHOM varchar(5),
	@MANHA varchar(5)
	--@MANHA varchar(5)
	--@MANHA varchar(5).... cho sửa mã trưởng nhóm điều kiện mã sinh viên phải là sinh trong nhóm
as
	update NHOM
	set TenNhom = @TENNHOM, SoLuongSV = @SOLUONG, MaTruongNhom = @MATRUONGNHOM, MaNha = @MANHA
	where MaNhom = @MANHOM
GO
/****** Object:  StoredProcedure [dbo].[sp_EditXa]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_EditXa]
	@MAXA VARCHAR(5),
	@TENXA NCHAR(50),
	--@MADIABAN VARCHAR(5),
	@MADGS VARCHAR(5)
AS
	update XA
	set TenXa = @TENXA, MaDoiGiamSat = @MADGS
	where MaXa = @MAXA
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertDiaBan]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_InsertDiaBan]
	@MADIABAN varchar(5),
	@TENDIABAN nvarchar(50)
as
	insert into DIABAN(MaDiaBan,TenDiaBan)
	values (@MADIABAN,@TENDIABAN)
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertGiangVien]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_InsertGiangVien]
	@MAGV varchar(5),
	@TENGV nvarchar(50),
	@MAKHOA varchar(5),
	@MADOIGIAMSAT VARCHAR(5)
as
	insert into GIANGVIEN(MaGV,TenGV,MaKhoa,MaDoiGiamSat)
	values (@MAGV,@TENGV,@MAKHOA,@MADOIGIAMSAT)
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertKhoa]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_InsertKhoa]
	@MAKHOA varchar(5),
	@TENKHOA nvarchar(50)
as
	insert into KHOA(MaKhoa, TenKhoa)
	values (@MAKHOA,@TENKHOA)
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertKT]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_InsertKT]
	@MAKT varchar(5),
	@NOIDUNG nvarchar(50)
as
	insert into KHENTHUONG(MaKT,NoiDungKT)
	values (@MAKT,@NOIDUNG)
GO
/****** Object:  StoredProcedure [dbo].[sp_KTBuoiTonTai]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_KTBuoiTonTai]
	@BUOI varchar(10),
	@NGAY date
as
	select *
	from BUOI
	where Buoi = @BUOI and Ngay = @NGAY
GO
/****** Object:  StoredProcedure [dbo].[SP_LayThongTinGiaoVien]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_LayThongTinGiaoVien]
@TENLOGIN NVARCHAR( 100)
AS
DECLARE @UID INT
DECLARE @MANV NVARCHAR(100)

SELECT @UID= UID , @MANV= NAME FROM SYS.SYSUSERS 
  WHERE SID = SUSER_SID(@TENLOGIN)
IF NOT EXISTS(SELECT * FROM DBO.GIANGVIEN WHERE MaGV=@MANV )
	BEGIN
			RAISERROR ('GIẢNG VIÊN KHÔNG TỒN TẠI !!',16,1)
			RETURN 
	END
SELECT MAGV= @MANV, 
       HOTEN = (SELECT TenGV FROM DBO.GIANGVIEN WHERE MaGV=@MANV ), 
       TENNHOM=NAME
  FROM SYS.SYSUSERS
    WHERE UID = (SELECT GROUPUID FROM SYS.SYSMEMBERS WHERE MEMBERUID=@UID)
GO
/****** Object:  StoredProcedure [dbo].[SP_LayThongTinSinhVien]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_LayThongTinSinhVien]
@TENLOGIN NVARCHAR( 100)
AS
DECLARE @UID INT
DECLARE @MANV NVARCHAR(100)

SELECT @UID= UID , @MANV= NAME FROM SYS.SYSUSERS 
  WHERE SID = SUSER_SID(@TENLOGIN)
IF NOT EXISTS(SELECT * FROM DBO.SINHVIEN WHERE MaSV=@MANV )
	BEGIN
			RAISERROR ('GIẢNG VIÊN KHÔNG TỒN TẠI !!',16,1)
			RETURN 
	END
SELECT MAGV= @MANV, 
       HOTEN = (SELECT TenSV FROM DBO.SINHVIEN WHERE MaSV=@MANV ), 
       TENNHOM=NAME
  FROM SYS.SYSUSERS
    WHERE UID = (SELECT GROUPUID FROM SYS.SYSMEMBERS WHERE MEMBERUID=@UID)
GO
/****** Object:  StoredProcedure [dbo].[sp_LogIn]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_LogIn]
	@LoginName varchar(50),
	@Password varchar(50),
	@UserName varchar(50),
	@Role varchar(50)
as
	declare @Ret int
	exec @Ret = sp_addlogin @LoginName, @Password, 'CHIENDICHMUAHE'
	if(@Ret = 1)--Login name bị trùng tên
		return 1

	exec @Ret = sp_grantdbaccess @LoginName, @UserName
	if(@Ret = 1)--User name bị trùng tên
	begin
		exec sp_droplogin @LoginName
		return 2
	end

	exec sp_addrolemember @Role, @UserName
	if(@Role = 'TRUONG' OR @Role = 'GIANGVIEN')
		EXEC sp_addsrvrolemember @LoginName, 'SecurityAdmin'
return 0 --Tạo thành công
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateAp]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_UpdateAp]
	@MAAP varchar(5),
	@TENAP nvarchar(50),
	@MAXA varchar(5)
as
	update AP
	set TenAp = @TENAP
	where MaAp = @MAAP
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateBuoi]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_UpdateBuoi]
	@BUOINGAY varchar(30),
	@BUOI varchar(10),
	@NGAY date
as
	update BUOI
	set Buoi = @BUOI, Ngay = @NGAY
	where BuoiNgay = @BUOINGAY
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateDiaBan]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_UpdateDiaBan]
	@MADIABAN varchar(5),
	@TENDIABAN nvarchar(50)
as
	update DIABAN
	set TenDiaBan = @TENDIABAN
	where MaDiaBan = @MADIABAN
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateGiangVien]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_UpdateGiangVien]
	@MAGV varchar(5),
	@TENGV nvarchar(50),
	@MAKHOA varchar(5),
	@MADOIGIAMSAT varchar(5)
as
	update GIANGVIEN
	set TenGV = @TENGV, MaDoiGiamSat = @MADOIGIAMSAT
	where MaGV = @MAGV
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateKhoa]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_UpdateKhoa]
	@MAKHOA varchar(5),
	@TENKHOA nvarchar(50)
as
	update KHOA
	set TenKhoa = @TENKHOA
	where MaKhoa = @MAKHOA
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateKT]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_UpdateKT]
	@MAKT varchar(5),
	@NOIDUNG nvarchar(50)
as
	update KHENTHUONG
	set NoiDungKT = @NOIDUNG
	where MaKT = @MAKT
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateNha]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_UpdateNha]
	@MANHA varchar(5),
	@TENNHA nvarchar(50),
	@MANHOM varchar(5)
as
	update NHA
	set TenNha = @TENNHA, MaNhom = @MANHOM
	where MaNha = @MANHA
GO
/****** Object:  StoredProcedure [dbo].[sp_XoaThucHien]    Script Date: 7/10/2023 1:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[sp_XoaThucHien]
	@BUOINGAY varchar(30),
	@NHOM varchar(5),
	@MACV varchar(5)
as
	delete NHOMTHUCHIEN
	where BuoiNgay = @BUOINGAY and Nhom = @NHOM and MaCV = @MACV
GO
USE [master]
GO
ALTER DATABASE [CHIENDICHMUAHE] SET  READ_WRITE 
GO
