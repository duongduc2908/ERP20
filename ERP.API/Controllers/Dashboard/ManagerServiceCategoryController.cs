using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Service;
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
    public class ManagerServiceCategoryController : BaseController
    {
        private readonly IServiceCategoryService _servicecategoryservice;

        private readonly IMapper _mapper;

        public ManagerServiceCategoryController()
        {

        }
        public ManagerServiceCategoryController(IServiceCategoryService servicecategoryservice, IMapper mapper)
        {
            this._servicecategoryservice = servicecategoryservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/services/all")]
        public IHttpActionResult Getservices()
        {
            ResponseDataDTO<IEnumerable<service_category>> response = new ResponseDataDTO<IEnumerable<service_category>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _servicecategoryservice.GetAll();
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
        [Route("api/services/page")]
        public IHttpActionResult GetservicesPaging(int pageSize, int pageNumber)
        {
            ResponseDataDTO<PagedResults<service_category>> response = new ResponseDataDTO<PagedResults<service_category>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _servicecategoryservice.CreatePagedResults(pageNumber, pageSize);
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
        [Route("api/service-category/get-name")]
        public IHttpActionResult GetAllName()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {

                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _servicecategoryservice.GetAllName();
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
                _servicecategoryservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}