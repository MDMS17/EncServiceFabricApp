using EncDataModel.MCPDIP;
using JsonLib;
using LoadMCPDIPResponse.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoadMCPDIPResponse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoadMCPDIPResponseController : ControllerBase
    {
        private readonly ILogger<LoadMCPDIPResponseController> _logger;
        private readonly HistoryContext _contextHistory;
        private readonly LogContext _contextLog;
        private readonly ResponseContext _contextResponse;
        public LoadMCPDIPResponseController(ILogger<LoadMCPDIPResponseController> logger, HistoryContext contextHistory, LogContext contextLog, ResponseContext contextResponse)
        {
            _logger = logger;
            _contextHistory = contextHistory;
            _contextLog = contextLog;
            _contextResponse = contextResponse;
        }
        //LoadMCPDIPResponse
        [HttpGet]
        public List<string> GetNewMCPDIPResponseFiles()
        {
            _logger.Log(LogLevel.Information, "inquiry unprocessed files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_MCPDIP"];
            string archivePath = configuration["Archive_MCPDIP"];
            List<string> result = Directory.EnumerateFiles(sourcePath).Select(x => Path.GetFileName(x)).ToList();
            result.Insert(0, archivePath);
            result.Insert(0, sourcePath);
            return result;
        }
        //LoadMCPDIPResponse/1
        [HttpGet("{id}")]
        public List<string> ProcessMCPDIPResponseFiles(long id)
        {
            List<string> result = new List<string>();
            _logger.Log(LogLevel.Information, "process new MCPDIP response files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_MCPDIP"];
            string archivePath = configuration["Archive_MCPDIP"];
            string outboundPath = configuration["Outbouynd_MCPDIP"];
            DirectoryInfo di = new DirectoryInfo(sourcePath);
            int totalFiles = 0;
            int goodFiles = 0;
            foreach (FileInfo fi in di.GetFiles())
            {
                if (fi.Name.ToUpper().Contains("MCPD"))
                {
                    ProcessMCPD(fi);
                    SplitMCPD(outboundPath, "Kaiser");
                    goodFiles++;
                }
                else if (fi.Name.ToUpper().Contains("PCPA"))
                {
                    ProcessPCPA(fi);
                    SplitPCPA(outboundPath, "Kaiser");
                    goodFiles++;
                }
                string destinationFileName = Path.Combine(archivePath, fi.Name);
                if (System.IO.File.Exists(destinationFileName)) System.IO.File.Delete(destinationFileName);
                fi.MoveTo(destinationFileName);
                totalFiles++;
            }

            result.Add(totalFiles.ToString());
            result.Add(goodFiles.ToString());
            return result;
        }
        private void ProcessPCPA(FileInfo fi)
        {
            ResponseFile pcpaResponseFile = new ResponseFile();

            pcpaResponseFile.responseHierarchy = new List<ResponseHierarchy>();
            pcpaResponseFile.responseHierarchy.Add(new ResponseHierarchy { levelIdentifier = "File", sectionIdentifier = null, responses = new List<ResponseDetail>(), children = new List<ResponseChildren>() });

            pcpaResponseFile.responseHierarchy[0].children.Add(new ResponseChildren { levelIdentifier = "Header", sectionIdentifier = null, responses = new List<ResponseDetail>() });
            pcpaResponseFile.responseHierarchy[0].children.Add(new ResponseChildren { levelIdentifier = "PCPA", sectionIdentifier = null, responses = new List<ResponseDetail>() });


            string ss2 = System.IO.File.ReadAllText(fi.FullName);
            pcpaResponseFile = JsonLib.JsonDeserialize.DeserializeReponseFile(ref ss2);
            //initialize contexts, if cancellation requested, no further process
            SubmissionLog slog = _contextLog.SubmissionLogs.FirstOrDefault(x => x.FileName == pcpaResponseFile.fileName);
            long pcpaPointer = 0;
            PcpHeader header = null;
            if (slog != null)
            {
                header = _contextHistory.PcpHeaders.FirstOrDefault(x => x.ReportingPeriod.Substring(0, 6) == slog.RecordYear + slog.RecordMonth);
                pcpaPointer = _contextHistory.PcpAssignments.Where(x => x.PcpHeaderId == header.PcpHeaderId).Min(x => x.PcpAssignmentId);
            }
            McpdipHeader rHeader = new McpdipHeader
            {
                FileName = pcpaResponseFile.fileName,
                FileType = pcpaResponseFile.fileType,
                SubmitterName = pcpaResponseFile.submitterName,
                SubmissionDate = pcpaResponseFile.submissionDate,
                ValidationStatus = pcpaResponseFile.validationStatus,
                Levels = pcpaResponseFile.levels,
                SchemaVersion = pcpaResponseFile.schemaVersion
            };
            if (slog != null)
            {
                rHeader.RecordYear = slog.RecordYear;
                rHeader.RecordMonth = slog.RecordMonth;
            }
            _contextResponse.McpdipHeaders.Add(rHeader);
            _contextResponse.SaveChanges();

            McpdipHierarchy hierarchy = new McpdipHierarchy
            {
                HeaderId = rHeader.HeaderId,
                LevelIdentifier = pcpaResponseFile.responseHierarchy[0].levelIdentifier,
                SectionIdentifier = pcpaResponseFile.responseHierarchy[0].sectionIdentifier
            };
            _contextResponse.McpdipHierarchies.Add(hierarchy);
            _contextResponse.SaveChanges();
            if (pcpaResponseFile.responseHierarchy[0].responses.Count > 0)
            {
                foreach (var response in pcpaResponseFile.responseHierarchy[0].responses)
                {
                    McpdipDetail detail = new McpdipDetail
                    {
                        ResponseTarget = "PcpaFile",
                        ChildrenId = hierarchy.HierarchyId,
                        ItemReference = response.itemReference,
                        Id = response.id,
                        Description = response.description,
                        Severity = response.severity
                    };
                    _contextResponse.McpdipDetails.Add(detail);
                }
                _contextResponse.SaveChanges();
            }
            foreach (var child in pcpaResponseFile.responseHierarchy[0].children)
            {
                McpdipChildren rChild = new McpdipChildren
                {
                    HierarchyId = hierarchy.HierarchyId,
                    LevelIdentifier = child.levelIdentifier == "Header" ? "PcpaHeader" : child.levelIdentifier,
                    SectionIDentifier = child.sectionIdentifier
                };
                _contextResponse.McpdipChildrens.Add(rChild);
                _contextResponse.SaveChanges();
                HashSet<int> LineNumbers = new HashSet<int>();
                if (child.responses.Count > 0)
                {
                    foreach (var response in child.responses)
                    {
                        McpdipDetail detail = new McpdipDetail
                        {
                            ResponseTarget = rChild.LevelIdentifier == "Header" ? "PcpaHeader" : rChild.LevelIdentifier,
                            ChildrenId = rChild.ChildrenId,
                            ItemReference = response.itemReference,
                            Id = response.id,
                            Description = response.description,
                            Severity = response.severity
                        };
                        if (rChild.LevelIdentifier != "Header" && slog != null)
                        {
                            detail.OriginalTable = "History.PcpAssignment";
                            int RecordNumber = -1;
                            if (!int.TryParse(response.itemReference.Split('[', ']')[1], out RecordNumber)) RecordNumber = -1;
                            if (RecordNumber >= 0)
                            {
                                detail.OriginalId = pcpaPointer + RecordNumber;
                                PcpAssignment assignment = _contextHistory.PcpAssignments.Find(detail.OriginalId);
                                if (assignment != null)
                                {
                                    detail.OriginalCin = assignment.Cin;
                                    detail.OriginalItemId = assignment.Npi;
                                    detail.OriginalDataSource = assignment.DataSource;
                                }
                                if (detail.Severity == "Error" && !LineNumbers.Contains(RecordNumber)) LineNumbers.Add(RecordNumber);
                            }
                        }
                        _contextResponse.McpdipDetails.Add(detail);
                    }
                    _contextResponse.SaveChanges();
                    if (slog != null)
                    {
                        slog.TotalPCPARejected = LineNumbers.Count();
                        slog.TotalPCPAAccepted = slog.TotalPCPASubmitted - slog.TotalPCPARejected;
                        slog.UpdateDate = DateTime.Now;
                        slog.UpdateBy = Environment.UserName;
                        _contextLog.SaveChanges();
                    }
                }
            }
        }
        private void ProcessMCPD(FileInfo fi)
        {
            ResponseFile mcpdResponseFile = new ResponseFile();

            mcpdResponseFile.responseHierarchy = new List<ResponseHierarchy>();
            mcpdResponseFile.responseHierarchy.Add(new ResponseHierarchy { levelIdentifier = "File", sectionIdentifier = null, responses = new List<ResponseDetail>(), children = new List<ResponseChildren>() });

            mcpdResponseFile.responseHierarchy[0].children.Add(new ResponseChildren { levelIdentifier = "Header", sectionIdentifier = null, responses = new List<ResponseDetail>() });
            mcpdResponseFile.responseHierarchy[0].children.Add(new ResponseChildren { levelIdentifier = "Grievances", sectionIdentifier = null, responses = new List<ResponseDetail>() });
            mcpdResponseFile.responseHierarchy[0].children.Add(new ResponseChildren { levelIdentifier = "Appeals", sectionIdentifier = null, responses = new List<ResponseDetail>() });
            mcpdResponseFile.responseHierarchy[0].children.Add(new ResponseChildren { levelIdentifier = "ContinuityOfCare", sectionIdentifier = null, responses = new List<ResponseDetail>() });
            mcpdResponseFile.responseHierarchy[0].children.Add(new ResponseChildren { levelIdentifier = "OutOfNetwork", sectionIdentifier = null, responses = new List<ResponseDetail>() });


            string ss2 = System.IO.File.ReadAllText(fi.FullName);
            mcpdResponseFile = JsonLib.JsonDeserialize.DeserializeReponseFile(ref ss2);
            //initialize contexts, if cancellation requested, no further process
            long grievancePointer = 0, appealPointer = 0, cocPointer = 0, oonPointer = 0;
            McpdHeader header = null;
            SubmissionLog slog = _contextLog.SubmissionLogs.FirstOrDefault(x => x.FileName == mcpdResponseFile.fileName);
            if (slog != null)
            {
                header = _contextHistory.McpdHeaders.FirstOrDefault(x => x.ReportingPeriod.Substring(0, 6) == slog.RecordYear + slog.RecordMonth);
                grievancePointer = _contextHistory.Grievances.Where(x => x.McpdHeaderId == header.McpdHeaderId).Min(x => x.McpdGrievanceId);
                appealPointer = _contextHistory.Appeals.Where(x => x.McpdHeaderId == header.McpdHeaderId).Min(x => x.McpdAppealId);
                cocPointer = _contextHistory.McpdContinuityOfCare.Where(x => x.McpdHeaderId == header.McpdHeaderId).Min(x => x.McpdContinuityOfCareId);
                oonPointer = _contextHistory.McpdOutOfNetwork.Where(x => x.McpdHeaderId == header.McpdHeaderId).Min(x => x.McpdOutOfNetworkId);
            }
            McpdipHeader rHeader = new McpdipHeader
            {
                FileName = mcpdResponseFile.fileName,
                FileType = mcpdResponseFile.fileType,
                SubmitterName = mcpdResponseFile.submitterName,
                SubmissionDate = mcpdResponseFile.submissionDate,
                ValidationStatus = mcpdResponseFile.validationStatus,
                Levels = mcpdResponseFile.levels,
                SchemaVersion = mcpdResponseFile.schemaVersion
            };
            if (slog != null)
            {
                rHeader.RecordYear = slog.RecordYear;
                rHeader.RecordMonth = slog.RecordMonth;
            }
            _contextResponse.McpdipHeaders.Add(rHeader);
            _contextResponse.SaveChanges();

            McpdipHierarchy hierarchy = new McpdipHierarchy
            {
                HeaderId = rHeader.HeaderId,
                LevelIdentifier = mcpdResponseFile.responseHierarchy[0].levelIdentifier,
                SectionIdentifier = mcpdResponseFile.responseHierarchy[0].sectionIdentifier
            };
            _contextResponse.McpdipHierarchies.Add(hierarchy);
            _contextResponse.SaveChanges();
            if (mcpdResponseFile.responseHierarchy[0].responses.Count > 0)
            {
                foreach (var response in mcpdResponseFile.responseHierarchy[0].responses)
                {
                    McpdipDetail detail = new McpdipDetail
                    {
                        ResponseTarget = "McpdFile",
                        ChildrenId = hierarchy.HierarchyId,
                        ItemReference = response.itemReference,
                        Id = response.id,
                        Description = response.description,
                        Severity = response.severity
                    };
                    _contextResponse.McpdipDetails.Add(detail);
                }
                _contextResponse.SaveChanges();
            }
            HashSet<int> LineNumberG = new HashSet<int>();
            HashSet<int> LineNumberA = new HashSet<int>();
            HashSet<int> LineNumberC = new HashSet<int>();
            HashSet<int> LineNumberO = new HashSet<int>();
            foreach (var child in mcpdResponseFile.responseHierarchy[0].children)
            {
                McpdipChildren rChild = new McpdipChildren
                {
                    HierarchyId = hierarchy.HierarchyId,
                    LevelIdentifier = child.levelIdentifier == "Header" ? "McpdHeader" : child.levelIdentifier,
                    SectionIDentifier = child.sectionIdentifier
                };
                _contextResponse.McpdipChildrens.Add(rChild);
                _contextResponse.SaveChanges();
                if (child.responses.Count > 0)
                {
                    foreach (var response in child.responses)
                    {
                        McpdipDetail detail = new McpdipDetail
                        {
                            ResponseTarget = rChild.LevelIdentifier == "Header" ? "McpdHeader" : rChild.LevelIdentifier,
                            ChildrenId = rChild.ChildrenId,
                            ItemReference = response.itemReference,
                            Id = response.id,
                            Description = response.description,
                            Severity = response.severity
                        };
                        if (rChild.LevelIdentifier != "Header" && slog != null)
                        {
                            int RecordNumber = -1;
                            if (!int.TryParse(response.itemReference.Split('[', ']')[1], out RecordNumber)) RecordNumber = -1;
                            switch (rChild.LevelIdentifier)
                            {
                                case "Grievances":
                                    detail.OriginalTable = "History.McpdGrievance";
                                    if (RecordNumber >= 0)
                                    {
                                        detail.OriginalId = grievancePointer + RecordNumber;
                                        McpdGrievance grievance = _contextHistory.Grievances.Find(detail.OriginalId);
                                        if (grievance != null)
                                        {
                                            detail.OriginalCin = grievance.Cin;
                                            detail.OriginalItemId = grievance.GrievanceId;
                                            detail.OriginalDataSource = grievance.DataSource;
                                        }
                                        if (detail.Severity == "Error" && !LineNumberG.Contains(RecordNumber)) LineNumberG.Add(RecordNumber);
                                    }
                                    break;
                                case "Appeals":
                                    detail.OriginalTable = "History.McpdAppeal";
                                    if (RecordNumber >= 0)
                                    {
                                        detail.OriginalId = appealPointer + RecordNumber;
                                        McpdAppeal appeal = _contextHistory.Appeals.Find(detail.OriginalId);
                                        if (appeal != null)
                                        {
                                            detail.OriginalCin = appeal.Cin;
                                            detail.OriginalItemId = appeal.AppealId;
                                            detail.OriginalDataSource = appeal.DataSource;
                                        }
                                        if (detail.Severity == "Error" && !LineNumberA.Contains(RecordNumber)) LineNumberA.Add(RecordNumber);
                                    }
                                    break;
                                case "ContinuityOfCare":
                                    detail.OriginalTable = "History.McpdContinuityOfCare";
                                    if (RecordNumber >= 0)
                                    {
                                        detail.OriginalId = cocPointer + RecordNumber;
                                        McpdContinuityOfCare coc = _contextHistory.McpdContinuityOfCare.Find(detail.OriginalId);
                                        if (coc != null)
                                        {
                                            detail.OriginalCin = coc.Cin;
                                            detail.OriginalItemId = coc.CocId;
                                            detail.OriginalDataSource = coc.DataSource;
                                        }
                                        if (detail.Severity == "Error" && !LineNumberC.Contains(RecordNumber)) LineNumberC.Add(RecordNumber);
                                    }
                                    break;
                                case "OutOfNetwork":
                                    detail.OriginalTable = "History.McpdOutOfNetwork";
                                    if (RecordNumber >= 0)
                                    {
                                        detail.OriginalId = oonPointer + RecordNumber;
                                        McpdOutOfNetwork oon = _contextHistory.McpdOutOfNetwork.Find(detail.OriginalId);
                                        if (oon != null)
                                        {
                                            detail.OriginalCin = oon.Cin;
                                            detail.OriginalItemId = oon.OonId;
                                            detail.OriginalDataSource = oon.DataSource;
                                        }
                                        if (detail.Severity == "Error" && !LineNumberO.Contains(RecordNumber)) LineNumberO.Add(RecordNumber);
                                    }
                                    break;
                            }
                        }
                        _contextResponse.McpdipDetails.Add(detail);
                    }
                    _contextResponse.SaveChanges();
                    if (slog != null)
                    {
                        slog.TotalGrievanceRejected = LineNumberG.Count();
                        slog.TotalGrievanceAccepted = slog.TotalGrievanceSubmitted - slog.TotalGrievanceRejected;
                        slog.TotalAppealRejected = LineNumberA.Count();
                        slog.TotalAppealAccepted = slog.TotalAppealSubmitted - slog.TotalAppealRejected;
                        slog.TotalCOCRejected = LineNumberC.Count();
                        slog.TotalCOCAccepted = slog.TotalCOCSubmitted - slog.TotalCOCRejected;
                        slog.TotalOONRejected = LineNumberO.Count();
                        slog.TotalOONAccepted = slog.TotalOONSubmitted - slog.TotalOONRejected;
                        slog.UpdateDate = DateTime.Now;
                        slog.UpdateBy = Environment.UserName;
                        _contextLog.SaveChanges();
                    }
                }
            }
        }
        private void SplitPCPA(string OutboundFolder, string IpaName)
        {
            McpdipHeader rHeader = _contextResponse.McpdipHeaders.Where(x => x.FileType == "PCPA").OrderByDescending(x => x.HeaderId).FirstOrDefault();
            McpdipHierarchy hierarchy = _contextResponse.McpdipHierarchies.FirstOrDefault(x => x.HeaderId == rHeader.HeaderId);
            McpdipChildren childrenHeader = _contextResponse.McpdipChildrens.FirstOrDefault(x => x.HierarchyId == hierarchy.HierarchyId && x.LevelIdentifier == "PcpaHeader");
            McpdipChildren childrenPcpa = _contextResponse.McpdipChildrens.FirstOrDefault(x => x.HierarchyId == hierarchy.HierarchyId && x.LevelIdentifier == "PCPA");
            ResponseFile mcpdResponseFile = new ResponseFile
            {
                fileName = rHeader.FileName,
                fileType = rHeader.FileType,
                levels = rHeader.Levels,
                schemaVersion = rHeader.SchemaVersion,
                submissionDate = rHeader.SubmissionDate,
                submitterName = rHeader.SubmitterName,
                validationStatus = rHeader.ValidationStatus
            };
            mcpdResponseFile.responseHierarchy = new List<ResponseHierarchy>();
            mcpdResponseFile.responseHierarchy.Add(new ResponseHierarchy { levelIdentifier = "File", sectionIdentifier = null, responses = new List<ResponseDetail>(), children = new List<ResponseChildren>() });

            mcpdResponseFile.responseHierarchy[0].children.Add(new ResponseChildren { levelIdentifier = "Header", sectionIdentifier = null, responses = new List<ResponseDetail>() });
            mcpdResponseFile.responseHierarchy[0].children.Add(new ResponseChildren { levelIdentifier = "PCPA", sectionIdentifier = null, responses = new List<ResponseDetail>() });

            foreach (var detail in _contextResponse.McpdipDetails.Where(x => x.ResponseTarget == "PcpaFile" && x.ChildrenId == hierarchy.HierarchyId))
            {
                mcpdResponseFile.responseHierarchy[0].responses.Add(new ResponseDetail
                {
                    description = detail.Description,
                    id = detail.Id,
                    itemReference = detail.ItemReference,
                    severity = detail.Severity
                });
            }
            foreach (var detail in _contextResponse.McpdipDetails.Where(x => x.ResponseTarget == "PcpaHeader" && x.ChildrenId == childrenHeader.ChildrenId))
            {
                mcpdResponseFile.responseHierarchy[0].children[0].responses.Add(new ResponseDetail
                {
                    description = detail.Description,
                    id = detail.Id,
                    itemReference = detail.ItemReference,
                    severity = detail.Severity
                });
            }
            foreach (var detail in _contextResponse.McpdipDetails.Where(x => x.ResponseTarget == "PCPA" && x.ChildrenId == childrenPcpa.ChildrenId && x.OriginalDataSource == IpaName))
            {
                mcpdResponseFile.responseHierarchy[0].children[1].responses.Add(new ResponseDetail
                {
                    description = detail.Description,
                    id = detail.Id,
                    itemReference = detail.ItemReference,
                    severity = detail.Severity
                });
            }

            string destinationFileName = Path.Combine(OutboundFolder, IpaName, rHeader.FileName.Replace(".json", "_RESP_Kaiser.json"));

            System.IO.File.WriteAllText(destinationFileName, JsonOperations.GetResponseJson(ref mcpdResponseFile));
        }
        private void SplitMCPD(string OutboundFolder, string IpaName)
        {
            McpdipHeader rHeader = _contextResponse.McpdipHeaders.Where(x => x.FileType == "MCPD").OrderByDescending(x => x.HeaderId).FirstOrDefault();
            McpdipHierarchy hierarchy = _contextResponse.McpdipHierarchies.FirstOrDefault(x => x.HeaderId == rHeader.HeaderId);
            McpdipChildren childrenHeader = _contextResponse.McpdipChildrens.FirstOrDefault(x => x.HierarchyId == hierarchy.HierarchyId && x.LevelIdentifier == "McpdHeader");
            McpdipChildren childrenGrievance = _contextResponse.McpdipChildrens.FirstOrDefault(x => x.HierarchyId == hierarchy.HierarchyId && x.LevelIdentifier == "Grievances");
            McpdipChildren childrenAppeal = _contextResponse.McpdipChildrens.FirstOrDefault(x => x.HierarchyId == hierarchy.HierarchyId && x.LevelIdentifier == "Appeals");
            McpdipChildren childrenCoc = _contextResponse.McpdipChildrens.FirstOrDefault(x => x.HierarchyId == hierarchy.HierarchyId && x.LevelIdentifier == "ContinuityOfCare");
            McpdipChildren childrenOon = _contextResponse.McpdipChildrens.FirstOrDefault(x => x.HierarchyId == hierarchy.HierarchyId && x.LevelIdentifier == "OutOfNetwork");
            ResponseFile mcpdResponseFile = new ResponseFile
            {
                fileName = rHeader.FileName,
                fileType = rHeader.FileType,
                levels = rHeader.Levels,
                schemaVersion = rHeader.SchemaVersion,
                submissionDate = rHeader.SubmissionDate,
                submitterName = rHeader.SubmitterName,
                validationStatus = rHeader.ValidationStatus
            };
            mcpdResponseFile.responseHierarchy = new List<ResponseHierarchy>();
            mcpdResponseFile.responseHierarchy.Add(new ResponseHierarchy { levelIdentifier = "File", sectionIdentifier = null, responses = new List<ResponseDetail>(), children = new List<ResponseChildren>() });

            mcpdResponseFile.responseHierarchy[0].children.Add(new ResponseChildren { levelIdentifier = "Header", sectionIdentifier = null, responses = new List<ResponseDetail>() });
            mcpdResponseFile.responseHierarchy[0].children.Add(new ResponseChildren { levelIdentifier = "Grievances", sectionIdentifier = null, responses = new List<ResponseDetail>() });
            mcpdResponseFile.responseHierarchy[0].children.Add(new ResponseChildren { levelIdentifier = "Appeals", sectionIdentifier = null, responses = new List<ResponseDetail>() });
            mcpdResponseFile.responseHierarchy[0].children.Add(new ResponseChildren { levelIdentifier = "ContinuityOfCare", sectionIdentifier = null, responses = new List<ResponseDetail>() });
            mcpdResponseFile.responseHierarchy[0].children.Add(new ResponseChildren { levelIdentifier = "OutOfNetwork", sectionIdentifier = null, responses = new List<ResponseDetail>() });

            foreach (var detail in _contextResponse.McpdipDetails.Where(x => x.ResponseTarget == "McpdFile" && x.ChildrenId == hierarchy.HierarchyId))
            {
                mcpdResponseFile.responseHierarchy[0].responses.Add(new ResponseDetail
                {
                    description = detail.Description,
                    id = detail.Id,
                    itemReference = detail.ItemReference,
                    severity = detail.Severity
                });
            }
            foreach (var detail in _contextResponse.McpdipDetails.Where(x => x.ResponseTarget == "McpdHeader" && x.ChildrenId == childrenHeader.ChildrenId))
            {
                mcpdResponseFile.responseHierarchy[0].children[0].responses.Add(new ResponseDetail
                {
                    description = detail.Description,
                    id = detail.Id,
                    itemReference = detail.ItemReference,
                    severity = detail.Severity
                });
            }
            foreach (var detail in _contextResponse.McpdipDetails.Where(x => x.ResponseTarget == "Grievances" && x.ChildrenId == childrenGrievance.ChildrenId && x.OriginalDataSource == IpaName))
            {
                mcpdResponseFile.responseHierarchy[0].children[1].responses.Add(new ResponseDetail
                {
                    description = detail.Description,
                    id = detail.Id,
                    itemReference = detail.ItemReference,
                    severity = detail.Severity
                });
            }
            foreach (var detail in _contextResponse.McpdipDetails.Where(x => x.ResponseTarget == "Appeals" && x.ChildrenId == childrenAppeal.ChildrenId && x.OriginalDataSource == IpaName))
            {
                mcpdResponseFile.responseHierarchy[0].children[2].responses.Add(new ResponseDetail
                {
                    description = detail.Description,
                    id = detail.Id,
                    itemReference = detail.ItemReference,
                    severity = detail.Severity
                });
            }
            foreach (var detail in _contextResponse.McpdipDetails.Where(x => x.ResponseTarget == "ContinuityOfCare" && x.ChildrenId == childrenCoc.ChildrenId && x.OriginalDataSource == IpaName))
            {
                mcpdResponseFile.responseHierarchy[0].children[3].responses.Add(new ResponseDetail
                {
                    description = detail.Description,
                    id = detail.Id,
                    itemReference = detail.ItemReference,
                    severity = detail.Severity
                });
            }
            foreach (var detail in _contextResponse.McpdipDetails.Where(x => x.ResponseTarget == "OutOfNetwork" && x.ChildrenId == childrenOon.ChildrenId && x.OriginalDataSource == IpaName))
            {
                mcpdResponseFile.responseHierarchy[0].children[4].responses.Add(new ResponseDetail
                {
                    description = detail.Description,
                    id = detail.Id,
                    itemReference = detail.ItemReference,
                    severity = detail.Severity
                });
            }

            string destinationFileName = Path.Combine(OutboundFolder, IpaName, rHeader.FileName.Replace(".json", "_RESP_Kaiser.json"));

            System.IO.File.WriteAllText(destinationFileName, JsonOperations.GetResponseJson(ref mcpdResponseFile));
        }

    }
}
