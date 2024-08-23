using IFFCO.NERRS.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;
using IFFCO.HRMS.Shared.Entities;

namespace IFFCO.NERRS.Web.ViewModels
{
    public class NERSC03ViewModel : BaseModel
    {

        public string PlantCD { get; set; }
        public string RentCode { get; set; }
        public string SlNo { get; set; }

        public string Rates { get; set; }
        public string OccupantCode { get; set; }
        public string OccupantType { get; set; }
        public string QuarterCode { get; set; }
        public string Occupant { get; set; }

        public DateTime RentFromDate { get; set; }
        public DateTime RentToDate { get; set; }
        public DateTime AllotmentDate { get; set; }
        public DateTime VacancyDate { get; set; }
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
        //Tables

        public FAllotmentRentDtls ObjFAllotmentRentDtls { get; set; }
          
            public VwAonlaNonEmpAllotStatus ObjVwAonlaNonEmpAllotStatus { get; set; }
            public string MonthDayType { get; set; }


            //List of Tables
            public List<FAllotmentRentDtls> listFAllotmentRentDtls { get; set; }
           
            public List<VwAonlaNonEmpAllotStatus> listVwAonlaNonEmpAllotStatus { get; set; }
            public List<VwAonlaNonEmpAllotStatus> listVwAonlaNonEmpAllotStatusShutdown { get; set; }



            //SelectLists
            public List<SelectListItem> UnitLOVBind { get; set; }
            public List<SelectListItem> AllotementNoLOVBind { get; set; }
            public List<SelectListItem> ShutDownOccupantLOVBind { get; set; }
            public List<SelectListItem> ShutDownRentTypeLOVBind { get; set; }
            public List<SelectListItem> QuarterLOVBind { get; set; }
            public List<SelectListItem> VendorLOVBind { get; set; }


           
        

    }
}
