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
    public class ManagerSupplierController : ApiController
    {
        private readonly ISupplierService _supplierservice;

        private readonly IMapper _mapper;

        public ManagerSupplierController() { }
        public ManagerSupplierController(ISupplierService supplierservice, IMapper mapper)
        {
            this._supplierservice = supplierservice;
            this._mapper = mapper;
        }

        #region methods


        [Route("api/suppliers/get-name")]
        public IHttpActionResult GetproductsPaging()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _supplierservice.GetAllName();
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
                _supplierservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}