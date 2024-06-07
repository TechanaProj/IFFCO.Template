using System;
using System.Collections.Generic;

namespace IFFCO.NERRS.Web.Models
{
    public partial class AdmEmporaMap
    {
        public long Empid { get; set; }
        public string Orausername { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Empname { get; set; }
        public string Emppwd { get; set; }
        public string RepUnit { get; set; }
        public string EmailId { get; set; }
        public long? MobileNo { get; set; }
    }
}
