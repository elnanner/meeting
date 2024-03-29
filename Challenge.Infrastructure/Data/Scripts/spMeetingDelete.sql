USE [Meetup]
GO
/****** Object:  StoredProcedure [dbo].[spMeetingDelete]    Script Date: 20/1/2021 09:42:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author:		<Matsuo, Luciano>
-- Create date: <19/01/2021>
-- Description:	<Elimina una meeting>
-- Comments:	<Se utiliza para eliminar meetings.>
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[spMeetingDelete](
	@MeetingId INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    Declare	@ErrorMessage	nvarchar(4000);  
    Declare @ErrorSeverity	int;  
    Declare @ErrorState		int;  

	BEGIN TRY
		-- Insert statements for procedure here
		IF((SELECT	COUNT(1) 
			FROM Meeting AS m WITH (NOLOCK)
			WHERE	m.MeetingId = @MeetingId
				) = 1)
		BEGIN
			DELETE FROM Meeting
			WHERE MeetingId = @MeetingId 
		END
		ELSE 
		BEGIN
			SET @ErrorMessage = 'La meeting no existe.';
			RAISERROR (@ErrorMessage, 16, 1);	
		END
		select @MeetingId
	END TRY
	BEGIN CATCH
		Select	@ErrorMessage = ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);  
	END CATCH
END