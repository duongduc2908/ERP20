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

namespace ERP.API.Controllers.Dashboard
{
    public class ManagerUndertakenLocationController : BaseController
    {
        private readonly IUndertakenLocationService _UndertakenLocationservice;

        private readonly IMapper _mapper;

        public ManagerUndertakenLocationController() { }
        public ManagerUndertakenLocationController(IUndertakenLocationService UndertakenLocationservice, IMapper mapper)
        {
            this._UndertakenLocationservice = UndertakenLocationservice;
            this._mapper = mapper;
        }

        #region[Get Province]
        [HttpGet]
        [Route("api/undertakenLocations/get-province")]
        public IHttpActionResult GetUndertakenLocations()
        {
            ResponseDataDTO<IEnumerable<dropdown>> response = new ResponseDataDTO<IEnumerable<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _UndertakenLocationservice.GetAllProvince();
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
        [Route("api/undertakenLocations/get-district")]
        public IHttpActionResult GetDistrict(int? province_id)
        {
            ResponseDataDTO<IEnumerable<dropdown>> response = new ResponseDataDTO<IEnumerable<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _UndertakenLocationservice.GetAllDistrictByIdPro(province_id);
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
        [Route("api/undertakenLocations/get-ward")]
        public IHttpActionResult GetWard(int? district_id)
        {
            ResponseDataDTO<IEnumerable<dropdown>> response = new ResponseDataDTO<IEnumerable<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _UndertakenLocationservice.GetAllWardByIdDis(district_id);
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

        #region["Get All"]
        [HttpGet]
        [Route("api/undertakenLocations/all")]
        public IHttpActionResult GetAll()
        {
            ResponseDataDTO<IEnumerable<undertaken_location>> response = new ResponseDataDTO<IEnumerable<undertaken_location>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                //response.Data = _UndertakenLocationservice.GetAllIncluing(t => t.pu_quantity == 1, q => q.OrderBy(s => s.pu_id));
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
        [Route("api/undertakenLocations/create")]
        public async Task<IHttpActionResult> CreateUndertakenLocation()
        {
            ResponseDataDTO<undertaken_location> response = new ResponseDataDTO<undertaken_location>();
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
                if (streamProvider.FormData["unl_province"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tinh/Thành phố không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["unl_district"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Quan/Huyen phố không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["unl_ward"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Phuong/Xa phố không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["staff_id"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Mã nhân viên không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                UndertakenLocationCreateViewModel undertakenlocationcreateViewModel = new UndertakenLocationCreateViewModel
                {
                    unl_province = Convert.ToString(streamProvider.FormData["unl_province"]),

                    unl_district = Convert.ToString(streamProvider.FormData["unl_district"]),
                    unl_ward = Convert.ToString(streamProvider.FormData["unl_ward"]),

                };
                undertakenlocationcreateViewModel.staff_id = Convert.ToInt32(streamProvider.FormData["staff_id"]);
                if (streamProvider.FormData["unl_detail"] == null)
                {
                    undertakenlocationcreateViewModel.unl_detail = null;
                }
                else
                {
                    undertakenlocationcreateViewModel.unl_detail = Convert.ToString(streamProvider.FormData["unl_detail"]);
                }
                if (streamProvider.FormData["unl_note"] == null)
                {
                    undertakenlocationcreateViewModel.unl_note = null;
                }
                else
                {
                    undertakenlocationcreateViewModel.unl_note = Convert.ToString(streamProvider.FormData["unl_note"]);
                }
                if (streamProvider.FormData["unl_geocoding"] == null)
                {
                    undertakenlocationcreateViewModel.unl_geocoding = null;
                }
                else
                {
                    undertakenlocationcreateViewModel.unl_geocoding = Convert.ToString(streamProvider.FormData["unl_geocoding"]);
                }
                var createUndertakenLocation = _mapper.Map<undertaken_location>(undertakenlocationcreateViewModel);
                _UndertakenLocationservice.Create(createUndertakenLocation);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = createUndertakenLocation;
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
        [Route("api/undertakenLocations/update")]

        public async Task<IHttpActionResult> UpdateUndertakenLocation()
        {
            ResponseDataDTO<undertaken_location> response = new ResponseDataDTO<undertaken_location>();
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
                if (streamProvider.FormData["unl_province"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tinh/Thành phố không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["unl_district"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Quan/Huyen phố không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["unl_ward"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Phuong/Xa phố không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["staff_id"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Mã nhân viên không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                UndertakenLocationUpdateViewModel undertakenlocationUpdateViewModel = new UndertakenLocationUpdateViewModel
                {
                    unl_id = Convert.ToInt32(streamProvider.FormData["unl_id"]),
                    unl_province = Convert.ToString(streamProvider.FormData["unl_province"]),
                    unl_district = Convert.ToString(streamProvider.FormData["unl_district"]),
                    unl_ward = Convert.ToString(streamProvider.FormData["unl_ward"]),

                };
                undertakenlocationUpdateViewModel.staff_id = Convert.ToInt32(streamProvider.FormData["staff_id"]);
                if (streamProvider.FormData["unl_detail"] == null)
                {
                    undertakenlocationUpdateViewModel.unl_detail = null;
                }
                else
                {
                    undertakenlocationUpdateViewModel.unl_detail = Convert.ToString(streamProvider.FormData["unl_detail"]);
                }
                if (streamProvider.FormData["unl_note"] == null)
                {
                    undertakenlocationUpdateViewModel.unl_note = null;
                }
                else
                {
                    undertakenlocationUpdateViewModel.unl_note = Convert.ToString(streamProvider.FormData["unl_note"]);
                }
                if (streamProvider.FormData["unl_geocoding"] == null)
                {
                    undertakenlocationUpdateViewModel.unl_geocoding = null;
                }
                else
                {
                    undertakenlocationUpdateViewModel.unl_geocoding = Convert.ToString(streamProvider.FormData["unl_geocoding"]);
                }
                // mapping view model to entity
                var updatedUndertakenLocation = _mapper.Map<undertaken_location>(undertakenlocationUpdateViewModel);

                // update UndertakenLocation
                _UndertakenLocationservice.Update(updatedUndertakenLocation, undertakenlocationUpdateViewModel.unl_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = updatedUndertakenLocation;
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
        [Route("api/undertakenLocations/delete")]
        public IHttpActionResult DeleteUndertakenLocation(int undertakenlocationId)
        {
            ResponseDataDTO<undertaken_location> response = new ResponseDataDTO<undertaken_location>();
            try
            {
                var UndertakenLocationDeleted = _UndertakenLocationservice.Find(undertakenlocationId);
                if (UndertakenLocationDeleted != null)
                {
                    _UndertakenLocationservice.Delete(UndertakenLocationDeleted);

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
                    response.Message = "Không thấy được mã địa chỉ";
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

        #endregion

        #region dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _UndertakenLocationservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
