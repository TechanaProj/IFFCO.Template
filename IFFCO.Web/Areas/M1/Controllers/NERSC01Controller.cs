using Microsoft.AspNetCore.Mvc;
using IFFCO.NERRS.Web.CommonFunctions;
using IFFCO.NERRS.Web.Controllers;
using IFFCO.NERRS.Web.Models;
using IFFCO.NERRS.Web.ViewModels;
using IFFCO.HRMS.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace IFFCO.NERRS.Web.Areas.M1.Controllers
{
    [Area("M1")]
    public class NERSC01Controller : BaseController<NERSC01ViewModel>
    {
        private readonly ModelContext _context;
        private readonly DropDownListBindWeb dropDownListBindWeb;
        private readonly NERRSCommonService nERRSCommonService;
        private readonly PrimaryKeyGen primaryKeyGen;
        private readonly CommonException<NERSC01ViewModel> commonException;
        public ReportRepositoryWithParameters reportRepository;

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
            CommonViewModel.listVwAonlaDeathCaseAllotStatus = new List<VwAonlaDeathCaseAllotStatus>();
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
                // Handle exception
            }
            return Json(CommonViewModel);



        }



        [HttpPost]
        public NERSC01ViewModel GetRentList(NERSC01ViewModel nERSC01ViewModel, string PlantCD, string OccupantCode)
        {
            CommonViewModel = nERSC01ViewModel;
            int PersonnelNumber = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
            CommonViewModel.listVwAonlaConsultantAllotStatus = new List<VwAonlaConsultantAllotStatus>();
            CommonViewModel.listVwAonlaNonEmpAllotStatus = new List<VwAonlaNonEmpAllotStatus>();
            CommonViewModel.listVwAonlaExEmpAllotStatus = new List<VwAonlaExEmpAllotStatus>();
            CommonViewModel.listVwAonlaDeathCaseAllotStatus = new List<VwAonlaDeathCaseAllotStatus>();



            if (OccupantCode == "1001")
            {
                CommonViewModel.listVwAonlaConsultantAllotStatus = nERRSCommonService.VwAonlaConsultantDtls(PlantCD);
            }
            else if (OccupantCode == "1002")
            {
                CommonViewModel.listVwAonlaDeathCaseAllotStatus = nERRSCommonService.VwAonlaDeathCaseAllotStatus(PlantCD);
            }
            else if (OccupantCode == "1016")
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

        public async Task<IActionResult> GetListSearch()
        {
            NERSC01ViewModel CommonViewModel = new NERSC01ViewModel();
            CommonViewModel = JsonConvert.DeserializeObject<NERSC01ViewModel>(TempData["CommonViewModel"].ToString());
            return View("Index", CommonViewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(NERSC01ViewModel nERSC01ViewModel)
        {
            string personnelNumber = Convert.ToString(HttpContext.Session.GetInt32("EmpID"));

            try
            {
                // Assuming you want to save the first row in the list
                var value = nERSC01ViewModel.listVwAonlaConsultantAllotStatus.FirstOrDefault();

                if (value != null)
                {
                    if (!_context.FAllotmentRentDtls.Any(x => x.UnitCode.Equals(value.UnitCode) && x.AllotmentNo == value.AllotmentNo))
                    {
                        var newRecord = new FAllotmentRentDtls
                        {
                           // UnitCode = Int32.Parse(value.UnitCode),
                            AllotmentNo = value.AllotmentNo,
                            //OccupantCode = value.OccupantType,
                           // QuarterCategory = value.QuarterCategory,
                           // QuarterNo = Int32.Parse(value.QuarterNo),
                          //  RentCode = value.RentType,
                            CreatedBy = personnelNumber,
                            DatetimeCreated = DateTime.Now
                        };

                        _context.Add(newRecord);
                        await _context.SaveChangesAsync();

                        CommonViewModel.Status = "Create";
                        CommonViewModel.Message = "Record Created";
                    }
                    else
                    {
                        CommonViewModel.Message = "Record already exists";
                        CommonViewModel.Alert = "Warning";
                        CommonViewModel.Status = "Warning";
                    }
                }
                else
                {
                    CommonViewModel.Message = "No data to save";
                    CommonViewModel.Alert = "Warning";
                    CommonViewModel.Status = "Warning";
                }

                CommonViewModel.IsAlertBox = true;
                CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
                CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();
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





        //public async Task<IActionResult> Add1(NERSC01ViewModel nERSC01ViewModel )
        //{
        //    int counter = 0;
        //    List<FAllotmentRentDtls> listFAllotmentRentDtls = new List<FAllotmentRentDtls>();

        //    string occupantType = nERSC01ViewModel.OccupantType;
        //    string personnelNumber = Convert.ToString(HttpContext.Session.GetInt32("EmpID"));

        //    try
        //    {
        //       // if (!string.IsNullOrWhiteSpace(occupantType))
        //      //  {
        //         //   switch (occupantType)
        //          //  {
        //             //   case "E": // Employees
        //                    foreach (var value in nERSC01ViewModel.listVwAonlaConsultantAllotStatus)
        //                    {
        //                        if (!_context.FAllotmentRentDtls.Any(x => x.UnitCode.Equals(value.UnitCode) && x.AllotmentNo == value.AllotmentNo))
        //                        {
        //                            var newRecord = new FAllotmentRentDtls
        //                            {
        //                                //UnitCode = value.UnitCode,
        //                                AllotmentNo = value.AllotmentNo,
        //                                OccupantCode = value.OccupantType,
        //                                QuarterCategory= value.QuarterCategory,
        //                                QuarterNo= Int32.Parse(value.QuarterNo),
        //                                RentCode = value.RentType,
        //                                CreatedBy = personnelNumber,
        //                                DatetimeCreated = DateTime.Now
        //                            };


        //                            _context.Add(newRecord);
        //                             counter++;
        //                        }
        //                    }

        //        //if (listFAllotmentRentDtls.Any())
        //        if (counter > 0)
        //        {

        //                        await _context.SaveChangesAsync();
        //                        CommonViewModel.Status = "Create";
        //                        CommonViewModel.Message = $"{counter} Records Created";

        //        }

        //                 //   break;

        //             //   default:
        //               //     break;
        //           // }
        //      //  }
        //        else
        //        {
        //            CommonViewModel.Message = "Please check data again";
        //            CommonViewModel.Alert = "Warning";
        //            CommonViewModel.Status = "Warning";
        //            CommonViewModel.ErrorMessage = "1";
        //        }

        //        CommonViewModel.IsAlertBox = true;
        //        CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
        //        CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();
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


    }
}
