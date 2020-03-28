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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
namespace ERP.API.Controllers.Dashboard
{
    [EnableCors("*", "*", "*")]
    public class ManagerServiceTimeController : BaseController
    {
        private readonly IServiceTimeService _servicetimeservice;

        private readonly IMapper _mapper;

        public ManagerServiceTimeController() { }
        public ManagerServiceTimeController(IServiceTimeService servicetimeservice, IMapper mapper)
        {
            this._servicetimeservice = servicetimeservice;
            this._mapper = mapper;
        }

        #region methon
        [HttpGet]
        [Route("api/service-time/get-all-repeat-type")]
        public IHttpActionResult Getservices()
        {
            ResponseDataDTO<IEnumerable<dropdown>> response = new ResponseDataDTO<IEnumerable<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _servicetimeservice.GetRepeatType();
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

    }
}