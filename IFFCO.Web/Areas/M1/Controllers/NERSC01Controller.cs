using Microsoft.AspNetCore.Mvc;
using IFFCO.NERRS.Web.CommonFunctions;
using IFFCO.NERRS.Web.Controllers;
using IFFCO.NERRS.Web.Models;
using IFFCO.NERRS.Web.ViewModels;
using IFFCO.HRMS.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFFCO.NERRS.Web.Areas.M1.Controllers
{
    [Area("M1")]
    public class NERSC01Controller : BaseController<NERSC01ViewModel>
    {
        private readonly ModelContext _context;
        private readonly DropDownListBindWeb dropDownListBindWeb = null;
        private readonly NERRSCommonService nERRSCommonService = null;
        private readonly PrimaryKeyGen primaryKeyGen = null;
        CommonException<NERSC01ViewModel> commonException = null;
        public ReportRepositoryWithParameters reportRepository = null;

        public NERSC01Controller(ModelContext context)
        {
            _context = context;
            nERRSCommonService = new NERRSCommonService();
            commonException = new CommonException<NERSC01ViewModel>();
            reportRepository = new ReportRepositoryWithParameters();
            dropDownListBindWeb = new DropDownListBindWeb();
            primaryKeyGen = new PrimaryKeyGen();
        }
        public IActionResult Index(string PlantCD = null, string OccupantType = null, string OccupantCode = null)
        {

            // ViewBag.PlantCDLOVList = dropDownListBindWeb.GetPlantCDLOV(PersonnelNumber);
            int EMP_ID = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
            string moduleid = Convert.ToString(HttpContext.Session.GetString("ModuleID"));
            if (Convert.ToString(TempData["Message"]) != "")
            {
                CommonViewModel.Message = Convert.ToString(TempData["Message"]);
                CommonViewModel.Alert = Convert.ToString(TempData["Alert"]);
                CommonViewModel.Status = Convert.ToString(TempData["Status"]);
            }

            CommonViewModel.UnitLOVBind = dropDownListBindWeb.GetUnitWithSecurity(Convert.ToString(EMP_ID), moduleid);
            CommonViewModel.OccupantLOVBind = dropDownListBindWeb.OccupantLOVBind();
            CommonViewModel.RentTypeLOVBind = dropDownListBindWeb.RentTypeLOVBind();

            if (OccupantType == "E")
            {
                CommonViewModel.OccupantLOVBind = dropDownListBindWeb.OccupantEmpLOVBind();
            }
            else if (OccupantType == "N")
            {
                CommonViewModel.OccupantLOVBind = dropDownListBindWeb.OccupantNonEmpLOVBind();
            }

            CommonViewModel.listFAllotmentRentDtls = new List<FAllotmentRentDtls>();
            CommonViewModel.listVwAonlaConsultantAllotStatus = new List<VwAonlaConsultantAllotStatus>();
            CommonViewModel.listVwAonlaExEmpAllotStatus = new List<VwAonlaExEmpAllotStatus>();
            CommonViewModel.listVwAonlaNonEmpAllotStatus = new List<VwAonlaNonEmpAllotStatus>();

            CommonViewModel = GetRentList(CommonViewModel, PlantCD, OccupantCode);
            CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
            CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();

            CommonViewModel.PlantCD = PlantCD;
            CommonViewModel.OccupantCode = OccupantCode;
            CommonViewModel.OccupantType = OccupantType;
            return View(CommonViewModel);
        }

        public List<SelectListItem> OccupantLOVBindJSON(string OccupantType)
        {
            List<SelectListItem> OccupantCodeLOV = new List<SelectListItem>();
            if (OccupantType == "E")
            {
                OccupantCodeLOV = dropDownListBindWeb.OccupantEmpLOVBind();
            }
            else
            {
                OccupantCodeLOV = dropDownListBindWeb.OccupantNonEmpLOVBind();
            }
            return OccupantCodeLOV;
        }

        public IActionResult Query(NERSC01ViewModel nERSC01ViewModel)
        {
            try
            {
                CommonViewModel = GetRentList(nERSC01ViewModel, nERSC01ViewModel.PlantCD, nERSC01ViewModel.OccupantCode);
                TempData["CommonViewModel"] = JsonConvert.SerializeObject(CommonViewModel);
                CommonViewModel.IsAlertBox = false;
                CommonViewModel.SelectedAction = "GetListSearch";
                CommonViewModel.ErrorMessage = "";
                CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
                CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();

            }
            catch (Exception ex)
            {

            }
            return Json(CommonViewModel);
        }





        //public JsonResult PopulateOccupanttDtl(NERSC01ViewModel nERSC01ViewModel)
        //{
        //    try
        //    {
        //        nERSC01ViewModel.Insert = CommonViewModel.Insert;
        //        nERSC01ViewModel.Edit = CommonViewModel.Edit;
        //        nERSC01ViewModel.Select = CommonViewModel.Select;
        //        nERSC01ViewModel.Delete = CommonViewModel.Delete;
        //        CommonViewModel = nERSC01ViewModel;

        //        var ConsultantDtlsObj = new List<VwAonlaConsultantAllotStatus>();

        //        VwAonlaConsultantAllotStatus vwAonlaConsultantAllotStatusDtls = nERRSCommonService.VwAonlaConsultantDtls().FirstOrDefault();
        //        CommonViewModel.AlottedOccupantList = ConsultantDtlsObj;


        //        CommonViewModel.ActionMode = "Update";
        //        CommonViewModel.IsAlertBox = false;
        //        CommonViewModel.SelectedAction = "GetListSearch";
        //        CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
        //        CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();
        //        TempData["CommonViewModel"] = JsonConvert.SerializeObject(CommonViewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        commonException.GetCommonExcepton(CommonViewModel, ex);
        //        CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
        //        CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();
        //        return Json(CommonViewModel);

        //    }
        //    return Json(CommonViewModel);
        //}
        public async Task<IActionResult> GetListSearch()
        {
            //int PersonnelNumber = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
            //ViewBag.PlantCDLOVList = dropDownListBindWeb.GetPlantCDLOV(PersonnelNumber);
            NERSC01ViewModel CommonViewModel = new NERSC01ViewModel();
            CommonViewModel = JsonConvert.DeserializeObject<NERSC01ViewModel>(TempData["CommonViewModel"].ToString());
            return View("Index", CommonViewModel);
        }






        [HttpPost]
        public NERSC01ViewModel GetRentList(NERSC01ViewModel nERSC01ViewModel, string PlantCD, string OccupantCode)
        {
            var str = string.Empty;
            CommonViewModel = nERSC01ViewModel;
            int PersonnelNumber = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
            CommonViewModel.listVwAonlaConsultantAllotStatus = new List<VwAonlaConsultantAllotStatus>();
            CommonViewModel.listVwAonlaNonEmpAllotStatus = new List<VwAonlaNonEmpAllotStatus>();
            CommonViewModel.listVwAonlaExEmpAllotStatus = new List<VwAonlaExEmpAllotStatus>();
            if (OccupantCode == "1001")
            {
                CommonViewModel.listVwAonlaConsultantAllotStatus = nERRSCommonService.VwAonlaConsultantDtls(PlantCD);
            }
            else if (OccupantCode == "1002")
            {
                CommonViewModel.listVwAonlaExEmpAllotStatus = nERRSCommonService.VwAonlaExEmpAllotStatusDtls(PlantCD);
            }
            else
                {
                CommonViewModel.listVwAonlaNonEmpAllotStatus = nERRSCommonService.VwAonlaNonEmpAllotStatus(PlantCD, OccupantCode);
            }

            CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
            CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();
            return CommonViewModel;
        }
    }
}
