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
    public class ManagerCompanyController : ApiController
    {
        // GET: ManagerCompany
        private readonly ICompanyService _companyservice;

        private readonly IMapper _mapper;

        public ManagerCompanyController() { }
        public ManagerCompanyController(ICompanyService companyservice, IMapper mapper)
        {
            this._companyservice = companyservice;
            this._mapper = mapper;
        }

        #region methods
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/companys/all")]
        public IHttpActionResult Getcompanys()
        {
            ResponseDataDTO<IEnumerable<company>> response = new ResponseDataDTO<IEnumerable<company>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _companyservice.GetAll();
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
        #endregion
    }
}