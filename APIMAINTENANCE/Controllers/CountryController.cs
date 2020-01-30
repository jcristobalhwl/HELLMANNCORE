using Common;
using Domain;
using Model.Response;
using Service.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace APIMAINTENANCE.Controllers
{
    public class CountryController : ApiController
    {
        private readonly CountryService service;
        public CountryController()
        {
            service = new CountryService();
        }
        [HttpGet]
        public JsonResult<ResponseBase<CountryListResponse>> List()
        {
            return Json(service.GetList());
        }
        [HttpGet]
        public JsonResult<ResponseBase<TBL_SLI_COUNTRY>> Get(int ID)
        {
            return Json(service.Get(ID));
        }
        //[HttpGet]
        //public JsonResult<ResponseBase<CountryListResponse>> Find(string param)
        //{
        //    return Json(service.Find(param));
        //}
        [HttpPost]
        public JsonResult<ResponseBase<TBL_SLI_COUNTRY>> Insert(TBL_SLI_COUNTRY country)
        {
            return Json(service.Insert(country));
        }
        [HttpPost]
        public JsonResult<ResponseBase<TBL_SLI_COUNTRY>> Update(TBL_SLI_COUNTRY country)
        {
            return Json(service.Update(country));
        }
        [HttpDelete]
        public JsonResult<ResponseBase<TBL_SLI_COUNTRY>> Delete(int ID)
        {
            return Json(service.Delete(ID));
        }
    }
}
