using IFFCO.HRMS.Entities.AppConfig;
using IFFCO.HRMS.Entities.Models;
using IFFCO.HRMS.Repository.Pattern.Core.Factories;
using IFFCO.HRMS.Repository.Pattern.UnitOfWork;
using IFFCO.NERRS.Web.Models;
using Devart.Data.Oracle;
using IFFCO.NERRS.Web.ViewModels;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;
using Microsoft.CodeAnalysis;
using IFFCO.HRMS.Service;
using NPOI.SS.Formula.Functions;

namespace IFFCO.NERRS.Web.CommonFunctions
{
    public class NERRSCommonService :CommonService
    {
        private readonly IRepositoryProvider _repositoryProvider = new RepositoryProvider(new RepositoryFactories());

        private readonly IUnitOfWorkAsync _unitOfWork;

        readonly string conn = new AppConfiguration().ConnectionString;

        //IDataContextAsync context;
        private readonly IFFCO.NERRS.Web.Models.ModelContext _context;

        DataTable _dt = new DataTable();

        public NERRSCommonService()
        {
            _context = new IFFCO.NERRS.Web.Models.ModelContext();
          
        }

        public List<VwAonlaConsultantAllotStatus> VwAonlaConsultantDtls(string PlantCD)    /*  contracts case */
        {

            //string sqlquery = " SELECT X.UNIT_CODE,X.ALLOTMENT_NO,X.QUARTER_FOR,X.QUARTER_ISSUED_TO,X.ISSUSE_TO, X.PERSONAL_NO,X.NAME,X.QUARTER_NO,X.QUARTER_NAME_FOR,X.APPLICATION_DATE,X.APPROVED_DATE,Z.QUARTER_CATEGORY,X.OCCUPANCY_DATE,X.CON_STATUS,X.EFFECTIVE_FROM ";
            //    sqlquery += " X.EFFECTIVE_TO,X.VACANCY_DATE,X.STAY_PERIOD,X.NO_OF_DAYS,X.NO_OF_YEARS,X.STATUS ";
            //    sqlquery += " Z.QUARTER_CATEGORY, Z.QUARTER_NO, Z.SL_NO, Z.PERSONAL_NO, Z.OCCUPANCY_DATE, Z.VACANCY_DATE, Z.STATUS, Z.CREATED_BY, Z.DATETIME_CREATED, Z.RENT_CODE, Z.OCCUPANT_CODE, Z.MONTH_DAY_TYPE ";
            //      sqlquery += "from VW_AONLA_CONSULTANT_ALLOT_STATUS X LEFT JOIN F_ALLOTMENT_RENT_DTLS Z ON Z.ALLOTMENT_NO = X.ALLOTMENT_NO AND Z.UNIT_CODE = X.UNIT_CODE WHERE X.UNIT_CODE = '"+ PlantCD + "' order by X.OCCUPANCY_DATE desc ";

            string sqlquery = $@"SELECT X.UNIT_CODE, X.ALLOTMENT_NO, X.QUARTER_FOR, X.QUARTER_ISSUED_TO, X.ISSUSE_TO, X.PERSONAL_NO, X.NAME, X.QUARTER_NO, X.QUARTER_NAME_FOR, X.APPLICATION_DATE, X.APPROVED_DATE, 
                                X.QUARTER_CATEGORY,X.OCCUPANCY_DATE,X.CON_STATUS,X.EFFECTIVE_FROM , X.EFFECTIVE_TO, X.VACANCY_DATE, X.STAY_PERIOD, X.NO_OF_DAYS, X.NO_OF_YEARS,
                                Z.QUARTER_CATEGORY, Z.QUARTER_NO, Z.SL_NO, Z.PERSONAL_NO, Z.OCCUPANCY_DATE, Z.VACANCY_DATE, Z.STATUS, Z.CREATED_BY,
                                Z.RENT_CODE, Z.OCCUPANT_CODE
                                FROM VW_AONLA_CONSULTANT_ALLOT_STATUS X 
                                LEFT JOIN F_ALLOTMENT_RENT_DTLS Z ON Z.ALLOTMENT_NO = X.ALLOTMENT_NO AND Z.UNIT_CODE = X.UNIT_CODE 
                                WHERE X.UNIT_CODE = '{PlantCD}' ORDER BY X.OCCUPANCY_DATE DESC";

            DateTime? z = null;
            DataTable dtDTL_VALUE = new DataTable();
            dtDTL_VALUE = _context.GetSQLQuery(sqlquery);
            List<VwAonlaConsultantAllotStatus> DTL_VALUE = new List<VwAonlaConsultantAllotStatus>();
            DTL_VALUE = (from DataRow dr in dtDTL_VALUE.Rows
                         select new VwAonlaConsultantAllotStatus()
                         {

                             UnitCode = Convert.ToString(dr["UNIT_CODE"]),
                             AllotmentNo = (dr["ALLOTMENT_NO"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["ALLOTMENT_NO"]),
                             QuarterFor = Convert.ToString(dr["QUARTER_FOR"]),
                             QuarterIssuedTo = Convert.ToString(dr["QUARTER_ISSUED_TO"]),
                             IssuedTo = Convert.ToString(dr["ISSUSE_TO"]),
                             PersonalNo = Convert.ToString(dr["PERSONAL_NO"]),
                             Name = Convert.ToString(dr["NAME"]),
                             QuarterNo = Convert.ToString(dr["QUARTER_NO"]),
                             QuarterNameFor = Convert.ToString(dr["QUARTER_NAME_FOR"]),
                             ApplicationDate = Convert.ToDateTime(dr["APPLICATION_DATE"]),
                             ApprovedDate = Convert.ToDateTime(dr["APPROVED_DATE"]),
                             QuarterCategory = Convert.ToString(dr["QUARTER_CATEGORY"]),
                             OccupancyDate = Convert.ToDateTime(dr["OCCUPANCY_DATE"]),
                             ConStatus = Convert.ToString(dr["CON_STATUS"]),
                             EffectiveFrom = Convert.ToDateTime(dr["EFFECTIVE_FROM"]),
                             //EffectiveTo = Convert.ToDateTime(dr["EFFECTIVE_TO"]),
                             VacancyDate = string.IsNullOrEmpty(Convert.ToString(dr["VACANCY_DATE"])) ? z : Convert.ToDateTime(Convert.ToString(dr["VACANCY_DATE"])),
                             RentType = Convert.ToString(dr["RENT_CODE"]),
                             OccupantType = Convert.ToString(dr["OCCUPANT_CODE"]),
                             //StayPeriod = Convert.ToString(dr["STAY_PERIOD"]),
                             //NoOfDays = (dr["NO_OF_DAYS"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["NO_OF_DAYS"]),
                             //NoOfYears = (dr["NO_OF_YEARS"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["NO_OF_YEARS"]),
                             //Status = Convert.ToString(dr["Status"]),




                         }).ToList();
            return DTL_VALUE;
        }

        public List<VwAonlaDeathCaseAllotStatus> VwAonlaDeathCaseAllotStatus(string PlantCD)   /*For Death case */
        {

            string sqlquery = " SELECT UNIT_CODE,ALLOTMENT_NO,QUARTER_FOR,QUARTER_ISSUED_TO,ISSUSE_TO, PERSONAL_NO,NAME,QUARTER_NO,QUARTER_NAME_FOR,APPLICATION_DATE,APPROVED_DATE,QUARTER_CATEGORY,OCCUPANCY_DATE,EFFECTIVE_FROM,";
            sqlquery += " EFFECTIVE_TO,VACANCY_DATE,STAY_PERIOD,NO_OF_DAYS,NO_OF_YEARS,STATUS ";
            sqlquery += " from VW_AONLA_DEATH_CASE_ALLOT_STATUS where UNIT_CODE = ' " + PlantCD + "' and QUARTER_ISSUED_TO = 'D' order by OCCUPANCY_DATE desc ";


            DataTable dtDTL_VALUE = new DataTable();
            dtDTL_VALUE = _context.GetSQLQuery(sqlquery);
            List<VwAonlaDeathCaseAllotStatus> DTL_VALUE = new List<VwAonlaDeathCaseAllotStatus>();
            DTL_VALUE = (from DataRow dr in dtDTL_VALUE.Rows
                         select new VwAonlaDeathCaseAllotStatus()
                         {

                             UnitCode = Convert.ToString(dr["UNIT_CODE"]),
                             AllotmentNo = (dr["ALLOTMENT_NO"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["ALLOTMENT_NO"]),
                             QuarterFor = Convert.ToString(dr["QUARTER_FOR"]),
                             Name = Convert.ToString(dr["NAME"]),
                             QuarterNo = Convert.ToString(dr["QUARTER_NO"]),
                             QuarterNameFor = Convert.ToString(dr["QUARTER_NAME_FOR"]),
                             PersonalNo = Convert.ToString(dr["PERSONAL_NO"]),

                             //ApplicationDate = Convert.ToDateTime(dr["APPLICATION_DATE"]),
                             //ApprovedDate = Convert.ToDateTime(dr["APPROVED_DATE"]),
                             QuarterCategory = Convert.ToString(dr["QUARTER_CATEGORY"]),
                             //OccupancyDate = Convert.ToDateTime(dr["OCCUPANCY_DATE"]),
                             //EffectiveFrom = Convert.ToDateTime(dr["EFFECTIVE_FROM"]),
                             //EffectiveTo = Convert.ToDateTime(dr["EFFECTIVE_TO"]),
                             //VacancyDate = Convert.ToDateTime(dr["VACANCY_DATE"]),
                             //StayPeriod = Convert.ToString(dr["STAY_PERIOD"]),
                             //NoOfDays = (dr["NO_OF_DAYS"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["NO_OF_DAYS"]),
                             //NoOfYears = (dr["NO_OF_YEARS"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["NO_OF_YEARS"]),
                             //Status = Convert.ToString(dr["Status"]),




                         }).ToList();
            return DTL_VALUE;
        }




        public List<VwAonlaExEmpAllotStatus> VwAonlaExEmpAllotStatusDtls(string PlantCD)    /*For Ex employee - Retired case */
        {

            string sqlquery = " SELECT UNIT_CODE,ALLOTMENT_NO,QUARTER_FOR,QUARTER_ISSUED_TO,ISSUSE_TO, PERSONAL_NO,NAME,QUARTER_NO,QUARTER_NAME_FOR,APPLICATION_DATE,APPROVED_DATE,QUARTER_CATEGORY,OCCUPANCY_DATE,EFFECTIVE_FROM,";
            sqlquery += " EFFECTIVE_TO,VACANCY_DATE,STAY_PERIOD,NO_OF_DAYS,NO_OF_YEARS,STATUS ";
            sqlquery += " from VW_AONLA_EX_EMP_ALLOT_STATUS where UNIT_CODE = ' "+ PlantCD + "' and QUARTER_ISSUED_TO = 'U' order by OCCUPANCY_DATE desc ";

            DateTime? vacay = null;
            DataTable dtDTL_VALUE = new DataTable();
            dtDTL_VALUE = _context.GetSQLQuery(sqlquery);
            List<VwAonlaExEmpAllotStatus> DTL_VALUE = new List<VwAonlaExEmpAllotStatus>();
            DTL_VALUE = (from DataRow dr in dtDTL_VALUE.Rows
                         select new VwAonlaExEmpAllotStatus()
                         {

                             UnitCode = Convert.ToString(dr["UNIT_CODE"]),
                             AllotmentNo = (dr["ALLOTMENT_NO"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["ALLOTMENT_NO"]),
                             QuarterFor = Convert.ToString(dr["QUARTER_FOR"]),
                             Name = Convert.ToString(dr["NAME"]),
                             QuarterNo = Convert.ToString(dr["QUARTER_NO"]),
                             QuarterNameFor = Convert.ToString(dr["QUARTER_NAME_FOR"]),
                             PersonalNo = Convert.ToString(dr["PERSONAL_NO"]),
                            
                             //ApplicationDate = Convert.ToDateTime(dr["APPLICATION_DATE"]),
                             ApprovedDate = Convert.ToDateTime(dr["APPROVED_DATE"]),
                             QuarterCategory = Convert.ToString(dr["QUARTER_CATEGORY"]),
                             OccupancyDate = Convert.ToDateTime(dr["OCCUPANCY_DATE"]),
                             //EffectiveFrom = Convert.ToDateTime(dr["EFFECTIVE_FROM"]),
                             //EffectiveTo = Convert.ToDateTime(dr["EFFECTIVE_TO"]),
                             //VacancyDate = Convert.ToDateTime(dr["VACANCY_DATE"]),
                             VacancyDate = string.IsNullOrEmpty(Convert.ToString(dr["VACANCY_DATE"])) ? vacay : Convert.ToDateTime(Convert.ToString(dr["VACANCY_DATE"])),
                             
                             //StayPeriod = Convert.ToString(dr["STAY_PERIOD"]),
                             //NoOfDays = (dr["NO_OF_DAYS"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["NO_OF_DAYS"]),
                             //NoOfYears = (dr["NO_OF_YEARS"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["NO_OF_YEARS"]),
                             //Status = Convert.ToString(dr["Status"]),




                         }).ToList();
            return DTL_VALUE;
        }

       



        public List<VwAonlaNonEmpAllotStatus> VwAonlaNonEmpAllotStatus(string PlantCD, string OccupantCode)   /*For Non-Employees */
        {

            string sqlquery = "select a.UNIT_CODE, a.ALLOTMENT_NO,a.QUARTER_FOR,a.ALLOTMENT_TYPE,a.QUARTER_ISSUED_TO,a.ISSUSE_TO,a.QUARTER_NAME_FOR,a.APPLICATION_DATE, a.APPROVED_DATE, a.QUARTER_CATEGORY, a.QUARTER_NO,a.OCCUPANCY_DATE, a.VACANCY_DATE, ";
            sqlquery += " a.STAY_PERIOD, a.NO_OF_DAYS, a.NO_OF_YEARS, a.STATUS ";
            sqlquery += " from VW_AONLA_NON_EMP_ALLOT_STATUS a  where UNIT_CODE = '"+ PlantCD + "' and QUARTER_ISSUED_TO = '" + OccupantCode + "' order by OCCUPANCY_DATE desc ";


            DataTable dtDTL_VALUE = new DataTable();
            dtDTL_VALUE = _context.GetSQLQuery(sqlquery);
            List<VwAonlaNonEmpAllotStatus> DTL_VALUE = new List<VwAonlaNonEmpAllotStatus>();
            DTL_VALUE = (from DataRow dr in dtDTL_VALUE.Rows
                         select new VwAonlaNonEmpAllotStatus()
                         {

                             UnitCode = Convert.ToString(dr["UNIT_CODE"]),
                             AllotmentNo = (dr["ALLOTMENT_NO"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["ALLOTMENT_NO"]),
                             QuarterFor = Convert.ToString(dr["QUARTER_FOR"]),
                             QuarterNo = Convert.ToString(dr["QUARTER_NO"]),
                             QuarterNameFor = Convert.ToString(dr["QUARTER_NAME_FOR"]),
                             //ApplicationDate = Convert.ToDateTime(dr["APPLICATION_DATE"]),
                             //ApprovedDate = Convert.ToDateTime(dr["APPROVED_DATE"]),
                             QuarterCategory = Convert.ToString(dr["QUARTER_CATEGORY"]),
                             // OccupancyDate = Convert.ToDateTime(dr["OCCUPANCY_DATE"]),
                             // EffectiveFrom = Convert.ToDateTime(dr["EFFECTIVE_FROM"]),
                             //EffectiveTo = Convert.ToDateTime(dr["EFFECTIVE_TO"]),
                            // VacancyDate = Convert.ToDateTime(dr["VACANCY_DATE"]),
                             //StayPeriod = Convert.ToString(dr["STAY_PERIOD"]),
                             //NoOfDays = (dr["NO_OF_DAYS"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["NO_OF_DAYS"]),
                             //NoOfYears = (dr["NO_OF_YEARS"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["NO_OF_YEARS"]),
                             Status = Convert.ToString(dr["Status"]),




                         }).ToList();
            return DTL_VALUE;
        }


    }
}
