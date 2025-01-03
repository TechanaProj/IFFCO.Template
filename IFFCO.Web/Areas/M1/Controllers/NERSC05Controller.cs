﻿using IFFCO.HRMS.Service;
using IFFCO.NERRS.Web.CommonFunctions;
using IFFCO.NERRS.Web.Controllers;
using IFFCO.NERRS.Web.Models;
using IFFCO.NERRS.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace IFFCO.NERRS.Web.Areas.M1.Controllers
{
   
        [Area("M1")]
        public class NERSC05Controller : BaseController<NERSC05ViewModel>
        {
            private readonly ModelContext _context;
            private readonly DropDownListBindWeb dropDownListBindWeb;
            private readonly NERRSCommonService nERRSCommonService;
            private readonly PrimaryKeyGen primaryKeyGen;
            private readonly CommonException<NERSC05ViewModel> commonException;
            public ReportRepositoryWithParameters reportRepository;


            public NERSC05Controller(ModelContext context)
            {

                _context = context;
                nERRSCommonService = new NERRSCommonService();
                commonException = new CommonException<NERSC05ViewModel>();
                reportRepository = new ReportRepositoryWithParameters();
                dropDownListBindWeb = new DropDownListBindWeb();
                primaryKeyGen = new PrimaryKeyGen();
            }

        public IActionResult Index(string AllotmentNo=null, string PlantCD = null,  DateTime? FromDate = null, DateTime? ToDate = null)
        {
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
            CommonViewModel.listFIntCompute = new List<FIntCompute>();

            if (FromDate == null)
            {
                CommonViewModel.FromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                CommonViewModel.ToDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
            }
            else
            {
                CommonViewModel.FromDate = FromDate;
                CommonViewModel.ToDate = ToDate;
            }

            if (AllotmentNo == null)
                { CommonViewModel.AllotmentNo = null; }
            else
            { CommonViewModel.AllotmentNo = AllotmentNo; }
            var ApplicantLov = dropDownListBindWeb.AllotementNoLOVBindnewall();
            CommonViewModel.AllotementNoLOVBind = ApplicantLov ?? new List<SelectListItem>();
            CommonViewModel.PlantCD = PlantCD;
           

            CommonViewModel = GetRentList(CommonViewModel, CommonViewModel.FromDate, CommonViewModel.ToDate, CommonViewModel.PlantCD,CommonViewModel.AllotmentNo);
            CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
            CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();

            //CommonViewModel.PlantCD = PlantCD;
            //CommonViewModel.OccupantCode = OccupantCode;
            //CommonViewModel.OccupantType = OccupantType;

            return View(CommonViewModel);
        }


        public IActionResult Query(NERSC05ViewModel nERSC05ViewModel)
        {
            try
            {
                CommonViewModel = GetRentList(nERSC05ViewModel, Convert.ToDateTime(nERSC05ViewModel.FromDate), Convert.ToDateTime(nERSC05ViewModel.ToDate), nERSC05ViewModel.PlantCD,nERSC05ViewModel.AllotmentNo);
                
                TempData["CommonViewModel"] = JsonConvert.SerializeObject(CommonViewModel);
                CommonViewModel.IsAlertBox = false;
                CommonViewModel.SelectedAction = "GetListSearch";
                CommonViewModel.ErrorMessage = "";
                CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
                CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();
            }
            catch (Exception ex)
            {
                // Handle exception
            }
            return Json(CommonViewModel);

        }
        [HttpPost]
        public NERSC05ViewModel GetRentList(NERSC05ViewModel nERSC05ViewModel, DateTime? FromDate, DateTime? ToDate, string PlantCD ,string AllotmentNo)
        {
            CommonViewModel = nERSC05ViewModel;
            int PersonnelNumber = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
            if (FromDate == null) { var first = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1); FromDate = first; }
            if (ToDate == null) { ToDate = DateTime.Today; }

            if (AllotmentNo == null)
            { CommonViewModel.listFIntCompute = nERRSCommonService.FinalIntCompute(PlantCD, FromDate.Value, ToDate.Value); }
            else
            { CommonViewModel.listFIntCompute = nERRSCommonService.FinalIntComputenew(PlantCD, AllotmentNo); }

            CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
            CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();
            return CommonViewModel;
        }

        public ActionResult GetRentCode(int allotmentNo)
        {
            try
            {

                var obj = dropDownListBindWeb.FetchRentCodeFromDatabase(allotmentNo);


                if (obj == null)
                {

                    return Json(new { rentCode = new string[0] });
                }


                var rentCodeArray = obj.Select(rc => rc.RentCode).ToArray();


                return Json(new { rentCode = rentCodeArray });
            }
            catch (Exception ex)
            {

                return BadRequest("An error occurred while fetching rent codes");
            }
        }
    }
    
}
