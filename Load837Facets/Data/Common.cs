using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Load837Facets.Data
{
    public class Common
    {
		public static string cn { get; set; }
		public static string cn_Facets { get; set; }
		public static string PlanId { get; set; }
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
		public static List<string> Includes { get; set; }
		public static List<string> Excludes { get; set; }
		public static string FacetsCount = @"select count(distinct HDR.CLCL_ID) as ClaimCount
	 From FACETS.dbo.CMC_CLCL_CLAIM HDR
INNER JOIN  FACETS.dbo.CMC_MEME_MEMBER MBR On (HDR.MEME_CK = MBR.MEME_CK)
INNER JOIN FACETS.dbo.CMC_SBSB_SUBSC SUBSC On (SUBSC.SBSB_CK = MBR.SBSB_CK)
WHERE HDR.CLCL_PAID_DT BETWEEN  '{0}' AND '{1}'
AND isnull(HDR.CLCL_ID,'') <> '' 
AND HDR.CLCL_CUR_STS IN('01','02','11','15','91') 
AND HDR.CLCL_CL_SUB_TYPE='{2}'
";

		public static string FacetsHeader = @"
Select 
	HDR.CLCL_ID As ClaimID
	, substring(HDR.CSPI_ID,1,5) As PlanID
	, CASE HDR.CLCL_CL_SUB_TYPE WHEN 'H' THEN 'I'
	  WHEN 'M' THEN 'P'
	  END As ClaimType
	, CASE WHEN ISNULL(HDR.CLCL_CAP_IND,'') = '' THEN '1' WHEN HDR.CLCL_CAP_IND= 'F' THEN '1' ELSE '0' END  As IsCapitated
	, HDR.CLCL_TOT_CHG As HeaderChargeAmt
	, HDR.CLCL_TOT_PAYABLE As HeaderPaidAmt
	, HDR.CLCL_PA_PAID_AMT As HeaderPatPaidAmt
	, HDR.CLCL_REL_INFO_IND As RelInfo
	, HDR.CLCL_ACD_STATE As ACDState
	, HDR.CLCL_OTHER_BN_IND As BenAssigned
	, HDR.CLCL_PAID_DT As PaidDate
	, HDR.CLCL_LOW_SVC_DT As DOSFromDate
	, HDR.CLCL_HIGH_SVC_DT As DOSToDate
	, HDR.CLCL_ID_ADJ_TO As ClaimIdAdjTo
	, HDR.CLCL_ID_ADJ_FROM As ClaimIdAdjFrom
	, HDR.CLCL_RECD_DT As ReceivedDate
	, cast(HDR.MEME_CK as varchar(15)) As MemberCK
	, substring(HOSP.CLHP_FAC_TYPE,patindex('%[^0]%',HOSP.CLHP_FAC_TYPE),1) As FacType
	, HOSP.CLHP_BILL_CLASS As BillClass
	, HOSP.CLHP_FREQUENCY As Frequency
	, HOSP.CLHP_DC_HOUR_CD As DisChgHr
	, HOSP.CLHP_STAMENT_FR_DT As StatFromDte
	, HOSP.CLHP_STAMENT_TO_DT As StatToDte
	, HOSP.CLHP_ADM_DT As AdmitDte
	, HOSP.CLHP_ADM_HOUR_CD As AdmitHr
	, substring(HOSP.CLHP_ADM_TYP,1,1) As AdmitType
	, HOSP.CLHP_ADM_SOURCE As AdmitSource
	, HOSP.CLHP_DC_STAT As DisChgSts
	, HOSP.CLHP_MED_REC_NO As MedRecNo
	, HOSP.CLHP_COVD_DAYS As CoveredDays
	, HDR.CLCL_SIMI_ILL_DT As SimilarIllnessDate
	, HOSP.CLHP_INPUT_AGRG_ID As DRGInfo 
	, HOSP.CLHP_DC_DT As DischargeDte
	, HDR.CLCL_INPUT_METH As InputMethod
    , SUBSC.SBSB_ID+Right('00'+cast(MEME_SFX as varchar(10)),2) As MemberID
    , MBR.MEME_SEX As MemberSex
    , MBR.MEME_BIRTH_DT As MemberBirthDate
	, MBR.MEME_SSN As MemberSSN
	, upper(MBR.MEME_LAST_NAME) As MemberLastName
	, upper(MBR.MEME_FIRST_NAME) As MemberFirstName 
	, MBR.MEME_MID_INIT As MemberMiddleInit
	, SBADADDR.SBAD_ADDR1 as MemberAddress1
	, SBADADDR.SBAD_ADDR2 As MemberAddress2
	, SBADADDR.SBAD_CITY As MemberAddressCity
	, SBADADDR.SBAD_STATE As MemberAddressState
	, left(SBADADDR.SBAD_ZIP,9) As MemberAddressZip
	,MBR.MEME_HICN as MedicareId
	,substring(MBR.MEME_MEDCD_NO,1,10) as  MedicaidId
	,substring(HDR.CSPI_ID,1,9) COBPayerId
	 From FACETS.dbo.CMC_CLCL_CLAIM HDR
	Left Join FACETS.dbo.CMC_CLHP_HOSP HOSP On (HDR.CLCL_ID = HOSP.CLCL_ID)
	Left Join FACETS.dbo.CMC_SGSG_SUB_GROUP SGSG On (HDR.SGSG_CK = SGSG.SGSG_CK)
	Left Join FACETS.dbo.CMC_CLED_EDI_DATA CLED On (HDR.CLCL_ID = CLED.CLCL_ID)
	Left Join FACETS.dbo.CMC_CLCK_CLM_CHECK FCLMCHK On (HDR.CLCL_ID =  FCLMCHK.CLCL_ID)
	Left Join FACETS.dbo.CMC_CDML_CL_LINE LINE On HDR.CLCL_ID=LINE.CLCL_ID
INNER JOIN  FACETS.dbo.CMC_MEME_MEMBER MBR On (HDR.MEME_CK = MBR.MEME_CK)
INNER JOIN FACETS.dbo.CMC_SBSB_SUBSC SUBSC On (SUBSC.SBSB_CK = MBR.SBSB_CK)
Left Join FACETS.dbo.CMC_SBAD_ADDR SBADADDR On (SUBSC.SBSB_CK = SBADADDR.SBSB_CK And SUBSC.SBAD_TYPE_HOME = SBADADDR.SBAD_TYPE)
WHERE HDR.CLCL_PAID_DT BETWEEN  '{1}' AND '{2}'
AND isnull(HDR.CLCL_ID,'') <> '' 
AND HDR.CLCL_CUR_STS IN('01','02','11','15','91') 
AND HDR.CLCL_CL_SUB_TYPE='{3}'
ORDER BY HDR.CLCL_ID
offset {4} rows fetch next 10000 rows only";

		public static string FacetsProvider = @"
SELECT      
HDR.CLCL_ID As ClaimID,
'Billing' As ProviderType,
[PROV].[PRPR_ID] AS [ProviderID],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_LAST_NAME] else PROV.PRPR_NAME end AS [ProviderLastName],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_FIRST_NAME] else null end AS [ProviderFirstName],
PROV.MCTN_ID As ProviderTaxID,
SUBSTRING([PROVADDR].[PRAD_ADDR1], 1, 55) AS [ProviderAddress1],
SUBSTRING([PROVADDR].[PRAD_ADDR2], 1, 55) AS [ProviderAddress2],
[PROVADDR].[PRAD_CITY] AS [ProviderAddressCity],
[PROVADDR].[PRAD_STATE] AS [ProviderAddressState],
substring([PROVADDR].[PRAD_ZIP],1,9) AS [ProviderAddressZip],
PROV.MCTN_ID as ProviderMedID,
PROV.PRPR_NPI as ProviderNPI,
[PROV].[PRCF_MCTR_SPEC] AS [ProviderSpecialtyCode],
[PROV].[PRPR_STS] AS [ProviderStatus],
[PROV].[PRPR_TAXONOMY_CD] AS [ProviderTaxonomy],
[PROVADDR].[PRAD_EFF_DT] AS [EffectiveDate],
[PROVADDR].[PRAD_TERM_DT] AS [TerminateDate],
[PROVADDR].[PRAD_PHONE] AS [ProviderTelephone]
From (SELECT CLCL_ID,CLCL_PAYEE_PR_ID FROM FACETS.dbo.CMC_CLCL_CLAIM
WHERE CLCL_PAID_DT BETWEEN  '{1}' AND '{2}'
AND isnull(CLCL_ID,'') <> '' 
AND CLCL_CUR_STS IN('01','02','11','15','91') 
AND CLCL_CL_SUB_TYPE='{3}'
ORDER BY CLCL_ID
offset {4} rows fetch next 10000 rows only
) HDR
INNER JOIN FACETS.dbo.[CMC_PRPR_PROV] [PROV] ON HDR.CLCL_PAYEE_PR_ID = [PROV].PRPR_ID
LEFT JOIN FACETS.dbo.[CMC_PRCP_COMM_PRAC] [PROV_PRAC] ON( [PROV].[PRCP_ID] = [PROV_PRAC].[PRCP_ID] )
LEFT JOIN FACETS.dbo.[CMC_PRAD_ADDRESS] [PROVADDR] ON( [PROV].[PRAD_ID] = [PROVADDR].[PRAD_ID]
	AND [PROV].[PRAD_TYPE_PRIM] = [PROVADDR].[PRAD_TYPE])
where ltrim(isnull(PROV.PRPR_ID,''))<>''

union      

select
HDR.CLCL_ID As ClaimID,
'Rendering' As ProviderType,
[PROV].[PRPR_ID] AS [ProviderID],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_LAST_NAME] else PROV.PRPR_NAME end AS [ProviderLastName],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_FIRST_NAME] else null end AS [ProviderFirstName],
PROV.MCTN_ID As ProviderTaxID,
SUBSTRING([PROVADDR].[PRAD_ADDR1], 1, 55) AS [ProviderAddress1],
SUBSTRING([PROVADDR].[PRAD_ADDR2], 1, 55) AS [ProviderAddress2],
[PROVADDR].[PRAD_CITY] AS [ProviderAddressCity],
[PROVADDR].[PRAD_STATE] AS [ProviderAddressState],
substring([PROVADDR].[PRAD_ZIP],1,9) AS [ProviderAddressZip],
PROV.MCTN_ID as ProviderMedID,
PROV.PRPR_NPI as ProviderNPI,
[PROV].[PRCF_MCTR_SPEC] AS [ProviderSpecialtyCode],
[PROV].[PRPR_STS] AS [ProviderStatus],
[PROV].[PRPR_TAXONOMY_CD] AS [ProviderTaxonomy],
[PROVADDR].[PRAD_EFF_DT] AS [EffectiveDate],
[PROVADDR].[PRAD_TERM_DT] AS [TerminateDate],
[PROVADDR].[PRAD_PHONE] AS [ProviderTelephone]
From (SELECT CLCL_ID,PRPR_ID FROM FACETS.dbo.CMC_CLCL_CLAIM
WHERE CLCL_PAID_DT BETWEEN  '{1}' AND '{2}'
AND isnull(CLCL_ID,'') <> '' 
AND CLCL_CUR_STS IN('01','02','11','15','91') 
AND CLCL_CL_SUB_TYPE='{3}'
ORDER BY CLCL_ID
offset {4} rows fetch next 10000 rows only
) HDR
Inner Join FACETS.dbo.CMC_CLPE_PROV_DATA RE on RE.CLCL_ID=HDR.CLCL_ID and RE.CLPE_PRPR_TYPE='RE'
INNER JOIN FACETS.dbo.[CMC_PRPR_PROV] [PROV] ON case when ltrim(isnull(RE.CLPE_PRPR_ID,''))='' then HDR.PRPR_ID else RE.CLPE_PRPR_ID end = [PROV].PRPR_ID
LEFT JOIN FACETS.dbo.[CMC_PRCP_COMM_PRAC] [PROV_PRAC] ON( [PROV].[PRCP_ID] = [PROV_PRAC].[PRCP_ID] )
LEFT JOIN FACETS.dbo.[CMC_PRAD_ADDRESS] [PROVADDR] ON( [PROV].[PRAD_ID] = [PROVADDR].[PRAD_ID]
	AND [PROV].[PRAD_TYPE_PRIM] = [PROVADDR].[PRAD_TYPE])
where ltrim(isnull(PROV.PRPR_ID,''))<>''

union

Select HDR.CLCL_ID As ClaimID, 
'Referring' As ProviderType,
[PROV].[PRPR_ID] AS [ProviderID],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_LAST_NAME] else PROV.PRPR_NAME end AS [ProviderLastName],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_FIRST_NAME] else null end AS [ProviderFirstName],
PROV.MCTN_ID As ProviderTaxID,
SUBSTRING([PROVADDR].[PRAD_ADDR1], 1, 55) AS [ProviderAddress1],
SUBSTRING([PROVADDR].[PRAD_ADDR2], 1, 55) AS [ProviderAddress2],
[PROVADDR].[PRAD_CITY] AS [ProviderAddressCity],
[PROVADDR].[PRAD_STATE] AS [ProviderAddressState],
substring([PROVADDR].[PRAD_ZIP],1,9) AS [ProviderAddressZip],
PROV.MCTN_ID as ProviderMedID,
PROV.PRPR_NPI as ProviderNPI,
[PROV].[PRCF_MCTR_SPEC] AS [ProviderSpecialtyCode],
[PROV].[PRPR_STS] AS [ProviderStatus],
[PROV].[PRPR_TAXONOMY_CD] AS [ProviderTaxonomy],
[PROVADDR].[PRAD_EFF_DT] AS [EffectiveDate],
[PROVADDR].[PRAD_TERM_DT] AS [TerminateDate],
[PROVADDR].[PRAD_PHONE] AS [ProviderTelephone]
From (SELECT CLCL_ID FROM FACETS.dbo.CMC_CLCL_CLAIM
WHERE CLCL_PAID_DT BETWEEN  '{1}' AND '{2}'
AND isnull(CLCL_ID,'') <> '' 
AND CLCL_CUR_STS IN('01','02','11','15','91') 
AND CLCL_CL_SUB_TYPE='{3}'
ORDER BY CLCL_ID
offset {4} rows fetch next 10000 rows only
) HDR
Inner Join FACETS.dbo.CMC_CLPE_PROV_DATA RF on RF.CLCL_ID=HDR.CLCL_ID and RF.CLPE_PRPR_TYPE='RF'
INNER JOIN FACETS.dbo.[CMC_PRPR_PROV] [PROV] ON RF.CLPE_PRPR_ID = [PROV].PRPR_ID
LEFT JOIN FACETS.dbo.[CMC_PRCP_COMM_PRAC] [PROV_PRAC] ON( [PROV].[PRCP_ID] = [PROV_PRAC].[PRCP_ID] )
LEFT JOIN FACETS.dbo.[CMC_PRAD_ADDRESS] [PROVADDR] ON( [PROV].[PRAD_ID] = [PROVADDR].[PRAD_ID]
	AND [PROV].[PRAD_TYPE_PRIM] = [PROVADDR].[PRAD_TYPE])
