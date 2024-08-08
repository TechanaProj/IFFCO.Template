using IFFCO.HRMS.Repository.Pattern;
using System;
using System.Collections.Generic;

namespace IFFCO.NERRS.Web.Models
{
    public partial class MVendorMsts : Entity
    {
        public string UnitCode { get; set; }
        public decimal? VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public decimal VendorSiteId { get; set; }
        public string VendorSiteCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string EmployeeNo { get; set; }
        public string FullName { get; set; }
        public DateTime? DatetimeCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DatetimeModified { get; set; }
        public string ModifiedBy { get; set; }
        public int? HrmsUnitCd { get; set; }
    }
}
