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
using System.Data;
using AspNetCore;

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
            FAllotmentRentDtls fAllotmentRentDtls = new FAllotmentRentDtls();
          
          
            try
            {

            //if(CommonViewModel.OccupantType == '1001')
                    
                

                    foreach (var value in nERSC01ViewModel.listVwAonlaConsultantAllotStatus)
                {
                    // Check if the record already exists
                    if (!_context.FAllotmentRentDtls.Any(x => x.UnitCode == Convert.ToInt32(value.UnitCode) && x.AllotmentNo == value.AllotmentNo))
                    {
                        DateTime? Dtime = null;
                        DataTable dt = _context.GetSQLQuery("select UNIT_CODE,ALLOTMENT_NO,PERSONAL_NO,APPROVED_DATE,QUARTER_CATEGORY, QUARTER_NO,APPROVED_DATE, OCCUPANCY_DATE, VACANCY_DATE " +
                            "from VW_AONLA_CONSULTANT_ALLOT_STATUS where ALLOTMENT_NO = " + value.AllotmentNo + " and UNIT_CODE= '3'  ");

                        List<VwAonlaConsultantAllotStatus> DTL_VALUE = new List<VwAonlaConsultantAllotStatus>();
                        DTL_VALUE = (from DataRow dr in dt.Rows
                                     select new VwAonlaConsultantAllotStatus()
                                     {

                                         UnitCode = Convert.ToString(dr["UNIT_CODE"]),
                                         AllotmentNo = (dr["ALLOTMENT_NO"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["ALLOTMENT_NO"]),
                                         PersonalNo = Convert.ToString(dr["PERSONAL_NO"]),
                                         QuarterNo = Convert.ToString(dr["QUARTER_NO"]),
                                         ApprovedDate = Convert.ToDateTime(dr["APPROVED_DATE"]),
                                        // VacancyDate = string.IsNullOrEmpty(Convert.ToString(dr["VACANCY_DATE"])) ? Dtime : Convert.ToDateTime(Convert.ToString(dr["VACANCY_DATE"])),
                                         QuarterCategory = Convert.ToString(dr["QUARTER_CATEGORY"]),
                                         OccupancyDate = Convert.ToDateTime(dr["OCCUPANCY_DATE"]),
                                         

                                     }).ToList();


                        string sqlquery = "select UNIT_CODE,RENT_CODE,TYPE_RESI_ACCOM,RATES,MONTH_DAY_TYPE from M_RENT_MSTS where Status = 'A' ";
                        DataTable dtDRP_VALUE = _context.GetSQLQuery(sqlquery);
                        
                        foreach (var x in DTL_VALUE)
                        {
                            DataRow[] filteredRows = dtDRP_VALUE.Select("RENT_CODE = '" + value.RentType + "'");

                            var y = _context.FAllotmentRentDtls.Where(z => z.AllotmentNo == x.AllotmentNo).FirstOrDefault();
                            if (y != null)
                            {
                                y.UnitCode = Convert.ToInt32(x.UnitCode);
                                y.AllotmentNo = x.AllotmentNo;
                                y.PersonalNo = Convert.ToInt32(x.PersonalNo);
                                y.QuarterCategory = x.QuarterCategory;
                                y.QuarterNo = Convert.ToInt32(x.QuarterNo);
                                y.OccupantCode = value.OccupantType;
                                y.RentCode = value.RentType;
                                y.VacancyDate = (DateTime)value.VacancyDate;
                                y.MonthDayType = (filteredRows != null && filteredRows.Length > 0 ? Convert.ToString(filteredRows[0]["MONTH_DAY_TYPE"]) : "");
                                y.ModifiedBy = personnelNumber;
                                y.DatetimeModified = DateTime.Now;


                                 _context.Update(y);
                                _context.SaveChanges();
                            }
                            else
                            {
                                fAllotmentRentDtls = new FAllotmentRentDtls
                                {
                                    UnitCode = Convert.ToInt32(x.UnitCode),
                                    AllotmentNo = value.AllotmentNo,
                                    PersonalNo = Convert.ToInt32(x.PersonalNo),
                                    QuarterCategory = x.QuarterCategory,
                                    QuarterNo = Convert.ToInt32(x.QuarterNo),
                                    AllotmentDate = (DateTime)x.ApprovedDate,
                                    //VacancyDate = (DateTime)value.VacancyDate,
                                    OccupantCode = value.OccupantType,
                                    RentCode = value.RentType,
                                    MonthDayType = (filteredRows != null && filteredRows.Length > 0 ? Convert.ToString(filteredRows[0]["MONTH_DAY_TYPE"]) : ""),
                                    SlNo = Convert.ToInt32("1"),
                                    Status = "A",
                                    CreatedBy = personnelNumber,
                                    DatetimeCreated = DateTime.Now
                                };

                               
                                _context.Add(fAllotmentRentDtls);
                                await _context.SaveChangesAsync();

                               

                            }

                        }
                        CommonViewModel.Alert = "success";
                        CommonViewModel.Status = "Create";
                        CommonViewModel.Message = "Record created successfully";

                        CommonViewModel.ErrorMessage = "";


                    }
                    else
                    {
                        CommonViewModel.Message = "Record already exists";
                        CommonViewModel.Alert = "Warning";
                        CommonViewModel.Status = "Warning";
                    }
                }
                

                if (nERSC01ViewModel.listVwAonlaConsultantAllotStatus.Count == 0)
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




       


    }
}