where ltrim(isnull(PROV.PRPR_ID,''))<>''

union

Select HDR.CLCL_ID As ClaimID,
'Referring' As ProviderType,
[PROV].[PRPR_ID] AS [ProviderID],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_LAST_NAME] else PROV.PRPR_NAME end AS [ProviderLastName],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_FIRST_NAME] else null end AS [ProviderFirstName],
PROV.MCTN_ID As ProviderTaxID,
SUBSTRING([PROVADDR].[PRAD_ADDR1], 1, 55) AS [ProviderAddress1],
SUBSTRING([PROVADDR].[PRAD_ADDR2], 1, 55) AS [ProviderAddress2],
[PROVADDR].[PRAD_CITY] AS [ProviderAddressCity],
[PROVADDR].[PRAD_STATE] AS [ProviderAddressState],
substring([PROVADDR].[PRAD_ZIP],1,9) AS [ProviderAddressZip],
PROV.MCTN_ID as ProviderMedID,
PROV.PRPR_NPI as ProviderNPI,
[PROV].[PRCF_MCTR_SPEC] AS [ProviderSpecialtyCode],
[PROV].[PRPR_STS] AS [ProviderStatus],
[PROV].[PRPR_TAXONOMY_CD] AS [ProviderTaxonomy],
[PROVADDR].[PRAD_EFF_DT] AS [EffectiveDate],
[PROVADDR].[PRAD_TERM_DT] AS [TerminateDate],
[PROVADDR].[PRAD_PHONE] AS [ProviderTelephone]
From (SELECT CLCL_ID,CLCL_PRPR_ID_REF,CLCL_PRPR_ID_PCP FROM FACETS.dbo.CMC_CLCL_CLAIM
WHERE CLCL_PAID_DT BETWEEN  '{1}' AND '{2}'
AND isnull(CLCL_ID,'') <> '' 
AND CLCL_CUR_STS IN('01','02','11','15','91') 
AND CLCL_CL_SUB_TYPE='{3}'
ORDER BY CLCL_ID
offset {4} rows fetch next 10000 rows only
) HDR 
INNER JOIN FACETS.dbo.[CMC_PRPR_PROV] [PROV] ON case when ltrim(isnull(HDR.CLCL_PRPR_ID_REF,''))='' then HDR.CLCL_PRPR_ID_PCP else HDR.CLCL_PRPR_ID_REF end = [PROV].PRPR_ID
LEFT JOIN FACETS.dbo.[CMC_PRCP_COMM_PRAC] [PROV_PRAC] ON( [PROV].[PRCP_ID] = [PROV_PRAC].[PRCP_ID] )
LEFT JOIN FACETS.dbo.[CMC_PRAD_ADDRESS] [PROVADDR] ON( [PROV].[PRAD_ID] = [PROVADDR].[PRAD_ID]
	AND [PROV].[PRAD_TYPE_PRIM] = [PROVADDR].[PRAD_TYPE])
where ltrim(isnull(PROV.PRPR_ID,''))<>''

union      

select  Distinct
HDR.CLCL_ID As ClaimID,
'ServiceFacility' As ProviderType,
[PROV].[PRPR_ID] AS [ProviderID],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_LAST_NAME] else PROV.PRPR_NAME end AS [ProviderLastName],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_FIRST_NAME] else null end AS [ProviderFirstName],
PROV.MCTN_ID As ProviderTaxID,
SUBSTRING([PROVADDR].[PRAD_ADDR1], 1, 55) AS [ProviderAddress1],
SUBSTRING([PROVADDR].[PRAD_ADDR2], 1, 55) AS [ProviderAddress2],
[PROVADDR].[PRAD_CITY] AS [ProviderAddressCity],
[PROVADDR].[PRAD_STATE] AS [ProviderAddressState],
substring([PROVADDR].[PRAD_ZIP],1,9) AS [ProviderAddressZip],
PROV.MCTN_ID as ProviderMedID,
PROV.PRPR_NPI as ProviderNPI,
[PROV].[PRCF_MCTR_SPEC] AS [ProviderSpecialtyCode],
[PROV].[PRPR_STS] AS [ProviderStatus],
[PROV].[PRPR_TAXONOMY_CD] AS [ProviderTaxonomy],
[PROVADDR].[PRAD_EFF_DT] AS [EffectiveDate],
[PROVADDR].[PRAD_TERM_DT] AS [TerminateDate],
[PROVADDR].[PRAD_PHONE] AS [ProviderTelephone]
From (SELECT CLCL_ID FROM FACETS.dbo.CMC_CLCL_CLAIM
WHERE CLCL_PAID_DT BETWEEN  '{1}' AND '{2}'
AND isnull(CLCL_ID,'') <> '' 
AND CLCL_CUR_STS IN('01','02','11','15','91') 
AND CLCL_CL_SUB_TYPE='{3}'
ORDER BY CLCL_ID
offset {4} rows fetch next 10000 rows only
) HDR
Inner Join FACETS.dbo.CMC_CLPE_PROV_DATA FA on FA.CLCL_ID=HDR.CLCL_ID and FA.CLPE_PRPR_TYPE='FA'
INNER JOIN FACETS.dbo.[CMC_PRPR_PROV] [PROV] ON FA.CLPE_NPI = [PROV].PRPR_NPI and PROV.PRPR_ENTITY='F'
LEFT JOIN FACETS.dbo.[CMC_PRCP_COMM_PRAC] [PROV_PRAC] ON( [PROV].[PRCP_ID] = [PROV_PRAC].[PRCP_ID] )
LEFT JOIN FACETS.dbo.[CMC_PRAD_ADDRESS] [PROVADDR] ON( [PROV].[PRAD_ID] = [PROVADDR].[PRAD_ID]
	AND [PROV].[PRAD_TYPE_PRIM] = [PROVADDR].[PRAD_TYPE])
where ltrim(isnull(PROV.PRPR_NPI,''))<>''

union      

select
HDR.CLCL_ID As ClaimID,
'Attending' As ProviderType,
[PROV].[PRPR_ID] AS [ProviderID],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_LAST_NAME] else PROV.PRPR_NAME end AS [ProviderLastName],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_FIRST_NAME] else null end AS [ProviderFirstName],
PROV.MCTN_ID As ProviderTaxID,
SUBSTRING([PROVADDR].[PRAD_ADDR1], 1, 55) AS [ProviderAddress1],
SUBSTRING([PROVADDR].[PRAD_ADDR2], 1, 55) AS [ProviderAddress2],
[PROVADDR].[PRAD_CITY] AS [ProviderAddressCity],
[PROVADDR].[PRAD_STATE] AS [ProviderAddressState],
substring([PROVADDR].[PRAD_ZIP],1,9) AS [ProviderAddressZip],
PROV.MCTN_ID as ProviderMedID,
PROV.PRPR_NPI as ProviderNPI,
[PROV].[PRCF_MCTR_SPEC] AS [ProviderSpecialtyCode],
[PROV].[PRPR_STS] AS [ProviderStatus],
[PROV].[PRPR_TAXONOMY_CD] AS [ProviderTaxonomy],
[PROVADDR].[PRAD_EFF_DT] AS [EffectiveDate],
[PROVADDR].[PRAD_TERM_DT] AS [TerminateDate],
[PROVADDR].[PRAD_PHONE] AS [ProviderTelephone]
From (SELECT CLCL_ID FROM FACETS.dbo.CMC_CLCL_CLAIM
WHERE CLCL_PAID_DT BETWEEN  '{1}' AND '{2}'
AND isnull(CLCL_ID,'') <> '' 
AND CLCL_CUR_STS IN('01','02','11','15','91') 
AND CLCL_CL_SUB_TYPE='{3}'
ORDER BY CLCL_ID
offset {4} rows fetch next 10000 rows only
) HDR
Inner Join FACETS.dbo.CMC_CLPE_PROV_DATA AD on AD.CLCL_ID=HDR.CLCL_ID and AD.CLPE_PRPR_TYPE='AD'
INNER JOIN FACETS.dbo.[CMC_PRPR_PROV] [PROV] ON AD.CLPE_PRPR_ID = [PROV].PRPR_ID
LEFT JOIN FACETS.dbo.[CMC_PRCP_COMM_PRAC] [PROV_PRAC] ON( [PROV].[PRCP_ID] = [PROV_PRAC].[PRCP_ID] )
LEFT JOIN FACETS.dbo.[CMC_PRAD_ADDRESS] [PROVADDR] ON( [PROV].[PRAD_ID] = [PROVADDR].[PRAD_ID]
	AND [PROV].[PRAD_TYPE_PRIM] = [PROVADDR].[PRAD_TYPE])
where ltrim(isnull(PROV.PRPR_ID,''))<>''

union      

select
HDR.CLCL_ID As ClaimID,
'Attending' As ProviderType,
[PROV].[PRPR_ID] AS [ProviderID],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_LAST_NAME] else PROV.PRPR_NAME end AS [ProviderLastName],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_FIRST_NAME] else null end AS [ProviderFirstName],
PROV.MCTN_ID As ProviderTaxID,
SUBSTRING([PROVADDR].[PRAD_ADDR1], 1, 55) AS [ProviderAddress1],
SUBSTRING([PROVADDR].[PRAD_ADDR2], 1, 55) AS [ProviderAddress2],
[PROVADDR].[PRAD_CITY] AS [ProviderAddressCity],
[PROVADDR].[PRAD_STATE] AS [ProviderAddressState],
substring([PROVADDR].[PRAD_ZIP],1,9) AS [ProviderAddressZip],
PROV.MCTN_ID as ProviderMedID,
PROV.PRPR_NPI as ProviderNPI,
[PROV].[PRCF_MCTR_SPEC] AS [ProviderSpecialtyCode],
[PROV].[PRPR_STS] AS [ProviderStatus],
[PROV].[PRPR_TAXONOMY_CD] AS [ProviderTaxonomy],
[PROVADDR].[PRAD_EFF_DT] AS [EffectiveDate],
[PROVADDR].[PRAD_TERM_DT] AS [TerminateDate],
[PROVADDR].[PRAD_PHONE] AS [ProviderTelephone]
From (SELECT CLCL_ID FROM FACETS.dbo.CMC_CLCL_CLAIM
WHERE CLCL_PAID_DT BETWEEN  '{1}' AND '{2}'
AND isnull(CLCL_ID,'') <> '' 
AND CLCL_CUR_STS IN('01','02','11','15','91') 
AND CLCL_CL_SUB_TYPE='{3}'
ORDER BY CLCL_ID
offset {4} rows fetch next 10000 rows only
) HDR
Inner Join FACETS.dbo.CMC_CLHP_HOSP HOSP on HOSP.CLCL_ID=HDR.CLCL_ID 
INNER JOIN FACETS.dbo.[CMC_PRPR_PROV] [PROV] ON HOSP.CLHP_PRPR_ID_ADM = [PROV].PRPR_ID
LEFT JOIN FACETS.dbo.[CMC_PRCP_COMM_PRAC] [PROV_PRAC] ON( [PROV].[PRCP_ID] = [PROV_PRAC].[PRCP_ID] )
LEFT JOIN FACETS.dbo.[CMC_PRAD_ADDRESS] [PROVADDR] ON( [PROV].[PRAD_ID] = [PROVADDR].[PRAD_ID]
	AND [PROV].[PRAD_TYPE_PRIM] = [PROVADDR].[PRAD_TYPE])
where ltrim(isnull(PROV.PRPR_ID,''))<>''

union      

select
HDR.CLCL_ID As ClaimID,
'Operating' As ProviderType,
[PROV].[PRPR_ID] AS [ProviderID],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_LAST_NAME] else PROV.PRPR_NAME end AS [ProviderLastName],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_FIRST_NAME] else null end AS [ProviderFirstName],
PROV.MCTN_ID As ProviderTaxID,
SUBSTRING([PROVADDR].[PRAD_ADDR1], 1, 55) AS [ProviderAddress1],
SUBSTRING([PROVADDR].[PRAD_ADDR2], 1, 55) AS [ProviderAddress2],
[PROVADDR].[PRAD_CITY] AS [ProviderAddressCity],
[PROVADDR].[PRAD_STATE] AS [ProviderAddressState],
substring([PROVADDR].[PRAD_ZIP],1,9) AS [ProviderAddressZip],
PROV.MCTN_ID as ProviderMedID,
PROV.PRPR_NPI as ProviderNPI,
[PROV].[PRCF_MCTR_SPEC] AS [ProviderSpecialtyCode],
[PROV].[PRPR_STS] AS [ProviderStatus],
[PROV].[PRPR_TAXONOMY_CD] AS [ProviderTaxonomy],
[PROVADDR].[PRAD_EFF_DT] AS [EffectiveDate],
[PROVADDR].[PRAD_TERM_DT] AS [TerminateDate],
[PROVADDR].[PRAD_PHONE] AS [ProviderTelephone]
From (SELECT CLCL_ID FROM FACETS.dbo.CMC_CLCL_CLAIM
WHERE CLCL_PAID_DT BETWEEN  '{1}' AND '{2}'
AND isnull(CLCL_ID,'') <> '' 
AND CLCL_CUR_STS IN('01','02','11','15','91') 
AND CLCL_CL_SUB_TYPE='{3}'
ORDER BY CLCL_ID
offset {4} rows fetch next 10000 rows only
) HDR
Inner Join FACETS.dbo.CMC_CLPE_PROV_DATA OP on OP.CLCL_ID=HDR.CLCL_ID and OP.CLPE_PRPR_TYPE='OP'
INNER JOIN FACETS.dbo.[CMC_PRPR_PROV] [PROV] ON OP.CLPE_PRPR_ID = [PROV].PRPR_ID
LEFT JOIN FACETS.dbo.[CMC_PRCP_COMM_PRAC] [PROV_PRAC] ON( [PROV].[PRCP_ID] = [PROV_PRAC].[PRCP_ID] )
LEFT JOIN FACETS.dbo.[CMC_PRAD_ADDRESS] [PROVADDR] ON( [PROV].[PRAD_ID] = [PROVADDR].[PRAD_ID]
	AND [PROV].[PRAD_TYPE_PRIM] = [PROVADDR].[PRAD_TYPE])
where ltrim(isnull(PROV.PRPR_ID,''))<>''

union      

