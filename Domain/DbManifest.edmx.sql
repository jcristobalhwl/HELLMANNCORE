
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/03/2020 08:30:45
-- Generated from EDMX file: C:\Users\jcristobal\source\repos\CORESOLUTION\Domain\DbManifest.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DB_MANIFEST];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[TBL_ADU_ADUANAAGENT]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TBL_ADU_ADUANAAGENT];
GO
IF OBJECT_ID(N'[dbo].[TBL_ADU_ADUANADESTINATION]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TBL_ADU_ADUANADESTINATION];
GO
IF OBJECT_ID(N'[dbo].[TBL_ADU_MANIFEST]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TBL_ADU_MANIFEST];
GO
IF OBJECT_ID(N'[dbo].[TBL_ADU_MANIFESTSHIPMENTDETAILDOC]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TBL_ADU_MANIFESTSHIPMENTDETAILDOC];
GO
IF OBJECT_ID(N'[dbo].[TBL_ADU_MANIFESTSHIPMENTDOC]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TBL_ADU_MANIFESTSHIPMENTDOC];
GO
IF OBJECT_ID(N'[dbo].[TBL_ADU_MASTERINFORMATION]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TBL_ADU_MASTERINFORMATION];
GO
IF OBJECT_ID(N'[dbo].[TBL_ADU_TRACK]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TBL_ADU_TRACK];
GO
IF OBJECT_ID(N'[dbo].[TBL_ADU_WAREDESCRIPTION]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TBL_ADU_WAREDESCRIPTION];
GO
IF OBJECT_ID(N'[dbo].[TBL_ADU_WEBTRACKING]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TBL_ADU_WEBTRACKING];
GO
IF OBJECT_ID(N'[dbo].[TBL_MAN_MANIFEST]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TBL_MAN_MANIFEST];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'TBL_ADU_ADUANAAGENT'
CREATE TABLE [dbo].[TBL_ADU_ADUANAAGENT] (
    [INT_ADUANAAGENTID] int IDENTITY(1,1) NOT NULL,
    [VCH_SUNATCODE] varchar(8)  NULL,
    [VCH_BUSINESSNAME] varchar(800)  NULL,
    [VCH_SUNATJURISDICTIONCODE] varchar(8)  NULL,
    [VCH_JURISDICTION] varchar(400)  NULL,
    [VCH_STATE] varchar(200)  NULL
);
GO

-- Creating table 'TBL_ADU_ADUANADESTINATION'
CREATE TABLE [dbo].[TBL_ADU_ADUANADESTINATION] (
    [NUM_ADUANADESTINATIONID] decimal(18,0) IDENTITY(1,1) NOT NULL,
    [VCH_MANIFEST] varchar(max)  NULL,
    [INT_SEC] int  NULL,
    [VCH_DUANUMBER] varchar(max)  NULL,
    [VCH_DATENUMBERING] varchar(23)  NULL,
    [INT_DETAIL] int  NULL,
    [VCH_AGENT] varchar(max)  NULL,
    [NUM_WEIGHTREQUESTED] decimal(18,3)  NULL,
    [INT_PACKAGEREQUESTED] int  NULL,
    [VCH_DATEDATED] varchar(23)  NULL,
    [DEC_MANIFESTSHIPDETDOCID] decimal(18,0)  NULL
);
GO

-- Creating table 'TBL_ADU_MANIFEST'
CREATE TABLE [dbo].[TBL_ADU_MANIFEST] (
    [NUM_MANIFESTID] decimal(18,0) IDENTITY(1,1) NOT NULL,
    [VCH_MANIFESTNUMBER] varchar(max)  NULL,
    [VCH_TERMINAL] varchar(max)  NULL,
    [VCH_AIRLINE] varchar(max)  NULL,
    [VCH_SHIPMENTPORT] varchar(max)  NULL,
    [VCH_FLIGHTNUMBER] varchar(max)  NULL,
    [DAT_DEPARTUREDATE] datetime  NULL,
    [CHR_REGIME] char(1)  NULL,
    [CHR_VIA] char(1)  NULL,
    [BIT_COMPLETED] bit  NULL,
    [DAT_DOWNLOADDATE] datetime  NULL
);
GO

-- Creating table 'TBL_ADU_MANIFESTSHIPMENTDETAILDOC'
CREATE TABLE [dbo].[TBL_ADU_MANIFESTSHIPMENTDETAILDOC] (
    [NUM_MANIFESTID] decimal(18,0)  NULL,
    [VCH_AIRGUIDE] varchar(max)  NULL,
    [VCH_DIRECTMASTERGUIDE] varchar(max)  NULL,
    [VCH_DETAIL] int  NULL,
    [VCH_TERMINALCODE] varchar(max)  NULL,
    [DEC_WEIGHTORIGIN] decimal(18,3)  NULL,
    [INT_PACKAGEORIGIN] int  NULL,
    [DEC_WEIGHTRECEIVED] decimal(18,3)  NULL,
    [INT_PACKAGERECEIVED] int  NULL,
    [VCH_CONSIGNEE] varchar(max)  NULL,
    [DAT_DATETRANSMISSIONDOCUMENT] datetime  NULL,
    [VCH_DESCRIPTION] varchar(max)  NULL,
    [DEC_MANIFESTEDWEIGHT] decimal(18,3)  NULL,
    [INT_MANIFESTEDPACKAGE] int  NULL,
    [VCH_SHIPPER] varchar(max)  NULL,
    [DEC_MANIFESTSHIPDETDOCID] decimal(18,0) IDENTITY(1,1) NOT NULL,
    [BIT_COMPLETED] bit  NULL
);
GO

-- Creating table 'TBL_ADU_MANIFESTSHIPMENTDOC'
CREATE TABLE [dbo].[TBL_ADU_MANIFESTSHIPMENTDOC] (
    [NUM_MANIFESTSHIPMENTDOCID] decimal(18,0) IDENTITY(1,1) NOT NULL,
    [NUM_MANIFESTID] decimal(18,0)  NULL,
    [VCH_MANIFESTNUMBER] varchar(max)  NULL,
    [INT_DETAILSNUMBER] int  NULL,
    [DAT_DEPARTUREDATE] varchar(max)  NULL,
    [DEC_GROSSWEIGHT] decimal(18,2)  NULL,
    [VCH_AIRLINE] varchar(max)  NULL,
    [VCH_NATIONALITY] varchar(max)  NULL,
    [VCH_FLIGHTNUMBER] varchar(max)  NULL,
    [INT_PACKAGES] int  NULL,
    [VCH_FINALDATEBOARD] varchar(max)  NULL,
    [VCH_DATEAUTHORIZATIONBOARD] varchar(max)  NULL,
    [VCH_MANIFESTTRANSMISSIONDATE] varchar(max)  NULL
);
GO

-- Creating table 'TBL_ADU_MASTERINFORMATION'
CREATE TABLE [dbo].[TBL_ADU_MASTERINFORMATION] (
    [DEC_MASTERINFORMATIONID] decimal(18,0) IDENTITY(1,1) NOT NULL,
    [VCH_MANIFEST] varchar(max)  NULL,
    [INT_DETAILNUMBER] int  NULL,
    [VCH_TERMINALCODE] varchar(max)  NULL,
    [VCH_SHIPMENTPORT] varchar(max)  NULL,
    [DEC_WEIGHTORIGIN] decimal(18,3)  NULL,
    [DEC_WEIGHTRECEIVED] decimal(18,0)  NULL,
    [VCH_CONSIGNEE] varchar(max)  NULL,
    [VCH_DESTINATIONPORT] varchar(max)  NULL,
    [VCH_TRANSMISSIONDATE] varchar(23)  NULL,
    [DEC_MANIFESTSHIPDETDOCID] decimal(18,0)  NULL,
    [VCH_SHIPPER] varchar(max)  NULL,
    [DEC_MANIFESTEDWEIGHT] decimal(18,3)  NULL,
    [INT_PACKAGEORIGIN] int  NULL,
    [INT_PACKAGERECEIVED] int  NULL,
    [INT_MANIFESTEDPACKAGE] int  NULL
);
GO

-- Creating table 'TBL_ADU_TRACK'
CREATE TABLE [dbo].[TBL_ADU_TRACK] (
    [INT_TRACKID] int IDENTITY(1,1) NOT NULL,
    [NUM_MANIFESTSHIPDETDOCID] decimal(18,0)  NULL,
    [VCH_DIRECTMASTERGUIDE] varchar(max)  NULL,
    [VCH_ORIGIN] varchar(10)  NULL,
    [VCH_DESTINATION] varchar(10)  NULL,
    [VCH_CONNECTION] varchar(10)  NULL,
    [INT_PIECES] int  NULL,
    [NUM_WEIGHT] decimal(18,3)  NULL,
    [NUM_VOLUME] decimal(18,3)  NULL,
    [VCH_PRODUCT] varchar(max)  NULL,
    [VCH_STATUS] varchar(125)  NULL,
    [VCH_CONFIRMATION] varchar(125)  NULL,
    [VCH_SERVICE] varchar(50)  NULL
);
GO

-- Creating table 'TBL_ADU_WAREDESCRIPTION'
CREATE TABLE [dbo].[TBL_ADU_WAREDESCRIPTION] (
    [DEC_WAREDESCRIPTION] decimal(18,0) IDENTITY(1,1) NOT NULL,
    [VCH_MANIFEST] varchar(max)  NULL,
    [INT_PACKAGES] int  NULL,
    [DEC_GROSSWEIGHT] decimal(18,3)  NULL,
    [VCH_PACKING] varchar(max)  NULL,
    [VCH_CONSIGNEE] varchar(max)  NULL,
    [VCH_MARKSANDNUMBERS] varchar(max)  NULL,
    [VCH_DESCRIPTION] varchar(max)  NULL,
    [DEC_MANIFESTSHIPDETDOCID] decimal(18,0)  NULL,
    [VCH_SHIPPER] varchar(max)  NULL
);
GO

-- Creating table 'TBL_ADU_WEBTRACKING'
CREATE TABLE [dbo].[TBL_ADU_WEBTRACKING] (
    [INT_WEBTRACKINGID] int IDENTITY(1,1) NOT NULL,
    [CHR_PREFIX] char(3)  NULL,
    [VCH_AIRLINE] varchar(50)  NULL,
    [VCH_LINK] varchar(200)  NULL
);
GO

-- Creating table 'TBL_MAN_MANIFEST'
CREATE TABLE [dbo].[TBL_MAN_MANIFEST] (
    [INT_MANIFESTID] int IDENTITY(1,1) NOT NULL,
    [INT_DAY] int  NULL,
    [INT_MONTH] int  NULL,
    [INT_YEAR] int  NULL,
    [INT_WEEK] int  NULL,
    [VCH_AIRLINE] varchar(20)  NULL,
    [VCH_FLIGHTNUMBER] varchar(20)  NULL,
    [VCH_AIRGUIDE] varchar(20)  NULL,
    [VCH_DIRECTMASTERGUIDE] varchar(20)  NULL,
    [VCH_DESCRIPTION] varchar(max)  NULL,
    [VCH_TERMINALCODE] varchar(20)  NULL,
    [DEC_WEIGHTORIGIN] decimal(6,2)  NULL,
    [INT_PACKAGEORIGIN] int  NULL,
    [DEC_MANIFESTEDWEIGHT] decimal(6,2)  NULL,
    [INT_MANIFESTEDPACKAGE] int  NULL,
    [DEC_WEIGHTRECEIVED] decimal(6,2)  NULL,
    [INT_PACKAGERECEIVED] int  NULL,
    [VCH_CONSIGNEE] varchar(50)  NULL,
    [DAT_DATETRANSMISSIONDOCUMENT] datetime  NULL,
    [VCH_ANOTHERAGENT] varchar(50)  NULL,
    [VCH_DESTINATION] varchar(50)  NULL,
    [VCH_SHIPPER] varchar(50)  NULL,
    [VCH_ORIGIN] varchar(50)  NULL,
    [DAT_DEPARTUREDATE] datetime  NULL,
    [VCH_MANIFESTNUMBER] varchar(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [INT_ADUANAAGENTID] in table 'TBL_ADU_ADUANAAGENT'
ALTER TABLE [dbo].[TBL_ADU_ADUANAAGENT]
ADD CONSTRAINT [PK_TBL_ADU_ADUANAAGENT]
    PRIMARY KEY CLUSTERED ([INT_ADUANAAGENTID] ASC);
GO

-- Creating primary key on [NUM_ADUANADESTINATIONID] in table 'TBL_ADU_ADUANADESTINATION'
ALTER TABLE [dbo].[TBL_ADU_ADUANADESTINATION]
ADD CONSTRAINT [PK_TBL_ADU_ADUANADESTINATION]
    PRIMARY KEY CLUSTERED ([NUM_ADUANADESTINATIONID] ASC);
GO

-- Creating primary key on [NUM_MANIFESTID] in table 'TBL_ADU_MANIFEST'
ALTER TABLE [dbo].[TBL_ADU_MANIFEST]
ADD CONSTRAINT [PK_TBL_ADU_MANIFEST]
    PRIMARY KEY CLUSTERED ([NUM_MANIFESTID] ASC);
GO

-- Creating primary key on [DEC_MANIFESTSHIPDETDOCID] in table 'TBL_ADU_MANIFESTSHIPMENTDETAILDOC'
ALTER TABLE [dbo].[TBL_ADU_MANIFESTSHIPMENTDETAILDOC]
ADD CONSTRAINT [PK_TBL_ADU_MANIFESTSHIPMENTDETAILDOC]
    PRIMARY KEY CLUSTERED ([DEC_MANIFESTSHIPDETDOCID] ASC);
GO

-- Creating primary key on [NUM_MANIFESTSHIPMENTDOCID] in table 'TBL_ADU_MANIFESTSHIPMENTDOC'
ALTER TABLE [dbo].[TBL_ADU_MANIFESTSHIPMENTDOC]
ADD CONSTRAINT [PK_TBL_ADU_MANIFESTSHIPMENTDOC]
    PRIMARY KEY CLUSTERED ([NUM_MANIFESTSHIPMENTDOCID] ASC);
GO

-- Creating primary key on [DEC_MASTERINFORMATIONID] in table 'TBL_ADU_MASTERINFORMATION'
ALTER TABLE [dbo].[TBL_ADU_MASTERINFORMATION]
ADD CONSTRAINT [PK_TBL_ADU_MASTERINFORMATION]
    PRIMARY KEY CLUSTERED ([DEC_MASTERINFORMATIONID] ASC);
GO

-- Creating primary key on [INT_TRACKID] in table 'TBL_ADU_TRACK'
ALTER TABLE [dbo].[TBL_ADU_TRACK]
ADD CONSTRAINT [PK_TBL_ADU_TRACK]
    PRIMARY KEY CLUSTERED ([INT_TRACKID] ASC);
GO

-- Creating primary key on [DEC_WAREDESCRIPTION] in table 'TBL_ADU_WAREDESCRIPTION'
ALTER TABLE [dbo].[TBL_ADU_WAREDESCRIPTION]
ADD CONSTRAINT [PK_TBL_ADU_WAREDESCRIPTION]
    PRIMARY KEY CLUSTERED ([DEC_WAREDESCRIPTION] ASC);
GO

-- Creating primary key on [INT_WEBTRACKINGID] in table 'TBL_ADU_WEBTRACKING'
ALTER TABLE [dbo].[TBL_ADU_WEBTRACKING]
ADD CONSTRAINT [PK_TBL_ADU_WEBTRACKING]
    PRIMARY KEY CLUSTERED ([INT_WEBTRACKINGID] ASC);
GO

-- Creating primary key on [INT_MANIFESTID] in table 'TBL_MAN_MANIFEST'
ALTER TABLE [dbo].[TBL_MAN_MANIFEST]
ADD CONSTRAINT [PK_TBL_MAN_MANIFEST]
    PRIMARY KEY CLUSTERED ([INT_MANIFESTID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------