using IFFCO.NERRS.Web.Models;
using IFFCO.HRMS.Repository.Pattern.Core.Factories;
using IFFCO.HRMS.Repository.Pattern.UnitOfWork;
using System;
using System.Data;

namespace IFFCO.NERRS.Web.CommonFunctions
{
    public class PrimaryKeyGen
    {
        private readonly IRepositoryProvider _repositoryProvider = new RepositoryProvider(new RepositoryFactories());

        private readonly IUnitOfWorkAsync _unitOfWork;

        //IDataContextAsync context;
        private readonly ModelContext _context;
        DataTable _dt = new DataTable();
        public PrimaryKeyGen()
        {
            _context = new ModelContext();
        }

      //  public string DiNoGen(string PlantCd)
      //  {
      //      string sqlquery = " SELECT P.PKGEN('D_DI_HDR', 'RD_NO', 'A', substr('"+PlantCd+"', 1, 2) || TO_CHAR(FINYR_STDT(SYSDATE), 'YY')) FROM DUAL ";
      //      string dtDRP_VALUE = _context.GetCharScalerFromDB(sqlquery);
      //      return (dtDRP_VALUE);
      //  }

      //  public string ClaimNoGen(string PlantCd, string ClaimType)
      //  {

      //      string sqlquery = " SELECT P.PKGEN('L_CLAIMS', 'CLAIM_NO', 'A', substr('" + PlantCd + "', 1, 2) || TO_CHAR(FINYR_STDT(SYSDATE), 'YY')||'" + ClaimType + "') FROM DUAL ";
      //      string dtDRP_VALUE = _context.GetCharScalerFromDB(sqlquery);
      //      return (dtDRP_VALUE);
      //  }

      //  public string SeaDiNoGen(string PlantCd)
      //  {
      //      string sqlquery = " SELECT P.PKGEN('S_DI_HDR', 'S_DI_NO', 'A', substr('" + PlantCd + "', 1, 2) || TO_CHAR(FINYR_STDT(SYSDATE), 'YY')) FROM DUAL ";
      //      string dtDRP_VALUE = _context.GetCharScalerFromDB(sqlquery);
      //      return (dtDRP_VALUE);
      //  }

 	    //public string RailIndentNoGen(string PlantCd)
      //  {
      //      string sqlquery = " SELECT P.PKGEN('L_RAKE_INDENT_HDR', 'INDENT_NO', 'A', substr('" + PlantCd + "', 1, 2) || SUBSTR(TO_CHAR(FINYR_STDT(SYSDATE),'YYYYMMDD'),3,2)) FROM DUAL ";
      //      string dtDRP_VALUE = _context.GetCharScalerFromDB(sqlquery);
      //      return (dtDRP_VALUE);
      //  }

      //  public string RailDiNoGen(string PlantCd)
      //  {
      //      string sqlquery = " SELECT P.PKGEN('L_DI_HDR', 'DI_NO', 'A', substr('" + PlantCd + "', 1, 2) || TO_CHAR(FINYR_STDT(SYSDATE), 'YY')) FROM DUAL ";
      //      string dtDRP_VALUE = _context.GetCharScalerFromDB(sqlquery);
      //      return (dtDRP_VALUE);
      //  }

      //  public string SeaTrfNoGen(string PlantCd)
      //  {
      //      string sqlquery = " SELECT P.PKGEN('S_STOCKTRF', 'SEA_TRF_NO', 'A', SUBSTR('" + PlantCd + "',1,2)||TO_CHAR(FINYR_STDT(SYSDATE), 'YY')) FROM DUAL";
      //      string dtDRP_VALUE = _context.GetCharScalerFromDB(sqlquery);
      //      return (dtDRP_VALUE);
      //  }

      //  public string SeaExGpNoGen(string PlantCd)
      //  {
      //      string sqlquery = " SELECT EX_FYR(SYSDATE)||'/'||LPAD((TO_NUMBER(NVl(SUBSTR(MAX(A.EX_GP_NO), 6,5), 0)) + 1), 5, '0') ";
      //      sqlquery += "  FROM V_EXCISE_GP_DA A WHERE (A.PLANT_CD = '"+PlantCd+"') AND (A.EX_GP_NO LIKE EX_FYR(SYSDATE)||'%') "; 
      //      string dtDRP_VALUE = _context.GetCharScalerFromDB(sqlquery);
      //      return (dtDRP_VALUE);
      //  }

