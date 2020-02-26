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

        #region methods
        [HttpGet]
        [Route("api/addresss/all")]
        public IHttpActionResult GetAll()
        {
            ResponseDataDTO<IEnumerable<address>> response = new ResponseDataDTO<IEnumerable<address>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                //response.Data = _addressservice.GetAllIncluing(t => t.pu_quantity == 1, q => q.OrderBy(s => s.pu_id));
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
        #region [Create]
        [HttpPost]
        [Route("api/addresss/create")]

        public async Task<IHttpActionResult> Createaddress()
        {
            ResponseDataDTO<address> response = new ResponseDataDTO<address>();
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
                if (streamProvider.FormData["add_province"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tinh/Thành phố không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["add_district"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Quan/Huyen phố không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["add_ward"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Phuong/Xa phố không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["staff_id"] == null && streamProvider.FormData["customer_id"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Phuong/Xa phố không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                AddressCreateViewModel addressCreateViewModel = new AddressCreateViewModel
                {
                    add_province = Convert.ToString(streamProvider.FormData["add_province"]),

                    add_district = Convert.ToString(streamProvider.FormData["add_district"]),
                    add_ward = Convert.ToString(streamProvider.FormData["add_ward"]),

                };
                if (streamProvider.FormData["staff_id"] == null)
                {
                    addressCreateViewModel.staff_id = null;
                }
                else
                {
                    addressCreateViewModel.staff_id = Convert.ToInt32(streamProvider.FormData["staff_id"]);
                }
                if (streamProvider.FormData["customer_id"] == null)
                {
                    addressCreateViewModel.customer_id = null;
                }
                else
                {
                    addressCreateViewModel.customer_id = Convert.ToInt32(streamProvider.FormData["customer_id"]);
                }

                var createAddress = _mapper.Map<address>(addressCreateViewModel);
                _addressservice.Create(createAddress);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = createAddress;
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
        [Route("api/addresss/update")]

        public async Task<IHttpActionResult> Updateaddress()
        {
            ResponseDataDTO<address> response = new ResponseDataDTO<address>();
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
                if (streamProvider.FormData["add_province"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tinh/Thành phố không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["add_district"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Quan/Huyen phố không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["add_ward"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Phuong/Xa phố không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["staff_id"] == null && streamProvider.FormData["customer_id"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Phuong/Xa phố không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                AddressUpdateViewModel addressUpdateViewModel = new AddressUpdateViewModel
                {
                    add_id = Convert.ToInt32(streamProvider.FormData["add_id"]),
                    add_province = Convert.ToString(streamProvider.FormData["add_province"]),

                    add_district = Convert.ToString(streamProvider.FormData["add_district"]),
                    add_ward = Convert.ToString(streamProvider.FormData["add_ward"]),

                };
                if (streamProvider.FormData["staff_id"] == null)
                {
                    addressUpdateViewModel.staff_id = null;
                }
                else
                {
                    addressUpdateViewModel.staff_id = Convert.ToInt32(streamProvider.FormData["staff_id"]);
                }
                if (streamProvider.FormData["customer_id"] == null)
                {
                    addressUpdateViewModel.customer_id = null;
                }
                else
                {
                    addressUpdateViewModel.customer_id = Convert.ToInt32(streamProvider.FormData["customer_id"]);
                }
                // mapping view model to entity
                var updatedaddress = _mapper.Map<address>(addressUpdateViewModel);

                // update address
                _addressservice.Update(updatedaddress, addressUpdateViewModel.add_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = updatedaddress;
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
        [Route("api/addresss/delete")]
        public IHttpActionResult Deleteaddress(int addressId)
        {
            ResponseDataDTO<address> response = new ResponseDataDTO<address>();
            try
            {
                var addressDeleted = _addressservice.Find(addressId);
                if (addressDeleted != null)
                {
                    _addressservice.Delete(addressDeleted);

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
                _addressservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}