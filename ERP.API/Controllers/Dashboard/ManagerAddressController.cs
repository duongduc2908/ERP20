//using AutoMapper;
//using ERP.API.Models;
//using ERP.Common.Constants;
//using ERP.Common.Models;
//using ERP.Data.Dto;
//using ERP.Data.ModelsERP;
//using ERP.Data.ModelsERP.ModelView;
//using ERP.Extension.Extensions;
//using ERP.Service.Services.IServices;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Web.Http;
//using System.Web.Http.Cors;

//namespace ERP.API.Controllers.Dashboard
//{
//    public class ManagerAddressController : BaseController
//    {
//        private readonly IAddressService _addressservice;

//        private readonly IMapper _mapper;

//        public ManagerAddressController() { }
//        public ManagerAddressController(IAddressService addressservice, IMapper mapper)
//        {
//            this._addressservice = addressservice;
//            this._mapper = mapper;
//        }

//        #region methods
//        [HttpGet]
//        [Route("api/addresss/all")]
//        public IHttpActionResult Getaddresss()
//        {
//            ResponseDataDTO<IEnumerable<address>> response = new ResponseDataDTO<IEnumerable<address>>();
//            try
//            {
//                response.Code = HttpCode.OK;
//                response.Message = MessageResponse.SUCCESS;
//                response.Data = _addressservice.GetAllIncluing(t => t.pu_quantity == 1, q => q.OrderBy(s => s.pu_id));
//            }
//            catch (Exception ex)
//            {
//                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
//                response.Message = ex.Message;
//                response.Data = null;

//                Console.WriteLine(ex.ToString());
//            }

//            return Ok(response);
//        }

        
        
//        #endregion
//        #region [Create]
//        [HttpPost]
//        [Route("api/addresss/create")]

//        public async Task<IHttpActionResult> Createaddress()
//        {
//            ResponseDataDTO<address> response = new ResponseDataDTO<address>();
//            try
//            {
//                var path = Path.GetTempPath();

//                if (!Request.Content.IsMimeMultipartContent("form-data"))
//                {
//                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
//                }

//                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

//                await Request.Content.ReadAsMultipartAsync(streamProvider);
//                //Các trường bắt buộc 
//                if (streamProvider.FormData["pu_name"] == null)
//                {
//                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
//                    response.Message = "Tên sản phẩm không được để trống";
//                    response.Data = null;
//                    return Ok(response);
//                }
//                if (streamProvider.FormData["pu_quantity"] == null)
//                {
//                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
//                    response.Message = "Số lượng không được để trống";
//                    response.Data = null;
//                    return Ok(response);
//                }
//                if (streamProvider.FormData["pu_buy_price"] == null)
//                {
//                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
//                    response.Message = "Giá bán không được để trống";
//                    response.Data = null;
//                    return Ok(response);
//                }

//                if (streamProvider.FormData["pu_sale_price"] == null)
//                {
//                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
//                    response.Message = "Giá mua không được để trống";
//                    response.Data = null;
//                    return Ok(response);
//                }
//                if (streamProvider.FormData["pu_tax"] == null)
//                {
//                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
//                    response.Message = "Thuế không được để trống";
//                    response.Data = null;
//                    return Ok(response);
//                }

//                if (streamProvider.FormData["pu_unit"] == null)
//                {
//                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
//                    response.Message = "Đơn vị không được để trống";
//                    response.Data = null;
//                    return Ok(response);
//                }
//                if (streamProvider.FormData["address_category_id"] == null)
//                {
//                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
//                    response.Message = "Loại sản phẩm không được để trống";
//                    response.Data = null;
//                    return Ok(response);
//                }
//                if (streamProvider.FormData["provider_id"] == null)
//                {
//                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
//                    response.Message = "Nhà cung cấp không được để trống";
//                    response.Data = null;
//                    return Ok(response);
//                }


//                // get data from formdata
//                AddressCreateViewModel addressCreateViewModel = new AddressCreateViewModel
//                {
//                    add_city = Convert.ToString(streamProvider.FormData["add_city"]),

//                    add_geocoding = Convert.ToInt32(streamProvider.FormData["add_geocoding"]),
//                    pu_buy_price = Convert.ToInt32(streamProvider.FormData["pu_buy_price"]),
//                    pu_sale_price = Convert.ToInt32(streamProvider.FormData["pu_sale_price"]),

//                    address_category_id = Convert.ToInt32(streamProvider.FormData["address_category_id"]),
//                    provider_id = Convert.ToInt32(streamProvider.FormData["provider_id"]),
//                    pu_tax = Convert.ToInt32(streamProvider.FormData["pu_tax"]),
//                    pu_unit = Convert.ToByte(streamProvider.FormData["pu_unit"]),

