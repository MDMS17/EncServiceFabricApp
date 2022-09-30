CREATE TABLE [dbo].[P274AffiliatedEntity](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileId] [int] NOT NULL,
	[DetailId] [varchar](50) NULL,
	[EntityQualifier] [varchar](50) NULL,
	[EntityTypeQualifier] [varchar](50) NULL,
	[EntityLastName] [varchar](50) NULL,
	[EntityIdQualifier] [varchar](50) NULL,
	[EntityId] [varchar](50) NULL,
	[EntityAdditionalName1] [varchar](50) NULL,
	[EntityAdditionalName2] [varchar](50) NULL,
	[HospitalStatusCode] [varchar](50) NULL,
	[HospitalIdQualifier] [varchar](50) NULL,
	[HospitalIdCode] [varchar](50) NULL,
 CONSTRAINT [PK_P274AffiliatedEntity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[P274Detail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileId] [int] NOT NULL,
	[DetailId] [varchar](50) NULL,
	[ProviderQualifier] [varchar](50) NULL,
	[ProviderTypeQualifier] [varchar](50) NULL,
	[ProviderLastName] [varchar](200) NULL,
	[ProviderFirstName] [varchar](50) NULL,
	[ProviderMiddleName] [varchar](50) NULL,
	[ProviderSuffix] [varchar](50) NULL,
	[ProviderIdQualifier] [varchar](50) NULL,
	[ProviderId] [varchar](50) NULL,
	[ProviderAdditionalName1] [varchar](200) NULL,
	[ProviderAdditionalName2] [varchar](200) NULL,
	[ProviderGender] [varchar](50) NULL,
	[ProviderDegreeCode1] [varchar](50) NULL,
	[ProviderDegreeDescription1] [varchar](50) NULL,
	[ProviderDegreeCode2] [varchar](50) NULL,
	[ProviderDegreeDescription2] [varchar](50) NULL,
	[ProviderDegreeCode3] [varchar](50) NULL,
	[ProviderDegreeDescription3] [varchar](50) NULL,
	[ProviderDegreeCode4] [varchar](50) NULL,
	[ProviderDegreeDescription4] [varchar](50) NULL,
	[ProviderDegreeCode5] [varchar](50) NULL,
	[ProviderDegreeDescription5] [varchar](50) NULL,
	[ProviderDegreeCode6] [varchar](50) NULL,
	[ProviderDegreeDescription6] [varchar](50) NULL,
	[ProviderDegreeCode7] [varchar](50) NULL,
	[ProviderDegreeDescription7] [varchar](50) NULL,
	[ProviderDegreeCode8] [varchar](50) NULL,
	[ProviderDegreeDescription8] [varchar](50) NULL,
	[ProviderDegreeCode9] [varchar](50) NULL,
	[ProviderDegreeDescription9] [varchar](50) NULL,
	[ProviderLanguage1CodeQualifier] [varchar](50) NULL,
	[ProviderLanguage1Code] [varchar](50) NULL,
	[ProviderLanguage1ProficiencyInd] [varchar](50) NULL,
	[ProviderLanguage2CodeQualifier] [varchar](50) NULL,
	[ProviderLanguage2Code] [varchar](50) NULL,
	[ProviderLanguage2ProficiencyInd] [varchar](50) NULL,
	[ProviderLanguage3CodeQualifier] [varchar](50) NULL,
	[ProviderLanguage3Code] [varchar](50) NULL,
	[ProviderLanguage3ProficiencyInd] [varchar](50) NULL,
	[ProviderLanguage4CodeQualifier] [varchar](50) NULL,
	[ProviderLanguage4Code] [varchar](50) NULL,
	[ProviderLanguage4ProficiencyInd] [varchar](50) NULL,
	[ProviderLanguage5CodeQualifier] [varchar](50) NULL,
	[ProviderLanguage5Code] [varchar](50) NULL,
	[ProviderLanguage5ProficiencyInd] [varchar](50) NULL,
	[ProviderLanguage6CodeQualifier] [varchar](50) NULL,
	[ProviderLanguage6Code] [varchar](50) NULL,
	[ProviderLanguage6ProficiencyInd] [varchar](50) NULL,
	[ProviderLanguage7CodeQualifier] [varchar](50) NULL,
	[ProviderLanguage7Code] [varchar](50) NULL,
	[ProviderLanguage7ProficiencyInd] [varchar](50) NULL,
	[ProviderLanguage8CodeQualifier] [varchar](50) NULL,
	[ProviderLanguage8Code] [varchar](50) NULL,
	[ProviderLanguage8ProficiencyInd] [varchar](50) NULL,
	[ProviderLanguage9CodeQualifier] [varchar](50) NULL,
	[ProviderLanguage9Code] [varchar](50) NULL,
	[ProviderLanguage9ProficiencyInd] [varchar](50) NULL,
	[ProviderContractEffectiveDate] [varchar](50) NULL,
	[ProviderContractExpirationDate] [varchar](50) NULL,
	[SiteContact1FunctionCode] [varchar](50) NULL,
	[SiteContact1Qualifier1] [varchar](50) NULL,
	[SiteContact1Number1] [varchar](50) NULL,
	[SiteContact1Qualifier2] [varchar](50) NULL,
	[SiteContact1Number2] [varchar](50) NULL,
	[SiteContact1Qualifier3] [varchar](50) NULL,
	[SiteContact1Number3] [varchar](50) NULL,
	[SiteContact2FunctionCode] [varchar](50) NULL,
	[SiteContact2Qualifier1] [varchar](50) NULL,
	[SiteContact2Number1] [varchar](50) NULL,
	[SiteContact2Qualifier2] [varchar](50) NULL,
	[SiteContact2Number2] [varchar](50) NULL,
	[SiteContact2Qualifier3] [varchar](50) NULL,
	[SiteContact2Number3] [varchar](50) NULL,
	[SitePatientAcceptanceCategory] [varchar](50) NULL,
	[SitePatientAcceptanceCondition] [varchar](50) NULL,
	[SitePatientAcceptanceCode] [varchar](50) NULL,
	[SiteRestrictionGender] [varchar](50) NULL,
	[SiteRestrictionMinAge] [varchar](50) NULL,
	[SiteRestrictionMaxAge] [varchar](50) NULL,
	[SiteLocationCode] [varchar](50) NULL,
	[SiteLocationAddress] [varchar](50) NULL,
	[SiteLocationAddress2] [varchar](50) NULL,
	[SiteLocationCity] [varchar](50) NULL,
	[SiteLocationState] [varchar](50) NULL,
	[SiteLocationZip] [varchar](50) NULL,
	[SiteLocationCountry] [varchar](50) NULL,
	[GroupId] [varchar](50) NULL,
	[SiteId] [varchar](50) NULL,
 CONSTRAINT [PK_P274Detail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[P274File](
	[FileId] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [varchar](200) NULL,
	[SenderId] [varchar](50) NULL,
	[ReceiverId] [varchar](50) NULL,
	[InterchangeControlNumber] [varchar](50) NULL,
	[TransactionSenderIdentifier] [varchar](50) NULL,
	[TransactionDate] [varchar](50) NULL,
	[TransactionTime] [varchar](50) NULL,
	[DirectoryExtractDate] [varchar](50) NULL,
	[ContactFunctionCode] [varchar](50) NULL,
	[ContactName] [varchar](50) NULL,
	[ContactQualifier1] [varchar](50) NULL,
	[ContactNumber1] [varchar](50) NULL,
	[ContactQualifier2] [varchar](50) NULL,
	[ContactNumber2] [varchar](50) NULL,
	[ContactQualifier3] [varchar](50) NULL,
	[ContactNumber3] [varchar](50) NULL,
	[AddedDate] [datetime] NULL,
	[ReportYear] [varchar](4) NULL,
	[ReportMonth] [varchar](2) NULL,
 CONSTRAINT [PK_P274File] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[P274GroupIdNumber](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileId] [int] NOT NULL,
	[DetailId] [varchar](50) NULL,
	[ReferenceQualifier] [varchar](50) NULL,
	[ReferenceId] [varchar](50) NULL,
 CONSTRAINT [PK_P274GroupIdNumber] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[P274Information](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileId] [int] NOT NULL,
	[EntityIdCode] [varchar](50) NULL,
	[EntityTypeQualifier] [varchar](50) NULL,
	[EntityLastName] [varchar](50) NULL,
	[EntityFirstName] [varchar](50) NULL,
	[EntityIdQualifier] [varchar](50) NULL,
	[EntityId] [varchar](50) NULL,
	[LoopName] [varchar](50) NULL,
 CONSTRAINT [PK_P274Information] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[P274SiteCRC](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileId] [int] NOT NULL,
	[DetailId] [varchar](50) NULL,
	[Category] [varchar](50) NULL,
	[Condition] [varchar](50) NULL,
	[Code1] [varchar](50) NULL,
	[Code2] [varchar](50) NULL,
	[Code3] [varchar](50) NULL,
	[Code4] [varchar](50) NULL,
	[Code5] [varchar](50) NULL,
 CONSTRAINT [PK_P274SiteCRC] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[P274SiteWorkSchedule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileId] [int] NOT NULL,
	[DetailId] [varchar](50) NULL,
	[ShiftCode] [varchar](50) NULL,
	[StartTime] [varchar](50) NULL,
	[EndTime] [varchar](50) NULL,
 CONSTRAINT [PK_P274SiteWorkSchedule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[P274SpecializationArea](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileId] [int] NOT NULL,
	[DetailId] [varchar](50) NULL,
	[LQQualifier] [varchar](50) NULL,
	[LQCode] [varchar](50) NULL,
	[NetworkRoleCode1] [varchar](50) NULL,
	[NetworkRoleCode2] [varchar](50) NULL,
	[CertificationConditionInd] [varchar](50) NULL,
	[BoardCertificationInd] [varchar](50) NULL,
 CONSTRAINT [PK_P274SpecializationArea] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


