using System;
using System.Collections.Generic;

namespace IFFCO.NERRS.Web.Models
{
    public partial class FAllotmentRentDtls
    {
        public int UnitCode { get; set; }
        public string QuarterCategory { get; set; }
        public int QuarterNo { get; set; }
        public int SlNo { get; set; }
        public int PersonalNo { get; set; }
        public int AllotmentNo { get; set; }
        public DateTime AllotmentDate { get; set; }
        public DateTime? OccupancyDate { get; set; }
        public DateTime? ExpectedVacancyDate { get; set; }
        public DateTime? VacancyDate { get; set; }
        public int? ExtensionApprovedBy { get; set; }
        public DateTime? ExtensionApprovedDate { get; set; }
        public DateTime? ExtensionFromDate { get; set; }
        public DateTime? ExtensionToDate { get; set; }
        public string ExtensionCategory { get; set; }
        public DateTime? NormarlHrrFromDate { get; set; }
        public DateTime? NormalHrrToDate { get; set; }
        public double? NormalHrrRate { get; set; }
        public DateTime? MarketHrrFromDate { get; set; }
        public DateTime? MarketHrrToDate { get; set; }
        public double? MarketHrrRate { get; set; }
        public DateTime? AcInstallationDate { get; set; }
        public double? NoOfAcs { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DatetimeCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DatetimeModified { get; set; }
        public int? ColonyCode { get; set; }
        public bool? FloorNo { get; set; }
        public DateTime? PenalHrrFromDate { get; set; }
        public DateTime? PenalHrrToDate { get; set; }
        public double? PenalHrrRate { get; set; }
        public DateTime? AllotmentCancelDate { get; set; }
        public string RepUnit { get; set; }
        public int? PayrollYearmonth { get; set; }
        public string PayrollProcessStatus { get; set; }
        public string RentCode { get; set; }
        public string VendorCode { get; set; }
        public string OccupantCode { get; set; }
        public decimal? ElecRate { get; set; }
        public decimal? ElectricityCount { get; set; }
        public string MonthDayType { get; set; }
    }
}
