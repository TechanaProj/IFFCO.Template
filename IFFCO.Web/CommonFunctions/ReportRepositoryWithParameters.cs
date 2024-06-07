using IFFCO.NERRS.Web.Models;
using IFFCO.HRMS.Repository.Pattern.Core.Factories;
using IFFCO.HRMS.Repository.Pattern.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System;

namespace IFFCO.NERRS.Web.CommonFunctions
{
    public class ReportRepositoryWithParameters
    {
        private readonly IRepositoryProvider _repositoryProvider = new RepositoryProvider(new RepositoryFactories());

        private readonly IUnitOfWorkAsync _unitOfWork;

        private readonly ModelContext _context;

        //private readonly string reportURL = "https://hoapps.iffco.coop/reports/rwservlet?cmdkey=desp_rep2"; //PRODUCTION
        private readonly string reportURL = "https://hoapp.iffco.coop/reports/rwservlet?cmdkey=desp_rep2"; //OCI
        //private readonly string reportURL = "https://hoapps.iffco.coop/reports/rwservlet?cmdkey=desp_dbdev"; //DBDEV
        //private readonly string reportURL = "http://10.12.1.57/reports/rwservlet?cmdkey=desp_dev"; //DEVELOPMENT


        //private readonly string reportURLFrame = "http://";
        //private readonly string reportURLFrame2 = "reports/rwservlet?cmdkey=desp_dbdev"; //DBDEV
        private readonly string reportURLFrame2 = "reports/rwservlet?cmdkey=desp_rep2"; //PRODUCTION
        //private readonly string reportURLFrame2 = "reports/rwservlet?cmdkey=desp_dev"; //DEVELOPMENT

       
        public ReportRepositoryWithParameters()
        {
            _context = new ModelContext();
        }

        public string GenerateSalaryCardReport(string model, string format)
        {

            string report = "";
            //string connstr = "hrmsadm_new";
            //string connstr = "desp_rep2";
            string connstr = "desp_dbdev";
            report = "http://10.12.1.132/reports/rwservlet?module=" + format + "+" + "cmdkey=" + connstr + "+" + model; //PRODCUTION           
            //report = "http://10.12.1.57/reports/rwservlet?module=" + format + "+" + "cmdkey=" + connstr + "+" + model; // DEVELOPEMENT 
            return report;
        }       

        public string GenerateReport(string querystring, string reportname)
        {
           
            string report = "";
            report = reportURL + "+module=" + reportname + "+" + querystring;
            report = "data:application/pdf;base64," + ReportService.EncodeServerName(report);
            return report;
        }

        public string GenerateReport(string querystring, string reportname, string NotEncode)
        {
            string report = "";
            if (NotEncode == "NotEncode")
            {
                report = reportURL + "+module=" + reportname + "+" + querystring;
            }
            else
            {
                report = NotEncode + reportURLFrame2 + "+module=" + reportname + "+" + querystring;
            }

            Encclass.ReportLog(string.Empty, reportname.Split("+")[0], reportname.Split("+")[0], querystring, string.Empty, string.Empty, string.Empty);

            return report;
        }

        public string GenerateReportExcel(string querystring, string reportname)
        {
            string report = "";
            report = reportURL + "+module=" + reportname + "+" + querystring;
            report = "data:application/vnd.ms-excel;base64," + ReportService.EncodeServerName(report);
            return report;
        }

        public string GenerateReportExcelWithoutEncode(string querystring, string reportname)
        {
            string report = "";
            report = reportURL + "+module=" + reportname + "+" + querystring;
            //report = "data:application/vnd.ms-excel;base64," + report;

            Encclass.ReportLog(string.Empty, reportname.Split("+")[0], reportname.Split("+")[0], querystring, string.Empty, string.Empty, string.Empty);
            return report;
        }

        public string GenerateReportRdlc(string reportrdlcUrl,  string querystring, string reportname, string module, string name, string personalNo, string fullClientIp, string clientIp)
        {
            string report = "";

            report = reportrdlcUrl + "/" + module + "/" + reportname + "?" + Encclass.GetEncryptedQueryString(querystring.Replace("''", ""));
            //report = reportrdlcUrl + "/"  + reportname + "?" + Encclass.GetEncryptedQueryString(querystring.Replace("''", ""));
            Encclass.ReportLog(module, reportname, name, querystring, personalNo, fullClientIp, clientIp);
            return report;
        }



    }
}
