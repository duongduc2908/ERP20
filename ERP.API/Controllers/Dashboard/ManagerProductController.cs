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
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ERP.API.Controllers.Dashboard
{
    [EnableCors("*", "*", "*")]
    [Authorize]
    public class ManagerProductController : BaseController
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

            new BaseController();
            var current_id = BaseController.current_id;
            ResponseDataDTO<IEnumerable<product>> response = new ResponseDataDTO<IEnumerable<product>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _productservice.GetAllIncluing(t => t.pu_quantity == 1, q => q.OrderBy(s => s.pu_id));
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
            ResponseDataDTO<PagedResults<productviewmodel>> response = new ResponseDataDTO<PagedResults<productviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _productservice.GetAllPage(pageNumber, pageSize);
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
        [Route("api/products/unit")]
        public IHttpActionResult GetUnit()
        {
            ResponseDataDTO<PagedResults<dropdown>> response = new ResponseDataDTO<PagedResults<dropdown>> ();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _productservice.GetUnit();
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
        #region [Get By Id]
        [Route("api/products/infor")]
        public IHttpActionResult GetAllById(int pu_id)
        {
            ResponseDataDTO<PagedResults<productviewmodel>> response = new ResponseDataDTO<PagedResults<productviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _productservice.GetAllPageById(pu_id);
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
        #region[Search]
        [Route("api/products/search")]
        public IHttpActionResult GetProducts(int pageNumber, int pageSize, string search_name, int? category_id)
        {
            ResponseDataDTO<PagedResults<productviewmodel>> response = new ResponseDataDTO<PagedResults<productviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _productservice.GetProducts(pageNumber:pageNumber, pageSize:pageSize, search_name:search_name, category_id:category_id);
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
        #endregion
        #region [Create]
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
                //Các trường bắt buộc 
                if (streamProvider.FormData["pu_name"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên sản phẩm không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["pu_quantity"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Số lượng không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["pu_buy_price"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Giá bán không được để trống";
                    response.Data = null;
                    return Ok(response);
                }

                if (streamProvider.FormData["pu_sale_price"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Giá mua không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["pu_tax"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Thuế không được để trống";
                    response.Data = null;
                    return Ok(response);
                }

                if (streamProvider.FormData["pu_unit"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Đơn vị không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["product_category_id"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Loại sản phẩm không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["provider_id"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Nhà cung cấp không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                

                // get data from formdata
                ProductCreateViewModel productCreateViewModel = new ProductCreateViewModel
                {
                    pu_name = Convert.ToString(streamProvider.FormData["pu_name"]),

                    pu_quantity = Convert.ToInt32(streamProvider.FormData["pu_quantity"]),
                    pu_buy_price = Convert.ToInt32(streamProvider.FormData["pu_buy_price"]),
                    pu_sale_price = Convert.ToInt32(streamProvider.FormData["pu_sale_price"]),
                    
                    product_category_id = Convert.ToInt32(streamProvider.FormData["product_category_id"]),
                    provider_id= Convert.ToInt32(streamProvider.FormData["provider_id"]),
                    pu_tax = Convert.ToInt32(streamProvider.FormData["pu_tax"]),
                    pu_unit = Convert.ToByte(streamProvider.FormData["pu_unit"]),

                };
                //Kiem tra cac truong con lại 
                if (streamProvider.FormData["pu_update_date"] == null)
                {
                    productCreateViewModel.pu_update_date = null;
                }
                else
                {
                    productCreateViewModel.pu_update_date = Convert.ToDateTime(streamProvider.FormData["pu_update_date"]);
                }

                if (streamProvider.FormData["pu_saleoff"] == null)
                {
                    productCreateViewModel.pu_saleoff = null;
                }
                else
                {
                    productCreateViewModel.pu_saleoff = Convert.ToInt32(streamProvider.FormData["pu_saleoff"]);
                }

                if (streamProvider.FormData["pu_weight"] == null)
                {
                    productCreateViewModel.pu_weight = null;
                }
                else
                {
                    productCreateViewModel.pu_weight = Convert.ToInt32(streamProvider.FormData["pu_weight"]);
                }

                if (streamProvider.FormData["pu_description"] == null)
                {
                    productCreateViewModel.pu_description = null;
                }
                else
                {
                    productCreateViewModel.pu_description = Convert.ToString(streamProvider.FormData["pu_description"]);
                }

                if (streamProvider.FormData["pu_short_description"] == null)
                {
                    productCreateViewModel.pu_short_description = null;
                }
                else
                {
                    productCreateViewModel.pu_short_description = Convert.ToString(streamProvider.FormData["pu_short_description"]);
                }
                //Tạo mã 
                var x = _productservice.GetLast();
                productCreateViewModel.pu_code = Utilis.CreateCode("CH", x.pu_id, 7);
                //Create date
                productCreateViewModel.pu_create_date = DateTime.Now;

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
        #endregion

        #region[Update]
        [HttpPut]
        [Route("api/products/update")]

        public async Task<IHttpActionResult> Updateproduct()
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
                //Các trường bắt buộc
                if (streamProvider.FormData["pu_name"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên sản phẩm không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["pu_quantity"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Số lượng không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["pu_buy_price"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Giá bán không được để trống";
                    response.Data = null;
                    return Ok(response);
                }

                if (streamProvider.FormData["pu_sale_price"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Giá mua không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["pu_tax"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Thuế không được để trống";
                    response.Data = null;
                    return Ok(response);
                }

                if (streamProvider.FormData["pu_unit"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Đơn vị không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["product_category_id"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Loại sản phẩm không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["provider_id"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Nhà cung cấp không được để trống";
                    response.Data = null;
                    return Ok(response);
                }

                
                // get data from formdata
                ProductUpdateViewModel productUpdateViewModel = new ProductUpdateViewModel
                {
                    pu_id = Convert.ToInt32(streamProvider.FormData["pu_id"]),
                    pu_name = Convert.ToString(streamProvider.FormData["pu_name"]),

                    pu_quantity = Convert.ToInt32(streamProvider.FormData["pu_quantity"]),
                    pu_buy_price = Convert.ToInt32(streamProvider.FormData["pu_buy_price"]),
                    pu_sale_price = Convert.ToInt32(streamProvider.FormData["pu_sale_price"]),

                    product_category_id = Convert.ToInt32(streamProvider.FormData["product_category_id"]),
                    provider_id = Convert.ToInt32(streamProvider.FormData["provider_id"]),
                    pu_tax = Convert.ToInt32(streamProvider.FormData["pu_tax"]),
                    pu_unit = Convert.ToByte(streamProvider.FormData["pu_unit"]),

                };
                //Lấy ra dữ liệu cũ 
                var existproduct = _productservice.Find(productUpdateViewModel.pu_id);
                //Kiem tra cac truong con lại 
                //Nếu là datetime, option  mà null thì cập nhập lại cái cũ 
                //Các trường khác datetime thì trả về null 
                if (streamProvider.FormData["pu_update_date"] == null)
                {
                    if (existproduct.pu_update_date != null)
                    {
                        productUpdateViewModel.pu_update_date = existproduct.pu_update_date;
                    }
                    else
                    {
                        productUpdateViewModel.pu_update_date = null;
                    }
                }
                else
                {
                    productUpdateViewModel.pu_update_date = Convert.ToDateTime(streamProvider.FormData["pu_update_date"]);
                }

                if (streamProvider.FormData["pu_saleoff"] == null)
                {
                    
                    productUpdateViewModel.pu_saleoff = null;
                    
                }
                else
                {
                    productUpdateViewModel.pu_saleoff = Convert.ToInt32(streamProvider.FormData["pu_saleoff"]);
                }

                if (streamProvider.FormData["pu_weight"] == null)
                {
                    productUpdateViewModel.pu_weight = null;
                }
                else
                {
                    productUpdateViewModel.pu_weight = Convert.ToInt32(streamProvider.FormData["pu_weight"]);
                }

                if (streamProvider.FormData["pu_description"] == null)
                {
                    productUpdateViewModel.pu_description = null;
                }
                else
                {
                    productUpdateViewModel.pu_description = Convert.ToString(streamProvider.FormData["pu_description"]);
                }

                if (streamProvider.FormData["pu_short_description"] == null)
                {
                    productUpdateViewModel.pu_short_description = null;
                }
                else
                {
                    productUpdateViewModel.pu_short_description = Convert.ToString(streamProvider.FormData["pu_short_description"]);
                }
                //Cap nhập lại date code 
                productUpdateViewModel.pu_create_date = existproduct.pu_create_date;
                productUpdateViewModel.pu_code = existproduct.pu_code;
                // mapping view model to entity
                var updatedproduct = _mapper.Map<product>(productUpdateViewModel);



                // update product
                _productservice.Update(updatedproduct, productUpdateViewModel.pu_id);
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
        #endregion
        #region [Delete]
        [HttpDelete]
        [Route("api/products/delete")]
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
                    response.Message = "Không thấy được sản phẩm";
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