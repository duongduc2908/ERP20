using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
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
    public class ManagerShipAddressController : BaseController
    {
        private readonly IShipAddressService _shipaddressservice;

        private readonly IMapper _mapper;

        public ManagerShipAddressController() { }
        public ManagerShipAddressController(IShipAddressService shipaddressservice, IMapper mapper)
        {
            this._shipaddressservice = shipaddressservice;
            this._mapper = mapper;
        }
        #region[Get Province]
        [HttpGet]
        [Route("api/ship-addresss/get-province")]
        public IHttpActionResult Getaddresss()
        {
            ResponseDataDTO<IEnumerable<dropdown>> response = new ResponseDataDTO<IEnumerable<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _shipaddressservice.GetAllProvince();
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
        [Route("api/ship-addresss/get-district")]
        public IHttpActionResult GetDistrict(int? province_id)
        {
            ResponseDataDTO<IEnumerable<dropdown>> response = new ResponseDataDTO<IEnumerable<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _shipaddressservice.GetAllDistrictByIdPro(province_id);
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
        [Route("api/ship-addresss/get-ward")]
        public IHttpActionResult GetWard(int? district_id)
        {
            ResponseDataDTO<IEnumerable<dropdown>> response = new ResponseDataDTO<IEnumerable<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _shipaddressservice.GetAllWardByIdDis(district_id);
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
        [Route("api/ship-addresss/all")]
        public IHttpActionResult GetAll()
        {
            ResponseDataDTO<IEnumerable<ship_address>> response = new ResponseDataDTO<IEnumerable<ship_address>>();
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
        [Route("api/ship-addresss/create")]
        public async Task<IHttpActionResult> Createaddress()
        {
            ResponseDataDTO<ship_address> response = new ResponseDataDTO<ship_address>();
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
                if (streamProvider.FormData["sha_province"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tinh/Thành phố không được để trống.";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["sha_district"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "QUận/Huyện không được để trống.";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["sha_ward"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Phường/Xã không được để trống.";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["customer_id"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Mã khách hàng không được để trống.";
                    response.Data = null;
                    return Ok(response);
                }
                ShipAddressCreateViewModel shipaddressCreateViewModel = new ShipAddressCreateViewModel
                {
                    sha_province = Convert.ToString(streamProvider.FormData["sha_province"]),
                    sha_district = Convert.ToString(streamProvider.FormData["sha_district"]),
                    sha_ward = Convert.ToString(streamProvider.FormData["sha_ward"]),

                };
                shipaddressCreateViewModel.customer_id = Convert.ToInt32(streamProvider.FormData["customer_id"]);
                if (streamProvider.FormData["sha_detail"] == null)
                {
                    shipaddressCreateViewModel.sha_detail = null;
                }
                else
                {
                    shipaddressCreateViewModel.sha_detail = Convert.ToString(streamProvider.FormData["sha_detail"]);
                }
                if (streamProvider.FormData["sha_note"] == null)
                {
                    shipaddressCreateViewModel.sha_note = null;
                }
                else
                {
                    shipaddressCreateViewModel.sha_note = Convert.ToString(streamProvider.FormData["sha_note"]);
                }
                if (streamProvider.FormData["sha_geocoding"] == null)
                {
                    shipaddressCreateViewModel.sha_geocoding = null;
                }
                else
                {
                    shipaddressCreateViewModel.sha_geocoding = Convert.ToString(streamProvider.FormData["sha_geocoding"]);
                }

                var createShipAddress = _mapper.Map<ship_address>(shipaddressCreateViewModel);
                _shipaddressservice.Create(createShipAddress);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = createShipAddress;
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
        [Route("api/ship-addresss/update")]
        public async Task<IHttpActionResult> Updateaddress()
        {
            ResponseDataDTO<ship_address> response = new ResponseDataDTO<ship_address>();
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
                if (streamProvider.FormData["sha_id"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Mã địa chỉ không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["sha_province"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tinh/Thành phố không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["sha_district"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Quận/Huyện không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["sha_ward"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Phường/Xã không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["customer_id"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Mã khách hàng không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                ShipAddressUpdateViewModel shipaddressUpdateViewModel = new ShipAddressUpdateViewModel
                {
                    sha_id = Convert.ToInt32(streamProvider.FormData["sha_id"]),
                    sha_province = Convert.ToString(streamProvider.FormData["sha_province"]),

                    sha_district = Convert.ToString(streamProvider.FormData["sha_district"]),
                    sha_ward = Convert.ToString(streamProvider.FormData["sha_ward"]),

                };
                shipaddressUpdateViewModel.customer_id = Convert.ToInt32(streamProvider.FormData["customer_id"]);
                if (streamProvider.FormData["sha_detail"] == null)
                {
                    shipaddressUpdateViewModel.sha_detail = null;
                }
                else
                {
                    shipaddressUpdateViewModel.sha_detail = Convert.ToString(streamProvider.FormData["sha_detail"]);
                }
                if (streamProvider.FormData["sha_note"] == null)
                {
                    shipaddressUpdateViewModel.sha_note = null;
                }
                else
                {
                    shipaddressUpdateViewModel.sha_note = Convert.ToString(streamProvider.FormData["sha_note"]);
                }
                if (streamProvider.FormData["sha_geocoding"] == null)
                {
                    shipaddressUpdateViewModel.sha_geocoding = null;
                }
                else
                {
                    shipaddressUpdateViewModel.sha_geocoding = Convert.ToString(streamProvider.FormData["sha_geocoding"]);
                }
                // mapping view model to entity
                var updatedshipaddress = _mapper.Map<ship_address>(shipaddressUpdateViewModel);

                // update address
                _shipaddressservice.Update(updatedshipaddress, shipaddressUpdateViewModel.sha_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = updatedshipaddress;
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
        [Route("api/ship-addresss/delete")]
        public IHttpActionResult Deleteaddress(int shipaddressId)
        {
            ResponseDataDTO<ship_address> response = new ResponseDataDTO<ship_address>();
            try
            {
                var shipaddressDeleted = _shipaddressservice.Find(shipaddressId);
                if (shipaddressDeleted != null)
                {
                    _shipaddressservice.Delete(shipaddressDeleted);

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
                    response.Message = "Không tìm thấy địa chỉ trong hệ thống.";
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
                _shipaddressservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