select
HDR.CLCL_ID As ClaimID,
'Operating' As ProviderType,
[PROV].[PRPR_ID] AS [ProviderID],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_LAST_NAME] else PROV.PRPR_NAME end AS [ProviderLastName],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_FIRST_NAME] else null end AS [ProviderFirstName],
PROV.MCTN_ID As ProviderTaxID,
SUBSTRING([PROVADDR].[PRAD_ADDR1], 1, 55) AS [ProviderAddress1],
SUBSTRING([PROVADDR].[PRAD_ADDR2], 1, 55) AS [ProviderAddress2],
[PROVADDR].[PRAD_CITY] AS [ProviderAddressCity],
[PROVADDR].[PRAD_STATE] AS [ProviderAddressState],
substring([PROVADDR].[PRAD_ZIP],1,9) AS [ProviderAddressZip],
PROV.MCTN_ID as ProviderMedID,
PROV.PRPR_NPI as ProviderNPI,
[PROV].[PRCF_MCTR_SPEC] AS [ProviderSpecialtyCode],
[PROV].[PRPR_STS] AS [ProviderStatus],
[PROV].[PRPR_TAXONOMY_CD] AS [ProviderTaxonomy],
[PROVADDR].[PRAD_EFF_DT] AS [EffectiveDate],
[PROVADDR].[PRAD_TERM_DT] AS [TerminateDate],
[PROVADDR].[PRAD_PHONE] AS [ProviderTelephone]
From (SELECT CLCL_ID FROM FACETS.dbo.CMC_CLCL_CLAIM
WHERE CLCL_PAID_DT BETWEEN  '{1}' AND '{2}'
AND isnull(CLCL_ID,'') <> '' 
AND CLCL_CUR_STS IN('01','02','11','15','91') 
AND CLCL_CL_SUB_TYPE='{3}'
ORDER BY CLCL_ID
offset {4} rows fetch next 10000 rows only
) HDR
Inner Join FACETS.dbo.CMC_CLHP_HOSP HOSP On HOSP.CLCL_ID=HDR.CLCL_ID
INNER JOIN FACETS.dbo.[CMC_PRPR_PROV] [PROV] ON HOSP.CLHP_PRPR_ID_OPER = [PROV].PRPR_ID
LEFT JOIN FACETS.dbo.[CMC_PRCP_COMM_PRAC] [PROV_PRAC] ON( [PROV].[PRCP_ID] = [PROV_PRAC].[PRCP_ID] )
LEFT JOIN FACETS.dbo.[CMC_PRAD_ADDRESS] [PROVADDR] ON( [PROV].[PRAD_ID] = [PROVADDR].[PRAD_ID]
	AND [PROV].[PRAD_TYPE_PRIM] = [PROVADDR].[PRAD_TYPE])
where ltrim(isnull(PROV.PRPR_ID,''))<>''

union      

select
HDR.CLCL_ID As ClaimID,
'Other' As ProviderType,
[PROV].[PRPR_ID] AS [ProviderID],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_LAST_NAME] else PROV.PRPR_NAME end AS [ProviderLastName],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_FIRST_NAME] else null end AS [ProviderFirstName],
PROV.MCTN_ID As ProviderTaxID,
SUBSTRING([PROVADDR].[PRAD_ADDR1], 1, 55) AS [ProviderAddress1],
SUBSTRING([PROVADDR].[PRAD_ADDR2], 1, 55) AS [ProviderAddress2],
[PROVADDR].[PRAD_CITY] AS [ProviderAddressCity],
[PROVADDR].[PRAD_STATE] AS [ProviderAddressState],
substring([PROVADDR].[PRAD_ZIP],1,9) AS [ProviderAddressZip],
PROV.MCTN_ID as ProviderMedID,
PROV.PRPR_NPI as ProviderNPI,
[PROV].[PRCF_MCTR_SPEC] AS [ProviderSpecialtyCode],
[PROV].[PRPR_STS] AS [ProviderStatus],
[PROV].[PRPR_TAXONOMY_CD] AS [ProviderTaxonomy],
[PROVADDR].[PRAD_EFF_DT] AS [EffectiveDate],
[PROVADDR].[PRAD_TERM_DT] AS [TerminateDate],
[PROVADDR].[PRAD_PHONE] AS [ProviderTelephone]
From (SELECT CLCL_ID FROM FACETS.dbo.CMC_CLCL_CLAIM
WHERE CLCL_PAID_DT BETWEEN  '{1}' AND '{2}'
AND isnull(CLCL_ID,'') <> '' 
AND CLCL_CUR_STS IN('01','02','11','15','91') 
AND CLCL_CL_SUB_TYPE='{3}'
ORDER BY CLCL_ID
offset {4} rows fetch next 10000 rows only
) HDR
Inner Join FACETS.dbo.CMC_CLHP_HOSP HOSP On HOSP.CLCL_ID=HDR.CLCL_ID
INNER JOIN FACETS.dbo.[CMC_PRPR_PROV] [PROV] ON case when ltrim(isnull(HOSP.CLHP_PRPR_ID_OTH1,''))='' then HOSP.CLHP_PRPR_ID_OTH2 else HOSP.CLHP_PRPR_ID_OTH1 end = [PROV].PRPR_ID
LEFT JOIN FACETS.dbo.[CMC_PRCP_COMM_PRAC] [PROV_PRAC] ON( [PROV].[PRCP_ID] = [PROV_PRAC].[PRCP_ID] )
LEFT JOIN FACETS.dbo.[CMC_PRAD_ADDRESS] [PROVADDR] ON( [PROV].[PRAD_ID] = [PROVADDR].[PRAD_ID]
	AND [PROV].[PRAD_TYPE_PRIM] = [PROVADDR].[PRAD_TYPE])
where ltrim(isnull(PROV.PRPR_ID,''))<>''
";

		public static string FacetsLine = @"
Select 
    HDR.CLCL_ID As ClaimID
    , LINE.CDML_SEQ_NO As ClaimSeqNbr
    , LINE.PSCD_ID As POS
    ,SUBSTRING(LINE.IPCD_ID, 1, 5) As ProcCode
    , CASE WHEN ISNULL(LINE.CDML_CAP_IND,'') = '' THEN '1' WHEN LINE.CDML_CAP_IND= 'Y' THEN '1' ELSE '0' END  As IsCap
    , SUBSTRING(LINE.IPCD_ID, 6, 2) As Mod1
    , LINE.CDML_IPCD_MOD2 As Mod2
    , LINE.CDML_IPCD_MOD3 As Mod3
    , LINE.CDML_IPCD_MOD4 As Mod4
    , LINE.CDML_CHG_AMT As ChargeAmt
    , LINE.CDML_UNITS As Units
    , LINE.CDML_UNITS_ALLOW As UnitsAllowed
    , LINE.CDML_DED_AMT As DtlDedAmt
    , LINE.CDML_COPAY_AMT As DtlCopayAmt
    , LINE.CDML_COINS_AMT As DtlCoInsAmt
    , LINE.CDML_FROM_DT As DtlDosFrom
    , LINE.CDML_TO_DT As DtlDosTo
    , LINE.CDML_ALLOW As DtlAllowedAmt
    , LINE.CDML_PR_PYMT_AMT As DtlProvPayAmt
    , LINE.CDML_SB_PYMT_AMT As DtlMbrPayAmt
	, LINE.CDML_PR_PYMT_AMT + LINE.CDML_SB_PYMT_AMT As DtlTotalPaidAmt
	, LINE.CDML_PAID_AMT As DtlPaidAmt
	, CDCBCOB.CDCB_COB_ALLOW As COBDtlAllowedAmt
    , CDCBCOB.CDCB_COB_AMT As COBDtlPaidAmt
    , CDCBCOB.CDCB_COB_COINS_AMT As COBDtlCoInsAmt
    , CDCBCOB.CDCB_COB_COPAY_AMT As COBDtlCoPayAmt
    , CDCBCOB.CDCB_COB_DED_AMT As COBDtlDedAmt
    , LINE.RCRC_ID As RevCode
	, substring(LINE.IDCD_ID,1,8) As DtlDiagCode
	, '01' As DiagPointer1
	, LINE.CDML_CLMD_TYPE2 As DiagPointer2
    , LINE.CDML_CLMD_TYPE3 As DiagPointer3
    , LINE.CDML_CLMD_TYPE4 As DiagPointer4
	,CDSD.CDSD_NDC_CODE as DrugCode
	,cast(case when ltrim(isnull(CDSD.CDSD_NDC_UNITS,''))='' then '0' else ltrim(CDSD.CDSD_NDC_UNITS) end as decimal(15,2)) as DrugUnits
	,case when CDID.CDID_TYPE='NDC' and CDID.CDID_ADDL_DATA_1='PU1' then 'F2'
    when CDID.CDID_TYPE='NDC' and CDID.CDID_ADDL_DATA_1='PU2' then 'GR'
	when CDID.CDID_TYPE='NDC' and CDID.CDID_ADDL_DATA_1='PU3' then 'ME'
	when CDID.CDID_TYPE='NDC' and CDID.CDID_ADDL_DATA_1='PU4' then 'ML'
	when CDID.CDID_TYPE='NDC' and CDID.CDID_ADDL_DATA_1='PU5' then 'UN'
	when CDSD.CDSD_NDC_MCTR_TYPE='PU1' then 'F2'
	when CDSD.CDSD_NDC_MCTR_TYPE='PU2' then 'GR'
	when CDSD.CDSD_NDC_MCTR_TYPE='PU3' then 'ME'
	when CDSD.CDSD_NDC_MCTR_TYPE='PU4' then 'ML'
	when CDSD.CDSD_NDC_MCTR_TYPE='PU5' then 'UN'
	else 'UN' end as DrugUOM
	,LINE.CDML_CAP_IND As DtlCapInd 
	,HDR.CLCL_PAID_DT as PaidDate
	,LTRIM(RTRIM(CDML_DISALL_EXCD)) AS ExcludeCode1
	,Left(Ltrim(atuf.ATUF_TEXT1),1) as FamilyPlanIndicator
From FACETS.dbo.CMC_CDML_CL_LINE LINE
	Left Join FACETS.dbo.CMC_CDCB_LI_COB CDCBCOB On (LINE.CLCL_ID = CDCBCOB.CLCL_ID And LINE.CDML_SEQ_NO = CDCBCOB.CDML_SEQ_NO)
	Left Join FACETS.dbo.CMC_CDNP_NWX_PRCNG PRCNG On (LINE.CLCL_ID = PRCNG.CLCL_ID And LINE.CDML_SEQ_NO = PRCNG.CDNP_LINE_SEQ_NO And PRCNG.CDNP_RATE = 111.88)
	Left Join FACETS.dbo.CMC_EXCD_EXPL_CD COBEXPL On (CDCBCOB.CDCB_COB_REAS_CD = COBEXPL.EXCD_ID)
	Left Join FACETS.dbo.CMC_EXCD_EXPL_CD EXPL On (LINE.CDML_DISALL_EXCD = EXPL.EXCD_ID)
	Left Join FACETS.dbo.CMC_UMUM_UTIL_MGT UMUM On (LINE.CDML_UMAUTH_ID = UMUM.UMUM_REF_ID)
	Left Join FACETS.dbo.CMC_UMSV_SERVICES CMUM On ( LINE.CDML_UMAUTH_ID =  CMUM.UMUM_REF_ID )
	  and (LINE.CDML_FROM_DT BETWEEN CMUM.UMSV_FROM_DT AND CMUM.UMSV_TO_DT  OR LINE.CDML_TO_DT BETWEEN CMUM.UMSV_FROM_DT AND CMUM.UMSV_TO_DT  ) AND CMUM.IPCD_ID <> ' '
	Left Join FACETS.dbo.CMC_CDOR_LI_OVR CDOR On (LINE.CLCL_ID = CDOR.CLCL_ID And LINE.CDML_SEQ_NO = CDOR.CDML_SEQ_NO And CDOR.CDOR_OR_ID = 'XS')
	Left Join FACETS.dbo.CMC_CDOR_LI_OVR CDOR_NJ On (LINE.CLCL_ID = CDOR_NJ.CLCL_ID And LINE.CDML_SEQ_NO = CDOR_NJ.CDML_SEQ_NO And CDOR_NJ.CDOR_OR_ID = 'AN')
	Left Join FACETS.dbo.CMC_CDSD_SUPP_DATA CDSD On CDSD.CLCL_ID=LINE.CLCL_ID And CDSD.CDML_SEQ_NO=LINE.CDML_SEQ_NO
	Left Join FACETS.dbo.CMC_CDID_DATA CDID On CDID.CLCL_ID=LINE.CLCL_ID and CDID.CDML_SEQ_NO=LINE.CDML_SEQ_NO
	Left Join FACETS.dbo.CER_ATXR_ATTACH_U atxr on atxr.ATXR_SOURCE_ID=LINE.ATXR_SOURCE_ID and atxr.ATSY_ID='FPLI'
	Left Join FACETS.dbo.CER_ATUF_USERFLD_D atuf on atuf.ATXR_DEST_ID=atxr.ATXR_DEST_ID
	INNER JOIN (SELECT CLCL_ID,CLCL_PAID_DT FROM FACETS.dbo.CMC_CLCL_CLAIM
	WHERE CLCL_PAID_DT BETWEEN  '{1}' AND '{2}'
AND isnull(CLCL_ID,'') <> '' 
AND CLCL_CUR_STS IN('01','02','11','15','91') 
AND CLCL_CL_SUB_TYPE='{3}'
ORDER BY CLCL_ID
offset {4} rows fetch next 10000 rows only) HDR ON HDR.CLCL_ID=LINE.CLCL_ID
where ((LINE.CDML_PAID_AMT>0 and not ((CDML_CONSIDER_CHG>0 and CDML_ALLOW=0) or CDML_CONSIDER_CHG=CDML_DISALL_AMT))
Or (LINE.CDML_PAID_AMT=0 and case when ltrim(rtrim(isnull(CDOR.EXCD_ID,'')))<>'' then ltrim(rtrim(CDOR.EXCD_ID)) else ltrim(rtrim(isnull(LINE.CDML_DISALL_EXCD,''))) end in ('','H01','H02','J01','J02','KD3','KZ0','KZ1','KZ2','KZ3','KZ4','KZ5','L23','L49','L50','TF1'))
Or (LINE.CDML_PAID_AMT=0 and right(LINE.CLCL_ID,2)<>'00')
)
And LINE.CDML_CUR_STS<>'91'
";

		public static string FacetsCode = @"
