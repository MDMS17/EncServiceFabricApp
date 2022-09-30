CREATE TABLE [dbo].[MAO_004_Header](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransmissionId] [bigint] NOT NULL,
	[ReportId] [varchar](7) NOT NULL,
	[ContractId] [varchar](5) NOT NULL,
	[ReportDate] [date] NOT NULL,
	[ReportDescription] [varchar](53) NOT NULL,
	[SubmissionFileType] [varchar](4) NOT NULL,
	[RecordCount] [int] NOT NULL,
	[Phase] [varchar](1) NOT NULL,
	[Version] [varchar](1) NOT NULL,
	[CreateUser] [varchar](20) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[FileName] [varchar](200) NULL,
 CONSTRAINT [PK_MAO_004_Header] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MAO_004_Header] ADD  CONSTRAINT [DF__MAO_004_H__Creat__571DF1D5]  DEFAULT (getdate()) FOR [CreateDate]
GO


CREATE TABLE [dbo].[MAO_004_Detail](
	[Id] [int] NOT NULL,
	[HeaderId] [int] NOT NULL,
	[ReportId] [varchar](7) NOT NULL,
	[MedicareContractId] [varchar](5) NOT NULL,
	[BeneficiaryHICN] [varchar](12) NOT NULL,
	[EncounterICN] [varchar](20) NOT NULL,
	[EncounterTypeSwitch] [varchar](1) NOT NULL,
	[OriginalEncounterICN] [varchar](20) NOT NULL,
	[OriginalEncounterStatus] [varchar](1) NOT NULL,
	[AllowDisallowFlag] [varchar](1) NOT NULL,
	[AllowDisallowReasnCode] [varchar](1) NOT NULL,
	[SubmissionDate] [date] NOT NULL,
	[ServiceStartDate] [date] NOT NULL,
	[ServiceEndDate] [date] NOT NULL,
	[ClaimType] [varchar](1) NOT NULL,
	[CreateUser] [varchar](20) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_MAO_004_Detail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MAO_004_Detail] ADD  CONSTRAINT [DF__MAO_004_D__Creat__5535A963]  DEFAULT (getdate()) FOR [CreateDate]
GO

ALTER TABLE [dbo].[MAO_004_Detail]  WITH CHECK ADD  CONSTRAINT [FK_MAO_004_Detail_To_MAO_004_Header] FOREIGN KEY([HeaderId])
REFERENCES [dbo].[MAO_004_Header] ([Id])
GO

ALTER TABLE [dbo].[MAO_004_Detail] CHECK CONSTRAINT [FK_MAO_004_Detail_To_MAO_004_Header]
GO


CREATE TABLE [dbo].[MAO_004_DiagnosisCode](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DetailId] [int] NOT NULL,
	[Code] [varchar](7) NOT NULL,
	[ICD] [varchar](1) NOT NULL,
	[Flag] [varchar](1) NOT NULL,
	[CreateUser] [varchar](20) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_MAO_004_DiagnosisCode] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MAO_004_DiagnosisCode] ADD  CONSTRAINT [DF__MAO_004_D__Creat__5629CD9C]  DEFAULT (getdate()) FOR [CreateDate]
GO

ALTER TABLE [dbo].[MAO_004_DiagnosisCode]  WITH CHECK ADD  CONSTRAINT [FK_MAO_004_DiagnosisCode_To_MAO_004_Detail] FOREIGN KEY([DetailId])
REFERENCES [dbo].[MAO_004_Detail] ([Id])
GO

ALTER TABLE [dbo].[MAO_004_DiagnosisCode] CHECK CONSTRAINT [FK_MAO_004_DiagnosisCode_To_MAO_004_Detail]
GO


