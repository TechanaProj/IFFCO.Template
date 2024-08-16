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
        public ActionResult Compute(DateTime? FromDate, DateTime? ToDate, string AllotmentNo, int slNo, int elecRate, int electricityCount)
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


                //if (TimePeriod == "M")
                //{

                //    if (FromDate.Value.Month != ToDate.Value.Month || FromDate.Value.Year != ToDate.Value.Year)
                //    {
                //        return Json(new { success = false, error = "From date and To date must be within the same month." });
                //    }

                   int daysInMonth = DateTime.DaysInMonth(FromDate.Value.Year, FromDate.Value.Month);
                //    if (FromDate.Value.Day != 1 || ToDate.Value.Day != daysInMonth)
                //    {
                //        return Json(new { success = false, error = $"From date and To date must cover the entire month. For {FromDate.Value.ToString("MMMM")}, it should be from 1 to {daysInMonth}." });
                //    }
                //}
                //else if (TimePeriod == "D")
                //{

                //    if (selectedPeriod.Days > 31)
                //    {
                //        return Json(new { success = false, error = "The selected period cannot exceed 31 days." });
                //    }
                //}


               

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

              
                return Json(new { sucess = false, error = "An error occurred while processing the request. Details: " + ex.Message });
            }
        }

        private double ExecuteProcedure(DateTime fromDate, DateTime toDate, int allotmentNo, int slNo, int empId, int elecRate, int electricityCount)
        {
            var fdt = fromDate.ToString("yyyy-MM-dd");
            var tdt = toDate.ToString("yyyy-MM-dd");
            try
            {
                List<OracleParameter> oracleParameterCollection = new List<OracleParameter>
        {
            new OracleParameter { ParameterName = "p_allotment_id", OracleDbType = OracleDbType.Int64, Value = allotmentNo },
           // new OracleParameter { ParameterName = "p_from_date", OracleDbType = OracleDbType.Date, Value = fromDate.Date },
            new OracleParameter { ParameterName = "p_from_date", OracleDbType = OracleDbType.VarChar, Value = fdt },
            new OracleParameter { ParameterName = "p_to_date", OracleDbType = OracleDbType.VarChar, Value = tdt },
            //new OracleParameter { ParameterName = "p_to_date", OracleDbType = OracleDbType.Date, Value = toDate.Date },
            new OracleParameter { ParameterName = "p_personal_number", OracleDbType = OracleDbType.Int64, Value = empId },
            new OracleParameter { ParameterName = "p_sl_no", OracleDbType = OracleDbType.Int64, Value = slNo },
            new OracleParameter { ParameterName = "p_electricity_count", OracleDbType = OracleDbType.Int64, Value = electricityCount },
            new OracleParameter { ParameterName = "p_electricity_rate", OracleDbType = OracleDbType.Int64, Value = elecRate },
            new OracleParameter { ParameterName = "o_total_amount", OracleDbType = OracleDbType.Double, Direction = ParameterDirection.Output }
        };

                // Convert the List<OracleParameter> to an object array as required by the ExecuteProcedure method
                object[] parameters = new object[] { oracleParameterCollection };

                // Call the ExecuteProcedure method to run the stored procedure
                _context.ExecuteProcedure("PROCESS_ALLOTMENT", parameters);

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

