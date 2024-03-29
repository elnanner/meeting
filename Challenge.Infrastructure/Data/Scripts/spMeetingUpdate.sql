USE [Meetup]
GO
/****** Object:  StoredProcedure [dbo].[spMeetingUpdate]    Script Date: 22/1/2021 00:16:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author:		<Matsuo, Luciano>
-- Create date: <19/01/2021>
-- Description:	<Actualiza una meeting>
-- Comments:	<Se utiliza para el update de meetings.>
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[spMeetingUpdate](
	@MeetingId INT,
	@Description VARCHAR(100),
	@Date DATETIME,
	@MaxPeople INT,
	@CityId INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    Declare	@ErrorMessage	nvarchar(4000);  
    Declare @ErrorSeverity	int;  
    Declare @ErrorState		int;  
	Declare @IdCliente		int;

	BEGIN TRY
		-- Insert statements for procedure here
		IF((SELECT	COUNT(1) 
			FROM Meeting AS m WITH (NOLOCK)
			WHERE	m.MeetingId = @MeetingId
				) = 1)
		BEGIN
			IF(SELECT COUNT(1) FROM City WHERE CityId = @CityId) = 0 
			BEGIN
				SET @ErrorMessage = 'Debe ingresar una ciudad válida.';
				RAISERROR (@ErrorMessage, 16, 1);
			END

			UPDATE Meeting
				
			SET 
				Description = @Description,
				Date = @Date, 
				MaxPeople = @MaxPeople,
				CityId = @CityId

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