      //  public string ReportTypeCheck(string id,string mode)
      //  {
      //      string sqlquery = "  SELECT DESP.REPORT_CALL_INV_CHL('"+id+"','"+mode+"')  FROM DUAL ";
      //      string dtDRP_VALUE = _context.GetCharScalerFromDB(sqlquery);
      //      return (dtDRP_VALUE);
      //  }

      //  public string ReportTypeCheckWithoutGSTNO(string id, string mode)
      //  {
      //      string sqlquery = "  SELECT DESP.REPORT_CALL_INV_CHL_2('" + id + "','" + mode + "')  FROM DUAL ";
      //      string dtDRP_VALUE = _context.GetCharScalerFromDB(sqlquery);
      //      return (dtDRP_VALUE);
      //  }

      //  public string RoadDistSeqGen(string PlantCd)
      //  {
      //      string sqlquery = " SELECT P.PKGEN('M_PLANT_WH_DIST', 'DIST_SEQ', 'A', SUBSTR('" + PlantCd + "',1,2)) FROM DUAL";
      //      //P.PKGEN('M_PLANT_WH_DIST', 'DIST_SEQ', 'A', SUBSTR(:CTRL_BLK.PLANT_CD, 1, 2));
      //      string dtDRP_VALUE = _context.GetCharScalerFromDB(sqlquery);
      //      return (dtDRP_VALUE);
      //  }

      //  public string RailDistSeqGen(string PlantCd)
      //  {
      //      string sqlquery = " SELECT P.PKGEN('M_PLANT_RKPT_DIST', 'DIST_SEQ', 'A', SUBSTR('" + PlantCd + "',1,2)) FROM DUAL";
      //      //P.PKGEN('M_PLANT_WH_DIST', 'DIST_SEQ', 'A', SUBSTR(:CTRL_BLK.PLANT_CD, 1, 2));
      //      string dtDRP_VALUE = _context.GetCharScalerFromDB(sqlquery);
      //      return (dtDRP_VALUE);
      //  }
      //  public string RailRateSeqGen(string PlantCd)
      //  {
      //      string sqlquery = " SELECT P.PKGEN('M_DEM_RATE_SLABS', 'RT_SEQ', 'A', SUBSTR('" + PlantCd + "',1,2)) FROM DUAL";
      //      //P.PKGEN('M_PLANT_WH_DIST', 'DIST_SEQ', 'A', SUBSTR(:CTRL_BLK.PLANT_CD, 1, 2));
      //      string dtDRP_VALUE = _context.GetCharScalerFromDB(sqlquery);
      //      return (dtDRP_VALUE);
      //  }
      //  public string RailFreeHourSeqGen(string PlantCd)
      //  {
      //      string sqlquery = " SELECT P.PKGEN('M_DEM_FREE_HOURS', 'FH_SEQ', 'A', SUBSTR('" + PlantCd + "',1,2)) FROM DUAL";
      //      //P.PKGEN('M_PLANT_WH_DIST', 'DIST_SEQ', 'A', SUBSTR(:CTRL_BLK.PLANT_CD, 1, 2));
      //      string dtDRP_VALUE = _context.GetCharScalerFromDB(sqlquery);
      //      return (dtDRP_VALUE);
      //  }
      //  public string RailFrghtSeqGen(string PlantCd)
      //  {
      //      string sqlquery = " SELECT P.PKGEN('M_RLY_FRGHT', 'FRGHT_CD', 'A', SUBSTR('" + PlantCd + "',1,2)) FROM DUAL";
      //      //P.PKGEN('M_PLANT_WH_DIST', 'DIST_SEQ', 'A', SUBSTR(:CTRL_BLK.PLANT_CD, 1, 2));
      //      string dtDRP_VALUE = _context.GetCharScalerFromDB(sqlquery);
      //      return (dtDRP_VALUE);
      //  }
      //  public string RailLdgFactortSeqGen(string PlantCd)
      //  {
      //      string sqlquery = " SELECT P.PKGEN('M_LDG_FACTOR', 'LDG_CD', 'A', SUBSTR('" + PlantCd + "',1,2)) FROM DUAL";
      //      //P.PKGEN('M_PLANT_WH_DIST', 'DIST_SEQ', 'A', SUBSTR(:CTRL_BLK.PLANT_CD, 1, 2));
      //      string dtDRP_VALUE = _context.GetCharScalerFromDB(sqlquery);
      //      return (dtDRP_VALUE);
      //  }
      //  public string RoadMdaEchitSeqGen(string PlantCd)
      //  {
      //      string sqlquery = " SELECT P.PKGEN('D_ENTRY_CHIT', 'ECHIT_SEQ', 'A', SUBSTR('" + PlantCd + "',1,2)||TO_CHAR(FINYR_STDT(SYSDATE), 'YY')) FROM DUAL";
      //      string dtDRP_VALUE = _context.GetCharScalerFromDB(sqlquery);
      //      return (dtDRP_VALUE);
      //  } 

