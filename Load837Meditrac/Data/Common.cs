using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Load837Meditrac.Data
{
    public class Common
    {
        public static string cn { get; set; }
        public static string cn_Meditrac { get; set; }
        public static string StatusCodes { get; set; }
        public static string GroupNumbers { get; set; }
        public static string ClaimType { get; set; }
        public static string StartDate { get; set; }
        public static string EndDate { get; set; }
        public static int PageNumber { get; set; }
        public static int TotalCMSP { get; set; }
        public static int ActualCMSP { get; set; }
        public static int TotalCMSI { get; set; }
        public static int ActualCMSI { get; set; }
        public static int TotalDualP { get; set; }
        public static int ActualDualP { get; set; }
        public static int TotalDualI { get; set; }
        public static int ActualDualI { get; set; }
        public static int TotalStateP { get; set; }
        public static int ActualStateP { get; set; }
        public static int TotalStateI { get; set; }
        public static int ActualStateI { get; set; }
        public static List<string> DMEProcCodes { get; set; }
        public static string QueryMeditracCounts = "select count(*) from claims (nolock) a inner join groups (nolock) b on a.GroupId=b.GroupId where a.ProcessingStatus='CLS' and a.Status in ({0}) and left(b.GroupNumber,3) in ({1}) and a.FormType='{2}' and a.LastUpdatedAt between '{3}' and '{4}'";
        public static string QueryMeditracHeader = @"select p1.ProviderSpecialtyPrimaryTaxonomyCode as BillingTaxonomyCode,
p1.ProviderLastName as BillingLastName,
p1.ProviderFirstName as BillingFirstName,
p1.ProviderMiddleInitial as BillingMiddleInitial,
p1.ProviderNPI as BillingNPI,
p1.OfficeAddress1 as BillingAddress,
p1.OfficeAddress2 as BillingAddress2,
p1.OfficeCity as BillingCity,
p1.OfficeState as BillingState,
p1.OfficeZip as BillingZip,
p1.CorporationEIN as BillingTaxId,
i.ClaimFilingIndicatorCode as ClaimFilingInd,
m.LastName as SubscriberLastName,
m.FirstName as SubscriberFirstName,
m.middlename as SubscriberMiddleInitial,
'MI' as SubscriberIdQual,
m.Policynumber as CIN,
m.HICN,
m.Address1 as SubscriberAddress,
m.Address2 as SubscriberAddress2,
m.City as SubscriberCity,
m.State as SubscriberState,
m.Zip as SubscriberZip,
convert(varchar(8),m.DateOfBirth,112) as SubscriberDateOfBirth,
m.Gender as SubscriberGender,
a.ClaimId,
b.TotalCharges as ChargeAmount,
case when a.formtype='HCF' then PlaceOfService else left(b.TypeOfBill,2) end as FacilityCode,
case when a.formtype='HCF' then 'B' else 'A' end as ClaimType,
case when a.formtype='HCF' then '1' else right(b.TypeOfBill,1) end as ClaimFrequencyCode,
ProviderSignature as ProviderSignatureInd,
AcceptAssignment as ProviderAssignmentInd,
b.AssignmentBenefits as BenefitAssignmentInd,
isnull(ReleaseInformation,'Y') as ReleaseOfInformationInd,
PatientSignature as PatientSignatureInd,
DelayReason as DelayReasonCode,
convert(varchar(8),AdmissionDate,112) as AdmissionDate,
case when a.status='CLD' then '09' when COBPayerPaidAmount=0 then '05' else '02' end as ContractTypeCode,
ContractAmount,
PatientLiabilityAmount as PatientPaidAmount,
a.ExternalClaimNumber as ExternalClaimId,
ClaimSubmissionNumber as MeditracSubmissionNumber,
AuthorizationNumber,
null as PayerControlNumber,
MedicalRecordNumber,
p2.ProviderSpecialtyPrimaryTaxonomyCode as ReferringTaxonomyCode,
p2.ProviderLastName as ReferringLastName,
p2.ProviderFirstName as ReferringFirstName,
p2.ProviderMiddleInitial as ReferringMiddleInitial,
p2.ProviderNPI as ReferringNPI,
p2.OfficeAddress1 as ReferringAddress,
p2.OfficeAddress2 as ReferringAddress2,
p2.OfficeCity as ReferringCity,
p2.OfficeState as ReferringState,
p2.OfficeZip as ReferringZip,
p2.CorporationEIN as ReferringTaxId,
p3.ProviderLastName as RenderingLastName,
p3.ProviderFirstName as RenderingFirstName,
p3.ProviderMiddleInitial as RenderingMiddleInitial,
p3.ProviderMiddleInitial as RenderingNPI,
p3.ProviderSpecialtyPrimaryTaxonomyCode as RenderingTaxonomyCode,
p3.OfficeAddress1 as RenderingAddress,
p3.OfficeAddress2 as RenderingAddress2,
p3.OfficeCity as RenderingCity,
p3.OfficeState as RenderingState,
p3.OfficeZip as RenderingZip,
p3.CorporationEIN as RenderingTaxId,
d.FacilityName as ServiceFacilityLastName,
d.FacilityAddress1 as ServiceFAcilityAddress,
d.FacilityAddress2 as ServiceFacilityAddress2,
d.FacilityCity as ServiceFacilityCity,
d.FacilityState as ServiceFacilityState,
d.FacilityZip as ServiceFacilityZip,
COBPayerPaidAmount, 
COBTotalNonCoveredAmount,
amtRemainingPAtientLiability as RemainingPatientLiabilityAmount,
DischargeHour,
convert(varchar(8),StatementCoversFrom,112) as StatementDateFrom,
convert(varchar(8),StatementCoversTo,112) as StatementDateTo,
b.TypeOfAdmission as AdmissionTypeCode,
b.SourceOfAdmission as AdmissionSourceCode,
PatientStatus as PatientStatusCode,
p4.ProviderLastName as AttLastName,
p4.ProviderFirstName as  AttFirstName,
p4.ProviderMiddleInitial as AttMiddleInitial,
p4.ProviderNPI as AttNPI,
p4.ProviderSpecialtyPrimaryTaxonomyCode as AttTaxonomyCode,
p4.OfficeAddress1 as AttAddress,
p4.OfficeAddress2 as AttAddress2,
p4.OfficeCity as AttCity,
p4.OfficeState as AttState,
p4.OfficeZip as AttZip,
p4.CorporationEIN as AttTaxId,
p5.ProviderLastName as OprLastName,
p5.ProviderFirstName as OprFirstName,
p5.ProviderMiddleInitial as OprMiddleInitial,
p5.ProviderNPI as OprNPI,
p6.ProviderLastName as OthLastName,
p6.ProviderFirstName as OthFirstName,
p6.ProviderMiddleInitial as OthMiddleInitial,
p6.ProviderNPI as OthNPI,
left(c.GroupNumber,3) as GroupNumber,
o.MedicaidID 
from claims (nolock) a 
inner join claim_master (nolock) b on a.claimid=b.claimid and a.AdjustmentVersion=b.AdjustmentVersion 
inner join Groups (nolock) c on a.GroupId=c.GroupId 
left join claim_master_data (nolock) d on d.claimid=a.claimid and d.AdjustmentVersion=a.AdjustmentVersion 
left join claimcobdata (nolock) i on i.claimid=a.claimid and i.AdjustmentVersion=a.AdjustmentVersion 
cross apply(select sum(isnull(AmtToPay,0)) as COBPayerPaidAmount,sum(isnull(AmtNotCovered,0)) as COBTotalNonCoveredAmount,sum(isnull(AmtPatientLiability,0)) as PatientLiabilityAmount,sum(isnull(amtEligible,0)) as ContractAmount from claim_results (nolock) where claimid=a.claimid and AdjustmentVersion=a.AdjustmentVersion) j 
cross apply(select min(placeofservice) as PlaceOfService from claim_details (nolock) where claimid=a.claimid and AdjustmentVersion=a.AdjustmentVersion) k 
outer apply (select top 1 ClaimSubmissionnumber from claimsubmissions (nolock) where claimid=a.claimid and InputAdjustmentVersion=a.AdjustmentVersion order by ClaimSubmissionId desc) l 
inner join meditrac.member (nolock) m on m.MemberID=a.MemberId 
left join diamond.Member o on o.MemberNumber=m.MemberNumber 
outer apply (select top 1 ProviderSpecialtyPrimaryTaxonomyCode,ProviderLastName,ProviderFirstName,ProviderMiddleInitial,ProviderNPI,CorporationEIN,OfficeAddress1,OfficeAddress2,OfficeCity,OfficeState,OfficeZip from meditrac.providerdata (nolock) where providerid=a.providerid and PanelDescription<>'TRM' order by PanelDescription desc) p1 
outer apply (select top 1 ProviderSpecialtyPrimaryTaxonomyCode,ProviderLastName,ProviderFirstName,ProviderMiddleInitial,ProviderNPI,CorporationEIN,OfficeAddress1,OfficeAddress2,OfficeCity,OfficeState,OfficeZip from meditrac.providerdata (nolock) where ProviderId=b.ReferringProviderId and PanelDescription<>'TRM' order by PanelDescription desc) p2 
outer apply (select top 1 ProviderLastName,ProviderFirstName,ProviderMiddleInitial,ProviderNPI,ProviderSpecialtyPrimaryTaxonomyCode,CorporationEIN,OfficeAddress1,OfficeAddress2,OfficeCity,OfficeState,OfficeZip from meditrac.providerdata (nolock) where providerid=(select top 1 renderingproviderid from Claim_Details where claimid=a.claimid and RenderingProviderId<>a.ProviderId) and PanelDescription<>'TRM' order by PanelDescription desc) p3 
outer apply (select top 1 ProviderLastName,ProviderFirstName,ProviderMiddleInitial,ProviderNPI,ProviderSpecialtyPrimaryTaxonomyCode,CorporationEIN,OfficeAddress1,OfficeAddress2,OfficeCity,OfficeState,OfficeZip from meditrac.providerdata (nolock) where providerid=b.AttendingProviderId and PanelDescription<>'TRM' order by PanelDescription desc) p4 
outer apply (select top 1 ProviderLastName,ProviderFirstName,ProviderMiddleInitial,ProviderNPI from meditrac.providerdata (nolock) where providerid=b.OperatingProviderId and PanelDescription<>'TRM' order by PanelDescription desc) p5 
outer apply (select top 1 ProviderLastName,ProviderFirstName,ProviderMiddleInitial,ProviderNPI from meditrac.providerdata (nolock) where providerid=b.OtherProviderId and PanelDescription<>'TRM' order by PanelDescription desc) p6 
where a.ProcessingStatus='CLS' 
and a.Status in ({0}) 
and left(c.GroupNumber,3) in ({1}) 
and a.FormType='{2}' 
and a.LastUpdatedAt between '{3}' and '{4}' 
order by a.claimid offset {5} rows fetch next 10000 rows only
";
        public static string QueryMeditracLine = @"select d.ClaimId
,d.LineNumber
,case FormType when 'HCF' then d.ProcedureCode else d.HCPCSRates end as ProcedureCode
,d.Modifier as Modifier1
,d.Modifier2
,d.Modifier3
,d.Modifier4
,d.Description as ProcedureDescription
,e.AmtCharged as LineChargeAmount
,d.UnitType as UnitOfMeasure
,d.ServiceUnits as Quantity
,d.PlaceOfService
,d.DiagnosisPtr1 as DiagPointer1
,d.DiagnosisPtr2 as DiagPointer2
,d.DiagnosisPtr3 as DiagPointer3
,d.DiagnosisPtr4 as DiagPointer4
,d.EMG as EmergencyInd
,d.EPSDTPlan as EPSDTInd
,convert(varchar(8),d.ServiceDateFrom,112) as ServiceDateFrom
,convert(varchar(8),d.ServiceDateTo,112) as ServiceDateTo
,d.ProductCode as NationalDrugCode
,d.ProductQuantity as DrugQuantity
,d.ProductUnitOfMeasure as DrugUnit
,e.AmtToPay as LinePaidAmount
,convert(varchar(8),e.LastUpdatedAt,112) as PaidDate
,e.AmtDeductible as LineDeductAmount
,e.AmtCoinsurance as LineCoinsuranceAmount
,e.AmtCopay as LineCopayAmount
,e.AmtCOB as LineCOBPaidAmount
,d.ProcedureCode as RevenueCode 
from  claim_details (nolock) d 
inner join claim_results (nolock) e on e.claimid=d.claimid and e.AdjustmentVersion=d.AdjustmentVersion and e.LineNumber=d.LineNumber 
left join ClaimCOBDataDetails (nolock) f on f.ClaimID=d.ClaimID and f.AdjustmentVersion=d.AdjustmentVersion and f.LineNumber=d.LineNumber 
inner join (select claimid,adjustmentversion,FormType from claims (nolock) a inner join groups (nolock) c on a.groupid=c.groupid 
where a.ProcessingStatus='CLS' 
and a.Status in ({0}) 
and left(c.GroupNumber,3) in ({1}) 
and a.FormType='{2}' 
and a.LastUpdatedAt between '{3}' and '{4}' 
order by a.claimid offset {5} rows fetch next 10000 rows only
) t on t.claimid=d.ClaimID and t.AdjustmentVersion=d.AdjustmentVersion
";
        public static string QueryMeditracCode = @"select b.ClaimId,Sequence,CodeType,Code,convert(varchar(8),daterecorded,112) as DateRecorded,convert(varchar(8),datethrough,112) as DateThrough,Amount,DiagnosisCodeQualifier, POAIndicator 
from Claim_Codes (nolock) b 
inner join (select claimid,adjustmentversion from claims (nolock) a inner join groups (nolock) c on a.groupid=c.groupid 
where a.ProcessingStatus='CLS' 
and a.Status in ({0}) 
and left(c.GroupNumber,3) in ({1}) 
and a.FormType='{2}' 
and a.LastUpdatedAt between '{3}' and '{4}' 
order by a.claimid offset {5} rows fetch next 10000 rows only
) t on t.claimid=b.ClaimID and t.AdjustmentVersion=b.AdjustmentVersion
";
    }
}
