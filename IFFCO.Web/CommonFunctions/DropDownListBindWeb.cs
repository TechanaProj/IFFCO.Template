using IFFCO.NERRS.Web.Models;
using IFFCO.NERRS.Web.ViewModels;
using IFFCO.HRMS.Repository.Pattern.Core.Factories;
using IFFCO.HRMS.Repository.Pattern.UnitOfWork;
using IFFCO.HRMS.Shared.CommonFunction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;

namespace IFFCO.NERRS.Web.CommonFunctions
{
    public class DropDownListBindWeb : DropDownListBind
    {
        private readonly IRepositoryProvider _repositoryProvider = new RepositoryProvider(new RepositoryFactories());

        private readonly IUnitOfWorkAsync _unitOfWork;

        //IDataContextAsync context;
        private readonly ModelContext _context;
        DataTable _dt = new DataTable();
        public DropDownListBindWeb()
        {
            _context = new ModelContext();
            // context = modelContext;
            //_unitOfWork = new UnitOfWork(_context, _repositoryProvider);
        }

       
        //public List<SelectListItem> GetPlantCDLOV(int PersonnelNo)
        //{
        //    //Returns the plant code LOV for employee as per rights in ADM_EMP_UNIT_ACCESS and DESP_ADM_EMP_PLANT_ACCESS_DTLS
        //    // Used in - DISC01Controller.cs


        //    string sqlquery = " SELECT A.PLANT_CD PLANT, A.PLANT_CD||' - '||B.PLANT_NAME DESCRIPTION FROM (SELECT B.DESP_UNIT_CD PLANT_CD FROM ADM_EMP_UNIT_ACCESS A, DESP_UNIT_MSTS B WHERE A.UNIT_CODE = B.HRMS_UNIT_CODE AND A.EMPID = '"+PersonnelNo+"' AND PROJECTID = 'NERRS' UNION   ";
        //    sqlquery += " SELECT DISTINCT A.PLANT_CD PLANT_CD FROM DESP_ADM_EMP_PLANT_ACCESS_DTLS A WHERE   PERSONAL_NO = '"+PersonnelNo+"' ) A, M_PLANT B WHERE A.PLANT_CD = B.PLANT_CD ";
        //    DataTable dtDRP_VALUE = _context.GetSQLQuery(sqlquery);
        //    List<SelectListItem> DRP_VALUE = new List<SelectListItem>();
        //    DRP_VALUE = (from DataRow dr in dtDRP_VALUE.Rows
        //                 select new SelectListItem()
        //                 {
        //                     Text = Convert.ToString(dr["DESCRIPTION"]),
        //                     Value = Convert.ToString(dr["PLANT"])


        //                 }).ToList();

        //    return DRP_VALUE;

        //}

        public List<SelectListItem> AdmSubMenuModuleLOVBind(string Projid)
        {
            List<SelectListItem> SubMenuCDLOV = new List<SelectListItem>();
            SubMenuCDLOV = _context.AdmProjmodMaster.Where(x => x.Projectid == Projid).Select(x => new SelectListItem
            {
                Text = string.Concat(x.Moduleid, " || ", x.Modulename),
                Value = x.Moduleid.ToString()
            }).ToList();
        
            return SubMenuCDLOV;
        }

      

        public List<SelectListItem> AdmSubMenuParentLOVBind(string Module, string Projid)
        {
             string sqlquery = " select '"+Module+ "' SUB_MENU_ID FROM DUAL UNION ALL SELECT DISTINCT SUB_MENU_ID SUB_MENU_ID FROM ADM_SUB_MENU_MSTS WHERE PROJECTID = '" + Projid+"' AND MODULEID = '"+Module+"'  ";
            DataTable dtDRP_VALUE = _context.GetSQLQuery(sqlquery);
            List<SelectListItem> DRP_VALUE = new List<SelectListItem>();
            DRP_VALUE = (from DataRow dr in dtDRP_VALUE.Rows
                         select new SelectListItem()
                         {
                             Text = Convert.ToString(dr["SUB_MENU_ID"]),
                             Value = Convert.ToString(dr["SUB_MENU_ID"])
                         }).ToList();

            return DRP_VALUE;

        }

