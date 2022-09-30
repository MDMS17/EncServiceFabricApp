using EncDataModel.Submission837;
using Export837.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X12Lib;

namespace Export837.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Export837Controller : ControllerBase
    {
        private readonly ILogger<Export837Controller> _logger;
        private readonly Sub837Context _context;
        public Export837Controller(ILogger<Export837Controller> logger, Sub837Context context)
        {
            _logger = logger;
            _context = context;
        }
        //Export837, initial query total records
        [HttpGet]
        public List<string> GetCountsForExport()
        {
            List<string> result = new List<string>();
            _logger.Log(LogLevel.Information, "query total counts for export 837");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string DestinationFolder = configuration["Destination837"];
            result.Add(DestinationFolder);
            //CMS professional
            result.Add(_context.ClaimHeaders.Count(x => x.ExportType == "CMSP").ToString());
            //CMS Institutional
            result.Add(_context.ClaimHeaders.Count(x => x.ExportType == "CMSI").ToString());
            //CMS DME
            result.Add(_context.ClaimHeaders.Count(x => x.ExportType == "CMSE").ToString());
            //CMD Dental
            //result.Add(_context.ClaimHeaders.Count(x => x.ExportType == "CMCD").ToString());
            //Dual Professional
            result.Add(_context.ClaimHeaders.Count(x => x.ExportType == "CCIP").ToString());
            //Dual Institutioal
            result.Add(_context.ClaimHeaders.Count(x => x.ExportType == "CCII").ToString());
            //Dual DME
            result.Add(_context.ClaimHeaders.Count(x => x.ExportType == "CCIE").ToString());
            //Dual Dental
            //result.Add(_context.ClaimHeaders.Count(x => x.ExportType == "CCID").ToString());
            //State Professional
            result.Add(_context.ClaimHeaders.Count(x => x.ExportType == "DHCSP").ToString());
            //State Institutioanl
            result.Add(_context.ClaimHeaders.Count(x => x.ExportType == "DHCSI").ToString());
            //State DME
            result.Add(_context.ClaimHeaders.Count(x => x.ExportType == "DHCSE").ToString());
            //State Dental
            //result.Add(_context.ClaimHeaders.Count(x => x.ExportType == "DHCSD").ToString());
            return result;
        }
        //Export837
        [HttpPost]
        public List<string> Export837Files([FromBody] List<string> items)
        {
            List<string> result = new List<string>();
            _logger.Log(LogLevel.Information, "Export 837 Files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string DestinationFolder = configuration["Destination837"];
            int totalDatabaseRecords = _context.ClaimHeaders.Count();
            int goodDatabaseRecords = 0;
            string ProductionFlag = "P";
            if (items[0] != "1") ProductionFlag = "T";
            if (items[1] == "True")
            {
                //CMS professional
                if (_context.ClaimHeaders.Count(x => x.ExportType == "CMSP") > 0)
                {
                    for (int page = 0; page <= _context.ClaimHeaders.Count(x => x.ExportType == "CMSP") / 5000; page++)
                    {
                        List<ClaimHeader> headers = _context.ClaimHeaders.Where(x=>x.ExportType=="CMSP").OrderBy(x => x.ID).Skip(5000 * page).Take(5000).ToList();
                        List<string> ClaimIds = headers.Select(x => x.ClaimID).ToList();
                        List<ClaimCAS> cases = _context.ClaimCAS.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimCRC> crcs = _context.ClaimCRCs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimLineFRM> frms = _context.ClaimLineFRMs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimHI> his = _context.ClaimHIs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimK3> k3s = _context.ClaimK3s.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimLineLQ> lqs = _context.ClaimLineLQs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimLineMEA> meas = _context.ClaimLineMEAs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimNte> notes = _context.ClaimNtes.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimPatient> patients = _context.ClaimPatients.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimProvider> providers = _context.ClaimProviders.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimPWK> pwks = _context.ClaimPWKs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimSBR> sbrs = _context.ClaimSBRs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimLineSVD> svds = _context.ClaimLineSVDs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimSecondaryIdentification> secondaryidentifications = _context.ClaimSecondaryIdentifications.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ProviderContact> providercontacts = _context.ProviderContacts.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ServiceLine> servicelines = _context.ServiceLines.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();

                        System.IO.File.WriteAllText(Path.Combine(DestinationFolder, "Prefix_837P_" + DateTime.Today.ToString("yyyyMMdd") + "_" + page.ToString().PadLeft(3, '0')), 
                            X12Exporter.ExportCMSP(ref headers,ref cases,ref crcs,ref his,ref k3s,ref frms,ref lqs,ref meas,ref svds,ref notes,ref patients,ref providers,ref pwks,ref sbrs,ref secondaryidentifications,ref providercontacts,ref servicelines, ProductionFlag));
                        goodDatabaseRecords += headers.Count;
                    }
                }
            }
            if (items[2] == "True")
            {
                //CMS institutional
                if (_context.ClaimHeaders.Count(x => x.ExportType == "CMSI") > 0)
                {
                    for (int page = 0; page <= _context.ClaimHeaders.Count(x => x.ExportType == "CMSI") / 5000; page++)
                    {
                        List<ClaimHeader> headers = _context.ClaimHeaders.Where(x => x.ExportType == "CMSI").OrderBy(x => x.ID).Skip(5000 * page).Take(5000).ToList();
                        List<string> ClaimIds = headers.Select(x => x.ClaimID).ToList();
                        List<ClaimCAS> cases = _context.ClaimCAS.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimCRC> crcs = _context.ClaimCRCs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimLineFRM> frms = _context.ClaimLineFRMs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimHI> his = _context.ClaimHIs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimK3> k3s = _context.ClaimK3s.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimLineLQ> lqs = _context.ClaimLineLQs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimLineMEA> meas = _context.ClaimLineMEAs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimNte> notes = _context.ClaimNtes.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimPatient> patients = _context.ClaimPatients.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimProvider> providers = _context.ClaimProviders.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimPWK> pwks = _context.ClaimPWKs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimSBR> sbrs = _context.ClaimSBRs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimLineSVD> svds = _context.ClaimLineSVDs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimSecondaryIdentification> secondaryidentifications = _context.ClaimSecondaryIdentifications.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ProviderContact> providercontacts = _context.ProviderContacts.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ServiceLine> servicelines = _context.ServiceLines.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();

                        System.IO.File.WriteAllText(Path.Combine(DestinationFolder, "Prefix_837I_" + DateTime.Today.ToString("yyyyMMdd") + "_" + page.ToString().PadLeft(3, '0')),
                            X12Exporter.ExportCMSI(ref headers, ref cases, ref crcs, ref his, ref k3s, ref frms, ref lqs, ref meas, ref svds, ref notes, ref patients, ref providers, ref pwks, ref sbrs, ref secondaryidentifications, ref providercontacts, ref servicelines, ProductionFlag));
                        goodDatabaseRecords += headers.Count;
                    }
                }
            }
            if (items[3] == "True")
            {
                //CMS DME
                if (_context.ClaimHeaders.Count(x => x.ExportType == "CMSE") > 0)
                {
                    for (int page = 0; page <= _context.ClaimHeaders.Count(x => x.ExportType == "CMSE") / 5000; page++)
                    {
                        List<ClaimHeader> headers = _context.ClaimHeaders.Where(x => x.ExportType == "CMSE").OrderBy(x => x.ID).Skip(5000 * page).Take(5000).ToList();
                        List<string> ClaimIds = headers.Select(x => x.ClaimID).ToList();
                        List<ClaimCAS> cases = _context.ClaimCAS.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimCRC> crcs = _context.ClaimCRCs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimLineFRM> frms = _context.ClaimLineFRMs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimHI> his = _context.ClaimHIs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimK3> k3s = _context.ClaimK3s.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimLineLQ> lqs = _context.ClaimLineLQs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimLineMEA> meas = _context.ClaimLineMEAs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimNte> notes = _context.ClaimNtes.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimPatient> patients = _context.ClaimPatients.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimProvider> providers = _context.ClaimProviders.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimPWK> pwks = _context.ClaimPWKs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimSBR> sbrs = _context.ClaimSBRs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimLineSVD> svds = _context.ClaimLineSVDs.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ClaimSecondaryIdentification> secondaryidentifications = _context.ClaimSecondaryIdentifications.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ProviderContact> providercontacts = _context.ProviderContacts.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();
                        List<ServiceLine> servicelines = _context.ServiceLines.Where(x => ClaimIds.Contains(x.ClaimID)).ToList();

                        System.IO.File.WriteAllText(Path.Combine(DestinationFolder, "Prefix_837E_" + DateTime.Today.ToString("yyyyMMdd") + "_" + page.ToString().PadLeft(3, '0')),
                            X12Exporter.ExportCMSE(ref headers, ref cases, ref crcs, ref his, ref k3s, ref frms, ref lqs, ref meas, ref svds, ref notes, ref patients, ref providers, ref pwks, ref sbrs, ref secondaryidentifications, ref providercontacts, ref servicelines, ProductionFlag));
                        goodDatabaseRecords += headers.Count;
                    }
                }
                if (items[4] == "True")
                {
                    //Dual professional
                }
                if (items[5] == "True")
                {
                    //Dual institutional
                }
                if (items[6] == "True")
                {
                    //Dual DME
                }
                if (items[7] == "True")
                {
                    //State professional
                }
                if (items[8] == "True")
                {
                    //State institutional
                }
                if (items[9] == "True")
                {
                    //State DME
                }
            }
            result.Add(totalDatabaseRecords.ToString());
            result.Add(goodDatabaseRecords.ToString());
            return result;
        }
    }
}