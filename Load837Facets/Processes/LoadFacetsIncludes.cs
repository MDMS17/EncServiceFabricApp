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
        public static void LoadIncludes(FacetsContext _contextFacets, Sub837Context _context) 
        {
            _contextFacets.Database.SetConnectionString(Common.cn_Facets);
            _context.Database.SetConnectionString(Common.cn);
            int counter = 0;
            foreach (string claimId in Common.Includes) 
            {
                FacetsHeader cacheHeader = _contextFacets.FacetsHeaders.FromSqlRaw(string.Format(Common.FacetsHeaderSingle, claimId)).FirstOrDefault();
                if (cacheHeader is null) return;
                List<FacetsLine> cacheLines = _contextFacets.FacetsLines.FromSqlRaw(string.Format(Common.FacetsLineSingle, claimId)).ToList();
                List<FacetsCode> CacheCodes = _contextFacets.FacetsCodes.FromSqlRaw(string.Format(Common.FacetsCodeSingle, claimId)).ToList();
                List<FacetsProvider> cacheProviders = _contextFacets.FacetsProviders.FromSqlRaw(string.Format(Common.FacetsProviderSingle, claimId)).ToList();
                List<FacetsExtraSbr> cacheExtraSbrs = _contextFacets.FacetsExtraSbrs.FromSqlRaw(string.Format(Common.FacetsExtraSbrSingle, claimId)).ToList();
                List<FacetsExtraSvd> CacheExtraSvds = _contextFacets.FacetsExtraSvds.FromSqlRaw(string.Format(Common.FacetsExtraSvdSingle, claimId)).ToList();
                ClaimHeader header = new ClaimHeader();
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
                ClaimProvider provider;
                string ProviderTaxId = "";
                if (cacheHeader.ClaimType == "P")
                {
                    //profesion
                    //check procedure codes, if DME, clone
                    if (cacheLines.Where(x => x.ClaimID == cacheHeader.ClaimID).Count(x => Common.DMEProcCodes.Contains(x.ProcCode)) > 0)
                    {
                        FacetsHeader DMEHeader = cacheHeader;
                        List<FacetsLine> DMELines = cacheLines.Where(x => x.ClaimID == cacheHeader.ClaimID).Where(x => Common.DMEProcCodes.Contains(x.ProcCode)).ToList();
                        ClaimHeader dmeHeader = new ClaimHeader();
                        dmeHeader.FileID = 0;
                        dmeHeader.ClaimID = cacheHeader.ClaimID;
                        dmeHeader.ClaimAmount = DMEHeader.HeaderChargeAmt.ToString("0.##");
                        dmeHeader.ClaimPOS = DMEHeader.FacType + DMEHeader.BillClass;
                        dmeHeader.ClaimType = "B";
                        dmeHeader.ClaimFrequencyCode = DMEHeader.Frequency;
                        dmeHeader.ClaimProviderSignature = "Y";
                        dmeHeader.ClaimProviderAssignment = "A";
                        dmeHeader.ClaimBenefitAssignment = cacheHeader.BenAssigned == "P" ? "Y" : "N";
                        dmeHeader.ClaimReleaseofInformationCode = DMEHeader.RelInfo;
                        if (DMEHeader.RelInfo == "S" || DMEHeader.RelInfo == "C") dmeHeader.ClaimReleaseofInformationCode = "Y";
                        else dmeHeader.ClaimReleaseofInformationCode = "I";

                        dmeHeader.AdmissionDate = DMEHeader.AdmitDte?.ToString("yyyyMMdd");
                        dmeHeader.PatientPaidAmount = DMEHeader.HeaderPatPaidAmt.ToString("#.##");
                        dmeHeader.ExportType = "E" + DMEHeader.PlanID;

                        dmeHeader.ContractCode = null;
                        dmeHeader.ContractAmount = DMELines.Where(x => x.ClaimID == cacheHeader.ClaimID).Sum(x => x.DtlPaidAmt).ToString("0.##");
                        headers.Add(dmeHeader);

                        foreach (FacetsCode code in CacheCodes.Where(x => x.ClaimID == DMEHeader.ClaimID).OrderBy(x => Tuple.Create(x.CodeType, x.CodeSequence)))
                        {
                            ClaimHI hi = new ClaimHI();
                            hi.FileID = 0;
                            hi.ClaimID = dmeHeader.ClaimID;
                            hi.HICode = code.CodeValue.Replace(".", "");
                            switch (code.CodeType)
                            {
                                case "DIAG":
                                    hi.HIQual = code.CodeSequence == "1" ? "ABK" : "ABF";
                                    hi.PresentOnAdmissionIndicator = code.DIAGPOI;
                                    break;
                                case "COND":
                                    hi.HIQual = "BG";
                                    break;
                                case "PP":
                                    hi.HIQual = "BBR";
                                    hi.HIFromDate = code.CodeStartDate?.ToString("yyyyMMdd");
                                    break;
                                case "OPI":
                                    hi.HIQual = "BBQ";
                                    hi.HIFromDate = code.CodeStartDate?.ToString("yyyyMMdd");
                                    break;
                            }
                            his.Add(hi);
                        }
                        if (his.Count(x => x.ClaimID == dmeHeader.ClaimID && x.HIQual == "ABK") == 0 && his.Count(x => x.ClaimID == dmeHeader.ClaimID && x.HIQual == "ABF") > 0)
                        {
                            his.FirstOrDefault(x => x.ClaimID == dmeHeader.ClaimID && x.HIQual == "ABF").HIQual = "ABK";
                        }
                        FacetsProvider facetsProvider = cacheProviders.FirstOrDefault(x => x.ClaimID == DMEHeader.ClaimID && x.ProviderType == "Billing");
                        if (facetsProvider != null)
                        {
                            provider = new ClaimProvider
                            {
                                FileID = 0,
                                ClaimID = dmeHeader.ClaimID,
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
                        facetsProvider = cacheProviders.FirstOrDefault(x => x.ClaimID == DMEHeader.ClaimID && x.ProviderType == "Referring");
                        if (facetsProvider != null)
                        {
                            provider = new ClaimProvider
                            {
                                FileID = 0,
                                ClaimID = dmeHeader.ClaimID,
                                LoopName = "2310A",
                                ServiceLineNumber = "0",
                                ProviderQualifier = "DN",
                                ProviderLastName = facetsProvider.ProviderLastName,
                                ProviderFirstName = facetsProvider.ProviderFirstName,
                                ProviderIDQualifier = "XX",
                                ProviderID = facetsProvider.ProviderNPI
                            };
                            providers.Add(provider);
                        }
                        facetsProvider = cacheProviders.FirstOrDefault(x => x.ClaimID == DMEHeader.ClaimID && x.ProviderType == "Rendering");
                        if (facetsProvider != null)
                        {
                            provider = new ClaimProvider
                            {
                                FileID = 0,
                                ClaimID = dmeHeader.ClaimID,
                                LoopName = "2310B",
                                ServiceLineNumber = "0",
                                ProviderQualifier = "82",
                                ProviderLastName = facetsProvider.ProviderLastName,
                                ProviderFirstName = facetsProvider.ProviderFirstName,
                                ProviderIDQualifier = "XX",
                                ProviderID = facetsProvider.ProviderNPI,
                                ProviderTaxonomyCode = facetsProvider.ProviderTaxonomy
                            };
                            providers.Add(provider);
                        }
                        provider = new ClaimProvider
                        {
                            FileID = 0,
                            ClaimID = dmeHeader.ClaimID,
                            LoopName = "2010BB",
                            ServiceLineNumber = "0",
                            ProviderQualifier = "PR",
                            ProviderLastName = "MMEDSCMS",
                            ProviderIDQualifier = "PI",
                            ProviderID = "80892",
                            ProviderAddress = "7500 Security Blvd",
                            ProviderCity = "Baltimore",
                            ProviderState = "MD",
                            ProviderZip = "212441850"
                        };
                        providers.Add(provider);

                        ClaimSBR dmeSbr = new ClaimSBR();
                        dmeSbr.FileID = 0;
                        dmeSbr.ClaimID = dmeHeader.ClaimID;
                        dmeSbr.LoopName = "2000B";
                        dmeSbr.SubscriberSequenceNumber = "S";
                        dmeSbr.SubscriberRelationshipCode = "18";
                        dmeSbr.ClaimFilingCode = "MC";
                        dmeSbr.LastName = DMEHeader.MemberLastName;
                        dmeSbr.FirstName = DMEHeader.MemberFirstName;
                        dmeSbr.MidddleName = DMEHeader.MemberMiddleInit;
                        dmeSbr.IDQualifier = "MI";
                        dmeSbr.IDCode = DMEHeader.MedicareId;
                        dmeSbr.SubscriberAddress = DMEHeader.MemberAddress1;
                        dmeSbr.SubscriberAddress2 = DMEHeader.MemberAddress2;
                        dmeSbr.SubscriberCity = DMEHeader.MemberAddressCity;
                        dmeSbr.SubscriberState = DMEHeader.MemberAddressState;
                        dmeSbr.SubscriberZip = DMEHeader.MemberAddressZip;
                        dmeSbr.SubscriberBirthDate = DMEHeader.MemberBirthDate.ToString("yyyyMMdd");
                        dmeSbr.SubscriberGender = DMEHeader.MemberSex;
                        sbrs.Add(dmeSbr);
                        FacetsExtraSbr facetsSbr = cacheExtraSbrs.FirstOrDefault(x => x.ClaimID == DMEHeader.ClaimID);
                        if (facetsSbr != null)
                        {
                            dmeSbr = new ClaimSBR();
                            dmeSbr.FileID = 0;
                            dmeSbr.ClaimID = dmeHeader.ClaimID;
                            dmeSbr.LoopName = "2320";
                            dmeSbr.SubscriberSequenceNumber = "P";
                            dmeSbr.SubscriberRelationshipCode = "18";
                            dmeSbr.ClaimFilingCode = facetsSbr.ClaimFilingIndicatorCode;
                            dmeSbr.COBPayerPaidAmount = DMELines.Where(x => x.ClaimID == DMEHeader.ClaimID).Sum(x => x.DtlPaidAmt).ToString("0.##");
                            dmeSbr.BenefitsAssignmentCertificationIndicator = "Y";
                            dmeSbr.ReleaseOfInformationCode = DMEHeader.RelInfo;
                            dmeSbr.LastName = DMEHeader.MemberLastName;
                            dmeSbr.FirstName = DMEHeader.MemberFirstName;
                            dmeSbr.MidddleName = DMEHeader.MemberMiddleInit;
                            dmeSbr.IDQualifier = "MI";
                            dmeSbr.IDCode = DMEHeader.MedicareId;
                            dmeSbr.SubscriberAddress = DMEHeader.MemberAddress1;
                            dmeSbr.SubscriberAddress2 = DMEHeader.MemberAddress2;
                            dmeSbr.SubscriberCity = DMEHeader.MemberAddressCity;
                            dmeSbr.SubscriberState = DMEHeader.MemberAddressState;
                            dmeSbr.SubscriberZip = DMEHeader.MemberAddressZip;
                            sbrs.Add(dmeSbr);
                            provider = new ClaimProvider
                            {
                                ClaimID = DMEHeader.ClaimID,
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
                            dmeSbr = new ClaimSBR();
                            dmeSbr.FileID = 0;
                            dmeSbr.ClaimID = dmeHeader.ClaimID;
                            dmeSbr.LoopName = "2320";
                            dmeSbr.SubscriberSequenceNumber = "T";
                            dmeSbr.SubscriberRelationshipCode = "18";
                            dmeSbr.ClaimFilingCode = "MC";
                            dmeSbr.COBPayerPaidAmount = "0.00";
                            dmeSbr.BenefitsAssignmentCertificationIndicator = "Y";
                            dmeSbr.ReleaseOfInformationCode = DMEHeader.RelInfo;
                            dmeSbr.LastName = DMEHeader.MemberLastName;
                            dmeSbr.FirstName = DMEHeader.MemberFirstName;
                            dmeSbr.MidddleName = DMEHeader.MemberMiddleInit;
                            dmeSbr.IDQualifier = "MI";
                            dmeSbr.IDCode = DMEHeader.MedicareId;
                            dmeSbr.SubscriberAddress = DMEHeader.MemberAddress1;
                            dmeSbr.SubscriberAddress2 = DMEHeader.MemberAddress2;
                            dmeSbr.SubscriberCity = DMEHeader.MemberAddressCity;
                            dmeSbr.SubscriberState = DMEHeader.MemberAddressState;
                            dmeSbr.SubscriberZip = DMEHeader.MemberAddressZip;
                            sbrs.Add(dmeSbr);
                            provider = new ClaimProvider
                            {
                                FileID = 0,
                                ClaimID = dmeHeader.ClaimID,
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
                            dmeSbr = new ClaimSBR();
                            dmeSbr.FileID = 0;
                            dmeSbr.ClaimID = dmeHeader.ClaimID;
                            dmeSbr.LoopName = "2320";
                            dmeSbr.SubscriberSequenceNumber = "P";
                            dmeSbr.SubscriberRelationshipCode = "18";
                            dmeSbr.ClaimFilingCode = "MC";
                            dmeSbr.COBPayerPaidAmount = DMELines.Where(x => x.ClaimID == DMEHeader.ClaimID).Sum(x => x.DtlPaidAmt).ToString("0.##");
                            dmeSbr.BenefitsAssignmentCertificationIndicator = "Y";
                            dmeSbr.ReleaseOfInformationCode = DMEHeader.RelInfo;
                            dmeSbr.LastName = DMEHeader.MemberLastName;
                            dmeSbr.FirstName = DMEHeader.MemberFirstName;
                            dmeSbr.MidddleName = DMEHeader.MemberMiddleInit;
                            dmeSbr.IDQualifier = "MI";
                            dmeSbr.IDCode = DMEHeader.MedicareId;
                            dmeSbr.SubscriberAddress = DMEHeader.MemberAddress1;
                            dmeSbr.SubscriberAddress2 = DMEHeader.MemberAddress2;
                            dmeSbr.SubscriberCity = DMEHeader.MemberAddressCity;
                            dmeSbr.SubscriberState = DMEHeader.MemberAddressState;
                            dmeSbr.SubscriberZip = DMEHeader.MemberAddressZip;
                            sbrs.Add(dmeSbr);
                            provider = new ClaimProvider
                            {
                                FileID = 0,
                                ClaimID = dmeHeader.ClaimID,
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
                        ClaimSecondaryIdentification dmeSid = new ClaimSecondaryIdentification
                        {
                            FileID = 0,
                            ClaimID = dmeHeader.ClaimID,
                            ServiceLineNumber = "0",
                            LoopName = "2010AA",
                            ProviderQualifier = "EI",
                            ProviderID = ProviderTaxId
                        };
                        sids.Add(dmeSid);

                        dmeSid = new ClaimSecondaryIdentification
                        {
                            FileID = 0,
                            ClaimID = dmeHeader.ClaimID,
                            ServiceLineNumber = "0",
                            LoopName = "2010BB",
                            ProviderQualifier = "2U",
                            ProviderID = "H5355"
                        };
                        sids.Add(dmeSid);

                        if (!string.IsNullOrEmpty(DMEHeader.MedRecNo))
                        {
                            dmeSid = new ClaimSecondaryIdentification
                            {
                                FileID = 0,
                                ClaimID = dmeHeader.ClaimID,
                                ServiceLineNumber = "0",
                                LoopName = "2300",
                                ProviderQualifier = "EA",
                                ProviderID = DMEHeader.MedRecNo
                            };
                            sids.Add(dmeSid);
                        }
                        int dmeLineNumber = 1;
                        decimal dmeHeaderChargeAmount = 0;
                        foreach (FacetsLine dmeLine in DMELines.OrderBy(x => x.ClaimSeqNbr))
                        {
                            decimal lineChargeAmount = 0, linePaidAmount = 0, lineCopayAmount = 0, lineCoinsuranceAmount = 0, lineDeductAmount = 0, lineCOBPaidAmount = 0;
                            if (dmeLine.ChargeAmt.HasValue && dmeLine.ChargeAmt > 0) lineChargeAmount = dmeLine.ChargeAmt ?? 0;
                            if (dmeLine.DtlPaidAmt > 0) linePaidAmount = dmeLine.DtlPaidAmt;
                            if (dmeLine.DtlCopayAmt > 0) lineCopayAmount = dmeLine.DtlCopayAmt;
                            if (dmeLine.DtlCoInsAmt > 0) lineCoinsuranceAmount = dmeLine.DtlCoInsAmt;
                            if (dmeLine.DtlDedAmt > 0) lineDeductAmount = dmeLine.DtlDedAmt;
                            if (dmeLine.COBDtlPaidAmt > 0) lineCOBPaidAmount = dmeLine.COBDtlPaidAmt;
                            //FacetsExtraSvd facetsExtraSvd = CacheExtraSvds.FirstOrDefault(x => x.ClaimID == dmeLine.ClaimID&&x.ClaimLineSeq==dmeLine.ClaimSeqNbr);
                            //if (facetsExtraSvd != null)
                            //{
                            //    ClaimLineSVD svd = new ClaimLineSVD
                            //    {
                            //        FileID = 0,
                            //        ClaimID = dmeLine.ClaimID,
                            //        ServiceLineNumber = dmeLineNumber.ToString(),
                            //        OtherPayerPrimaryIdentifier = facetsExtraSvd.COBPayerID,
                            //        ServiceLinePaidAmount = (facetsExtraSvd.COBDtlPaidAmt??0).ToString("0.##"),
                            //        ServiceQualifier = "HC",
                            //        ProcedureCode = dmeLine.ProcCode,
                            //        ProcedureModifier1 = dmeLine.Mod1,
                            //        ProcedureModifier2 = dmeLine.Mod2,
                            //        ProcedureModifier3 = dmeLine.Mod3,
                            //        ProcedureModifier4 = dmeLine.Mod4,
                            //        PaidServiceUnitCount = dmeLine.Units.ToString("#.###"),
                            //        AdjudicationDate = dmeLine.PaidDate.ToString("yyyyMMdd"),
                            //        RepeatSequence="P"
                            //    };
                            //    lineSvds.Add(svd);
                            //    ClaimCAS cas;
                            //    if ((facetsExtraSvd.COBDtlCoPayAmt??0) > 0)
                            //    {
                            //        cas = new ClaimCAS
                            //        {
                            //            FileID = 0,
                            //            ClaimID = dmeHeader.ClaimID,
                            //            ServiceLineNumber = dmeLineNumber.ToString(),
                            //            GroupCode = "PR",
                            //            ReasonCode = "3",
                            //            AdjustmentAmount = facetsExtraSvd.COBDtlCoPayAmt?.ToString("#.##"),

                            //        };
                            //        cases.Add(cas);
                            //    }
                            //    if (lineDeductAmount > 0)
                            //    {
                            //        cas = new ClaimCAS
                            //        {
                            //            FileID = 0,
                            //            ClaimID = dmeHeader.ClaimID,
                            //            ServiceLineNumber = dmeLineNumber.ToString(),
                            //            GroupCode = "PR",
                            //            ReasonCode = "1",
                            //            AdjustmentAmount = lineDeductAmount.ToString("#.##")
                            //        };
                            //        cases.Add(cas);
                            //    }
                            //    if (lineCoinsuranceAmount > 0)
                            //    {
                            //        cas = new ClaimCAS
                            //        {
                            //            FileID = 0,
                            //            ClaimID = dmeHeader.ClaimID,
                            //            ServiceLineNumber = dmeLineNumber.ToString(),
                            //            GroupCode = "OA",
                            //            ReasonCode = "2",
                            //            AdjustmentAmount = lineCoinsuranceAmount.ToString("#.##")
                            //        };
                            //        cases.Add(cas);
                            //    }
                            //    if (lineCOBPaidAmount > 0)
                            //    {
                            //        cas = new ClaimCAS
                            //        {
                            //            FileID = 0,
                            //            ClaimID = dmeHeader.ClaimID,
                            //            ServiceLineNumber = dmeLineNumber.ToString(),
                            //            GroupCode = "OA",
                            //            ReasonCode = "23",
                            //            AdjustmentAmount = lineCOBPaidAmount.ToString("#.##")
                            //        };
                            //        cases.Add(cas);
                            //    }

                            //}
                            //else 
                            //{
                            //}
                            ClaimLineSVD svd = new ClaimLineSVD
                            {
                                FileID = 0,
                                ClaimID = dmeHeader.ClaimID,
                                ServiceLineNumber = dmeLineNumber.ToString(),
                                OtherPayerPrimaryIdentifier = "H5355",
                                ServiceLinePaidAmount = linePaidAmount.ToString("0.##"),
                                ServiceQualifier = "HC",
                                ProcedureCode = dmeLine.ProcCode,
                                ProcedureModifier1 = dmeLine.Mod1,
                                ProcedureModifier2 = dmeLine.Mod2,
                                ProcedureModifier3 = dmeLine.Mod3,
                                ProcedureModifier4 = dmeLine.Mod4,
                                PaidServiceUnitCount = dmeLine.Units.ToString("#.###"),
                                AdjudicationDate = dmeLine.PaidDate.ToString("yyyyMMdd")
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
                                    ClaimID = dmeHeader.ClaimID,
                                    ServiceLineNumber = dmeLineNumber.ToString(),
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
                                    ClaimID = dmeHeader.ClaimID,
                                    ServiceLineNumber = dmeLineNumber.ToString(),
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
                                    ClaimID = dmeHeader.ClaimID,
                                    ServiceLineNumber = dmeLineNumber.ToString(),
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
                                    ClaimID = dmeHeader.ClaimID,
                                    ServiceLineNumber = dmeLineNumber.ToString(),
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
                                    ClaimID = dmeHeader.ClaimID,
                                    ServiceLineNumber = dmeLineNumber.ToString(),
                                    GroupCode = "OA",
                                    ReasonCode = "23",
                                    AdjustmentAmount = lineCOBPaidAmount.ToString("#.##")
                                };
                                cases.Add(cas);
                            }
                            ServiceLine line = new ServiceLine
                            {
                                FileID = 0,
                                ClaimID = dmeHeader.ClaimID,
                                ServiceLineNumber = dmeLineNumber.ToString(),
                                ServiceIDQualifier = "HC",
                                ProcedureCode = dmeLine.ProcCode,
                                ProcedureModifier1 = dmeLine.Mod1,
                                ProcedureModifier2 = dmeLine.Mod2,
                                ProcedureModifier3 = dmeLine.Mod3,
                                ProcedureModifier4 = dmeLine.Mod4,
                                LineItemChargeAmount = lineChargeAmount.ToString("0.##"),
                                LineItemUnit = "UN",
                                ServiceUnitQuantity = dmeLine.Units.ToString("#.###"),
                                LineItemPOS = dmeLine.POS,
                                ServiceFromDate = dmeLine.DtlDosFrom.ToString("yyyyMMdd"),
                                ServiceToDate = dmeLine.DtlDosTo.ToString("yyyyMMdd"),
                                LINQualifier = "N4",
                                NationalDrugCode = dmeLine.DrugCode,
                                DrugQuantity = dmeLine.DrugUnits.ToString("#.###"),
                                DrugQualifier = "UN"
                            };
                            int diagCount = his.Count(x => x.ClaimID == dmeHeader.ClaimID && (x.HIQual == "ABK" || x.HIQual == "ABF"));
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
                            dmeLineNumber++;
                            dmeHeaderChargeAmount += lineChargeAmount;
                        }
                        headers.Last().ClaimAmount = dmeHeaderChargeAmount.ToString("0.##");

                    }
                    if (cacheLines.Where(x => x.ClaimID == cacheHeader.ClaimID).Count(x => !Common.DMEProcCodes.Contains(x.ProcCode)) > 0)
                    {
                        List<FacetsLine> facetsLines = cacheLines.Where(x => x.ClaimID == cacheHeader.ClaimID).Where(x => !Common.DMEProcCodes.Contains(x.ProcCode)).ToList();
                        header.FileID = 0;
                        header.ClaimID = cacheHeader.ClaimID;
                        header.ClaimAmount = cacheHeader.HeaderChargeAmt.ToString("0.##");
                        header.ClaimPOS = cacheHeader.FacType + cacheHeader.BillClass;
                        header.ClaimType = "B";
                        header.ClaimFrequencyCode = cacheHeader.Frequency;
                        header.ClaimProviderSignature = "Y";
                        header.ClaimProviderAssignment = "A";
                        header.ClaimBenefitAssignment = cacheHeader.BenAssigned;
                        header.ClaimReleaseofInformationCode = cacheHeader.RelInfo;
                        header.AdmissionDate = cacheHeader.AdmitDte?.ToString("yyyyMMdd");
                        header.PatientPaidAmount = cacheHeader.HeaderPatPaidAmt.ToString("#.##");
                        header.ExportType = "P" + cacheHeader.PlanID;
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
                                    hi.HIQual = code.CodeSequence == "1" ? "ABK" : "ABF";
                                    hi.PresentOnAdmissionIndicator = code.DIAGPOI;
                                    break;
                                case "COND":
                                    hi.HIQual = "BG";
                                    break;
                                case "PP":
                                    hi.HIQual = "BBR";
                                    hi.HIFromDate = code.CodeStartDate?.ToString("yyyyMMdd");
                                    break;
                                case "OPI":
                                    hi.HIQual = "BBQ";
                                    hi.HIFromDate = code.CodeStartDate?.ToString("yyyyMMdd");
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
                        facetsProvider = cacheProviders.FirstOrDefault(x => x.ClaimID == cacheHeader.ClaimID && x.ProviderType == "Refering");
                        if (facetsProvider != null)
                        {
                            provider = new ClaimProvider
                            {
                                FileID = 0,
                                ClaimID = header.ClaimID,
                                LoopName = "2310A",
                                ServiceLineNumber = "0",
                                ProviderQualifier = "DN",
                                ProviderLastName = facetsProvider.ProviderLastName,
                                ProviderFirstName = facetsProvider.ProviderFirstName,
                                ProviderIDQualifier = "XX",
                                ProviderID = facetsProvider.ProviderNPI
                            };
                            providers.Add(provider);
                        }
                        facetsProvider = cacheProviders.FirstOrDefault(x => x.ClaimID == cacheHeader.ClaimID && x.ProviderType == "Rendering");
                        if (facetsProvider != null)
                        {
                            provider = new ClaimProvider
                            {
                                FileID = 0,
                                ClaimID = header.ClaimID,
                                LoopName = "2310B",
                                ServiceLineNumber = "0",
                                ProviderQualifier = "82",
                                ProviderLastName = facetsProvider.ProviderLastName,
                                ProviderFirstName = facetsProvider.ProviderFirstName,
                                ProviderIDQualifier = "XX",
                                ProviderID = facetsProvider.ProviderNPI,
                                ProviderTaxonomyCode = facetsProvider.ProviderTaxonomy
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
                            ProviderID = "80892",
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
                }
                else
                {
                    //institutional
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
                            ProviderTaxonomyCode = facetsProvider.ProviderTaxonomy
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
                        sbr.COBPayerPaidAmount = cacheLines.Where(x => x.ClaimID == header.ClaimID).Sum(x => x.DtlPaidAmt).ToString("0.##");
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
                        sbr.COBPayerPaidAmount = cacheLines.Where(x => x.ClaimID == header.ClaimID).Sum(x => x.DtlPaidAmt).ToString("0.##");
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
                    foreach (FacetsLine cacheLine in cacheLines)
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
                _context.ClaimHeaders.Add(header);
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
                counter++;
                if (counter >= 1000) 
                {
                    _context.SaveChanges();
                    counter = 0;
                }
            }
            _context.SaveChanges();
        }
    }
}
