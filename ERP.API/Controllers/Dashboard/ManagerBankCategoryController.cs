using ERP.Data.Dto;
using AutoMapper;
using ERP.API.Models;
using ERP.API.Providers;
using ERP.Common.Constants;
using ERP.Common.Excel;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;

using System.Web.Http;
using System.Web.Http.Cors;
using ERP.Data.ModelsERP.ModelView;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net;
using ERP.Extension.Extensions;

namespace ERP.API.Controllers.Dashboard
{
    public class ManagerBankCategoryController : ApiController
    {
        // GET: ManagerCompany
        private readonly IBankCategoryService _bankcategoryservice;

        private readonly IMapper _mapper;

        public ManagerBankCategoryController() { }
        public ManagerBankCategoryController(IBankCategoryService bankcategoryservice, IMapper mapper)
        {
            this._bankcategoryservice = bankcategoryservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/bank_category/getall")]
        public IHttpActionResult GetAllName()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _bankcategoryservice.GetAllDropDown();
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
        /*
        [HttpGet]
        [Route("api/bank_category/search")]
        public IHttpActionResult Getcompanys(int pageNumber, int pageSize, string search_name)
        {
            ResponseDataDTO<PagedResults<bank>> response = new ResponseDataDTO<PagedResults<bank>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _bankcategoryservice.GetAllSearch(pageNumber, pageSize, search_name);
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
        [HttpGet]
        [Route("api/bank_category/get_by_id")]
        public IHttpActionResult GetById(int ba_id)
        {
            ResponseDataDTO<bank> response = new ResponseDataDTO<bank>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _bankcategoryservice.GetById(ba_id);
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
        */
        
        #endregion
    }
}