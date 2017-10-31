*** MongoDB installation ***
https://docs.mongodb.com/manual/tutorial/install-mongodb-on-windows/
Adjusts connection string in Connection.cs

*** SQL Studio-like for MongoDB ***
https://mongobooster.com/ (many others exist)

*** SQL Database ***
Adjusts connection string in Connection.cs
1- Create manually DB: RelayRace
2- Create table Active Teams

USE [RelayRace]
GO

/****** Object:  Table [dbo].[ActiveTeams]    Script Date: 2017-10-10 10:01:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ActiveTeams](
	[TeamId] [uniqueidentifier] NOT NULL,
	[ChipId] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

3- Create table LapStatistics

USE [RelayRace]
GO

/****** Object:  Table [dbo].[LapStatistics]    Script Date: 2017-10-10 10:02:56 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LapStatistics](
	[Id] [uniqueidentifier] NOT NULL,
	[TeamName] [nvarchar](max) NOT NULL,
	[RaceId] [uniqueidentifier] NOT NULL,
	[TeamId] [uniqueidentifier] NOT NULL,
	[DistanceInMeters] [int] NOT NULL,
	[Pace] [nvarchar](max) NOT NULL,
	[Length] [nvarchar](max) NOT NULL,
	[CompletedOn] [date] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


4- Create table Races:

USE [RelayRace]
GO

/****** Object:  Table [dbo].[Races]    Script Date: 2017-10-10 10:03:15 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Races](
	[RaceId] [uniqueidentifier] NOT NULL,
	[Name] [nchar](100) NOT NULL,
	[LapDistanceInMeters] [int] NOT NULL,
	[Start] [date] NULL,
	[End] [date] NULL,
	[ChipIds] [nvarchar](max) NULL,
	[TeamIds] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


5- Create table Teams:

USE [RelayRace]
GO

/****** Object:  Table [dbo].[Teams]    Script Date: 2017-10-10 10:03:32 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Teams](
	[TeamId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[RaceId] [uniqueidentifier] NOT NULL,
	[ChipId] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


6- Create table Laps:

USE [RelayRace]
GO

/****** Object:  Table [dbo].[Laps]    Script Date: 2017-10-30 12:45:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Laps](
	[LapId] [uniqueidentifier] NOT NULL,
	[Start] [date] NOT NULL,
	[End] [date] NULL,
	[TeamId] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

GO





*** MongDB Indexes ***

db.lapStatistics.createIndex({RaceId: 1})
db.lapStatistics.reIndex()

db.races.createIndex({RaceId: 1})
db.races.reIndex()

db.teams.createIndex({TeamId: 1})
db.teams.reIndex()