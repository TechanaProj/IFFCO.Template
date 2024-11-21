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
                                X.QUARTER_CATEGORY,X.OCCUPANCY_DATE,X.CON_STATUS,X.EFFECTIVE_FROM , X.EFFECTIVE_TO, X.STAY_PERIOD, X.NO_OF_DAYS, X.NO_OF_YEARS,
                                 Z.SL_NO, Z.VACANCY_DATE, Z.STATUS, Z.CREATED_BY,Z.RENT_FROM_DATE,Z.RENT_TO_DATE,
                                Z.RENT_CODE, Z.OCCUPANT_CODE
                                FROM VW_AONLA_CONSULTANT_ALLOT_STATUS X 
                                LEFT JOIN F_ALLOTMENT_RENT_DTLS Z ON Z.ALLOTMENT_NO = X.ALLOTMENT_NO AND Z.UNIT_CODE = X.UNIT_CODE 
                                WHERE X.UNIT_CODE = '{PlantCD}' ORDER BY  Z.RENT_FROM_DATE,X.OCCUPANCY_DATE DESC";

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

            string sqlquery = $@"SELECT X.UNIT_CODE, X.ALLOTMENT_NO, X.QUARTER_FOR, X.QUARTER_ISSUED_TO, X.ISSUSE_TO, X.PERSONAL_NO, X.NAME, X.QUARTER_NO, X.QUARTER_NAME_FOR, X.APPLICATION_DATE, X.APPROVED_DATE, X.QUARTER_CATEGORY, X.OCCUPANCY_DATE, X.EFFECTIVE_FROM,
                               X.EFFECTIVE_TO, X.STAY_PERIOD, X.NO_OF_DAYS, X.NO_OF_YEARS, X.STATUS,
                               Z.QUARTER_CATEGORY, Z.QUARTER_NO, Z.SL_NO, Z.PERSONAL_NO, Z.OCCUPANCY_DATE, Z.VACANCY_DATE, Z.STATUS, Z.CREATED_BY, Z.RENT_CODE, Z.OCCUPANT_CODE,Z.RENT_FROM_DATE,Z.RENT_TO_DATE
                               from VW_AONLA_DEATH_CASE_ALLOT_STATUS X 
                               LEFT JOIN F_ALLOTMENT_RENT_DTLS Z ON Z.ALLOTMENT_NO = X.ALLOTMENT_NO AND Z.UNIT_CODE = X.UNIT_CODE
                               WHERE X.UNIT_CODE = '{PlantCD}' ORDER BY  Z.RENT_FROM_DATE,X.OCCUPANCY_DATE DESC";



            // string sqlquery = " SELECT UNIT_CODE,ALLOTMENT_NO,QUARTER_FOR,QUARTER_ISSUED_TO,ISSUSE_TO, PERSONAL_NO,NAME,QUARTER_NO,QUARTER_NAME_FOR,APPLICATION_DATE,APPROVED_DATE,QUARTER_CATEGORY,OCCUPANCY_DATE,EFFECTIVE_FROM,";
            // sqlquery += " EFFECTIVE_TO,VACANCY_DATE,STAY_PERIOD,NO_OF_DAYS,NO_OF_YEARS,STATUS ";
            //  sqlquery += " from VW_AONLA_DEATH_CASE_ALLOT_STATUS where UNIT_CODE = ' " + PlantCD + "' and QUARTER_ISSUED_TO = 'D' order by OCCUPANCY_DATE desc ";

            DateTime? vacay = null;
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
                             ApplicationDate = Convert.ToDateTime(dr["APPLICATION_DATE"]),
                             ApprovedDate = Convert.ToDateTime(dr["APPROVED_DATE"]),
                             QuarterCategory = Convert.ToString(dr["QUARTER_CATEGORY"]),
                             OccupancyDate = Convert.ToDateTime(dr["OCCUPANCY_DATE"]),
                             VacancyDate = string.IsNullOrEmpty(Convert.ToString(dr["VACANCY_DATE"])) ? vacay : Convert.ToDateTime(Convert.ToString(dr["VACANCY_DATE"])),
                             RentType = Convert.ToString(dr["RENT_CODE"]),
                             OccupantType = Convert.ToString(dr["OCCUPANT_CODE"]),
                             RentFromDate = string.IsNullOrEmpty(Convert.ToString(dr["RENT_FROM_DATE"])) ? vacay : Convert.ToDateTime(Convert.ToString(dr["RENT_FROM_DATE"])),
                             RentToDate = string.IsNullOrEmpty(Convert.ToString(dr["RENT_TO_DATE"])) ? vacay : Convert.ToDateTime(Convert.ToString(dr["RENT_TO_DATE"])),
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
            string sqlquery = $@"SELECT X.UNIT_CODE, X.ALLOTMENT_NO, X.QUARTER_FOR, X.QUARTER_ISSUED_TO, X.ISSUSE_TO, X.PERSONAL_NO, X.NAME, X.QUARTER_NO, X.QUARTER_NAME_FOR, X.APPLICATION_DATE, X.APPROVED_DATE, X.QUARTER_CATEGORY, X.OCCUPANCY_DATE, X.EFFECTIVE_FROM,
                                 X.EFFECTIVE_TO,  X.STAY_PERIOD, X.NO_OF_DAYS, X.NO_OF_YEARS, X.STATUS ,
                                 Z.QUARTER_CATEGORY, Z.SL_NO, Z.PERSONAL_NO, Z.OCCUPANCY_DATE, Z.VACANCY_DATE, Z.STATUS, Z.CREATED_BY, Z.RENT_CODE, Z.OCCUPANT_CODE,Z.RENT_FROM_DATE,Z.RENT_TO_DATE,Z.MARKET_HRR_FROM_DATE
                                from VW_AONLA_EX_EMP_ALLOT_STATUS X
                                LEFT JOIN F_ALLOTMENT_RENT_DTLS Z ON Z.ALLOTMENT_NO = X.ALLOTMENT_NO AND Z.UNIT_CODE = X.UNIT_CODE
                                WHERE X.UNIT_CODE = '{PlantCD}' ORDER BY Z.RENT_FROM_DATE,X.OCCUPANCY_DATE DESC";


            //string sqlquery = " SELECT UNIT_CODE,ALLOTMENT_NO,QUARTER_FOR,QUARTER_ISSUED_TO,ISSUSE_TO, PERSONAL_NO,NAME,QUARTER_NO,QUARTER_NAME_FOR,APPLICATION_DATE,APPROVED_DATE,QUARTER_CATEGORY,OCCUPANCY_DATE,EFFECTIVE_FROM,";
            //sqlquery += " EFFECTIVE_TO,VACANCY_DATE,STAY_PERIOD,NO_OF_DAYS,NO_OF_YEARS,STATUS ";
            // sqlquery += " from VW_AONLA_EX_EMP_ALLOT_STATUS where UNIT_CODE = ' "+ PlantCD + "' and QUARTER_ISSUED_TO = 'U' order by OCCUPANCY_DATE desc ";

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
                             MarketHrrFromDate = string.IsNullOrEmpty(Convert.ToString(dr["MARKET_HRR_FROM_DATE"])) ? vacay : Convert.ToDateTime(Convert.ToString(dr["MARKET_HRR_FROM_DATE"])),
                             RentType = Convert.ToString(dr["RENT_CODE"]),
                             OccupantType = Convert.ToString(dr["OCCUPANT_CODE"]),
                            // RentFromDate = Convert.ToDateTime(dr["RENT_FROM_DATE"]),
                            // RentToDate = Convert.ToDateTime(dr["RENT_TO_DATE"]),
                             //StayPeriod = Convert.ToString(dr["STAY_PERIOD"]),
                             //NoOfDays = (dr["NO_OF_DAYS"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["NO_OF_DAYS"]),
                             //NoOfYears = (dr["NO_OF_YEARS"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["NO_OF_YEARS"]),
                             //Status = Convert.ToString(dr["Status"]),




                         }).ToList();
            return DTL_VALUE;
        }

       



        public List<VwAonlaNonEmpAllotStatus> VwAonlaNonEmpAllotStatus(string PlantCD, string OccupantCode)   /*For Non-Employees */
        {
            //string sqlquery = $@"select X.UNIT_CODE, X.ALLOTMENT_NO,X.QUARTER_FOR,X.ALLOTMENT_TYPE,X.QUARTER_ISSUED_TO,X.ISSUSE_TO,X.QUARTER_NAME_FOR,X.APPLICATION_DATE, X.APPROVED_DATE, X.QUARTER_CATEGORY, X.QUARTER_NO,X.OCCUPANCY_DATE, X.VACANCY_DATE, 
            //                  X.STAY_PERIOD, X.NO_OF_DAYS, X.NO_OF_YEARS, X.STATUS,
            //                    Z.QUARTER_CATEGORY, Z.SL_NO, Z.PERSONAL_NO, Z.OCCUPANCY_DATE, Z.VACANCY_DATE, Z.STATUS, Z.CREATED_BY, Z.RENT_CODE, Z.OCCUPANT_CODE , Z.VENDOR_CODE, Z.SL_NO, Z.RENT_FROM_DATE,Z.RENT_TO_DATE  
            //                    from VW_AONLA_NON_EMP_ALLOT_STATUS X  
            //                    LEFT JOIN F_ALLOTMENT_RENT_DTLS Z ON Z.ALLOTMENT_NO = X.ALLOTMENT_NO AND Z.UNIT_CODE = X.UNIT_CODE 
            //                    where X.UNIT_CODE = '{PlantCD}' 
            //                    order by X.OCCUPANCY_DATE desc";

            string sqlquery = $@"select X.UNIT_CODE, X.ALLOTMENT_NO,X.QUARTER_FOR,X.ALLOTMENT_TYPE,X.QUARTER_ISSUED_TO,X.ISSUSE_TO,X.QUARTER_NAME_FOR,X.APPLICATION_DATE, X.APPROVED_DATE, X.QUARTER_CATEGORY, X.QUARTER_NO,X.OCCUPANCY_DATE, X.VACANCY_DATE, 
                              X.STAY_PERIOD, X.NO_OF_DAYS, X.NO_OF_YEARS, X.STATUS,
                                Z.QUARTER_CATEGORY, Z.SL_NO, Z.PERSONAL_NO, Z.OCCUPANCY_DATE, Z.VACANCY_DATE, Z.STATUS, Z.CREATED_BY, Z.RENT_CODE, Z.OCCUPANT_CODE , Z.VENDOR_CODE, Z.SL_NO, Z.RENT_FROM_DATE,Z.RENT_TO_DATE  
                                from VW_AONLA_NON_EMP_ALLOT_STATUS X  
                                LEFT JOIN F_ALLOTMENT_RENT_DTLS Z ON Z.ALLOTMENT_NO = X.ALLOTMENT_NO AND Z.UNIT_CODE = X.UNIT_CODE 
                                where X.UNIT_CODE = '{PlantCD}' and X.QUARTER_ISSUED_TO = '{OccupantCode}'
                                order by  Z.RENT_FROM_DATE,X.OCCUPANCY_DATE DESC";

            // string sqlquery = "select a.UNIT_CODE, a.ALLOTMENT_NO,a.QUARTER_FOR,a.ALLOTMENT_TYPE,a.QUARTER_ISSUED_TO,a.ISSUSE_TO,a.QUARTER_NAME_FOR,a.APPLICATION_DATE, a.APPROVED_DATE, a.QUARTER_CATEGORY, a.QUARTER_NO,a.OCCUPANCY_DATE, a.VACANCY_DATE, ";
            // sqlquery += " a.STAY_PERIOD, a.NO_OF_DAYS, a.NO_OF_YEARS, a.STATUS ";
            // sqlquery += " from VW_AONLA_NON_EMP_ALLOT_STATUS a  where UNIT_CODE = '"+ PlantCD + "' and QUARTER_ISSUED_TO = '" + OccupantCode + "' order by OCCUPANCY_DATE desc ";

            DateTime? vacay = null;
            DataTable dtDTL_VALUE = new DataTable();
            dtDTL_VALUE = _context.GetSQLQuery(sqlquery);
            List<VwAonlaNonEmpAllotStatus> DTL_VALUE = new List<VwAonlaNonEmpAllotStatus>();
            DTL_VALUE = (from DataRow dr in dtDTL_VALUE.Rows
                         select new VwAonlaNonEmpAllotStatus()
                         {
                             UnitCode = Convert.ToString(dr["UNIT_CODE"]),
                             AllotmentNo = dr["ALLOTMENT_NO"] != DBNull.Value ? Convert.ToInt32(dr["ALLOTMENT_NO"]) : 0,
                             QuarterFor = Convert.ToString(dr["QUARTER_FOR"]),
                             QuarterNo = Convert.ToString(dr["QUARTER_NO"]),
                             QuarterNameFor = Convert.ToString(dr["QUARTER_NAME_FOR"]),

                             ApprovedDate = dr["APPROVED_DATE"] != DBNull.Value ? Convert.ToDateTime(dr["APPROVED_DATE"]) : (DateTime?)null,
                             QuarterCategory = Convert.ToString(dr["QUARTER_CATEGORY"]),

                             OccupancyDate = !string.IsNullOrEmpty(Convert.ToString(dr["OCCUPANCY_DATE"])) ?
                                             Convert.ToDateTime(dr["OCCUPANCY_DATE"]) : vacay,

                             VacancyDate = !string.IsNullOrEmpty(Convert.ToString(dr["VACANCY_DATE"])) ?
                                           Convert.ToDateTime(dr["VACANCY_DATE"]) : vacay,

                             RentType = Convert.ToString(dr["RENT_CODE"]),
                             VendorCode = Convert.ToString(dr["VENDOR_CODE"]),
                             SlNo = dr["SL_NO"] != DBNull.Value ? Convert.ToInt32(dr["SL_NO"]) : 0,

                             RentFromDate = !string.IsNullOrEmpty(Convert.ToString(dr["RENT_FROM_DATE"])) ?
                                            Convert.ToDateTime(dr["RENT_FROM_DATE"]) : vacay,

                             RentToDate = !string.IsNullOrEmpty(Convert.ToString(dr["RENT_TO_DATE"])) ?
                                          Convert.ToDateTime(dr["RENT_TO_DATE"]) : vacay,

                             OccupantType = Convert.ToString(dr["OCCUPANT_CODE"]),
                         }).ToList();

            //DTL_VALUE = (from DataRow dr in dtDTL_VALUE.Rows
            //             select new VwAonlaNonEmpAllotStatus()
            //             {

            //                 UnitCode = Convert.ToString(dr["UNIT_CODE"]),
            //                 AllotmentNo = (dr["ALLOTMENT_NO"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["ALLOTMENT_NO"]),
            //                 QuarterFor = Convert.ToString(dr["QUARTER_FOR"]),
            //                 QuarterNo = Convert.ToString(dr["QUARTER_NO"]),
            //                 QuarterNameFor = Convert.ToString(dr["QUARTER_NAME_FOR"]),
            //                 //ApplicationDate = Convert.ToDateTime(dr["APPLICATION_DATE"]),
            //                 ApprovedDate = Convert.ToDateTime(dr["APPROVED_DATE"]),
            //                 QuarterCategory = Convert.ToString(dr["QUARTER_CATEGORY"]),
            //                 //OccupancyDate = Convert.ToDateTime(dr["OCCUPANCY_DATE"]),
            //                 OccupancyDate = string.IsNullOrEmpty(Convert.ToString(dr["OCCUPANCY_DATE"])) ? vacay : Convert.ToDateTime(Convert.ToString(dr["OCCUPANCY_DATE"])),

            //                 // EffectiveFrom = Convert.ToDateTime(dr["EFFECTIVE_FROM"]),
            //                 //EffectiveTo = Convert.ToDateTime(dr["EFFECTIVE_TO"]),
            //                 //VacancyDate = Convert.ToDateTime(dr["VACANCY_DATE"]),
            //                 VacancyDate = string.IsNullOrEmpty(Convert.ToString(dr["VACANCY_DATE"])) ? vacay : Convert.ToDateTime(Convert.ToString(dr["VACANCY_DATE"])),
            //                 RentType = Convert.ToString(dr["RENT_CODE"]),
            //                 VendorCode = Convert.ToString(dr["VENDOR_CODE"]),
            //                 SlNo = Convert.ToInt32(dr["SL_NO"]),
            //                 RentFromDate = string.IsNullOrEmpty(Convert.ToString(dr["RENT_FROM_DATE"])) ? vacay : Convert.ToDateTime(Convert.ToString(dr["RENT_FROM_DATE"])),
            //                 RentToDate = string.IsNullOrEmpty(Convert.ToString(dr["RENT_TO_DATE"])) ? vacay : Convert.ToDateTime(Convert.ToString(dr["RENT_TO_DATE"])),
            //                 OccupantType = Convert.ToString(dr["OCCUPANT_CODE"]),
            //                 //StayPeriod = Convert.ToString(dr["STAY_PERIOD"]),
            //                 //NoOfDays = (dr["NO_OF_DAYS"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["NO_OF_DAYS"]),
            //                 //NoOfYears = (dr["NO_OF_YEARS"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["NO_OF_YEARS"]),
            //                 // Status = Convert.ToString(dr["Status"]),
            //             }).ToList();
            return DTL_VALUE;
        }



        //public List<VwAonlaNonEmpAllotStatus> VwAonlaNonEmpShutDown(string PlantCD, string OccupantCode, string QuarterCode)   /*For ShutDown  */
        public List<VwAonlaNonEmpAllotStatus> VwAonlaNonEmpShutDown(string PlantCD, string OccupantCode)   /*For ShutDown , string QuarterCode */
        {
            // Check if PlantCD and OccupantCode are provided
            if (string.IsNullOrEmpty(PlantCD) || string.IsNullOrEmpty(OccupantCode))
            {
                
                return new List<VwAonlaNonEmpAllotStatus>();
            }

            // Split the combined value into separate components
            var parts = OccupantCode.Split('-');
            if (parts.Length < 2)
            {
                // Return an empty list or handle invalid input format
                return new List<VwAonlaNonEmpAllotStatus>();
            }

            // Trim each part to remove any surrounding whitespace
            string QuarterCategory = parts[0].Trim(); // e.g., "A0"
            string QuarterCode = parts[1].Trim();      // e.g., "4"

           

            string sqlquery = $@"select X.UNIT_CODE, X.ALLOTMENT_NO,X.QUARTER_FOR,X.ALLOTMENT_TYPE,X.QUARTER_ISSUED_TO,X.ISSUSE_TO,X.QUARTER_NAME_FOR,X.APPLICATION_DATE, X.APPROVED_DATE, X.QUARTER_CATEGORY, X.QUARTER_NO,X.OCCUPANCY_DATE, Z.VACANCY_DATE, 
                              X.STAY_PERIOD, X.NO_OF_DAYS, X.NO_OF_YEARS, X.STATUS,
                                Z.QUARTER_CATEGORY, Z.SL_NO, Z.PERSONAL_NO, Z.OCCUPANCY_DATE, Z.VACANCY_DATE, Z.STATUS, Z.CREATED_BY, Z.RENT_CODE, Z.OCCUPANT_CODE, Z.RENT_FROM_DATE,Z.RENT_TO_DATE,Z.VENDOR_CODE,Z.RENT_FROM_DATE
                                from VW_AONLA_NON_EMP_ALLOT_STATUS X  
                                LEFT JOIN F_ALLOTMENT_RENT_DTLS Z ON Z.ALLOTMENT_NO = X.ALLOTMENT_NO AND Z.UNIT_CODE = X.UNIT_CODE 
                              where X.UNIT_CODE = '{PlantCD}'  and X.QUARTER_CATEGORY ='{QuarterCategory}' and X.QUARTER_NO = '{QuarterCode}' and X.QUARTER_ISSUED_TO = 'A'
                               order by Z.SL_NO";

            // string sqlquery = "select a.UNIT_CODE, a.ALLOTMENT_NO,a.QUARTER_FOR,a.ALLOTMENT_TYPE,a.QUARTER_ISSUED_TO,a.ISSUSE_TO,a.QUARTER_NAME_FOR,a.APPLICATION_DATE, a.APPROVED_DATE, a.QUARTER_CATEGORY, a.QUARTER_NO,a.OCCUPANCY_DATE, a.VACANCY_DATE, ";
            // sqlquery += " a.STAY_PERIOD, a.NO_OF_DAYS, a.NO_OF_YEARS, a.STATUS ";   where X.UNIT_CODE = '{PlantCD}'  and X.QUARTER_CATEGORY ='{OccupantCode}' and X.QUARTER_NO = '{QuarterCode}' and and X.QUARTER_ISSUED_TO = 'A'
            // sqlquery += " from VW_AONLA_NON_EMP_ALLOT_STATUS a  where UNIT_CODE = '"+ PlantCD + "' and QUARTER_ISSUED_TO = '" + OccupantCode + "' order by OCCUPANCY_DATE desc ";

            DateTime? vacay = null;
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
                             ApprovedDate = Convert.ToDateTime(dr["APPROVED_DATE"]),
                             QuarterCategory = Convert.ToString(dr["QUARTER_CATEGORY"]),
                             //OccupancyDate = Convert.ToDateTime(dr["OCCUPANCY_DATE"]),
                             OccupancyDate = string.IsNullOrEmpty(Convert.ToString(dr["OCCUPANCY_DATE"])) ? vacay : Convert.ToDateTime(Convert.ToString(dr["OCCUPANCY_DATE"])),
                             SlNo = (dr["SL_NO"] == DBNull.Value) ? 1 : Convert.ToInt32(dr["SL_NO"]),
                             // EffectiveFrom = Convert.ToDateTime(dr["EFFECTIVE_FROM"]),
                             //EffectiveTo = Convert.ToDateTime(dr["EFFECTIVE_TO"]),
                             //VacancyDate = Convert.ToDateTime(dr["VACANCY_DATE"]),
                             VacancyDate = string.IsNullOrEmpty(Convert.ToString(dr["VACANCY_DATE"])) ? vacay : Convert.ToDateTime(Convert.ToString(dr["VACANCY_DATE"])),
                             RentType = Convert.ToString(dr["RENT_CODE"]),
                             OccupantType = Convert.ToString(dr["OCCUPANT_CODE"]),
                             RentFromDate = string.IsNullOrEmpty(Convert.ToString(dr["RENT_FROM_DATE"])) ? vacay : Convert.ToDateTime(Convert.ToString(dr["RENT_FROM_DATE"])),
                             RentToDate = string.IsNullOrEmpty(Convert.ToString(dr["RENT_TO_DATE"])) ? vacay : Convert.ToDateTime(Convert.ToString(dr["RENT_TO_DATE"])),
                             //StayPeriod = Convert.ToString(dr["STAY_PERIOD"]),
                             //NoOfDays = (dr["NO_OF_DAYS"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["NO_OF_DAYS"]),
                             //NoOfYears = (dr["NO_OF_YEARS"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["NO_OF_YEARS"]),
                             // Status = Convert.ToString(dr["Status"]),
                             VendorCode = Convert.ToString(dr["VENDOR_CODE"])




                         }).ToList();
            return DTL_VALUE;
        }

        public List<FIntCompute> FinalIntCompute(string PlantCD, DateTime FromDate, DateTime ToDate)   /*For Final computation details NERSC05*/
        {
            string sqlquery = "SELECT ";
            sqlquery += "A.UNIT_CODE, A.QUARTER_CATEGORY, A.QUARTER_NO, A.SL_NO, EMP.EMP_NAME, A.ALLOTMENT_NO, ";
            sqlquery += "A.ALLOTMENT_DATE, A.OCCUPANCY_DATE, A.VACANCY_DATE, A.TOTAL_AMT, R.TYPE_RESI_ACCOM,R.RATES, ";
            sqlquery += "A.VENDOR_CODE, MAX(V.VENDOR_NAME) AS VENDOR_NAME, O.OCCUPANT_TYPE, A.DAYS_REMAINING, ";
            sqlquery += "A.COMPUTATION_RUN, A.FROM_DATE, A.TO_DATE, A.NXT_FROM_DATE, A.NXT_TO_DATE, ";
            sqlquery += "A.MONTH_DAY_TYPE, A.ELECT_RATE, A.ELECT_UNIT, A.ELECT_AMT, A.CURRENT_COMPUTE_AMOUNT, ";
            sqlquery += "A.DAYS_COMPUTED, A.RENT_RATE ";
            sqlquery += "FROM F_INT_COMPUTE A ";
            sqlquery += "LEFT JOIN M_OCCUPANT_MSTS O ON A.OCCUPANT_CODE = O.OCCUPANT_CODE ";
            sqlquery += "LEFT JOIN M_RENT_MSTS R ON A.RENT_CODE = R.RENT_CODE ";
            sqlquery += "LEFT JOIN V_EB_EMPLOYEE_COMPLETE_DTLS EMP ON A.PERSONAL_NO = EMP.PERSONAL_NO ";
            sqlquery += "LEFT JOIN M_VENDOR_MSTS V ON A.VENDOR_CODE = V.VENDOR_CODE ";
            sqlquery += "AND A.UNIT_CODE = '" + PlantCD + "' AND A.FROM_DATE =  '" + FromDate.ToString("dd/MMM/yyyy") + "' AND A.TO_DATE =  '" + ToDate.ToString("dd/MMM/yyyy") + "'  ";
            sqlquery += "GROUP BY ";
            sqlquery += "A.UNIT_CODE, A.QUARTER_CATEGORY, A.QUARTER_NO, A.SL_NO, EMP.EMP_NAME, A.ALLOTMENT_NO, ";
            sqlquery += "A.ALLOTMENT_DATE, A.OCCUPANCY_DATE, A.VACANCY_DATE, A.TOTAL_AMT, R.TYPE_RESI_ACCOM,R.RATES, ";
            sqlquery += "A.VENDOR_CODE, O.OCCUPANT_TYPE, A.DAYS_REMAINING, A.COMPUTATION_RUN, A.FROM_DATE, ";
            sqlquery += "A.TO_DATE, A.NXT_FROM_DATE, A.NXT_TO_DATE, A.MONTH_DAY_TYPE, A.ELECT_RATE, ";
            sqlquery += "A.ELECT_UNIT, A.ELECT_AMT, A.CURRENT_COMPUTE_AMOUNT, A.DAYS_COMPUTED, A.RENT_RATE ";
            sqlquery += " ORDER BY A.COMPUTATION_RUN DESC ";



            DateTime? vacay = null;
            DataTable dtDTL_VALUE = new DataTable();
            dtDTL_VALUE = _context.GetSQLQuery(sqlquery);
            List<FIntCompute> DTL_VALUE = new List<FIntCompute>();
            DTL_VALUE = (from DataRow dr in dtDTL_VALUE.Rows
                         select new FIntCompute()
                         {

                             UnitCode = Convert.ToInt32(dr["UNIT_CODE"]),
                             //PersonalNo = Convert.ToInt32(dr["PERSONAL_NO"]),
                             EmpName = Convert.ToString(dr["EMP_NAME"]),
                             VendorCode = Convert.ToString(dr["VENDOR_NAME"]),
                             ComputationRun = Convert.ToInt32(dr["COMPUTATION_RUN"]),
                             AllotmentNo = (dr["ALLOTMENT_NO"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["ALLOTMENT_NO"]),
                             QuarterCategory = Convert.ToString(dr["QUARTER_CATEGORY"]),
                             QuarterNo = Convert.ToInt32(dr["QUARTER_NO"]),
                             AllotmentDate = Convert.ToDateTime(dr["ALLOTMENT_DATE"]),
                             ToDate = Convert.ToDateTime(dr["TO_DATE"]),
                             FromDate = Convert.ToDateTime(dr["FROM_DATE"]),
                         
                             OccupancyDate = string.IsNullOrEmpty(Convert.ToString(dr["OCCUPANCY_DATE"])) ? vacay : Convert.ToDateTime(Convert.ToString(dr["OCCUPANCY_DATE"])),
                             VacancyDate = string.IsNullOrEmpty(Convert.ToString(dr["VACANCY_DATE"])) ? vacay : Convert.ToDateTime(Convert.ToString(dr["VACANCY_DATE"])),
                             RentCode = Convert.ToString(dr["RATES"]),
                             OccupantCode = Convert.ToString(dr["OCCUPANT_TYPE"]),
                             ElectRate = Convert.ToInt32(dr["ELECT_RATE"]),
                             ElectAmt = Convert.ToInt32(dr["ELECT_AMT"]),
                             ElectUnit = Convert.ToInt32(dr["ELECT_UNIT"]),
                             DaysRemaining = Convert.ToInt32(dr["DAYS_REMAINING"]),
                             CurrentComputeAmount = Convert.ToInt32(dr["CURRENT_COMPUTE_AMOUNT"]),

                          

                         }).ToList();
            return DTL_VALUE;
        }


        public List<FIntCompute> FinalIntComputenew(string PlantCD, string AllotmentNo)   /*For Final computation details NERSC05*/
        {
            string sqlquery = "SELECT ";
            sqlquery += "A.UNIT_CODE, A.QUARTER_CATEGORY, A.QUARTER_NO, A.SL_NO, EMP.EMP_NAME, A.ALLOTMENT_NO, ";
            sqlquery += "A.ALLOTMENT_DATE, A.OCCUPANCY_DATE, A.VACANCY_DATE, A.TOTAL_AMT, R.TYPE_RESI_ACCOM,R.RATES, ";
            sqlquery += "A.VENDOR_CODE, MAX(V.VENDOR_NAME) AS VENDOR_NAME, O.OCCUPANT_TYPE, A.DAYS_REMAINING, ";
            sqlquery += "A.COMPUTATION_RUN, A.FROM_DATE, A.TO_DATE, A.NXT_FROM_DATE, A.NXT_TO_DATE, ";
            sqlquery += "A.MONTH_DAY_TYPE, A.ELECT_RATE, A.ELECT_UNIT, A.ELECT_AMT, A.CURRENT_COMPUTE_AMOUNT, ";
            sqlquery += "A.DAYS_COMPUTED, A.RENT_RATE ";
            sqlquery += "FROM F_INT_COMPUTE A ";
            sqlquery += "LEFT JOIN M_OCCUPANT_MSTS O ON A.OCCUPANT_CODE = O.OCCUPANT_CODE ";
            sqlquery += "LEFT JOIN M_RENT_MSTS R ON A.RENT_CODE = R.RENT_CODE ";
            sqlquery += "LEFT JOIN V_EB_EMPLOYEE_COMPLETE_DTLS EMP ON A.PERSONAL_NO = EMP.PERSONAL_NO ";
            sqlquery += "LEFT JOIN M_VENDOR_MSTS V ON A.VENDOR_CODE = V.VENDOR_CODE ";
            sqlquery += "WHERE A.UNIT_CODE = '" + PlantCD + "' AND A.ALLOTMENT_NO =  '" + AllotmentNo + "' ";
            sqlquery += "GROUP BY ";
            sqlquery += "A.UNIT_CODE, A.QUARTER_CATEGORY, A.QUARTER_NO, A.SL_NO, EMP.EMP_NAME, A.ALLOTMENT_NO, ";
            sqlquery += "A.ALLOTMENT_DATE, A.OCCUPANCY_DATE, A.VACANCY_DATE, A.TOTAL_AMT, R.TYPE_RESI_ACCOM,R.RATES, ";
            sqlquery += "A.VENDOR_CODE, O.OCCUPANT_TYPE, A.DAYS_REMAINING, A.COMPUTATION_RUN, A.FROM_DATE, ";
            sqlquery += "A.TO_DATE, A.NXT_FROM_DATE, A.NXT_TO_DATE, A.MONTH_DAY_TYPE, A.ELECT_RATE, ";
            sqlquery += "A.ELECT_UNIT, A.ELECT_AMT, A.CURRENT_COMPUTE_AMOUNT, A.DAYS_COMPUTED, A.RENT_RATE ";
            sqlquery += " ORDER BY A.COMPUTATION_RUN DESC ";



            DateTime? vacay = null;
            DataTable dtDTL_VALUE = new DataTable();
            dtDTL_VALUE = _context.GetSQLQuery(sqlquery);
            List<FIntCompute> DTL_VALUE = new List<FIntCompute>();
            DTL_VALUE = (from DataRow dr in dtDTL_VALUE.Rows
                         select new FIntCompute()
                         {

                             UnitCode = Convert.ToInt32(dr["UNIT_CODE"]),
                             //PersonalNo = Convert.ToInt32(dr["PERSONAL_NO"]),
                             EmpName = Convert.ToString(dr["EMP_NAME"]),
                             VendorCode = Convert.ToString(dr["VENDOR_NAME"]),
                             ComputationRun = Convert.ToInt32(dr["COMPUTATION_RUN"]),
                             AllotmentNo = (dr["ALLOTMENT_NO"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["ALLOTMENT_NO"]),
                             QuarterCategory = Convert.ToString(dr["QUARTER_CATEGORY"]),
                             QuarterNo = Convert.ToInt32(dr["QUARTER_NO"]),
                             AllotmentDate = Convert.ToDateTime(dr["ALLOTMENT_DATE"]),
                             ToDate = Convert.ToDateTime(dr["TO_DATE"]),
                             FromDate = Convert.ToDateTime(dr["FROM_DATE"]),

                             OccupancyDate = string.IsNullOrEmpty(Convert.ToString(dr["OCCUPANCY_DATE"])) ? vacay : Convert.ToDateTime(Convert.ToString(dr["OCCUPANCY_DATE"])),
                             VacancyDate = string.IsNullOrEmpty(Convert.ToString(dr["VACANCY_DATE"])) ? vacay : Convert.ToDateTime(Convert.ToString(dr["VACANCY_DATE"])),
                             RentCode = Convert.ToString(dr["RATES"]),
                             OccupantCode = Convert.ToString(dr["OCCUPANT_TYPE"]),
                             ElectRate = Convert.ToInt32(dr["ELECT_RATE"]),
                             ElectAmt = Convert.ToInt32(dr["ELECT_AMT"]),
                             ElectUnit = Convert.ToInt32(dr["ELECT_UNIT"]),
                             DaysRemaining = Convert.ToInt32(dr["DAYS_REMAINING"]),
                             CurrentComputeAmount = Convert.ToInt32(dr["CURRENT_COMPUTE_AMOUNT"]),



                         }).ToList();
            return DTL_VALUE;
        }

    }
}