//                };
//                //Kiem tra cac truong con lại 
//                if (streamProvider.FormData["pu_update_date"] == null)
//                {
//                    addressCreateViewModel.pu_update_date = null;
//                }
//                else
//                {
//                    addressCreateViewModel.pu_update_date = Convert.ToDateTime(streamProvider.FormData["pu_update_date"]);
//                }

//                if (streamProvider.FormData["pu_saleoff"] == null)
//                {
//                    addressCreateViewModel.pu_saleoff = null;
//                }
//                else
//                {
//                    addressCreateViewModel.pu_saleoff = Convert.ToInt32(streamProvider.FormData["pu_saleoff"]);
//                }

//                if (streamProvider.FormData["pu_weight"] == null)
//                {
//                    addressCreateViewModel.pu_weight = null;
//                }
//                else
//                {
//                    addressCreateViewModel.pu_weight = Convert.ToInt32(streamProvider.FormData["pu_weight"]);
//                }

//                if (streamProvider.FormData["pu_description"] == null)
//                {
//                    addressCreateViewModel.pu_description = null;
//                }
//                else
//                {
//                    addressCreateViewModel.pu_description = Convert.ToString(streamProvider.FormData["pu_description"]);
//                }

//                if (streamProvider.FormData["pu_short_description"] == null)
//                {
//                    addressCreateViewModel.pu_short_description = null;
//                }
//                else
//                {
//                    addressCreateViewModel.pu_short_description = Convert.ToString(streamProvider.FormData["pu_short_description"]);
//                }
//                //Tạo mã 
//                var x = _addressservice.GetLast();
//                addressCreateViewModel.pu_code = Utilis.CreateCode("PR", x.pu_id, 7);
//                //Create date
//                addressCreateViewModel.pu_create_date = DateTime.Now;

//                // mapping view model to entity
//                var createdaddress = _mapper.Map<address>(addressCreateViewModel);


//                // save new address
//                _addressservice.Create(createdaddress);
//                // return response
//                response.Code = HttpCode.OK;
//                response.Message = MessageResponse.SUCCESS;
//                response.Data = createdaddress;
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
//                response.Message = ex.Message;
//                response.Data = null;
//                Console.WriteLine(ex.ToString());

//                return Ok(response);
//            }

//        }
//        #endregion

//        #region[Update]
//        [HttpPut]
//        [Route("api/addresss/update")]

//        public async Task<IHttpActionResult> Updateaddress()
//        {
//            ResponseDataDTO<address> response = new ResponseDataDTO<address>();
//            try
//            {
//                var path = Path.GetTempPath();

//                if (!Request.Content.IsMimeMultipartContent("form-data"))
//                {
//                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
//                }

//                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

//                await Request.Content.ReadAsMultipartAsync(streamProvider);
//                //Các trường bắt buộc
//                if (streamProvider.FormData["pu_name"] == null)
//                {
//                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
//                    response.Message = "Tên sản phẩm không được để trống";
//                    response.Data = null;
//                    return Ok(response);
//                }
//                if (streamProvider.FormData["pu_quantity"] == null)
//                {
//                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
//                    response.Message = "Số lượng không được để trống";
//                    response.Data = null;
//                    return Ok(response);
//                }
//                if (streamProvider.FormData["pu_buy_price"] == null)
//                {
//                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
//                    response.Message = "Giá bán không được để trống";
//                    response.Data = null;
//                    return Ok(response);
//                }

//                if (streamProvider.FormData["pu_sale_price"] == null)
//                {
//                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
//                    response.Message = "Giá mua không được để trống";
//                    response.Data = null;
//                    return Ok(response);
//                }
//                if (streamProvider.FormData["pu_tax"] == null)
//                {
//                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
//                    response.Message = "Thuế không được để trống";
//                    response.Data = null;
//                    return Ok(response);
//                }

//                if (streamProvider.FormData["pu_unit"] == null)
//                {
//                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
//                    response.Message = "Đơn vị không được để trống";
//                    response.Data = null;
//                    return Ok(response);
//                }
//                if (streamProvider.FormData["address_category_id"] == null)
//                {
//                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
//                    response.Message = "Loại sản phẩm không được để trống";
//                    response.Data = null;
//                    return Ok(response);
//                }
//                if (streamProvider.FormData["provider_id"] == null)
//                {
//                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
//                    response.Message = "Nhà cung cấp không được để trống";
//                    response.Data = null;
//                    return Ok(response);
//                }


//                // get data from formdata
//                AddressUpdateViewModel addressUpdateViewModel = new AddressUpdateViewModel
//                {
//                    pu_id = Convert.ToInt32(streamProvider.FormData["pu_id"]),
//                    pu_name = Convert.ToString(streamProvider.FormData["pu_name"]),

