USE [Meetup]
GO
/****** Object:  StoredProcedure [dbo].[spMeetingInsert]    Script Date: 21/1/2021 23:25:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author:		<Matsuo, Luciano>
-- Create date: <19/01/2021>
-- Description:	<Si la meetin no existe la inserta, sino informa el error>
-- Comments:	<Se utiliza para el alta de meetings.>
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[spMeetingInsert](
	@AdminId INT,
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
			WHERE	m.AdminId = @AdminId 
				AND m.Description = @Description
				AND m.Date = @Date
				AND m.CityId = @CityId
				) = 0)
		BEGIN
			IF(SELECT COUNT(1) FROM City WHERE CityId = @CityId) = 0 
			BEGIN
				SET @ErrorMessage = 'Debe ingresar una ciudad válida.';
				RAISERROR (@ErrorMessage, 16, 1);
			END

			INSERT INTO Meeting
				(AdminId, Description, Date, MaxPeople, CityId)
			VALUES
				(@AdminId, @Description, @Date, @MaxPeople, @CityId)

			DECLARE @MeetingId INT = NULL
			SET @MeetingId = @@identity;

			INSERT INTO MeetingUser (UserId, MeetingId, [Status])
			VALUES(@AdminId, @MeetingId, 1)

		END
		ELSE 
		BEGIN
			SET @ErrorMessage = 'Ya existe una meeting para los valores ingresados.';
			RAISERROR (@ErrorMessage, 16, 1);	
		END
		
		--devolemos el nuevo id
		

		SELECT
		m.MeetingId,
		m.Description,
		m.AdminId, 
		m.Date, 
		m.MaxPeople,
		m.CityId,
		c.CityId  as 'Id',
		c.Name,
		c.Latitude,
		c.Longitude
			FROM Meeting AS m WITH (NOLOCK) inner join City c WITH (NOLOCK)
			on m.CityId = c.CityId
			WHERE	m.MeetingId = @MeetingId

	END TRY
	BEGIN CATCH
		Select	@ErrorMessage = ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);  
	END CATCH
END