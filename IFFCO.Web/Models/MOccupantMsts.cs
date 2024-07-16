using System;
using System.Collections.Generic;

namespace IFFCO.NERRS.Web.Models
{
    public partial class MOccupantMsts
    {
        public int UnitCode { get; set; }
        public string OccupantCode { get; set; }
        public string OccupantType { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public DateTime DatetimeCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DatetimeModified { get; set; }
        public string ModifiedBy { get; set; }
        public string QuarterFor { get; set; }
        public string QuarterIssuedTo { get; set; }
    }
}
