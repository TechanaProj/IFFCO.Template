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
using ModelContext = IFFCO.NERRS.Web.Models.ModelContext;
using System;
using System.Linq;

using System.Collections.Generic;
using Devart.Data.Oracle;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace IFFCO.NERRS.Web.Areas.M1.Controllers
{
    [Area("M1")]
    public class NERSC02Controller : BaseController<NERSC02ViewModel>
    {
        private readonly ModelContext _context;
        private readonly DropDownListBindWeb dropDownListBindWeb = null;
        private readonly NERRSCommonService nERRSCommonService = null;
        private readonly PrimaryKeyGen primaryKeyGen = null;
        CommonException<NERSC02ViewModel> commonException = null;
        public ReportRepositoryWithParameters reportRepository = null;



        public NERSC02Controller(ModelContext context)
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
            CommonViewModel.AllotementNoLOVBind = ApplicantLov ?? new List<SelectListItem>(); 

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
        [HttpGet]
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



        [HttpPost]
        public ActionResult Compute(DateTime? FromDate, DateTime? ToDate, string AllotmentNo, string TimePeriod, int elecRate, int electricityCount)
        {
           
                try
            {

                if (!FromDate.HasValue || !ToDate.HasValue)
                {
                    return Json(new { success = false, error = "From date and To date are required." });
                }



                if (!int.TryParse(AllotmentNo, out int allotmentNoInt))
                {
                    return Json(new { success = false, error = "Invalid Allotment No." });
                }


                var allotmentCompute = _context.FIntCompute.Where(c => c.AllotmentNo == Int32.Parse(AllotmentNo));




                var allotmentDetails = _context.FAllotmentRentDtls.FirstOrDefault(m => m.AllotmentNo == allotmentNoInt);

                if (allotmentDetails == null)
                {
                    return Json(new { success = false, error = "Allotment details not found in database." });
                }


                TimeSpan selectedPeriod = ToDate.Value - FromDate.Value;
                DateTime allotmentStartDate = new DateTime(allotmentDetails.AllotmentDate.Year, allotmentDetails.AllotmentDate.Month, 1);
                TimeSpan allotmentPeriod = (TimeSpan)(allotmentDetails.VacancyDate - allotmentStartDate);


                if (TimePeriod == "M")
                {

                    if (FromDate.Value.Month != ToDate.Value.Month || FromDate.Value.Year != ToDate.Value.Year)
                    {
                        return Json(new { success = false, error = "From date and To date must be within the same month." });
                    }

                    int daysInMonth = DateTime.DaysInMonth(FromDate.Value.Year, FromDate.Value.Month);
                    if (FromDate.Value.Day != 1 || ToDate.Value.Day != daysInMonth)
                    {
                        return Json(new { success = false, error = $"From date and To date must cover the entire month. For {FromDate.Value.ToString("MMMM")}, it should be from 1 to {daysInMonth}." });
                    }
                }
                else if (TimePeriod == "D")
                {

                    if (selectedPeriod.Days > 31)
                    {
                        return Json(new { success = false, error = "The selected period cannot exceed 31 days." });
                    }
                }


               

                // Perform the procedure execution
                int EMP_ID = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
                var result = ExecuteProcedure(FromDate.Value, ToDate.Value, allotmentNoInt, TimePeriod, EMP_ID);

                // Return success with the result from the procedure
                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine("An error occurred in Compute method: " + ex.Message);
                Console.WriteLine("Stack trace: " + ex.StackTrace);

                // Return error response
                return Json(new { sucess = false, error = "An error occurred while processing the request. Details: " + ex.Message });
            }
        }

        private double ExecuteProcedure(DateTime fromDate, DateTime toDate, int allotmentNo, string timePeriod, int empId)
       // private double ExecuteProcedure(DateTime fromDate, DateTime toDate, int allotmentNo, string timePeriod, int empId, int elecRate, int electricityCount)
        {
            try
            {

                /*--New Method Start--*/

                //string OutputStat = string.Empty;
                //OracleParameter O1 = new OracleParameter("p_from_date", fromDate.Date);
                //OracleParameter O2 = new OracleParameter("p_to_date", toDate.Date);
                //OracleParameter O3 = new OracleParameter("p_allotment_id", allotmentNo);
                //OracleParameter O4 = new OracleParameter("p_computation_type", timePeriod);
                //OracleParameter O5 = new OracleParameter("p_personal_number", empId);
                //OracleParameter O6 = new OracleParameter("o_total_amount", OracleDbType.Char, ParameterDirection.Output);
                //string smt1 = "BEGIN CALCULATE_TOTAL_AMOUNT(:p_from_date,:p_to_date,:p_allotment_id,:p_computation_type,:p_personal_number,:o_total_amount); END;";
                //var exec = _context.Database.ExecuteSqlCommand(smt1, O1, O2, O3, O4, O5, O6);
                //OutputStat = O6.OracleValue.ToString();

                List<OracleParameter> oracleParameterCollection = new List<OracleParameter>
                {
                    new OracleParameter { ParameterName = "p_from_date", OracleDbType = OracleDbType.Date, Value = fromDate.Date },
                    new OracleParameter { ParameterName = "p_to_date", OracleDbType = OracleDbType.Date, Value = toDate.Date },
                    new OracleParameter { ParameterName = "p_allotment_id", OracleDbType = OracleDbType.Int64, Value = allotmentNo },
                    new OracleParameter { ParameterName = "p_personal_number", OracleDbType = OracleDbType.Int64, Value = empId },
                    new OracleParameter { ParameterName = "p_computation_type", OracleDbType = OracleDbType.VarChar, Value = timePeriod },
                   // new OracleParameter { ParameterName = "p_electricity_count", OracleDbType = OracleDbType.Int64, Value = electricityCount },
                    //new OracleParameter { ParameterName = "p_electricity_rate", OracleDbType = OracleDbType.Int64, Value = elecRate },
                    new OracleParameter { ParameterName = "p_no_of_beds", OracleDbType = OracleDbType.Int64, Value = 1 },
                    new OracleParameter { ParameterName = "o_total_amount", OracleDbType = OracleDbType.Double, Size = 2000, Direction = ParameterDirection.Output }
                };

                _context.ExecuteProcedure("CALCULATE_TOTAL_AMOUNT", oracleParameterCollection);



                /*--New Method End--*/

                //int rowsAffected = _context.Database.ExecuteSqlCommand(sqlQuery, O1, O2, O3, O4, O5, O6);


                // string totalAmount = O6.Value.ToString();


                // float toatalmount = Convert.ToSingle(oracleParameterCollection.FirstOrDefault(x => x.ParameterName == "o_total_amount").Value;
                double totalAmount = Convert.ToDouble(oracleParameterCollection.Find(p => p.ParameterName == "o_total_amount").Value);

                return totalAmount;
            }
            catch (Exception ex)
            {

                Console.WriteLine("An error occurred while executing the procedure: " + ex.Message);
                throw;
            }


        }









    }
}