//                    pu_quantity = Convert.ToInt32(streamProvider.FormData["pu_quantity"]),
//                    pu_buy_price = Convert.ToInt32(streamProvider.FormData["pu_buy_price"]),
//                    pu_sale_price = Convert.ToInt32(streamProvider.FormData["pu_sale_price"]),

//                    address_category_id = Convert.ToInt32(streamProvider.FormData["address_category_id"]),
//                    provider_id = Convert.ToInt32(streamProvider.FormData["provider_id"]),
//                    pu_tax = Convert.ToInt32(streamProvider.FormData["pu_tax"]),
//                    pu_unit = Convert.ToByte(streamProvider.FormData["pu_unit"]),

//                };
//                //Lấy ra dữ liệu cũ 
//                var existaddress = _addressservice.Find(addressUpdateViewModel.pu_id);
//                //Kiem tra cac truong con lại 
//                //Nếu là datetime, option  mà null thì cập nhập lại cái cũ 
//                //Các trường khác thì trả về null 
//                if (streamProvider.FormData["pu_update_date"] == null)
//                {
//                    if (existaddress.pu_update_date != null)
//                    {
//                        addressUpdateViewModel.pu_update_date = existaddress.pu_update_date;
//                    }
//                    else
//                    {
//                        addressUpdateViewModel.pu_update_date = null;
//                    }
//                }
//                else
//                {
//                    addressUpdateViewModel.pu_update_date = Convert.ToDateTime(streamProvider.FormData["pu_update_date"]);
//                }

//                if (streamProvider.FormData["pu_saleoff"] == null)
//                {

//                    addressUpdateViewModel.pu_saleoff = null;

//                }
//                else
//                {
//                    addressUpdateViewModel.pu_saleoff = Convert.ToInt32(streamProvider.FormData["pu_saleoff"]);
//                }

//                if (streamProvider.FormData["pu_weight"] == null)
//                {
//                    addressUpdateViewModel.pu_weight = null;
//                }
//                else
//                {
//                    addressUpdateViewModel.pu_weight = Convert.ToInt32(streamProvider.FormData["pu_weight"]);
//                }

//                if (streamProvider.FormData["pu_description"] == null)
//                {
//                    addressUpdateViewModel.pu_description = null;
//                }
//                else
//                {
//                    addressUpdateViewModel.pu_description = Convert.ToString(streamProvider.FormData["pu_description"]);
//                }

//                if (streamProvider.FormData["pu_short_description"] == null)
//                {
//                    addressUpdateViewModel.pu_short_description = null;
//                }
//                else
//                {
//                    addressUpdateViewModel.pu_short_description = Convert.ToString(streamProvider.FormData["pu_short_description"]);
//                }
//                //Cap nhập lại date code 
//                addressUpdateViewModel.pu_create_date = existaddress.pu_create_date;
//                addressUpdateViewModel.pu_code = existaddress.pu_code;
//                // mapping view model to entity
//                var updatedaddress = _mapper.Map<address>(addressUpdateViewModel);



//                // update address
//                _addressservice.Update(updatedaddress, addressUpdateViewModel.pu_id);
//                // return response
//                response.Code = HttpCode.OK;
//                response.Message = MessageResponse.SUCCESS;
//                response.Data = updatedaddress;
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
//                response.Message = ex.Message;
//                response.Data = null;
//                Console.WriteLine(ex.ToString());

//                return Ok(response);
//            }
//        }
//        #endregion
//        #region [Delete]
//        [HttpDelete]
//        [Route("api/addresss/delete")]
//        public IHttpActionResult Deleteaddress(int addressId)
//        {
//            ResponseDataDTO<address> response = new ResponseDataDTO<address>();
//            try
//            {
//                var addressDeleted = _addressservice.Find(addressId);
//                if (addressDeleted != null)
//                {
//                    _addressservice.Delete(addressDeleted);

//                    // return response
//                    response.Code = HttpCode.OK;
//                    response.Message = MessageResponse.SUCCESS;
//                    response.Data = null;
//                    return Ok(response);
//                }
//                else
//                {
//                    // return response
//                    response.Code = HttpCode.NOT_FOUND;
//                    response.Message = "Không thấy được sản phẩm";
//                    response.Data = null;

//                    return Ok(response);
//                }


//            }
//            catch (Exception ex)
//            {
//                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
//                response.Message = ex.Message;
//                response.Data = null;
//                Console.WriteLine(ex.ToString());

//                return Ok(response);
//            }
//        }
//        #endregion


//        #region dispose

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                _addressservice.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//        #endregion
//    }
//}