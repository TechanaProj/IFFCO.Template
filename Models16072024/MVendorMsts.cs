using IFFCO.HRMS.Repository.Pattern;
using System;
using System.Collections.Generic;

namespace IFFCO.NERRS.Web.Models
{
    public partial class MVendorMsts : Entity
    {
        public int UnitCode { get; set; }
        public string VendorCode { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public DateTime? DatetimeCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DatetimeModified { get; set; }
        public string ModifiedBy { get; set; }
    }
}