SELECT DISTINCT [DIAG].[CLCL_ID] AS [ClaimID],
				CASE WHEN (TRY_CAST([DIAG].[CLMD_TYPE] AS INT)) = 1 THEN 'PD' WHEN [DIAG].[CLMD_TYPE] = 'AD' THEN 'ADMT'
				WHEN LEFT(LTRIM(DIAG.CLMD_TYPE),1)='R' THEN 'PR' ELSE 'DIAG' END AS [CodeType],
				CASE WHEN [DIAG].[CLMD_TYPE] = 'AD' THEN 1 
				WHEN LEFt(LTRIM(DIAG.CLMD_TYPE),1)='R' THEN TRY_CAST(SUBSTRING(LTRIM(DIAG.CLMD_TYPE),2,1) AS INT)
				ELSE TRY_CAST([DIAG].[CLMD_TYPE] AS INT) END AS [CodeSequence],
				'' AS [CodeLetter],
				CAST([DIAG].[IDCD_ID] AS VARCHAR(8)) AS [CodeValue],
				NULL AS [CodeAmount],
				NULL AS [CodeStartDate],
				NULL AS [CodeEndDate],
				case CMF.CLMF_ICD_QUAL_IND when '9' THEN 'ICD9' 
				WHEN '0' THEN 'ICD10' ELSE NULL END as ICDTypeCode,
				CASE WHEN DIAG.CLMD_POA_IND IN ('Y','N','W','U') THEN DIAG.CLMD_POA_IND
				     WHEN LTRIM(ISNULL(DIAG.CLMD_POA_IND,''))<>'' THEN 'U'
					 ELSE NULL END as DIAGPOI
FROM FACETS.dbo.[CMC_CLMD_DIAG] [DIAG]
	 INNER JOIN FACETS.dbo.[CMC_IDCD_DIAG_CD] [CDE] ON( [DIAG].[IDCD_ID] = [CDE].[IDCD_ID] )
	 INNER JOIN FACETS.dbo.[CMC_CLMF_MULT_FUNC] [CMF] ON( [DIAG].[CLCL_ID] = [CMF].[CLCL_ID] )
	 INNER JOIN (SELECT CLCL_ID, CLCL_LOW_SVC_DT FROM FACETS.dbo.[CMC_CLCL_CLAIM] 
WHERE CLCL_PAID_DT BETWEEN  '{1}' AND '{2}'
AND isnull(CLCL_ID,'') <> '' 
AND CLCL_CUR_STS IN('01','02','11','15','91') 
AND CLCL_CL_SUB_TYPE='{3}'
ORDER BY CLCL_ID
offset {4} rows fetch next 10000 rows only	 )  HDR ON HDR.CLCL_ID=DIAG.CLCL_ID
WHERE( [CDE].[IDCD_ACTION] <> 'D'
	OR ( [DIAG].[CLMD_TYPE] IN( 'A', '1', 'AD', '01')
	   )
	OR ( [CDE].[IDCD_ACTION] = 'D'
	 AND [HDR].[CLCL_LOW_SVC_DT] >= [CDE].[IDCD_EFF_DT]
	 AND [HDR].[CLCL_LOW_SVC_DT] <= [CDE].[IDCD_TERM_DT]
	   )
	 )
	 

UNION

SELECT DISTINCT [VAL].[CLCL_ID] AS [ClaimID],
				'VAL' AS [CodeType],
				ROW_NUMBER()  OVER (Partition  BY [VAL].[CLCL_ID] order by [VAL].[CLVC_LETTER]) AS [CodeSequence],
				[VAL].[CLVC_LETTER] AS [CodeLetter],
				CAST([VAL].[CLVC_CODE] AS VARCHAR(8)) AS [CodeValue],
				[VAL].[CLVC_AMT] AS [CodeAmount],
				NULL AS [CodeStartDate],
				NULL AS [CodeEndDate],
				case CMF.CLMF_ICD_QUAL_IND when '9' THEN 'ICD9' 
				WHEN '0' THEN 'ICD10' ELSE NULL END as ICDTypeCode,
				NULL as DiagPOI
FROM FACETS.dbo.[CMC_CLVC_VAL_CODE] [VAL]
	  INNER JOIN FACETS.dbo.[CMC_CLMF_MULT_FUNC] [CMF] ON( [VAL].[CLCL_ID] = [CMF].[CLCL_ID] )
	 INNER JOIN (SELECT CLCL_ID FROM FACETS.dbo.[CMC_CLCL_CLAIM] 
WHERE CLCL_PAID_DT BETWEEN  '{1}' AND '{2}'
AND isnull(CLCL_ID,'') <> '' 
AND CLCL_CUR_STS IN('01','02','11','15','91') 
AND CLCL_CL_SUB_TYPE='{3}'
ORDER BY CLCL_ID
offset {4} rows fetch next 10000 rows only	 )  HDR ON HDR.CLCL_ID=VAL.CLCL_ID
	  WHERE [VAL].[CLVC_AMT] <> 0 and [VAL].[CLVC_NUMBER] = 39
UNION

SELECT DISTINCT [COND].[CLCL_ID] [ClaimID],
				'COND' AS [CodeType],
				CAST([COND].[CLHC_SEQ_NO] AS VARCHAR(2)) AS [CodeSequence],
				'' AS [CodeLetter],
				CAST([COND].[CLHC_COND_CD] AS VARCHAR(8)) AS [CodeValue],
				NULL AS [CodeAmount],
				NULL AS [CodeStartDate],
				NULL AS [CodeEndDate],
				case CMF.CLMF_ICD_QUAL_IND when '9' THEN 'ICD9' 
				WHEN '0' THEN 'ICD10' ELSE NULL END as ICDTypeCode,
				NULL as DiagPOI
FROM FACETS.dbo.[CMC_CLHC_COND_CODE] [COND]
	  INNER JOIN FACETS.dbo.[CMC_CLMF_MULT_FUNC] [CMF] ON( [COND].[CLCL_ID] = [CMF].[CLCL_ID] )
	 INNER JOIN (SELECT CLCL_ID FROM FACETS.dbo.[CMC_CLCL_CLAIM] 
WHERE CLCL_PAID_DT BETWEEN  '{1}' AND '{2}'
AND isnull(CLCL_ID,'') <> '' 
AND CLCL_CUR_STS IN('01','02','11','15','91') 
AND CLCL_CL_SUB_TYPE='{3}'
ORDER BY CLCL_ID
offset {4} rows fetch next 10000 rows only	 )  HDR ON HDR.CLCL_ID=COND.CLCL_ID

UNION

SELECT DISTINCT HOSP.CLCL_ID as ClaimID,
'DRG' as CodeType,
'1' as CodeSequence,
'' as CodeLetter,
HOSP.CLHP_INPUT_AGRG_ID as CodeValue,
NULL as CodeAmount,
null as CodeStartDate,
null as CodeEndDate,
'ICD10' as ICDTypeCode,
null as DiagPOI
FROM FACETS.dbo.CMC_CLHP_HOSP [HOSP]
	 INNER JOIN (SELECT CLCL_ID FROM FACETS.dbo.[CMC_CLCL_CLAIM] 
WHERE CLCL_PAID_DT BETWEEN  '{1}' AND '{2}'
AND isnull(CLCL_ID,'') <> '' 
AND CLCL_CUR_STS IN('01','02','11','15','91') 
AND CLCL_CL_SUB_TYPE='{3}'
ORDER BY CLCL_ID
offset {4} rows fetch next 10000 rows only	 )  HDR ON HDR.CLCL_ID=HOSP.CLCL_ID
where ltrim(isnull(HOSP.CLHP_INPUT_AGRG_ID,''))<>''

UNION

SELECT DISTINCT [PROCCode].[CLCL_ID] [ClaimID],
				CASE WHEN PROCCode.CLHI_TYPE = 'P' THEN 'PP' ELSE 'OPI' END AS [CodeType]	,
				CAST(1 AS VARCHAR(2)) AS [CodeSequence],
				'' AS [CodeLetter],
				CAST([PROCCode].[IPCD_ID] AS VARCHAR(8)) AS [CodeValue],
				NULL AS [CodeAmount],
				[PROCCode].[CLHI_IP_DT] AS [CodeStartDate],
				[PROCCode].[CLHI_IP_DT] AS [CodeEndDate],
				case CMF.CLMF_ICD_QUAL_IND when '9' THEN 'ICD9' 
				WHEN '0' THEN 'ICD10' ELSE NULL END as ICDTypeCode,
				NULL as DiagPOI
FROM FACETS.dbo.[CMC_CLHI_PROC] [PROCCode]
	  INNER JOIN FACETS.dbo.[CMC_CLMF_MULT_FUNC] [CMF] ON( [PROCCode].[CLCL_ID] = [CMF].[CLCL_ID] )
	 INNER JOIN (SELECT CLCL_ID FROM FACETS.dbo.[CMC_CLCL_CLAIM] 
WHERE CLCL_PAID_DT BETWEEN  '{1}' AND '{2}'
AND isnull(CLCL_ID,'') <> '' 
AND CLCL_CUR_STS IN('01','02','11','15','91') 
AND CLCL_CL_SUB_TYPE='{3}'
ORDER BY CLCL_ID
offset {4} rows fetch next 10000 rows only	 )  HDR ON HDR.CLCL_ID=PROCCode.CLCL_ID

UNION

SELECT [OCCBH].[CLCL_ID] [ClaimID],
	   'OCC_BH' AS [CodeType],
	   CAST([OCCBH].[CLHO_SEQ_NO] AS VARCHAR(2)) AS [CodeSequence],
	   '' AS [CodeLetter],
	   CAST([OCCBH].[CLHO_OCC_CODE] AS VARCHAR(8)) AS [CodeValue],
	   NULL AS [CodeAmount],
	   [OCCBH].[CLHO_OCC_FROM_DT] AS [CodeStartDate],
	   [OCCBH].[CLHO_OCC_TO_DT] AS [CodeEndDate],
				case CMF.CLMF_ICD_QUAL_IND when '9' THEN 'ICD9' 
				WHEN '0' THEN 'ICD10' ELSE NULL END as ICDTypeCode,
				NULL as DiagPOI
FROM FACETS.dbo.[CMC_CLHO_OCC_CODE] [OCCBH]
	  INNER JOIN FACETS.dbo.[CMC_CLMF_MULT_FUNC] [CMF] ON( [OCCBH].[CLCL_ID] = [CMF].[CLCL_ID] )
	 INNER JOIN (SELECT CLCL_ID FROM FACETS.dbo.[CMC_CLCL_CLAIM] 
WHERE CLCL_PAID_DT BETWEEN  '{1}' AND '{2}'
AND isnull(CLCL_ID,'') <> '' 
AND CLCL_CUR_STS IN('01','02','11','15','91') 
AND CLCL_CL_SUB_TYPE='{3}'
ORDER BY CLCL_ID
offset {4} rows fetch next 10000 rows only	 )  HDR ON HDR.CLCL_ID=OCCBH.CLCL_ID
WHERE CONVERT(VARCHAR(10), [CLHO_OCC_FROM_DT], 101) <> '01/01/1753'
  AND CONVERT(VARCHAR(10), [OCCBH].[CLHO_OCC_TO_DT], 101) = '01/01/1753'

UNION

SELECT 
[OCCBI].[CLCL_ID] [ClaimID],
'OCC_BI' AS [CodeType],
CAST([OCCBI].[CLHO_SEQ_NO] AS VARCHAR(2)) AS [CodeSequence],
'' AS [CodeLetter],
CAST([OCCBI].[CLHO_OCC_CODE] AS VARCHAR(8)) AS [CodeValue],
NULL AS [CodeAmount],
[OCCBI].[CLHO_OCC_FROM_DT] AS [CodeStartDate],
[OCCBI].[CLHO_OCC_TO_DT] AS [CodeEndDate],
				case CMF.CLMF_ICD_QUAL_IND when '9' THEN 'ICD9' 
				WHEN '0' THEN 'ICD10' ELSE NULL END as ICDTypeCode,
				NULL as DiagPOI
FROM FACETS.dbo.[CMC_CLHO_OCC_CODE] [OCCBI]
	  INNER JOIN FACETS.dbo.[CMC_CLMF_MULT_FUNC] [CMF] ON( [OCCBI].[CLCL_ID] = [CMF].[CLCL_ID] )
	 INNER JOIN (SELECT CLCL_ID FROM FACETS.dbo.[CMC_CLCL_CLAIM] 
WHERE CLCL_PAID_DT BETWEEN  '{1}' AND '{2}'
AND isnull(CLCL_ID,'') <> '' 
AND CLCL_CUR_STS IN('01','02','11','15','91') 
AND CLCL_CL_SUB_TYPE='{3}'
ORDER BY CLCL_ID
offset {4} rows fetch next 10000 rows only	 )  HDR ON HDR.CLCL_ID=OCCBI.CLCL_ID
WHERE CONVERT(VARCHAR(10), [CLHO_OCC_FROM_DT], 101) <> '01/01/1753'
  AND CONVERT(VARCHAR(10), [CLHO_OCC_TO_DT], 101) <> '01/01/1753'
";

		public static string FacetsExtraSvd = @"
SELECT DISTINCT [cdml].[CLCL_ID] AS [ClaimID],
				[cdml].[CDML_SEQ_NO] [ClaimLineSeq],
				substring(ltrim([clcl].[MEME_CK]),1,15) AS [MemberID],
				'P' AS [COBPayerType],
				right(rtrim(MCREENT.MCRE_ID),3) AS [COBPayerID],
				MCREENT.MCRE_NAME AS [COBPayerName],
				[CDCBCOB].[CDCB_COB_ALLOW] AS [COBDtlAllowedAmt],
				[CDCBCOB].[CDCB_COB_COINS_AMT] AS [COBDtlCoInsAmt],
				[CDCBCOB].[CDCB_COB_COPAY_AMT] AS [COBDtlCoPayAmt],
				[CDCBCOB].[CDCB_COB_DED_AMT] AS [COBDtlDedAmt],
				[CDCBCOB].[CDCB_COB_AMT] AS [COBDtlPaidAmt],
				[CDCBCOB].[CDCB_COB_DISALLOW] AS [COBDtlDisAlwAmt],
				0 as IsHealthPlan
FROM (SELECT CLCL_ID,MEME_CK FROM FACETS.dbo.CMC_CLCL_CLAIM
WHERE CLCL_PAID_DT BETWEEN  '{1}' AND '{2}'
AND isnull(CLCL_ID,'') <> '' 
AND CLCL_CUR_STS IN('01','02','11','15','91') 
AND CLCL_CL_SUB_TYPE='{3}'
ORDER BY CLCL_ID
offset {4} rows fetch next 10000 rows only
) [clcl]
INNER JOIN FACETS.dbo.[CMC_CDML_CL_LINE] [cdml] ON( [clcl].[CLCL_ID] = [cdml].[CLCL_ID] )
INNER JOIN FACETS.dbo.[CMC_CLCB_CL_COB] [CLCBCOB] ON( [clcl].[MEME_CK] = [CLCBCOB].[MEME_CK]
									AND [clcl].[CLCL_ID] = [CLCBCOB].[CLCL_ID]
										)
