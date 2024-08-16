using IFFCO.HRMS.Shared.Entities;
using IFFCO.NERRS.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace IFFCO.NERRS.Web.ViewModels
{
    public class NERSC05ViewModel : BaseModel
    {
        public string PlantCD { get; set; }
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
        public FIntCompute MstsFIntCompute { get; set; }

        public List<FIntCompute> listFIntCompute { get; set; }
        public List<SelectListItem> UnitLOVBind { get; set; }
        public List<SelectListItem> OccupantLOVBind { get; set; }
    }
}
