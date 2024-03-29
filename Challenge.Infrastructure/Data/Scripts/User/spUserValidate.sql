USE [Meetup]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author:		<Matsuo, Luciano>
-- Create date: <20/01/2021>
-- Description:	<Valida un usuario>
-- Comments:	<Se utiliza pra el login>
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[spUserValidate](
	@Username VARCHAR(50),
	@Password VARCHAR(50)
	)
AS
BEGIN
SET NOCOUNT ON;

    Declare	@ErrorMessage	nvarchar(4000);  
    Declare @ErrorSeverity	int;  
    Declare @ErrorState		int;  

	BEGIN TRY
		-- Insert statements for procedure here
		IF((SELECT	COUNT(1) 
			FROM UserLogin AS u WITH (NOLOCK)
			WHERE	u.Username = UPPER(rtrim(LTrim(@Username)))
					AND u.Password = UPPER(rtrim(LTrim(@Password)))
				) = 1)
		BEGIN
			--SET @ErrorMessage = 'Usuario inexistente.';
			--RAISERROR (@ErrorMessage, 16, 1);	
		--END
		
		SELECT * FROM [User] as u WHERE u.Email = UPPER(rtrim(LTrim(@Username)))
END
	END TRY
	BEGIN CATCH
		Select	@ErrorMessage = ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);  
	END CATCH
				
END