INNER JOIN (select MEME_CK,MCRE_ID,MECB_POLICY_ID,row_number() over(partition by MEME_CK order by MECB_INSUR_ORDER) as RN from FACETS.dbo.[CMC_MECB_COB]) [MECBCOB] ON [clcl].[MEME_CK] = [MECBCOB].[MEME_CK] and MECBCOB.RN=1
INNER JOIN FACETS.dbo.[CMC_MCRE_RELAT_ENT] [MCREENT] ON [MECBCOB].[MCRE_ID] = [MCREENT].[MCRE_ID]
INNER JOIN FACETS.dbo.[CMC_CDCB_LI_COB] [CDCBCOB] ON( [cdml].[CLCL_ID] = [CDCBCOB].[CLCL_ID]
										   AND [cdml].[CDML_SEQ_NO] = [CDCBCOB].[CDML_SEQ_NO]
											 )
WHERE [clcl].CLCL_ID <> '' and ltrim(MCREENT.MCRE_ID)<>'';
";

		public static string FacetsExtraSbr = @"
SELECT DISTINCT [clcl].[CLCL_ID] AS [ClaimID],
				[clcl].[MEME_CK] AS [MemberID],
				right(rtrim(MCREENT.MCRE_ID),3) AS [COBPayerID],
				MCREENT.MCRE_NAME AS [PayerFullName]
, MCREENT.MCRE_NAME AS PayerLastName
, NULL AS PayerFirstName
, NULL AS PayerMiddleInitial
, substring(ltrim(MCREENT.MCRE_ADDR1),1,35) AS PayerAddress1
, NULL AS PayerAddress2
, MCREENT.MCRE_CITY AS PayerAddressCity
, MCREENT.MCRE_STATE AS PayerState
, LEFT(MCREENT.MCRE_ZIP, 10) AS PayerZip
, NULL AS PayerCounty
, NULL AS PayerCountryCode
, 0 AS IsHealthPlan
, CASE WHEN LEFT(LTRIM(RTRIM(MECB_POLICY_ID)), 3) = '600' THEN 'MA' 
	WHEN LEFT(LTRIM(RTRIM(MECB_POLICY_ID)), 3) = '100' THEN 'MB'
	WHEN LEFT(LTRIM(RTRIM(MECB_POLICY_ID)), 3) = '103' THEN 'OF'
	WHEN LEFT(LTRIM(RTRIM(MECB_POLICY_ID)), 3) = '511' THEN '16'
	ELSE 'CI' END AS ClaimFilingIndicatorCode
,'P' AS COBPayerType
FROM (SELECT CLCL_ID,MEME_CK FROM FACETS.dbo.CMC_CLCL_CLAIM
WHERE CLCL_PAID_DT BETWEEN  '{1}' AND '{2}'
AND isnull(CLCL_ID,'') <> '' 
AND CLCL_CUR_STS IN('01','02','11','15','91') 
AND CLCL_CL_SUB_TYPE='{3}'
ORDER BY CLCL_ID
offset {4} rows fetch next 10000 rows only
) [clcl]
INNER JOIN FACETS.dbo.[CMC_CLCB_CL_COB] [CLCBCOB] ON( [clcl].[MEME_CK] = [CLCBCOB].[MEME_CK]
											AND [clcl].[CLCL_ID] = [CLCBCOB].[CLCL_ID]
											  )
INNER JOIN (select MEME_CK,MCRE_ID,MECB_POLICY_ID,row_number() over(partition by MEME_CK order by MECB_INSUR_ORDER) as RN from FACETS.dbo.[CMC_MECB_COB]) [MECBCOB] ON [clcl].[MEME_CK] = [MECBCOB].[MEME_CK] and MECBCOB.RN=1
INNER JOIN FACETS.dbo.[CMC_MCRE_RELAT_ENT] [MCREENT] ON [MECBCOB].[MCRE_ID] = [MCREENT].[MCRE_ID]
WHERE ISNULL(MECB_POLICY_ID, '') <> '';
";
		public static string FacetsHeaderSingle = @"
Select 
	HDR.CLCL_ID As ClaimID
	, substring(HDR.CSPI_ID,1,5) As PlanID
	, CASE HDR.CLCL_CL_SUB_TYPE WHEN 'H' THEN 'I'
	  WHEN 'M' THEN 'P'
	  END As ClaimType
	, CASE WHEN ISNULL(HDR.CLCL_CAP_IND,'') = '' THEN '1' WHEN HDR.CLCL_CAP_IND= 'F' THEN '1' ELSE '0' END  As IsCapitated
	, HDR.CLCL_TOT_CHG As HeaderChargeAmt
	, HDR.CLCL_TOT_PAYABLE As HeaderPaidAmt
	, HDR.CLCL_PA_PAID_AMT As HeaderPatPaidAmt
	, HDR.CLCL_REL_INFO_IND As RelInfo
	, HDR.CLCL_ACD_STATE As ACDState
	, HDR.CLCL_OTHER_BN_IND As BenAssigned
	, HDR.CLCL_PAID_DT As PaidDate
	, HDR.CLCL_LOW_SVC_DT As DOSFromDate
	, HDR.CLCL_HIGH_SVC_DT As DOSToDate
	, HDR.CLCL_ID_ADJ_TO As ClaimIdAdjTo
	, HDR.CLCL_ID_ADJ_FROM As ClaimIdAdjFrom
	, HDR.CLCL_RECD_DT As ReceivedDate
	, cast(HDR.MEME_CK as varchar(15)) As MemberCK
	, substring(HOSP.CLHP_FAC_TYPE,patindex('%[^0]%',HOSP.CLHP_FAC_TYPE),1) As FacType
	, HOSP.CLHP_BILL_CLASS As BillClass
	, HOSP.CLHP_FREQUENCY As Frequency
	, HOSP.CLHP_DC_HOUR_CD As DisChgHr
	, HOSP.CLHP_STAMENT_FR_DT As StatFromDte
	, HOSP.CLHP_STAMENT_TO_DT As StatToDte
	, HOSP.CLHP_ADM_DT As AdmitDte
	, HOSP.CLHP_ADM_HOUR_CD As AdmitHr
	, substring(HOSP.CLHP_ADM_TYP,1,1) As AdmitType
	, HOSP.CLHP_ADM_SOURCE As AdmitSource
	, HOSP.CLHP_DC_STAT As DisChgSts
	, HOSP.CLHP_MED_REC_NO As MedRecNo
	, HOSP.CLHP_COVD_DAYS As CoveredDays
	, HDR.CLCL_SIMI_ILL_DT As SimilarIllnessDate
	, HOSP.CLHP_INPUT_AGRG_ID As DRGInfo 
	, HOSP.CLHP_DC_DT As DischargeDte
	, HDR.CLCL_INPUT_METH As InputMethod
    , SUBSC.SBSB_ID+Right('00'+cast(MEME_SFX as varchar(10)),2) As MemberID
    , MBR.MEME_SEX As MemberSex
    , MBR.MEME_BIRTH_DT As MemberBirthDate
	, MBR.MEME_SSN As MemberSSN
	, upper(MBR.MEME_LAST_NAME) As MemberLastName
	, upper(MBR.MEME_FIRST_NAME) As MemberFirstName 
	, MBR.MEME_MID_INIT As MemberMiddleInit
	, SBADADDR.SBAD_ADDR1 as MemberAddress1
	, SBADADDR.SBAD_ADDR2 As MemberAddress2
	, SBADADDR.SBAD_CITY As MemberAddressCity
	, SBADADDR.SBAD_STATE As MemberAddressState
	, left(SBADADDR.SBAD_ZIP,9) As MemberAddressZip
	,MBR.MEME_HICN as MedicareId
	,substring(MBR.MEME_MEDCD_NO,1,10) as  MedicaidId
	,substring(HDR.CSPI_ID,1,9) COBPayerId
	 From FACETS.dbo.CMC_CLCL_CLAIM HDR
	Left Join FACETS.dbo.CMC_CLHP_HOSP HOSP On (HDR.CLCL_ID = HOSP.CLCL_ID)
	Left Join FACETS.dbo.CMC_SGSG_SUB_GROUP SGSG On (HDR.SGSG_CK = SGSG.SGSG_CK)
	Left Join FACETS.dbo.CMC_CLED_EDI_DATA CLED On (HDR.CLCL_ID = CLED.CLCL_ID)
	Left Join FACETS.dbo.CMC_CLCK_CLM_CHECK FCLMCHK On (HDR.CLCL_ID =  FCLMCHK.CLCL_ID)
	Left Join FACETS.dbo.CMC_CDML_CL_LINE LINE On HDR.CLCL_ID=LINE.CLCL_ID
INNER JOIN  FACETS.dbo.CMC_MEME_MEMBER MBR On (HDR.MEME_CK = MBR.MEME_CK)
INNER JOIN FACETS.dbo.CMC_SBSB_SUBSC SUBSC On (SUBSC.SBSB_CK = MBR.SBSB_CK)
Left Join FACETS.dbo.CMC_SBAD_ADDR SBADADDR On (SUBSC.SBSB_CK = SBADADDR.SBSB_CK And SUBSC.SBAD_TYPE_HOME = SBADADDR.SBAD_TYPE)
WHERE HDR.CLCL_ID='{0}'";

		public static string FacetsProviderSingle = @"
SELECT      
HDR.CLCL_ID As ClaimID,
'Billing' As ProviderType,
[PROV].[PRPR_ID] AS [ProviderID],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_LAST_NAME] else PROV.PRPR_NAME end AS [ProviderLastName],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_FIRST_NAME] else null end AS [ProviderFirstName],
PROV.MCTN_ID As ProviderTaxID,
SUBSTRING([PROVADDR].[PRAD_ADDR1], 1, 55) AS [ProviderAddress1],
SUBSTRING([PROVADDR].[PRAD_ADDR2], 1, 55) AS [ProviderAddress2],
[PROVADDR].[PRAD_CITY] AS [ProviderAddressCity],
[PROVADDR].[PRAD_STATE] AS [ProviderAddressState],
substring([PROVADDR].[PRAD_ZIP],1,9) AS [ProviderAddressZip],
PROV.MCTN_ID as ProviderMedID,
PROV.PRPR_NPI as ProviderNPI,
[PROV].[PRCF_MCTR_SPEC] AS [ProviderSpecialtyCode],
[PROV].[PRPR_STS] AS [ProviderStatus],
[PROV].[PRPR_TAXONOMY_CD] AS [ProviderTaxonomy],
[PROVADDR].[PRAD_EFF_DT] AS [EffectiveDate],
[PROVADDR].[PRAD_TERM_DT] AS [TerminateDate],
[PROVADDR].[PRAD_PHONE] AS [ProviderTelephone]
From FACETS.dbo.CMC_CLCL_CLAIM HDR
INNER JOIN FACETS.dbo.[CMC_PRPR_PROV] [PROV] ON HDR.CLCL_PAYEE_PR_ID = [PROV].PRPR_ID
LEFT JOIN FACETS.dbo.[CMC_PRCP_COMM_PRAC] [PROV_PRAC] ON( [PROV].[PRCP_ID] = [PROV_PRAC].[PRCP_ID] )
LEFT JOIN FACETS.dbo.[CMC_PRAD_ADDRESS] [PROVADDR] ON( [PROV].[PRAD_ID] = [PROVADDR].[PRAD_ID]
	AND [PROV].[PRAD_TYPE_PRIM] = [PROVADDR].[PRAD_TYPE])
where ltrim(isnull(PROV.PRPR_ID,''))<>''
AND HDR.CLCL_ID='{0}'

union      

select
HDR.CLCL_ID As ClaimID,
'Rendering' As ProviderType,
[PROV].[PRPR_ID] AS [ProviderID],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_LAST_NAME] else PROV.PRPR_NAME end AS [ProviderLastName],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_FIRST_NAME] else null end AS [ProviderFirstName],
PROV.MCTN_ID As ProviderTaxID,
SUBSTRING([PROVADDR].[PRAD_ADDR1], 1, 55) AS [ProviderAddress1],
SUBSTRING([PROVADDR].[PRAD_ADDR2], 1, 55) AS [ProviderAddress2],
[PROVADDR].[PRAD_CITY] AS [ProviderAddressCity],
[PROVADDR].[PRAD_STATE] AS [ProviderAddressState],
substring([PROVADDR].[PRAD_ZIP],1,9) AS [ProviderAddressZip],
PROV.MCTN_ID as ProviderMedID,
PROV.PRPR_NPI as ProviderNPI,
[PROV].[PRCF_MCTR_SPEC] AS [ProviderSpecialtyCode],
[PROV].[PRPR_STS] AS [ProviderStatus],
[PROV].[PRPR_TAXONOMY_CD] AS [ProviderTaxonomy],
[PROVADDR].[PRAD_EFF_DT] AS [EffectiveDate],
[PROVADDR].[PRAD_TERM_DT] AS [TerminateDate],
[PROVADDR].[PRAD_PHONE] AS [ProviderTelephone]
From FACETS.dbo.CMC_CLCL_CLAIM HDR
Inner Join FACETS.dbo.CMC_CLPE_PROV_DATA RE on RE.CLCL_ID=HDR.CLCL_ID and RE.CLPE_PRPR_TYPE='RE'
INNER JOIN FACETS.dbo.[CMC_PRPR_PROV] [PROV] ON case when ltrim(isnull(RE.CLPE_PRPR_ID,''))='' then HDR.PRPR_ID else RE.CLPE_PRPR_ID end = [PROV].PRPR_ID
LEFT JOIN FACETS.dbo.[CMC_PRCP_COMM_PRAC] [PROV_PRAC] ON( [PROV].[PRCP_ID] = [PROV_PRAC].[PRCP_ID] )
LEFT JOIN FACETS.dbo.[CMC_PRAD_ADDRESS] [PROVADDR] ON( [PROV].[PRAD_ID] = [PROVADDR].[PRAD_ID]
	AND [PROV].[PRAD_TYPE_PRIM] = [PROVADDR].[PRAD_TYPE])
where ltrim(isnull(PROV.PRPR_ID,''))<>''
AND HDR.CLCL_ID='{0}'

union

