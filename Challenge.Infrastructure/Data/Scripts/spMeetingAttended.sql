USE [Meetup]
GO
/****** Object:  StoredProcedure [dbo].[spMeetingAttended]    Script Date: 22/1/2021 01:52:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Author:		<Matsuo, Luciano>
-- Create date: <20/01/2021>
-- Description:	<Indica que un usuario asistirá a una meeting>
-- Comments:	<Se utiliza para indicar que asistirán a una meeting.>
-- =============================================
CREATE OR ALTER   PROCEDURE [dbo].[spMeetingAttended](
	@MeetingId INT,
	@UserId INT,
	@Status INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    Declare	@ErrorMessage	nvarchar(4000);  
    Declare @ErrorSeverity	int;  
    Declare @ErrorState		int;  
	Declare @StatusActual	int;

	BEGIN TRY
		-- Insert statements for procedure here

		IF((SELECT	COUNT(1) 
			FROM MeetingUser AS mu WITH (NOLOCK)
			WHERE	mu.MeetingId = @MeetingId AND mu.UserId = @UserId 
				) = 1)
		BEGIN
			SET @StatusActual =(SELECT [Status] from MeetingUser 
				WHERE MeetingId = @MeetingId AND UserId = @UserId)
				
			--usuario inscripto, le ponems que asistió
			IF(@StatusActual = 1) 
			BEGIN
				UPDATE MeetingUser SET Status = @Status
				WHERE MeetingId = @MeetingId AND UserId = @UserId 
			END

			--usario avisa ya estuvo ahí
			IF(@StatusActual = 2) 
			BEGIN
				SET @ErrorMessage = 'El usuario ya ha indicado que ha asistido a la meeting.'
				RAISERROR (@ErrorMessage, 16, 1);	
			END
		END
		ELSE 
		BEGIN
		-- El usuario no esta incripto
			SET @ErrorMessage = 'El usuario indicado no se ha inscripto la meeting.'
					
			RAISERROR (@ErrorMessage, 16, 1);	
		END
		--devolvemos el meetingId para saber que fue todo ok
		select @MeetingId
	END TRY
	BEGIN CATCH
		Select	@ErrorMessage = ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);  
	END CATCH
END