        public List<SelectListItem> AdmPrgParentLOVBind(string Module, string Projid)
        {
            string sqlquery = "SELECT DISTINCT SUB_MENU_ID SUB_MENU_ID FROM ADM_SUB_MENU_MSTS WHERE PROJECTID = '" + Projid + "' AND MODULEID = '" + Module + "'  ";
            DataTable dtDRP_VALUE = _context.GetSQLQuery(sqlquery);
            List<SelectListItem> DRP_VALUE = new List<SelectListItem>();
            DRP_VALUE = (from DataRow dr in dtDRP_VALUE.Rows
                         select new SelectListItem()
                         {
                             Text = Convert.ToString(dr["SUB_MENU_ID"]),
                             Value = Convert.ToString(dr["SUB_MENU_ID"])
                         }).ToList();

            return DRP_VALUE;

        }

        public List<SelectListItem> GetUnitWithSecurity(string empid, string moduleid)
        {
            StringBuilder sqlquery = new StringBuilder();
            sqlquery.Append(" select distinct x.unit_code,y.description from (select a.unit_code from adm_emp_unit_access a ");
            sqlquery.Append(" where a.empid= " + empid + " ");
            sqlquery.Append(" and   a.moduleid = '" + moduleid + "' ");
            sqlquery.Append(" and   a.hier_yn = 'N' ");
            sqlquery.Append("   ) x, eb_unit_msts y  ");
            sqlquery.Append("  where y.unit_code = x.unit_code order by 1 ");
            DataTable dt = _context.GetSQLQuery(sqlquery.ToString());
            var appUnitList = (from DataRow dr in dt.Rows
                               select new SelectListItem
                               {
                                   Text = Convert.ToString(Convert.ToString(dr["UNIT_CODE"] + "-" + dr["DESCRIPTION"])),
                                   Value = Convert.ToString(dr["unit_code"])
                               }).ToList();

            return appUnitList;


            //StringBuilder sqlquery = new StringBuilder();
            //sqlquery.Append(" select distinct x.unit_code,y.description from (select a.unit_code from adm_emp_unit_access a ");
            //sqlquery.Append(" where a.empid= " + empid + " ");
            //sqlquery.Append(" and   a.moduleid = '" + moduleid + "' ");
            //sqlquery.Append(" and   a.hier_yn = 'N' union Select b.unit_code from  eb_unit_msts b  "); //where b.unit_code = b.process_unit_code   eb_unit_msts
            //sqlquery.Append(" start with b.unit_code = (select min(c.unit_code) from adm_emp_unit_access c where  c.empid= " + empid + "   ");
            //sqlquery.Append("  and   c.moduleid =  '" + moduleid + "'  ");
            //sqlquery.Append("  and   c.hier_yn = 'Y' )  ");
            //sqlquery.Append("  connect by prior b.unit_code=b.unit_parent_code ) x, eb_unit_msts y  ");
            //sqlquery.Append("  where y.unit_code = x.unit_code order by 1 ");
            //DataTable dt = _context.GetSQLQuery(sqlquery.ToString());
            //var appUnitList = (from DataRow dr in dt.Rows
            //                   select new SelectListItem
            //                   {
            //                       Text = Convert.ToString(Convert.ToString(dr["UNIT_CODE"] + "-" + dr["DESCRIPTION"])),
            //                       Value = Convert.ToString(dr["unit_code"])
            //                   }).ToList();

            //return appUnitList;
        }

        public List<SelectListItem> GetEmpForSecurity(string unit)
        {
            StringBuilder sqlquery = new StringBuilder();
            sqlquery.Append(" select name, personal_no  from VW_DESP_USER_MST where unit_code = '"+unit+ "' order by grade_code asc  ");
            DataTable dt = _context.GetSQLQuery(sqlquery.ToString());
            var appUnitList = (from DataRow dr in dt.Rows
                               select new SelectListItem
                               {
                                   Text = Convert.ToString(Convert.ToString(dr["personal_no"] + " - " + dr["name"])),
                                   Value = Convert.ToString(dr["personal_no"])
                               }).ToList();

            return appUnitList;
        }

