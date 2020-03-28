using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Service;
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
    public class ManagerServiceController : ApiController
    {
        private readonly IServiceService _serviceservice;

        private readonly IMapper _mapper;

        public ManagerServiceController()
        {

        }
        public ManagerServiceController(IServiceService serviceservice, IMapper mapper)
        {
            this._serviceservice = serviceservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/services/all")]
        public IHttpActionResult Getservices()
        {
            ResponseDataDTO<IEnumerable<service>> response = new ResponseDataDTO<IEnumerable<service>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _serviceservice.GetAll();
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
        [Route("api/services/page")]
        public IHttpActionResult GetservicesPaging(int pageSize, int pageNumber)
        {
            ResponseDataDTO<PagedResults<service>> response = new ResponseDataDTO<PagedResults<service>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _serviceservice.CreatePagedResults(pageNumber, pageSize);
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
        [Route("api/service/get-all-search")]
        public IHttpActionResult GetAllPageSearch(int pageSize, int pageNumber,string search_name)
        {
            ResponseDataDTO<PagedResults<serviceviewmodel>> response = new ResponseDataDTO<PagedResults<serviceviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _serviceservice.GetAllPageSearch(pageNumber, pageSize,search_name);
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
        //PagedResults<serviceinforviewmodel> GetAllPageInforService(int pageNumber, int pageSize, string search_name)
        [HttpGet]
        [Route("api/service/get-search-infor")]
        public IHttpActionResult GetAllPageInforService(int pageNumber, int pageSize, string search_name)
        {
            ResponseDataDTO<PagedResults<serviceinforviewmodel>> response = new ResponseDataDTO<PagedResults<serviceinforviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _serviceservice.GetAllPageInforService(pageNumber, pageSize, search_name);
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
        [Route("api/service/get-type")]
        public IHttpActionResult GetType()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _serviceservice.GetType();
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
        [Route("api/service/create")]

        public async Task<IHttpActionResult> Createservice()
        {
            ResponseDataDTO<service> response = new ResponseDataDTO<service>();
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
                ServiceCreateViewModel serviceCreateViewModel = new ServiceCreateViewModel
                {
                    se_name = Convert.ToString(streamProvider.FormData["se_name"]),
                    se_description = Convert.ToString(streamProvider.FormData["se_description"]),
                    service_category_id = Convert.ToInt32(streamProvider.FormData["service_category_id"]),
                    se_price = Convert.ToInt32(streamProvider.FormData["se_price"]),
                    se_saleoff = Convert.ToInt32(streamProvider.FormData["se_saleoff"]),
                    se_type = Convert.ToByte(streamProvider.FormData["se_type"]),
                };
               
                // mapping view model to entity
                var createdservice = _mapper.Map<service>(serviceCreateViewModel);
                //Tạo mã 
                var x = _serviceservice.GetLast();
                if (x == null) createdservice.se_code = Utilis.CreateCode("DV", 0, 7);
                else createdservice.se_code = Utilis.CreateCode("DV", x.se_id, 7);
                //save file 
                string fileName = "";
                foreach (MultipartFileData fileData in streamProvider.FileData)
                {
                    fileName = (FileExtension.SaveFileServiceOnDisk(fileData, createdservice.se_code));
                }
                if (fileName == "")
                {
                    createdservice.se_thumbnai = "/Uploads/Images/default/service.png";
                }
                else createdservice.se_thumbnai = fileName;
                // save new service
                _serviceservice.Create(createdservice);

                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = createdservice;
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
        [Route("api/service/update/")]

        public async Task<IHttpActionResult> Updateservice()
        {
            ResponseDataDTO<service> response = new ResponseDataDTO<service>();
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);

                
                int se_id  = Convert.ToInt32(streamProvider.FormData["se_id"]);
                var existservice = _serviceservice.Find(se_id);
                existservice.se_name = Convert.ToString(streamProvider.FormData["se_name"]);
                existservice.se_description = Convert.ToString(streamProvider.FormData["se_description"]);
                existservice.service_category_id = Convert.ToInt32(streamProvider.FormData["service_category_id"]);
                existservice.se_price = Convert.ToInt32(streamProvider.FormData["se_price"]);
                existservice.se_saleoff = Convert.ToInt32(streamProvider.FormData["se_saleoff"]);
                existservice.se_type = Convert.ToByte(streamProvider.FormData["se_type"]);

                // save file
                string fileName = "";
                if (streamProvider.FileData.Count > 0)
                {
                    foreach (MultipartFileData fileData in streamProvider.FileData)
                    {
                        fileName = FileExtension.SaveFileServiceOnDisk(fileData, existservice.se_code);
                    }
                }
                if (fileName == "")
                {
                    existservice.se_thumbnai = "/Uploads/Images/default/service.png";
                }
                else existservice.se_thumbnai = fileName;
                // update service
                _serviceservice.Update(existservice, existservice.se_id);

                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = existservice;
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
        [Route("api/service/delete")]
        public IHttpActionResult Deleteservice(int serviceId)
        {
            ResponseDataDTO<service> response = new ResponseDataDTO<service>();
            try
            {
                var serviceDeleted = _serviceservice.Find(serviceId);
                if (serviceDeleted != null)
                {
                    _serviceservice.Delete(serviceDeleted);

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
                _serviceservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}