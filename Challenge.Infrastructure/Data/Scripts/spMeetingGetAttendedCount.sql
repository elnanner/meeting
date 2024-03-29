USE [Meetup]
GO
/****** Object:  StoredProcedure [dbo].[spMeetingGetAttendedCount]    Script Date: 20/1/2021 18:53:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Author:		<Matsuo, Luciano>
-- Create date: <20/01/2021>
-- Description:	<Devuelve la cantidad de inscriptos>
-- Comments:	<Se utiliza para indicar la cantidad de invitados que asistirán a una meeting.>
-- =============================================
CREATE OR ALTER   PROCEDURE [dbo].[spMeetingGetAttendedCount](
	@MeetingId INT,
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
	Declare @TotalCount	int = 0;

	BEGIN TRY
		-- Insert statements for procedure here

		IF((SELECT	COUNT(1) 
			FROM Meeting AS m WITH (NOLOCK)
			WHERE	m.MeetingId = @MeetingId
				) = 0)
		BEGIN
		-- El usuario no esta incripto
			SET @ErrorMessage = 'No existe la meeting.'
					
			RAISERROR (@ErrorMessage, 16, 1);	
			
						
		END
		
		SELECT COUNT(*)
			FROM Meeting m INNER JOIN MeetingUser mu
			ON m.MeetingId = mu.MeetingId
			WHERE m.MeetingId = @MeetingId
				AND mu.Status = @Status
			
	END TRY
	BEGIN CATCH
		Select	@ErrorMessage = ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);  
	END CATCH
END
