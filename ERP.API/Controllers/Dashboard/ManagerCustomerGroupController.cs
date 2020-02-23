using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Extension.Extensions;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ERP.API.Controllers.Dashboard
{
    [EnableCors("*", "*", "*")]
    [Authorize]
    public class ManagerCustomerGroupController : ApiController
    {
        private readonly ICustomerGroupService _customer_groupservice;

        private readonly IMapper _mapper;

        public ManagerCustomerGroupController() { }
        public ManagerCustomerGroupController(ICustomerGroupService customer_groupservice, IMapper mapper)
        {
            this._customer_groupservice = customer_groupservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/customer-groups/search")]
        public IHttpActionResult Getcustomer_groupsPaging(int pageSize, int pageNumber, int? cg_id, string name)
        {
            ResponseDataDTO<PagedResults<customergroupviewmodel>> response = new ResponseDataDTO<PagedResults<customergroupviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customer_groupservice.GetAllPageSearch(pageNumber, pageSize, cg_id, name);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = null;

                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("api/customer-groups/all")]
        public IHttpActionResult GetAll()
        {
            ResponseDataDTO<IEnumerable<customer_group>> response = new ResponseDataDTO<IEnumerable<customer_group>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customer_groupservice.GetAll();
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = null;

                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }

        #endregion

        #region dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _customer_groupservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}