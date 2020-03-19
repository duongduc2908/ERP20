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
    [Authorize]
    public class ManagerAddressController : BaseController
    {
        private readonly IAddressService _addressservice;

        private readonly IMapper _mapper;

        public ManagerAddressController() { }
        public ManagerAddressController(IAddressService addressservice, IMapper mapper)
        {
            this._addressservice = addressservice;
            this._mapper = mapper;
        }
        #region[Get Province]
        [HttpGet]
        [Route("api/addresss/get-province")]
        public IHttpActionResult Getaddresss()
        {
            ResponseDataDTO<IEnumerable<dropdown>> response = new ResponseDataDTO<IEnumerable<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _addressservice.GetAllProvince();
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
        [Route("api/addresss/get-district")]
        public IHttpActionResult GetDistrict(int? province_id)
        {
            ResponseDataDTO<IEnumerable<dropdown>> response = new ResponseDataDTO<IEnumerable<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _addressservice.GetAllDistrictByIdPro(province_id);
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
        [Route("api/addresss/get-ward")]
        public IHttpActionResult GetWard(int? district_id)
        {
            ResponseDataDTO<IEnumerable<dropdown>> response = new ResponseDataDTO<IEnumerable<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _addressservice.GetAllWardByIdDis(district_id);
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