        public List<SelectListItem> UnitLOVBind()
        {
            string sqlquery = "select UNIT_CODE,ERP_UNIT_NAME from EB_UNIT_MSTS  ";
            DataTable dtDRP_VALUE = _context.GetSQLQuery(sqlquery);
            List<SelectListItem> DRP_VALUE = new List<SelectListItem>();
            DRP_VALUE = (from DataRow dr in dtDRP_VALUE.Rows
                         select new SelectListItem()
                         {
                             Text = Convert.ToString(dr["ERP_UNIT_NAME"]) + " - " + Convert.ToString(dr["UNIT_CODE"]),
                             Value = Convert.ToString(dr["UNIT_CODE"])


                         }).ToList();

            return DRP_VALUE;

        }



        public List<SelectListItem> OccupantLOVBind()
        {
            string sqlquery = "select UNIT_CODE,OCCUPANT_CODE,OCCUPANT_TYPE from M_OCCUPANT_MSTS where Status = 'A' ";
            DataTable dtDRP_VALUE = _context.GetSQLQuery(sqlquery);
            List<SelectListItem> DRP_VALUE = new List<SelectListItem>();
            DRP_VALUE = (from DataRow dr in dtDRP_VALUE.Rows
                         select new SelectListItem()
                         {
                             Text = Convert.ToString(dr["OCCUPANT_TYPE"]) + " - " + Convert.ToString(dr["OCCUPANT_CODE"]),
                             Value = Convert.ToString(dr["OCCUPANT_CODE"])


                         }).ToList();

            return DRP_VALUE;

        }

        public List<SelectListItem> QuarterLOVBind()
        {
            string sqlquery = "select QUARTER_CATEGORY,QUARTER_NO,QUARTER_ISSUED_TO, UNIT_CODE from VW_AONLA_NON_EMP_ALLOT_STATUS where QUARTER_ISSUED_TO = 'A'";
            DataTable dtDRP_VALUE = _context.GetSQLQuery(sqlquery);
            List<SelectListItem> DRP_VALUE = new List<SelectListItem>();
            DRP_VALUE = (from DataRow dr in dtDRP_VALUE.Rows
                         select new SelectListItem()
                         {
                             Text = Convert.ToString(dr["QUARTER_CATEGORY"]) + " - " + Convert.ToString(dr["QUARTER_NO"]),
                             //Value = Convert.ToString(dr["QUARTER_CATEGORY"]) + " - " + Convert.ToString(dr["QUARTER_NO"]),
                             Value = $"{Convert.ToString(dr["QUARTER_CATEGORY"])}-{Convert.ToString(dr["QUARTER_NO"])}",

                             //Value = Convert.ToString(dr["QUARTER_ISSUED_TO"])


                         }).ToList();

            return DRP_VALUE;

        }



        public List<SelectListItem> OccupantEmpLOVBind()
        {
            string sqlquery = "select UNIT_CODE,OCCUPANT_CODE,OCCUPANT_TYPE from M_OCCUPANT_MSTS where Status = 'A' and QUARTER_FOR = 'E' ";
            DataTable dtDRP_VALUE = _context.GetSQLQuery(sqlquery);
            List<SelectListItem> DRP_VALUE = new List<SelectListItem>();
            DRP_VALUE = (from DataRow dr in dtDRP_VALUE.Rows
                         select new SelectListItem()
                         {
                             Text = Convert.ToString(dr["OCCUPANT_TYPE"]) + " - " + Convert.ToString(dr["OCCUPANT_CODE"]),
                             Value = Convert.ToString(dr["OCCUPANT_CODE"])


                         }).ToList();

            return DRP_VALUE;

        }

        public List<SelectListItem> OccupantNonEmpLOVBind()
        {
            string sqlquery = "select UNIT_CODE,OCCUPANT_CODE,OCCUPANT_TYPE, QUARTER_ISSUED_TO from M_OCCUPANT_MSTS where Status = 'A' and QUARTER_FOR = 'N' ";
            DataTable dtDRP_VALUE = _context.GetSQLQuery(sqlquery);
            List<SelectListItem> DRP_VALUE = new List<SelectListItem>();
            DRP_VALUE = (from DataRow dr in dtDRP_VALUE.Rows
                         select new SelectListItem()
                         {
                             Text = Convert.ToString(dr["OCCUPANT_TYPE"]) + " - " + Convert.ToString(dr["OCCUPANT_CODE"]),
                             Value = Convert.ToString(dr["QUARTER_ISSUED_TO"])
                             //Value = Convert.ToString(dr["OCCUPANT_CODE"])


                         }).ToList();

            return DRP_VALUE;

        }


