USE [Meetup]
GO
/****** Object:  StoredProcedure [dbo].[spMeetingGetAll]    Script Date: 20/1/2021 16:13:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luciano Matsuo
-- Create date: 2020-01-20
-- Description:	Get user by Id
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[spUserGetById](
	@UserId INT
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		 u.UserId,
		 u.[Name],
		 u.Email,
		 u.[Role]
	FROM [User] u
		
	WHERE u.UserId = @UserId
END