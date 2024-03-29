USE [Meetup]
GO
/****** Object:  StoredProcedure [dbo].[spMeetingWillAttend]    Script Date: 22/1/2021 00:27:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Author:		<Matsuo, Luciano>
-- Create date: <20/01/2021>
-- Description:	<Indica que un usuario asistirá a una meeting>
-- Comments:	<Se utiliza para indicar que asistirán a una meeting.>
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[spMeetingWillAttend](
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
			DECLARE @MaxPeople INT = 0 
			SET @MaxPeople = (SELECT Maxpeople from Meeting where MeetingId = @MeetingId) 

			IF((SELECT COUNT(*) as 'Total' from Meeting m
							inner join MeetingUser mu 
							on m.MeetingId = mu.MeetingId
							where m.MeetingId = @MeetingId
							group by mu.MeetingId) >= @Maxpeople)
			BEGIN
				SET @ErrorMessage = 'La capacidad de la meeting está colmada.'
				RAISERROR(@ErrorMessage, 16, 1);
			END

			IF((SELECT COUNT(1)
				FROM MeetingUser AS mu WITH (NOLOCK)
				WHERE	mu.MeetingId = @MeetingId AND mu.UserId = @UserId 
					) = 0 )
					-- si no existe y no supera la maxima cantidad lo agrego sino veo su estado(asistio o esta inscripto)
			BEGIN

				INSERT INTO MeetingUser (MeetingId, UserId, Status)
				VALUES(@MeetingId, @UserId, @Status)
			END
			ELSE 
			BEGIN
			-- ver cual de las opciones se da, si el usuario ya esta inscripto o si ya asistió

			SELECT @StatusActual =[Status] from MeetingUser 
				WHERE MeetingId = @MeetingId AND UserId = @UserId
				
			IF(@StatusActual = 1) 
			BEGIN
				SET @ErrorMessage = 'El usuario ya esta inscripto en la meeting.'
			END

			IF(@StatusActual = 2) 
			BEGIN
				SET @ErrorMessage = 'El usuario ha indicado que ha asistido a la meeting.'
			END
			
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
