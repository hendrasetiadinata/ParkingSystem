USE [master]
GO
/****** Object:  Database [AisSlots]    Script Date: 2/6/2022 8:33:45 AM ******/
CREATE DATABASE [AisSlots]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AisSlots', FILENAME = N'/var/opt/mssql/data/AisSlots.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'AisSlots_log', FILENAME = N'/var/opt/mssql/data/AisSlots_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [AisSlots] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AisSlots].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AisSlots] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AisSlots] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AisSlots] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AisSlots] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AisSlots] SET ARITHABORT OFF 
GO
ALTER DATABASE [AisSlots] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [AisSlots] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AisSlots] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AisSlots] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AisSlots] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AisSlots] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AisSlots] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AisSlots] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AisSlots] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AisSlots] SET  DISABLE_BROKER 
GO
ALTER DATABASE [AisSlots] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AisSlots] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AisSlots] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AisSlots] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AisSlots] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AisSlots] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AisSlots] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AisSlots] SET RECOVERY FULL 
GO
ALTER DATABASE [AisSlots] SET  MULTI_USER 
GO
ALTER DATABASE [AisSlots] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AisSlots] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AisSlots] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AisSlots] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [AisSlots] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [AisSlots] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'AisSlots', N'ON'
GO
ALTER DATABASE [AisSlots] SET QUERY_STORE = OFF
GO
USE [AisSlots]
GO
/****** Object:  Table [dbo].[ParkingOrder]    Script Date: 2/6/2022 8:33:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ParkingOrder](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SlotNo] [int] NOT NULL,
	[LicensePlate] [varchar](50) NOT NULL,
	[Vehicle] [varchar](10) NOT NULL,
	[Colour] [varchar](20) NOT NULL,
	[CheckIn] [datetime] NOT NULL,
	[CheckOut] [datetime] NULL,
	[TotalMinutes] [int] NULL,
	[ParkingFeePerHour] [decimal](18, 0) NOT NULL,
	[ParkingFee] [decimal](18, 0) NULL,
	[CreatedTime] [datetime] NOT NULL,
	[LastUpdatedTime] [datetime] NULL,
 CONSTRAINT [PK_ParkingOrder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Slot]    Script Date: 2/6/2022 8:33:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Slot](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Total] [int] NOT NULL,
	[Available] [int] NOT NULL,
	[Used] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[ParkingFeePerHour] [decimal] (18,0) NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[LastUpdatedTime] [datetime] NULL,
 CONSTRAINT [PK_Slot] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SlotItem]    Script Date: 2/6/2022 8:33:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SlotItem](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SlotNo] [int] NOT NULL,
	[Used] [bit] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[LastUpdatedTime] [datetime] NULL,
 CONSTRAINT [PK_SlotItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [ix_parking_order_licenseplate]    Script Date: 2/6/2022 8:33:45 AM ******/
CREATE NONCLUSTERED INDEX [ix_parking_order_licenseplate] ON [dbo].[ParkingOrder]
(
	[LicensePlate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [AisSlots] SET  READ_WRITE 
GO
