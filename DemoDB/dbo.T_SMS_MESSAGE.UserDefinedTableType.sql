USE [Demo]
GO
/****** Object:  UserDefinedTableType [dbo].[T_SMS_MESSAGE]    Script Date: 4.6.2019. 23:42:56 ******/
CREATE TYPE [dbo].[T_SMS_MESSAGE] AS TABLE(
	[RECIPIENT] [nvarchar](100) NOT NULL,
	[SMS_FILENAME] [nvarchar](100) NOT NULL,
	[CELLPHONE] [nvarchar](100) NOT NULL,
	[TIME_SENT] [datetime2](7) NOT NULL
)
GO
