using IFFCO.HRMS.Shared.Entities;
using IFFCO.NERRS.Web.Controllers;
using IFFCO.NERRS.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace IFFCO.NERRS.Web.ViewModels
{
    public class NERM04ViewModel : BaseModel
    {

        public int UnitCode { get; set; }
        public int VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public int VendorSiteId { get; set; }
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

        public List<MVendorMsts> ListMVendorMsts { get; set; }

        public MVendorMsts ObjMVendorMsts { get; set; }

        
    }
}
