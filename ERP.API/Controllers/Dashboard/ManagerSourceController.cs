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
    public class ManagerSourceController : ApiController
    {
        private readonly ISourceService _sourceservice;

        private readonly IMapper _mapper;

        public ManagerSourceController() { }
        public ManagerSourceController(ISourceService sourceservice, IMapper mapper)
        {
            this._sourceservice = sourceservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/source/getall")]
        public IHttpActionResult GetAllDropdown()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _sourceservice.GetAllDropdown();
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
        [Route("api/sources/all")]
        public IHttpActionResult GetAll()
        {
            ResponseDataDTO<IEnumerable<source>> response = new ResponseDataDTO<IEnumerable<source>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _sourceservice.GetAll();
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
                _sourceservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
