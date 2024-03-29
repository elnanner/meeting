USE [Meetup]
GO
/****** Object:  User [meetup]    Script Date: 23/1/2021 13:13:37 ******/
CREATE USER [meetup] FOR LOGIN [meetup] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[City]    Script Date: 23/1/2021 13:13:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[City](
	[CityId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
 CONSTRAINT [PK_Cities] PRIMARY KEY CLUSTERED 
(
	[CityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Meeting]    Script Date: 23/1/2021 13:13:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Meeting](
	[MeetingId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](100) NOT NULL,
	[AdminId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[MaxPeople] [int] NOT NULL,
	[CityId] [int] NOT NULL,
 CONSTRAINT [PK_Meeting] PRIMARY KEY CLUSTERED 
(
	[MeetingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MeetingUser]    Script Date: 23/1/2021 13:13:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MeetingUser](
	[MeetingId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Status] [tinyint] NOT NULL,
 CONSTRAINT [PK_MeetingUser] PRIMARY KEY CLUSTERED 
(
	[MeetingId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 23/1/2021 13:13:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Role] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Userlogin]    Script Date: 23/1/2021 13:13:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Userlogin](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Password] [varchar](8) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[spMeetingAttended]    Script Date: 23/1/2021 13:13:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Author:		<Matsuo, Luciano>
-- Create date: <20/01/2021>
-- Description:	<Indica que un usuario asistirá a una meeting>
-- Comments:	<Se utiliza para indicar que asistirán a una meeting.>
-- =============================================
CREATE   PROCEDURE [dbo].[spMeetingAttended](
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
GO
/****** Object:  StoredProcedure [dbo].[spMeetingDelete]    Script Date: 23/1/2021 13:13:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author:		<Matsuo, Luciano>
-- Create date: <19/01/2021>
-- Description:	<Elimina una meeting>
-- Comments:	<Se utiliza para eliminar meetings.>
-- =============================================
CREATE PROCEDURE [dbo].[spMeetingDelete](
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
	Declare @IdCliente		int;

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
GO
/****** Object:  StoredProcedure [dbo].[spMeetingGetAll]    Script Date: 23/1/2021 13:13:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luciano Matsuo
-- Create date: 2020-01-18
-- Description:	Get all meetings
-- =============================================
CREATE PROCEDURE [dbo].[spMeetingGetAll](
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
GO
/****** Object:  StoredProcedure [dbo].[spMeetingGetAttendedCount]    Script Date: 23/1/2021 13:13:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Author:		<Matsuo, Luciano>
-- Create date: <20/01/2021>
-- Description:	<Devuelve la cantidad de inscriptos y con status = @Status>
-- Comments:	<Se utiliza para indicar la cantidad de invitados que asistirán a una meeting.>
-- =============================================
CREATE   PROCEDURE [dbo].[spMeetingGetAttendedCount](
	@MeetingId INT,
	@Status INT
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
			
		SELECT COUNT(*)
			FROM Meeting m INNER JOIN MeetingUser mu
			ON m.MeetingId = mu.MeetingId
			WHERE m.MeetingId = @MeetingId
				AND mu.Status = @Status

END
GO
/****** Object:  StoredProcedure [dbo].[spMeetingGetById]    Script Date: 23/1/2021 13:13:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author:		<Matsuo, Luciano>
-- Create date: <19/01/2021>
-- Description:	<Devuelte una meeting por id>
-- Comments:	<Se utiliza para encontrar una meetings.>
-- =============================================
CREATE PROCEDURE [dbo].[spMeetingGetById](
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
GO
/****** Object:  StoredProcedure [dbo].[spMeetingInsert]    Script Date: 23/1/2021 13:13:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author:		<Matsuo, Luciano>
-- Create date: <19/01/2021>
-- Description:	<Si la meetin no existe la inserta, sino informa el error>
-- Comments:	<Se utiliza para el alta de meetings.>
-- =============================================
CREATE   PROCEDURE [dbo].[spMeetingInsert](
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
GO
/****** Object:  StoredProcedure [dbo].[spMeetingUpdate]    Script Date: 23/1/2021 13:13:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author:		<Matsuo, Luciano>
-- Create date: <19/01/2021>
-- Description:	<Actualiza una meeting>
-- Comments:	<Se utiliza para el update de meetings.>
-- =============================================
CREATE   PROCEDURE [dbo].[spMeetingUpdate](
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
GO
/****** Object:  StoredProcedure [dbo].[spMeetingWillAttend]    Script Date: 23/1/2021 13:13:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Author:		<Matsuo, Luciano>
-- Create date: <20/01/2021>
-- Description:	<Indica que un usuario asistirá a una meeting>
-- Comments:	<Se utiliza para indicar que asistirán a una meeting.>
-- =============================================
CREATE   PROCEDURE [dbo].[spMeetingWillAttend](
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
GO
/****** Object:  StoredProcedure [dbo].[spUserGetAll]    Script Date: 23/1/2021 13:13:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luciano Matsuo
-- Create date: 2020-01-20
-- Description:	Get all users
-- =============================================
CREATE   PROCEDURE [dbo].[spUserGetAll](
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
GO
/****** Object:  StoredProcedure [dbo].[spUserGetById]    Script Date: 23/1/2021 13:13:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luciano Matsuo
-- Create date: 2020-01-20
-- Description:	Get user by Id
-- =============================================
CREATE   PROCEDURE [dbo].[spUserGetById](
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
GO
/****** Object:  StoredProcedure [dbo].[spUserValidate]    Script Date: 23/1/2021 13:13:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author:		<Matsuo, Luciano>
-- Create date: <20/01/2021>
-- Description:	<Valida un usuario>
-- Comments:	<Se utiliza pra el login>
-- =============================================
CREATE   PROCEDURE [dbo].[spUserValidate](
	@Username VARCHAR(50),
	@Password VARCHAR(50)
	)
AS
BEGIN
SET NOCOUNT ON;

    Declare	@ErrorMessage	nvarchar(4000);  
    Declare @ErrorSeverity	int;  
    Declare @ErrorState		int;  

	BEGIN TRY
		-- Insert statements for procedure here
		IF((SELECT	COUNT(1) 
			FROM UserLogin AS u WITH (NOLOCK)
			WHERE	u.Username = UPPER(rtrim(LTrim(@Username)))
					AND u.Password = UPPER(rtrim(LTrim(@Password)))
				) = 1)
		BEGIN
			--SET @ErrorMessage = 'Usuario inexistente.';
			--RAISERROR (@ErrorMessage, 16, 1);	
		--END
		
		SELECT * FROM [User] as u WHERE u.Email = UPPER(rtrim(LTrim(@Username)))
END
	END TRY
	BEGIN CATCH
		Select	@ErrorMessage = ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);  
	END CATCH
				
END
GO
