using EncDataModel.Provider274;
using Export274.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncDataModel.X12;

namespace Export274.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Export274Controller : ControllerBase
    {
        private readonly ILogger<Export274Controller> _logger;
        private readonly P274Context _context;
        public Export274Controller(ILogger<Export274Controller> logger, P274Context context)
        {
            _logger = logger;
            _context = context;
        }
        //Export274, initial query total records
        [HttpGet]
        public List<string> Get274CountsForExport()
        {
            List<string> result = new List<string>();
            _logger.Log(LogLevel.Information, "query total counts for 274 export");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string DestinationFolder = configuration["P274DestinationFolder"];
            int TotalRecords = _context.Details.Count();
            result.Add(TotalRecords.ToString());
            result.Add(DestinationFolder);
            return result;
        }
        //Export274/1
        [HttpGet("{id}")]
        public List<string> Export274(long id)
        {
            List<string> result = new List<string>();
            _logger.Log(LogLevel.Information, "exporting 274");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string DestinationFolder = configuration["P274DestinationFolder"];
            int goodDatabaseRecords = 0;

            StringBuilder sb = new StringBuilder();
            List<string> counties = _context.Files.Select(x => x.SenderId).ToList();
            counties = counties.Select(x => x.Substring(9, 3)).Distinct().ToList();
            foreach (string CountyCode in counties) 
            {
                sb.Clear();
                P274File file274 = _context.Files.FirstOrDefault(x=>x.SenderId.Substring(9,3) ==CountyCode);
                List<P274Detail> details = _context.Details.Where(x=>x.FileId==file274.FileId).ToList();
                List<P274AffiliatedEntity> entities = _context.Entities.Where(x=>x.FileId==file274.FileId).ToList();
                List<P274GroupIdNumber> groupIds = _context.Groups.Where(x=>x.FileId==file274.FileId).ToList();
                List<P274Information> infos = _context.Infos.Where(x=>x.FileId==file274.FileId).ToList();
                List<P274SiteCRC> crcs = _context.Crcs.Where(x=>x.FileId==file274.FileId).ToList();
                List<P274SiteWorkSchedule> schedules = _context.Schedules.Where(x=>x.FileId==file274.FileId).ToList();
                List<P274SpecializationArea> areas = _context.Areas.Where(x=>x.FileId==file274.FileId ).ToList();
                ISA_274 isa = new ISA_274();
                string icn = (DateTime.Today.DayOfYear + 100).ToString() + DateTime.Now.ToString("HHmmssfff").Substring(1, 6);
                string transactionDate = DateTime.Today.ToString("yyyyMMdd");
                string transactionTime = DateTime.Now.ToString("HHmm");
                int HLID = 1;
                int HLID_2000A = 0; //sender
                int HLID_2000B = 0; //receiver
                int HLID_2000C = 0; //group
                int HLID_2000D = 0; //site

                isa.ReportingMonth = file274.ReportYear + file274.ReportMonth;
                isa.InterchangeSenderID = file274.SenderId;
                isa.InterchangeReceiverID = file274.ReceiverId;
                isa.InterchangeDate = transactionDate;
                isa.InterchangeTime = transactionTime;
                isa.ProductionFlag = "P";
                isa.InterchangeControlNumber = icn;
                sb.Append(isa.ToX12String());
                GS gs = new GS();
                gs.FunctionalIDCode = "PW";
                gs.SenderID = file274.SenderId.Substring(0,9);
                gs.ReceiverID = file274.ReceiverId;
                gs.TransactionDate = transactionDate;
                gs.TransactrionTime = transactionTime;
                gs.GroupControlNumber = icn;
                gs.ResponsibleAgencyCode = "X";
                gs.VersionID = "004050X109";
                sb.Append(gs.ToX12String());
                ST_274 st = new ST_274();
                st.TransactionControlNumber = icn;
                st.VersionNumber = "004050X109";
                sb.Append(st.ToX12String());
                BHT bht = new BHT();
                bht.HirarchicalStructureCode = "0028";
                bht.TransactionSetPurposeCode = "27";
                bht.TransactionID = icn;
                bht.TransactionDate = transactionDate;
                bht.TransactionTime = transactionTime;
                sb.Append(bht.ToX12String());
                DTM_274 dtm = new DTM_274();
                dtm.TransactionDate = transactionDate;
                sb.Append(dtm.ToX12String());
                PER_274 per = new PER_274();
                PER274_Item peritem = new PER274_Item();
                peritem.FunctionCode = "BJ";
                peritem.ContactName = "IEHP";
                peritem.Phone = "9098902054";
                per.Pers.Add(peritem);
                sb.Append(per.ToX12String());
                HL_274 hl = new HL_274();
                hl.HLID = HLID.ToString();
                hl.ParentID = "";
                hl.LevelCode = "20";
                hl.ChildCode = "1";
                HLID_2000A = HLID;
                HLID++;
                sb.Append(hl.ToX12String());
                List<P274Information> info_Source = infos.Where(x => x.LoopName == "2000A").ToList();
                foreach (P274Information info in info_Source) 
                {
                    NM1 nm1 = new NM1();
                    nm1.NameQualifier = info.EntityIdCode;
                    nm1.NameType = info.EntityTypeQualifier;
                    nm1.LastName = info.EntityLastName;
                    nm1.FirstName = info.EntityFirstName;
                    nm1.IDQualifer = info.EntityIdQualifier;
                    nm1.IDCode = info.EntityId;
                    sb.Append(nm1.ToX12String());
                }
                hl = new HL_274();
                hl.HLID = HLID.ToString();
                hl.ParentID = HLID_2000A.ToString();
                hl.LevelCode = "21";
                hl.ChildCode = "1";
                sb.Append(hl.ToX12String());
                HLID_2000B = HLID;
                HLID++;
                List<P274Information> info_Receiver = infos.Where(x => x.LoopName == "2000B").ToList();
                foreach (P274Information info in info_Receiver)
                {
                    NM1 nm1 = new NM1();
                    nm1.NameQualifier = info.EntityIdCode;
                    nm1.NameType = info.EntityTypeQualifier;
                    nm1.LastName = info.EntityLastName;
                    nm1.FirstName = info.EntityFirstName;
                    nm1.IDQualifer = info.EntityIdQualifier;
                    nm1.IDCode = info.EntityId;
                    sb.Append(nm1.ToX12String());
                }

                List<P274Detail> groups = details.Where(x => x.ProviderQualifier == "QV").ToList();
                foreach (P274Detail group in groups) 
                {
                    hl = new HL_274();
                    hl.HLID = HLID.ToString();
                    hl.ParentID = HLID_2000B.ToString();
                    hl.LevelCode = "4";
                    hl.ChildCode = "1";
                    sb.Append(hl.ToX12String());
                    HLID_2000C = HLID;
                    HLID++;
                    NM1 nm1 = new NM1();
                    nm1.NameQualifier = group.ProviderQualifier;
                    nm1.NameType = group.ProviderTypeQualifier;
                    nm1.LastName = group.ProviderLastName;
                    nm1.FirstName = group.ProviderFirstName;
                    nm1.MiddleName = group.ProviderMiddleName;
                    nm1.Suffix = group.ProviderSuffix;
                    nm1.IDQualifer = group.ProviderIdQualifier;
                    nm1.IDCode = group.ProviderId;
                    sb.Append(nm1.ToX12String());
                    goodDatabaseRecords++;
                    N2_274 n2 = new N2_274();
                    n2.AdditionalName1 = group.ProviderAdditionalName1;
                    n2.AdditionalName2 = group.ProviderAdditionalName2;
                    sb.Append(n2.ToX12String());
                    if (!string.IsNullOrEmpty(group.ProviderContractEffectiveDate)) 
                    {
                        DTP dtp = new DTP();
                        dtp.DateCode = "092";
                        dtp.DateQualifier = "D8";
                        dtp.StartDate = group.ProviderContractEffectiveDate;
                        sb.Append(dtp.ToX12String());
                    }
                    if (!string.IsNullOrEmpty(group.ProviderContractExpirationDate)) 
                    {
                        DTP dtp = new DTP();
                        dtp.DateCode = "093";
                        dtp.DateQualifier = "D8";
                        dtp.StartDate = group.ProviderContractExpirationDate;
                        sb.Append(dtp.ToX12String());
                    }
                    List<P274SpecializationArea> groupAreas = areas.Where(x => x.DetailId == group.DetailId).ToList();
                    foreach (P274SpecializationArea area in groupAreas) 
                    {
                        LQ_274 lq = new LQ_274();
                        lq.TaxonomyQualifier = area.LQQualifier;
                        lq.TaxonomyCode = area.LQCode;
                        sb.Append(lq.ToX12String());
                    }
                    List<P274GroupIdNumber> groupReferences = groupIds.Where(x => x.DetailId == group.DetailId).ToList();
                    foreach (P274GroupIdNumber reference in groupReferences) 
                    {
                        REF_274 ref274 = new REF_274();
                        ref274.ReferenceQualifier = reference.ReferenceQualifier;
                        ref274.ReferenceId = reference.ReferenceId;
                        sb.Append(ref274.ToX12String());
                    }
                    List<P274Detail> sites = details.Where(x => x.GroupId == group.GroupId && x.ProviderQualifier == "77").ToList();
                    foreach (P274Detail site in sites) 
                    {
                        hl = new HL_274();
                        hl.HLID = HLID.ToString();
                        hl.ParentID = HLID_2000C.ToString();
                        hl.LevelCode = "N";
                        hl.ChildCode = "1";
                        sb.Append(hl.ToX12String());
                        HLID_2000D = HLID;
                        HLID++;
                        nm1 = new NM1();
                        nm1.NameQualifier = site.ProviderQualifier;
                        nm1.NameType = site.ProviderTypeQualifier;
                        nm1.LastName = site.ProviderLastName;
                        nm1.FirstName = site.ProviderFirstName;
                        nm1.MiddleName = site.ProviderMiddleName;
                        nm1.Suffix = site.ProviderSuffix;
                        nm1.IDQualifer = site.ProviderIdQualifier;
                        nm1.IDCode = site.ProviderId;
                        sb.Append(nm1.ToX12String());
                        goodDatabaseRecords++;
                        n2 = new N2_274();
                        n2.AdditionalName1 = site.ProviderAdditionalName1;
                        n2.AdditionalName2 = site.ProviderAdditionalName2;
                        sb.Append(n2.ToX12String());
                        per = new PER_274();
                        peritem = new PER274_Item();
                        peritem.FunctionCode = "AJ";
                        if (site.SiteContact1Qualifier1 == "TE") peritem.Phone = site.SiteContact1Number1;
                        if (site.SiteContact1Qualifier2 == "TE") peritem.Phone = site.SiteContact1Number2;
                        if (site.SiteContact1Qualifier3 == "TE") peritem.Phone = site.SiteContact1Number3;
                        if (site.SiteContact1Qualifier1 == "EM") peritem.Email = site.SiteContact1Number1;
                        if (site.SiteContact1Qualifier2 == "EM") peritem.Email = site.SiteContact1Number2;
                        if (site.SiteContact1Qualifier3 == "EM") peritem.Email = site.SiteContact1Number3;
                        if (site.SiteContact1Qualifier1 == "FX") peritem.Fax = site.SiteContact1Number1;
                        if (site.SiteContact1Qualifier2 == "FX") peritem.Fax = site.SiteContact1Number2;
                        if (site.SiteContact1Qualifier3 == "FX") peritem.Fax = site.SiteContact1Number3;
                        per.Pers.Add(peritem);
                        sb.Append(per.ToX12String());
                        if (!string.IsNullOrEmpty(site.ProviderLanguage1CodeQualifier)) 
                        {
                            LUI_274 lui = new LUI_274();
                            lui.LanguageQualifier = "LE";
                            lui.LanguageCode = site.ProviderLanguage1Code;
                            lui.LanguageProficiency = site.ProviderLanguage1ProficiencyInd;
                            sb.Append(lui.ToX12String());
                        }
                        if (!string.IsNullOrEmpty(site.ProviderLanguage2CodeQualifier))
                        {
                            LUI_274 lui = new LUI_274();
                            lui.LanguageQualifier = "LE";
                            lui.LanguageCode = site.ProviderLanguage2Code;
                            lui.LanguageProficiency = site.ProviderLanguage2ProficiencyInd;
                            sb.Append(lui.ToX12String());
                        }
                        if (!string.IsNullOrEmpty(site.ProviderLanguage3CodeQualifier))
                        {
                            LUI_274 lui = new LUI_274();
                            lui.LanguageQualifier = "LE";
                            lui.LanguageCode = site.ProviderLanguage3Code;
                            lui.LanguageProficiency = site.ProviderLanguage3ProficiencyInd;
                            sb.Append(lui.ToX12String());
                        }
                        if (!string.IsNullOrEmpty(site.ProviderLanguage4CodeQualifier))
                        {
                            LUI_274 lui = new LUI_274();
                            lui.LanguageQualifier = "LE";
                            lui.LanguageCode = site.ProviderLanguage4Code;
                            lui.LanguageProficiency = site.ProviderLanguage4ProficiencyInd;
                            sb.Append(lui.ToX12String());
                        }
                        if (!string.IsNullOrEmpty(site.ProviderLanguage5CodeQualifier))
                        {
                            LUI_274 lui = new LUI_274();
                            lui.LanguageQualifier = "LE";
                            lui.LanguageCode = site.ProviderLanguage5Code;
                            lui.LanguageProficiency = site.ProviderLanguage5ProficiencyInd;
                            sb.Append(lui.ToX12String());
                        }
                        if (!string.IsNullOrEmpty(site.ProviderLanguage6CodeQualifier))
                        {
                            LUI_274 lui = new LUI_274();
                            lui.LanguageQualifier = "LE";
                            lui.LanguageCode = site.ProviderLanguage6Code;
                            lui.LanguageProficiency = site.ProviderLanguage6ProficiencyInd;
                            sb.Append(lui.ToX12String());
                        }
                        if (!string.IsNullOrEmpty(site.ProviderLanguage7CodeQualifier))
                        {
                            LUI_274 lui = new LUI_274();
                            lui.LanguageQualifier = "LE";
                            lui.LanguageCode = site.ProviderLanguage7Code;
                            lui.LanguageProficiency = site.ProviderLanguage7ProficiencyInd;
                            sb.Append(lui.ToX12String());
                        }
                        if (!string.IsNullOrEmpty(site.ProviderLanguage8CodeQualifier))
                        {
                            LUI_274 lui = new LUI_274();
                            lui.LanguageQualifier = "LE";
                            lui.LanguageCode = site.ProviderLanguage8Code;
                            lui.LanguageProficiency = site.ProviderLanguage8ProficiencyInd;
                            sb.Append(lui.ToX12String());
                        }
                        if (!string.IsNullOrEmpty(site.ProviderLanguage9CodeQualifier))
                        {
                            LUI_274 lui = new LUI_274();
                            lui.LanguageQualifier = "LE";
                            lui.LanguageCode = site.ProviderLanguage9Code;
                            lui.LanguageProficiency = site.ProviderLanguage9ProficiencyInd;
                            sb.Append(lui.ToX12String());
                        }
                        if (!string.IsNullOrEmpty(site.ProviderContractEffectiveDate))
                        {
                            DTP dtp = new DTP();
                            dtp.DateCode = "092";
                            dtp.DateQualifier = "D8";
                            dtp.StartDate = site.ProviderContractEffectiveDate;
                            sb.Append(dtp.ToX12String());
                        }
                        if (!string.IsNullOrEmpty(site.ProviderContractExpirationDate))
                        {
                            DTP dtp = new DTP();
                            dtp.DateCode = "093";
                            dtp.DateQualifier = "D8";
                            dtp.StartDate = site.ProviderContractExpirationDate;
                            sb.Append(dtp.ToX12String());
                        }
                        List<P274SiteWorkSchedule> siteSchedules = schedules.Where(x => x.DetailId == site.DetailId).ToList();
                        foreach (P274SiteWorkSchedule schedule in siteSchedules) 
                        {
                            WS_274 ws = new WS_274();
                            ws.ShiftCode = schedule.ShiftCode;
                            ws.StartTime = schedule.StartTime;
                            ws.EndTime = schedule.EndTime;
                            sb.Append(ws.ToX12String());
                        }
                        List<P274SiteCRC> siteCrcs = crcs.Where(x => x.DetailId == site.DetailId).ToList();
                        foreach (P274SiteCRC crc in siteCrcs) 
                        {
                            CRC_274 crc274 = new CRC_274();
                            crc274.Category = crc.Category;
                            crc274.Condition = crc.Condition;
                            crc274.Code1 = crc.Code1;
                            crc274.Code2 = crc.Code2;
                            crc274.Code3 = crc.Code3;
                            crc274.Code4 = crc.Code4;
                            crc274.Code5 = crc.Code5;
                            sb.Append(crc274.ToX12String());
                        }
                        if (!string.IsNullOrEmpty(site.SiteRestrictionGender) || !string.IsNullOrEmpty(site.SiteRestrictionMaxAge) || !string.IsNullOrEmpty(site.SiteRestrictionMinAge)) 
                        {
                            PDI_274 pdi = new PDI_274();
                            pdi.RestrictionGender = site.SiteRestrictionGender;
                            pdi.RestrictionAgeMax = site.SiteRestrictionMaxAge;
                            pdi.RestrictionAgeMin = site.SiteRestrictionMinAge;
                            sb.Append(pdi.ToX12String());
                        }
                        NX1_274 nx1 = new NX1_274();
                        nx1.EntityIdCode = "77";
                        sb.Append(nx1.ToX12String());
                        N3 n3 = new N3();
                        n3.Address = site.SiteLocationAddress;
                        n3.Address2 = site.SiteLocationAddress2;
                        sb.Append(n3.ToX12String());
                        N4 n4 = new N4();
                        n4.City = site.SiteLocationCity;
                        n4.State = site.SiteLocationState;
                        n4.Zipcode = site.SiteLocationZip;
                        sb.Append(n4.ToX12String());
                        List<P274SpecializationArea> siteAreas = areas.Where(x => x.DetailId == site.DetailId).ToList();
                        foreach (P274SpecializationArea area in siteAreas)
                        {
                            LQ_274 lq = new LQ_274();
                            lq.TaxonomyQualifier = area.LQQualifier;
                            lq.TaxonomyCode = area.LQCode;
                            sb.Append(lq.ToX12String());
                            if (!string.IsNullOrEmpty(area.NetworkRoleCode1)) 
                            {
                                TPB_274 tpb = new TPB_274();
                                tpb.RoleCode = area.NetworkRoleCode1;
                                sb.Append(tpb.ToX12String());
                            }
                            if (!string.IsNullOrEmpty(area.NetworkRoleCode2)) 
                            {
                                TPB_274 tpb = new TPB_274();
                                tpb.RoleCode = area.NetworkRoleCode2;
                                sb.Append(tpb.ToX12String());
                            }
                        }
                        List<P274GroupIdNumber> siteReferences = groupIds.Where(x => x.DetailId == site.DetailId).ToList();
                        foreach (P274GroupIdNumber reference in siteReferences)
                        {
                            REF_274 ref274 = new REF_274();
                            ref274.ReferenceQualifier = reference.ReferenceQualifier;
                            ref274.ReferenceId = reference.ReferenceId;
                            sb.Append(ref274.ToX12String());
                        }
                        List<P274AffiliatedEntity> siteEntities = entities.Where(x => x.DetailId == site.DetailId).ToList();
                        foreach (P274AffiliatedEntity entity in siteEntities) 
                        {
                            nm1 = new NM1();
                            nm1.NameQualifier = entity.EntityQualifier;
                            nm1.NameType = "2";
                            nm1.LastName = entity.EntityLastName;
                            nm1.IDQualifer = entity.EntityIdQualifier;
                            nm1.IDCode = entity.EntityId;
                            sb.Append(nm1.ToX12String());
                            if (!string.IsNullOrEmpty(entity.EntityAdditionalName1)) 
                            {
                                n2 = new N2_274();
                                n2.AdditionalName1 = entity.EntityAdditionalName1;
                                n2.AdditionalName2 = entity.EntityAdditionalName2;
                                sb.Append(n2.ToX12String());
                            }
                        }
                        List<P274Detail> providers = details.Where(x => x.GroupId == group.GroupId && x.SiteId == site.SiteId && x.ProviderQualifier == "1P").ToList();
                        foreach (P274Detail provider in providers) 
                        {
                            hl = new HL_274();
                            hl.HLID = HLID.ToString();
                            hl.ParentID = HLID_2000D.ToString();
                            hl.LevelCode = "19";
                            hl.ChildCode = "0";
                            sb.Append(hl.ToX12String());
                            HLID_2000D = HLID;
                            HLID++;
                            nm1 = new NM1();
                            nm1.NameQualifier = provider.ProviderQualifier;
                            nm1.NameType = provider.ProviderTypeQualifier;
                            nm1.LastName = provider.ProviderLastName;
                            nm1.FirstName = provider.ProviderFirstName;
                            nm1.Suffix = provider.ProviderSuffix;
                            nm1.MiddleName = provider.ProviderMiddleName;
                            nm1.IDQualifer = provider.ProviderIdQualifier;
                            nm1.IDCode = provider.ProviderId;
                            sb.Append(nm1.ToX12String());
                            goodDatabaseRecords++;
                            if (!string.IsNullOrEmpty(provider.ProviderAdditionalName1)) 
                            {
                                n2 = new N2_274();
                                n2.AdditionalName1 = provider.ProviderAdditionalName1;
                                n2.AdditionalName2 = provider.ProviderAdditionalName2;
                                sb.Append(n2.ToX12String());
                            }
                            if (!string.IsNullOrEmpty(provider.ProviderDegreeCode1)) 
                            {
                                DEG_274 deg = new DEG_274();
                                deg.DegreeCode = provider.ProviderDegreeCode1;
                                deg.DegreeDescription = provider.ProviderDegreeDescription1;
                                sb.Append(deg.ToX12String());
                            }
                            if (!string.IsNullOrEmpty(provider.ProviderDegreeCode2))
                            {
                                DEG_274 deg = new DEG_274();
                                deg.DegreeCode = provider.ProviderDegreeCode2;
                                deg.DegreeDescription = provider.ProviderDegreeDescription2;
                                sb.Append(deg.ToX12String());
                            }
                            if (!string.IsNullOrEmpty(provider.ProviderDegreeCode3))
                            {
                                DEG_274 deg = new DEG_274();
                                deg.DegreeCode = provider.ProviderDegreeCode3;
                                deg.DegreeDescription = provider.ProviderDegreeDescription3;
                                sb.Append(deg.ToX12String());
                            }
                            if (!string.IsNullOrEmpty(provider.ProviderDegreeCode4))
                            {
                                DEG_274 deg = new DEG_274();
                                deg.DegreeCode = provider.ProviderDegreeCode4;
                                deg.DegreeDescription = provider.ProviderDegreeDescription4;
                                sb.Append(deg.ToX12String());
                            }
                            if (!string.IsNullOrEmpty(provider.ProviderDegreeCode5))
                            {
                                DEG_274 deg = new DEG_274();
                                deg.DegreeCode = provider.ProviderDegreeCode5;
                                deg.DegreeDescription = provider.ProviderDegreeDescription5;
                                sb.Append(deg.ToX12String());
                            }
                            if (!string.IsNullOrEmpty(provider.ProviderDegreeCode6))
                            {
                                DEG_274 deg = new DEG_274();
                                deg.DegreeCode = provider.ProviderDegreeCode6;
                                deg.DegreeDescription = provider.ProviderDegreeDescription6;
                                sb.Append(deg.ToX12String());
                            }
                            if (!string.IsNullOrEmpty(provider.ProviderDegreeCode7))
                            {
                                DEG_274 deg = new DEG_274();
                                deg.DegreeCode = provider.ProviderDegreeCode7;
                                deg.DegreeDescription = provider.ProviderDegreeDescription7;
                                sb.Append(deg.ToX12String());
                            }
                            if (!string.IsNullOrEmpty(provider.ProviderDegreeCode8))
                            {
                                DEG_274 deg = new DEG_274();
                                deg.DegreeCode = provider.ProviderDegreeCode8;
                                deg.DegreeDescription = provider.ProviderDegreeDescription8;
                                sb.Append(deg.ToX12String());
                            }
                            if (!string.IsNullOrEmpty(provider.ProviderDegreeCode9))
                            {
                                DEG_274 deg = new DEG_274();
                                deg.DegreeCode = provider.ProviderDegreeCode9;
                                deg.DegreeDescription = provider.ProviderDegreeDescription9;
                                sb.Append(deg.ToX12String());
                            }
                            if (!string.IsNullOrEmpty(provider.ProviderLanguage1CodeQualifier))
                            {
                                LUI_274 lui = new LUI_274();
                                lui.LanguageQualifier = "LE";
                                lui.LanguageCode = provider.ProviderLanguage1Code;
                                lui.LanguageProficiency = provider.ProviderLanguage1ProficiencyInd;
                                sb.Append(lui.ToX12String());
                            }
                            if (!string.IsNullOrEmpty(provider.ProviderLanguage2CodeQualifier))
                            {
                                LUI_274 lui = new LUI_274();
                                lui.LanguageQualifier = "LE";
                                lui.LanguageCode = provider.ProviderLanguage2Code;
                                lui.LanguageProficiency = provider.ProviderLanguage2ProficiencyInd;
                                sb.Append(lui.ToX12String());
                            }
                            if (!string.IsNullOrEmpty(provider.ProviderLanguage3CodeQualifier))
                            {
                                LUI_274 lui = new LUI_274();
                                lui.LanguageQualifier = "LE";
                                lui.LanguageCode = provider.ProviderLanguage3Code;
                                lui.LanguageProficiency = provider.ProviderLanguage3ProficiencyInd;
                                sb.Append(lui.ToX12String());
                            }
                            if (!string.IsNullOrEmpty(provider.ProviderLanguage4CodeQualifier))
                            {
                                LUI_274 lui = new LUI_274();
                                lui.LanguageQualifier = "LE";
                                lui.LanguageCode = provider.ProviderLanguage4Code;
                                lui.LanguageProficiency = provider.ProviderLanguage4ProficiencyInd;
                                sb.Append(lui.ToX12String());
                            }
                            if (!string.IsNullOrEmpty(provider.ProviderLanguage5CodeQualifier))
                            {
                                LUI_274 lui = new LUI_274();
                                lui.LanguageQualifier = "LE";
                                lui.LanguageCode = provider.ProviderLanguage5Code;
                                lui.LanguageProficiency = provider.ProviderLanguage5ProficiencyInd;
                                sb.Append(lui.ToX12String());
                            }
                            if (!string.IsNullOrEmpty(provider.ProviderLanguage6CodeQualifier))
                            {
                                LUI_274 lui = new LUI_274();
                                lui.LanguageQualifier = "LE";
                                lui.LanguageCode = provider.ProviderLanguage6Code;
                                lui.LanguageProficiency = provider.ProviderLanguage6ProficiencyInd;
                                sb.Append(lui.ToX12String());
                            }
                            if (!string.IsNullOrEmpty(provider.ProviderLanguage7CodeQualifier))
                            {
                                LUI_274 lui = new LUI_274();
                                lui.LanguageQualifier = "LE";
                                lui.LanguageCode = provider.ProviderLanguage7Code;
                                lui.LanguageProficiency = provider.ProviderLanguage7ProficiencyInd;
                                sb.Append(lui.ToX12String());
                            }
                            if (!string.IsNullOrEmpty(provider.ProviderLanguage8CodeQualifier))
                            {
                                LUI_274 lui = new LUI_274();
                                lui.LanguageQualifier = "LE";
                                lui.LanguageCode = provider.ProviderLanguage8Code;
                                lui.LanguageProficiency = provider.ProviderLanguage8ProficiencyInd;
                                sb.Append(lui.ToX12String());
                            }
                            if (!string.IsNullOrEmpty(provider.ProviderLanguage9CodeQualifier))
                            {
                                LUI_274 lui = new LUI_274();
                                lui.LanguageQualifier = "LE";
                                lui.LanguageCode = provider.ProviderLanguage9Code;
                                lui.LanguageProficiency = provider.ProviderLanguage9ProficiencyInd;
                                sb.Append(lui.ToX12String());
                            }
                            if (!string.IsNullOrEmpty(provider.ProviderContractEffectiveDate))
                            {
                                DTP dtp = new DTP();
                                dtp.DateCode = "092";
                                dtp.DateQualifier = "D8";
                                dtp.StartDate = provider.ProviderContractEffectiveDate;
                                sb.Append(dtp.ToX12String());
                            }
                            if (!string.IsNullOrEmpty(provider.ProviderContractExpirationDate))
                            {
                                DTP dtp = new DTP();
                                dtp.DateCode = "093";
                                dtp.DateQualifier = "D8";
                                dtp.StartDate = provider.ProviderContractExpirationDate;
                                sb.Append(dtp.ToX12String());
                            }
                            List<P274SpecializationArea> providerAreas = areas.Where(x => x.DetailId == provider.DetailId).ToList();
                            foreach (P274SpecializationArea area in providerAreas)
                            {
                                LQ_274 lq = new LQ_274();
                                lq.TaxonomyQualifier = area.LQQualifier;
                                lq.TaxonomyCode = area.LQCode;
                                sb.Append(lq.ToX12String());
                                if (!string.IsNullOrEmpty(area.NetworkRoleCode1))
                                {
                                    TPB_274 tpb = new TPB_274();
                                    tpb.RoleCode = area.NetworkRoleCode1;
                                    sb.Append(tpb.ToX12String());
                                }
                                if (!string.IsNullOrEmpty(area.NetworkRoleCode2))
                                {
                                    TPB_274 tpb = new TPB_274();
                                    tpb.RoleCode = area.NetworkRoleCode2;
                                    sb.Append(tpb.ToX12String());
                                }
                                if (!string.IsNullOrEmpty(area.CertificationConditionInd)) 
                                {
                                    YNQ_274 ynq = new YNQ_274();
                                    ynq.ConditionIndicator = area.CertificationConditionInd;
                                    ynq.ResponseCode = area.BoardCertificationInd;
                                    sb.Append(ynq.ToX12String());
                                }
                            }
                            List<P274GroupIdNumber> providerReferences = groupIds.Where(x => x.DetailId == provider.DetailId).ToList();
                            foreach (P274GroupIdNumber reference in providerReferences)
                            {
                                REF_274 ref274 = new REF_274();
                                ref274.ReferenceQualifier = reference.ReferenceQualifier;
                                ref274.ReferenceId = reference.ReferenceId;
                                sb.Append(ref274.ToX12String());
                            }

                        }
                    }
                }
                SE se = new SE();
                se.SegmentCount = (sb.ToString().Count(x => x == '~')-1).ToString();
                se.TransactionControlNumber = icn;
                sb.Append(se.ToX12String());
                GE ge = new GE();
                ge.NumberofTransactionSets = "1";
                ge.GroupControlNumber = icn;
                sb.Append(ge.ToX12String());
                IEA iea = new IEA();
                iea.NumberofFunctionalGroups = "1";
                iea.InterchangeControlNumber = icn;
                sb.Append(iea.ToX12String());

                System.IO.File.WriteAllText(Path.Combine(DestinationFolder, "IEHP_" + CountyCode + "_274_" + DateTime.Today.ToString("yyyyMMdd") + "_00001.dat"), sb.ToString());
            }

            result.Add(_context.Details.Count().ToString());
            result.Add(goodDatabaseRecords.ToString());
            return result;
        }
    }
}
