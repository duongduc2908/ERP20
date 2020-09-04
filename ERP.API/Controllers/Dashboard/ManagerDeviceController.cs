using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Device;
using ERP.Service.Services.IServices;

namespace ERP.API.Controllers.Dashboard
{
    public class ManagerDeviceController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IDeviceService _deviceservice;
        public ManagerDeviceController(IDeviceService deviceservice, IMapper mapper)
        {
            this._deviceservice = deviceservice;
            this._mapper = mapper;
        }

        #region Method

        #region["GetAll"]
        [HttpGet]
        [Route("api/device/get_all")]
        public IHttpActionResult GetAllDropDown()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                int company_id = BaseController.get_company_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _deviceservice.GetAllDropDown(company_id);
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

        #region["GetUnit"]
        [HttpGet]
        [Route("api/device/get_unit")]
        public IHttpActionResult GetUnit()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                int company_id = BaseController.get_company_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _deviceservice.GetUnint(company_id);
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

        #region["Search"]
        [HttpGet]
        [Route("api/device/search")]
        public IHttpActionResult GetDevices(int pageNumber, int pageSize, string search_name, DateTime? start_date, DateTime? end_date)
        {
            ResponseDataDTO<PagedResults<deviceviewmodel>> response = new ResponseDataDTO<PagedResults<deviceviewmodel>>();
            try
            {
                int compnay_id = BaseController.get_company_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _deviceservice.GetAllSearch(pageNumber, pageSize, search_name, compnay_id, start_date, end_date);
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

        #region["Get by id"]
        [Route("api/device/get_by_id")]
        public IHttpActionResult GetDevices(int dev_id)
        {
            ResponseDataDTO<deviceviewmodel> response = new ResponseDataDTO<deviceviewmodel>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _deviceservice.GetById(dev_id);
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

        #region["Create"]
        [HttpPost]
        [Route("api/device/create")]
        public async Task<IHttpActionResult> Create([FromBody] DeviceCreateViewModelJson create_device)
        {
            ResponseDataDTO<device> response = new ResponseDataDTO<device>();
            try
            {
                var device = create_device;

                #region["Check null"]

                if (device.dev_name == null || device.dev_name.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên vật tư không được để trống";
                    response.Error = "dev_name";
                    return Ok(response);
                }
                else
                {
                    if (check_exits_name(device.dev_name.Trim(),create:true,device_id:null))
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Tên vật tư đã tồn tại";
                        response.Error = "dev_name";
                        return Ok(response);
                    }
                }
                if (device.dev_unit == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Đơn vị không được để trống";
                    response.Error = "dev_unit";
                    return Ok(response);
                }

                #endregion

                #region["Save Database"]
                device device_create = new device();
                //Thong tin chung 
                device_create.dev_name = device.dev_name.Trim();
                device_create.dev_create_date = DateTime.Now;
                device_create.dev_note = device.dev_note.Trim();
                device_create.dev_number = device.dev_number;
                device_create.dev_unit = device.dev_unit;
                var x = _deviceservice.GetLast();
                if (x == null) device_create.dev_code = "VT0000";
                else device_create.dev_code = Utilis.CreateCodeByCode("VT", x.dev_code, 6);
                device_create.company_id = BaseController.get_company_id_current();
                _deviceservice.Create(device_create);
                #endregion
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = device_create;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = null;

                return Ok(response);
            }

        }
        #endregion

        #region["Update"]
        [HttpPut]
        [Route("api/Device/update")]
        public async Task<IHttpActionResult> Update([FromBody] DeviceUpdateViewModelJson update_device)
        {
            ResponseDataDTO<device> response = new ResponseDataDTO<device>();
            try
            {
                var device = update_device;

                #region["Check null"]

                if (device.dev_name == null || device.dev_name.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên vật tư không được để trống";
                    response.Error = "dev_name";
                    return Ok(response);
                }
                else
                {
                    if (check_exits_name(device.dev_name.Trim(), create: false,device_id: update_device.dev_id))
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Tên vật tư đã tồn tại";
                        response.Error = "dev_name";
                        return Ok(response);
                    }
                }
                if (device.dev_unit == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Đơn vị không được để trống";
                    response.Error = "dev_unit";
                    return Ok(response);
                }

                #endregion

                //end kiểm tra
                #region["Save Database"]
                device device_exists = _deviceservice.Find(device.dev_id);
                //Thong tin chung 
                device_exists.dev_name = device.dev_name.Trim();
                device_exists.dev_note = device.dev_note.Trim();
                device_exists.dev_number = device.dev_number;
                device_exists.dev_unit = device.dev_unit;
                device_exists.company_id = BaseController.get_company_id_current();
                _deviceservice.Update(device_exists, device_exists.dev_id);
                #endregion
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = device_exists;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = null;

                return Ok(response);
            }

        }
        #endregion

        #region["Delete"]
        [HttpDelete]
        [Route("api/Device/delete")]
        public IHttpActionResult Delete(int dev_id)
        {
            ResponseDataDTO<device> response = new ResponseDataDTO<device>();
            try
            {
                var deviceDeleted = _deviceservice.Find(dev_id);
                if (deviceDeleted != null)
                {
                    _deviceservice.Delete(deviceDeleted);

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

        #endregion

        #region["function common"]
        bool check_exits_name(string name, int? device_id, bool create=true)
        {
            device x = new device();
            if (create && device_id==null)
            {
                 x = _deviceservice.GetAllIncluing(t => t.dev_name.ToLower().Equals(name.ToLower())).FirstOrDefault();
            }
            else
            {
                x = _deviceservice.GetAllIncluing(t => t.dev_name.ToLower().Equals(name.ToLower()) && t.dev_id !=device_id).FirstOrDefault();
            }
            if (x != null)
                return true;
            return false;
        }
        #endregion

        #region dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _deviceservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
