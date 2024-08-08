using IFFCO.HRMS.Repository.Pattern;
using System;
using System.Collections.Generic;

namespace IFFCO.NERRS.Web.Models
{
    public partial class AdmPrgMaster : Entity
    {
        public string Projectid { get; set; }
        public string Moduleid { get; set; }
        public string Programtype { get; set; }
        public string Programid { get; set; }
        public string Programname { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Ismainform { get; set; }
        public string SubMenuName { get; set; }
        public decimal? DisplayOrder { get; set; }
        public string ActiveInactive { get; set; }
    }
}
