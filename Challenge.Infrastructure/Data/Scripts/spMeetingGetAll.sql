USE [Meetup]
GO
/****** Object:  StoredProcedure [dbo].[spMeetingGetAll]    Script Date: 20/1/2021 00:38:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luciano Matsuo
-- Create date: 2020-01-18
-- Description:	Get all meetings
-- =============================================
ALTER PROCEDURE [dbo].[spMeetingGetAll](
	@adminId int = null,
	@date datetime = null,
	@description varchar(50) = null
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
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
		
	FROM meeting m inner join City c 
	on m.cityid = c.CityId
	WHERE m.AdminId = ISNULL(@adminId, m.AdminId)
	and m.description like ISNULL('%'+@description+'%', m.description)
	and CONVERT(varchar(10), m.Date, 126) = ISNULL(CONVERT(varchar(10),@date, 126), CONVERT(varchar(10), m.Date, 126))
	order by m.Meetingid
END