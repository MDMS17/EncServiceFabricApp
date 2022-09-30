using EncDataModel.Submission837;
using EncDataModel.X12;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X12Lib
{
    public partial class X12Exporter
    {
        public static string Export999(ref SubmissionLog submittedFile, int transactionCount, int recordCount) 
        {
            StringBuilder sb = new StringBuilder();
            ISA_999 isa = new ISA_999 
            {
                 InterchangeControlNumber=submittedFile.InterchangeControlNumber,
                 InterchangeDate=submittedFile.InterchangeDate,
                 InterchangeReceiverID=submittedFile.ReceiverID,
                 InterchangeSenderID=submittedFile.SubmitterID,
                 InterchangeTime=submittedFile.InterchangeTime,
                 ProductionFlag=submittedFile.ProductionFlag,
            };
            sb.Append(isa.ToX12String());
            GS gs = new GS 
            {
                 FunctionalIDCode="FA",
                 GroupControlNumber="1",
                 ReceiverID=submittedFile.ReceiverID,
                 ResponsibleAgencyCode="X",
                 SenderID=submittedFile.SubmitterID,
                 TransactionDate=submittedFile.InterchangeDate,
                 TransactrionTime=submittedFile.InterchangeTime,
                 VersionID= "005010X231A1"
            };
            sb.Append(gs.ToX12String());
            ST_999 st = new ST_999
            {
                TransactionControlNumber = "0001",
                VersionNumber= "005010X231A1"
            };
            sb.Append(st.ToX12String());
            AK1_999 ak1 = new AK1_999 
            { 
                FunctionalCode="HC",
                GroupControlNumber="0001",
                VersionId= "005010X222A1"
            };
            sb.Append(ak1.ToX12String());
            for (int i = 0; i < transactionCount; i++)
            {
                AK2_999 ak2 = new AK2_999
                {
                    TransactionCode = "837",
                    TransactionControlNumber = "0001",
                    VersionId = "005010X222A1"
                };
                sb.Append(ak2.ToX12String());
                IK5_999 ik5 = new IK5_999
                {
                    TransactionAckCode="A"
                };
                sb.Append(ik5.ToX12String());
            }
            AK9_999 ak9 = new AK9_999
            {
                FunctionAckCode = "A",
                NumberofTransactions = recordCount.ToString(),
                NumberOfReceivedTransactions=recordCount.ToString(),
                NumberOfAccpetedTransactions=recordCount.ToString()
            };
            sb.Append(ak9.ToX12String());
            SE se = new SE 
            {
                SegmentCount = (sb.ToString().Count(x => x == '~') - 1).ToString(),
                TransactionControlNumber ="0001"
            };
            sb.Append(se.ToX12String());
            GE ge = new GE 
            { 
                NumberofTransactionSets="1",
                GroupControlNumber="1"
            };
            sb.Append(ge.ToX12String());
            IEA iea = new IEA 
            { 
                NumberofFunctionalGroups="1",
                InterchangeControlNumber=submittedFile.InterchangeControlNumber 
            };
            sb.Append(iea.ToX12String());
            return sb.ToString();
        }
        public static void Export277CA() 
        { 

        }
        private const int Every10Lines = 10;
        private const string DetailColumnSchema = @"{0, -15}{1,-21}{2,-5}{3,-15}{4,-40}{5, -16}{6,-197}";
        public static readonly string[] ExemptInProgress = { "63" };
        public static readonly string[] ExemptDuplicated = { "52", "53" };
        public static readonly string[] ExemptMemberNotEligible = { "90" };

        public static string EVRReport(CalculatedCounts counts)
        {
            var sb = new StringBuilder();
            sb.AppendLine(EVRHeader(counts));

            sb.AppendLine(EVRSummary(counts));

            sb.AppendLine(EVRDetails(counts.ValidationErrors));

            return sb.ToString();
        }

        public static string EVRHeader(CalculatedCounts counts)
        {
            return
                $@"
Processed Date: {counts.Now}  File Name: {counts.TransmissionmName}
";
        }

        public static string EVRDetails(IEnumerable<ClaimValidationError> claimsInError)
        {
            var sb = new StringBuilder();

            sb.AppendLine(ForColumnLabels());
            sb.AppendLine(ForRowPartition());

            // Build ValidationError and Claims rows
            var rowIndex = 0;
            var cachedClaim = "";
            foreach (var error in claimsInError)
            {
                var isCachedClaim = error.ClaimId.Equals(cachedClaim);

                if (IsTenLines(rowIndex, isCachedClaim))
                    sb.AppendLine(ForRowPartition());

                sb.AppendLine(ForValidation(error));

                if (isCachedClaim)
                    continue;

                ++rowIndex;

                cachedClaim = error.ClaimId;
            }

            return sb.ToString();
        }

        public static bool IsTenLines(int rowIndex, bool isCachedClaim)
        {
            return (rowIndex % Every10Lines == 0) && (rowIndex > 0) && !isCachedClaim;
        }

        private static string ForColumnLabels()
        {
            var sb = new StringBuilder();
            sb.Append(string.Format(DetailColumnSchema
                    , "Record"
                    , "Claim ID"
                    , "No"
                    , "Loop"
                    , "Element Name"
                    , "Error Severity"
                    , "Message"
                )
            );

            return sb.ToString();
        }

        public static string ForRowPartition()
        {
            var sb = new StringBuilder();
            var rowPartitions = string.Format(DetailColumnSchema
                , "--------------"
                , "----------------"
                , "----"
                , "--------------"
                , "---------------------------------------"
                , "---------------"
                ,
                "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------"
            );

            sb.Append(rowPartitions);

            return sb.ToString();
        }

        private static string ForValidation(ClaimValidationError error)
        {
            error.ErrorSeverityName = "Invalid";

            if (ExemptDuplicated.Contains(error.ErrorId))
                error.ErrorSeverityName = "Duplicate";

            if (ExemptMemberNotEligible.Contains(error.ErrorId))
                error.ErrorSeverityName = "NotElig";

            var sb = new StringBuilder();
            sb.Append(string.Format(DetailColumnSchema,
                error.Record,
                error.ClaimId,
                error.ErrorSequencePerEncounter.ToString().PadLeft(3, ' '),
                error.LoopNumber.PadRight(15).Substring(0, 14).Replace("_Loop", ""),
                error.ElementName.PadRight(40).Substring(0, 39),
                error.ErrorSeverityName,
                error.ErrorDescription
            ));

            return sb.ToString();
        }
        public static string EVRSummary(CalculatedCounts counts)
        {
            return $@"
 Stage 1 - File Level (999 Acknowledgement Transaction Sets)
+=================================================================================================
|                  Record Count |    {counts.RecordCount,5} |                           
+-------------------------------+----------+------------------------------------------------------
|                      Rejected |    {counts.RejectedCount,5} |   Individual Encounters in Non-compliant ST/SE Sets 
+-------------------------------+----------+------------------------------------------------------      
|                      Accepted |    {counts.ProcessedCount,5} |   Individual Encounters in Compliant ST/SE Sets      
+-------------------------------+----------+------------------------------------------------------      

 Stage 2 - Encounter Level
+=================================================================================================
|                     Duplicate |    {counts.ExemptDuplicateCount,5} |  
+-------------------------------+----------+------------------------------------------------------    
|           Member Not Eligible |    {counts.ExemptMemberNotEligibleCount,5} |              
+-------------------------------+----------+------------------------------------------------------
|  Accepted For IEHP Validation |    {counts.EligibileForIehpEditChecks,5} |   
+-------------------------------+----------+------------------------------------------------------
|                   In Progress |    {counts.ExemptInProgressCount,5} |     
+-------------------------------+----------+------------------------------------------------------      
|       Total Records Processed |    {counts.ProcessedCount,5} |  
+-------------------------------+----------+------------------------------------------------------       

 Stage 3 - Validity 
+=================================================================================================
|                       Invalid |    {counts.InvalidCount,5} |   Encounter(s) Failed IEHP Validation        
+-------------------------------+----------+------------------------------------------------------
|                         Valid |    {counts.ValidCount,5} |   Encounter(s) Passed IEHP Validation            
+-------------------------------+----------+------------------------------------------------------
|       Total Records Validated |    {counts.EligibileForIehpEditChecks,5} |   
+-------------------------------+----------+------------------------------------------------------      

+=================================================================================================
|                      Validity |   {counts.Validity,4:0.0} % |   Valid / Accepted For IEHP Validation   
+=================================================================================================
";
        }


    }
}
