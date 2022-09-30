using EncDataModel.Submission837;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Load837WPC.Processes
{
    public partial class ProcessHipaaXml
    {
        public static void Process837I(ref string ClaimId, ref Claim claim, ref string xmlDocument) 
        {
            XDocument xdoc = XDocument.Parse(xmlDocument);
            claim.Header.ClaimID = ClaimId;
            claim.Header.FileID = 0;
            XNamespace ns = "http://schemas.microsoft.com/BizTalk/EDI/X12/2006";
            ClaimProvider provider = new ClaimProvider();
            provider.ClaimID = ClaimId;
            provider.FileID = 0;
            provider.ServiceLineNumber = "0";
            provider.LoopName = "2000A";
            XElement loop2000A = xdoc.Descendants(ns + "TS837_2000A_Loop").FirstOrDefault();
            XElement BillProvPRV = loop2000A.Descendants(ns + "PRV_BillingProviderSpecialtyInformation").FirstOrDefault();
            if (BillProvPRV != null)
            {
                foreach (XElement ele in BillProvPRV.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("PRV03"))
                    {
                        provider.ProviderTaxonomyCode = ele.Value;
                    }
                }
            }
            XElement loop2010AA = loop2000A.Descendants(ns + "NM1_SubLoop_2").FirstOrDefault().Descendants(ns + "TS837_2010AA_Loop").FirstOrDefault();
            XElement BillProvNM1 = loop2010AA.Descendants(ns + "NM1_BillingProviderName").FirstOrDefault();
            foreach (XElement ele in BillProvNM1.Descendants())
            {
                if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                if (ele.Name.ToString().StartsWith("NM104")) provider.ProviderFirstName = ele.Value;
                if (ele.Name.ToString().StartsWith("NM105")) provider.ProviderMiddle = ele.Value;
                if (ele.Name.ToString().StartsWith("NM107")) provider.ProviderSuffix = ele.Value;
                if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
            }
            XElement BillProvN3 = loop2010AA.Descendants(ns + "N3_BillingProviderAddress").FirstOrDefault();
            foreach (XElement ele in BillProvN3.Descendants())
            {
                if (ele.Name.ToString().StartsWith("N301")) provider.ProviderAddress = ele.Value;
                if (ele.Name.ToString().StartsWith("N302")) provider.ProviderAddress2 = ele.Value;
            }
            XElement BillProvN4 = loop2010AA.Descendants(ns + "N4_BillingProviderCity_State_ZIPCode").FirstOrDefault();
            foreach (XElement ele in BillProvN4.Descendants())
            {
                if (ele.Name.ToString().StartsWith("N401")) provider.ProviderCity = ele.Value;
                if (ele.Name.ToString().StartsWith("N402")) provider.ProviderState = ele.Value;
                if (ele.Name.ToString().StartsWith("N403")) provider.ProviderZip = ele.Value;
                if (ele.Name.ToString().StartsWith("N404")) provider.ProviderCountry = ele.Value;
                if (ele.Name.ToString().StartsWith("N407")) provider.ProviderCountrySubCode = ele.Value;
            }
            claim.Providers.Add(provider);
            XElement BillingProviderTaxIdentification = loop2010AA.Descendants(ns + "REF_BillingProviderTaxIdentification").FirstOrDefault();
            if (BillingProviderTaxIdentification != null)
            {
                ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                si.ClaimID = ClaimId;
                si.FileID = 0;
                si.ServiceLineNumber = "0";
                si.LoopName = "2010AA";
                foreach (XElement ele in BillingProviderTaxIdentification.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("REF01")) si.ProviderQualifier = ele.Value;
                    if (ele.Name.ToString().StartsWith("REF02")) si.ProviderID = ele.Value;
                }
                claim.SecondaryIdentifications.Add(si);
            }
            foreach (XElement BillProvPER in loop2010AA.Descendants(ns + "PER_BillingProviderContactInformation"))
            {
                ProviderContact pc = new ProviderContact();
                pc.ClaimID = ClaimId;
                pc.FileID = 0;
                pc.ServiceLineNumber = "0";
                pc.LoopName = "2000A";
                foreach (XElement ele in BillProvPER.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("PER02")) pc.ContactName = ele.Value;
                    if (ele.Name.ToString().StartsWith("PER04"))
                    {
                        switch (((XElement)ele.PreviousNode).Value)
                        {
                            case "TE":
                                pc.Phone = ele.Value;
                                break;
                            case "EM":
                                pc.Email = ele.Value;
                                break;
                            case "FX":
                                pc.Fax = ele.Value;
                                break;
                        }
                    }
                    if (ele.Name.ToString().StartsWith("PER06"))
                    {
                        switch (((XElement)ele.PreviousNode).Value)
                        {
                            case "TE":
                                pc.Phone = ele.Value;
                                break;
                            case "EM":
                                pc.Email = ele.Value;
                                break;
                            case "FX":
                                pc.Fax = ele.Value;
                                break;
                            case "EX":
                                pc.PhoneEx = ele.Value;
                                break;
                        }
                    }
                    if (ele.Name.ToString().StartsWith("PER08"))
                    {
                        switch (((XElement)ele.PreviousNode).Value)
                        {
                            case "TE":
                                pc.Phone = ele.Value;
                                break;
                            case "EM":
                                pc.Email = ele.Value;
                                break;
                            case "FX":
                                pc.Fax = ele.Value;
                                break;
                            case "EX":
                                pc.PhoneEx = ele.Value;
                                break;
                        }
                    }
                }
                claim.ProviderContacts.Add(pc);
            }
            XElement loop2010AB = loop2000A.Descendants(ns + "NM1_SubLoop_2").FirstOrDefault().Descendants(ns + "TS837_2010AB_Loop").FirstOrDefault();
            if (loop2010AB != null)
            {
                provider = new ClaimProvider();
                provider.ClaimID = ClaimId; ;
                provider.FileID = 0;
                provider.ServiceLineNumber = "0";
                provider.LoopName = "2010AB";
                XElement loop2010ABNM1 = loop2010AB.Descendants(ns + "NM1_Pay_toAddressName").FirstOrDefault();
                if (loop2010ABNM1 != null)
                {
                    foreach (XElement ele in loop2010ABNM1.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM104")) provider.ProviderFirstName = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM105")) provider.ProviderMiddle = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM107")) provider.ProviderSuffix = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
                    }
                }
                XElement loop2010ABN3 = loop2010AB.Descendants(ns + "N3_Pay_ToAddress_ADDRESS").FirstOrDefault();
                if (loop2010ABN3 != null)
                {
                    foreach (XElement ele in loop2010ABN3.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("N301")) provider.ProviderAddress = ele.Value;
                        if (ele.Name.ToString().StartsWith("N302")) provider.ProviderAddress2 = ele.Value;
                    }
                }
                XElement loop2010ABN4 = loop2010AB.Descendants(ns + "N4_Pay_toAddressCity_State_ZIPCode").FirstOrDefault();
                if (loop2010ABN4 != null)
                {
                    foreach (XElement ele in loop2010ABN4.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("N401")) provider.ProviderCity = ele.Value;
                        if (ele.Name.ToString().StartsWith("N402")) provider.ProviderState = ele.Value;
                        if (ele.Name.ToString().StartsWith("N403")) provider.ProviderZip = ele.Value;
                        if (ele.Name.ToString().StartsWith("N404")) provider.ProviderCountry = ele.Value;
                        if (ele.Name.ToString().StartsWith("N407")) provider.ProviderCountrySubCode = ele.Value;
                    }
                }
                claim.Providers.Add(provider);
            }
            ClaimSBR subscriber = new ClaimSBR();
            subscriber.ClaimID = ClaimId; ;
            subscriber.FileID = 0;
            subscriber.LoopName = "2000B";
            XElement loop2000B = loop2000A.Descendants(ns + "TS837_2000B_Loop").FirstOrDefault();
            XElement Subscriber = loop2000B.Descendants(ns + "SBR_SubscriberInformation").FirstOrDefault();
            foreach (XElement ele in Subscriber.Descendants())
            {
                if (ele.Name.ToString().StartsWith("SBR01")) subscriber.SubscriberSequenceNumber = ele.Value;
                if (ele.Name.ToString().StartsWith("SBR02")) subscriber.SubscriberRelationshipCode = ele.Value;
                if (ele.Name.ToString().StartsWith("SBR03")) subscriber.InsuredGroupNumber = ele.Value;
                if (ele.Name.ToString().StartsWith("SBR04")) subscriber.OtherInsuredGroupName = ele.Value;
                if (ele.Name.ToString().StartsWith("SBR05")) subscriber.InsuredTypeCode = ele.Value;
                if (ele.Name.ToString().StartsWith("SBR09")) subscriber.ClaimFilingCode = ele.Value;
            }
            XElement subscriberPAT = loop2000B.Descendants(ns + "PAT_PatientInformation").FirstOrDefault();
            if (subscriberPAT != null)
            {
                foreach (XElement ele in subscriberPAT.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("PAT06")) subscriber.DeathDate = ele.Value;
                    if (ele.Name.ToString().StartsWith("PAT08")) subscriber.Weight = ele.Value;
                    if (ele.Name.ToString().StartsWith("PAT09")) subscriber.PregnancyIndicator = ele.Value;
                }
            }
            XElement loop2010BA = loop2000A.Descendants(ns + "NM1_SubLoop_3").FirstOrDefault().Descendants(ns + "TS837_2010BA_Loop").FirstOrDefault();
            XElement subscriberNM1 = loop2010BA.Descendants(ns + "NM1_SubscriberName").FirstOrDefault();
            foreach (XElement ele in subscriberNM1.Descendants())
            {
                if (ele.Name.ToString().StartsWith("NM103")) subscriber.LastName = ele.Value;
                if (ele.Name.ToString().StartsWith("NM104")) subscriber.FirstName = ele.Value;
                if (ele.Name.ToString().StartsWith("NM105")) subscriber.MidddleName = ele.Value;
                if (ele.Name.ToString().StartsWith("NM107")) subscriber.NameSuffix = ele.Value;
                if (ele.Name.ToString().StartsWith("NM108")) subscriber.IDQualifier = ele.Value;
                if (ele.Name.ToString().StartsWith("NM109")) subscriber.IDCode = ele.Value;
            }
            XElement subscriberN3 = loop2010BA.Descendants(ns + "N3_SubscriberAddress").FirstOrDefault();
            if (subscriberN3 != null)
            {
                foreach (XElement ele in subscriberN3.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("N301")) subscriber.SubscriberAddress = ele.Value;
                    if (ele.Name.ToString().StartsWith("N302")) subscriber.SubscriberAddress2 = ele.Value;
                }
            }
            XElement subscriberN4 = loop2010BA.Descendants(ns + "N4_SubscriberCity_State_ZIPCode").FirstOrDefault();
            if (subscriberN4 != null)
            {
                foreach (XElement ele in subscriberN4.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("N401")) subscriber.SubscriberCity = ele.Value;
                    if (ele.Name.ToString().StartsWith("N402")) subscriber.SubscriberState = ele.Value;
                    if (ele.Name.ToString().StartsWith("N403")) subscriber.SubscriberZip = ele.Value;
                    if (ele.Name.ToString().StartsWith("N404")) subscriber.SubscriberCountry = ele.Value;
                    if (ele.Name.ToString().StartsWith("N407")) subscriber.SubscriberCountrySubCode = ele.Value;
                }
            }
            XElement subscriberDMG = loop2010BA.Descendants(ns + "DMG_SubscriberDemographicInformation").FirstOrDefault();
            if (subscriberDMG != null)
            {
                foreach (XElement ele in subscriberDMG.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("DMG02")) subscriber.SubscriberBirthDate = ele.Value;
                    if (ele.Name.ToString().StartsWith("DMG03")) subscriber.SubscriberGender = ele.Value;
                }
            }
            XElement subscriberREF = loop2010BA.Descendants(ns + "REF_SubLoop_3").FirstOrDefault();
            if (subscriberREF != null)
            {
                foreach (XElement ele in subscriberREF.Nodes())
                {
                    ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                    si.ClaimID = ClaimId;
                    si.FileID = 0;
                    si.ServiceLineNumber = "0";
                    si.LoopName = "2010BA";
                    foreach (XElement child_ele in ele.Descendants())
                    {
                        if (child_ele.Name.ToString().StartsWith("REF01")) si.ProviderQualifier = child_ele.Value;
                        if (child_ele.Name.ToString().StartsWith("REF02")) si.ProviderID = child_ele.Value;
                    }
                    claim.SecondaryIdentifications.Add(si);
                }
            }
            claim.Subscribers.Add(subscriber);
            provider = new ClaimProvider();
            provider.ClaimID = ClaimId;
            provider.FileID = 0;
            provider.ServiceLineNumber = "0";
            provider.LoopName = "2010BB";
            XElement loop2010BB = loop2000A.Descendants(ns + "NM1_SubLoop_3").FirstOrDefault().Descendants(ns + "TS837_2010BB_Loop").FirstOrDefault();
            XElement subscriberPayerNM1 = loop2010BB.Descendants(ns + "NM1_PayerName").FirstOrDefault();
            foreach (XElement ele in subscriberPayerNM1.Descendants())
            {
                if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
            }
            XElement subscriberPayerN3 = loop2010BB.Descendants(ns + "N3_PayerAddress").FirstOrDefault();
            if (subscriberPayerN3 != null)
            {
                foreach (XElement ele in subscriberPayerN3.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("N301")) provider.ProviderAddress = ele.Value;
                    if (ele.Name.ToString().StartsWith("N302")) provider.ProviderAddress2 = ele.Value;
                }
            }
            XElement subscriberPayerN4 = loop2010BB.Descendants(ns + "N4_PayerCity_StatE_ZIPCode").FirstOrDefault();
            if (subscriberPayerN4 != null)
            {
                foreach (XElement ele in subscriberPayerN4.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("N401")) provider.ProviderCity = ele.Value;
                    if (ele.Name.ToString().StartsWith("N402")) provider.ProviderState = ele.Value;
                    if (ele.Name.ToString().StartsWith("N403")) provider.ProviderZip = ele.Value;
                    if (ele.Name.ToString().StartsWith("N404")) provider.ProviderCountry = ele.Value;
                    if (ele.Name.ToString().StartsWith("N407")) provider.ProviderCountrySubCode = ele.Value;
                }
            }
            claim.Providers.Add(provider);
            XElement loop2300 = loop2000B.Descendants(ns + "TS837_2300_Loop").FirstOrDefault();
            XElement Clm = loop2300.Descendants(ns + "CLM_Claiminformation").FirstOrDefault();
            XElement ClmPOS = Clm.Descendants(ns + "C023_HealthCareServiceLocationInformation").FirstOrDefault();
            foreach (XElement ele in Clm.Descendants())
            {
                if (ele.Name.ToString().StartsWith("CLM02")) claim.Header.ClaimAmount = ele.Value;
                if (ele.Name.ToString().StartsWith("CLM06")) claim.Header.ClaimProviderSignature = ele.Value;
                if (ele.Name.ToString().StartsWith("CLM07")) claim.Header.ClaimProviderAssignment = ele.Value;
                if (ele.Name.ToString().StartsWith("CLM08")) claim.Header.ClaimBenefitAssignment = ele.Value;
                if (ele.Name.ToString().StartsWith("CLM09")) claim.Header.ClaimReleaseofInformationCode = ele.Value;
            }
            foreach (XElement ele in ClmPOS.Descendants())
            {
                if (ele.Name.ToString().StartsWith("C02301")) claim.Header.ClaimPOS = ele.Value;
                if (ele.Name.ToString().StartsWith("C02302")) claim.Header.ClaimType = ele.Value;
                if (ele.Name.ToString().StartsWith("C02303")) claim.Header.ClaimFrequencyCode = ele.Value;
            }
            XElement claimDTP = loop2300.Descendants(ns + "DTP_SubLoop").FirstOrDefault();
            if (claimDTP != null)
            {
                foreach (XElement ele in claimDTP.Nodes())
                {
                    foreach (XElement child_ele in ele.Descendants())
                    {
                        if (child_ele.Name.ToString().StartsWith("DTP01"))
                        {
                            switch (child_ele.Value)
                            {
                                case "304":
                                    claim.Header.LastSeenDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                    break;
                                case "431":
                                    claim.Header.CurrentIllnessDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                    break;
                                case "435":
                                    claim.Header.AdmissionDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                    break;
                                case "439":
                                    claim.Header.AccidentDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                    break;
                                case "454":
                                    claim.Header.InitialTreatmentDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                    break;
                                case "471":
                                    claim.Header.PrescriptionDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                    break;
                                case "484":
                                    claim.Header.LastMenstrualPeriodDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                    break;
                                case "096":
                                    //institutional discharge hour
                                    claim.Header.DischargeDate = ((XElement)child_ele.NextNode.NextNode).Value;
                                    break;
                                case "434":
                                    //institutional statement date
                                    claim.Header.StatementFromDate = ((XElement)child_ele.NextNode.NextNode).Value.Split('-')[0];
                                    claim.Header.StatementToDate = ((XElement)child_ele.NextNode.NextNode).Value.Split('-')[1];
                                    break;
                            }
                            break;
                        }
                    }
                }
            }
            XElement CL1 = loop2300.Descendants(ns + "CL1_InstitutionalClaimCode").FirstOrDefault();
            if (CL1 != null)
            {
                foreach (XElement ele in CL1.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("CL101")) claim.Header.AdmissionTypeCode = ele.Value;
                    if (ele.Name.ToString().StartsWith("CL102")) claim.Header.AdmissionSourceCode = ele.Value;
                    if (ele.Name.ToString().StartsWith("CL103")) claim.Header.PatientStatusCode = ele.Value;
                }
            }
            XElement CN1 = loop2000B.Descendants(ns + "CN1_ContractInformation").FirstOrDefault();
            if (CN1 != null)
            {
                foreach (XElement ele in CN1.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("CN101")) claim.Header.ContractTypeCode = ele.Value;
                    if (ele.Name.ToString().StartsWith("CN102")) claim.Header.ContractAmount = ele.Value;
                    if (ele.Name.ToString().StartsWith("CN104")) claim.Header.ContractCode = ele.Value;
                }
            }
            XElement patientResponsibilityAmount = loop2300.Descendants(ns + "AMT_PatientEstimatedAmountDue").FirstOrDefault();
            if (patientResponsibilityAmount != null)
            {
                foreach (XElement ele in patientResponsibilityAmount.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("AMT02")) claim.Header.PatientResponsibilityAmount = ele.Value;
                }
            }
            XElement ClaimSecondaryIdentifications = loop2300.Descendants(ns + "REF_SubLoop_4").FirstOrDefault();
            if (ClaimSecondaryIdentifications != null)
            {
                foreach (XElement ele in ClaimSecondaryIdentifications.Nodes())
                {
                    ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                    si.ClaimID = ClaimId;
                    si.FileID = 0;
                    si.ServiceLineNumber = "0";
                    si.LoopName = "2300";
                    foreach (XElement child_ele in ele.Descendants())
                    {
                        if (child_ele.Name.ToString().StartsWith("REF01")) si.ProviderQualifier = child_ele.Value;
                        if (child_ele.Name.ToString().StartsWith("REF02")) si.ProviderID = child_ele.Value;
                    }
                    claim.SecondaryIdentifications.Add(si);
                }
            }
            if (claim.SecondaryIdentifications.Where(x => x.LoopName == "2300" && x.ProviderQualifier == "F8").FirstOrDefault() == null)
            {
                ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                si.ClaimID = ClaimId;
                si.FileID = 0;
                si.LoopName = "2300";
                si.ServiceLineNumber = "0";
                si.ProviderQualifier = "F8";
                si.ProviderID = ClaimId;
                claim.SecondaryIdentifications.Add(si);
            }
            XElement note = loop2000B.Descendants(ns + "NTE_SubLoop").FirstOrDefault();
            if (note != null)
            {
                foreach (XElement ele in note.Nodes())
                {
                    ClaimNte nte = new ClaimNte();
                    nte.ClaimID = ClaimId;
                    nte.FileID = 0;
                    nte.ServiceLineNumber = "0";
                    foreach (XElement child_ele in ele.Descendants())
                    {
                        if (child_ele.Name.ToString().StartsWith("NTE01")) nte.NoteCode = child_ele.Value;
                        if (child_ele.Name.ToString().StartsWith("NTE02")) nte.NoteText = child_ele.Value;
                    }
                    claim.Notes.Add(nte);
                }
            }
            XElement his = loop2300.Descendants(ns + "HI_SubLoop").FirstOrDefault();
            foreach (XElement ele in his.Nodes())
            {
                foreach (XElement child_ele in ele.Nodes())
                {
                    if (child_ele.Name.ToString().StartsWith("HI_Other"))
                    {
                        foreach (XElement grand_ele in child_ele.Descendants())
                        {
                            ClaimHI hi = new ClaimHI();
                            hi.ClaimID = ClaimId;
                            hi.FileID = 0;
                            foreach (XElement grand_grand_ele in grand_ele.Descendants())
                            {
                                if (grand_grand_ele.Name.ToString().StartsWith("C02201")) hi.HIQual = grand_grand_ele.Value;
                                if (grand_grand_ele.Name.ToString().StartsWith("C02202")) hi.HICode = grand_grand_ele.Value;
                                if (grand_grand_ele.Name.ToString().StartsWith("C02204")) hi.HIFromDate = grand_grand_ele.Value;
                                if (grand_grand_ele.Name.ToString().StartsWith("C02205")) hi.HIAmount = grand_grand_ele.Value;
                                if (grand_grand_ele.Name.ToString().StartsWith("C02209")) hi.PresentOnAdmissionIndicator = grand_grand_ele.Value;
                                if (!string.IsNullOrEmpty(hi.HIFromDate) && hi.HIFromDate.Contains("-"))
                                {
                                    hi.HIToDate = hi.HIFromDate.Substring(hi.HIFromDate.Length - 8, 8);
                                    hi.HIFromDate = hi.HIFromDate.Substring(0, 8);
                                }
                            }
                            claim.His.Add(hi);
                        }
                    }
                    else
                    {
                        ClaimHI hi = new ClaimHI();
                        hi.ClaimID = ClaimId;
                        hi.FileID = 0;
                        foreach (XElement grand_ele in child_ele.Descendants())
                        {
                            if (grand_ele.Name.ToString().StartsWith("C02201")) hi.HIQual = grand_ele.Value;
                            if (grand_ele.Name.ToString().StartsWith("C02202")) hi.HICode = grand_ele.Value;
                            if (grand_ele.Name.ToString().StartsWith("C02204")) hi.HIFromDate = grand_ele.Value;
                            if (grand_ele.Name.ToString().StartsWith("C02205")) hi.HIAmount = grand_ele.Value;
                            if (grand_ele.Name.ToString().StartsWith("C02209")) hi.PresentOnAdmissionIndicator = grand_ele.Value;
                            if (!string.IsNullOrEmpty(hi.HIFromDate) && hi.HIFromDate.Contains("-"))
                            {
                                hi.HIToDate = hi.HIFromDate.Substring(hi.HIFromDate.Length - 8, 8);
                                hi.HIFromDate = hi.HIFromDate.Substring(0, 8);
                            }
                        }
                        claim.His.Add(hi);
                    }
                }
            }
            XElement claimlevelPWK = loop2000B.Descendants(ns + "PWK_ClaimSupplementalInformation").FirstOrDefault();
            if (claimlevelPWK != null)
            {
                ClaimPWK pwk = new ClaimPWK();
                pwk.ClaimID = ClaimId;
                pwk.FileID = 0;
                pwk.ServiceLineNumber = "0";
                foreach (XElement ele in claimlevelPWK.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("PWK01")) pwk.ReportTypeCode = ele.Value;
                    if (ele.Name.ToString().StartsWith("PWK02")) pwk.ReportTransmissionCode = ele.Value;
                }
                claim.PWKs.Add(pwk);
            }

            XElement AtteProv = loop2300.Descendants(ns + "NM1_SubLoop_4").Descendants(ns + "TS837_2310A_Loop").FirstOrDefault();
            if (AtteProv != null)
            {
                provider = new ClaimProvider();
                provider.ClaimID = ClaimId;
                provider.FileID = 0;
                provider.ServiceLineNumber = "0";
                provider.LoopName = "2310A";
                provider.RepeatSequence = (claim.Providers.Count + 1).ToString();
                foreach (XElement ele in AtteProv.Descendants(ns + "NM1_AttendingProviderName").Descendants())
                {
                    if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM104")) provider.ProviderFirstName = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM105")) provider.ProviderMiddle = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM107")) provider.ProviderSuffix = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
                }
                if (AtteProv.Descendants(ns + "REF_AttendingProviderSecondaryIdentification").Count() > 0)
                {
                    foreach (XElement ele in AtteProv.Descendants(ns + "REF_ReferringProviderSecondaryIdentification"))
                    {
                        ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                        si.ClaimID = ClaimId;
                        si.FileID = 0;
                        si.ServiceLineNumber = "0";
                        si.LoopName = "2310A";
                        foreach (XElement child_ele in ele.Descendants())
                        {
                            if (child_ele.Name.ToString().StartsWith("REF01")) si.ProviderQualifier = child_ele.Value;
                            if (child_ele.Name.ToString().StartsWith("REF02")) si.ProviderID = child_ele.Value;
                        }
                        claim.SecondaryIdentifications.Add(si);
                    }
                }
                claim.Providers.Add(provider);
            }

            XElement operProv = loop2300.Descendants(ns + "NM1_SubLoop_4").Descendants(ns + "TS837_2310B_Loop").FirstOrDefault();
            if (operProv != null)
            {
                provider = new ClaimProvider();
                provider.ClaimID = ClaimId;
                provider.FileID = 0;
                provider.ServiceLineNumber = "0";
                provider.LoopName = "2310B";
                XElement operProvNM1 = operProv.Descendants(ns + "NM1_OperatingPhysicianName").FirstOrDefault();
                if (operProvNM1 != null)
                {
                    foreach (XElement ele in operProvNM1.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM104")) provider.ProviderFirstName = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM105")) provider.ProviderMiddle = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM107")) provider.ProviderSuffix = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                        if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
                    }
                }
                claim.Providers.Add(provider);
                if (operProv.Descendants(ns + "REF_OperatingPhysicianSecondaryIdentification").Count() > 0)
                {
                    foreach (XElement rendProvSI in operProv.Descendants(ns + "REF_OperatingPhysicianSecondaryIdentification"))
                    {
                        ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                        si.ClaimID = ClaimId;
                        si.FileID = 0;
                        si.ServiceLineNumber = "0";
                        si.LoopName = "2310B";
                        foreach (XElement ele in rendProvSI.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("REF01")) si.ProviderQualifier = ele.Value;
                            if (ele.Name.ToString().StartsWith("REF02")) si.ProviderID = ele.Value;
                        }
                        claim.SecondaryIdentifications.Add(si);
                    }
                }
            }
            XElement loop2320 = loop2300.Descendants(ns + "TS837_2320_Loop").FirstOrDefault();
            if (loop2320 != null)
            {
                subscriber = new ClaimSBR();
                subscriber.ClaimID = ClaimId;
                subscriber.FileID = 0;
                subscriber.LoopName = "2320";
                XElement othersubscriber = loop2320.Descendants(ns + "SBR_OtherSubscriberInformation").FirstOrDefault();
                if (othersubscriber != null)
                {
                    foreach (XElement ele in othersubscriber.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("SBR01")) subscriber.SubscriberSequenceNumber = ele.Value;
                        if (ele.Name.ToString().StartsWith("SBR02")) subscriber.SubscriberRelationshipCode = ele.Value;
                        if (ele.Name.ToString().StartsWith("SBR03")) subscriber.InsuredGroupNumber = ele.Value;
                        if (ele.Name.ToString().StartsWith("SBR04")) subscriber.OtherInsuredGroupName = ele.Value;
                        if (ele.Name.ToString().StartsWith("SBR05")) subscriber.InsuredTypeCode = ele.Value;
                        if (ele.Name.ToString().StartsWith("SBR09")) subscriber.ClaimFilingCode = ele.Value;
                    }
                    claim.Subscribers.Add(subscriber);
                }
                XElement othersubscriberAMT = loop2320.Descendants(ns + "AMT_SubLoop").FirstOrDefault();
                if (othersubscriberAMT != null)
                {
                    XElement cobPayerPaidAmount = othersubscriberAMT.Descendants(ns + "AMT_CoordinationofBenefits_COB_PayerPaidAmount").FirstOrDefault();
                    if (cobPayerPaidAmount != null)
                    {
                        foreach (XElement ele in cobPayerPaidAmount.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("AMT02")) subscriber.COBPayerPaidAmount = ele.Value;
                        }
                    }
                }
                XElement othersubscriberOI = loop2320.Descendants(ns + "OI_OtherInsuranceCoverageInformation").FirstOrDefault();
                foreach (XElement ele in othersubscriberOI.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("OI03")) subscriber.BenefitsAssignmentCertificationIndicator = ele.Value;
                    if (ele.Name.ToString().StartsWith("OI04")) subscriber.PatientSignatureSourceCode = ele.Value;
                    if (ele.Name.ToString().StartsWith("OI06")) subscriber.ReleaseOfInformationCode = ele.Value;
                }
                XElement loop2330A = loop2320.Descendants(ns + "NM1_SubLoop_5").FirstOrDefault().Descendants(ns + "TS837_2330A_Loop").FirstOrDefault();
                XElement otherSubcriberNM1 = loop2330A.Descendants(ns + "NM1_OtherSubscriberName").FirstOrDefault();
                foreach (XElement ele in otherSubcriberNM1.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("NM103")) subscriber.LastName = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM104")) subscriber.FirstName = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM105")) subscriber.MidddleName = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM107")) subscriber.NameSuffix = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM108")) subscriber.IDQualifier = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM109")) subscriber.IDCode = ele.Value;
                }
                XElement otherSubscriberN3 = loop2330A.Descendants(ns + "N3_OtherSubscriberAddress").FirstOrDefault();
                if (otherSubscriberN3 != null)
                {
                    foreach (XElement ele in otherSubscriberN3.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("N301")) subscriber.SubscriberAddress = ele.Value;
                        if (ele.Name.ToString().StartsWith("N302")) subscriber.SubscriberAddress2 = ele.Value;
                    }
                }
                XElement otherSubscriberN4 = loop2330A.Descendants(ns + "N4_OtherSubscriberCity_State_ZIPCode").FirstOrDefault();
                if (otherSubscriberN4 != null)
                {
                    foreach (XElement ele in otherSubscriberN4.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("N401")) subscriber.SubscriberCity = ele.Value;
                        if (ele.Name.ToString().StartsWith("N402")) subscriber.SubscriberState = ele.Value;
                        if (ele.Name.ToString().StartsWith("N403")) subscriber.SubscriberZip = ele.Value;
                        if (ele.Name.ToString().StartsWith("N404")) subscriber.SubscriberCountry = ele.Value;
                        if (ele.Name.ToString().StartsWith("N407")) subscriber.SubscriberCountrySubCode = ele.Value;
                    }
                }
                claim.Subscribers.Add(subscriber);
                XElement loop2330B = loop2320.Descendants(ns + "NM1_SubLoop_5").FirstOrDefault().Descendants(ns + "TS837_2330B_Loop").FirstOrDefault();
                provider = new ClaimProvider();
                provider.ClaimID = ClaimId;
                provider.FileID = 0;
                provider.LoopName = "2330B";
                provider.ServiceLineNumber = "0";
                provider.RepeatSequence = claim.Subscribers.Last().SubscriberSequenceNumber;
                XElement otherPayerNM1 = loop2330B.Descendants(ns + "NM1_OtherPayerName").FirstOrDefault();
                foreach (XElement ele in otherPayerNM1.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("NM101")) provider.ProviderQualifier = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM103")) provider.ProviderLastName = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM108")) provider.ProviderIDQualifier = ele.Value;
                    if (ele.Name.ToString().StartsWith("NM109")) provider.ProviderID = ele.Value;
                }
                XElement otherPayerN3 = loop2330B.Descendants(ns + "N3_OtherPayerAddress").FirstOrDefault();
                if (otherPayerN3 != null)
                {
                    foreach (XElement ele in otherPayerN3.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("N301")) provider.ProviderAddress = ele.Value;
                        if (ele.Name.ToString().StartsWith("N302")) provider.ProviderAddress2 = ele.Value;
                    }
                }
                XElement otherPayerN4 = loop2330B.Descendants(ns + "N4_OtherPayerCity_State_ZIPCode").FirstOrDefault();
                if (otherPayerN4 != null)
                {
                    foreach (XElement ele in otherPayerN4.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("N401")) provider.ProviderCity = ele.Value;
                        if (ele.Name.ToString().StartsWith("N402")) provider.ProviderState = ele.Value;
                        if (ele.Name.ToString().StartsWith("N403")) provider.ProviderZip = ele.Value;
                        if (ele.Name.ToString().StartsWith("N404")) provider.ProviderCountry = ele.Value;
                        if (ele.Name.ToString().StartsWith("N407")) provider.ProviderCountrySubCode = ele.Value;
                    }
                }
                claim.Providers.Add(provider);
            }
            foreach (XElement loop2400 in loop2300.Descendants(ns + "TS837_2400_Loop"))
            {
                ServiceLine line = new ServiceLine();
                line.ClaimID = ClaimId;
                line.FileID = 0;
                XElement LX = loop2400.Descendants(ns + "LX_ServiceLineNumber").FirstOrDefault();
                foreach (XElement ele in LX.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("LX01")) line.ServiceLineNumber = ele.Value;
                }
                XElement sv2 = loop2400.Descendants(ns + "SV2_InstitutionalServiceLine").FirstOrDefault();
                XElement lineProcedure = sv2.Descendants(ns + "C003_CompositeMedicalProcedureIdentifier").FirstOrDefault();
                //XElement lineDiagPointer = sv2.Descendants(ns + "C004_CompositeDiagnosisCodePointer").FirstOrDefault();
                if (lineProcedure != null)
                {
                    foreach (XElement ele in lineProcedure.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("C00301")) line.ServiceIDQualifier = ele.Value;
                        if (ele.Name.ToString().StartsWith("C00302")) line.ProcedureCode = ele.Value;
                        if (ele.Name.ToString().StartsWith("C00303")) line.ProcedureModifier1 = ele.Value;
                        if (ele.Name.ToString().StartsWith("C00304")) line.ProcedureModifier2 = ele.Value;
                        if (ele.Name.ToString().StartsWith("C00305")) line.ProcedureModifier3 = ele.Value;
                        if (ele.Name.ToString().StartsWith("C00306")) line.ProcedureModifier4 = ele.Value;
                        if (ele.Name.ToString().StartsWith("C00307")) line.ProcedureDescription = ele.Value;
                    }
                }
                foreach (XElement ele in sv2.Descendants())
                {
                    if (ele.Name.ToString().StartsWith("SV201")) line.RevenueCode = ele.Value;
                    if (ele.Name.ToString().StartsWith("SV203")) line.LineItemChargeAmount = ele.Value;
                    if (ele.Name.ToString().StartsWith("SV204")) line.LineItemUnit = ele.Value;
                    if (ele.Name.ToString().StartsWith("SV205")) line.ServiceUnitQuantity = ele.Value;
                    if (ele.Name.ToString().StartsWith("SV207")) line.LineItemDeniedChargeAmount = ele.Value;
                }
                //foreach (XElement ele in lineDiagPointer.Descendants())
                //{
                //    if (ele.Name.ToString().StartsWith("C00401")) line.DiagPointer1 = ele.Value;
                //    if (ele.Name.ToString().StartsWith("C00402")) line.DiagPointer2 = ele.Value;
                //    if (ele.Name.ToString().StartsWith("C00403")) line.DiagPointer3 = ele.Value;
                //    if (ele.Name.ToString().StartsWith("C00404")) line.DiagPointer4 = ele.Value;
                //}
                XElement dtps = loop2400.Descendants(ns + "DTP_Date_ServiceDate").FirstOrDefault();
                if (dtps != null)
                {
                    foreach (XElement ele in dtps.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("DTP01"))
                        {
                            switch (ele.Value)
                            {
                                case "472":
                                    line.ServiceFromDate = ((XElement)ele.NextNode.NextNode).Value.Split('-')[0];
                                    if (((XElement)ele.NextNode.NextNode).Value.Contains("-")) line.ServiceToDate = ((XElement)ele.NextNode.NextNode).Value.Split('-')[1];
                                    break;
                            }
                            break;
                        }
                    }
                }
                XElement lineCN1 = loop2400.Descendants(ns + "CN1_ContractInformation_2").FirstOrDefault();
                if (lineCN1 != null)
                {
                    foreach (XElement ele in lineCN1.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("CN101")) line.ContractTypeCode = ele.Value;
                        if (ele.Name.ToString().StartsWith("CN102")) line.ContractAmount = ele.Value;
                        if (ele.Name.ToString().StartsWith("CN103")) line.ContractPercentage = ele.Value;
                        if (ele.Name.ToString().StartsWith("CN104")) line.ContractCode = ele.Value;
                        if (ele.Name.ToString().StartsWith("CN105")) line.TermsDiscountPercentage = ele.Value;
                        if (ele.Name.ToString().StartsWith("CN106")) line.ContractVersionIdentifier = ele.Value;
                    }
                }
                claim.Lines.Add(line);
                XElement lineREF = loop2400.Descendants(ns + "REF_SubLoop_7").FirstOrDefault();
                if (lineREF != null)
                {
                    foreach (XElement ele in lineREF.Nodes())
                    {
                        ClaimSecondaryIdentification si = new ClaimSecondaryIdentification();
                        si.ClaimID = ClaimId;
                        si.FileID = 0;
                        si.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                        si.LoopName = "2400";
                        foreach (XElement child_ele in ele.Descendants())
                        {
                            if (child_ele.Name.ToString().StartsWith("REF01")) si.ProviderQualifier = child_ele.Value;
                            if (child_ele.Name.ToString().StartsWith("REF02")) si.ProviderID = child_ele.Value;
                        }
                        claim.SecondaryIdentifications.Add(si);
                    }
                }
                XElement lineNte = loop2400.Descendants(ns + "NTE_SubLoop").FirstOrDefault();
                if (lineNte != null)
                {
                    foreach (XElement ele in lineNte.Nodes())
                    {
                        ClaimNte nte = new ClaimNte();
                        nte.ClaimID = ClaimId;
                        nte.FileID = 0;
                        nte.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                        foreach (XElement child_ele in ele.Descendants())
                        {
                            if (child_ele.Name.ToString().StartsWith("NTE01")) nte.NoteCode = child_ele.Value;
                            if (child_ele.Name.ToString().StartsWith("NTE02")) nte.NoteText = child_ele.Value;
                        }
                        claim.Notes.Add(nte);
                    }
                }
                XElement loop2410 = loop2400.Descendants(ns + "TS837_2410_Loop").FirstOrDefault();
                if (loop2410 != null)
                {
                    XElement linDrugIdentification = loop2410.Descendants(ns + "LIN_DrugIdentification").FirstOrDefault();
                    if (linDrugIdentification != null)
                    {
                        foreach (XElement ele in linDrugIdentification.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("LIN02")) claim.Lines.Last().LINQualifier = ele.Value;
                            if (ele.Name.ToString().StartsWith("LIN03")) claim.Lines.Last().NationalDrugCode = ele.Value;
                        }
                    }
                    XElement ctpDrugQuantity = loop2410.Descendants(ns + "CTP_DrugQuantity").FirstOrDefault();
                    if (ctpDrugQuantity != null)
                    {
                        foreach (XElement ele in ctpDrugQuantity.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("CTP04")) claim.Lines.Last().DrugQuantity = ele.Value;
                        }
                        foreach (XElement ele in ctpDrugQuantity.Descendants(ns + "C001_CompositeUnitofMeasure_2").First().Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("C00101")) claim.Lines.Last().DrugQualifier = ele.Value;
                        }
                    }
                }
                foreach (XElement loop2430 in loop2400.Descendants(ns + "TS837_2430_Loop"))
                {
                    ClaimLineSVD svd = new ClaimLineSVD();
                    svd.ClaimID = ClaimId;
                    svd.FileID = 0;
                    svd.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                    svd.RepeatSequence = (claim.SVDs.Count + 1).ToString();
                    XElement linesvd = loop2430.Descendants(ns + "SVD_LineAdjudicationInformation").FirstOrDefault();
                    claim.SVDs.Add(svd);
                    if (linesvd != null)
                    {
                        foreach (XElement ele in linesvd.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("SVD01")) claim.SVDs.Last().OtherPayerPrimaryIdentifier = ele.Value;
                            if (ele.Name.ToString().StartsWith("SVD02")) claim.SVDs.Last().ServiceLinePaidAmount = ele.Value;
                            if (ele.Name.ToString().StartsWith("SVD04")) claim.SVDs.Last().ServiceLineRevenueCode = ele.Value;
                            if (ele.Name.ToString().StartsWith("SVD05")) claim.SVDs.Last().PaidServiceUnitCount = ele.Value;
                            if (ele.Name.ToString().StartsWith("SVD06")) claim.SVDs.Last().BundledLineNumber = ele.Value;
                        }
                        XElement svdProcedure = linesvd.Descendants(ns + "C003_CompositeMedicalProcedureIdentifier_2").FirstOrDefault();
                        if (svdProcedure != null)
                        {
                            foreach (XElement ele in svdProcedure.Descendants())
                            {
                                if (ele.Name.ToString().StartsWith("C00301")) claim.SVDs.Last().ServiceQualifier = ele.Value;
                                if (ele.Name.ToString().StartsWith("C00302")) claim.SVDs.Last().ProcedureCode = ele.Value;
                                if (ele.Name.ToString().StartsWith("C00303")) claim.SVDs.Last().ProcedureModifier1 = ele.Value;
                                if (ele.Name.ToString().StartsWith("C00304")) claim.SVDs.Last().ProcedureModifier2 = ele.Value;
                                if (ele.Name.ToString().StartsWith("C00305")) claim.SVDs.Last().ProcedureModifier3 = ele.Value;
                                if (ele.Name.ToString().StartsWith("C00306")) claim.SVDs.Last().ProcedureModifier4 = ele.Value;
                                if (ele.Name.ToString().StartsWith("C00307")) claim.SVDs.Last().ProcedureDescription = ele.Value;
                            }
                        }
                    }
                    foreach (XElement linecas in loop2400.Descendants(ns + "CAS_LineAdjustment"))
                    {
                        ClaimCAS cas = new ClaimCAS();
                        cas.ClaimID = ClaimId;
                        cas.FileID = 0;
                        cas.ServiceLineNumber = claim.Lines.Last().ServiceLineNumber;
                        cas.SubscriberSequenceNumber = claim.SVDs.Last().RepeatSequence;
                        foreach (XElement ele in linecas.Descendants())
                        {
                            if (ele.Name.ToString().StartsWith("CAS01")) cas.GroupCode = ele.Value;
                            if (ele.Name.ToString().StartsWith("CAS02")) cas.ReasonCode = ele.Value;
                            if (ele.Name.ToString().StartsWith("CAS03")) cas.AdjustmentAmount = ele.Value;
                        }
                        claim.Cases.Add(cas);
                    }
                    XElement linedtp = loop2400.Descendants(ns + "DTP_LineCheckorRemittanceDate").FirstOrDefault();
                    foreach (XElement ele in linedtp.Descendants())
                    {
                        if (ele.Name.ToString().StartsWith("DTP03")) claim.SVDs.Last().AdjudicationDate = ele.Value;
                    }
                }
            }

        }
    }
}
