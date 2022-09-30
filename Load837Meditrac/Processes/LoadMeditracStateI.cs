using EncDataModel.Meditrac;
using EncDataModel.Submission837;
using Load837Meditrac.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Load837Meditrac.Processes
{
    public partial class Loading
    {
        public static void LoadMeditracStateI(MeditracContext _contextMeditrac, Sub837Context _context)
        {
            List<MeditracHeader> cacheHeaders = _contextMeditrac.meditracHeaders.FromSqlRaw(string.Format(Common.QueryMeditracHeader, Common.StatusCodes, Common.GroupNumbers, Common.ClaimType, Common.StartDate, Common.EndDate, (Common.PageNumber * 10000).ToString())).ToList();
            List<MeditracLine> cacheLines = _contextMeditrac.meditracLines.FromSqlRaw(string.Format(Common.QueryMeditracLine, Common.StatusCodes, Common.GroupNumbers, Common.ClaimType, Common.StartDate, Common.EndDate, (Common.PageNumber * 10000).ToString())).ToList();
            List<MeditracCode> CacheCodes = _contextMeditrac.meditracCodes.FromSqlRaw(string.Format(Common.QueryMeditracCode, Common.StatusCodes, Common.GroupNumbers, Common.ClaimType, Common.StartDate, Common.EndDate, (Common.PageNumber * 10000).ToString())).ToList();
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
            int sequenceNumber = 0;
            foreach (MeditracHeader cacheHeader in cacheHeaders)
            {
                ClaimProvider provider;
                if (string.IsNullOrEmpty(cacheHeader.BillingLastName) && !string.IsNullOrEmpty(cacheHeader.AttLastName))
                {
                    cacheHeader.BillingLastName = cacheHeader.AttLastName;
                    cacheHeader.BillingFirstName = cacheHeader.AttFirstName;
                    cacheHeader.BillingMiddleInitial = cacheHeader.AttMiddleInitial;
                    cacheHeader.BillingNPI = cacheHeader.AttNPI;
                    cacheHeader.BillingTaxId = cacheHeader.AttTaxId;
                    cacheHeader.BillingTaxonomyCode = cacheHeader.AttTaxonomyCode;
                    cacheHeader.BillingAddress = cacheHeader.AttAddress;
                    cacheHeader.BillingAddress2 = cacheHeader.AttAddress2;
                    cacheHeader.BillingCity = cacheHeader.AttCity;
                    cacheHeader.BillingState = cacheHeader.AttState;
                    cacheHeader.BillingZip = cacheHeader.AttZip;
                }
                else if (string.IsNullOrEmpty(cacheHeader.BillingLastName) && !string.IsNullOrEmpty(cacheHeader.ReferringLastName))
                {
                    cacheHeader.BillingLastName = cacheHeader.ReferringLastName;
                    cacheHeader.BillingFirstName = cacheHeader.ReferringFirstName;
                    cacheHeader.BillingMiddleInitial = cacheHeader.ReferringMiddleInitial;
                    cacheHeader.BillingNPI = cacheHeader.ReferringNPI;
                    cacheHeader.BillingTaxId = cacheHeader.ReferringTaxId;
                    cacheHeader.BillingTaxonomyCode = cacheHeader.ReferringTaxonomyCode;
                    cacheHeader.BillingAddress = cacheHeader.ReferringAddress;
                    cacheHeader.BillingAddress2 = cacheHeader.ReferringAddress2;
                    cacheHeader.BillingCity = cacheHeader.ReferringCity;
                    cacheHeader.BillingState = cacheHeader.ReferringState;
                    cacheHeader.BillingZip = cacheHeader.ReferringZip;
                }
                if (string.IsNullOrEmpty(cacheHeader.BillingLastName))
                {
                    continue;
                }
                ClaimHeader header = new ClaimHeader();
                header.FileID = 0;
                header.ClaimID = cacheHeader.GroupNumber + DateTime.Now.ToString("yyyyMMddHH") + "I" + Common.PageNumber.ToString().PadLeft(2, '0') + sequenceNumber.ToString().PadLeft(4, '0');
                header.ClaimAmount = cacheHeader.ChargeAmount?.ToString("0.##");
                header.ClaimPOS = cacheHeader.FacilityCode;
                header.ClaimType = "A";
                header.ClaimFrequencyCode = "1";
                header.ClaimFrequencyCode = cacheHeader.ClaimFrequencyCode;
                header.ClaimProviderSignature = "Y";
                header.ClaimProviderAssignment = "A";
                header.ClaimBenefitAssignment = cacheHeader.BenefitAssignmentInd;
                header.ClaimReleaseofInformationCode = cacheHeader.ReleaseOfInformationInd;
                header.ClaimPatientSignatureSourceCode = "P";
                header.StatementFromDate = cacheHeader.StatementDateFrom;
                header.StatementToDate = cacheHeader.StatementDateTo;
                header.AdmissionDate = cacheHeader.AdmissionDate;
                header.AdmissionTypeCode = cacheHeader.AdmissionTypeCode;
                header.AdmissionSourceCode = cacheHeader.AdmissionSourceCode;
                header.PatientStatusCode = cacheHeader.PatientStatusCode;
                header.ServiceFromDate = "";
                header.ServiceToDate = "";
                header.ExportType = "IDHCS" + cacheHeader.GroupNumber;
                header.ContractTypeCode = cacheHeader.ContractTypeCode;
                if (CacheCodes.Count(x => x.ClaimId == cacheHeader.ClaimId && x.CodeType == "DRG") > 0) header.ContractTypeCode = "01";
                header.ContractCode = cacheHeader.GroupNumber;
                header.ContractAmount = cacheLines.Where(x => x.ClaimId == cacheHeader.ClaimId).Sum(y => y.LinePaidAmount ?? 0).ToString("0.##");
                headers.Add(header);

                foreach (MeditracCode code in CacheCodes.Where(x => x.ClaimId == cacheHeader.ClaimId).OrderBy(y => Tuple.Create(y.CodeType, y.Sequence)))
                {
                    ClaimHI hi = new ClaimHI();
                    hi.FileID = 0;
                    hi.ClaimID = header.ClaimID;
                    hi.HICode = code.Code.Replace(".", "");
                    switch (code.CodeType)
                    {
                        case "PRINDIAGCODE":
                            hi.HIQual = "ABK";
                            hi.PresentOnAdmissionIndicator = code.POAIndicator;
                            break;
                        case "DIAGNOSIS":
                            hi.HIQual = "ABF";
                            hi.PresentOnAdmissionIndicator = code.POAIndicator == "1" ? "Y" : code.POAIndicator;
                            break;
                        case "ADMDIAGCODE":
                            hi.HIQual = "ABJ";
                            break;
                        case "CONDITION":
                            hi.HIQual = "BG";
                            break;
                        case "DRG":
                            hi.HIQual = "DR";
                            break;
                        case "ECODE":
                            hi.HIQual = "ABN";
                            hi.PresentOnAdmissionIndicator = code.POAIndicator;
                            break;
                        case "OCCURRENCE":
                            if (code.Sequence <= 24)
                            {
                                hi.HIQual = "BI";
                                hi.HIFromDate = code.DateRecorded;
                                hi.HIToDate = code.DateThrough;
                            }
                            else
                            {
                                hi.HIQual = "BH";
                                hi.HIFromDate = code.DateRecorded;
                            }
                            break;
                        case "PATIENTREASONFORVISIT":
                            hi.HIQual = "APR";
                            break;
                        case "PRINPROCCODE":
                            hi.HIQual = "BBR";
                            hi.HIFromDate = code.DateRecorded;
                            break;
                        case "PROCEDURE":
                            hi.HIQual = "BBQ";
                            hi.HIFromDate = code.DateRecorded;
                            break;
                        case "VALUE":
                            hi.HIQual = "BE";
                            hi.HIAmount = code.Amount?.ToString("0.##");
                            break;
                    }
                    his.Add(hi);

                }

                if (!string.IsNullOrEmpty(cacheHeader.BillingLastName))
                {
                    provider = new ClaimProvider
                    {
                        FileID = 0,
                        ClaimID = header.ClaimID,
                        LoopName = "2000A",
                        ServiceLineNumber = "0",
                        ProviderQualifier = "85",
                        ProviderTaxonomyCode = cacheHeader.BillingTaxonomyCode,
                        ProviderLastName = cacheHeader.BillingLastName,
                        ProviderFirstName = cacheHeader.BillingFirstName,
                        ProviderMiddle = cacheHeader.BillingMiddleInitial,
                        ProviderIDQualifier = "XX",
                        ProviderID = cacheHeader.BillingNPI,
                        ProviderAddress = cacheHeader.BillingAddress,
                        ProviderAddress2 = cacheHeader.BillingAddress2?.Trim().Replace(":", ""),
                        ProviderCity = cacheHeader.BillingCity,
                        ProviderState = cacheHeader.BillingState,
                        ProviderZip = cacheHeader.BillingZip
                    };
                    if (provider.ProviderZip.Length == 5) provider.ProviderZip += "9998";
                    providers.Add(provider);
                }
                if (!string.IsNullOrEmpty(cacheHeader.AttLastName))
                {
                    provider = new ClaimProvider
                    {
                        FileID = 0,
                        ClaimID = header.ClaimID,
                        LoopName = "2310A",
                        ServiceLineNumber = "0",
                        ProviderQualifier = "71",
                        ProviderLastName = cacheHeader.AttLastName,
                        ProviderFirstName = cacheHeader.AttFirstName,
                        ProviderMiddle = cacheHeader.AttMiddleInitial,
                        ProviderIDQualifier = "XX",
                        ProviderID = cacheHeader.AttNPI,
                        ProviderTaxonomyCode = cacheHeader.AttTaxonomyCode
                    };
                    providers.Add(provider);
                }
                if (!string.IsNullOrEmpty(cacheHeader.OprLastName))
                {
                    provider = new ClaimProvider
                    {
                        FileID = 0,
                        ClaimID = header.ClaimID,
                        LoopName = "2310B",
                        ServiceLineNumber = "0",
                        ProviderQualifier = "72",
                        ProviderLastName = cacheHeader.OprLastName,
                        ProviderFirstName = cacheHeader.OprFirstName,
                        ProviderMiddle = cacheHeader.OprMiddleInitial,
                        ProviderIDQualifier = "XX",
                        ProviderID = cacheHeader.OprNPI
                    };
                    providers.Add(provider);
                }
                if (!string.IsNullOrEmpty(cacheHeader.OthLastName))
                {
                    provider = new ClaimProvider
                    {
                        FileID = 0,
                        ClaimID = header.ClaimID,
                        LoopName = "2310C",
                        ServiceLineNumber = "0",
                        ProviderQualifier = "ZZ",
                        ProviderLastName = cacheHeader.OthLastName,
                        ProviderFirstName = cacheHeader.OthFirstName,
                        ProviderMiddle = cacheHeader.OthMiddleInitial,
                        ProviderIDQualifier = "XX",
                        ProviderID = cacheHeader.OthNPI
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
                sbr.OtherInsuredGroupName = "CCI";
                sbr.ClaimFilingCode = "MC";
                sbr.LastName = cacheHeader.SubscriberLastName;
                sbr.FirstName = cacheHeader.SubscriberFirstName;
                sbr.MidddleName = cacheHeader.SubscriberMiddleInitial;
                sbr.IDQualifier = "MI";
                sbr.IDCode = cacheHeader.HICN?.Trim();
                sbr.SubscriberAddress = cacheHeader.SubscriberAddress;
                sbr.SubscriberAddress2 = cacheHeader.SubscriberAddress2?.Trim().Replace(":", "");
                sbr.SubscriberCity = cacheHeader.SubscriberCity;
                sbr.SubscriberState = cacheHeader.SubscriberState;
                sbr.SubscriberZip = cacheHeader.SubscriberZip;
                sbr.SubscriberBirthDate = cacheHeader.SubscriberDateOfBirth;
                sbr.SubscriberGender = cacheHeader.SubscriberGender;
                sbrs.Add(sbr);
                sbr = new ClaimSBR();
                sbr.FileID = 0;
                sbr.ClaimID = header.ClaimID;
                sbr.LoopName = "2320";
                sbr.SubscriberSequenceNumber = "P";
                sbr.SubscriberRelationshipCode = "18";
                sbr.ClaimFilingCode = "MC";
                sbr.COBPayerPaidAmount = cacheLines.Where(x => x.ClaimId == cacheHeader.ClaimId).Sum(x => x.LinePaidAmount ?? 0).ToString("0.##");
                sbr.BenefitsAssignmentCertificationIndicator = "Y";
                sbr.ReleaseOfInformationCode = cacheHeader.ReleaseOfInformationInd;
                sbr.LastName = cacheHeader.SubscriberLastName;
                sbr.FirstName = cacheHeader.SubscriberFirstName;
                sbr.MidddleName = cacheHeader.SubscriberMiddleInitial;
                sbr.IDQualifier = "MI";
                sbr.IDCode = cacheHeader.HICN?.Trim();
                sbr.SubscriberAddress = cacheHeader.SubscriberAddress;
                sbr.SubscriberAddress2 = cacheHeader.SubscriberAddress2?.Trim().Replace(":", "");
                sbr.SubscriberCity = cacheHeader.SubscriberCity;
                sbr.SubscriberState = cacheHeader.SubscriberState;
                sbr.SubscriberZip = cacheHeader.SubscriberZip;
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

                ClaimSecondaryIdentification sid = new ClaimSecondaryIdentification
                {
                    FileID = 0,
                    ClaimID = header.ClaimID,
                    ServiceLineNumber = "0",
                    LoopName = "2010AA",
                    ProviderQualifier = "EI",
                    ProviderID = cacheHeader.BillingTaxId ?? "199999997"
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

                sid = new ClaimSecondaryIdentification
                {
                    FileID = 0,
                    ClaimID = header.ClaimID,
                    ServiceLineNumber = "0",
                    LoopName = "2300",
                    ProviderQualifier = "D9",
                    ProviderID = cacheHeader.ClaimId.ToString()
                };
                sids.Add(sid);
                if (!string.IsNullOrEmpty(cacheHeader.MedicalRecordNumber))
                {
                    sid = new ClaimSecondaryIdentification
                    {
                        FileID = 0,
                        ClaimID = header.ClaimID,
                        ServiceLineNumber = "0",
                        LoopName = "2300",
                        ProviderQualifier = "EA",
                        ProviderID = cacheHeader.MedicalRecordNumber
                    };
                    sids.Add(sid);
                }
                if (!string.IsNullOrEmpty(cacheHeader.MeditracSubmissionNumber))
                {
                    sid = new ClaimSecondaryIdentification
                    {
                        FileID = 0,
                        ClaimID = header.ClaimID,
                        ServiceLineNumber = "0",
                        LoopName = "2300",
                        ProviderQualifier = "P4",
                        ProviderID = cacheHeader.MeditracSubmissionNumber
                    };
                    sids.Add(sid);
                }
                if (!string.IsNullOrEmpty(cacheHeader.AuthorizationNumber))
                {
                    sid = new ClaimSecondaryIdentification
                    {
                        FileID = 0,
                        ClaimID = header.ClaimID,
                        ServiceLineNumber = "0",
                        LoopName = "2300",
                        ProviderQualifier = "G1",
                        ProviderID = cacheHeader.AuthorizationNumber
                    };
                    sids.Add(sid);
                }
                string countyCode = cacheHeader.GroupNumber == "810" ? "33" : "36";
                ClaimNte note = new ClaimNte
                {
                    FileID = 0,
                    ClaimID = header.ClaimID,
                    ServiceLineNumber = "0",
                    NoteCode = "ADD",
                    NoteText = cacheHeader.CIN + ";" + cacheHeader.MedicaidID + ";" + countyCode + ";"
                };
                notes.Add(note);
                decimal headerChargeAmount = 0;
                foreach (MeditracLine cacheLine in cacheLines.Where(x => x.ClaimId == cacheHeader.ClaimId).OrderBy(x => x.LineNumber))
                {
                    decimal lineChargeAmount = 0, linePaidAmount = 0, lineCopayAmount = 0, lineCoinsuranceAmount = 0, lineDeductAmount = 0, lineCOBPaidAmount = 0;
                    if (cacheLine.LineChargeAmount.HasValue && cacheLine.LineChargeAmount > 0) lineChargeAmount = cacheLine.LineChargeAmount ?? 0;
                    if (cacheLine.LinePaidAmount.HasValue && cacheLine.LinePaidAmount > 0) linePaidAmount = cacheLine.LinePaidAmount ?? 0;
                    if (cacheLine.LineCopayAmount.HasValue && cacheLine.LineCopayAmount > 0) lineCopayAmount = cacheLine.LineCopayAmount ?? 0;
                    if (cacheLine.LineCoinsuranceAmount.HasValue && cacheLine.LineCoinsuranceAmount > 0) lineCoinsuranceAmount = cacheLine.LineCoinsuranceAmount ?? 0;
                    if (cacheLine.LineDeductAmount.HasValue && cacheLine.LineDeductAmount > 0) lineDeductAmount = cacheLine.LineDeductAmount ?? 0;
                    if (cacheLine.LineCOBPaidAmount.HasValue && cacheLine.LineCOBPaidAmount > 0) lineCOBPaidAmount = cacheLine.LineCOBPaidAmount ?? 0;
                    ClaimLineSVD svd = new ClaimLineSVD
                    {
                        FileID = 0,
                        ClaimID = header.ClaimID,
                        ServiceLineNumber = cacheLine.LineNumber.ToString(),
                        OtherPayerPrimaryIdentifier = "H5355",
                        ServiceLinePaidAmount = linePaidAmount.ToString("0.##"),
                        ServiceQualifier = cacheLine.RevenueCode.PadLeft(4, '0').Substring(0, 3) == "002" ? "HP" : "HC",
                        ProcedureCode = cacheLine.ProcedureCode,
                        ProcedureModifier1 = cacheLine.Modifier1,
                        ProcedureModifier2 = cacheLine.Modifier2,
                        ProcedureModifier3 = cacheLine.Modifier3,
                        ProcedureModifier4 = cacheLine.Modifier4,
                        PaidServiceUnitCount = cacheLine.Quantity?.ToString("#.###"),
                        AdjudicationDate = cacheLine.PaidDate,
                        ServiceLineRevenueCode = cacheLine.RevenueCode.PadLeft(4, '0')

                    };
                    if (string.IsNullOrEmpty(svd.ProcedureCode)) svd.ServiceQualifier = "";
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
                            ServiceLineNumber = cacheLine.LineNumber.ToString(),
                            GroupCode = "CO",
                            ReasonCode = "45",
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
                            ServiceLineNumber = cacheLine.LineNumber.ToString(),
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
                            ServiceLineNumber = cacheLine.LineNumber.ToString(),
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
                            ServiceLineNumber = cacheLine.LineNumber.ToString(),
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
                            ServiceLineNumber = cacheLine.LineNumber.ToString(),
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
                        ServiceLineNumber = cacheLine.LineNumber.ToString(),
                        ServiceIDQualifier = "HC",
                        ProcedureCode = cacheLine.ProcedureCode,
                        ProcedureModifier1 = cacheLine.Modifier1,
                        ProcedureModifier2 = cacheLine.Modifier2,
                        ProcedureModifier3 = cacheLine.Modifier3,
                        ProcedureModifier4 = cacheLine.Modifier4,
                        LineItemChargeAmount = lineChargeAmount.ToString("0.##"),
                        LineItemUnit = cacheLine.UnitOfMeasure,
                        ServiceUnitQuantity = cacheLine.Quantity?.ToString("#.###"),
                        LineItemPOS = cacheLine.PlaceOfService,
                        EPSDTIndicator = cacheLine.EPSDTInd,
                        ServiceFromDate = cacheLine.ServiceDateFrom,
                        ServiceToDate = cacheLine.ServiceDateTo,
                        LINQualifier = "N4",
                        NationalDrugCode = cacheLine.NationalDrugCode,
                        DrugQuantity = cacheLine.DrugQuantity?.ToString("#.###"),
                        DrugQualifier = cacheLine.DrugUnit,
                        RevenueCode = cacheLine.RevenueCode
                    };
                    if (string.IsNullOrEmpty(line.ProcedureCode)) line.ServiceIDQualifier = "";
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
                }
                headers.Last().ClaimAmount = headerChargeAmount.ToString("0.##");
                sequenceNumber++;
            }
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
