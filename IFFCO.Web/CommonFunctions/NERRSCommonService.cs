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

        public List<VwAonlaConsultantAllotStatus> VwAonlaConsultantDtls()
        {

            string sqlquery = "SELECT UNIT_CODE,ALLOTMENT_NO,QUARTER_FOR,QUARTER_ISSUED_TO,ISSUSE_TO, PERSONAL_NO,NAME,QUARTER_NO,QUARTER_NAME_FOR,APPLICATION_DATE,APPROVED_DATE,QUARTER_CATEGORY,OCCUPANCY_DATE,CON_STATUS,EFFECTIVE_FROM";
                  sqlquery += "EFFECTIVE_TO,VACANCY_DATE,STAY_PERIOD,NO_OF_DAYS,NO_OF_YEARS,STATUS";
                  sqlquery += "from VW_AONLA_CONSULTANT_ALLOT_STATUS order by OCCUPANCY_DATE desc ";
           

            DataTable dtDTL_VALUE = new DataTable();
            dtDTL_VALUE = _context.GetSQLQuery(sqlquery);
            List<VwAonlaConsultantAllotStatus> DTL_VALUE = new List<VwAonlaConsultantAllotStatus>();
            DTL_VALUE = (from DataRow dr in dtDTL_VALUE.Rows
                         select new VwAonlaConsultantAllotStatus()
                         {

                             UnitCode = Convert.ToString(dr["UNIT_CODE"]),
                             AllotmentNo = (dr["ALLOTMENT_NO"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["ALLOTMENT_NO"]),
                             QuarterFor = Convert.ToString(dr["QUARTER_FOR"]),
                            
                             


                         }).ToList();
            return DTL_VALUE;
        }

    }
}
