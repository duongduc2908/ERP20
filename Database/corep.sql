USE [coerp]
GO
/****** Object:  Table [dbo].[company]    Script Date: 10/01/2020 14:28:22 ******/
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
/****** Object:  Table [dbo].[customer]    Script Date: 10/01/2020 14:28:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[customer](
	[cu_id] [int] IDENTITY(1,1) NOT NULL,
	[cu_code] [nchar](12) NULL,
	[cu_mobile] [varchar](20) NULL,
	[cu_thumbnail] [nvarchar](50) NULL,
	[cu_email] [nvarchar](50) NULL,
	[cu_fullname] [nvarchar](50) NULL,
	[cu_type] [tinyint] NULL,
	[cu_address] [nvarchar](250) NULL,
	[cu_create_date] [datetime] NULL,
	[cu_note] [nvarchar](max) NULL,
	[social_id] [int] NULL,
	[customer_group_id] [int] NULL,
	[customer_address_id] [int] NULL,
	[source_id] [int] NULL,
 CONSTRAINT [PK_customer] PRIMARY KEY CLUSTERED 
(
	[cu_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[department]    Script Date: 10/01/2020 14:28:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[department](
	[de_id] [int] IDENTITY(1,1) NOT NULL,
	[de_name] [nvarchar](250) NULL,
	[de_thumbnail] [nvarchar](250) NULL,
	[de_description] [nvarchar](max) NULL,
	[de_manager] [nvarchar](250) NULL,
	[company_id] [int] NULL,
 CONSTRAINT [PK_department] PRIMARY KEY CLUSTERED 
(
	[de_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[function]    Script Date: 10/01/2020 14:28:22 ******/
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
/****** Object:  Table [dbo].[function_setting]    Script Date: 10/01/2020 14:28:22 ******/
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
/****** Object:  Table [dbo].[group_role]    Script Date: 10/01/2020 14:28:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[group_role](
	[gr_id] [int] IDENTITY(1,1) NOT NULL,
	[gr_name] [nvarchar](250) NULL,
	[gr_thumbnail] [nvarchar](250) NULL,
	[gr_description] [nvarchar](max) NULL,
	[gr_status] [tinyint] NULL,
 CONSTRAINT [PK_group_role] PRIMARY KEY CLUSTERED 
(
	[gr_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[notification]    Script Date: 10/01/2020 14:28:22 ******/
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
/****** Object:  Table [dbo].[order_product]    Script Date: 10/01/2020 14:28:22 ******/
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
/****** Object:  Table [dbo].[order_service]    Script Date: 10/01/2020 14:28:22 ******/
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
/****** Object:  Table [dbo].[package]    Script Date: 10/01/2020 14:28:22 ******/
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
/****** Object:  Table [dbo].[position]    Script Date: 10/01/2020 14:28:22 ******/
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
/****** Object:  Table [dbo].[product]    Script Date: 10/01/2020 14:28:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[product](
	[pu_id] [int] IDENTITY(1,1) NOT NULL,
	[pu_code] [nvarchar](50) NULL,
	[pu_quantity] [int] NULL,
	[pu_buy_price] [nvarchar](50) NULL,
	[pu_sale_price] [nvarchar](50) NULL,
	[pu_saleoff] [int] NULL,
	[pu_short_description] [int] NULL,
	[pu_create_date] [datetime] NULL,
	[pu_update_date] [datetime] NULL,
	[pu_description] [nvarchar](max) NULL,
	[pu_rate] [tinyint] NULL,
	[pu_unit] [tinyint] NULL,
	[pu_status] [tinyint] NULL,
	[pu_size] [tinyint] NULL,
	[product_category_id] [int] NULL,
	[provider_id] [int] NULL,
 CONSTRAINT [PK_product_1] PRIMARY KEY CLUSTERED 
(
	[pu_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[project]    Script Date: 10/01/2020 14:28:22 ******/
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
/****** Object:  Table [dbo].[service]    Script Date: 10/01/2020 14:28:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[service](
	[se_id] [int] IDENTITY(1,1) NOT NULL,
	[se_type] [nvarchar](50) NULL,
	[se_name] [nvarchar](250) NULL,
	[se_description] [nvarchar](max) NULL,
	[se_thumbnai] [nvarchar](250) NULL,
	[se_price] [float] NULL,
	[service_category_id] [int] NULL,
 CONSTRAINT [PK_service] PRIMARY KEY CLUSTERED 
(
	[se_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[staff]    Script Date: 10/01/2020 14:28:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[staff](
	[sta_id] [int] IDENTITY(1,1) NOT NULL,
	[sta_code] [nchar](12) NULL,
	[sta_thumbnai] [nvarchar](50) NULL,
	[sta_fullname] [nvarchar](50) NULL,
	[sta_username] [nvarchar](50) NULL,
	[sta_password] [nvarchar](max) NULL,
	[sta_sex] [tinyint] NULL,
	[sta_birthday] [datetime] NULL,
	[sta_email] [nvarchar](50) NULL,
	[sta_position] [nvarchar](50) NULL,
	[sta_status] [bit] NULL,
	[sta_aboutme] [nvarchar](max) NULL,
	[sta_mobile] [varchar](20) NULL,
	[sta_identity_card] [varchar](250) NULL,
	[sta_identity_card_date] [datetime] NULL,
	[sta_address] [nvarchar](250) NULL,
	[sta_created_date] [datetime] NULL,
	[department_id] [int] NULL,
	[group_role_id] [int] NULL,
	[social_id] [int] NULL,
	[source_id] [int] NULL,
 CONSTRAINT [PK_staff_1] PRIMARY KEY CLUSTERED 
(
	[sta_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tasks]    Script Date: 10/01/2020 14:28:22 ******/
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