        public List<SelectListItem> RentTypeLOVBind()
        {
            string sqlquery = "select UNIT_CODE,RENT_CODE,TYPE_RESI_ACCOM,RATES,MONTH_DAY_TYPE from M_RENT_MSTS where Status = 'A' ";
            //string sqlquery = "SELECT UNIT_CODE,RENT_CODE,TYPE_RESI_ACCOM,RATES,MONTH_DAY_TYPE FROM M_RENT_MSTS WHERE RENT_CODE NOT LIKE 'IB%' AND RENT_CODE NOT LIKE 'IG%' AND RENT_CODE NOT LIKE 'GL%' AND STATUS = 'A' ";
             DataTable dtDRP_VALUE = _context.GetSQLQuery(sqlquery);
            List<SelectListItem> DRP_VALUE = new List<SelectListItem>();
            DRP_VALUE = (from DataRow dr in dtDRP_VALUE.Rows
                         select new SelectListItem()
                         {
                             Text = Convert.ToString(dr["TYPE_RESI_ACCOM"]) + " - " + Convert.ToString(dr["RENT_CODE"]) + " -Rates " + Convert.ToString(dr["RATES"]) + " - " + Convert.ToString(dr["MONTH_DAY_TYPE"]),
                             Value = Convert.ToString(dr["RENT_CODE"])


                         }).ToList();

            return DRP_VALUE;

        }

        public List<SelectListItem> ShutDownRentTypeLOVBind()
        {
            string sqlquery = "SELECT UNIT_CODE,RENT_CODE,TYPE_RESI_ACCOM,RATES,MONTH_DAY_TYPE FROM M_RENT_MSTS WHERE STATUS = 'A' AND MONTH_DAY_TYPE = 'D' ";
            DataTable dtDRP_VALUE = _context.GetSQLQuery(sqlquery);
            List<SelectListItem> DRP_VALUE = new List<SelectListItem>();
            DRP_VALUE = (from DataRow dr in dtDRP_VALUE.Rows
                         select new SelectListItem()
                         {
                             Text = Convert.ToString(dr["TYPE_RESI_ACCOM"]) + " - " + Convert.ToString(dr["RENT_CODE"]) + " -Rates " + Convert.ToString(dr["RATES"]) + " - " + Convert.ToString(dr["MONTH_DAY_TYPE"]),
                             Value = Convert.ToString(dr["RENT_CODE"])


                         }).ToList();

            return DRP_VALUE;

        }

        public List<SelectListItem_Custom> RentTypeLOVBindnew()
        {
            //string sqlquery = "select UNIT_CODE,RENT_CODE,TYPE_RESI_ACCOM,RATES,MONTH_DAY_TYPE from M_RENT_MSTS where Status = 'A' ";
             string sqlquery = "SELECT UNIT_CODE,   RENT_CODE, TYPE_RESI_ACCOM,    RATES,    MONTH_DAY_TYPE,   CASE   WHEN SUBSTR(RENT_CODE, 1, 2) = 'IB' THEN 'I' WHEN SUBSTR(RENT_CODE, 1, 2) = 'IG' THEN 'S'  WHEN SUBSTR(RENT_CODE, 1, 2) = 'GL' THEN 'G' ELSE SUBSTR(RENT_CODE, 1, 2)   END AS OccupantCode FROM  M_RENT_MSTS WHERE    STATUS = 'A' ";
           // string sqlquery = "select UNIT_CODE,RENT_CODE,TYPE_RESI_ACCOM,RATES,MONTH_DAY_TYPE, DECODE(SUBSTR(RENT_CODE, 1, 2), 'IB', 'I', 'IG', 'S', 'GL','G', NULL)OccupantCode from M_RENT_MSTS where Status = 'A' ";

            DataTable dtDRP_VALUE = _context.GetSQLQuery(sqlquery);
            List<SelectListItem_Custom> DRP_VALUE = new List<SelectListItem_Custom>();
            DRP_VALUE = (from DataRow dr in dtDRP_VALUE.Rows
                         select new SelectListItem_Custom()
                         {
                             Text = Convert.ToString(dr["TYPE_RESI_ACCOM"]) + " - " + Convert.ToString(dr["RENT_CODE"]) + " -Rates " + Convert.ToString(dr["RATES"]) + " - " + Convert.ToString(dr["MONTH_DAY_TYPE"]),
                             Value = Convert.ToString(dr["RENT_CODE"]),
                             Value2 = Convert.ToString(dr["OccupantCode"])

                         }).ToList();

            return DRP_VALUE;

        }

