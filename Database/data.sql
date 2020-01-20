USE [master]
GO
/****** Object:  Database [coerp]    Script Date: 1/16/2020 7:42:22 AM ******/
CREATE DATABASE [coerp]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'coerp', FILENAME = N'D:\CSDL\MSSQL14.MSSQLSERVER\MSSQL\DATA\coerp.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'coerp_log', FILENAME = N'D:\CSDL\MSSQL14.MSSQLSERVER\MSSQL\DATA\coerp_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [coerp].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [coerp] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [coerp] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [coerp] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [coerp] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [coerp] SET ARITHABORT OFF 
GO
ALTER DATABASE [coerp] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [coerp] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [coerp] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [coerp] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [coerp] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [coerp] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [coerp] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [coerp] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [coerp] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [coerp] SET  DISABLE_BROKER 
GO
ALTER DATABASE [coerp] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [coerp] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [coerp] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [coerp] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [coerp] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [coerp] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [coerp] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [coerp] SET RECOVERY FULL 
GO
ALTER DATABASE [coerp] SET  MULTI_USER 
GO
ALTER DATABASE [coerp] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [coerp] SET DB_CHAINING OFF 
GO
ALTER DATABASE [coerp] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [coerp] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [coerp] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'coerp', N'ON'
GO
USE [coerp]
GO
/****** Object:  Table [dbo].[company]    Script Date: 1/16/2020 7:42:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[company](
	[co_id] [int] IDENTITY(1,1) NOT NULL,
	[co_name] [nvarchar](250) NULL,
	[co_vision] [nvarchar](500) NULL,
	[co_address] [nvarchar](250) NULL,
	[co_mission] [nvarchar](500) NULL,
	[co_target] [nvarchar](max) NULL,
	[co_description] [nvarchar](max) NULL,
	[co_logo] [nvarchar](250) NULL,
	[co_bio] [nvarchar](max) NULL,
	[co_type] [tinyint] NULL,
	[co_no_of_employees] [int] NULL,
	[co_revenue] [int] NULL,
 CONSTRAINT [PK_company] PRIMARY KEY CLUSTERED 
(
	[co_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[customer]    Script Date: 1/16/2020 7:42:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[customer](
	[cu_id] [int] IDENTITY(1,1) NOT NULL,
	[cu_code] [varchar](45) NULL,
	[cu_mobile] [varchar](10) NULL,
	[cu_thumbnail] [nvarchar](45) NULL,
	[cu_email] [varchar](40) NULL,
	[cu_fullname] [nvarchar](45) NULL,
	[cu_type] [tinyint] NULL,
	[cu_address] [nvarchar](120) NULL,
	[cu_create_date] [datetime] NULL,
	[cu_note] [nvarchar](250) NULL,
	[cu_geocoding] [int] NULL,
	[customer_group_id] [int] NULL,
	[cu_status] [tinyint] NULL,
 CONSTRAINT [PK_customer] PRIMARY KEY CLUSTERED 
(
	[cu_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[department]    Script Date: 1/16/2020 7:42:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[department](
	[de_id] [int] IDENTITY(1,1) NOT NULL,
	[de_name] [nvarchar](50) NULL,
	[de_thumbnail] [varchar](45) NULL,
	[de_description] [nvarchar](500) NULL,
	[de_manager] [nvarchar](150) NULL,
	[company_id] [int] NULL,
 CONSTRAINT [PK_department] PRIMARY KEY CLUSTERED 
(
	[de_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[function]    Script Date: 1/16/2020 7:42:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[function](
	[fun_id] [int] IDENTITY(1,1) NOT NULL,
	[fun_name] [nvarchar](250) NULL,
 CONSTRAINT [PK_function] PRIMARY KEY CLUSTERED 
(
	[fun_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[function_setting]    Script Date: 1/16/2020 7:42:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[function_setting](
	[fs_id] [int] IDENTITY(1,1) NOT NULL,
	[fs_create_date] [datetime] NULL,
	[function_id] [int] NULL,
	[department_id] [int] NULL,
 CONSTRAINT [PK_function_setting] PRIMARY KEY CLUSTERED 
(
	[fs_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[group_role]    Script Date: 1/16/2020 7:42:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[group_role](
	[gr_id] [int] IDENTITY(1,1) NOT NULL,
	[gr_name] [nvarchar](20) NULL,
	[gr_thumbnail] [varchar](45) NULL,
	[gr_description] [text] NULL,
	[gr_status] [tinyint] NULL,
 CONSTRAINT [PK_group_role] PRIMARY KEY CLUSTERED 
(
	[gr_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[notification]    Script Date: 1/16/2020 7:42:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[notification](
	[ntf_id] [int] IDENTITY(1,1) NOT NULL,
	[ntf_title] [nvarchar](50) NULL,
	[ntf_description] [nvarchar](max) NULL,
	[ntf_datetime] [datetime] NULL,
	[ntf_confim_datetime] [datetime] NULL,
	[staff_id] [int] NULL,
 CONSTRAINT [PK_notification] PRIMARY KEY CLUSTERED 
(
	[ntf_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[order_product]    Script Date: 1/16/2020 7:42:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[order_product](
	[op_id] [int] IDENTITY(1,1) NOT NULL,
	[op_code] [nvarchar](50) NULL,
	[op_quantity] [int] NULL,
	[op_status] [tinyint] NULL,
	[op_note] [nvarchar](max) NULL,
	[op_datetime] [datetime] NULL,
	[staff_id] [int] NULL,
	[product_id] [int] NULL,
	[customer_id] [int] NULL,
 CONSTRAINT [PK_order_product] PRIMARY KEY CLUSTERED 
(
	[op_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[order_service]    Script Date: 1/16/2020 7:42:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[order_service](
	[os_id] [int] IDENTITY(1,1) NOT NULL,
	[os_code] [nvarchar](50) NULL,
	[os_before_image] [nvarchar](250) NULL,
	[os_after_image] [nvarchar](250) NULL,
	[os_requiment] [nvarchar](250) NULL,
	[os_evaluation] [tinyint] NULL,
	[os_create_date] [datetime] NULL,
	[os_status] [tinyint] NULL,
	[customer_id] [int] NULL,
	[service_id] [int] NULL,
	[staff_id] [int] NULL,
 CONSTRAINT [PK_order_service] PRIMARY KEY CLUSTERED 
(
	[os_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[package]    Script Date: 1/16/2020 7:42:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[package](
	[pac_id] [int] IDENTITY(1,1) NOT NULL,
	[pac_name] [nvarchar](250) NULL,
	[pac_icon] [nvarchar](250) NULL,
	[pac_price] [float] NULL,
	[pac_status] [tinyint] NULL,
	[pac_duration] [datetime] NULL,
	[funtion_id] [int] NULL,
 CONSTRAINT [PK_package] PRIMARY KEY CLUSTERED 
(
	[pac_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[position]    Script Date: 1/16/2020 7:42:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[position](
	[pos_id] [int] IDENTITY(1,1) NOT NULL,
	[pos_name] [nvarchar](250) NULL,
	[pos_competence] [nvarchar](50) NULL,
	[pos_abilty] [nvarchar](150) NULL,
	[pos_authority] [nvarchar](50) NULL,
	[pos_responsibility] [nvarchar](250) NULL,
	[pos_description] [nvarchar](250) NULL,
 CONSTRAINT [PK_position] PRIMARY KEY CLUSTERED 
(
	[pos_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[product]    Script Date: 1/16/2020 7:42:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[product](
	[pu_id] [int] IDENTITY(1,1) NOT NULL,
	[pu_code] [varchar](45) NULL,
	[pu_name] [nvarchar](45) NULL,
	[pu_quantity] [int] NULL,
	[pu_buy_price] [int] NULL,
	[pu_sale_price] [int] NULL,
	[pu_saleoff] [int] NULL,
	[pu_short_description] [nvarchar](200) NULL,
	[pu_create_date] [datetime] NULL,
	[pu_update_date] [datetime] NULL,
	[pu_description] [text] NULL,
	[pu_unit] [tinyint] NULL,
	[product_category_id] [int] NULL,
	[provider_id] [int] NULL,
	[pu_tax] [int] NULL,
	[pu_expired_date] [datetime] NULL,
	[pu_weight] [int] NULL,
 CONSTRAINT [PK_product_1] PRIMARY KEY CLUSTERED 
(
	[pu_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[project]    Script Date: 1/16/2020 7:42:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[project](
	[pro_id] [int] IDENTITY(1,1) NOT NULL,
	[pro_name] [nvarchar](250) NULL,
	[pro_status] [tinyint] NULL,
 CONSTRAINT [PK_project] PRIMARY KEY CLUSTERED 
(
	[pro_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[service]    Script Date: 1/16/2020 7:42:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[service](
	[se_id] [int] IDENTITY(1,1) NOT NULL,
	[se_code] [varchar](10) NULL,
	[se_type] [tinyint] NULL,
	[se_name] [nvarchar](100) NULL,
	[se_description] [text] NULL,
	[se_thumbnai] [varchar](45) NULL,
	[se_price] [int] NULL,
	[se_saleoff] [int] NULL,
	[service_category_id] [int] NULL,
 CONSTRAINT [PK_service] PRIMARY KEY CLUSTERED 
(
	[se_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[staff]    Script Date: 1/16/2020 7:42:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[staff](
	[sta_id] [int] IDENTITY(1,1) NOT NULL,
	[sta_code] [varchar](50) NULL,
	[sta_thumbnai] [varchar](120) NULL,
	[sta_fullname] [nvarchar](45) NULL,
	[sta_username] [varchar](45) NULL,
	[sta_password] [varchar](120) NULL,
	[sta_sex] [tinyint] NULL,
	[sta_birthday] [datetime] NULL,
	[sta_email] [varchar](30) NULL,
	[sta_status] [tinyint] NULL,
	[sta_aboutme] [nvarchar](500) NULL,
	[sta_mobile] [varchar](11) NULL,
	[sta_identity_card] [varchar](20) NULL,
	[sta_identity_card_date] [datetime] NULL,
	[sta_address] [nvarchar](120) NULL,
	[sta_created_date] [datetime] NULL,
	[department_id] [int] NULL,
	[group_role_id] [int] NULL,
	[social_id] [int] NULL,
	[sta_hometown] [nvarchar](120) NULL,
	[position_id] [int] NULL,
	[sta_leader_id] [int] NULL,
 CONSTRAINT [PK_staff_1] PRIMARY KEY CLUSTERED 
(
	[sta_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tasks]    Script Date: 1/16/2020 7:42:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tasks](
	[tsk_id] [int] IDENTITY(1,1) NOT NULL,
	[tsk_title] [nvarchar](250) NULL,
	[project_id] [int] NULL,
	[staff_id] [int] NULL,
	[tsk_start_datetime] [datetime] NULL,
	[tsk_end_datetime] [datetime] NULL,
	[tsk_status] [tinyint] NULL,
	[tsk_content] [nvarchar](250) NULL,
 CONSTRAINT [PK_tasks] PRIMARY KEY CLUSTERED 
(
	[tsk_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[customer] ON 

INSERT [dbo].[customer] ([cu_id], [cu_code], [cu_mobile], [cu_thumbnail], [cu_email], [cu_fullname], [cu_type], [cu_address], [cu_create_date], [cu_note], [cu_geocoding], [customer_group_id], [cu_status]) VALUES (3, N'646645', N'012345678', N'/Uploads/g6574.jpg', N'tienmta@gmail.com', N'tienmta99', 1, N'HN', CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'Ghi chu ', 1, 1, 1)
SET IDENTITY_INSERT [dbo].[customer] OFF
SET IDENTITY_INSERT [dbo].[department] ON 

INSERT [dbo].[department] ([de_id], [de_name], [de_thumbnail], [de_description], [de_manager], [company_id]) VALUES (2, N'Qu?n lý', N'/Uploads/g2003.jpg', N'Có t?t c? các quy?n', N'1', 1)
INSERT [dbo].[department] ([de_id], [de_name], [de_thumbnail], [de_description], [de_manager], [company_id]) VALUES (3, N'Quản lý', N'/Uploads/g2052.jpg', N'Có tất cả các quyền', N'1', 1)
SET IDENTITY_INSERT [dbo].[department] OFF
SET IDENTITY_INSERT [dbo].[position] ON 

INSERT [dbo].[position] ([pos_id], [pos_name], [pos_competence], [pos_abilty], [pos_authority], [pos_responsibility], [pos_description]) VALUES (1, N'Ha noi ', N'Code fontend 1', N'FullStack', N'Đây là thẩm uyền ', N'Trách nhiệm', N'Lalalala')
SET IDENTITY_INSERT [dbo].[position] OFF
SET IDENTITY_INSERT [dbo].[product] ON 

INSERT [dbo].[product] ([pu_id], [pu_code], [pu_name], [pu_quantity], [pu_buy_price], [pu_sale_price], [pu_saleoff], [pu_short_description], [pu_create_date], [pu_update_date], [pu_description], [pu_unit], [product_category_id], [provider_id], [pu_tax], [pu_expired_date], [pu_weight]) VALUES (1, N'646645', N'nguyen huu tien ', 1, 1, 1, 1, N'tom tat ngan ', CAST(N'2015-05-29T05:50:00.000' AS DateTime), CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'Noi dung tom tat', 1, 1, 1, 1, CAST(N'2015-05-29T05:50:00.000' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[product] OFF
SET IDENTITY_INSERT [dbo].[service] ON 

INSERT [dbo].[service] ([se_id], [se_code], [se_type], [se_name], [se_description], [se_thumbnai], [se_price], [se_saleoff], [service_category_id]) VALUES (2, NULL, 1, N'Server bootai', N'Day la tom tat ', N'', 1000, NULL, 2)
INSERT [dbo].[service] ([se_id], [se_code], [se_type], [se_name], [se_description], [se_thumbnai], [se_price], [se_saleoff], [service_category_id]) VALUES (3, N'98098', 1, N'Server bootai', N'Day la tom tat ', N'/Uploads/g3880.jpg', 1000, 1, 2)
SET IDENTITY_INSERT [dbo].[service] OFF
SET IDENTITY_INSERT [dbo].[staff] ON 

INSERT [dbo].[staff] ([sta_id], [sta_code], [sta_thumbnai], [sta_fullname], [sta_username], [sta_password], [sta_sex], [sta_birthday], [sta_email], [sta_status], [sta_aboutme], [sta_mobile], [sta_identity_card], [sta_identity_card_date], [sta_address], [sta_created_date], [department_id], [group_role_id], [social_id], [sta_hometown], [position_id], [sta_leader_id]) VALUES (6, N'11111       ', N'/Uploads/g.jpg', N'nguyen huu dung', N'tienmta', N'openupmta@gmail.com', 1, CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'openupmta@gmail.com', 1, N'openupm', N'98798798798', N'876876786', CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'Ha noi', CAST(N'2015-05-29T05:50:00.000' AS DateTime), 1, 1, 1, NULL, NULL, NULL)
INSERT [dbo].[staff] ([sta_id], [sta_code], [sta_thumbnai], [sta_fullname], [sta_username], [sta_password], [sta_sex], [sta_birthday], [sta_email], [sta_status], [sta_aboutme], [sta_mobile], [sta_identity_card], [sta_identity_card_date], [sta_address], [sta_created_date], [department_id], [group_role_id], [social_id], [sta_hometown], [position_id], [sta_leader_id]) VALUES (7, N'11111       ', N'/Uploads/g4235.jpg', N'nguyen huu dung', N'tienmta', N'openupmta@gmail.com', 1, CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'openupmta@gmail.com', 1, N'openupm', N'98798798798', N'876876786', CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'Ha noi', CAST(N'2015-05-29T05:50:00.000' AS DateTime), 1, 1, 1, NULL, NULL, NULL)
INSERT [dbo].[staff] ([sta_id], [sta_code], [sta_thumbnai], [sta_fullname], [sta_username], [sta_password], [sta_sex], [sta_birthday], [sta_email], [sta_status], [sta_aboutme], [sta_mobile], [sta_identity_card], [sta_identity_card_date], [sta_address], [sta_created_date], [department_id], [group_role_id], [social_id], [sta_hometown], [position_id], [sta_leader_id]) VALUES (8, N'abc         ', N'/Uploads/g2845.jpg', N'nguyen huu thinh
', N'tienmta', N'78554cc2552b0f7593b6ad5cbd5ab4a4', 1, CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'openupmta@gmail.com', 1, N'hkjahfhdsa', N'89798709', N'iughoshghsd', CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'Ha noi', CAST(N'2015-05-29T05:50:00.000' AS DateTime), 1, 1, 1, NULL, NULL, NULL)
INSERT [dbo].[staff] ([sta_id], [sta_code], [sta_thumbnai], [sta_fullname], [sta_username], [sta_password], [sta_sex], [sta_birthday], [sta_email], [sta_status], [sta_aboutme], [sta_mobile], [sta_identity_card], [sta_identity_card_date], [sta_address], [sta_created_date], [department_id], [group_role_id], [social_id], [sta_hometown], [position_id], [sta_leader_id]) VALUES (9, N'abc         ', N'/Uploads/g5056.jpg', N'fahfaho
', N'tienmta', N'MMxv9Ccm0ObFyspTmaH1hA==', 1, CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'openupmta@gmail.com', 1, N'hkjahfhdsa', N'89798709', N'iughoshghsd', CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'Ha noi', CAST(N'2015-05-29T05:50:00.000' AS DateTime), 1, 1, 1, NULL, NULL, NULL)
INSERT [dbo].[staff] ([sta_id], [sta_code], [sta_thumbnai], [sta_fullname], [sta_username], [sta_password], [sta_sex], [sta_birthday], [sta_email], [sta_status], [sta_aboutme], [sta_mobile], [sta_identity_card], [sta_identity_card_date], [sta_address], [sta_created_date], [department_id], [group_role_id], [social_id], [sta_hometown], [position_id], [sta_leader_id]) VALUES (10, N'abc         ', N'/Uploads/g2148.jpg', N'fahfaho
', N'tienmta', N'MMxv9Ccm0ObFyspTmaH1hA==', 1, CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'openupmta@gmail.com', 1, N'hkjahfhdsa', N'89798709', N'iughoshghsd', CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'Ha noi', CAST(N'2015-05-29T05:50:00.000' AS DateTime), 1, 1, 1, NULL, NULL, NULL)
INSERT [dbo].[staff] ([sta_id], [sta_code], [sta_thumbnai], [sta_fullname], [sta_username], [sta_password], [sta_sex], [sta_birthday], [sta_email], [sta_status], [sta_aboutme], [sta_mobile], [sta_identity_card], [sta_identity_card_date], [sta_address], [sta_created_date], [department_id], [group_role_id], [social_id], [sta_hometown], [position_id], [sta_leader_id]) VALUES (11, N'abc         ', N'/Uploads/g6985.jpg', N'fahfaho
', N'tienmta', N'MMxv9Ccm0ObFyspTmaH1hA==', 1, CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'openupmta@gmail.com', 1, N'hkjahfhdsa', N'89798709', N'iughoshghsd', CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'Ha noi', CAST(N'2015-05-29T05:50:00.000' AS DateTime), 1, 1, 1, NULL, NULL, NULL)
INSERT [dbo].[staff] ([sta_id], [sta_code], [sta_thumbnai], [sta_fullname], [sta_username], [sta_password], [sta_sex], [sta_birthday], [sta_email], [sta_status], [sta_aboutme], [sta_mobile], [sta_identity_card], [sta_identity_card_date], [sta_address], [sta_created_date], [department_id], [group_role_id], [social_id], [sta_hometown], [position_id], [sta_leader_id]) VALUES (12, N'abc         ', N'/Uploads/g1034.jpg', N'fahfaho
', N'tienmta', N'MMxv9Ccm0ObFyspTmaH1hA==', 1, CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'openupmta@gmail.com', 1, N'hkjahfhdsa', N'89798709', N'iughoshghsd', CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'Ha noi', CAST(N'2015-05-29T05:50:00.000' AS DateTime), 1, 1, 1, NULL, NULL, NULL)
INSERT [dbo].[staff] ([sta_id], [sta_code], [sta_thumbnai], [sta_fullname], [sta_username], [sta_password], [sta_sex], [sta_birthday], [sta_email], [sta_status], [sta_aboutme], [sta_mobile], [sta_identity_card], [sta_identity_card_date], [sta_address], [sta_created_date], [department_id], [group_role_id], [social_id], [sta_hometown], [position_id], [sta_leader_id]) VALUES (13, N'abc         ', N'/Uploads/g1987.jpg', N'fahfaho
', N'tienmta', N'MMxv9Ccm0ObFyspTmaH1hA==', 1, CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'openupmta@gmail.com', 1, N'hkjahfhdsa', N'89798709', N'iughoshghsd', CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'Ha noi', CAST(N'2015-05-29T05:50:00.000' AS DateTime), 1, 1, 1, NULL, NULL, NULL)
INSERT [dbo].[staff] ([sta_id], [sta_code], [sta_thumbnai], [sta_fullname], [sta_username], [sta_password], [sta_sex], [sta_birthday], [sta_email], [sta_status], [sta_aboutme], [sta_mobile], [sta_identity_card], [sta_identity_card_date], [sta_address], [sta_created_date], [department_id], [group_role_id], [social_id], [sta_hometown], [position_id], [sta_leader_id]) VALUES (14, N'abc         ', N'/Uploads/g6748.jpg', N'fahfaho
', N'tienmta', N'MMxv9Ccm0ObFyspTmaH1hA==', 1, CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'openupmta@gmail.com', 1, N'hkjahfhdsa', N'89798709', N'iughoshghsd', CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'Ha noi', CAST(N'2015-05-29T05:50:00.000' AS DateTime), 1, 1, 1, NULL, NULL, NULL)
INSERT [dbo].[staff] ([sta_id], [sta_code], [sta_thumbnai], [sta_fullname], [sta_username], [sta_password], [sta_sex], [sta_birthday], [sta_email], [sta_status], [sta_aboutme], [sta_mobile], [sta_identity_card], [sta_identity_card_date], [sta_address], [sta_created_date], [department_id], [group_role_id], [social_id], [sta_hometown], [position_id], [sta_leader_id]) VALUES (15, N'abc         ', N'/Uploads/g9846.jpg', N'fahfaho
', N'tienmta', N'MMxv9Ccm0ObFyspTmaH1hA==', 1, CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'openupmta@gmail.com', 1, N'hkjahfhdsa', N'89798709', N'iughoshghsd', CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'Ha noi', CAST(N'2015-05-29T05:50:00.000' AS DateTime), 1, 1, 1, NULL, NULL, NULL)
INSERT [dbo].[staff] ([sta_id], [sta_code], [sta_thumbnai], [sta_fullname], [sta_username], [sta_password], [sta_sex], [sta_birthday], [sta_email], [sta_status], [sta_aboutme], [sta_mobile], [sta_identity_card], [sta_identity_card_date], [sta_address], [sta_created_date], [department_id], [group_role_id], [social_id], [sta_hometown], [position_id], [sta_leader_id]) VALUES (16, N'abc         ', N'/Uploads/g4585.jpg', N'fahfaho
', N'tienmta', N'MMxv9Ccm0ObFyspTmaH1hA==', 1, CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'openupmta@gmail.com', 1, N'hkjahfhdsa', N'89798709', N'iughoshghsd', CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'Ha noi', CAST(N'2015-05-29T05:50:00.000' AS DateTime), 1, 1, 1, NULL, NULL, NULL)
INSERT [dbo].[staff] ([sta_id], [sta_code], [sta_thumbnai], [sta_fullname], [sta_username], [sta_password], [sta_sex], [sta_birthday], [sta_email], [sta_status], [sta_aboutme], [sta_mobile], [sta_identity_card], [sta_identity_card_date], [sta_address], [sta_created_date], [department_id], [group_role_id], [social_id], [sta_hometown], [position_id], [sta_leader_id]) VALUES (17, N'abc         ', N'/Uploads/g9467.jpg', N'fahfaho
', N'tienmta', N'MMxv9Ccm0ObFyspTmaH1hA==', 1, CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'openupmta@gmail.com', 1, N'hkjahfhdsa', N'89798709', N'iughoshghsd', CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'Ha noi', CAST(N'2015-05-29T05:50:00.000' AS DateTime), 1, 1, 1, NULL, NULL, NULL)
INSERT [dbo].[staff] ([sta_id], [sta_code], [sta_thumbnai], [sta_fullname], [sta_username], [sta_password], [sta_sex], [sta_birthday], [sta_email], [sta_status], [sta_aboutme], [sta_mobile], [sta_identity_card], [sta_identity_card_date], [sta_address], [sta_created_date], [department_id], [group_role_id], [social_id], [sta_hometown], [position_id], [sta_leader_id]) VALUES (18, N'abc         ', N'/Uploads/g440.jpg', N'fahfaho
', N'tienmta', N'MMxv9Ccm0ObFyspTmaH1hA==', NULL, NULL, N'openupmta@gmail.com', NULL, N'hkjahfhdsa', N'89798709', N'iughoshghsd', NULL, N'Ha noi', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[staff] ([sta_id], [sta_code], [sta_thumbnai], [sta_fullname], [sta_username], [sta_password], [sta_sex], [sta_birthday], [sta_email], [sta_status], [sta_aboutme], [sta_mobile], [sta_identity_card], [sta_identity_card_date], [sta_address], [sta_created_date], [department_id], [group_role_id], [social_id], [sta_hometown], [position_id], [sta_leader_id]) VALUES (19, N'abc         ', N'/Uploads/g6003.jpg', N'fahfaho
', N'tienmta', N'MMxv9Ccm0ObFyspTmaH1hA==', NULL, NULL, N'openupmta@gmail.com', NULL, N'hkjahfhdsa', N'89798709', N'iughoshghsd', NULL, N'Ha noi', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[staff] ([sta_id], [sta_code], [sta_thumbnai], [sta_fullname], [sta_username], [sta_password], [sta_sex], [sta_birthday], [sta_email], [sta_status], [sta_aboutme], [sta_mobile], [sta_identity_card], [sta_identity_card_date], [sta_address], [sta_created_date], [department_id], [group_role_id], [social_id], [sta_hometown], [position_id], [sta_leader_id]) VALUES (20, N'abc', N'/Uploads/g7015.jpg', N'fahfaho
', N'tienmta', N'MMxv9Ccm0ObFyspTmaH1hA==', 1, CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'openupmta@gmail.com', 1, N'hkjahfhdsa', N'89798709', N'iughoshghsd', CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'Ha noi', CAST(N'2015-05-29T05:50:00.000' AS DateTime), 1, 1, 1, N'Ha noi ', 1, 0)
INSERT [dbo].[staff] ([sta_id], [sta_code], [sta_thumbnai], [sta_fullname], [sta_username], [sta_password], [sta_sex], [sta_birthday], [sta_email], [sta_status], [sta_aboutme], [sta_mobile], [sta_identity_card], [sta_identity_card_date], [sta_address], [sta_created_date], [department_id], [group_role_id], [social_id], [sta_hometown], [position_id], [sta_leader_id]) VALUES (21, N'abc', N'/Uploads/g5407.jpg', N'fahfaho
', N'tienmta', N'MMxv9Ccm0ObFyspTmaH1hA==', 1, CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'openupmta@gmail.com', 1, N'hkjahfhdsa', N'89798709', N'iughoshghsd', CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'Ha noi', CAST(N'2015-05-29T05:50:00.000' AS DateTime), 1, 1, 1, N'Ha noi ', 1, 0)
INSERT [dbo].[staff] ([sta_id], [sta_code], [sta_thumbnai], [sta_fullname], [sta_username], [sta_password], [sta_sex], [sta_birthday], [sta_email], [sta_status], [sta_aboutme], [sta_mobile], [sta_identity_card], [sta_identity_card_date], [sta_address], [sta_created_date], [department_id], [group_role_id], [social_id], [sta_hometown], [position_id], [sta_leader_id]) VALUES (22, N'abc', N'/Uploads/g5840.jpg', N'Nguyen Huu Tien', N'tienmta', N'MMxv9Ccm0ObFyspTmaH1hA==', 1, CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'openupmta@gmail.com', 1, N'hkjahfhdsa', N'89798709', N'iughoshghsd', CAST(N'2015-05-29T05:50:00.000' AS DateTime), N'Ha noi', CAST(N'2015-05-29T05:50:00.000' AS DateTime), 1, 1, 1, N'Ha noi ', 1, 0)
SET IDENTITY_INSERT [dbo].[staff] OFF
USE [master]
GO
ALTER DATABASE [coerp] SET  READ_WRITE 
GO
