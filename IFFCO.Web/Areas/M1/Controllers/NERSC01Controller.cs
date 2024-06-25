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
        public IActionResult Index()
        {
            //int PersonnelNumber = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
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
            CommonViewModel.listFAllotmentRentDtls = new List<FAllotmentRentDtls>();
            CommonViewModel.listVwAonlaConsultantAllotStatus = new List<VwAonlaConsultantAllotStatus>();
            // CommonViewModel.FromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            // CommonViewModel.ToDate = DateTime.Today;
            //CommonViewModel = GetRentList(CommonViewModel, CommonViewModel.FromDate, CommonViewModel.ToDate, null, "Y");
            CommonViewModel = GetRentList(CommonViewModel,CommonViewModel.PlantCD, CommonViewModel.OccupantCode);
            CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
            CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View(CommonViewModel);
        }



        public IActionResult Query(NERSC01ViewModel nERSC01ViewModel)
        {
            try
            {
                // CommonViewModel = GetRailDiList(nERSC01ViewModel, Convert.ToDateTime(nERSC01ViewModel.FromDate), Convert.ToDateTime(nERSC01ViewModel.ToDate), nERSC01ViewModel.PlantCD, nERSC01ViewModel.Pending);
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



        public JsonResult PopulateOccupanttDtl(NERSC01ViewModel nERSC01ViewModel)
        {
            try
            {
                nERSC01ViewModel.Insert = CommonViewModel.Insert;
                nERSC01ViewModel.Edit = CommonViewModel.Edit;
                nERSC01ViewModel.Select = CommonViewModel.Select;
                nERSC01ViewModel.Delete = CommonViewModel.Delete;
                CommonViewModel = nERSC01ViewModel;

                var ConsultantDtlsObj = new List<VwAonlaConsultantAllotStatus>();

                VwAonlaConsultantAllotStatus vwAonlaConsultantAllotStatusDtls = nERRSCommonService.VwAonlaConsultantDtls().FirstOrDefault();

                //var dOcpDtlObj = new List<FAllotmentRentDtls>();
               // FAllotmentRentDtls fAllotmentRentDtls = _context.FAllotmentRentDtls.FirstOrDefault(x => x.RentCode.Equals(nERSC01ViewModel.RentCode));

                //var rentcode = vwAonlaConsultantAllotStatusDtls.RentCode;
                //var allotdate = vwAonlaConsultantAllotStatusDtls.AllotmentDate;
               // nERSC01ViewModel.RentCode = rentcode;
              //  CommonViewModel = Convert.ToDateTime(allotdate);
                //ConsultantDtlsObj = nERRSCommonService.VwAonlaConsultantDtls().FirstOrDefault().ToList();
                //ConsultantDtlsObj = _context.FAllotmentRentDtls.Where(x => x.RentCode == nERSC01ViewModel.RentCode && x.UnitCode.Equals(nERSC01ViewModel.UnitCode)).ToList();
               // CommonViewModel.indentHdr = _context.FAllotmentRentDtls.FirstOrDefault(x => x.RentCode == nERSC01ViewModel.RentCode && x.UnitCode.Equals(nERSC01ViewModel.UnitCode));
                CommonViewModel.AlottedOccupantList = ConsultantDtlsObj;


                //nERSC01ViewModel.RailIndentDistLov = dropDownListBindWeb.GetDiTptrLOV(nERSC01ViewModel.PlantCD, nERSC01ViewModel.DINo);
                //var obj = nERSC01ViewModel.RailIndentDistLov;

                //CommonViewModel.WagonTypeLov = _context.MWagon.Where(x => x.Status == "A").Select(x => new SelectListItem
                //{
                //    Text = string.Concat(x.WagonType, " - ", x.WagonDesc, " - CC-", x.WagonCc, " - ChargeWt ~ ", x.ChargeWt),
                //    Value = x.WagonType.ToString()
                //}).ToList();


                // CommonViewModel.SelectedDestSno = obj.FirstOrDefault().Value ?? "001";

                // CommonViewModel.AlottedOccupantList = RowDiDtlPopulate(CommonViewModel);
                // CommonViewModel.AlottedOccupantList = dOcpDtlObj;
                CommonViewModel.ActionMode = "Update";
                CommonViewModel.IsAlertBox = false;
                CommonViewModel.SelectedAction = "GetListSearch";
                CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
                CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();
                TempData["CommonViewModel"] = JsonConvert.SerializeObject(CommonViewModel);
            }
            catch (Exception ex)
            {
                commonException.GetCommonExcepton(CommonViewModel, ex);
                CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
                CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();
                return Json(CommonViewModel);

            }
            return Json(CommonViewModel);
        }
        public async Task<IActionResult> GetListSearch()
        {
            //int PersonnelNumber = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
            //ViewBag.PlantCDLOVList = dropDownListBindWeb.GetPlantCDLOV(PersonnelNumber);
            NERSC01ViewModel CommonViewModel = new NERSC01ViewModel();
            CommonViewModel = JsonConvert.DeserializeObject<NERSC01ViewModel>(TempData["CommonViewModel"].ToString());
            return View("Index", CommonViewModel);
        }

        [HttpPost]
        //public NERSC01ViewModel GetRailDiList(NERSC01ViewModel nERSC01ViewModel, DateTime? FromDate, DateTime? ToDate, string PlantCD, string Pending = "All")
        public NERSC01ViewModel GetRentList(NERSC01ViewModel nERSC01ViewModel, string PlantCD, string OccupantCode)
        {
            var str = string.Empty;
            CommonViewModel = nERSC01ViewModel;
            int PersonnelNumber = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
            // if (PlantCD == null) { PlantCD = dropDownListBindWeb.GetPlantCDSingle(PersonnelNumber); }
            // if (FromDate == null) { var first = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1); FromDate = first; }
            // if (ToDate == null) { ToDate = DateTime.Today; }
            CommonViewModel.listVwAonlaConsultantAllotStatus = new List<VwAonlaConsultantAllotStatus>();
            CommonViewModel.listVwAonlaNonEmpAllotStatus = new List<VwAonlaNonEmpAllotStatus>();
            CommonViewModel.listVwAonlaExEmpAllotStatus = new List<VwAonlaExEmpAllotStatus>();
            //CommonViewModel.listFAllotmentRentDtls = new List<FAllotmentRentDtls>();
            //if (Pending == "All")
            //{
            //    CommonViewModel.ObjList = _context.VNetRailDi.Where(x => x.PlantCd == PlantCD).Where(x => x.DiDt >= FromDate && x.DiDt <= ToDate).ToList();
            //}
            //else
            //{
            //    CommonViewModel.ObjList = _context.VNetRailDi.Where(x => x.PlantCd == PlantCD && x.Pending == Pending).Where(x => x.DiDt >= FromDate && x.DiDt <= ToDate).ToList();
            //}

            CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
            CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();
            return CommonViewModel;
        }
    }
}
