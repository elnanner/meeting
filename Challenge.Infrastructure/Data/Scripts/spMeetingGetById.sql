SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author:		<Matsuo, Luciano>
-- Create date: <19/01/2021>
-- Description:	<Devuelte una meeting por id>
-- Comments:	<Se utiliza para encontrar una meetings.>
-- =============================================
alter PROCEDURE spMeetingGetById(
	@MeetingId INT
	)
AS
BEGIN

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
				
END