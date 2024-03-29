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
-- Description:	Get all users
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[spUserGetAll](
	@Name VARCHAR(50) = null,
	@Email VARCHAR(50) = null,
	@Role VARCHAR(20) = null
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
		
	WHERE u.Name like '%'+ ISNULL(@Name, u.Name)+ '%'
	and u.Email like '%'+ ISNULL(@Email, u.Email)+ '%'
	and u.Role like '%'+ ISNULL(@Role, u.Role) +'%'
	
END