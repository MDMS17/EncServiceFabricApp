using EncDataModel.Facets;
using EncDataModel.Submission837;
using Load837Facets.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Load837Facets.Processes
{
    public partial class Loading
    {
        public static void LoadFacetsCMSI(FacetsContext _contextFacets, Sub837Context _context)
        {
            _contextFacets.Database.SetConnectionString(Common.cn_Facets);
            _context.Database.SetConnectionString(Common.cn);
            List<FacetsHeader> cacheHeaders = _contextFacets.FacetsHeaders.FromSqlRaw(string.Format(Common.FacetsHeader, Common.PlanId, Common.StartDate, Common.EndDate, Common.ClaimType, (Common.PageNumber * 10000).ToString())).ToList();
            List<FacetsLine> cacheLines = _contextFacets.FacetsLines.FromSqlRaw(string.Format(Common.FacetsLine, Common.PlanId, Common.StartDate, Common.EndDate, Common.ClaimType, (Common.PageNumber * 10000).ToString())).ToList();
            List<FacetsCode> CacheCodes = _contextFacets.FacetsCodes.FromSqlRaw(string.Format(Common.FacetsCode, Common.PlanId, Common.StartDate, Common.EndDate, Common.ClaimType, (Common.PageNumber * 10000).ToString())).ToList();
            List<FacetsProvider> cacheProviders = _contextFacets.FacetsProviders.FromSqlRaw(string.Format(Common.FacetsProvider, Common.PlanId, Common.StartDate, Common.EndDate, Common.ClaimType, (Common.PageNumber * 10000).ToString())).ToList();
            List<FacetsExtraSbr> cacheExtraSbrs = _contextFacets.FacetsExtraSbrs.FromSqlRaw(string.Format(Common.FacetsExtraSbr, Common.PlanId, Common.StartDate, Common.EndDate, Common.ClaimType, (Common.PageNumber * 10000).ToString())).ToList();
            List<FacetsExtraSvd> CacheMultiSvds = _contextFacets.FacetsExtraSvds.FromSqlRaw(string.Format(Common.FacetsExtraSvd, Common.PlanId, Common.StartDate, Common.EndDate, Common.ClaimType, (Common.PageNumber * 10000).ToString())).ToList();
            if (Common.Excludes != null && Common.Excludes.Count > 0) 
            {
                foreach (FacetsHeader fh in cacheHeaders) if (Common.Excludes.Contains(fh.ClaimID)) cacheHeaders.Remove(fh);
            }
            if (Common.Includes != null && Common.Includes.Count > 0) 
            {
                Common.Includes = Common.Includes.Except(Common.Includes.Intersect(cacheHeaders.Select(x => x.ClaimID))).ToList();
            }
            List<ClaimCAS> cases = new List<ClaimCAS>();
            List<ClaimCRC> crcs = new List<ClaimCRC>();
            List<ClaimHeader> headers = new List<ClaimHeader>();
            List<ClaimHI> his = new List<ClaimHI>();
            List<ClaimK3> k3s = new List<ClaimK3>();
            List<ClaimNte> notes = new List<ClaimNte>();
            List<ClaimProvider> providers = new List<ClaimProvider>();
            List<ClaimPWK> pwks = new List<ClaimPWK>();
            List<ClaimSBR> sbrs = new List<ClaimSBR>();
            List<ClaimSecondaryIdentification> sids = new List<ClaimSecondaryIdentification>();
            List<ClaimLineSVD> lineSvds = new List<ClaimLineSVD>();
            List<ServiceLine> lines = new List<ServiceLine>();
            foreach (FacetsHeader cacheHeader in cacheHeaders)
            {
                ClaimProvider provider;
                string ProviderTaxId = "";

                List<FacetsLine> facetsLines = cacheLines.Where(x => x.ClaimID == cacheHeader.ClaimID).Where(x => !Common.DMEProcCodes.Contains(x.ProcCode)).ToList();
                ClaimHeader header = new ClaimHeader();
                header.FileID = 0;
                header.ClaimID = cacheHeader.ClaimID;
                header.ClaimAmount = cacheHeader.HeaderChargeAmt.ToString("0.##");
                header.ClaimPOS = cacheHeader.FacType + cacheHeader.BillClass;
                header.ClaimType = "A";
                header.ClaimFrequencyCode = cacheHeader.Frequency;
                header.ClaimProviderSignature = "Y";
                header.ClaimProviderAssignment = "A";
                header.ClaimBenefitAssignment = cacheHeader.BenAssigned;
                header.ClaimReleaseofInformationCode = cacheHeader.RelInfo;
                header.StatementFromDate = cacheHeader.StatFromDte?.ToString("yyyyMMdd");
                header.StatementToDate = cacheHeader.StatToDte?.ToString("yyyyMMdd");
                header.AdmissionDate = cacheHeader.AdmitDte?.ToString("yyyyMMdd");
                header.AdmissionTypeCode = cacheHeader.AdmitType;
                header.AdmissionSourceCode = cacheHeader.AdmitSource;
                header.PatientPaidAmount = cacheHeader.HeaderPatPaidAmt.ToString("#.##");
                header.ExportType = "I" + cacheHeader.PlanID;
                header.ContractAmount = cacheLines.Where(x => x.ClaimID == cacheHeader.ClaimID).Sum(x => x.DtlPaidAmt).ToString("0.##");
                headers.Add(header);

                foreach (FacetsCode code in CacheCodes.Where(x => x.ClaimID == cacheHeader.ClaimID).OrderBy(y => Tuple.Create(y.CodeType, y.CodeSequence)))
                {
                    ClaimHI hi = new ClaimHI();
                    hi.FileID = 0;
                    hi.ClaimID = header.ClaimID;
                    hi.HICode = code.CodeValue.Replace(".", "");
                    switch (code.CodeType)
                    {
                        case "DIAG":
                            hi.HIQual = hi.HIQual = code.CodeSequence == "1" ? "ABK" : "ABF";
                            hi.PresentOnAdmissionIndicator = code.DIAGPOI;
                            break;
                        case "ADMT":
                            hi.HIQual = "ABJ";
                            break;
                        case "COND":
                            hi.HIQual = "BG";
                            break;
                        case "DRG":
                            hi.HIQual = "DR";
                            break;
                        case "PD":
                            hi.HIQual = "ABN";
                            hi.PresentOnAdmissionIndicator = code.DIAGPOI;
                            break;
                        case "OCC_BH":
                            hi.HIQual = "BH";
                            hi.HIFromDate = code.CodeStartDate?.ToString("yyyyMMdd");
                            break;
                        case "OCC_BI":
                            hi.HIQual = "BI";
                            hi.HIFromDate = code.CodeStartDate?.ToString("yyyyMMdd");
                            hi.HIToDate = code.CodeEndDate?.ToString("yyyyMMdd");
                            break;
                        case "PR":
                            hi.HIQual = "APR";
                            break;
                        case "PP":
                            hi.HIQual = "BBR";
                            hi.HIFromDate = code.CodeStartDate?.ToString("yyyyMMdd");
                            break;
                        case "OPI":
                            hi.HIQual = "BBQ";
                            hi.HIFromDate = code.CodeStartDate?.ToString("yyyyMMdd");
                            break;
                        case "VAL":
                            hi.HIQual = "BE";
                            hi.HIAmount = code.CodeAmount?.ToString("0.##");
                            break;
                    }
                    his.Add(hi);
                }
                if (his.Count(x => x.ClaimID == header.ClaimID && x.HIQual == "ABK") == 0 && his.Count(x => x.ClaimID == header.ClaimID && x.HIQual == "ABF") > 0)
                {
                    his.FirstOrDefault(x => x.ClaimID == header.ClaimID && x.HIQual == "ABF").HIQual = "ABK";
                }
                FacetsProvider facetsProvider = cacheProviders.FirstOrDefault(x => x.ClaimID == header.ClaimID && x.ProviderType == "Billing");
                if (facetsProvider != null)
                {
                    provider = new ClaimProvider
                    {
                        FileID = 0,
                        ClaimID = header.ClaimID,
                        LoopName = "2000A",
                        ServiceLineNumber = "0",
                        ProviderQualifier = "85",
                        ProviderTaxonomyCode = facetsProvider.ProviderTaxonomy,
                        ProviderLastName = facetsProvider.ProviderLastName,
                        ProviderFirstName = facetsProvider.ProviderFirstName,
                        ProviderIDQualifier = "XX",
                        ProviderID = facetsProvider.ProviderNPI,
                        ProviderAddress = facetsProvider.ProviderAddress1,
                        ProviderAddress2 = facetsProvider.ProviderAddress2,
                        ProviderCity = facetsProvider.ProviderAddressCity,
                        ProviderState = facetsProvider.ProviderAddressState,
                        ProviderZip = facetsProvider.ProviderAddressZip
                    };
                    if (provider.ProviderZip.Length == 5) provider.ProviderZip += "9998";
                    providers.Add(provider);
                    ProviderTaxId = facetsProvider.ProviderTaxID;
                }
                facetsProvider = cacheProviders.FirstOrDefault(x => x.ClaimID == cacheHeader.ClaimID && x.ProviderType == "Attending");
                if (facetsProvider != null)
                {
                    provider = new ClaimProvider
                    {
                        FileID = 0,
                        ClaimID = header.ClaimID,
                        LoopName = "2310A",
                        ServiceLineNumber = "0",
                        ProviderQualifier = "71",
                        ProviderLastName = facetsProvider.ProviderLastName,
                        ProviderFirstName = facetsProvider.ProviderFirstName,
                        ProviderIDQualifier = "XX",
                        ProviderID = facetsProvider.ProviderNPI,
                        ProviderTaxonomyCode=facetsProvider.ProviderTaxonomy
                    };
                    providers.Add(provider);
                }
                facetsProvider = cacheProviders.FirstOrDefault(x => x.ClaimID == cacheHeader.ClaimID && x.ProviderType == "Operating");
                if (facetsProvider != null)
                {
                    provider = new ClaimProvider
                    {
                        FileID = 0,
                        ClaimID = header.ClaimID,
                        LoopName = "2310B",
                        ServiceLineNumber = "0",
                        ProviderQualifier = "72",
                        ProviderLastName = facetsProvider.ProviderLastName,
                        ProviderFirstName = facetsProvider.ProviderFirstName,
                        ProviderIDQualifier = "XX",
                        ProviderID = facetsProvider.ProviderNPI
                    };
                    providers.Add(provider);
                }
                facetsProvider = cacheProviders.FirstOrDefault(x => x.ClaimID == cacheHeader.ClaimID && x.ProviderType == "Other");
                if (facetsProvider != null)
                {
                    provider = new ClaimProvider
                    {
                        FileID = 0,
                        ClaimID = header.ClaimID,
                        LoopName = "2310B",
                        ServiceLineNumber = "0",
                        ProviderQualifier = "ZZ",
                        ProviderLastName = facetsProvider.ProviderLastName,
                        ProviderFirstName = facetsProvider.ProviderFirstName,
                        ProviderIDQualifier = "XX",
                        ProviderID = facetsProvider.ProviderNPI
                    };
                    providers.Add(provider);
                }
                provider = new ClaimProvider
                {
                    FileID = 0,
                    ClaimID = header.ClaimID,
                    LoopName = "2010BB",
                    ServiceLineNumber = "0",
                    ProviderQualifier = "PR",
                    ProviderLastName = "MMEDSCMS",
                    ProviderIDQualifier = "PI",
                    ProviderID = "80891",
                    ProviderAddress = "7500 Security Blvd",
                    ProviderCity = "Baltimore",
                    ProviderState = "MD",
                    ProviderZip = "212441850"
                };
                providers.Add(provider);

                ClaimSBR sbr = new ClaimSBR();
                sbr.FileID = 0;
                sbr.ClaimID = header.ClaimID;
                sbr.LoopName = "2000B";
                sbr.SubscriberSequenceNumber = "S";
                sbr.SubscriberRelationshipCode = "18";
                sbr.ClaimFilingCode = "MB";
                sbr.LastName = cacheHeader.MemberLastName;
                sbr.FirstName = cacheHeader.MemberFirstName;
                sbr.MidddleName = cacheHeader.MemberMiddleInit;
                sbr.IDQualifier = "MI";
                sbr.IDCode = cacheHeader.MedicareId;
                sbr.SubscriberAddress = cacheHeader.MemberAddress1;
                sbr.SubscriberAddress2 = cacheHeader.MemberAddress2;
                sbr.SubscriberCity = cacheHeader.MemberAddressCity;
                sbr.SubscriberState = cacheHeader.MemberAddressState;
                sbr.SubscriberZip = cacheHeader.MemberAddressZip;
                sbr.SubscriberBirthDate = cacheHeader.MemberBirthDate.ToString("yyyyMMdd");
                sbr.SubscriberGender = cacheHeader.MemberSex;
                sbrs.Add(sbr);
                FacetsExtraSbr facetsSbr = cacheExtraSbrs.FirstOrDefault(x => x.ClaimID == header.ClaimID);
                if (facetsSbr != null)
                {
                    sbr = new ClaimSBR();
                    sbr.FileID = 0;
                    sbr.ClaimID = header.ClaimID;
                    sbr.LoopName = "2320";
                    sbr.SubscriberSequenceNumber = "P";
                    sbr.SubscriberRelationshipCode = "18";
                    sbr.ClaimFilingCode = facetsSbr.ClaimFilingIndicatorCode;
                    sbr.COBPayerPaidAmount = facetsLines.Where(x => x.ClaimID == header.ClaimID).Sum(x => x.DtlPaidAmt).ToString("0.##");
                    sbr.BenefitsAssignmentCertificationIndicator = "Y";
                    sbr.ReleaseOfInformationCode = cacheHeader.RelInfo;
                    sbr.LastName = cacheHeader.MemberLastName;
                    sbr.FirstName = cacheHeader.MemberFirstName;
                    sbr.MidddleName = cacheHeader.MemberMiddleInit;
                    sbr.IDQualifier = "MI";
                    sbr.IDCode = cacheHeader.MedicareId;
                    sbr.SubscriberAddress = cacheHeader.MemberAddress1;
                    sbr.SubscriberAddress2 = cacheHeader.MemberAddress2;
                    sbr.SubscriberCity = cacheHeader.MemberAddressCity;
                    sbr.SubscriberState = cacheHeader.MemberAddressState;
                    sbr.SubscriberZip = cacheHeader.MemberAddressZip;
                    sbrs.Add(sbr);
                    provider = new ClaimProvider
                    {
                        ClaimID = header.ClaimID,
                        FileID = 0,
                        LoopName = "2330B",
                        ProviderAddress = facetsSbr.PayerAddress1,
                        ProviderAddress2 = facetsSbr.PayerAddress2,
                        ProviderCity = facetsSbr.PayerAddressCity,
                        ProviderState = facetsSbr.PayerState,
                        ProviderZip = facetsSbr.PayerZip,
                        ProviderLastName = facetsSbr.PayerLastName,
                        ProviderIDQualifier = "PI",
                        ProviderID = facetsSbr.COBPayerID,
                        RepeatSequence = "P"
                    };
                    providers.Add(provider);
                    sbr = new ClaimSBR();
                    sbr.FileID = 0;
                    sbr.ClaimID = header.ClaimID;
                    sbr.LoopName = "2320";
                    sbr.SubscriberSequenceNumber = "T";
                    sbr.SubscriberRelationshipCode = "18";
                    sbr.ClaimFilingCode = "MC";
                    sbr.COBPayerPaidAmount = "0.00";
                    sbr.BenefitsAssignmentCertificationIndicator = "Y";
                    sbr.ReleaseOfInformationCode = cacheHeader.RelInfo;
                    sbr.LastName = cacheHeader.MemberLastName;
                    sbr.FirstName = cacheHeader.MemberFirstName;
                    sbr.MidddleName = cacheHeader.MemberMiddleInit;
                    sbr.IDQualifier = "MI";
                    sbr.IDCode = cacheHeader.MedicareId;
                    sbr.SubscriberAddress = cacheHeader.MemberAddress1;
                    sbr.SubscriberAddress2 = cacheHeader.MemberAddress2;
                    sbr.SubscriberCity = cacheHeader.MemberAddressCity;
                    sbr.SubscriberState = cacheHeader.MemberAddressState;
                    sbr.SubscriberZip = cacheHeader.MemberAddressZip;
                    sbrs.Add(sbr);
                    provider = new ClaimProvider
                    {
                        FileID = 0,
                        ClaimID = header.ClaimID,
                        ServiceLineNumber = "0",
                        LoopName = "2330B",
                        ProviderQualifier = "PR",
                        ProviderLastName = "Inland Empire Health Plan",
                        ProviderIDQualifier = "XV",
                        ProviderID = "H5355",
                        ProviderAddress = "PO BOX 1800",
                        ProviderCity = "RANCHO CUCAMONGA",
                        ProviderState = "CA",
                        ProviderZip = "917291800",
                        RepeatSequence = "T"
                    };
                    providers.Add(provider);
                }
                else
                {
                    sbr = new ClaimSBR();
                    sbr.FileID = 0;
                    sbr.ClaimID = header.ClaimID;
                    sbr.LoopName = "2320";
                    sbr.SubscriberSequenceNumber = "P";
                    sbr.SubscriberRelationshipCode = "18";
                    sbr.ClaimFilingCode = "MC";
                    sbr.COBPayerPaidAmount = facetsLines.Where(x => x.ClaimID == header.ClaimID).Sum(x => x.DtlPaidAmt).ToString("0.##");
                    sbr.BenefitsAssignmentCertificationIndicator = "Y";
                    sbr.ReleaseOfInformationCode = cacheHeader.RelInfo;
                    sbr.LastName = cacheHeader.MemberLastName;
                    sbr.FirstName = cacheHeader.MemberFirstName;
                    sbr.MidddleName = cacheHeader.MemberMiddleInit;
                    sbr.IDQualifier = "MI";
                    sbr.IDCode = cacheHeader.MedicareId;
                    sbr.SubscriberAddress = cacheHeader.MemberAddress1;
                    sbr.SubscriberAddress2 = cacheHeader.MemberAddress2;
                    sbr.SubscriberCity = cacheHeader.MemberAddressCity;
                    sbr.SubscriberState = cacheHeader.MemberAddressState;
                    sbr.SubscriberZip = cacheHeader.MemberAddressZip;
                    sbrs.Add(sbr);
                    provider = new ClaimProvider
                    {
                        FileID = 0,
                        ClaimID = header.ClaimID,
                        ServiceLineNumber = "0",
                        LoopName = "2330B",
                        ProviderQualifier = "PR",
                        ProviderLastName = "Inland Empire Health Plan",
                        ProviderIDQualifier = "XV",
                        ProviderID = "H5355",
                        ProviderAddress = "PO BOX 1800",
                        ProviderCity = "RANCHO CUCAMONGA",
                        ProviderState = "CA",
                        ProviderZip = "917291800",
                        RepeatSequence = "P"
                    };
                    providers.Add(provider);

                }
                ClaimSecondaryIdentification sid = new ClaimSecondaryIdentification
                {
                    FileID = 0,
                    ClaimID = header.ClaimID,
                    ServiceLineNumber = "0",
                    LoopName = "2010AA",
                    ProviderQualifier = "EI",
                    ProviderID = ProviderTaxId
                };
                sids.Add(sid);

                sid = new ClaimSecondaryIdentification
                {
                    FileID = 0,
                    ClaimID = header.ClaimID,
                    ServiceLineNumber = "0",
                    LoopName = "2010BB",
                    ProviderQualifier = "2U",
                    ProviderID = "H5355"
                };
                sids.Add(sid);

                if (!string.IsNullOrEmpty(cacheHeader.MedRecNo))
                {
                    sid = new ClaimSecondaryIdentification
                    {
                        FileID = 0,
                        ClaimID = header.ClaimID,
                        ServiceLineNumber = "0",
                        LoopName = "2300",
                        ProviderQualifier = "EA",
                        ProviderID = cacheHeader.MedRecNo
                    };
                    sids.Add(sid);
                }
                int lineNumber = 1;
                decimal headerChargeAmount = 0;
                foreach (FacetsLine cacheLine in facetsLines)
                {
                    decimal lineChargeAmount = 0, linePaidAmount = 0, lineCopayAmount = 0, lineCoinsuranceAmount = 0, lineDeductAmount = 0, lineCOBPaidAmount = 0;
                    if (cacheLine.ChargeAmt.HasValue && cacheLine.ChargeAmt > 0) lineChargeAmount = cacheLine.ChargeAmt ?? 0;
                    if (cacheLine.DtlPaidAmt > 0) linePaidAmount = cacheLine.DtlPaidAmt;
                    if (cacheLine.DtlCopayAmt > 0) lineCopayAmount = cacheLine.DtlCopayAmt;
                    if (cacheLine.DtlCoInsAmt > 0) lineCoinsuranceAmount = cacheLine.DtlCoInsAmt;
                    if (cacheLine.DtlDedAmt > 0) lineDeductAmount = cacheLine.DtlDedAmt;
                    if (cacheLine.COBDtlPaidAmt > 0) lineCOBPaidAmount = cacheLine.COBDtlPaidAmt;
                    ClaimLineSVD svd = new ClaimLineSVD
                    {
                        FileID = 0,
                        ClaimID = header.ClaimID,
                        ServiceLineNumber = lineNumber.ToString(),
                        OtherPayerPrimaryIdentifier = "H5355",
                        ServiceLinePaidAmount = linePaidAmount.ToString("0.##"),
                        ServiceQualifier = "HC",
                        ProcedureCode = cacheLine.ProcCode,
                        ProcedureModifier1 = cacheLine.Mod1,
                        ProcedureModifier2 = cacheLine.Mod2,
                        ProcedureModifier3 = cacheLine.Mod3,
                        ProcedureModifier4 = cacheLine.Mod4,
                        PaidServiceUnitCount = cacheLine.Units.ToString("#.###"),
                        AdjudicationDate = cacheLine.PaidDate.ToString("yyyyMMdd")

                    };
                    lineSvds.Add(svd);
                    ClaimCAS cas;
                    if (lineChargeAmount - linePaidAmount - lineCopayAmount - lineCoinsuranceAmount - lineDeductAmount - lineCOBPaidAmount < 0)
                    {
                        lineChargeAmount = linePaidAmount + lineCopayAmount + lineCoinsuranceAmount + lineDeductAmount + lineCOBPaidAmount;
                    }
                    else if (lineChargeAmount - linePaidAmount - lineCopayAmount - lineCoinsuranceAmount - lineDeductAmount - lineCOBPaidAmount > 0)
                    {
                        cas = new ClaimCAS
                        {
                            FileID = 0,
                            ClaimID = header.ClaimID,
                            ServiceLineNumber = lineNumber.ToString(),
                            GroupCode = "CO",
                            ReasonCode = "24",
                            AdjustmentAmount = (lineChargeAmount - linePaidAmount - lineCopayAmount - lineCoinsuranceAmount - lineDeductAmount - lineCOBPaidAmount).ToString("#.##")
                        };
                        cases.Add(cas);
                    }
                    if (lineCopayAmount > 0)
                    {
                        cas = new ClaimCAS
                        {
                            FileID = 0,
                            ClaimID = header.ClaimID,
                            ServiceLineNumber = lineNumber.ToString(),
                            GroupCode = "PR",
                            ReasonCode = "3",
                            AdjustmentAmount = lineCopayAmount.ToString("#.##")
                        };
                        cases.Add(cas);
                    }
                    if (lineDeductAmount > 0)
                    {
                        cas = new ClaimCAS
                        {
                            FileID = 0,
                            ClaimID = header.ClaimID,
                            ServiceLineNumber = lineNumber.ToString(),
                            GroupCode = "PR",
                            ReasonCode = "1",
                            AdjustmentAmount = lineDeductAmount.ToString("#.##")
                        };
                        cases.Add(cas);
                    }
                    if (lineCoinsuranceAmount > 0)
                    {
                        cas = new ClaimCAS
                        {
                            FileID = 0,
                            ClaimID = header.ClaimID,
                            ServiceLineNumber = lineNumber.ToString(),
                            GroupCode = "OA",
                            ReasonCode = "2",
                            AdjustmentAmount = lineCoinsuranceAmount.ToString("#.##")
                        };
                        cases.Add(cas);
                    }
                    if (lineCOBPaidAmount > 0)
                    {
                        cas = new ClaimCAS
                        {
                            FileID = 0,
                            ClaimID = header.ClaimID,
                            ServiceLineNumber = lineNumber.ToString(),
                            GroupCode = "OA",
                            ReasonCode = "23",
                            AdjustmentAmount = lineCOBPaidAmount.ToString("#.##")
                        };
                        cases.Add(cas);
                    }
                    ServiceLine line = new ServiceLine
                    {
                        FileID = 0,
                        ClaimID = header.ClaimID,
                        ServiceLineNumber = lineNumber.ToString(),
                        ServiceIDQualifier = "HC",
                        ProcedureCode = cacheLine.ProcCode,
                        ProcedureModifier1 = cacheLine.Mod1,
                        ProcedureModifier2 = cacheLine.Mod2,
                        ProcedureModifier3 = cacheLine.Mod3,
                        ProcedureModifier4 = cacheLine.Mod4,
                        LineItemChargeAmount = lineChargeAmount.ToString("0.##"),
                        LineItemUnit = "UN",
                        ServiceUnitQuantity = cacheLine.Units.ToString("#.###"),
                        LineItemPOS = cacheLine.POS,
                        ServiceFromDate = cacheLine.DtlDosFrom.ToString("yyyyMMdd"),
                        ServiceToDate = cacheLine.DtlDosTo.ToString("yyyyMMdd"),
                        LINQualifier = "N4",
                        NationalDrugCode = cacheLine.DrugCode,
                        DrugQuantity = cacheLine.DrugUnits.ToString("#.###"),
                        DrugQualifier = "UN"
                    };
                    int diagCount = his.Count(x => x.ClaimID == header.ClaimID && (x.HIQual == "ABK" || x.HIQual == "ABF"));
                    if (diagCount == 1)
                    {
                        line.DiagPointer1 = "1";
                    }
                    else if (diagCount == 2)
                    {
                        line.DiagPointer1 = "1";
                        line.DiagPointer2 = "2";
                    }
                    else if (diagCount == 3)
                    {
                        line.DiagPointer1 = "1";
                        line.DiagPointer2 = "2";
                        line.DiagPointer3 = "3";
                    }
                    else if (diagCount > 3)
                    {
                        line.DiagPointer1 = "1";
                        line.DiagPointer2 = "2";
                        line.DiagPointer3 = "3";
                        line.DiagPointer4 = "4";
                    }
                    lines.Add(line);
                    headerChargeAmount += lineChargeAmount;
                    lineNumber++;
                }
                headers.Last().ClaimAmount = headerChargeAmount.ToString("0.##");
            }
            Common.ActualCMSI = headers.Count;
            _context.ClaimCAS.AddRange(cases);
            _context.ClaimCRCs.AddRange(crcs);
            _context.ClaimHeaders.AddRange(headers);
            _context.ClaimHIs.AddRange(his);
            _context.ClaimK3s.AddRange(k3s);
            _context.ClaimLineSVDs.AddRange(lineSvds);
            _context.ClaimNtes.AddRange(notes);
            _context.ClaimProviders.AddRange(providers);
            _context.ClaimPWKs.AddRange(pwks);
            _context.ClaimSBRs.AddRange(sbrs);
            _context.ClaimSecondaryIdentifications.AddRange(sids);
            _context.ServiceLines.AddRange(lines);
            _context.SaveChanges();
        }
    }
}
