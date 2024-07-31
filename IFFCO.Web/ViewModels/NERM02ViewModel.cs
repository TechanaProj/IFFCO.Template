using IFFCO.HRMS.Repository.Pattern;
using IFFCO.HRMS.Shared.Entities;
using IFFCO.NERRS.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Composition;

namespace IFFCO.NERRS.Web.ViewModels
{
    public class NERM02ViewModel : BaseModel
    {
        public int UnitCode { get; set; }
        public MOccupantMsts Msts { get; set; }

        public List<MOccupantMsts> List { get; set; }

        public List<SelectListItem> PlantCDLOVList { get; set; }
    }
}
