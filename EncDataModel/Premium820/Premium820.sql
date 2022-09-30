CREATE TABLE [dbo].[File820](
	[FileId] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [varchar](200) NULL,
	[SenderId] [varchar](50) NULL,
	[ReceiverId] [varchar](50) NULL,
	[TransactionDate] [varchar](50) NULL,
	[TransactionTime] [varchar](50) NULL,
	[InterchangeControlNumber] [varchar](50) NULL,
	[TotalPremiumAmount] [varchar](50) NULL,
	[PaymentMethodQualifier] [varchar](50) NULL,
	[PaymentMethod] [varchar](50) NULL,
	[TransactionNumber] [varchar](50) NULL,
	[CheckIssueDate] [varchar](50) NULL,
	[TraceTypeCode] [varchar](50) NULL,
	[TraceNumber] [varchar](50) NULL,
	[PayeeIdQualifier] [varchar](50) NULL,
	[PayeeId] [varchar](50) NULL,
	[CoverageFirstDate] [varchar](50) NULL,
	[CoverageLastDate] [varchar](50) NULL,
	[PayeeLastName] [varchar](200) NULL,
	[PayeeAddress] [varchar](200) NULL,
	[PayeeCity] [varchar](50) NULL,
	[PayeeState] [varchar](50) NULL,
	[PayeeZip] [varchar](50) NULL,
	[PayerName] [varchar](200) NULL,
	[PayerAddress] [varchar](200) NULL,
	[PayerCity] [varchar](50) NULL,
	[PayerState] [varchar](50) NULL,
	[PayerZip] [varchar](50) NULL,
 CONSTRAINT [PK_File820] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Member820](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileId] [int] NOT NULL,
	[EntityIdQualifier] [varchar](50) NULL,
	[EntityId] [varchar](50) NULL,
	[MemberLastName] [varchar](200) NULL,
	[MemberFirstName] [varchar](200) NULL,
	[MemberMiddleName] [varchar](50) NULL,
	[MemberIdQualifier] [varchar](50) NULL,
	[MemberId] [varchar](50) NULL,
	[InsuranceRemittanceReferenceNumber] [varchar](50) NULL,
	[DetailPremiumPaymentAmount] [varchar](50) NULL,
	[BilledPremiumAmount] [varchar](50) NULL,
	[CountyCode] [varchar](50) NULL,
	[OrganizationalReferenceId] [varchar](50) NULL,
	[OrganizationalDescription] [varchar](200) NULL,
	[CapitationFromDate] [varchar](50) NULL,
	[CapitationThroughDate] [varchar](50) NULL,
	[AdjustmentAmount] [varchar](50) NULL,
	[AdjustmentReasonCode] [varchar](50) NULL,
 CONSTRAINT [PK_Member820] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

