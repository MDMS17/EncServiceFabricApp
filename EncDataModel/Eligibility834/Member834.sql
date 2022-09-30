create table dbo.M834File(
FileId int identity(1,1) not null,
FileName varchar(200) null,
SenderId varchar(50) null,
ReceiverId varchar(50) null,
InterchangeControlNumber varchar(50) null,
TransactionDate varchar(50) null,
TransactionTime varchar(50) null,
TransactionPurposeCode varchar(50) null,
TransactionReferenceNumber varchar(50) null,
TransactionTimeCode varchar(50) null,
TransactionActionCode varchar(50) null,
TransactionPolicyNumber varchar(50) null,
EffectiveDate varchar(50) null,
ReportStartDate varchar(50) null,
ReportEndDate varchar(50) null,
MaintenanceEffectiveDate varchar(50) null,
EnrollmentDate varchar(50) null,
PaymentDate varchar(50) null,
DependentQuantity varchar(50) null,
EmployeeQuantity varchar(50) null,
TotalQuantity varchar(50) null,
SponsorName varchar(200) null,
SponsorIdQualifier varchar(50) null,
SponsorId varchar(50) null,
PayerName varchar(200) null,
PayerIdQualifier varchar(50) null,
PayerId varchar(50) null,
Broker1Name varchar(200) null,
Broker1IdQualifier varchar(50) null,
Broker1Id varchar(50) null,
Broker1AccountNumber varchar(50) null,
Broker1AccountNumber2 varchar(50) null,
Broker2Name varchar(200) null,
Broker2IdQualifier varchar(50) null,
Broker2Id varchar(50) null,
Broker2AccountNumber varchar(50) null,
Broker2AccountNumber2 varchar(50) null,
CreateUser varchar(50) not null,
CreateDate datetime not null,
 CONSTRAINT [PK_M834File] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


create table M834Detail (
Id int identity(1,1) not null,
FileId int not null,                
DetailId varchar(50) not null,
ConditionCode varchar(50) null,                                
RelationshipCode varchar(50) null,                     
MaintenanceTypeCode varchar(50) null,                  
MaintenanceReasonCode varchar(50) null,
BenefitStatusCode varchar(50) null,
MedicarePlanCode varchar(50) null,
MedicareEligibilityReasonCode varchar(50) null,                  
COBRACode varchar(50) null,     
EmploymentStatusCode varchar(50) null,
StudentStatusCode varchar(50) null,
HandicapInd varchar(50) null,
MemberDeathDate varchar(50) null,
ConfidentialityCode varchar(50) null,
BirthSequenceNumber varchar(50) null,
SubscriberId varchar(50) null,
PolicyNumber varchar(50) null,
MemberQualifier varchar(50) null,
MemberLastName varchar(200) null,
MemberFirstName varchar(200) null,
MemberMiddleName varchar(50) null,
MemberPrefix varchar(50) null,
MemberSuffix varchar(50) null,
MemberIdQualifier varchar(50) null,
MemberId varchar(50) null,
CommunicationQualifier1 varchar(50) null,
Communicationnumber1 varchar(50) null,
CommunicationQualifier2 varchar(50) null,
CommunicationNumber2 varchar(50) null,
CommunicationQualifier3 varchar(50) null,
CommunicationNumber3 varchar(50) null,
MemberAddress varchar(200) null,
MemberAddress2 varchar(200) null,
MemberCity varchar(50) null,
MemberState varchar(50) null,
MemberZip varchar(50) null,
MemberCountry varchar(50) null,
MemberLocationQualifier varchar(50) null,
MemberLocationId varchar(50) null,
MemberCountrySubCode varchar(50) null,
MemberBirthDate varchar(50) null,
MemberGender varchar(50) null,
MemberMaritalStatus varchar(50) null,
MemberEthnicity varchar(50) null,
MemberCitizenship varchar(50) null,
MemberEthnicityClassificationCode varchar(50) null,
MemberEthnicityCollectionCode varchar(50) null,
MemberIncomeFrequencyCode varchar(50) null,
MemberIncome varchar(50) null,
MemberWorkHours varchar(50) null,
MemberWorkDepartment varchar(50) null,
MemberSalaryGrade varchar(50) null,
MemberHealthCode varchar(50) null,
MemberHeight varchar(50) null,
MemberWeight varchar(50) null,
 CONSTRAINT [PK_M834Detail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

create table M834SubId(
Id int identity(1,1) not null,
FileId int not null,
DetailId varchar(50) not null,
SubIdQualifier varchar(50) null,
SubId varchar(50) null,
 CONSTRAINT [PK_M834SubId] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

create table M834MemberLevelDate(
Id int identity(1,1) not null,
FileId int not null,
DetailId varchar(50) not null,
DateQualifier varchar(50) null,
MemberLevelDate varchar(50) null,
 CONSTRAINT [PK_M834MemberLevelDate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

create table M834EmploymentClass(
Id int identity(1,1) not null,
FileId int not null,
DetailId varchar(50) not null,
EmploymentClassCode1 varchar(50) null,
EmploymentClassCode2 varchar(50) null,
EmploymentClassCode3 varchar(50) null, 
 CONSTRAINT [PK_M834EmploymentClass] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

create table M834PolicyAmount(
Id int identity(1,1) not null,
FileId int not null,
DetailId varchar(50) not null,
AmountQualifier varchar(50) null,
ContractAmount varchar(50) null,
 CONSTRAINT [PK_M834PolicyAmount] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

create table M834Language(
Id int identity(1,1) not null,
FileId int not null,
DetailId varchar(50) not null,
IdQualifier varchar(50) null,
LanguageId varchar(50) null,
LanguageDescription varchar(200) null,
LanguageUsageInd varchar(50) null,
 CONSTRAINT [PK_M834Language] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

create table M834AdditionalName(
Id int identity(1,1) not null,
FileId int not null,
DetailId varchar(50) not null,
NameQualifier varchar(50) null,
LastName varchar(200) null,
FirstName varchar(200) null,
MiddleName varchar(50) null,
Prefix varchar(50) null,
Suffix varchar(50) null,
IdQualifier varchar(50) null,
NameId varchar(50) null,
BirthDate varchar(50) null,
Gender varchar(50) null,
MaritalStatus varchar(50) null,
Ethnicity varchar(50) null,
EthnicityClassificationCode varchar(50) null,
Citizenship varchar(50) null,
EthnicityCollectionCode varchar(50) null,
ContactName varchar(200) null,
ContactQualifier1 varchar(50) null,
ContactNumber1 varchar(50) null,
ContactQualifier2 varchar(50) null,
ContactNumber2 varchar(50) null,
ContactQualifier3 varchar(50) null,
ContactNumber3 varchar(50) null,
Address varchar(200) null,
Address2 varchar(200) null,
AddressCity varchar(50) null,
AddressState varchar(50) null,
AddressZip varchar(50) null,
AddressCountry varchar(50) null,
AddressCountrySubCode varchar(50) null,
 CONSTRAINT [PK_M834AdditionalName] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

create table M834DisabilityInfo(
Id int identity(1,1) not null,
FileId int not null,
DetailId varchar(50) not null,
DisabilityTypeCode varchar(50) null,
ProductIdQualifier varchar(50) null,
DiagnosisCode varchar(50) null,
DisabilityStartDate varchar(50) null,
DisabilityEndDate varchar(50) null,
 CONSTRAINT [PK_M834DisabilityInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

create table M834ReportingCategory(
Id int identity(1,1) not null,
FileId int not null,
DetailId varchar(50) not null,
SequenceNumber varchar(50) null,
ParticipantName varchar(200) null,
ReferenceIdQualifier varchar(50) null,
ReferenceId varchar(50) null,
ReportBeginDate varchar(50) null,
ReportEndDate varchar(50) null,
 CONSTRAINT [PK_M834ReportingCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

create table M834HealthCoverage(
Id int identity(1,1) not null,
FileId int not null,
DetailId varchar(50) not null,
HCId varchar(50) not null,
MaintenanceTypeCode varchar(50) null,
InsuranceLineCode varchar(50) null,
PlanCoverageDescription varchar(50) null,
CoverageLevelCode varchar(50) null,
LateEnrollmentInd varchar(50) null,
DateQualifier1 varchar(50) null,
DateBegin1 varchar(50) null,
DateEnd1 varchar(50) null,
DateQualifier2 varchar(50) null,
DateBegin2 varchar(50) null,
DateEnd2 varchar(50) null,
DateQualifier3 varchar(50) null,
DateBegin3 varchar(50) null,
DateEnd3 varchar(50) null,
DateQualifier4 varchar(50) null,
DateBegin4 varchar(50) null,
DateEnd4 varchar(50) null,
DateQualifier5 varchar(50) null,
DateBegin5 varchar(50) null,
DateEnd5 varchar(50) null,
DateQualifier6 varchar(50) null,
DateBegin6 varchar(50) null,
DateEnd6 varchar(50) null,
AmountQualifier1 varchar(50) null,
Amount1 varchar(50) null,
AmountQualifier2 varchar(50) null,
Amount2 varchar(50) null,
AmountQualifier3 varchar(50) null,
Amount3 varchar(50) null,
AmountQualifier4 varchar(50) null,
Amount4 varchar(50) null,
AmountQualifier5 varchar(50) null,
Amount5 varchar(50) null,
AmountQualifier6 varchar(50) null,
Amount6 varchar(50) null,
AmountQualifier7 varchar(50) null,
Amount7 varchar(50) null,
AmountQualifier8 varchar(50) null,
Amount8 varchar(50) null,
AmountQualifier9 varchar(50) null,
Amount9 varchar(50) null,
PolicyQualifier01 varchar(50) null,
PolicyNumber01 varchar(50) null,
PolicyQualifier02 varchar(50) null,
PolicyNumber02 varchar(50) null,
PolicyQualifier03 varchar(50) null,
PolicyNumber03 varchar(50) null,
PolicyQualifier04 varchar(50) null,
PolicyNumber04 varchar(50) null,
PolicyQualifier05 varchar(50) null,
PolicyNumber05 varchar(50) null,
PolicyQualifier06 varchar(50) null,
PolicyNumber06 varchar(50) null,
PolicyQualifier07 varchar(50) null,
PolicyNumber07 varchar(50) null,
PolicyQualifier08 varchar(50) null,
PolicyNumber08 varchar(50) null,
PolicyQualifier09 varchar(50) null,
PolicyNumber09 varchar(50) null,
PolicyQualifier10 varchar(50) null,
PolicyNumber10 varchar(50) null,
PolicyQualifier11 varchar(50) null,
PolicyNumber11 varchar(50) null,
PolicyQualifier12 varchar(50) null,
PolicyNumber12 varchar(50) null,
PolicyQualifier13 varchar(50) null,
PolicyNumber13 varchar(50) null,
PolicyQualifier14 varchar(50) null,
PolicyNumber14 varchar(50) null,
PriorCoverageMonthCount varchar(50) null,
PlanCoverageDescription1 varchar(50) null,
IdCardTypeCode1 varchar(50) null,
IdCardCount1 varchar(50) null,
ActionCode1 varchar(50) null,
PlanCoverageDescription2 varchar(50) null,
IdCardTypeCode2 varchar(50) null,
IdCardCount2 varchar(50) null,
ActionCode2 varchar(50) null,
PlanCoverageDescription3 varchar(50) null,
IdCardTypeCode3 varchar(50) null,
IdCardCount3 varchar(50) null,
ActionCode3 varchar(50) null,
 CONSTRAINT [PK_M834HealthCoverage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



create table M834HCProviderInfo(
Id int identity(1,1) not null,
FileId int not null,
DetailId varchar(50) not null,
HCId varchar(50) not null,
SequenceNumber varchar(50) null,
ProviderQualifier varchar(50) null,
ProviderLastName varchar(200) null,
ProviderFirstName varchar(200) null,
ProviderMiddleName varchar(50) null,
ProviderPrefix varchar(50) null,
ProviderSuffix varchar(50) null,
ProviderIdQualifier varchar(50) null,
ProviderId varchar(50) null,
EntityRelationshipCode varchar(50) null,
ProviderAddress1 varchar(200) null,
ProviderAddress12 varchar(200) null,
ProviderAddress2 varchar(200) null,
ProviderAddress22 varchar(200) null,
ProviderAddressCity varchar(50) null,
ProviderAddressState varchar(50) null,
ProviderAddressZip varchar(50) null,
ProviderAddressCountry varchar(50) null,
ProviderAddressCountrySubCode varchar(50) null,
ProviderContact1Qualifier1 varchar(50) null,
ProviderContact1Number1 varchar(50) null,
ProviderContact1Qualifier2 varchar(50) null,
ProviderContact1Number2 varchar(50) null,
ProviderContact1Qualifier3 varchar(50) null,
ProviderContact1Number3 varchar(50) null,
ProviderContact2Qualifier1 varchar(50) null,
ProviderContact2Number1 varchar(50) null,
ProviderContact2Qualifier2 varchar(50) null,
ProviderContact2Number2 varchar(50) null,
ProviderContact2Qualifier3 varchar(50) null,
ProviderContact2Number3 varchar(50) null,
ProviderChangeCode varchar(50) null,
ProviderChangeQualifier varchar(50) null,
ProviderChangeDate varchar(50) null,
ProviderChangeReasonCode varchar(50) null,
 CONSTRAINT [PK_M834HCProviderInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

create table M834HCCOBInfo(
Id int identity(1,1) not null,
FileId int not null,
DetailId varchar(50) not null,
HCId varchar(50) not null,
SequenceCode varchar(50) null,
MemberGroupNumber varchar(50) null,
CobCode varchar(50) null,
ServiceTypeCode varchar(50) null,
CobQualifier1 varchar(50) null,
CobPolicyNumber1 varchar(50) null,
CobQualifier2 varchar(50) null,
CobPolicyNumber2 varchar(50) null,
CobQualifier3 varchar(50) null,
CobPolicyNumber3 varchar(50) null,
CobQualifier4 varchar(50) null,
CobPolicyNumber4 varchar(50) null,
CobBeginDate varchar(50) null,
CobEndDate varchar(50) null,
Entity1Qualifier varchar(50) null,
Entity1LastName varchar(50) null,
Entity1IdQualifier varchar(50) null,
Entity1Id varchar(50) null,
Entity1Address varchar(200) null,
Entity1Address2 varchar(200) null,
Entity1AddressCity varchar(50) null,
Entity1AddressState varchar(50) null,
Entity1AddressZip varchar(50) null,
Entity1AddressCountry varchar(50) null,
Entity1AddressCountrySubCode varchar(50) null,
Entity1ContactQualifier varchar(50) null,
Entity1ContactNumber varchar(50) null,
Entity2Qualifier varchar(50) null,
Entity2LastName varchar(50) null,
Entity2IdQualifier varchar(50) null,
Entity2Id varchar(50) null,
Entity2Address varchar(200) null,
Entity2Address2 varchar(200) null,
Entity2AddressCity varchar(50) null,
Entity2AddressState varchar(50) null,
Entity2AddressZip varchar(50) null,
Entity2AddressCountry varchar(50) null,
Entity2AddressCountrySubCode varchar(50) null,
Entity2ContactQualifier varchar(50) null,
Entity2ContactNumber varchar(50) null,
Entity3Qualifier varchar(50) null,
Entity3LastName varchar(50) null,
Entity3IdQualifier varchar(50) null,
Entity3Id varchar(50) null,
Entity3Address varchar(200) null,
Entity3Address2 varchar(200) null,
Entity3AddressCity varchar(50) null,
Entity3AddressState varchar(50) null,
Entity3AddressZip varchar(50) null,
Entity3AddressCountry varchar(50) null,
Entity3AddressCountrySubCode varchar(50) null,
Entity3ContactQualifier varchar(50) null,
Entity3ContactNumber varchar(50) null,
 CONSTRAINT [PK_M834HCCOBInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
