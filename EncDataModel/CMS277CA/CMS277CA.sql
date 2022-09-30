CREATE SCHEMA Response;
go
CREATE TABLE [Response].[277CABillProv](
	[HeaderId] [bigint] IDENTITY(1,1) NOT NULL,
	[FileId] [int] NOT NULL,
	[BillProvName] [varchar](200) NULL,
	[BillProvIdQual] [varchar](50) NULL,
	[BillProvId] [varchar](50) NULL,
	[ClaimId] [varchar](50) NULL,
	[BillProvSecondIdQual1] [varchar](50) NULL,
	[BillProvSecondId1] [varchar](50) NULL,
	[BillProvSecondIdQual2] [varchar](50) NULL,
	[BillProvSecondId2] [varchar](50) NULL,
	[BillProvSecondIdQual3] [varchar](50) NULL,
	[BillProvSecondId3] [varchar](50) NULL,
	[BillProvAcceptedQuantity] [varchar](50) NULL,
	[BillProvRejectedQuantity] [varchar](50) NULL,
	[BillProvAcceptedAmount] [varchar](50) NULL,
	[BillProvRejectedAmount] [varchar](50) NULL,
 CONSTRAINT [PK_277CABillProv] PRIMARY KEY CLUSTERED 
(
	[HeaderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Response].[277CAFile](
	[FileId] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [varchar](255) NOT NULL,
	[SenderId] [varchar](50) NOT NULL,
	[ReceiverId] [varchar](50) NOT NULL,
	[SenderName] [varchar](200) NOT NULL,
	[ReceiverName] [varchar](200) NOT NULL,
	[TransactionDate] [varchar](50) NOT NULL,
	[TransactionTime] [varchar](50) NOT NULL,
	[ICN] [varchar](50) NOT NULL,
	[GroupControlNumber] [varchar](50) NOT NULL,
	[BatchId] [varchar](50) NOT NULL,
	[TotalAcceptedQuantity] [varchar](50) NULL,
	[TotalRejectedQuantity] [varchar](50) NULL,
	[TotalAcceptedAmount] [varchar](50) NULL,
	[TotalRejectedAmount] [varchar](50) NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_277CAFile] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Response].[277CALine](
	[LineId] [bigint] IDENTITY(1,1) NOT NULL,
	[FileId] [int] NOT NULL,
	[ClaimId] [varchar](50) NOT NULL,
	[ProcedureQual] [varchar](50) NULL,
	[ProcedureCode] [varchar](50) NULL,
	[Modifier1] [varchar](50) NULL,
	[Modifier2] [varchar](50) NULL,
	[Modifier3] [varchar](50) NULL,
	[Modifier4] [varchar](50) NULL,
	[LineChargeAmount] [varchar](50) NULL,
	[RevenueCode] [varchar](50) NULL,
	[UnitCount] [varchar](50) NULL,
	[LineItemControlNumber] [varchar](50) NULL,
	[PrescriptionNumber] [varchar](50) NULL,
	[ServiceDateFrom] [varchar](50) NULL,
	[ServiceDateTo] [varchar](50) NULL,
 CONSTRAINT [PK_277CALine] PRIMARY KEY CLUSTERED 
(
	[LineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Response].[277CAPatient](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FileId] [int] NOT NULL,
	[ClaimId] [varchar](50) NULL,
	[BillProvId] [varchar](50) NULL,
	[PatientLastName] [varchar](200) NULL,
	[PatientFirstName] [varchar](200) NULL,
	[PatientMI] [varchar](50) NULL,
	[PatientIdQual] [varchar](50) NULL,
	[PatientId] [varchar](50) NULL,
	[PayerClaimControlNumber] [varchar](50) NULL,
	[ClearingHouseTraceNumber] [varchar](50) NULL,
	[BillType] [varchar](50) NULL,
	[ServiceDateFrom] [varchar](50) NULL,
	[ServiceDateTo] [varchar](50) NULL,
 CONSTRAINT [PK_277CAPatient] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Response].[277CAStc](
	[StcId] [bigint] IDENTITY(1,1) NOT NULL,
	[StcType] [varchar](50) NOT NULL,
	[FileId] [int] NOT NULL,
	[ClaimId] [varchar](50) NULL,
	[BillProvId] [varchar](50) NULL,
	[PatientId] [varchar](50) NULL,
	[ClaimStatusCategory1] [varchar](50) NULL,
	[ClaimStatusCode1] [varchar](50) NULL,
	[EntityIDentifier1] [varchar](50) NULL,
	[StatusInfoEffDate] [varchar](50) NULL,
	[ActionCode] [varchar](50) NULL,
	[ChargeAmount] [varchar](50) NULL,
	[ClaimStatusCategory2] [varchar](50) NULL,
	[ClaimStatusCode2] [varchar](50) NULL,
	[EntityIDentifier2] [varchar](50) NULL,
	[ClaimStatusCategory3] [varchar](50) NULL,
	[ClaimStatusCode3] [varchar](50) NULL,
	[EntityIDentifier3] [varchar](50) NULL,
 CONSTRAINT [PK_277CAStc] PRIMARY KEY CLUSTERED 
(
	[StcId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


