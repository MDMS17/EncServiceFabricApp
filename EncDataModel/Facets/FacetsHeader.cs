using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.Facets
{
    public class FacetsHeader
    {
        public string ClaimID { get; set; }
        public string PlanID { get; set; }
        public string ClaimType { get; set; }
        public bool? IsCapitated { get; set; }
        public decimal HeaderChargeAmt { get; set; }
        public decimal HeaderPaidAmt { get; set; }
        public decimal HeaderPatPaidAmt { get; set; }
        public string RelInfo { get; set; }
        public string ACDState { get; set; }
        public string BenAssigned { get; set; }
        public DateTime PaidDate { get; set; }
        public DateTime DosFromDate { get; set; }
        public DateTime DosToDate { get; set; }
        public string ClaimIdAdjTo { get; set; }
        public string ClaimIdAdjFrom { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string MemberCK { get; set; }
        public string FacType { get; set; }
        public string BillClass { get; set; }
        public string Frequency { get; set; }
        public string DisChgHr { get; set; }
        public DateTime? StatFromDte { get; set; }
        public DateTime? StatToDte { get; set; }
        public DateTime? AdmitDte { get; set; }
        public string AdmitHr { get; set; }
        public string AdmitType { get; set; }
        public string AdmitSource { get; set; }
        public string DisChgSts { get; set; }
        public string MedRecNo { get; set; }
        public Int16 CoveredDays { get; set; }
        public DateTime SimilarIllnessDate { get; set; }
        public string DRGInfo { get; set; }
        public DateTime? DisChargeDte { get; set; }
        public string InputMethod { get; set; }
        public string MemberID { get; set; }
        public string MemberSex { get; set; }
        public DateTime MemberBirthDate { get; set; }
        public string MemberSSN { get; set; }
        public string MemberLastName { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberMiddleInit { get; set; }
        public string MemberAddress1 { get; set; }
        public string MemberAddress2 { get; set; }
        public string MemberAddressCity { get; set; }
        public string MemberAddressState { get; set; }
        public string MemberAddressZip { get; set; }
        public string MedicareId { get; set; }
        public string MedicaidId { get; set; }
        public string COBPayerId { get; set; }
    }
}
