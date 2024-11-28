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
using System.Globalization;

namespace IFFCO.NERRS.Web.Areas.M1.Controllers
{
    [Area("M1")]
    public class NERSC03Controller : BaseController<NERSC03ViewModel>
    {
        private readonly ModelContext _context;
        private readonly DropDownListBindWeb dropDownListBindWeb;
        private readonly NERRSCommonService nERRSCommonService;
        private readonly PrimaryKeyGen primaryKeyGen;
        private readonly CommonException<NERSC03ViewModel> commonException;
        public ReportRepositoryWithParameters reportRepository;

        public NERSC03Controller(ModelContext context)
        {
            _context = context;
            nERRSCommonService = new NERRSCommonService();
            commonException = new CommonException<NERSC03ViewModel>();
            reportRepository = new ReportRepositoryWithParameters();
            dropDownListBindWeb = new DropDownListBindWeb();
            primaryKeyGen = new PrimaryKeyGen();
        }

        public IActionResult Index(string PlantCD = null, string OccupantCode = null)
        {
            Console.WriteLine($"Received PlantCD: {PlantCD}");
            Console.WriteLine($"Received OccupantCode: {OccupantCode}");
            int EMP_ID = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
            string moduleid = Convert.ToString(HttpContext.Session.GetString("ModuleID"));

            if (Convert.ToString(TempData["Message"]) != "")
            {
                CommonViewModel.Message = Convert.ToString(TempData["Message"]);
                CommonViewModel.Alert = Convert.ToString(TempData["Alert"]);
                CommonViewModel.Status = Convert.ToString(TempData["Status"]);
            }

            CommonViewModel.UnitLOVBind = dropDownListBindWeb.GetUnitWithSecurity(Convert.ToString(EMP_ID), moduleid);
            CommonViewModel.QuarterLOVBind = dropDownListBindWeb.QuarterLOVBind();
            CommonViewModel.ShutDownOccupantLOVBind = dropDownListBindWeb.ShutDownOccupantLOVBind();
            CommonViewModel.ShutDownRentTypeLOVBind = dropDownListBindWeb.ShutDownRentTypeLOVBind();
            CommonViewModel.VendorLOVBind = dropDownListBindWeb.VendorLOVBind();
            CommonViewModel.listFAllotmentRentDtls = new List<FAllotmentRentDtls>();
            CommonViewModel.listVwAonlaNonEmpAllotStatusShutdown = new List<VwAonlaNonEmpAllotStatus>();

            // CommonViewModel = GetRentList(CommonViewModel, PlantCD, OccupantCode, QuarterCode);
            CommonViewModel = GetRentList(CommonViewModel, PlantCD, OccupantCode);
            CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
            CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();

            CommonViewModel.PlantCD = PlantCD;
            CommonViewModel.OccupantCode = OccupantCode;  //Quarter code



            return View(CommonViewModel);
        }

