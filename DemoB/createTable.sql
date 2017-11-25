USE [cap_trans_test]
GO

/****** Object:  Table [dbo].[Grade]    Script Date: 2017-11-25 18:04:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Grade](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[personId] [int] NOT NULL,
	[name] [nchar](10) NOT NULL,
 CONSTRAINT [PK_Grade] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO