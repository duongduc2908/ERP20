using AutoMapper;
using ERP.Common.Constants;
using ERP.Data.Dto;
using ERP.Data.ModelsERP.ModelView;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ERP.API.Controllers.Dashboard
{
    public class ManagerStaffTypeController : BaseController
    {
        private readonly IStaffTypeService _stafftypeservice;

        public ManagerStaffTypeController() { }
        public ManagerStaffTypeController(IStaffTypeService stafftypeservice)
        {
            this._stafftypeservice = stafftypeservice;
        }

        #region methods
        [HttpGet]
        [Route("api/staff_type/getall")]
        public IHttpActionResult GetAllName()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                int company_id = BaseController.get_company_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _stafftypeservice.GetAllDropdown(company_id);
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