        public IActionResult Query(NERSC03ViewModel nERSC03ViewModel)
        {
            try
            {
                // CommonViewModel = GetRentList(nERSC03ViewModel, nERSC03ViewModel.PlantCD, nERSC03ViewModel.OccupantCode, nERSC03ViewModel.QuarterCode);
                CommonViewModel = GetRentList(nERSC03ViewModel, nERSC03ViewModel.PlantCD, nERSC03ViewModel.OccupantCode);

                foreach (var item in nERSC03ViewModel.listVwAonlaNonEmpAllotStatusShutdown)
                {
                    item.IsVendorDropdownDisabled = !string.IsNullOrEmpty(item.VendorCode);
                    item.VendorSelectList = new SelectList(nERSC03ViewModel.VendorLOVBind, "Value", "Text", item.VendorCode);
                }

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
        public NERSC03ViewModel GetRentList(NERSC03ViewModel nERSC03ViewModel, string PlantCD, string OccupantCode)
        {
            CommonViewModel = nERSC03ViewModel;
            int PersonnelNumber = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
            CommonViewModel.listVwAonlaNonEmpAllotStatusShutdown = new List<VwAonlaNonEmpAllotStatus>();
            // CommonViewModel.listVwAonlaNonEmpAllotStatus = nERRSCommonService.VwAonlaNonEmpAllotStatus(PlantCD, OccupantCode);
            CommonViewModel.listVwAonlaNonEmpAllotStatusShutdown = nERRSCommonService.VwAonlaNonEmpShutDown(PlantCD, OccupantCode);
            //  CommonViewModel.listVwAonlaNonEmpAllotStatusShutdown = nERRSCommonService.VwAonlaNonEmpShutDown(PlantCD, OccupantCode,QuarterCode);

            CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
            CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();
            return CommonViewModel;
        }

        public async Task<IActionResult> GetListSearch()
        {
            NERSC03ViewModel CommonViewModel = new NERSC03ViewModel();
            CommonViewModel = JsonConvert.DeserializeObject<NERSC03ViewModel>(TempData["CommonViewModel"].ToString());
            return View("Index", CommonViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Add(NERSC03ViewModel nERSC03ViewModel)
        {
            string personnelNumber = Convert.ToString(HttpContext.Session.GetInt32("EmpID"));
            FAllotmentRentDtls fAllotmentRentDtls = new FAllotmentRentDtls();


            try
            {


                foreach (var value in nERSC03ViewModel.listVwAonlaNonEmpAllotStatusShutdown)
                {

                    var alt = value.AllotmentNo;
                    var s1no = value.SlNo; //New Change
                    var allotmentRentDetail = _context.FAllotmentRentDtls.SingleOrDefault(x => x.UnitCode == Convert.ToInt32(nERSC03ViewModel.PlantCD) && x.AllotmentNo == value.AllotmentNo && x.SlNo == s1no);
                    int sno = allotmentRentDetail?.SlNo ?? 0;
                    DateTime? v_date = allotmentRentDetail?.VacancyDate;
                    DateTime? v_date_model = string.IsNullOrEmpty(value.VacancyDate_Text) ? null : (DateTime?)DateTime.ParseExact(value.VacancyDate_Text.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    // Check if the record already exists
                    if (!_context.FAllotmentRentDtls.Any(x => x.UnitCode == Convert.ToInt32(nERSC03ViewModel.PlantCD) && x.AllotmentNo == value.AllotmentNo && x.SlNo == s1no && v_date == v_date_model))
                    {

                        DateTime? Dtime = null;
                        DataTable dt = _context.GetSQLQuery(" select UNIT_CODE, ALLOTMENT_NO,QUARTER_FOR,ALLOTMENT_TYPE,QUARTER_ISSUED_TO,ISSUSE_TO,QUARTER_NAME_FOR,APPLICATION_DATE, APPROVED_DATE, QUARTER_CATEGORY, QUARTER_NO, OCCUPANCY_DATE, VACANCY_DATE " +
                                                          "from VW_AONLA_NON_EMP_ALLOT_STATUS where ALLOTMENT_NO = " + alt + " and UNIT_CODE= ' " + nERSC03ViewModel.PlantCD + " '  ");

                        List<VwAonlaNonEmpAllotStatus> DTL_VALUE = new List<VwAonlaNonEmpAllotStatus>();
                        DTL_VALUE = (from DataRow dr in dt.Rows
                                     select new VwAonlaNonEmpAllotStatus()
                                     {

                                         UnitCode = Convert.ToString(dr["UNIT_CODE"]),
                                         AllotmentNo = (dr["ALLOTMENT_NO"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["ALLOTMENT_NO"]),
                                         QuarterNo = Convert.ToString(dr["QUARTER_NO"]),
                                         QuarterNameFor = Convert.ToString(dr["QUARTER_NAME_FOR"]),
                                         //PersonalNo = Convert.ToString(dr["PERSONAL_NO"]),
                                         ApprovedDate = Convert.ToDateTime(dr["APPROVED_DATE"]),
                                         QuarterCategory = Convert.ToString(dr["QUARTER_CATEGORY"]),
                                         OccupancyDate = Convert.ToDateTime(dr["OCCUPANCY_DATE"]),
                                         VacancyDate = string.IsNullOrEmpty(Convert.ToString(dr["VACANCY_DATE"])) ? Dtime : Convert.ToDateTime(Convert.ToString(dr["VACANCY_DATE"])),


                                     }).ToList();



                        string sqlquery = "select UNIT_CODE,RENT_CODE,TYPE_RESI_ACCOM,RATES,MONTH_DAY_TYPE from M_RENT_MSTS where Status = 'A' ";
                        DataTable dtDRP_VALUE = _context.GetSQLQuery(sqlquery);


                        foreach (var xy in DTL_VALUE)
                        {
                            DataRow[] filteredRows = dtDRP_VALUE.Select("RENT_CODE = '" + value.RentType + "'");


                            var y = _context.FAllotmentRentDtls.Where(z => z.AllotmentNo == xy.AllotmentNo && value.SlNo == sno).FirstOrDefault();
                            if (y != null)
                            {
                                //y.UnitCode = Convert.ToInt32(xy.UnitCode);
                               // y.AllotmentNo = xy.AllotmentNo;
                               // y.VendorCode = value.VendorCode;
                              //  y.QuarterCategory = xy.QuarterCategory;
                                //y.QuarterNo = Convert.ToInt32(xy.QuarterNo);
                                //y.OccupantCode = Convert.ToString(1010);
                                //y.RentCode = value.RentType;
                                //y.NoOfBeds = value.NofBeds;
                                y.VacancyDate = string.IsNullOrEmpty(value.VacancyDate_Text) ? null : (DateTime?)DateTime.ParseExact(value.VacancyDate_Text.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                               // y.MarketHrrFromDate = (DateTime)value.MarketHrrFromDate;
                               // y.RentFromDate = (DateTime)value.MarketHrrFromDate;
                               // y.MonthDayType = (filteredRows != null && filteredRows.Length > 0 ? Convert.ToString(filteredRows[0]["MONTH_DAY_TYPE"]) : "");
                                //y.SlNo = value.SlNo;
                                y.ModifiedBy = personnelNumber;
                                y.DatetimeModified = DateTime.Now;


                                _context.Update(y);
                                _context.SaveChanges();

                                CommonViewModel.Alert = "Update";
                                CommonViewModel.Status = "Update";
                                CommonViewModel.Message = "Record Updated successfully";

                                CommonViewModel.ErrorMessage = "";
                            }
                            else
                            {
                                fAllotmentRentDtls = new FAllotmentRentDtls
                                {

                                    UnitCode = Convert.ToInt32(nERSC03ViewModel.PlantCD),
                                    AllotmentNo = alt,
                                    VendorCode = value.VendorCode,
                                    QuarterCategory = xy.QuarterCategory,
                                    QuarterNo = Convert.ToInt32(xy.QuarterNo),
                                    AllotmentDate = (DateTime)xy.ApprovedDate,
                                    NoOfBeds = value.NofBeds > 0 ? value.NofBeds : 1,
                                    VacancyDate = string.IsNullOrEmpty(value.VacancyDate_Text) ? null : (DateTime?)DateTime.ParseExact(value.VacancyDate_Text.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    MarketHrrFromDate = string.IsNullOrEmpty(value.OccupancyDate_Text) ? null : (DateTime?)DateTime.ParseExact(value.OccupancyDate_Text.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    RentFromDate = string.IsNullOrEmpty(value.OccupancyDate_Text) ? null : (DateTime?)DateTime.ParseExact(value.OccupancyDate_Text.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    OccupantCode = Convert.ToString(1010),
                                    RentCode = value.RentType,
                                    MonthDayType = (filteredRows != null && filteredRows.Length > 0 ? Convert.ToString(filteredRows[0]["MONTH_DAY_TYPE"]) : ""),
                                    // SlNo = Convert.ToInt32("1"),
                                    SlNo = value.SlNo,
                                    Status = "A",
                                    CreatedBy = personnelNumber,
                                    DatetimeCreated = DateTime.Now
                                };


                                _context.Add(fAllotmentRentDtls);
                                await _context.SaveChangesAsync();

                                CommonViewModel.Alert = "success";
                                CommonViewModel.Status = "Create";
                                CommonViewModel.Message = "Record created successfully";

                                CommonViewModel.ErrorMessage = "";

                            }

                        }


                    }
                    else
                    {
                        CommonViewModel.Message = "Record already exists";
                        CommonViewModel.Alert = "Warning";
                        CommonViewModel.Status = "Warning";
                    }
                }


                if (nERSC03ViewModel.listVwAonlaNonEmpAllotStatusShutdown.Count == 0)
                {
                    CommonViewModel.Message = "No data to save";
                    CommonViewModel.Alert = "Warning";
                    CommonViewModel.Status = "Warning";
                }

                CommonViewModel.IsAlertBox = true;
                CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
                CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();
                CommonViewModel.SelectedAction = "Index";
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
