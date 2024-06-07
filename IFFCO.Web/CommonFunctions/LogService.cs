//using IFFCO.NERRS.Web.Models;
//using IFFCO.HRMS.Repository.Pattern.Core.Factories;
//using IFFCO.HRMS.Repository.Pattern.UnitOfWork;
//using IFFCO.HRMS.Shared.Entities;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace IFFCO.NERRS.Web.CommonFunctions
//{
//    public class LogService : BaseModel
//    {
//        private readonly IRepositoryProvider _repositoryProvider = new RepositoryProvider(new RepositoryFactories());    
//        private readonly IUnitOfWorkAsync _unitOfWork;
//        private readonly ModelContext _context = null;
//        private readonly NerrsCommonService nerrsCommonService = null;

//        public LogService(ModelContext context)
//        {
//            //_context = new ModelContext();
//            _context = context;
//            nerrsCommonService = new  NerrsCommonService();
//        }

//        public async Task Log(NerrsGeneralLog nerrsGeneralLog)
//        {
//            _context.NerrsGeneralLog.Add(nerrsGeneralLog);
//            await _context.SaveChangesAsync();
//        }

//       // public async Task<IEnumerable<NerrsGeneralLog>> Get()
//       // {
//       //     var items = _context.NerrsGeneralLog.Where(x => x.LogType.Equals("API"));
//       //     return await items.ToListAsync();
//       // }

//        //private async Task<byte[]> ReadRequestBody(HttpRequest request)
//        //{
//        //    request.EnableRewind();

//        //    var buffer = new byte[Convert.ToInt32(request.ContentLength)];
//        //    await request.Body.ReadAsync(buffer, 0, buffer.Length);
//        //    return buffer;
//        //}

//        //private async Task<byte[]> ReadResponseBody(HttpResponse response)
//        //{


//        //    var buffer = new byte[Convert.ToInt32(response.Body)];
//        //    await response.Body.ReadAsync(buffer, 0, buffer.Length);
//        //    return buffer;
//        //}

//        public async Task SafeLog(string Operation,
//                            string OperationBy,        
//                            int StatusCode,
//                            string Type,
//                            string Method,                            
//                            string path,
//                            string queryString,
//                            string requestBodyContent,
//                            string responseBodyContent)
//            {

//            if (queryString.Length > 100)
//            {
//                queryString = $"(Truncated to 100 chars) {queryString.Substring(0, 100)}";
//            }

//            //if (requestBodyContent == null)
//            //{
//            //    requestBodyContent = Enumerable.Repeat((byte)0x20, 100).ToArray();
//            //}

//            //if (responseBodyContent == null)
//            //{
//            //    responseBodyContent = Enumerable.Repeat((byte)0x20, 100).ToArray();
//            //}

//            nerrsCommonService.DespGeneralLogEntry(Operation, OperationBy, Type, Method, requestBodyContent, responseBodyContent, Convert.ToString(StatusCode), path, queryString);
//        }
//    }
//}
