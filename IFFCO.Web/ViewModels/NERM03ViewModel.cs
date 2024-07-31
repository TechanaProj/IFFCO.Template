using IFFCO.HRMS.Shared.Entities;
using IFFCO.NERRS.Web.Controllers;
using IFFCO.NERRS.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace IFFCO.NERRS.Web.ViewModels
{
    public class NERM03ViewModel : BaseModel
    {
        public int UnitCode { get; set; }
        public MRentMsts Msts { get; set; }

        public List<MRentMsts> List { get; set; }

        public List<SelectListItem> PlantCDLOVList { get; set; }
    }
}
