﻿using IFFCO.HRMS.Shared.Entities;
using System;

namespace IFFCO.NERRS.Web.ViewModels
{
    public class NERM01ViewModel : BaseModel
    {
        public int UnitCode { get; set; }
        public string RentCode { get; set; }
        public string TypeResiAccom { get; set; }
        public double Rates { get; set; }
        public string MonthDayType { get; set; }
        public string Status { get; set; }
        public DateTime DatetimeCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DatetimeModified { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string QuarterIssuedTo { get; set; }
    }
}
