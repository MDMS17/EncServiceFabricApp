CREATE TABLE [Response].[DHCSFile](
	[FileId] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [varchar](200) NULL,
	[EncounterFileName] [varchar](200) NULL,
	[SubmitterName] [varchar](200) NULL,
	[SubmissionDate] [varchar](200) NULL,
	[ValidationStatus] [varchar](50) NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_DHCSFile] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Response].[DHCSTransaction](
	[TransactionId] [int] IDENTITY(1,1) NOT NULL,
	[FileId] [int] NOT NULL,
	[TransactionStatus] [varchar](50) NULL,
	[TransactionNumber] [varchar](50) NULL,
	[ISAControlNumber] [varchar](50) NULL,
	[GroupControlNumber] [varchar](50) NULL,
	[OriginatorTransactionId] [varchar](50) NULL,
 CONSTRAINT [PK_DHCSTransaction] PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Response].[DHCSEncounter](
	[EncounterId] [bigint] IDENTITY(1,1) NOT NULL,
	[FileId] [int] NOT NULL,
	[TransactionNumber] [varchar](50) NULL,
	[EncounterStatus] [varchar](50) NULL,
	[EncounterReferenceNumber] [varchar](50) NULL,
	[DHCSEncounterId] [varchar](50) NULL,
 CONSTRAINT [PK_DHCSEncounter] PRIMARY KEY CLUSTERED 
(
	[EncounterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Response].[DHCSEncounterResponse](
	[EncounterResponseId] [bigint] IDENTITY(1,1) NOT NULL,
	[FileId] [int] NOT NULL,
	[TransactionNumber] [varchar](50) NULL,
	[EncounterReferenceNumber] [varchar](50) NULL,
	[Severity] [varchar](50) NULL,
	[IssueId] [varchar](50) NULL,
	[IsSNIP] [varchar](50) NULL,
	[IssueDescription] [varchar](2000) NULL,
 CONSTRAINT [PK_DHCSEncounterResponse] PRIMARY KEY CLUSTERED 
(
	[EncounterResponseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Response].[DHCSServiceLine](
	[ServiceLineId] [bigint] IDENTITY(1,1) NOT NULL,
	[FileId] [int] NOT NULL,
	[TransactionNumber] [varchar](50) NULL,
	[EncounterReferenceNumber] [varchar](50) NULL,
	[Line] [varchar](50) NULL,
	[Severity] [varchar](50) NULL,
	[Id] [varchar](50) NULL,
	[Description] [varchar](255) NULL,
 CONSTRAINT [PK_DHCSServiceLine] PRIMARY KEY CLUSTERED 
(
	[ServiceLineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


