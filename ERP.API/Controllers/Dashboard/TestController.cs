using AutoMapper;
using ERP.Common.Constants;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace ERP.API.Controllers.Dashboard
{
    [EnableCors("*", "*", "*")]
    public class TestController : ApiController
    {
        private readonly ITestService _testservice;

        private readonly IMapper _mapper;

        public TestController()
        {

        }
        public TestController(ITestService customerservice, IMapper mapper)
        {
            this._testservice = customerservice;
            this._mapper = mapper;
        }
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/customer/all")]
        public IHttpActionResult Getcustomers()
        {
            ResponseDataDTO<IEnumerable<customer>> response = new ResponseDataDTO<IEnumerable<customer>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _testservice.GetAll();
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = MessageResponse.FAIL;
                response.Data = null;

                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }
    }
}