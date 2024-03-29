USE [Demo]
GO
/****** Object:  StoredProcedure [dbo].[insertRecipients]    Script Date: 4.6.2019. 23:42:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Script for SelectTopNRows command from SSMS  ******/
CREATE PROCEDURE [dbo].[insertRecipients] ( @FULLNAME nvarchar(200) , @CELLPHONE nvarchar(50), @INSERTED_ID bigint OUTPUT )
AS 
DECLARE @tempTableVar table (id bigint);
BEGIN
    IF NOT EXISTS ( SELECT 1 FROM dbo.Recipients ap WITH (UPDLOCK) WHERE ap.CELLPHONE  = @CELLPHONE AND ap.FULLNAME = @FULLNAME  ) 
	BEGIN
      INSERT INTO dbo.Recipients (FULLNAME,CELLPHONE,DATE_INSERTED,DATE_UPDATED)
					OUTPUT INSERTED.ID INTO @tempTableVar
                                VALUES(@FULLNAME ,@CELLPHONE, GETDATE(),GETDATE()); 
		SELECT @INSERTED_ID = id FROM @tempTableVar;									
	END
	ELSE
		THROW 51016, 'Contact has already been added to database!', 1;  

END
GO