        public List<SelectListItem> ShutDownOccupantLOVBind()
        {
            string sqlquery = "select UNIT_CODE,OCCUPANT_CODE,OCCUPANT_TYPE from M_OCCUPANT_MSTS where OCCUPANT_CODE = '1010' and  Status = 'A' ";
            DataTable dtDRP_VALUE = _context.GetSQLQuery(sqlquery);
            List<SelectListItem> DRP_VALUE = new List<SelectListItem>();
            DRP_VALUE = (from DataRow dr in dtDRP_VALUE.Rows
                         select new SelectListItem()
                         {
                             Text = Convert.ToString(dr["OCCUPANT_TYPE"]) + " - " + Convert.ToString(dr["OCCUPANT_CODE"]),
                             Value = Convert.ToString(dr["OCCUPANT_CODE"])


                         }).ToList();

            return DRP_VALUE;

        }

        public List<SelectListItem> VendorLOVBind()
        {
            string sqlquery = "select UNIT_CODE,VENDOR_CODE,VENDOR_NAME,VENDOR_SITE_CODE,VENDOR_SITE_ID,HRMS_UNIT_CD from M_VENDOR_MSTS ORDER BY VENDOR_NAME";
            DataTable dtDRP_VALUE = _context.GetSQLQuery(sqlquery);
            List<SelectListItem> DRP_VALUE = new List<SelectListItem>();
            DRP_VALUE = (from DataRow dr in dtDRP_VALUE.Rows
                         select new SelectListItem()
                         {
                             Text = Convert.ToString(dr["VENDOR_NAME"]) + " - " + Convert.ToString(dr["VENDOR_SITE_CODE"]) + " - " + Convert.ToString(dr["VENDOR_CODE"]),
                             Value = Convert.ToString(dr["VENDOR_CODE"])


                         }).ToList();

            return DRP_VALUE;

        }
        public List<SelectListItem> IOBRentTypeLOVBind()
        {
            string sqlquery = "select UNIT_CODE,RENT_CODE,TYPE_RESI_ACCOM,RATES,MONTH_DAY_TYPE from M_RENT_MSTS where RENT_CODE like 'IB%' and Status = 'A' ";
            DataTable dtDRP_VALUE = _context.GetSQLQuery(sqlquery);
            List<SelectListItem> DRP_VALUE = new List<SelectListItem>();
            DRP_VALUE = (from DataRow dr in dtDRP_VALUE.Rows
                         select new SelectListItem()
                         {
                             Text = Convert.ToString(dr["TYPE_RESI_ACCOM"]) + " - " + Convert.ToString(dr["RENT_CODE"]) + " -Rates " + Convert.ToString(dr["RATES"]) + " - " + Convert.ToString(dr["MONTH_DAY_TYPE"]),
                             Value = Convert.ToString(dr["RENT_CODE"])


                         }).ToList();

            return DRP_VALUE;

        }

        public List<SelectListItem> IGTIRentTypeLOVBind()
        {
            string sqlquery = "select UNIT_CODE,RENT_CODE,TYPE_RESI_ACCOM,RATES,MONTH_DAY_TYPE from M_RENT_MSTS where RENT_CODE like 'IG%' and Status = 'A'";
            DataTable dtDRP_VALUE = _context.GetSQLQuery(sqlquery);
            List<SelectListItem> DRP_VALUE = new List<SelectListItem>();
            DRP_VALUE = (from DataRow dr in dtDRP_VALUE.Rows
                         select new SelectListItem()
                         {
                             Text = Convert.ToString(dr["TYPE_RESI_ACCOM"]) + " - " + Convert.ToString(dr["RENT_CODE"]) + " -Rates " + Convert.ToString(dr["RATES"]) + " - " + Convert.ToString(dr["MONTH_DAY_TYPE"]),
                             Value = Convert.ToString(dr["RENT_CODE"])


                         }).ToList();

            return DRP_VALUE;

        }

