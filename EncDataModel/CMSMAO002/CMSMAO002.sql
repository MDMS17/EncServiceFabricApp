CREATE TABLE [Response].[MAO2Detail](
	[DetailId] [bigint] IDENTITY(1,1) NOT NULL,
	[FileId] [int] NOT NULL,
	[ClaimId] [varchar](50) NULL,
	[InternalControlNumber] [varchar](50) NULL,
	[LineNumber] [varchar](50) NULL,
	[EncounterStatus] [varchar](50) NULL,
	[ErrorCode] [varchar](50) NULL,
	[ErrorDescription] [varchar](200) NULL,
 CONSTRAINT [PK_MAO2Detail] PRIMARY KEY CLUSTERED 
(
	[DetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Response].[MAO2File](
	[FileId] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [varchar](255) NOT NULL,
	[SenderId] [varchar](50) NULL,
	[ICN] [varchar](50) NULL,
	[TransactionDate] [varchar](50) NULL,
	[RecordType] [varchar](50) NULL,
	[ProductionFlag] [varchar](50) NULL,
	[TotalErrors] [varchar](50) NULL,
	[TotalLinesAccepted] [varchar](50) NULL,
	[TotalLinesRejected] [varchar](50) NULL,
	[TotalLinesSubmitted] [varchar](50) NULL,
	[TotalEncountersAccepted] [varchar](50) NULL,
	[TotalEncountersRejected] [varchar](50) NULL,
	[TotalEncountersSubmitted] [varchar](50) NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_MAO2File] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


