using IFFCO.HRMS.Repository.Pattern;
using System;
using System.Collections.Generic;

namespace IFFCO.NERRS.Web.Models
{
    public partial class AdmEmpUnitAccess : Entity
    {
        public int Empid { get; set; }
        public string Moduleid { get; set; }
        public int UnitCode { get; set; }
        public string DefaultUnit { get; set; }
        public string HierYn { get; set; }
        public string AllDeptAccess { get; set; }
        public string AllSectionAccess { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DatetimeCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DatetimeModified { get; set; }
        public string OnlyAreaAccess { get; set; }
        public string Projectid { get; set; }
    }
}
