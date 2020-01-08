USE [coerp]
GO
/****** Object:  Table [dbo].[staff]    Script Date: 1/8/2020 10:56:48 AM ******/
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
