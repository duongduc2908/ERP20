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
    public class ManagerProductCategoryController : ApiController
    {
        private readonly IProductCategoryService _product_categoryservice;

        private readonly IMapper _mapper;

        public ManagerProductCategoryController() { }
        public ManagerProductCategoryController(IProductCategoryService product_categoryservice, IMapper mapper)
        {
            this._product_categoryservice = product_categoryservice;
            this._mapper = mapper;
        }

        #region methods
        

        [Route("api/product-categorys/get-name")]
        public IHttpActionResult GetproductsPaging()
        {
            ResponseDataDTO<PagedResults<string>> response = new ResponseDataDTO<PagedResults<string>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _product_categoryservice.GetAllName();
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
                _product_categoryservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}