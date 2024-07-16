//using IFFCO.NERRS.Web.Models;
using IFFCO.NERRS.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFFCO.NERRS.Web.ViewModels
{
    public class NERSC02ViewModel
    {

        public string PlantCD { get; set; }
        public int UnitCode { get; set; }
        public int AllotmentNo { get; set; }
        public string RentCode { get; set; }
        public string OccupantCode { get; set; }
        public string OccupantType { get; set; }
        public string Occupant { get; set; }

        public DateTime AllotmentDate { get; set; }
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
        //Tables

        public FAllotmentRentDtls ObjFAllotmentRentDtls { get; set; }
        public MOccupantMsts ObjMOccupantMsts { get; set; }
        public VwAonlaConsultantAllotStatus ObjVwAonlaConsultantAllotStatus { get; set; }
        public VwAonlaExEmpAllotStatus ObjVwAonlaExEmpAllotStatus { get; set; }
        public VwAonlaNonEmpAllotStatus ObjVwAonlaNonEmpAllotStatus { get; set; }
        public string MonthDayType { get; set; }


        //List of Tables
        public List<FAllotmentRentDtls> listFAllotmentRentDtls { get; set; }
        public List<MOccupantMsts> listMOccupantMsts { get; set; }
        public List<VwAonlaConsultantAllotStatus> listVwAonlaConsultantAllotStatus { get; set; }
        public List<VwAonlaExEmpAllotStatus> listVwAonlaExEmpAllotStatus { get; set; }
        public List<VwAonlaNonEmpAllotStatus> listVwAonlaNonEmpAllotStatus { get; set; }



        //SelectLists
        public List<SelectListItem> UnitLOVBind { get; set; }
        public List<SelectListItem> AllotementNoLOVBind { get; set; }
        public List<SelectListItem> OccupantLOVBind { get; set; }
        public List<SelectListItem> RentTypeLOVBind { get; set; }
    }
}