        public List<SelectListItem> GAILRentTypeLOVBind()
        {
            string sqlquery = "select UNIT_CODE,RENT_CODE,TYPE_RESI_ACCOM,RATES,MONTH_DAY_TYPE from M_RENT_MSTS where RENT_CODE like 'GL%' and Status = 'A'";
            DataTable dtDRP_VALUE = _context.GetSQLQuery(sqlquery);
            List<SelectListItem> DRP_VALUE = new List<SelectListItem>();
            DRP_VALUE = (from DataRow dr in dtDRP_VALUE.Rows
                         select new SelectListItem()
                         {
                             Text = Convert.ToString(dr["TYPE_RESI_ACCOM"]) + " - " + Convert.ToString(dr["RENT_CODE"]) + " -Rates " + Convert.ToString(dr["RATES"]) + " - " + Convert.ToString(dr["MONTH_DAY_TYPE"]),
                             Value = Convert.ToString(dr["RENT_CODE"])


                         }).ToList();

            return DRP_VALUE;

        }
        public List<SelectListItem> AllotementNoLOVBind()
        {
            string sqlquery = "SELECT A.QUARTER_CATEGORY, A.QUARTER_NO, A.ALLOTMENT_NO, A.PERSONAL_NO, B.EMP_NAME FROM F_ALLOTMENT_RENT_DTLS A JOIN V_EB_EMPLOYEE_COMPLETE_DTLS B ON A.PERSONAL_NO = B.PERSONAL_NO WHERE A.Status = 'A'ORDER BY A.ALLOTMENT_NO ASC";
            DataTable dtDRP_VALUE = _context.GetSQLQuery(sqlquery);
            List<SelectListItem> DRP_VALUE = new List<SelectListItem>();
            DRP_VALUE = (from DataRow dr in dtDRP_VALUE.Rows
                         select new SelectListItem()
                         {
                             Text = Convert.ToString(dr["EMP_NAME"]) + " | " + Convert.ToString(dr["QUARTER_CATEGORY"]) + " - " + Convert.ToString(dr["QUARTER_NO"]) + " | " + Convert.ToString(dr["ALLOTMENT_NO"]),
                             Value = Convert.ToString(dr["ALLOTMENT_NO"])


                         }).ToList();

            return DRP_VALUE;

        }



        //public List<SelectListItem> AllotementNoLOVBind()
        //{

        //    var AllotmentNoLOV = _context.FAllotmentRentDtls
        //                                .Where(x => x.Status == "A")
        //                                .OrderBy(x => x.AllotmentNo)
        //                                .Select(x => new SelectListItem
        //                                {
        //                                    Text = x.AllotmentNo.ToString() + " || " + x.AllotmentNo.ToString() + " || " + x.QuarterCategory + " - " + x.QuarterNo,
        //                                    Value = x.AllotmentNo.ToString()
        //                                })
        //                                .ToList();

        //    // Prepend a default option (optional)
        //    AllotmentNoLOV.Insert(0, new SelectListItem { Text = "Select Allotment No", Value = "" });

        //    return AllotmentNoLOV;
        //}

        public List<FAllotmentRentDtls> GetFilteredAllotmentRentDetails(DateTime? fromDate, DateTime? toDate, int? allotmentNo = null)
        {
            var query = _context.FAllotmentRentDtls.AsQueryable();
            if (allotmentNo.HasValue)
            {
                query = query.Where(d => d.AllotmentNo == allotmentNo.Value);
            }
            else if (fromDate.HasValue && toDate.HasValue)
           
            {
                query = query.Where(d => d.AllotmentDate >= fromDate.Value && d.AllotmentDate <= toDate.Value);
            }

           

            return query.ToList();
        }


        //public List<FAllotmentRentDtls> GetFilteredAllotmentRentDetails(DateTime fromDate, DateTime toDate, int? allotmentNo = null)
        //{
        //    var query = _context.FAllotmentRentDtls
        //        .Where(d => d.AllotmentDate >= fromDate && d.AllotmentDate <= toDate) ;

        //    if (allotmentNo.HasValue)
        //    {
        //        query = query.Where(d => d.AllotmentNo == allotmentNo.Value);
        //    }

        //    return query.ToList();
        //}

        public List<FAllotmentRentDtls> FetchRentCodeFromDatabase(int allotmentno)
        {
            var query = _context.FAllotmentRentDtls
                .Where(d => d.AllotmentNo == allotmentno
                            );


            return query.ToList();
        }










    }
}
