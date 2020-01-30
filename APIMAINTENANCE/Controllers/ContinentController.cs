using Common;
using Domain;
using Service.Implementations.Maintenance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace APIMAINTENANCE.Controllers
{
    public class ContinentController : ApiController
    {

        private readonly ContinentService service;
        public ContinentController()
        {
            service = new ContinentService();
        }
        [HttpGet]
        public JsonResult<ResponseBase<TBL_SLI_CONTINENT>> List()
        {
            return Json(service.GetList());
        }
        [HttpGet]
        public JsonResult<ResponseBase<TBL_SLI_CONTINENT>> Get(int ID)
        {
            return Json(service.Get(ID));
        }
        //[HttpGet]
        //public JsonResult<ResponseBase<CountryListResponse>> Find(string param)
        //{
        //    return Json(service.Find(param));
        //}
        [HttpPost]
        public JsonResult<ResponseBase<TBL_SLI_CONTINENT>> Insert(TBL_SLI_CONTINENT country)
        {
            return Json(service.Insert(country));
        }
        [HttpPost]
        public JsonResult<ResponseBase<TBL_SLI_CONTINENT>> Update(TBL_SLI_CONTINENT country)
        {
            return Json(service.Update(country));
        }
        [HttpDelete]
        public JsonResult<ResponseBase<TBL_SLI_CONTINENT>> Delete(int ID)
        {
            return Json(service.Delete(ID));
        }
    }
}
