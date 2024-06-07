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

        
        public List<SelectListItem> GetPlantCDLOV(int PersonnelNo)
        {
            //Returns the plant code LOV for employee as per rights in ADM_EMP_UNIT_ACCESS and DESP_ADM_EMP_PLANT_ACCESS_DTLS
            // Used in - DISC01Controller.cs


            string sqlquery = " SELECT A.PLANT_CD PLANT, A.PLANT_CD||' - '||B.PLANT_NAME DESCRIPTION FROM (SELECT B.DESP_UNIT_CD PLANT_CD FROM ADM_EMP_UNIT_ACCESS A, DESP_UNIT_MSTS B WHERE A.UNIT_CODE = B.HRMS_UNIT_CODE AND A.EMPID = '"+PersonnelNo+"' AND PROJECTID = 'DESP' UNION   ";
            sqlquery += " SELECT DISTINCT A.PLANT_CD PLANT_CD FROM DESP_ADM_EMP_PLANT_ACCESS_DTLS A WHERE   PERSONAL_NO = '"+PersonnelNo+"' ) A, M_PLANT B WHERE A.PLANT_CD = B.PLANT_CD ";
            DataTable dtDRP_VALUE = _context.GetSQLQuery(sqlquery);
            List<SelectListItem> DRP_VALUE = new List<SelectListItem>();
            DRP_VALUE = (from DataRow dr in dtDRP_VALUE.Rows
                         select new SelectListItem()
                         {
                             Text = Convert.ToString(dr["DESCRIPTION"]),
                             Value = Convert.ToString(dr["PLANT"])


                         }).ToList();

            return DRP_VALUE;

        }

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


    }
}
