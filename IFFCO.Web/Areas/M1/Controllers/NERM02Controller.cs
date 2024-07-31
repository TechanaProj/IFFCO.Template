using IFFCO.HRMS.Service;
using IFFCO.NERRS.Web.CommonFunctions;
using IFFCO.NERRS.Web.Controllers;
using IFFCO.NERRS.Web.Models;
using IFFCO.NERRS.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;


namespace IFFCO.NERRS.Web.Areas.M1.Controllers
{
    [Area("M1")]
    public class NERM02Controller : BaseController<NERM02ViewModel>
    {
        private readonly ModelContext _context;
        private readonly DropDownListBindWeb dropDownListBindWeb;
        private readonly NERRSCommonService nERRSCommonService;
        private readonly PrimaryKeyGen primaryKeyGen;
        private readonly CommonException<NERM02ViewModel> commonException;
        public ReportRepositoryWithParameters reportRepository;


        public NERM02Controller(ModelContext context)
        {

            _context = context;
            nERRSCommonService = new NERRSCommonService();
            commonException = new CommonException<NERM02ViewModel>();
            reportRepository = new ReportRepositoryWithParameters();
            dropDownListBindWeb = new DropDownListBindWeb();
            primaryKeyGen = new PrimaryKeyGen();
        }

        // GET: M1/NERM02
        public async Task<IActionResult> Index()
        {
            int PersonnelNumber = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
            int Unit = Convert.ToInt32(HttpContext.Session.GetInt32("UnitCode"));
            CommonViewModel.List = new List<MOccupantMsts>();
            CommonViewModel.List = _context.MOccupantMsts.ToList();
           // CommonViewModel.PlantCDLOVList = dropDownListBindWeb.GetPlantCDLOV(PersonnelNumber);
          //  ViewBag.StateCDLOVList = dropDownListBindWeb.GetStateLOV();
            return View(CommonViewModel);
        }


        // GET: M5/SEAM01/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mOccupant = await _context.MOccupantMsts
                .FirstOrDefaultAsync(m => m.OccupantCode == id);
            if (mOccupant == null)
            {
                return NotFound();
            }

            return View(mOccupant);
        }

        // GET: M5/SEAM01/Create
        public IActionResult Create()
        {
            int PersonnelNumber = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
            int Unit = Convert.ToInt32(HttpContext.Session.GetInt32("UnitCode"));
            CommonViewModel.List = new List<MOccupantMsts>();
            CommonViewModel.List = _context.MOccupantMsts.ToList();
           // CommonViewModel.PlantCDLOVList = dropDownListBindWeb.GetPlantCDLOV(PersonnelNumber);
           // ViewBag.StateCDLOVList = dropDownListBindWeb.GetStateLOV();
            CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();  // Populating Area name for forming the page URL
            CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString(); // Populating Menu name for forming the page URL
            CommonViewModel.Status = "Create";
            return View("Index", CommonViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NERM02ViewModel nERM02ViewModel)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(nERM02ViewModel.Msts.OccupantCode))
                {
                    MOccupantMsts Obj = new MOccupantMsts
                    {
                        OccupantCode = nERM02ViewModel.Msts.OccupantCode = Convert.ToString(Convert.ToInt32(_context.MOccupantMsts.AsEnumerable().OrderByDescending(x => x.OccupantCode).First().OccupantCode) + 1),
                        CreatedBy = Convert.ToString(HttpContext.Session.GetInt32("EmpID")),
                        //UnitCode  = Convert.ToInt32(HttpContext.Session.GetInt32("UnitCode")),
                        UnitCode  = 3,
                        DatetimeCreated = DateTime.Now,
                        OccupantType = nERM02ViewModel.Msts.OccupantType,
                        QuarterFor = nERM02ViewModel.Msts.QuarterFor,
                        Status = nERM02ViewModel.Msts.Status,
                        QuarterIssuedTo = nERM02ViewModel.Msts.QuarterIssuedTo
                    };
                    _context.Add(Obj);
                }
            }
            catch (Exception ex)
            {
                commonException.GetCommonExcepton(CommonViewModel, ex);
                CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
                CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();
                return Json(CommonViewModel);
            }


            await _context.SaveChangesAsync();
            CommonViewModel.Message = "Occupant No " + Convert.ToString(nERM02ViewModel.Msts.OccupantCode);
            CommonViewModel.Alert = "Create";
            CommonViewModel.Status = "Create";

            CommonViewModel.ErrorMessage = "";
            CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
            CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();
            return Json(CommonViewModel);
        }


        // GET: M1/NERM02/Edit/5
        public async Task<IActionResult> Edit(string id, string mod)
        {
            if (id == null)
            {
                return NotFound();
            }

            int PersonnelNumber = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
            int Unit = Convert.ToInt32(HttpContext.Session.GetInt32("UnitCode"));
            CommonViewModel.Msts = new MOccupantMsts();
            CommonViewModel.Msts = _context.MOccupantMsts.FirstOrDefault(x => x.OccupantCode == id);
            CommonViewModel.List = new List<MOccupantMsts>();
            CommonViewModel.List = _context.MOccupantMsts.ToList();
            //CommonViewModel.PlantCDLOVList = dropDownListBindWeb.GetPlantCDLOV(PersonnelNumber);
            //ViewBag.StateCDLOVList = dropDownListBindWeb.GetStateLOV();
            CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();  // Populating Area name for forming the page URL
            CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString(); // Populating Menu name for forming the page URL
            CommonViewModel.Status = "Edit";
            return View("Index", CommonViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NERM02ViewModel nERM02ViewModel)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(nERM02ViewModel.Msts.OccupantCode)   )
                {
                    nERM02ViewModel.Msts.ModifiedBy = Convert.ToString(HttpContext.Session.GetInt32("EmpID"));
                    int Unit = Convert.ToInt32(HttpContext.Session.GetInt32("UnitCode"));
                    nERM02ViewModel.Msts.DatetimeModified = DateTime.Now;
                    _context.Update(nERM02ViewModel.Msts);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                commonException.GetCommonExcepton(CommonViewModel, ex);
                CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
                CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();
                return Json(CommonViewModel);
            }


            await _context.SaveChangesAsync();
            CommonViewModel.Message = "Occupant No " + Convert.ToString(nERM02ViewModel.Msts.OccupantCode);
            CommonViewModel.Alert = "Update";
            CommonViewModel.Status = "Update";

            CommonViewModel.ErrorMessage = "";
            CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
            CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();
            return Json(CommonViewModel);


        }

    }
}