Select HDR.CLCL_ID As ClaimID, 
'Referring' As ProviderType,
[PROV].[PRPR_ID] AS [ProviderID],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_LAST_NAME] else PROV.PRPR_NAME end AS [ProviderLastName],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_FIRST_NAME] else null end AS [ProviderFirstName],
PROV.MCTN_ID As ProviderTaxID,
SUBSTRING([PROVADDR].[PRAD_ADDR1], 1, 55) AS [ProviderAddress1],
SUBSTRING([PROVADDR].[PRAD_ADDR2], 1, 55) AS [ProviderAddress2],
[PROVADDR].[PRAD_CITY] AS [ProviderAddressCity],
[PROVADDR].[PRAD_STATE] AS [ProviderAddressState],
substring([PROVADDR].[PRAD_ZIP],1,9) AS [ProviderAddressZip],
PROV.MCTN_ID as ProviderMedID,
PROV.PRPR_NPI as ProviderNPI,
[PROV].[PRCF_MCTR_SPEC] AS [ProviderSpecialtyCode],
[PROV].[PRPR_STS] AS [ProviderStatus],
[PROV].[PRPR_TAXONOMY_CD] AS [ProviderTaxonomy],
[PROVADDR].[PRAD_EFF_DT] AS [EffectiveDate],
[PROVADDR].[PRAD_TERM_DT] AS [TerminateDate],
[PROVADDR].[PRAD_PHONE] AS [ProviderTelephone]
From FACETS.dbo.CMC_CLCL_CLAIM HDR
Inner Join FACETS.dbo.CMC_CLPE_PROV_DATA RF on RF.CLCL_ID=HDR.CLCL_ID and RF.CLPE_PRPR_TYPE='RF'
INNER JOIN FACETS.dbo.[CMC_PRPR_PROV] [PROV] ON RF.CLPE_PRPR_ID = [PROV].PRPR_ID
LEFT JOIN FACETS.dbo.[CMC_PRCP_COMM_PRAC] [PROV_PRAC] ON( [PROV].[PRCP_ID] = [PROV_PRAC].[PRCP_ID] )
LEFT JOIN FACETS.dbo.[CMC_PRAD_ADDRESS] [PROVADDR] ON( [PROV].[PRAD_ID] = [PROVADDR].[PRAD_ID]
	AND [PROV].[PRAD_TYPE_PRIM] = [PROVADDR].[PRAD_TYPE])
where ltrim(isnull(PROV.PRPR_ID,''))<>''
AND HDR.CLCL_ID='{0}'

union

Select HDR.CLCL_ID As ClaimID,
'Referring' As ProviderType,
[PROV].[PRPR_ID] AS [ProviderID],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_LAST_NAME] else PROV.PRPR_NAME end AS [ProviderLastName],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_FIRST_NAME] else null end AS [ProviderFirstName],
PROV.MCTN_ID As ProviderTaxID,
SUBSTRING([PROVADDR].[PRAD_ADDR1], 1, 55) AS [ProviderAddress1],
SUBSTRING([PROVADDR].[PRAD_ADDR2], 1, 55) AS [ProviderAddress2],
[PROVADDR].[PRAD_CITY] AS [ProviderAddressCity],
[PROVADDR].[PRAD_STATE] AS [ProviderAddressState],
substring([PROVADDR].[PRAD_ZIP],1,9) AS [ProviderAddressZip],
PROV.MCTN_ID as ProviderMedID,
PROV.PRPR_NPI as ProviderNPI,
[PROV].[PRCF_MCTR_SPEC] AS [ProviderSpecialtyCode],
[PROV].[PRPR_STS] AS [ProviderStatus],
[PROV].[PRPR_TAXONOMY_CD] AS [ProviderTaxonomy],
[PROVADDR].[PRAD_EFF_DT] AS [EffectiveDate],
[PROVADDR].[PRAD_TERM_DT] AS [TerminateDate],
[PROVADDR].[PRAD_PHONE] AS [ProviderTelephone]
From FACETS.dbo.CMC_CLCL_CLAIM HDR 
INNER JOIN FACETS.dbo.[CMC_PRPR_PROV] [PROV] ON case when ltrim(isnull(HDR.CLCL_PRPR_ID_REF,''))='' then HDR.CLCL_PRPR_ID_PCP else HDR.CLCL_PRPR_ID_REF end = [PROV].PRPR_ID
LEFT JOIN FACETS.dbo.[CMC_PRCP_COMM_PRAC] [PROV_PRAC] ON( [PROV].[PRCP_ID] = [PROV_PRAC].[PRCP_ID] )
LEFT JOIN FACETS.dbo.[CMC_PRAD_ADDRESS] [PROVADDR] ON( [PROV].[PRAD_ID] = [PROVADDR].[PRAD_ID]
	AND [PROV].[PRAD_TYPE_PRIM] = [PROVADDR].[PRAD_TYPE])
where ltrim(isnull(PROV.PRPR_ID,''))<>''
AND HDR.CLCL_ID='{0}'

union      

select  Distinct
HDR.CLCL_ID As ClaimID,
'ServiceFacility' As ProviderType,
[PROV].[PRPR_ID] AS [ProviderID],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_LAST_NAME] else PROV.PRPR_NAME end AS [ProviderLastName],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_FIRST_NAME] else null end AS [ProviderFirstName],
PROV.MCTN_ID As ProviderTaxID,
SUBSTRING([PROVADDR].[PRAD_ADDR1], 1, 55) AS [ProviderAddress1],
SUBSTRING([PROVADDR].[PRAD_ADDR2], 1, 55) AS [ProviderAddress2],
[PROVADDR].[PRAD_CITY] AS [ProviderAddressCity],
[PROVADDR].[PRAD_STATE] AS [ProviderAddressState],
substring([PROVADDR].[PRAD_ZIP],1,9) AS [ProviderAddressZip],
PROV.MCTN_ID as ProviderMedID,
PROV.PRPR_NPI as ProviderNPI,
[PROV].[PRCF_MCTR_SPEC] AS [ProviderSpecialtyCode],
[PROV].[PRPR_STS] AS [ProviderStatus],
[PROV].[PRPR_TAXONOMY_CD] AS [ProviderTaxonomy],
[PROVADDR].[PRAD_EFF_DT] AS [EffectiveDate],
[PROVADDR].[PRAD_TERM_DT] AS [TerminateDate],
[PROVADDR].[PRAD_PHONE] AS [ProviderTelephone]
From FROM FACETS.dbo.CMC_CLCL_CLAIM HDR
Inner Join FACETS.dbo.CMC_CLPE_PROV_DATA FA on FA.CLCL_ID=HDR.CLCL_ID and FA.CLPE_PRPR_TYPE='FA'
INNER JOIN FACETS.dbo.[CMC_PRPR_PROV] [PROV] ON FA.CLPE_NPI = [PROV].PRPR_NPI and PROV.PRPR_ENTITY='F'
LEFT JOIN FACETS.dbo.[CMC_PRCP_COMM_PRAC] [PROV_PRAC] ON( [PROV].[PRCP_ID] = [PROV_PRAC].[PRCP_ID] )
LEFT JOIN FACETS.dbo.[CMC_PRAD_ADDRESS] [PROVADDR] ON( [PROV].[PRAD_ID] = [PROVADDR].[PRAD_ID]
	AND [PROV].[PRAD_TYPE_PRIM] = [PROVADDR].[PRAD_TYPE])
where ltrim(isnull(PROV.PRPR_NPI,''))<>''
AND HDR.CLCL_ID='{0}'

union      

select
HDR.CLCL_ID As ClaimID,
'Attending' As ProviderType,
[PROV].[PRPR_ID] AS [ProviderID],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_LAST_NAME] else PROV.PRPR_NAME end AS [ProviderLastName],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_FIRST_NAME] else null end AS [ProviderFirstName],
PROV.MCTN_ID As ProviderTaxID,
SUBSTRING([PROVADDR].[PRAD_ADDR1], 1, 55) AS [ProviderAddress1],
SUBSTRING([PROVADDR].[PRAD_ADDR2], 1, 55) AS [ProviderAddress2],
[PROVADDR].[PRAD_CITY] AS [ProviderAddressCity],
[PROVADDR].[PRAD_STATE] AS [ProviderAddressState],
substring([PROVADDR].[PRAD_ZIP],1,9) AS [ProviderAddressZip],
PROV.MCTN_ID as ProviderMedID,
PROV.PRPR_NPI as ProviderNPI,
[PROV].[PRCF_MCTR_SPEC] AS [ProviderSpecialtyCode],
[PROV].[PRPR_STS] AS [ProviderStatus],
[PROV].[PRPR_TAXONOMY_CD] AS [ProviderTaxonomy],
[PROVADDR].[PRAD_EFF_DT] AS [EffectiveDate],
[PROVADDR].[PRAD_TERM_DT] AS [TerminateDate],
[PROVADDR].[PRAD_PHONE] AS [ProviderTelephone]
From FACETS.dbo.CMC_CLCL_CLAIM HDR
Inner Join FACETS.dbo.CMC_CLPE_PROV_DATA AD on AD.CLCL_ID=HDR.CLCL_ID and AD.CLPE_PRPR_TYPE='AD'
INNER JOIN FACETS.dbo.[CMC_PRPR_PROV] [PROV] ON AD.CLPE_PRPR_ID = [PROV].PRPR_ID
LEFT JOIN FACETS.dbo.[CMC_PRCP_COMM_PRAC] [PROV_PRAC] ON( [PROV].[PRCP_ID] = [PROV_PRAC].[PRCP_ID] )
LEFT JOIN FACETS.dbo.[CMC_PRAD_ADDRESS] [PROVADDR] ON( [PROV].[PRAD_ID] = [PROVADDR].[PRAD_ID]
	AND [PROV].[PRAD_TYPE_PRIM] = [PROVADDR].[PRAD_TYPE])
where ltrim(isnull(PROV.PRPR_ID,''))<>''
AND HDR.CLCL_ID='{0}'

union      

select
HDR.CLCL_ID As ClaimID,
'Attending' As ProviderType,
[PROV].[PRPR_ID] AS [ProviderID],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_LAST_NAME] else PROV.PRPR_NAME end AS [ProviderLastName],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_FIRST_NAME] else null end AS [ProviderFirstName],
PROV.MCTN_ID As ProviderTaxID,
SUBSTRING([PROVADDR].[PRAD_ADDR1], 1, 55) AS [ProviderAddress1],
SUBSTRING([PROVADDR].[PRAD_ADDR2], 1, 55) AS [ProviderAddress2],
[PROVADDR].[PRAD_CITY] AS [ProviderAddressCity],
[PROVADDR].[PRAD_STATE] AS [ProviderAddressState],
substring([PROVADDR].[PRAD_ZIP],1,9) AS [ProviderAddressZip],
PROV.MCTN_ID as ProviderMedID,
PROV.PRPR_NPI as ProviderNPI,
[PROV].[PRCF_MCTR_SPEC] AS [ProviderSpecialtyCode],
[PROV].[PRPR_STS] AS [ProviderStatus],
[PROV].[PRPR_TAXONOMY_CD] AS [ProviderTaxonomy],
[PROVADDR].[PRAD_EFF_DT] AS [EffectiveDate],
[PROVADDR].[PRAD_TERM_DT] AS [TerminateDate],
[PROVADDR].[PRAD_PHONE] AS [ProviderTelephone]
From FACETS.dbo.CMC_CLCL_CLAIM HDR
Inner Join FACETS.dbo.CMC_CLHP_HOSP HOSP on HOSP.CLCL_ID=HDR.CLCL_ID 
INNER JOIN FACETS.dbo.[CMC_PRPR_PROV] [PROV] ON HOSP.CLHP_PRPR_ID_ADM = [PROV].PRPR_ID
LEFT JOIN FACETS.dbo.[CMC_PRCP_COMM_PRAC] [PROV_PRAC] ON( [PROV].[PRCP_ID] = [PROV_PRAC].[PRCP_ID] )
LEFT JOIN FACETS.dbo.[CMC_PRAD_ADDRESS] [PROVADDR] ON( [PROV].[PRAD_ID] = [PROVADDR].[PRAD_ID]
	AND [PROV].[PRAD_TYPE_PRIM] = [PROVADDR].[PRAD_TYPE])
where ltrim(isnull(PROV.PRPR_ID,''))<>''
AND HDR.CLCL_ID='{0}'

union      

select
HDR.CLCL_ID As ClaimID,
'Operating' As ProviderType,
[PROV].[PRPR_ID] AS [ProviderID],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_LAST_NAME] else PROV.PRPR_NAME end AS [ProviderLastName],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_FIRST_NAME] else null end AS [ProviderFirstName],
PROV.MCTN_ID As ProviderTaxID,
SUBSTRING([PROVADDR].[PRAD_ADDR1], 1, 55) AS [ProviderAddress1],
SUBSTRING([PROVADDR].[PRAD_ADDR2], 1, 55) AS [ProviderAddress2],
[PROVADDR].[PRAD_CITY] AS [ProviderAddressCity],
[PROVADDR].[PRAD_STATE] AS [ProviderAddressState],
substring([PROVADDR].[PRAD_ZIP],1,9) AS [ProviderAddressZip],
PROV.MCTN_ID as ProviderMedID,
PROV.PRPR_NPI as ProviderNPI,
[PROV].[PRCF_MCTR_SPEC] AS [ProviderSpecialtyCode],
[PROV].[PRPR_STS] AS [ProviderStatus],
[PROV].[PRPR_TAXONOMY_CD] AS [ProviderTaxonomy],
[PROVADDR].[PRAD_EFF_DT] AS [EffectiveDate],
[PROVADDR].[PRAD_TERM_DT] AS [TerminateDate],
[PROVADDR].[PRAD_PHONE] AS [ProviderTelephone]
From FACETS.dbo.CMC_CLCL_CLAIM HDR
Inner Join FACETS.dbo.CMC_CLPE_PROV_DATA OP on OP.CLCL_ID=HDR.CLCL_ID and OP.CLPE_PRPR_TYPE='OP'
INNER JOIN FACETS.dbo.[CMC_PRPR_PROV] [PROV] ON OP.CLPE_PRPR_ID = [PROV].PRPR_ID
LEFT JOIN FACETS.dbo.[CMC_PRCP_COMM_PRAC] [PROV_PRAC] ON( [PROV].[PRCP_ID] = [PROV_PRAC].[PRCP_ID] )
LEFT JOIN FACETS.dbo.[CMC_PRAD_ADDRESS] [PROVADDR] ON( [PROV].[PRAD_ID] = [PROVADDR].[PRAD_ID]
	AND [PROV].[PRAD_TYPE_PRIM] = [PROVADDR].[PRAD_TYPE])
where ltrim(isnull(PROV.PRPR_ID,''))<>''
AND HDR.CLCL_ID='{0}'

union      

