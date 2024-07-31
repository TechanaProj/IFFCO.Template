using IFFCO.HRMS.Service;
using IFFCO.NERRS.Web.CommonFunctions;
using IFFCO.NERRS.Web.Models;
using IFFCO.NERRS.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using IFFCO.NERRS.Web.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IFFCO.NERRS.Web.Areas.M1.Controllers
{
    [Area("M1")]
    public class NERM03Controller : BaseController<NERM03ViewModel>
    {
        private readonly ModelContext _context;
        private readonly DropDownListBindWeb dropDownListBindWeb;
        private readonly NERRSCommonService nERRSCommonService;
        private readonly PrimaryKeyGen primaryKeyGen;
        private readonly CommonException<NERM03ViewModel> commonException;
        public ReportRepositoryWithParameters reportRepository;


        public NERM03Controller(ModelContext context)
        {

            _context = context;
            nERRSCommonService = new NERRSCommonService();
            commonException = new CommonException<NERM03ViewModel>();
            reportRepository = new ReportRepositoryWithParameters();
            dropDownListBindWeb = new DropDownListBindWeb();
            primaryKeyGen = new PrimaryKeyGen();
        }
        // GET: M1/NERM02
        public async Task<IActionResult> Index()
        {
            int PersonnelNumber = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
            int Unit = Convert.ToInt32(HttpContext.Session.GetInt32("UnitCode"));
            CommonViewModel.List = new List<MRentMsts>();
            CommonViewModel.List = _context.MRentMsts.ToList();
            return View(CommonViewModel);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mOccupant = await _context.MRentMsts
                .FirstOrDefaultAsync(m =>m.RentCode  == id);
            if (mOccupant == null)
            {
                return NotFound();
            }

            return View(mOccupant);
        }


        public IActionResult Create()
        {
            int PersonnelNumber = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
            int Unit = Convert.ToInt32(HttpContext.Session.GetInt32("UnitCode"));
            CommonViewModel.List = new List<MRentMsts>();
            CommonViewModel.List = _context.MRentMsts.ToList();
            // CommonViewModel.PlantCDLOVList = dropDownListBindWeb.GetPlantCDLOV(PersonnelNumber);
            // ViewBag.StateCDLOVList = dropDownListBindWeb.GetStateLOV();
            CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();  // Populating Area name for forming the page URL
            CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString(); // Populating Menu name for forming the page URL
            CommonViewModel.Status = "Create";
            return View("Index", CommonViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NERM03ViewModel nERM03ViewModel)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(nERM03ViewModel.Msts.RentCode))
                {
                    MRentMsts Obj = new MRentMsts
                    {
                         //DataTable dtSno = _context.GetSQLQuery("select nvl(max(RentCode),0)+1 RentCode from GH_ITEM_MST");

                       RentCode = nERM03ViewModel.Msts.RentCode = Convert.ToString(Convert.ToInt32(_context.MRentMsts.AsEnumerable().OrderByDescending(x => x.RentCode).First().RentCode) + 1),
                        CreatedBy = Convert.ToString(HttpContext.Session.GetInt32("EmpID")),
                        //UnitCode  = Convert.ToInt32(HttpContext.Session.GetInt32("UnitCode")),
                        UnitCode = 3,
                        DatetimeCreated = DateTime.Now,
                        TypeResiAccom = nERM03ViewModel.Msts.TypeResiAccom,
                        Rates = nERM03ViewModel.Msts.Rates,
                        MonthDayType = nERM03ViewModel.Msts.MonthDayType,
                        Status = nERM03ViewModel.Msts.Status
                       
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
            CommonViewModel.Message = "Rent No " + Convert.ToString(nERM03ViewModel.Msts.RentCode);
            CommonViewModel.Alert = "Create";
            CommonViewModel.Status = "Create";

            CommonViewModel.ErrorMessage = "";
            CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
            CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();
            return Json(CommonViewModel);
        }
    }
}
