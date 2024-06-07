using System;
using System.Collections.Generic;

namespace IFFCO.NERRS.Web.Models
{
    public partial class AdmProjmodMaster
    {
        public string Projectid { get; set; }
        public string Moduleid { get; set; }
        public string Modulename { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ReadOnly { get; set; }
        public string ErpPrefix { get; set; }
        public string GstCompliant { get; set; }
        public string Migrated { get; set; }
    }
}
