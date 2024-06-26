using IFFCO.HRMS.Entities.Models;
using IFFCO.HRMS.Service;
using IFFCO.NERRS.Web.CommonFunctions;
using IFFCO.NERRS.Web.Controllers;
using IFFCO.NERRS.Web.Models;
//using IFFCO.NERRS.Web.Models;
using IFFCO.NERRS.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFFCO.NERRS.Web.Areas.M1.Controllers
{
    [Area("M1")]
    public class NERSC02Controller : BaseController<NERSC02ViewModel>
    {
        private readonly HRMS.Entities.Models.ModelContext _context;
        private readonly DropDownListBindWeb dropDownListBindWeb = null;
        private readonly NERRSCommonService nERRSCommonService = null;
        private readonly PrimaryKeyGen primaryKeyGen = null;
        CommonException<NERSC02ViewModel> commonException = null;
        public ReportRepositoryWithParameters reportRepository = null;

        public NERSC02Controller(HRMS.Entities.Models.ModelContext context)
        {
            _context = context;
            nERRSCommonService = new NERRSCommonService();
            commonException = new CommonException<NERSC02ViewModel>();
            reportRepository = new ReportRepositoryWithParameters();
            dropDownListBindWeb = new DropDownListBindWeb();
            primaryKeyGen = new PrimaryKeyGen();
        }
        public IActionResult Index()
        {
            int EMP_ID = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
            string moduleid = Convert.ToString(HttpContext.Session.GetString("ModuleID"));

            var ApplicantLov = dropDownListBindWeb.AllotementNoLOVBind();
            CommonViewModel.AllotementNoLOVBind = ApplicantLov ?? new List<SelectListItem>(); // Ensure it's not null

            if (!string.IsNullOrEmpty(Convert.ToString(TempData["Message"])))
            {
                CommonViewModel.Message = Convert.ToString(TempData["Message"]);
                CommonViewModel.Alert = Convert.ToString(TempData["Alert"]);
                CommonViewModel.Status = Convert.ToString(TempData["Status"]);
            }

            CommonViewModel.listFAllotmentRentDtls = new List<FAllotmentRentDtls>();
            CommonViewModel.UnitLOVBind = dropDownListBindWeb.GetUnitWithSecurity(Convert.ToString(EMP_ID), moduleid) ?? new List<SelectListItem>();
            CommonViewModel.FromDate = new DateTime(DateTime.Today.AddYears(-3).Year, DateTime.Today.AddYears(-3).Month, 1);
            CommonViewModel.ToDate = DateTime.Today;

            return View(CommonViewModel);
        }

        [HttpPost]
        public IActionResult Query(DateTime FromDate, DateTime ToDate, int? AllotmentNo = null)
        {
            try
            {
                if (ToDate < FromDate)
                {
                    return Json(new { success = false, error = "End date cannot be before start date." });
                }

                var allotmentRentDetails = dropDownListBindWeb.GetFilteredAllotmentRentDetails(FromDate, ToDate, AllotmentNo);
                return Json(new { success = true, data = allotmentRentDetails });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }







    }







}
