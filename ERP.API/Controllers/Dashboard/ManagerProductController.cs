using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
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
    public class ManagerProductController : ApiController
    {
        private readonly IProductService _productservice;

        private readonly IMapper _mapper;

        public ManagerProductController() { }
        public ManagerProductController(IProductService productservice, IMapper mapper)
        {
            this._productservice = productservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/products/all")]
        public IHttpActionResult Getproducts()
        {
            ResponseDataDTO<IEnumerable<product>> response = new ResponseDataDTO<IEnumerable<product>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _productservice.GetAll();
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

        [Route("api/products/page")]
        public IHttpActionResult GetproductsPaging(int pageSize, int pageNumber)
        {
            ResponseDataDTO<PagedResults<product>> response = new ResponseDataDTO<PagedResults<product>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _productservice.CreatePagedResults(pageNumber, pageSize);
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
        [Route("api/products/search")]
        public IHttpActionResult GetProducts(string search_name)
        {
            ResponseDataDTO<PagedResults<product>> response = new ResponseDataDTO<PagedResults<product>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _productservice.GetProducts(search_name);
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

        [HttpPost]
        [Route("api/products/create")]

        public async Task<IHttpActionResult> Createproduct()
        {
            ResponseDataDTO<product> response = new ResponseDataDTO<product>();
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);

                // get data from formdata
                ProductCreateViewModel productCreateViewModel = new ProductCreateViewModel
                {
                    pu_code = Convert.ToString(streamProvider.FormData["pu_code"]),
                    pu_name = Convert.ToString(streamProvider.FormData["pu_name"]),
                    pu_short_description = Convert.ToString(streamProvider.FormData["pu_short_description"]),
                    pu_description = Convert.ToString(streamProvider.FormData["pu_description"]),


                    pu_quantity = Convert.ToInt32(streamProvider.FormData["pu_quantity"]),
                    pu_buy_price = Convert.ToInt32(streamProvider.FormData["pu_buy_price"]),
                    pu_sale_price = Convert.ToInt32(streamProvider.FormData["pu_sale_price"]),
                    pu_saleoff= Convert.ToInt32(streamProvider.FormData["pu_saleoff"]),
                    product_category_id = Convert.ToInt32(streamProvider.FormData["product_category_id"]),
                    provider_id= Convert.ToInt32(streamProvider.FormData["provider_id"]),
                    pu_tax = Convert.ToInt32(streamProvider.FormData["pu_tax"]),
                    pu_weight = Convert.ToInt32(streamProvider.FormData["pu_weight"]),

                    pu_create_date = Convert.ToDateTime(streamProvider.FormData["pu_create_date"]),
                    pu_update_date = Convert.ToDateTime(streamProvider.FormData["pu_update_date"]),
                    pu_expired_date = Convert.ToDateTime(streamProvider.FormData["pu_expired_date"]),

                                        
                    pu_unit = Convert.ToByte(streamProvider.FormData["pu_unit"]),
                    


                };

                // mapping view model to entity
                var createdproduct = _mapper.Map<product>(productCreateViewModel);


                // save new product
                _productservice.Create(createdproduct);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = createdproduct;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = null;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }

        }




        [HttpPut]
        [Route("api/products/update")]

        public async Task<IHttpActionResult> Updateproduct(int? pu_id)
        {
            ResponseDataDTO<product> response = new ResponseDataDTO<product>();
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);
               

                // get data from formdata
                ProductUpdateViewModel productUpdateViewModel = new ProductUpdateViewModel
                {
                    pu_id = Convert.ToInt32(streamProvider.FormData["pu_id"]),
                    pu_code = Convert.ToString(streamProvider.FormData["pu_code"]),
                    pu_name = Convert.ToString(streamProvider.FormData["pu_name"]),
                    pu_short_description = Convert.ToString(streamProvider.FormData["pu_short_description"]),
                    pu_description = Convert.ToString(streamProvider.FormData["pu_description"]),


                    pu_quantity = Convert.ToInt32(streamProvider.FormData["pu_quantity"]),
                    pu_buy_price = Convert.ToInt32(streamProvider.FormData["pu_buy_price"]),
                    pu_sale_price = Convert.ToInt32(streamProvider.FormData["pu_sale_price"]),
                    pu_saleoff = Convert.ToInt32(streamProvider.FormData["pu_saleoff"]),
                    product_category_id = Convert.ToInt32(streamProvider.FormData["product_category_id"]),
                    provider_id = Convert.ToInt32(streamProvider.FormData["provider_id"]),
                    pu_tax = Convert.ToInt32(streamProvider.FormData["pu_tax"]),
                    pu_weight = Convert.ToInt32(streamProvider.FormData["pu_weight"]),

                    pu_create_date = Convert.ToDateTime(streamProvider.FormData["pu_create_date"]),
                    pu_update_date = Convert.ToDateTime(streamProvider.FormData["pu_update_date"]),
                    pu_expired_date = Convert.ToDateTime(streamProvider.FormData["pu_expired_date"]),


                    pu_unit = Convert.ToByte(streamProvider.FormData["pu_unit"]),

                };



                // mapping view model to entity
                var updatedproduct = _mapper.Map<product>(productUpdateViewModel);



                // update product
                _productservice.Update(updatedproduct, pu_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = updatedproduct;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = null;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }
        }

        [HttpDelete]
        [Route("api/products/delete/{productId}")]
        public IHttpActionResult Deleteproduct(int productId)
        {
            ResponseDataDTO<product> response = new ResponseDataDTO<product>();
            try
            {
                var productDeleted = _productservice.Find(productId);
                if (productDeleted != null)
                {
                    _productservice.Delete(productDeleted);

                    // return response
                    response.Code = HttpCode.OK;
                    response.Message = MessageResponse.SUCCESS;
                    response.Data = null;
                    return Ok(response);
                }
                else
                {
                    // return response
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = MessageResponse.FAIL;
                    response.Data = null;

                    return Ok(response);
                }


            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = null;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }
        }
        #endregion

        #region dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _productservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}