select
HDR.CLCL_ID As ClaimID,
'Operating' As ProviderType,
[PROV].[PRPR_ID] AS [ProviderID],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_LAST_NAME] else PROV.PRPR_NAME end AS [ProviderLastName],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_FIRST_NAME] else null end AS [ProviderFirstName],
PROV.MCTN_ID As ProviderTaxID,
SUBSTRING([PROVADDR].[PRAD_ADDR1], 1, 55) AS [ProviderAddress1],
SUBSTRING([PROVADDR].[PRAD_ADDR2], 1, 55) AS [ProviderAddress2],
[PROVADDR].[PRAD_CITY] AS [ProviderAddressCity],
[PROVADDR].[PRAD_STATE] AS [ProviderAddressState],
substring([PROVADDR].[PRAD_ZIP],1,9) AS [ProviderAddressZip],
PROV.MCTN_ID as ProviderMedID,
PROV.PRPR_NPI as ProviderNPI,
[PROV].[PRCF_MCTR_SPEC] AS [ProviderSpecialtyCode],
[PROV].[PRPR_STS] AS [ProviderStatus],
[PROV].[PRPR_TAXONOMY_CD] AS [ProviderTaxonomy],
[PROVADDR].[PRAD_EFF_DT] AS [EffectiveDate],
[PROVADDR].[PRAD_TERM_DT] AS [TerminateDate],
[PROVADDR].[PRAD_PHONE] AS [ProviderTelephone]
From FACETS.dbo.CMC_CLCL_CLAIM HDR
Inner Join FACETS.dbo.CMC_CLHP_HOSP HOSP On HOSP.CLCL_ID=HDR.CLCL_ID
INNER JOIN FACETS.dbo.[CMC_PRPR_PROV] [PROV] ON HOSP.CLHP_PRPR_ID_OPER = [PROV].PRPR_ID
LEFT JOIN FACETS.dbo.[CMC_PRCP_COMM_PRAC] [PROV_PRAC] ON( [PROV].[PRCP_ID] = [PROV_PRAC].[PRCP_ID] )
LEFT JOIN FACETS.dbo.[CMC_PRAD_ADDRESS] [PROVADDR] ON( [PROV].[PRAD_ID] = [PROVADDR].[PRAD_ID]
	AND [PROV].[PRAD_TYPE_PRIM] = [PROVADDR].[PRAD_TYPE])
where ltrim(isnull(PROV.PRPR_ID,''))<>''
AND HDR.CLCL_ID='{0}'

union      

select
HDR.CLCL_ID As ClaimID,
'Other' As ProviderType,
[PROV].[PRPR_ID] AS [ProviderID],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_LAST_NAME] else PROV.PRPR_NAME end AS [ProviderLastName],
case when PROV.PRPR_ENTITY='P' then [PROV_PRAC].[PRCP_FIRST_NAME] else null end AS [ProviderFirstName],
PROV.MCTN_ID As ProviderTaxID,
SUBSTRING([PROVADDR].[PRAD_ADDR1], 1, 55) AS [ProviderAddress1],
SUBSTRING([PROVADDR].[PRAD_ADDR2], 1, 55) AS [ProviderAddress2],
[PROVADDR].[PRAD_CITY] AS [ProviderAddressCity],
[PROVADDR].[PRAD_STATE] AS [ProviderAddressState],
substring([PROVADDR].[PRAD_ZIP],1,9) AS [ProviderAddressZip],
PROV.MCTN_ID as ProviderMedID,
PROV.PRPR_NPI as ProviderNPI,
[PROV].[PRCF_MCTR_SPEC] AS [ProviderSpecialtyCode],
[PROV].[PRPR_STS] AS [ProviderStatus],
[PROV].[PRPR_TAXONOMY_CD] AS [ProviderTaxonomy],
[PROVADDR].[PRAD_EFF_DT] AS [EffectiveDate],
[PROVADDR].[PRAD_TERM_DT] AS [TerminateDate],
[PROVADDR].[PRAD_PHONE] AS [ProviderTelephone]
From FACETS.dbo.CMC_CLCL_CLAIM HDR
Inner Join FACETS.dbo.CMC_CLHP_HOSP HOSP On HOSP.CLCL_ID=HDR.CLCL_ID
INNER JOIN FACETS.dbo.[CMC_PRPR_PROV] [PROV] ON case when ltrim(isnull(HOSP.CLHP_PRPR_ID_OTH1,''))='' then HOSP.CLHP_PRPR_ID_OTH2 else HOSP.CLHP_PRPR_ID_OTH1 end = [PROV].PRPR_ID
LEFT JOIN FACETS.dbo.[CMC_PRCP_COMM_PRAC] [PROV_PRAC] ON( [PROV].[PRCP_ID] = [PROV_PRAC].[PRCP_ID] )
LEFT JOIN FACETS.dbo.[CMC_PRAD_ADDRESS] [PROVADDR] ON( [PROV].[PRAD_ID] = [PROVADDR].[PRAD_ID]
	AND [PROV].[PRAD_TYPE_PRIM] = [PROVADDR].[PRAD_TYPE])
where ltrim(isnull(PROV.PRPR_ID,''))<>''
AND HDR.CLCL_ID='{0}'
";

		public static string FacetsLineSingle = @"
Select 
    HDR.CLCL_ID As ClaimID
    , LINE.CDML_SEQ_NO As ClaimSeqNbr
    , LINE.PSCD_ID As POS
    ,SUBSTRING(LINE.IPCD_ID, 1, 5) As ProcCde
    , CASE WHEN ISNULL(LINE.CDML_CAP_IND,'') = '' THEN '1' WHEN LINE.CDML_CAP_IND= 'Y' THEN '1' ELSE '0' END  As IsCap
    , SUBSTRING(LINE.IPCD_ID, 6, 2) As Mod1
    , LINE.CDML_IPCD_MOD2 As Mod2
    , LINE.CDML_IPCD_MOD3 As Mod3
    , LINE.CDML_IPCD_MOD4 As Mod4
    , LINE.CDML_CHG_AMT As ChargeAmt
    , LINE.CDML_UNITS As Units
    , LINE.CDML_UNITS_ALLOW As UnitsAllowed
    , LINE.CDML_DED_AMT As DtlDedAmt
    , LINE.CDML_COPAY_AMT As DtlCopayAmt
    , LINE.CDML_COINS_AMT As DtlCoInsAmt
    , LINE.CDML_FROM_DT As DtlDosFrom
    , LINE.CDML_TO_DT As DtlDosTo
    , LINE.CDML_ALLOW As DtlAllowedAmt
    , LINE.CDML_PR_PYMT_AMT As DtlProvPayAmt
    , LINE.CDML_SB_PYMT_AMT As DtlMbrPayAmt
	, LINE.CDML_PR_PYMT_AMT + LINE.CDML_SB_PYMT_AMT As DtlTotalPaidAmt
	, LINE.CDML_PAID_AMT As DtlPaidAmt
	, CDCBCOB.CDCB_COB_ALLOW As COBDtlAllowedAmt
    , CDCBCOB.CDCB_COB_AMT As COBDtlPaidAmt
    , CDCBCOB.CDCB_COB_COINS_AMT As COBDtlCoInsAmt
    , CDCBCOB.CDCB_COB_COPAY_AMT As COBDtlCoPayAmt
    , CDCBCOB.CDCB_COB_DED_AMT As COBDtlDedAmt
    , LINE.RCRC_ID As RevCode
	, substring(LINE.IDCD_ID,1,8) As DtlDiagCode
	, '01' As DiagPointer1
	, LINE.CDML_CLMD_TYPE2 As DiagPointer2
    , LINE.CDML_CLMD_TYPE3 As DiagPointer3
    , LINE.CDML_CLMD_TYPE4 As DiagPointer4
	,CDSD.CDSD_NDC_CODE as DrugCode
	,cast(case when ltrim(isnull(CDSD.CDSD_NDC_UNITS,''))='' then '0' else ltrim(CDSD.CDSD_NDC_UNITS) end as decimal(15,2)) as DrugUnits
	,case when CDID.CDID_TYPE='NDC' and CDID.CDID_ADDL_DATA_1='PU1' then 'F2'
    when CDID.CDID_TYPE='NDC' and CDID.CDID_ADDL_DATA_1='PU2' then 'GR'
	when CDID.CDID_TYPE='NDC' and CDID.CDID_ADDL_DATA_1='PU3' then 'ME'
	when CDID.CDID_TYPE='NDC' and CDID.CDID_ADDL_DATA_1='PU4' then 'ML'
	when CDID.CDID_TYPE='NDC' and CDID.CDID_ADDL_DATA_1='PU5' then 'UN'
	when CDSD.CDSD_NDC_MCTR_TYPE='PU1' then 'F2'
	when CDSD.CDSD_NDC_MCTR_TYPE='PU2' then 'GR'
	when CDSD.CDSD_NDC_MCTR_TYPE='PU3' then 'ME'
	when CDSD.CDSD_NDC_MCTR_TYPE='PU4' then 'ML'
	when CDSD.CDSD_NDC_MCTR_TYPE='PU5' then 'UN'
	else 'UN' end as DrugUOM
	,LINE.CDML_CAP_IND As DtlCapInd 
	,HDR.CLCL_PAID_DT as PaidDate
	,LTRIM(RTRIM(CDML_DISALL_EXCD)) AS ExcludeCode1
	,Left(Ltrim(atuf.ATUF_TEXT1),1) as FamilyPlanIndicator
From FACETS.dbo.CMC_CDML_CL_LINE LINE
	Left Join FACETS.dbo.CMC_CDCB_LI_COB CDCBCOB On (LINE.CLCL_ID = CDCBCOB.CLCL_ID And LINE.CDML_SEQ_NO = CDCBCOB.CDML_SEQ_NO)
	Left Join FACETS.dbo.CMC_CDNP_NWX_PRCNG PRCNG On (LINE.CLCL_ID = PRCNG.CLCL_ID And LINE.CDML_SEQ_NO = PRCNG.CDNP_LINE_SEQ_NO And PRCNG.CDNP_RATE = 111.88)
	Left Join FACETS.dbo.CMC_EXCD_EXPL_CD COBEXPL On (CDCBCOB.CDCB_COB_REAS_CD = COBEXPL.EXCD_ID)
	Left Join FACETS.dbo.CMC_EXCD_EXPL_CD EXPL On (LINE.CDML_DISALL_EXCD = EXPL.EXCD_ID)
	Left Join FACETS.dbo.CMC_UMUM_UTIL_MGT UMUM On (LINE.CDML_UMAUTH_ID = UMUM.UMUM_REF_ID)
	Left Join FACETS.dbo.CMC_UMSV_SERVICES CMUM On ( LINE.CDML_UMAUTH_ID =  CMUM.UMUM_REF_ID )
	  and (LINE.CDML_FROM_DT BETWEEN CMUM.UMSV_FROM_DT AND CMUM.UMSV_TO_DT  OR LINE.CDML_TO_DT BETWEEN CMUM.UMSV_FROM_DT AND CMUM.UMSV_TO_DT  ) AND CMUM.IPCD_ID <> ' '
	Left Join FACETS.dbo.CMC_CDOR_LI_OVR CDOR On (LINE.CLCL_ID = CDOR.CLCL_ID And LINE.CDML_SEQ_NO = CDOR.CDML_SEQ_NO And CDOR.CDOR_OR_ID = 'XS')
	Left Join FACETS.dbo.CMC_CDOR_LI_OVR CDOR_NJ On (LINE.CLCL_ID = CDOR_NJ.CLCL_ID And LINE.CDML_SEQ_NO = CDOR_NJ.CDML_SEQ_NO And CDOR_NJ.CDOR_OR_ID = 'AN')
	Left Join FACETS.dbo.CMC_CDSD_SUPP_DATA CDSD On CDSD.CLCL_ID=LINE.CLCL_ID And CDSD.CDML_SEQ_NO=LINE.CDML_SEQ_NO
	Left Join FACETS.dbo.CMC_CDID_DATA CDID On CDID.CLCL_ID=LINE.CLCL_ID and CDID.CDML_SEQ_NO=LINE.CDML_SEQ_NO
	Left Join FACETS.dbo.CER_ATXR_ATTACH_U atxr on atxr.ATXR_SOURCE_ID=LINE.ATXR_SOURCE_ID and atxr.ATSY_ID='FPLI'
	Left Join FACETS.dbo.CER_ATUF_USERFLD_D atuf on atuf.ATXR_DEST_ID=atxr.ATXR_DEST_ID
	INNER JOIN FACETS.dbo.CMC_CLCL_CLAIM HDR ON HDR.CLCL_ID=LINE.CLCL_ID
where ((LINE.CDML_PAID_AMT>0 and not ((CDML_CONSIDER_CHG>0 and CDML_ALLOW=0) or CDML_CONSIDER_CHG=CDML_DISALL_AMT))
Or (LINE.CDML_PAID_AMT=0 and case when ltrim(rtrim(isnull(CDOR.EXCD_ID,'')))<>'' then ltrim(rtrim(CDOR.EXCD_ID)) else ltrim(rtrim(isnull(LINE.CDML_DISALL_EXCD,''))) end in ('','H01','H02','J01','J02','KD3','KZ0','KZ1','KZ2','KZ3','KZ4','KZ5','L23','L49','L50','TF1'))
Or (LINE.CDML_PAID_AMT=0 and right(LINE.CLCL_ID,2)<>'00')
)
And LINE.CDML_CUR_STS<>'91'
AND HDR.CLCL_ID='{0}'
";

		public static string FacetsCodeSingle = @"
SELECT DISTINCT [DIAG].[CLCL_ID] AS [ClaimID],
				CASE WHEN (TRY_CAST([DIAG].[CLMD_TYPE] AS INT)) = 1 THEN 'PD' WHEN [DIAG].[CLMD_TYPE] = 'AD' THEN 'ADMT'
				WHEN LEFT(LTRIM(DIAG.CLMD_TYPE),1)='R' THEN 'PR' ELSE 'DIAG' END AS [CodeType],
				CASE WHEN [DIAG].[CLMD_TYPE] = 'AD' THEN 1 
				WHEN LEFt(LTRIM(DIAG.CLMD_TYPE),1)='R' THEN TRY_CAST(SUBSTRING(LTRIM(DIAG.CLMD_TYPE),2,1) AS INT)
				ELSE TRY_CAST([DIAG].[CLMD_TYPE] AS INT) END AS [CodeSequence],
				'' AS [CodeLetter],
				CAST([DIAG].[IDCD_ID] AS VARCHAR(8)) AS [CodeValue],
				NULL AS [CodeAmount],
				NULL AS [CodeStartDate],
				NULL AS [CodeEndDate],
				case CMF.CLMF_ICD_QUAL_IND when '9' THEN 'ICD9' 
				WHEN '0' THEN 'ICD10' ELSE NULL END as ICDTypeCode,
				CASE WHEN DIAG.CLMD_POA_IND IN ('Y','N','W','U') THEN DIAG.CLMD_POA_IND
				     WHEN LTRIM(ISNULL(DIAG.CLMD_POA_IND,''))<>'' THEN 'U'
					 ELSE NULL END as DIAGPOI
