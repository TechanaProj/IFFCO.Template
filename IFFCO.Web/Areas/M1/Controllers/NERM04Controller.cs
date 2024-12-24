using IFFCO.HRMS.Service;
using IFFCO.NERRS.Web.CommonFunctions;
using IFFCO.NERRS.Web.Models;
using IFFCO.NERRS.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using IFFCO.NERRS.Web.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IFFCO.NERRS.Web.Areas.M1.Controllers
{
    [Area("M1")]
    public class NERM04Controller : BaseController<NERM04ViewModel>
    {
        private readonly ModelContext _context;
        private readonly DropDownListBindWeb dropDownListBindWeb;
        private readonly NERRSCommonService nERRSCommonService;
        private readonly PrimaryKeyGen primaryKeyGen;
        private readonly CommonException<NERM04ViewModel> commonException;
        public ReportRepositoryWithParameters reportRepository;


        public NERM04Controller(ModelContext context)
        {

            _context = context;
            nERRSCommonService = new NERRSCommonService();
            commonException = new CommonException<NERM04ViewModel>();
            reportRepository = new ReportRepositoryWithParameters();
            dropDownListBindWeb = new DropDownListBindWeb();
            primaryKeyGen = new PrimaryKeyGen();
        }
        // GET: M1/NERM04
      
        public async Task<IActionResult> Index()
        {
            int PersonnelNumber = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
            int Unit = Convert.ToInt32(HttpContext.Session.GetInt32("UnitCode"));
            CommonViewModel.ListMVendorMsts = new List<MVendorMsts>();
            CommonViewModel.ListMVendorMsts = _context.MVendorMsts.ToList();
            return View(CommonViewModel);
        }


        public IActionResult Create()
        {
            int PersonnelNumber = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
            int Unit = Convert.ToInt32(HttpContext.Session.GetInt32("UnitCode"));
            CommonViewModel.ListMVendorMsts = new List<MVendorMsts>();
            CommonViewModel.ListMVendorMsts = _context.MVendorMsts.ToList();
            // CommonViewModel.PlantCDLOVList = dropDownListBindWeb.GetPlantCDLOV(PersonnelNumber);
            // ViewBag.StateCDLOVList = dropDownListBindWeb.GetStateLOV();
            CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();  // Populating Area name for forming the page URL
            CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString(); // Populating Menu name for forming the page URL
            CommonViewModel.Status = "Create";
            return View("Index", CommonViewModel);
        }

        // POST: M1/NERM04/Create
        // POST: M1/NERM04/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MVendorMsts model)
        {
            int PersonnelNumber = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
            int Unit = Convert.ToInt32(HttpContext.Session.GetInt32("UnitCode"));
            CommonViewModel.ListMVendorMsts = new List<MVendorMsts>();

            // Check if the model is valid
            if (ModelState.IsValid)
            {
                try
                {
                   
                    // Create the new vendor object and populate the required fields
                    MVendorMsts newVendor = new MVendorMsts
                    {
                        VendorId = model.VendorId,
                        VendorCode = model.VendorCode,
                        CreatedBy = Convert.ToString(PersonnelNumber),
                        DatetimeCreated = DateTime.Now,
                   
                        VendorName = model.VendorName,
                        VendorSiteId = model.VendorSiteId,
                        VendorSiteCode = model.VendorSiteCode,
                        City = model.City,
                        State = model.State,
                        Country = model.Country
                        
                    };

                    // Add the new vendor to the database context
                    _context.MVendorMsts.Add(newVendor);

                    // Save the changes asynchronously
                    await _context.SaveChangesAsync();

                    // Prepare success message
                    CommonViewModel.Message = "Vendor with Rent Code " + model.VendorCode + " has been successfully created.";
                    CommonViewModel.Alert = "Create";
                    CommonViewModel.Status = "Create";

                    // Reset error message
                    CommonViewModel.ErrorMessage = "";

                    // Return to the Index page or send back success response
                    CommonViewModel.ListMVendorMsts = _context.MVendorMsts.ToList(); // Update the vendor list
                    CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
                    CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();

                    return Json(CommonViewModel); // Sending back the success response
                }
                catch (Exception ex)
                {
                    // Handle any exception and add error message to ModelState
                    CommonViewModel.ErrorMessage = $"Error occurred: {ex.Message}";
                    ModelState.AddModelError("", $"Error occurred: {ex.Message}");

                    // Return the same view with error message
                    CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
                    CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();
                    return Json(CommonViewModel); // Return JSON response with error information
                }
            }

            // If the model is not valid, return the same page with validation errors
            CommonViewModel.ListMVendorMsts = _context.MVendorMsts.ToList(); // Rebind list of vendors
            CommonViewModel.Status = "Create"; // Keep the "Create" status

            return View("Index", CommonViewModel); // Return the view with updated model
        }

        
        // GET: M1/NERM04/Edit
        public IActionResult Edit(int VendorId)
        {
            int PersonnelNumber = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
            int Unit = Convert.ToInt32(HttpContext.Session.GetInt32("UnitCode"));

            // Fetch the vendor details for the given VendorCode
            var vendor = _context.MVendorMsts.FirstOrDefault(v => v.VendorId == VendorId);
            if (vendor == null)
            {
                CommonViewModel.ErrorMessage = $"Vendor with code {VendorId} does not exist.";
                return View("Index", CommonViewModel);
            }

            CommonViewModel.ObjMVendorMsts = vendor; // Load the vendor details into the ViewModel
            CommonViewModel.ListMVendorMsts = _context.MVendorMsts.ToList(); // Load all vendors
            CommonViewModel.Status = "Edit"; // Set status to Edit
            CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
            CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();

            return View("Index", CommonViewModel); // Render the Index view with the Edit form preloaded
        }
        // POST: M1/NERM04/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MVendorMsts model)
        {
            int PersonnelNumber = Convert.ToInt32(HttpContext.Session.GetInt32("EmpID"));
            int Unit = Convert.ToInt32(HttpContext.Session.GetInt32("UnitCode"));

            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the existing vendor based on the VendorCode
                    var existingVendor = _context.MVendorMsts.FirstOrDefault(v => v.VendorCode == model.VendorCode);
                    if (existingVendor == null)
                    {
                        CommonViewModel.ErrorMessage = $"Vendor with code {model.VendorCode} does not exist.";
                        return Json(CommonViewModel); // Return JSON response with an error
                    }

                    // Update vendor details
                    existingVendor.VendorName = model.VendorName;
                    existingVendor.VendorSiteId = model.VendorSiteId;
                    existingVendor.VendorSiteCode = model.VendorSiteCode;
                    existingVendor.City = model.City;
                    existingVendor.State = model.State;
                    existingVendor.Country = model.Country;
                    existingVendor.CreatedBy = Convert.ToString(PersonnelNumber);
                    existingVendor.DatetimeCreated = DateTime.Now;

                    // Save changes asynchronously
                    await _context.SaveChangesAsync();

                    // Prepare success response
                    CommonViewModel.Message = $"Vendor with code {model.VendorCode} has been successfully updated.";
                    CommonViewModel.Alert = "Edit";
                    CommonViewModel.Status = "Edit";
                    CommonViewModel.ErrorMessage = "";

                    // Refresh the vendor list
                    CommonViewModel.ListMVendorMsts = _context.MVendorMsts.ToList();
                    CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
                    CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();

                    return Json(CommonViewModel); // Return success response
                }
                catch (Exception ex)
                {
                    CommonViewModel.ErrorMessage = $"Error occurred: {ex.Message}";
                    ModelState.AddModelError("", $"Error occurred: {ex.Message}");

                    // Return JSON response with error information
                    return Json(CommonViewModel);
                }
            }

            // If model state is invalid, return the same page with errors
            CommonViewModel.ListMVendorMsts = _context.MVendorMsts.ToList();
            CommonViewModel.Status = "Edit";

            return View("Index", CommonViewModel);
        }
        //[HttpPost]
        //public IActionResult DeleteConfirmed(string id)
        //{
        //    try
        //    {
        //        // Logic to delete the vendor using the provided 'id'
        //        bool success = DeleteVendorById(id);

        //        if (success)
        //        {
        //            // Redirect back to the list or wherever appropriate
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            // Show an error message if the delete fails
        //            ViewData["ErrorMessage"] = "Error: Unable to delete vendor.";
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any exceptions
        //        ViewData["ErrorMessage"] = $"Error: {ex.Message}";
        //        return RedirectToAction("Index");
        //    }
        //}
        //private bool DeleteVendorById(string vendorCode)
        //{
        //    // Implement your delete logic here
        //    // Example:
        //    var vendor = _context.MVendorMsts.FirstOrDefault(v => v.VendorCode == vendorCode);
        //    if (vendor != null)
        //    {
        //        _context.MVendorMsts.Remove(vendor);
        //        _context.SaveChanges();
        //        CommonViewModel.Message = $"Vendor with code {vendor.VendorCode} has been successfully updated.";
        //        CommonViewModel.Alert = "Edit";
        //        CommonViewModel.Status = "deleted";
        //        CommonViewModel.ErrorMessage = "";
        //        return true;
        //    }
        //    return false;
        //}

        // POST: RECMSC01Controller/Delete
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id != null)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        int count = _context.MVendorMsts.Where(x => x.VendorCode.Equals(id)).ToList().Count;
                        var DeleteDataTemp = _context.MVendorMsts.Find(id);
                        if (DeleteDataTemp != null && count == 0)
                        {
                            _context.MVendorMsts.Remove(DeleteDataTemp);
                            _context.SaveChanges();
                            CommonViewModel.Message = "State Code - " + id;
                            CommonViewModel.Alert = "Delete";
                            CommonViewModel.Status = "Delete";
                            CommonViewModel.ErrorMessage = "";
                        }
                        else
                        {
                            CommonViewModel.Message = "Cannot Perform Delete Operation.";
                            CommonViewModel.ErrorMessage = "Cannot Perform Delete Operation.";
                            CommonViewModel.Alert = "Warning";
                            CommonViewModel.Status = "Warning";
                        }

                    }
                    else
                    {
                        CommonViewModel.Message = "State Code Unavailable";
                        CommonViewModel.ErrorMessage = "State Code Unavailable";
                        CommonViewModel.Alert = "Warning";
                        CommonViewModel.Status = "Warning";
                    }

                }
                catch (Exception ex)
                {
                    commonException.GetCommonExcepton(CommonViewModel, ex);
                    CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
                    CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();

                }
            }

            CommonViewModel.AreaName = this.ControllerContext.RouteData.Values["area"].ToString();
            CommonViewModel.SelectedMenu = this.ControllerContext.RouteData.Values["controller"].ToString();
            return Json(CommonViewModel);
        }

    }
}
