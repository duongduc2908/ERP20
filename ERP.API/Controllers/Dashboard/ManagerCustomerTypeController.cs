using ERP.Data.Dto;
using AutoMapper;
using ERP.Common.Constants;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;

using System.Web.Http;
using ERP.Data.ModelsERP.ModelView;
namespace ERP.API.Controllers.Dashboard
{
    public class ManagerCustomerTypeController : ApiController
    {
        // GET: ManagerCompany
        private readonly ICustomerTypeService _customertypeservice;

        private readonly IMapper _mapper;

        public ManagerCustomerTypeController() { }
        public ManagerCustomerTypeController(ICustomerTypeService customertypeservice, IMapper mapper)
        {
            this._customertypeservice = customertypeservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/customer_type/getall")]
        public IHttpActionResult GetAllName()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                int company_id = BaseController.get_company_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customertypeservice.GetAllDropdown(company_id);
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