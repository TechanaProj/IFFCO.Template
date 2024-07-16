using IFFCO.HRMS.Repository.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFFCO.NERRS.Web.Models
{
    public class FIntCompute: Entity
    {
        public int UnitCode { get; set; }
        public string QuarterCategory { get; set; }
        public int QuarterNo { get; set; }
        public int? SlNo { get; set; }
        public int? PersonalNo { get; set; }
        public int AllotmentNo { get; set; }
        public DateTime AllotmentDate { get; set; }
        public DateTime? OccupancyDate { get; set; }
        public DateTime? VacancyDate { get; set; }
        public double? TotalAmt { get; set; }
        public double? NoOfAcs { get; set; }
        public string Remarks { get; set; }
        public string Flag { get; set; }
        public string Status { get; set; }
        public string RepUnit { get; set; }
        public string RentCode { get; set; }
        public string VendorCode { get; set; }
        public string OccupantCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DatetimeCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DatetimeModified { get; set; }
        public decimal? DaysRemaining { get; set; }
        public decimal ComputationRun { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? NxtFromDate { get; set; }
        public DateTime? NxtToDate { get; set; }
        public string MonthDayType { get; set; }
    }
}
