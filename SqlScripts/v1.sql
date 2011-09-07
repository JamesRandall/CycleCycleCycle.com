USE [CycleCycleCycle]
GO

/****** Object:  Table [dbo].[Accounts]    Script Date: 08/29/2011 17:44:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Accounts](
	[AccountID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC
)WITH ( STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF) 
) 

GO

USE [CycleCycleCycle]
GO

/****** Object:  Table [dbo].[Favourites]    Script Date: 08/29/2011 17:44:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Favourites](
	[FavouriteID] [int] IDENTITY(1,1) NOT NULL,
	[RouteID] [int] NOT NULL,
	[AccountID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[FavouriteID] ASC
)WITH ( STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF) 
) 

GO

USE [CycleCycleCycle]
GO

/****** Object:  Table [dbo].[Rides]    Script Date: 08/29/2011 17:44:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Rides](
	[RideID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NOT NULL,
	[RouteID] [int] NOT NULL,
	[TimeOfRide] [datetime] NOT NULL,
	[TimeTaken] [time](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[RideID] ASC
)WITH ( STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF) 
) 

GO

USE [CycleCycleCycle]
GO

/****** Object:  Table [dbo].[RoutePoints]    Script Date: 08/29/2011 17:44:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RoutePoints](
	[RoutePointID] [int] IDENTITY(1,1) NOT NULL,
	[RouteID] [int] NOT NULL,
	[Longitude] [float] NOT NULL,
	[Latitude] [float] NOT NULL,
	[Elevation] [float] NOT NULL,
	[TimeRecorded] [datetime] NOT NULL,
	[SequenceIndex] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoutePointID] ASC
)WITH ( STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF) 
) 

GO

USE [CycleCycleCycle]
GO

/****** Object:  Table [dbo].[RouteReviews]    Script Date: 08/29/2011 17:44:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RouteReviews](
	[RouteReviewID] [int] IDENTITY(1,1) NOT NULL,
	[RouteID] [int] NOT NULL,
	[AccountID] [int] NOT NULL,
	[Rating] [int] NOT NULL,
	[Review] [nvarchar](1024) NULL,
	[DateCreated] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RouteReviewID] ASC
)WITH ( STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF) 
) 

GO

USE [CycleCycleCycle]
GO

/****** Object:  Table [dbo].[Routes]    Script Date: 08/29/2011 17:44:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Routes](
	[RouteID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NULL,
	[Name] [nvarchar](max) NULL,
	[DateCreated] [datetime] NOT NULL,
	[Distance] [float] NOT NULL,
	[TotalAscent] [float] NOT NULL,
	[TotalDescent] [float] NOT NULL,
	[AverageRating] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[RouteID] ASC
)WITH ( STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF) 
) 

GO

ALTER TABLE [dbo].[Favourites]  WITH CHECK ADD  CONSTRAINT [Favourite_Account] FOREIGN KEY([AccountID])
REFERENCES [dbo].[Accounts] ([AccountID])
GO

ALTER TABLE [dbo].[Favourites] CHECK CONSTRAINT [Favourite_Account]
GO

ALTER TABLE [dbo].[Favourites]  WITH CHECK ADD  CONSTRAINT [Favourite_Route] FOREIGN KEY([RouteID])
REFERENCES [dbo].[Routes] ([RouteID])
GO

ALTER TABLE [dbo].[Favourites] CHECK CONSTRAINT [Favourite_Route]
GO

ALTER TABLE [dbo].[Rides]  WITH CHECK ADD  CONSTRAINT [Ride_Account] FOREIGN KEY([AccountID])
REFERENCES [dbo].[Accounts] ([AccountID])
GO

ALTER TABLE [dbo].[Rides] CHECK CONSTRAINT [Ride_Account]
GO

ALTER TABLE [dbo].[Rides]  WITH CHECK ADD  CONSTRAINT [Ride_Route] FOREIGN KEY([RouteID])
REFERENCES [dbo].[Routes] ([RouteID])
GO

ALTER TABLE [dbo].[Rides] CHECK CONSTRAINT [Ride_Route]
GO

ALTER TABLE [dbo].[RoutePoints]  WITH CHECK ADD  CONSTRAINT [RoutePoint_Route] FOREIGN KEY([RouteID])
REFERENCES [dbo].[Routes] ([RouteID])
GO

ALTER TABLE [dbo].[RoutePoints] CHECK CONSTRAINT [RoutePoint_Route]
GO

ALTER TABLE [dbo].[RouteReviews]  WITH CHECK ADD  CONSTRAINT [RouteReview_Account] FOREIGN KEY([AccountID])
REFERENCES [dbo].[Accounts] ([AccountID])
GO

ALTER TABLE [dbo].[RouteReviews] CHECK CONSTRAINT [RouteReview_Account]
GO

ALTER TABLE [dbo].[RouteReviews]  WITH CHECK ADD  CONSTRAINT [RouteReview_Route] FOREIGN KEY([RouteID])
REFERENCES [dbo].[Routes] ([RouteID])
GO

ALTER TABLE [dbo].[RouteReviews] CHECK CONSTRAINT [RouteReview_Route]
GO

ALTER TABLE [dbo].[Routes]  WITH CHECK ADD  CONSTRAINT [Account_Routes] FOREIGN KEY([AccountID])
REFERENCES [dbo].[Accounts] ([AccountID])
GO

ALTER TABLE [dbo].[Routes] CHECK CONSTRAINT [Account_Routes]
GO