      //  public string RoadWOSeqGen(string PlantCd)
      //  {
      //      string sqlquery = " SELECT P.PKGEN('M_WORK_ORDER', 'WO_SEQ', 'A', substr('"+ PlantCd + "',1,2))  FROM DUAL ";
      //      string dtDRP_VALUE = _context.GetCharScalerFromDB(sqlquery);
      //      return (dtDRP_VALUE);
      //  }

      //   public string RailRakeSeqGen(string PlantCd)
      //  {
      //      string sqlquery = " SELECT P.PKGEN('L_RAKE_HDR', 'RAKE_NO', 'A', SUBSTR('" + PlantCd + "',1,2)||TO_CHAR(FINYR_STDT(SYSDATE), 'YY')) FROM DUAL";
      //      string dtDRP_VALUE = _context.GetCharScalerFromDB(sqlquery);
      //      return (dtDRP_VALUE);
      //  }

      //  public string RailLoNoGen(string PlantCd)
      //  {
      //      string sqlquery = " SELECT P.PKGEN('L_LO', 'LO_NO', 'A', SUBSTR('" + PlantCd + "',1,2)||SUBSTR(TO_CHAR(FINYR_STDT(SYSDATE),'DD/MM/RRRR'),9,2)) FROM DUAL";
      //      string dtDRP_VALUE = _context.GetCharScalerFromDB(sqlquery);
      //      return (dtDRP_VALUE);
      //  }
       

      //  public string RailDaNoGen(string PlantCd, string StateCd)
      //  {
      //      string sqlquery = " SELECT P.PKGEN('L_DA', 'DA_NO', 'A', SUBSTR('" + PlantCd + "',1,2)||SUBSTR(TO_CHAR(FINYR_STDT(SYSDATE),'DD/MM/RRRR'),9,2)||'" + StateCd+"') FROM DUAL";
      //      string dtDRP_VALUE = _context.GetCharScalerFromDB(sqlquery);
      //      return (dtDRP_VALUE);
      //  }

      //  public long GeneralLogSeqNoGen()
      //  {
      //      string sqlquery = "  select TO_NUMBER(to_char(add_months(sysdate,-3),'YY')||to_char(add_months(sysdate,9),'YY')||LPAD(TO_CHAR( nvl( max(TO_NUMBER(SUBSTR(TO_CHAR(A.SEQ_NO),5,11))),0)+1),11,0)) from DESP_GENERAL_LOG A WHERE SUBSTR(TO_CHAR(A.SEQ_NO),1,4) LIKE to_char(add_months(sysdate,-3),'YY')||to_char(add_months(sysdate,9),'YY'); ";
      //      string dtDRP_VALUE = _context.GetCharScalerFromDB(sqlquery);
      //      return (Convert.ToInt64(dtDRP_VALUE));
      //  }
    }
}
