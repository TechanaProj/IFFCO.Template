using System.ComponentModel.DataAnnotations;
using System;
using IFFCO.HRMS.Repository.Pattern.Core;
using System.ComponentModel.DataAnnotations.Schema;
using NPOI.SS.Formula.Functions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace IFFCO.NERRS.Web.Models
{
    public class VwAonlaNonEmpAllotStatus : Entity

    {
        public List<SelectListItem> RentTypeLOVBind { get; set; }
        [Display(Name = "Unit Code")]
        public string UnitCode { get; set; }

        [Display(Name = "Allotment Number")]
        public int AllotmentNo { get; set; }

        [Display(Name = "Quarter For")]
        public string QuarterFor { get; set; }

        [Display(Name = "Allotment Type")]
        public string AllotmentType { get; set; }

        [Display(Name = "Quarter Issued To")]
        public string QuarterIssuedTo { get; set; }

        [Display(Name = "Issued To")]
        public string IssuedTo { get; set; }

        [Display(Name = "Quarter Name For")]
        public string QuarterNameFor { get; set; }

        [Display(Name = "Application Date")]
        [DataType(DataType.Date)]
        public DateTime ApplicationDate { get; set; }

        [Display(Name = "Approved Date")]
        [DataType(DataType.Date)]
        public DateTime? ApprovedDate { get; set; }

        [Display(Name = "Quarter Category")]
        public string QuarterCategory { get; set; }

        [Display(Name = "Quarter Number")]
        public string QuarterNo { get; set; }

        [Display(Name = "Occupancy Date")]
        [DataType(DataType.Date)]
        public DateTime? OccupancyDate { get; set; }

        //[Display(Name = "Vacancy Date")]
        
        [DataType(DataType.Date)]
        public DateTime? VacancyDate { get; set; }
        [NotMapped] public string VacancyDate_Text { get; set; }



        [Display(Name = "Stay Period")]
        public string StayPeriod { get; set; }

        [Display(Name = "Number of Days")]
        public int NoOfDays { get; set; }

        [Display(Name = "Number of Years")]
        public int NoOfYears { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }
        [NotMapped]
        public string OccupantType { get; set; }
        [NotMapped]
        public string RentType { get; set; }

        [NotMapped]
        [Display(Name = "Personal Number")]
        public string PersonalNo { get; set; }

        [NotMapped]

        public int SlNo { get; set; }
        [NotMapped]

        public string VendorCode { get; set; }
        [NotMapped]
        public DateTime? RentFromDate { get; set; }
        [NotMapped]
        public DateTime? RentToDate { get; set; } 
       

    }
}
