
CREATE TABLE [dbo].[Mao001Detail](
	[DetailId] [int] IDENTITY(1,1) NOT NULL,
	[HeaderId] [int] NOT NULL,
	[ContractId] [varchar](50) NULL,
	[EncounterId] [varchar](50) NULL,
	[InternalControlNumber] [varchar](50) NULL,
	[LineNumber] [varchar](50) NULL,
	[DupEncounterId] [varchar](50) NULL,
	[DupInternalControlNumber] [varchar](50) NULL,
	[DupLineNumber] [varchar](50) NULL,
	[HICN] [varchar](50) NULL,
	[DateOfService] [varchar](50) NULL,
	[ErrorCode] [varchar](50) NULL,
 CONSTRAINT [PK_Mao001Detail] PRIMARY KEY CLUSTERED 
(
	[DetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Mao001Header](
	[HeaderId] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [varchar](200) NULL,
	[SenderId] [varchar](50) NULL,
	[InterchangeControlNumber] [varchar](50) NULL,
	[InterchangeDate] [varchar](50) NULL,
	[RecordType] [varchar](50) NULL,
	[ProductionFlag] [varchar](50) NULL,
	[TotalDupLines] [varchar](50) NULL,
	[TotalLines] [varchar](50) NULL,
	[TotalEncounters] [varchar](50) NULL,
 CONSTRAINT [PK_Mao001Header] PRIMARY KEY CLUSTERED 
(
	[HeaderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


