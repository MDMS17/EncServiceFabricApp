create schema Response;
go
CREATE TABLE [Response].[999Element](
	[ElementId] [int] IDENTITY(1,1) NOT NULL,
	[FileId] [int] NOT NULL,
	[TransactionControlNumber] [varchar](50) NULL,
	[PositionInTransaction] [varchar](50) NULL,
	[PositionInSegment] [varchar](50) NULL,
	[ElementReferenceNumber] [varchar](50) NULL,
	[ElementErrorCode] [varchar](50) NULL,
	[ElementBadDataCopy] [varchar](255) NULL,
	[ElementSegmentCode] [varchar](50) NULL,
	[ElementSegmentPositionInTransaction] [varchar](50) NULL,
	[ElementLoopCode] [varchar](50) NULL,
	[ElementPositionInSegment] [varchar](50) NULL,
	[ElementReferenceInSegment] [varchar](50) NULL,
 CONSTRAINT [PK_999Element] PRIMARY KEY CLUSTERED 
(
	[ElementId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Response].[999Error](
	[ErrorId] [int] IDENTITY(1,1) NOT NULL,
	[FileId] [int] NOT NULL,
	[TransactionControlNumber] [varchar](50) NULL,
	[SegmentCode] [varchar](50) NULL,
	[PositionInTransaction] [varchar](50) NULL,
	[LoopCode] [varchar](50) NULL,
	[ErrorCode] [varchar](50) NULL,
	[BusinessUnitName] [varchar](50) NULL,
	[BusinessUnitCode] [varchar](50) NULL,
	[CtxSegmentCode] [varchar](50) NULL,
	[CtxPositionInTransaction] [varchar](50) NULL,
	[CtxLoopCode] [varchar](50) NULL,
	[CtxPositionInSegment] [varchar](50) NULL,
	[CtxReferenceInSegment] [varchar](50) NULL,
 CONSTRAINT [PK_999Error] PRIMARY KEY CLUSTERED 
(
	[ErrorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Response].[999File](
	[FileId] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [varchar](255) NOT NULL,
	[ReceiverId] [varchar](50) NULL,
	[SenderId] [varchar](50) NULL,
	[ICN] [varchar](50) NULL,
	[GroupControlNumber] [varchar](50) NULL,
	[TransactionDate] [varchar](50) NULL,
	[TransactionTime] [varchar](50) NULL,
	[TransactionsIncluded] [varchar](50) NULL,
	[TransactionsReceived] [varchar](50) NULL,
	[TransactionsAccepted] [varchar](50) NULL,
	[FileAckCode] [varchar](50) NULL,
	[ProductionFlag] [varchar](50) NULL,
	[FileError1] [varchar](50) NULL,
	[FileError2] [varchar](50) NULL,
	[FileError3] [varchar](50) NULL,
	[FileError4] [varchar](50) NULL,
	[FileError5] [varchar](50) NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_999File] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Response].[999Transaction](
	[TransactionId] [int] IDENTITY(1,1) NOT NULL,
	[FileId] [int] NOT NULL,
	[TransactionControlNumber] [varchar](50) NULL,
	[TransactionAckCode] [varchar](50) NULL,
	[TransactionError1] [varchar](50) NULL,
	[TransactionError2] [varchar](50) NULL,
	[TransactionError3] [varchar](50) NULL,
	[TransactionError4] [varchar](50) NULL,
	[TransactionError5] [varchar](50) NULL,
 CONSTRAINT [PK_999Transaction] PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


