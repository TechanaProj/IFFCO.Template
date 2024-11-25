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

            var ApplicantLov = dropDownListBindWeb.AllotementNoLOVBindnew();
            CommonViewModel.AllotementNoLOVBind = ApplicantLov ?? new List<SelectListItem>(); 

            if (!string.IsNullOrEmpty(Convert.ToString(TempData["Message"])))
            {
                CommonViewModel.Message = Convert.ToString(TempData["Message"]);
                CommonViewModel.Alert = Convert.ToString(TempData["Alert"]);
                CommonViewModel.Status = Convert.ToString(TempData["Status"]);
            }

            CommonViewModel.listFAllotmentRentDtls = new List<FAllotmentRentDtls>();
            CommonViewModel.UnitLOVBind = dropDownListBindWeb.GetUnitWithSecurity(Convert.ToString(EMP_ID), moduleid) ?? new List<SelectListItem>();
            CommonViewModel.FromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            CommonViewModel.ToDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

            //CommonViewModel.ToDate = DateTime.Today;

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

        public ActionResult Compute(DateTime? FromDate, DateTime? ToDate, string AllotmentNo, int slNo, int elecRate, int electricityCount)
        {
            try
            {
                // Validate if FromDate and ToDate are provided
                if (!FromDate.HasValue || !ToDate.HasValue)
                {
                    return Json(new { success = false, error = "From date and To date are required." });
                }

                // Ensure that FromDate and ToDate are within the start and end of the month
                DateTime startOfMonth = new DateTime(FromDate.Value.Year, FromDate.Value.Month, 1);
                DateTime endOfMonth = new DateTime(FromDate.Value.Year, FromDate.Value.Month, DateTime.DaysInMonth(FromDate.Value.Year, FromDate.Value.Month));

                if (FromDate.Value != startOfMonth || ToDate.Value != endOfMonth)
                {
                    return Json(new { success = false, error = "From date and To date must be the start and end of the month." });
                }

                // Parse and validate AllotmentNo
                if (!int.TryParse(AllotmentNo, out int allotmentNoInt))
                {
                    return Json(new { success = false, error = "Invalid Allotment No." });
                }

                // Check if computation for the same month is already done

                var existingComputation = _context.FIntCompute.FirstOrDefault(m =>
                   m.AllotmentNo == allotmentNoInt &&
                   m.SlNo == slNo &&
                   ((FromDate.Value >= m.FromDate && FromDate.Value <= m.ToDate) ||
                    (ToDate.Value >= m.FromDate && ToDate.Value <= m.ToDate) ||
                    (m.FromDate >= FromDate.Value && m.FromDate <= ToDate.Value) ||
                    (m.ToDate >= FromDate.Value && m.ToDate <= ToDate.Value))
               );

                if (existingComputation != null)
                {
                    return Json(new { success = false, error = "Data is already calculated for the selected period." });
                }

                var allotmentDetails = _context.FAllotmentRentDtls.FirstOrDefault(m => m.AllotmentNo == allotmentNoInt && m.SlNo == slNo);
                DateTime currentCheckDate = new DateTime(allotmentDetails.MarketHrrFromDate.Value.Year, allotmentDetails.MarketHrrFromDate.Value.Month, 1);

                while (currentCheckDate < startOfMonth)
                {
                    DateTime monthEnd = new DateTime(currentCheckDate.Year, currentCheckDate.Month, DateTime.DaysInMonth(currentCheckDate.Year, currentCheckDate.Month));

                    var computationForMonth = _context.FIntCompute.FirstOrDefault(m =>
                        m.AllotmentNo == allotmentNoInt &&
                        m.SlNo == slNo &&
                        m.FromDate == currentCheckDate &&
                        m.ToDate == monthEnd);

                    if (computationForMonth == null)
                    {
                        return Json(new { success = false, error = $"Calculation for {currentCheckDate:MMMM yyyy} must be completed before proceeding." });
                    }

                    // Move to the next month
                    currentCheckDate = currentCheckDate.AddMonths(1);
                }

                

                if (allotmentDetails == null)
                {
                    return Json(new { success = false, error = "Allotment details not found in database." });
                }

                // Ensure that FromDate and ToDate are between AllotmentDate and VacancyDate (inclusive)
                if (FromDate.HasValue && ToDate.HasValue && allotmentDetails.MarketHrrFromDate.HasValue &&
                    (FromDate.Value < allotmentDetails.MarketHrrFromDate.Value ||
                     FromDate.Value > allotmentDetails.VacancyDate.Value ||
                     FromDate.Value >= ToDate.Value))
                {
                    return Json(new { success = false, error = "From date must be between the Allotment date and Vacancy date, inclusive, and less than To date." });
                }

                // Calculate the period selected by the user
                TimeSpan selectedPeriod = ToDate.Value - FromDate.Value;

                // Perform the procedure execution
                int EMP_ID = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
                var result = ExecuteProcedure(FromDate.Value, ToDate.Value, allotmentNoInt, slNo, EMP_ID, electricityCount, elecRate);

                // Return success with the result from the procedure
                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine("An error occurred in Compute method: " + ex.Message);
                Console.WriteLine("Stack trace: " + ex.StackTrace);

                // Return error response
                return Json(new { success = false, error = "An error occurred while processing the request. Details: " + ex.Message });
            }
        }

        //        public ActionResult Compute(DateTime? FromDate, DateTime? ToDate, string AllotmentNo, int slNo, int elecRate, int electricityCount)
        //        {
        //        try
        //        {
        //        // Validate if FromDate and ToDate are provided
        //        if (!FromDate.HasValue || !ToDate.HasValue)
        //        {
        //            return Json(new { success = false, error = "From date and To date are required." });
        //        }

        //        // Ensure that FromDate and ToDate are within the start and end of the month
        //        DateTime startOfMonth = new DateTime(FromDate.Value.Year, FromDate.Value.Month, 1);
        //        DateTime endOfMonth = new DateTime(FromDate.Value.Year, FromDate.Value.Month, DateTime.DaysInMonth(FromDate.Value.Year, FromDate.Value.Month));

        //        if (FromDate.Value != startOfMonth || ToDate.Value != endOfMonth)
        //        {
        //            return Json(new { success = false, error = "From date and To date must be the start and end of the month." });
        //        }

        //        // Parse and validate AllotmentNo
        //        if (!int.TryParse(AllotmentNo, out int allotmentNoInt))
        //        {
        //            return Json(new { success = false, error = "Invalid Allotment No." });
        //        }

        //        // Fetch allotment details from the database
        //        var allotmentDetails = _context.FAllotmentRentDtls.FirstOrDefault(m => m.AllotmentNo == allotmentNoInt && m.SlNo == slNo);

        //        if (allotmentDetails == null)
        //        {
        //            return Json(new { success = false, error = "Allotment details not found in database." });
        //        }

        //                // Ensure that FromDate and ToDate are between AllotmentDate and VacancyDate (inclusive)
        //        if (FromDate.HasValue && ToDate.HasValue && allotmentDetails.MarketHrrFromDate.HasValue &&
        //            FromDate.Value < allotmentDetails.MarketHrrFromDate.Value ||
        //            FromDate.Value > allotmentDetails.VacancyDate.Value ||
        //            FromDate.Value >= ToDate.Value)
        //        {
        //              return Json(new { success = false, error = "From date must be between the Allotment date and Vacancy date, inclusive, and less than To date." });
        //        }

        //                // Calculate the period selected by the user
        //                TimeSpan selectedPeriod = ToDate.Value - FromDate.Value;

        //        // Perform the procedure execution
        //        int EMP_ID = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
        //        var result = ExecuteProcedure(FromDate.Value, ToDate.Value, allotmentNoInt, slNo, EMP_ID, electricityCount, elecRate);

        //        // Return success with the result from the procedure
        //        return Json(new { success = true, data = result });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception details
        //        Console.WriteLine("An error occurred in Compute method: " + ex.Message);
        //        Console.WriteLine("Stack trace: " + ex.StackTrace);

        //        // Return error response
        //        return Json(new { success = false, error = "An error occurred while processing the request. Details: " + ex.Message });
        //    }
        //}

        private double ExecuteProcedure(DateTime fromDate, DateTime toDate, int allotmentNo, int slNo, int empId, int elecRate, int electricityCount)
        {
            DateTime FromDate = DateTime.Parse(Request.Form["fromDate"]); 
            DateTime ToDate = DateTime.Parse(Request.Form["toDate"]);

            //var fdt = fromDate.ToString("yyyy-MM-dd");
            //var tdt = toDate.ToString("yyyy-MM-dd");
            try
            {
                //        List<OracleParameter> oracleParameterCollection = new List<OracleParameter>
                //{
                //    new OracleParameter { ParameterName = "p_allotment_id", OracleDbType = OracleDbType.Int64, Value = allotmentNo },
                //   // new OracleParameter { ParameterName = "p_from_date", OracleDbType = OracleDbType.Date, Value = fromDate.Date },
                //    new OracleParameter { ParameterName = "p_from_date", OracleDbType = OracleDbType.VarChar, Value = fdt },
                //    new OracleParameter { ParameterName = "p_to_date", OracleDbType = OracleDbType.VarChar, Value = tdt },
                //    //new OracleParameter { ParameterName = "p_to_date", OracleDbType = OracleDbType.Date, Value = toDate.Date },
                //    new OracleParameter { ParameterName = "p_personal_number", OracleDbType = OracleDbType.Int64, Value = empId },
                //    new OracleParameter { ParameterName = "p_sl_no", OracleDbType = OracleDbType.Int64, Value = slNo },
                //    new OracleParameter { ParameterName = "p_electricity_count", OracleDbType = OracleDbType.Int64, Value = electricityCount },
                //    new OracleParameter { ParameterName = "p_electricity_rate", OracleDbType = OracleDbType.Int64, Value = elecRate },
                //    new OracleParameter { ParameterName = "o_total_amount", OracleDbType = OracleDbType.Double, Direction = ParameterDirection.Output }
                //};

                    List<OracleParameter> oracleParameterCollection = new List<OracleParameter>
                        {
                            new OracleParameter { ParameterName = "p_allotment_id", OracleDbType = OracleDbType.Int64, Value = allotmentNo },
                            new OracleParameter { ParameterName = "p_from_date", OracleDbType = OracleDbType.Date, Value = FromDate },
                            new OracleParameter { ParameterName = "p_to_date", OracleDbType = OracleDbType.Date, Value = ToDate },
                            new OracleParameter { ParameterName = "p_personal_number", OracleDbType = OracleDbType.Int64, Value = empId },
                            new OracleParameter { ParameterName = "p_sl_no", OracleDbType = OracleDbType.Int64, Value = slNo },
                            new OracleParameter { ParameterName = "p_electricity_count", OracleDbType = OracleDbType.Int64, Value = electricityCount },
                            new OracleParameter { ParameterName = "p_electricity_rate", OracleDbType = OracleDbType.Int64, Value = elecRate },
                            new OracleParameter { ParameterName = "o_total_amount", OracleDbType = OracleDbType.Double, Direction = ParameterDirection.Output }
                        };



                // Convert the List<OracleParameter> to an object array as required by the ExecuteProcedure method
                object[] parameters = new object[] { oracleParameterCollection };

              
                _context.ExecuteProcedure("PROCESS_ALLOTMENTV1", parameters);

                // Check if the output parameter is DBNull before casting
                var totalAmountParameter = oracleParameterCollection.Find(p => p.ParameterName == "o_total_amount");
                double totalAmount = (totalAmountParameter.Value != DBNull.Value)
                    ? Convert.ToDouble(totalAmountParameter.Value)
                    : 0.0; // Set a default value (e.g., 0.0) if DBNull

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

