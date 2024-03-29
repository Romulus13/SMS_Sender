USE [Demo]
GO
/****** Object:  StoredProcedure [dbo].[insertMessages]    Script Date: 4.6.2019. 23:42:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Script for SelectTopNRows command from SSMS  ******/
CREATE PROCEDURE [dbo].[insertMessages] 
(
@MessagesToInput dbo.T_SMS_MESSAGE Readonly
)
As
Begin
insert into   dbo.SMS_MESSAGES(RECIPIENT,SMS_FILENAME,CELLPHONE,TIME_SENT)
select RECIPIENT,SMS_FILENAME,CELLPHONE,TIME_SENT from @MessagesToInput;
End
GO
