CREATE SCHEMA Recon;
GO
CREATE TABLE [Recon].[ClaimCAS](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[ClaimID] [varchar](50) NULL,
	[ServiceLineNumber] [varchar](50) NULL,
	[SubscriberSequenceNumber] [varchar](50) NULL,
	[GroupCode] [varchar](50) NULL,
	[ReasonCode] [varchar](50) NULL,
	[AdjustmentAmount] [varchar](50) NULL,
	[AdjustmentQuantity] [varchar](50) NULL,
 CONSTRAINT [PK_Recon.ClaimCAS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Recon].[ClaimCRCs](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[ClaimID] [varchar](50) NULL,
	[ServiceLineNumber] [varchar](50) NULL,
	[CodeCategory] [varchar](50) NULL,
	[ConditionIndicator] [varchar](50) NULL,
	[ConditionCode] [varchar](50) NULL,
	[ConditionCode2] [varchar](50) NULL,
	[ConditionCode3] [varchar](50) NULL,
	[ConditionCode4] [varchar](50) NULL,
	[ConditionCode5] [varchar](50) NULL,
 CONSTRAINT [PK_Recon.ClaimCRCs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Recon].[ClaimHeaders](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[ClaimID] [varchar](50) NULL,
	[ClaimAmount] [varchar](50) NULL,
	[ClaimPOS] [varchar](50) NULL,
	[ClaimType] [varchar](50) NULL,
	[ClaimFrequencyCode] [varchar](50) NULL,
	[ClaimProviderSignature] [varchar](50) NULL,
	[ClaimProviderAssignment] [varchar](50) NULL,
	[ClaimBenefitAssignment] [varchar](50) NULL,
	[ClaimReleaseofInformationCode] [varchar](50) NULL,
	[ClaimPatientSignatureSourceCode] [varchar](50) NULL,
	[ClaimRelatedCausesQualifier] [varchar](50) NULL,
	[ClaimRelatedCausesCode] [varchar](50) NULL,
	[ClaimRelatedStateCode] [varchar](50) NULL,
	[ClaimRelatedCountryCode] [varchar](50) NULL,
	[ClaimSpecialProgramCode] [varchar](50) NULL,
	[ClaimDelayReasonCode] [varchar](50) NULL,
	[CurrentIllnessDate] [varchar](50) NULL,
	[InitialTreatmentDate] [varchar](50) NULL,
	[LastSeenDate] [varchar](50) NULL,
	[AcuteManifestestationDate] [varchar](50) NULL,
	[AccidentDate] [varchar](50) NULL,
	[LastMenstrualPeriodDate] [varchar](50) NULL,
	[LastXrayDate] [varchar](50) NULL,
	[PrescriptionDate] [varchar](50) NULL,
	[DisabilityDate] [varchar](50) NULL,
	[DisabilityStartDate] [varchar](50) NULL,
	[DisabilityEndDate] [varchar](50) NULL,
	[LastWorkedDate] [varchar](50) NULL,
	[AuthorizedReturnToWorkDate] [varchar](50) NULL,
	[AdmissionDate] [varchar](50) NULL,
	[DischargeDate] [varchar](50) NULL,
	[AssumedStartDate] [varchar](50) NULL,
	[AssumedEndDate] [varchar](50) NULL,
	[FirstContactDate] [varchar](50) NULL,
	[RepricerReceivedDate] [varchar](50) NULL,
	[AppliancePlacementDate] [varchar](50) NULL,
	[ServiceFromDate] [varchar](50) NULL,
	[ServiceToDate] [varchar](50) NULL,
	[OrthoMonthTotal] [varchar](50) NULL,
	[OrthoMonthRemaining] [varchar](50) NULL,
	[ContractTypeCode] [varchar](50) NULL,
	[ContractAmount] [varchar](50) NULL,
	[ContractPercentage] [varchar](50) NULL,
	[ContractCode] [varchar](50) NULL,
	[ContractTermsDiscountPercentage] [varchar](50) NULL,
	[ContractVersionIdentifier] [varchar](50) NULL,
	[PatientPaidAmount] [varchar](50) NULL,
	[AmbulanceWeight] [varchar](50) NULL,
	[AmbulanceReasonCode] [varchar](50) NULL,
	[AmbulanceQuantity] [varchar](50) NULL,
	[AmbulanceRoundTripDescription] [varchar](100) NULL,
	[AmbulanceStretcherDescription] [varchar](100) NULL,
	[PatientConditionCode] [varchar](50) NULL,
	[PatientConditionDescription1] [varchar](50) NULL,
	[PatientConditionDescription2] [varchar](50) NULL,
	[PricingMethodology] [varchar](50) NULL,
	[RepricedAllowedAmount] [varchar](50) NULL,
	[RepricedSavingAmount] [varchar](50) NULL,
	[RepricingOrganizationID] [varchar](50) NULL,
	[RepricingRate] [varchar](50) NULL,
	[RepricedGroupCode] [varchar](50) NULL,
	[RepricedGroupAmount] [varchar](50) NULL,
	[RepricingRevenueCode] [varchar](50) NULL,
	[RepricingUnit] [varchar](50) NULL,
	[RepricingQuantity] [varchar](50) NULL,
	[RejectReasonCode] [varchar](50) NULL,
	[PolicyComplianceCode] [varchar](50) NULL,
	[ExceptionCode] [varchar](50) NULL,
	[StatementFromDate] [varchar](50) NULL,
	[StatementToDate] [varchar](50) NULL,
	[AdmissionTypeCode] [varchar](50) NULL,
	[AdmissionSourceCode] [varchar](50) NULL,
	[PatientStatusCode] [varchar](50) NULL,
	[PatientResponsibilityAmount] [varchar](50) NULL,
	[ExportType] [varchar](50) NULL,
 CONSTRAINT [PK_Recon.ClaimHeaders] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Recon].[ClaimHIs](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[ClaimID] [varchar](50) NULL,
	[HIQual] [varchar](50) NULL,
	[HICode] [varchar](50) NULL,
	[PresentOnAdmissionIndicator] [varchar](50) NULL,
	[HIFromDate] [varchar](50) NULL,
	[HIToDate] [varchar](50) NULL,
	[HIAmount] [varchar](50) NULL,
 CONSTRAINT [PK_Recon.ClaimHIs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Recon].[ClaimK3s](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[ClaimID] [varchar](50) NULL,
	[ServiceLineNumber] [varchar](50) NULL,
	[K3] [varchar](100) NULL,
 CONSTRAINT [PK_Recon.ClaimK3s] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Recon].[ClaimLineFRMs](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[ClaimID] [varchar](50) NULL,
	[ServiceLineNumber] [varchar](50) NULL,
	[LQSequence] [varchar](50) NULL,
	[QuestionNumber] [varchar](50) NULL,
	[QuestionResponseIndicator] [varchar](50) NULL,
	[QuestionResponse] [varchar](50) NULL,
	[QuestionResponseDate] [varchar](50) NULL,
	[AllowedChargePercentage] [varchar](50) NULL,
 CONSTRAINT [PK_Recon.ClaimLineFRMs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Recon].[ClaimLineLQs](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[ClaimID] [varchar](50) NULL,
	[ServiceLineNumber] [varchar](50) NULL,
	[LQSequence] [varchar](50) NULL,
	[FormQualifier] [varchar](50) NULL,
	[IndustryCode] [varchar](50) NULL,
 CONSTRAINT [PK_Recon.ClaimLineLQs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Recon].[ClaimLineMEAs](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[ClaimID] [varchar](50) NULL,
	[ServiceLineNumber] [varchar](50) NULL,
	[TestCode] [varchar](50) NULL,
	[MeasurementQualifier] [varchar](50) NULL,
	[TestResult] [varchar](50) NULL,
 CONSTRAINT [PK_Recon.ClaimLineMEAs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Recon].[ClaimLineSVDs](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[ClaimID] [varchar](50) NULL,
	[ServiceLineNumber] [varchar](50) NULL,
	[RepeatSequence] [varchar](50) NULL,
	[OtherPayerPrimaryIdentifier] [varchar](50) NULL,
	[ServiceLinePaidAmount] [varchar](50) NULL,
	[ServiceQualifier] [varchar](50) NULL,
	[ProcedureCode] [varchar](50) NULL,
	[ProcedureModifier1] [varchar](50) NULL,
	[ProcedureModifier2] [varchar](50) NULL,
	[ProcedureModifier3] [varchar](50) NULL,
	[ProcedureModifier4] [varchar](50) NULL,
	[ProcedureDescription] [varchar](100) NULL,
	[PaidServiceUnitCount] [varchar](50) NULL,
	[BundledLineNumber] [varchar](50) NULL,
	[ServiceLineRevenueCode] [varchar](50) NULL,
	[AdjudicationDate] [varchar](50) NULL,
	[ReaminingPatientLiabilityAmount] [varchar](50) NULL,
 CONSTRAINT [PK_Recon.ClaimLineSVDs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Recon].[ClaimNtes](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[ClaimID] [varchar](50) NULL,
	[ServiceLineNumber] [varchar](50) NULL,
	[NoteCode] [varchar](50) NULL,
	[NoteText] [varchar](200) NULL,
 CONSTRAINT [PK_Recon.ClaimNtes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Recon].[ClaimPatients](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[ClaimID] [varchar](50) NULL,
	[PatientRelatedCode] [varchar](50) NULL,
	[PatientRelatedDeathDate] [varchar](50) NULL,
	[PatientRelatedUnit] [varchar](50) NULL,
	[PatientRelatedWeight] [varchar](50) NULL,
	[PatientRelatedPregnancyIndicator] [varchar](50) NULL,
	[PatientLastName] [varchar](50) NULL,
	[PatientFirstName] [varchar](50) NULL,
	[PatientMiddle] [varchar](50) NULL,
	[PatientSuffix] [varchar](50) NULL,
	[PatientAddress] [varchar](200) NULL,
	[PatientAddress2] [varchar](50) NULL,
	[PatientCity] [varchar](50) NULL,
	[PatientState] [varchar](50) NULL,
	[PatientZip] [varchar](50) NULL,
	[PatientCountry] [varchar](50) NULL,
	[PatientCountrySubCode] [varchar](50) NULL,
	[PatientBirthDate] [varchar](50) NULL,
	[PatientGender] [varchar](50) NULL,
	[PatientContactName] [varchar](50) NULL,
	[PatientContactPhone] [varchar](50) NULL,
	[PatientContactPhoneEx] [varchar](50) NULL,
 CONSTRAINT [PK_Recon.ClaimPatients] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Recon].[ClaimProviders](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[LoopName] [varchar](50) NULL,
	[ClaimID] [varchar](50) NULL,
	[ServiceLineNumber] [varchar](50) NULL,
	[ProviderQualifier] [varchar](50) NULL,
	[ProviderTaxonomyCode] [varchar](50) NULL,
	[ProviderCurrencyCode] [varchar](50) NULL,
	[ProviderLastName] [varchar](100) NULL,
	[ProviderFirstName] [varchar](50) NULL,
	[ProviderMiddle] [varchar](50) NULL,
	[ProviderSuffix] [varchar](50) NULL,
	[ProviderIDQualifier] [varchar](50) NULL,
	[ProviderID] [varchar](50) NULL,
	[ProviderAddress] [varchar](200) NULL,
	[ProviderAddress2] [varchar](100) NULL,
	[ProviderCity] [varchar](50) NULL,
	[ProviderState] [varchar](50) NULL,
	[ProviderZip] [varchar](50) NULL,
	[ProviderCountry] [varchar](50) NULL,
	[ProviderCountrySubCode] [varchar](50) NULL,
	[RepeatSequence] [varchar](50) NULL,
 CONSTRAINT [PK_Recon.ClaimProviders] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Recon].[ClaimPWKs](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[ClaimID] [varchar](50) NULL,
	[ServiceLineNumber] [varchar](50) NULL,
	[ReportTypeCode] [varchar](50) NULL,
	[ReportTransmissionCode] [varchar](50) NULL,
	[AttachmentControlNumber] [varchar](50) NULL,
 CONSTRAINT [PK_Recon.ClaimPWKs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Recon].[ClaimSBRs](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[ClaimID] [varchar](50) NULL,
	[LoopName] [varchar](50) NULL,
	[SubscriberSequenceNumber] [varchar](50) NULL,
	[SubscriberRelationshipCode] [varchar](50) NULL,
	[InsuredGroupNumber] [varchar](50) NULL,
	[OtherInsuredGroupName] [varchar](100) NULL,
	[InsuredTypeCode] [varchar](50) NULL,
	[ClaimFilingCode] [varchar](50) NULL,
	[DeathDate] [varchar](50) NULL,
	[Unit] [varchar](50) NULL,
	[Weight] [varchar](50) NULL,
	[PregnancyIndicator] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[FirstName] [varchar](50) NULL,
	[MidddleName] [varchar](50) NULL,
	[NameSuffix] [varchar](50) NULL,
	[IDQualifier] [varchar](50) NULL,
	[IDCode] [varchar](50) NULL,
	[SubscriberAddress] [varchar](200) NULL,
	[SubscriberAddress2] [varchar](100) NULL,
	[SubscriberCity] [varchar](50) NULL,
	[SubscriberState] [varchar](50) NULL,
	[SubscriberZip] [varchar](50) NULL,
	[SubscriberCountry] [varchar](50) NULL,
	[SubscriberCountrySubCode] [varchar](50) NULL,
	[SubscriberBirthDate] [varchar](50) NULL,
	[SubscriberGender] [varchar](50) NULL,
	[SubscriberContactName] [varchar](50) NULL,
	[SubscriberContactPhone] [varchar](50) NULL,
	[SubscriberContactPhoneEx] [varchar](50) NULL,
	[COBPayerPaidAmount] [varchar](50) NULL,
	[COBNonCoveredAmount] [varchar](50) NULL,
	[COBRemainingPatientLiabilityAmount] [varchar](50) NULL,
	[BenefitsAssignmentCertificationIndicator] [varchar](50) NULL,
	[PatientSignatureSourceCode] [varchar](50) NULL,
	[ReleaseOfInformationCode] [varchar](50) NULL,
	[ReimbursementRate] [varchar](50) NULL,
	[HCPCSPayableAmount] [varchar](50) NULL,
	[MOA_ClaimPaymentRemarkCode1] [varchar](50) NULL,
	[MOA_ClaimPaymentRemarkCode2] [varchar](50) NULL,
	[MOA_ClaimPaymentRemarkCode3] [varchar](50) NULL,
	[MOA_ClaimPaymentRemarkCode4] [varchar](50) NULL,
	[MOA_ClaimPaymentRemarkCode5] [varchar](50) NULL,
	[EndStageRenalDiseasePaymentAmount] [varchar](50) NULL,
	[MOA_NonPayableProfessionalComponentBilledAmount] [varchar](50) NULL,
	[CoveredDays] [varchar](50) NULL,
	[LifetimePsychiatricDays] [varchar](50) NULL,
	[ClaimDRGAmount] [varchar](50) NULL,
	[MIA_ClaimPaymentRemarkCode1] [varchar](50) NULL,
	[ClaimDisproportionateShareAmount] [varchar](50) NULL,
	[ClaimMSPPassThroughAmount] [varchar](50) NULL,
	[ClaimPPSCapitalAmount] [varchar](50) NULL,
	[PPSCapitalFSPDRGAmount] [varchar](50) NULL,
	[PPSCapitalHSPDRGAmount] [varchar](50) NULL,
	[PPSCapitalDSHDRGAmount] [varchar](50) NULL,
	[OldCapitalAmount] [varchar](50) NULL,
	[PPSCapitalIMEAmount] [varchar](50) NULL,
	[PPSOperatingHospitalSpecificDRGAmount] [varchar](50) NULL,
	[CostReportDayCount] [varchar](50) NULL,
	[PPSOperatingFederalSpecificDRGAmount] [varchar](50) NULL,
	[CLaimPPSCapitalOutlierAmount] [varchar](50) NULL,
	[ClaimIndirectTeachingAmount] [varchar](50) NULL,
	[MIA_NonPayableProfessionalComponentBilledAmount] [varchar](50) NULL,
	[MIA_ClaimPaymentRemarkCode2] [varchar](50) NULL,
	[MIA_ClaimPaymentRemarkCode3] [varchar](50) NULL,
	[MIA_ClaimPaymentRemarkCode4] [varchar](50) NULL,
	[MIA_ClaimPaymentRemarkCode5] [varchar](50) NULL,
	[PPSCapitalExceptionAmount] [varchar](50) NULL,
	[PaymentDate] [varchar](50) NULL,
 CONSTRAINT [PK_Recon.ClaimSBRs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Recon].[ClaimSecondaryIdentifications](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[ClaimID] [varchar](50) NULL,
	[ServiceLineNumber] [varchar](50) NULL,
	[LoopName] [varchar](50) NULL,
	[ProviderQualifier] [varchar](50) NULL,
	[ProviderID] [varchar](50) NULL,
	[OtherPayerPrimaryIDentification] [varchar](50) NULL,
	[RepeatSequence] [varchar](50) NULL,
 CONSTRAINT [PK_Recon.ClaimSecondaryIdentifications] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Recon].[ClaimTempHost](
	[ClaimID] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Recon.ClaimID] PRIMARY KEY CLUSTERED 
(
	[ClaimID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Recon].[Hipaa_XML](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ClaimType] [varchar](50) NOT NULL,
	[ClaimID] [varchar](50) NOT NULL,
	[EncounterId] [varchar](50) NULL,
	[ClaimHipaaXML] [varchar](max) NOT NULL,
 CONSTRAINT [PK_HipaaXML] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [Recon].[ProviderContacts](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[ClaimID] [varchar](50) NULL,
	[ServiceLineNumber] [varchar](50) NULL,
	[LoopName] [varchar](50) NULL,
	[ProviderQualifier] [varchar](50) NULL,
	[ProviderNPI] [varchar](50) NULL,
	[ContactName] [varchar](100) NULL,
	[Email] [varchar](50) NULL,
	[Fax] [varchar](50) NULL,
	[Phone] [varchar](50) NULL,
	[PhoneEx] [varchar](50) NULL,
 CONSTRAINT [PK_Recon.ProviderContacts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Recon].[ServiceLines](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[ClaimID] [varchar](50) NULL,
	[ServiceLineNumber] [varchar](50) NULL,
	[ServiceIDQualifier] [varchar](50) NULL,
	[ProcedureCode] [varchar](50) NULL,
	[ProcedureModifier1] [varchar](50) NULL,
	[ProcedureModifier2] [varchar](50) NULL,
	[ProcedureModifier3] [varchar](50) NULL,
	[ProcedureModifier4] [varchar](50) NULL,
	[ProcedureDescription] [varchar](100) NULL,
	[LineItemChargeAmount] [varchar](50) NULL,
	[LineItemUnit] [varchar](50) NULL,
	[ServiceUnitQuantity] [varchar](50) NULL,
	[LineItemPOS] [varchar](50) NULL,
	[DiagPointer1] [varchar](50) NULL,
	[DiagPointer2] [varchar](50) NULL,
	[DiagPointer3] [varchar](50) NULL,
	[DiagPointer4] [varchar](50) NULL,
	[EmergencyIndicator] [varchar](50) NULL,
	[EPSDTIndicator] [varchar](50) NULL,
	[FamilyPlanningIndicator] [varchar](50) NULL,
	[CopayStatusCode] [varchar](50) NULL,
	[DMEQualifier] [varchar](50) NULL,
	[DMEProcedureCode] [varchar](50) NULL,
	[DMEDays] [varchar](50) NULL,
	[DMERentalPrice] [varchar](50) NULL,
	[DMEPurchasePrice] [varchar](50) NULL,
	[DMEFrequencyCode] [varchar](50) NULL,
	[PatientWeight] [varchar](50) NULL,
	[AmbulanceTransportReasonCode] [varchar](50) NULL,
	[TransportDistance] [varchar](50) NULL,
	[RoundTripPurposeDescription] [varchar](100) NULL,
	[StretcherPueposeDescription] [varchar](100) NULL,
	[CertificationTypeCode] [varchar](50) NULL,
	[DMEDuration] [varchar](50) NULL,
	[ServiceFromDate] [varchar](50) NULL,
	[ServiceToDate] [varchar](50) NULL,
	[PrescriptionDate] [varchar](50) NULL,
	[CertificationDate] [varchar](50) NULL,
	[BeginTherapyDate] [varchar](50) NULL,
	[LastCertificationDate] [varchar](50) NULL,
	[LastSeenDate] [varchar](50) NULL,
	[TestDateHemo] [varchar](50) NULL,
	[TestDateSerum] [varchar](50) NULL,
	[ShippedDate] [varchar](50) NULL,
	[LastXrayDate] [varchar](50) NULL,
	[InitialTreatmentDate] [varchar](50) NULL,
	[AmbulancePatientCount] [varchar](50) NULL,
	[ObstetricAdditionalUnits] [varchar](50) NULL,
	[ContractTypeCode] [varchar](50) NULL,
	[ContractAmount] [varchar](50) NULL,
	[ContractPercentage] [varchar](50) NULL,
	[ContractCode] [varchar](50) NULL,
	[TermsDiscountPercentage] [varchar](50) NULL,
	[ContractVersionIdentifier] [varchar](50) NULL,
	[SalesTaxAmount] [varchar](50) NULL,
	[PostageClaimedAmount] [varchar](50) NULL,
	[PurchasedServiceProviderIdentifier] [varchar](50) NULL,
	[PurchasedServiceChargeAmount] [varchar](50) NULL,
	[PricingMethodology] [varchar](50) NULL,
	[RepricedAllowedAmount] [varchar](50) NULL,
	[RepricedSavingAmount] [varchar](50) NULL,
	[RepricingOrganizationIdentifier] [varchar](50) NULL,
	[RepricingRate] [varchar](50) NULL,
	[RepricedAmbulatoryPatientGroupCode] [varchar](50) NULL,
	[RepricedAmbulatoryPatientGroupAmount] [varchar](50) NULL,
	[HCPQualifier] [varchar](50) NULL,
	[RepricedHCPCSCode] [varchar](50) NULL,
	[RepricingUnit] [varchar](50) NULL,
	[RepricingQuantity] [varchar](50) NULL,
	[RejectReasonCode] [varchar](50) NULL,
	[PolicyComplianceCode] [varchar](50) NULL,
	[ExceptionCode] [varchar](50) NULL,
	[LINQualifier] [varchar](50) NULL,
	[NationalDrugCode] [varchar](50) NULL,
	[DrugQuantity] [varchar](50) NULL,
	[DrugQualifier] [varchar](50) NULL,
	[RevenueCode] [varchar](50) NULL,
	[LineItemDeniedChargeAmount] [varchar](50) NULL,
	[ServiceTaxAmount] [varchar](50) NULL,
	[FacilityTaxAmount] [varchar](50) NULL,
	[PriorPlacementDate] [varchar](50) NULL,
	[AppliancePlacementDate] [varchar](50) NULL,
	[ReplacementDate] [varchar](50) NULL,
	[TreatmentStartDate] [varchar](50) NULL,
	[TreatmentCompletionDate] [varchar](50) NULL,
	[OralCavityDesignationCode1] [varchar](50) NULL,
	[OralCavityDesignationCode2] [varchar](50) NULL,
	[OralCavityDesignationCode3] [varchar](50) NULL,
	[OralCavityDesignationCode4] [varchar](50) NULL,
	[OralCavityDesignationCode5] [varchar](50) NULL,
	[ProsthesisCrownOrInlayCode] [varchar](50) NULL,
 CONSTRAINT [PK_Recon.ServiceLines] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Recon].[SubmissionLogs](
	[FileID] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [varchar](200) NULL,
	[FilePath] [varchar](200) NULL,
	[FileType] [varchar](50) NULL,
	[ReportType] [varchar](50) NULL,
	[EncounterCount] [int] NOT NULL,
	[SubmitterID] [varchar](50) NULL,
	[ReceiverID] [varchar](50) NULL,
	[InterchangeControlNumber] [varchar](50) NULL,
	[ProductionFlag] [varchar](50) NULL,
	[InterchangeDate] [varchar](50) NULL,
	[InterchangeTime] [varchar](50) NULL,
	[BatchControlNumber] [varchar](50) NULL,
	[SubmitterLastName] [varchar](50) NULL,
	[SubmitterFirstName] [varchar](50) NULL,
	[SubmitterMiddle] [varchar](50) NULL,
	[SubmitterContactName1] [varchar](50) NULL,
	[SubmitterContactEmail1] [varchar](50) NULL,
	[SubmitterContactFax1] [varchar](50) NULL,
	[SubmitterContactPhone1] [varchar](50) NULL,
	[SubmitterContactPhoneEx1] [varchar](50) NULL,
	[SubmitterContactName2] [varchar](50) NULL,
	[SubmitterContactEmail2] [varchar](50) NULL,
	[SubmitterContactFax2] [varchar](50) NULL,
	[SubmitterContactPhone2] [varchar](50) NULL,
	[SubmitterContactPhoneEx2] [varchar](50) NULL,
	[ReceiverLastName] [varchar](50) NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Recon.SubmissionLogs] PRIMARY KEY CLUSTERED 
(
	[FileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [Recon].[ToothStatus](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[LoopName] [varchar](50) NULL,
	[ClaimID] [varchar](50) NULL,
	[ServiceLineNumber] [varchar](50) NULL,
	[ToothNumber] [varchar](50) NULL,
	[StatusCode] [varchar](50) NULL,
	[SurfaceCode2] [varchar](50) NULL,
	[SurfaceCode3] [varchar](50) NULL,
	[SurfaceCode4] [varchar](50) NULL,
	[SurfaceCode5] [varchar](50) NULL,
 CONSTRAINT [PK_Recon.ToothStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Recon].[ResponseError](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileId] [int] NOT NULL,
	[ClaimId] [varchar](50) NULL,
	[LineNumber] [varchar](50) NULL,
	[ErrorId1] [varchar](50) NULL,
	[ErrorId2] [varchar](50) NULL,
	[ErrorId3] [varchar](50) NULL,
	[ErrorCategory] [varchar](50) NULL,
	[ErrorDescription] [varchar](1000) NULL,
	[Fixed] [bit] NULL,
	[AddedDate] [datetime] NULL,
 CONSTRAINT [PK_ResponseError] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [Recon].[LoadLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileId] [int] NOT NULL,
	[FileName] [varchar](200) NOT NULL,
	[ReconLoadedDate] [datetime] NULL,
	[ReloadNeeded] [bit] NULL,
 CONSTRAINT [PK_Recon.LoadLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