FROM FACETS.dbo.[CMC_CLMD_DIAG] [DIAG]
	 INNER JOIN FACETS.dbo.[CMC_IDCD_DIAG_CD] [CDE] ON( [DIAG].[IDCD_ID] = [CDE].[IDCD_ID] )
	 INNER JOIN FACETS.dbo.[CMC_CLMF_MULT_FUNC] [CMF] ON( [DIAG].[CLCL_ID] = [CMF].[CLCL_ID] )
	 INNER JOIN FACETS.dbo.[CMC_CLCL_CLAIM] HDR ON HDR.CLCL_ID=DIAG.CLCL_ID
WHERE HDR.CLCL_ID='{0}'
AND ( [CDE].[IDCD_ACTION] <> 'D'
	OR ( [DIAG].[CLMD_TYPE] IN( 'A', '1', 'AD', '01')
	   )
	OR ( [CDE].[IDCD_ACTION] = 'D'
	 AND [HDR].[CLCL_LOW_SVC_DT] >= [CDE].[IDCD_EFF_DT]
	 AND [HDR].[CLCL_LOW_SVC_DT] <= [CDE].[IDCD_TERM_DT]
	   )
	 )
	 

UNION

SELECT DISTINCT [VAL].[CLCL_ID] AS [ClaimID],
				'VAL' AS [CodeType],
				ROW_NUMBER()  OVER (Partition  BY [VAL].[CLCL_ID] order by [VAL].[CLVC_LETTER]) AS [CodeSequence],
				[VAL].[CLVC_LETTER] AS [CodeLetter],
				CAST([VAL].[CLVC_CODE] AS VARCHAR(8)) AS [CodeValue],
				[VAL].[CLVC_AMT] AS [CodeAmount],
				NULL AS [CodeStartDate],
				NULL AS [CodeEndDate],
				case CMF.CLMF_ICD_QUAL_IND when '9' THEN 'ICD9' 
				WHEN '0' THEN 'ICD10' ELSE NULL END as ICDTypeCode,
				NULL as DiagPOI
FROM FACETS.dbo.[CMC_CLVC_VAL_CODE] [VAL]
	  INNER JOIN FACETS.dbo.[CMC_CLMF_MULT_FUNC] [CMF] ON( [VAL].[CLCL_ID] = [CMF].[CLCL_ID] )
	 INNER JOIN FACETS.dbo.[CMC_CLCL_CLAIM] HDR ON HDR.CLCL_ID=VAL.CLCL_ID
	  WHERE HDR.CLCL_ID='{0}'
		AND [VAL].[CLVC_AMT] <> 0 and [VAL].[CLVC_NUMBER] = 39
UNION

SELECT DISTINCT [COND].[CLCL_ID] [ClaimID],
				'COND' AS [CodeType],
				CAST([COND].[CLHC_SEQ_NO] AS VARCHAR(2)) AS [CodeSequence],
				'' AS [CodeLetter],
				CAST([COND].[CLHC_COND_CD] AS VARCHAR(8)) AS [CodeValue],
				NULL AS [CodeAmount],
				NULL AS [CodeStartDate],
				NULL AS [CodeEndDate],
				case CMF.CLMF_ICD_QUAL_IND when '9' THEN 'ICD9' 
				WHEN '0' THEN 'ICD10' ELSE NULL END as ICDTypeCode,
				NULL as DiagPOI
FROM FACETS.dbo.[CMC_CLHC_COND_CODE] [COND]
	  INNER JOIN FACETS.dbo.[CMC_CLMF_MULT_FUNC] [CMF] ON( [COND].[CLCL_ID] = [CMF].[CLCL_ID] )
	 INNER JOIN FACETS.dbo.[CMC_CLCL_CLAIM] HDR ON HDR.CLCL_ID=COND.CLCL_ID
WHERE HDR.CLCL_ID='{0}'

UNION

SELECT DISTINCT HOSP.CLCL_ID as ClaimID,
'DRG' as CodeType,
'1' as CodeSequence,
'' as CodeLetter,
HOSP.CLHP_INPUT_AGRG_ID as CodeValue,
NULL as CodeAmount,
null as CodeStartDate,
null as CodeEndDate,
'ICD10' as ICDTypeCode,
null as DiagPOI
FROM FACETS.dbo.CMC_CLHP_HOSP [HOSP]
	 INNER JOIN FACETS.dbo.[CMC_CLCL_CLAIM] HDR ON HDR.CLCL_ID=HOSP.CLCL_ID
where HDR.CLCL_ID='{0}'
	AND ltrim(isnull(HOSP.CLHP_INPUT_AGRG_ID,''))<>''

UNION

SELECT DISTINCT [PROCCode].[CLCL_ID] [ClaimID],
				CASE WHEN PROCCode.CLHI_TYPE = 'P' THEN 'PP' ELSE 'OPI' END AS [CodeType]	,
				CAST(1 AS VARCHAR(2)) AS [CodeSequence],
				'' AS [CodeLetter],
				CAST([PROCCode].[IPCD_ID] AS VARCHAR(8)) AS [CodeValue],
				NULL AS [CodeAmount],
				[PROCCode].[CLHI_IP_DT] AS [CodeStartDate],
				[PROCCode].[CLHI_IP_DT] AS [CodeEndDate],
				case CMF.CLMF_ICD_QUAL_IND when '9' THEN 'ICD9' 
				WHEN '0' THEN 'ICD10' ELSE NULL END as ICDTypeCode,
				NULL as DiagPOI
FROM FACETS.dbo.[CMC_CLHI_PROC] [PROCCode]
	  INNER JOIN FACETS.dbo.[CMC_CLMF_MULT_FUNC] [CMF] ON( [PROCCode].[CLCL_ID] = [CMF].[CLCL_ID] )
	 INNER JOIN FACETS.dbo.[CMC_CLCL_CLAIM] HDR ON HDR.CLCL_ID=PROCCode.CLCL_ID
WHERE HDR.CLCL_ID='{0}'

UNION

SELECT [OCCBH].[CLCL_ID] [ClaimID],
	   'OCC_BH' AS [CodeType],
	   CAST([OCCBH].[CLHO_SEQ_NO] AS VARCHAR(2)) AS [CodeSequence],
	   '' AS [CodeLetter],
	   CAST([OCCBH].[CLHO_OCC_CODE] AS VARCHAR(8)) AS [CodeValue],
	   NULL AS [CodeAmount],
	   [OCCBH].[CLHO_OCC_FROM_DT] AS [CodeStartDate],
	   [OCCBH].[CLHO_OCC_TO_DT] AS [CodeEndDate],
				case CMF.CLMF_ICD_QUAL_IND when '9' THEN 'ICD9' 
				WHEN '0' THEN 'ICD10' ELSE NULL END as ICDTypeCode,
				NULL as DiagPOI
FROM FACETS.dbo.[CMC_CLHO_OCC_CODE] [OCCBH]
	  INNER JOIN FACETS.dbo.[CMC_CLMF_MULT_FUNC] [CMF] ON( [OCCBH].[CLCL_ID] = [CMF].[CLCL_ID] )
	 INNER JOIN FACETS.dbo.[CMC_CLCL_CLAIM] HDR ON HDR.CLCL_ID=OCCBH.CLCL_ID
WHERE HDR.CLCL_ID='{0}'
  AND CONVERT(VARCHAR(10), [CLHO_OCC_FROM_DT], 101) <> '01/01/1753'
  AND CONVERT(VARCHAR(10), [OCCBH].[CLHO_OCC_TO_DT], 101) = '01/01/1753'

UNION

SELECT 
[OCCBI].[CLCL_ID] [ClaimID],
'OCC_BI' AS [CodeType],
CAST([OCCBI].[CLHO_SEQ_NO] AS VARCHAR(2)) AS [CodeSequence],
'' AS [CodeLetter],
CAST([OCCBI].[CLHO_OCC_CODE] AS VARCHAR(8)) AS [CodeValue],
NULL AS [CodeAmount],
[OCCBI].[CLHO_OCC_FROM_DT] AS [CodeStartDate],
[OCCBI].[CLHO_OCC_TO_DT] AS [CodeEndDate],
				case CMF.CLMF_ICD_QUAL_IND when '9' THEN 'ICD9' 
				WHEN '0' THEN 'ICD10' ELSE NULL END as ICDTypeCode,
				NULL as DiagPOI
FROM FACETS.dbo.[CMC_CLHO_OCC_CODE] [OCCBI]
	  INNER JOIN FACETS.dbo.[CMC_CLMF_MULT_FUNC] [CMF] ON( [OCCBI].[CLCL_ID] = [CMF].[CLCL_ID] )
	 INNER JOIN FACETS.dbo.[CMC_CLCL_CLAIM] HDR ON HDR.CLCL_ID=OCCBI.CLCL_ID
WHERE HDR.CLCL_ID='{0}'
  AND CONVERT(VARCHAR(10), [CLHO_OCC_FROM_DT], 101) <> '01/01/1753'
  AND CONVERT(VARCHAR(10), [CLHO_OCC_TO_DT], 101) <> '01/01/1753'
";

		public static string FacetsExtraSvdSingle = @"
SELECT DISTINCT [cdml].[CLCL_ID] AS [ClaimID],
				[cdml].[CDML_SEQ_NO] [ClaimLineSeq],
				substring(ltrim([clcl].[MEME_CK]),1,15) AS [MemberID],
				'P' AS [COBPayerType],
				right(rtrim(MCREENT.MCRE_ID),3) AS [COBPayerID],
				MCREENT.MCRE_NAME AS [COBPayerName],
				[CDCBCOB].[CDCB_COB_ALLOW] AS [COBDtlAllowedAmt],
				[CDCBCOB].[CDCB_COB_COINS_AMT] AS [COBDtlCoInsAmt],
				[CDCBCOB].[CDCB_COB_COPAY_AMT] AS [COBDtlCoPayAmt],
				[CDCBCOB].[CDCB_COB_DED_AMT] AS [COBDtlDedAmt],
				[CDCBCOB].[CDCB_COB_AMT] AS [COBDtlPaidAmt],
				[CDCBCOB].[CDCB_COB_DISALLOW] AS [COBDtlDisAlwAmt],
				0 as IsHealthPlan
FROM FACETS.dbo.CMC_CLCL_CLAIM [clcl]
INNER JOIN FACETS.dbo.[CMC_CDML_CL_LINE] [cdml] ON( [clcl].[CLCL_ID] = [cdml].[CLCL_ID] )
INNER JOIN FACETS.dbo.[CMC_CLCB_CL_COB] [CLCBCOB] ON( [clcl].[MEME_CK] = [CLCBCOB].[MEME_CK]
									AND [clcl].[CLCL_ID] = [CLCBCOB].[CLCL_ID]
										)
INNER JOIN (select MEME_CK,MCRE_ID,MECB_POLICY_ID,row_number() over(partition by MEME_CK order by MECB_INSUR_ORDER) as RN from FACETS.dbo.[CMC_MECB_COB]) [MECBCOB] ON [clcl].[MEME_CK] = [MECBCOB].[MEME_CK] and MECBCOB.RN=1
INNER JOIN FACETS.dbo.[CMC_MCRE_RELAT_ENT] [MCREENT] ON [MECBCOB].[MCRE_ID] = [MCREENT].[MCRE_ID]
INNER JOIN FACETS.dbo.[CMC_CDCB_LI_COB] [CDCBCOB] ON( [cdml].[CLCL_ID] = [CDCBCOB].[CLCL_ID]
										   AND [cdml].[CDML_SEQ_NO] = [CDCBCOB].[CDML_SEQ_NO]
											 )
WHERE [clcl].CLCL_ID = '{0}' and ltrim(MCREENT.MCRE_ID)<>'';
";

		public static string FacetsExtraSbrSingle = @"
SELECT DISTINCT [clcl].[CLCL_ID] AS [ClaimID],
				[clcl].[MEME_CK] AS [MemberID],
				right(rtrim(MCREENT.MCRE_ID),3) AS [COBPayerID],
				MCREENT.MCRE_NAME AS [PayerFullName]
, MCREENT.MCRE_NAME AS PayerLastName
, NULL AS PayerFirstName
, NULL AS PayerMiddleInitial
, substring(ltrim(MCREENT.MCRE_ADDR1),1,35) AS PayerAddress1
, NULL AS PayerAddress2
, MCREENT.MCRE_CITY AS PayerAddressCity
, MCREENT.MCRE_STATE AS PayerState
, LEFT(MCREENT.MCRE_ZIP, 10) AS PayerZip
, NULL AS PayerCounty
, NULL AS PayerCountryCode
, 0 AS IsHealthPlan
, CASE WHEN LEFT(LTRIM(RTRIM(MECB_POLICY_ID)), 3) = '600' THEN 'MA' 
	WHEN LEFT(LTRIM(RTRIM(MECB_POLICY_ID)), 3) = '100' THEN 'MB'
	WHEN LEFT(LTRIM(RTRIM(MECB_POLICY_ID)), 3) = '103' THEN 'OF'
	WHEN LEFT(LTRIM(RTRIM(MECB_POLICY_ID)), 3) = '511' THEN '16'
	ELSE 'CI' END AS ClaimFilingIndicatorCode
,'P' AS COBPayerType
FROM FACETS.dbo.CMC_CLCL_CLAIM [clcl]
INNER JOIN FACETS.dbo.[CMC_CLCB_CL_COB] [CLCBCOB] ON( [clcl].[MEME_CK] = [CLCBCOB].[MEME_CK]
											AND [clcl].[CLCL_ID] = [CLCBCOB].[CLCL_ID]
											  )
INNER JOIN (select MEME_CK,MCRE_ID,MECB_POLICY_ID,row_number() over(partition by MEME_CK order by MECB_INSUR_ORDER) as RN from FACETS.dbo.[CMC_MECB_COB]) [MECBCOB] ON [clcl].[MEME_CK] = [MECBCOB].[MEME_CK] and MECBCOB.RN=1
INNER JOIN FACETS.dbo.[CMC_MCRE_RELAT_ENT] [MCREENT] ON [MECBCOB].[MCRE_ID] = [MCREENT].[MCRE_ID]
WHERE clcl.CLCL_ID='{0}' AND ISNULL(MECB_POLICY_ID, '') <> '';
";
	}
}
