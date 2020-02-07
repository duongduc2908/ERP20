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

        [HttpPost]
        [Route("api/services/create")]

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
                // save file
                string fileName = "";
                foreach (MultipartFileData fileData in streamProvider.FileData)
                {
                    fileName = (FileExtension.SaveFileOnDisk(fileData));
                }
                // get data from formdata
                ServiceCreateViewModel serviceCreateViewModel = new ServiceCreateViewModel
                {
                    se_code = Convert.ToString(streamProvider.FormData["se_code"]),
                    se_name = Convert.ToString(streamProvider.FormData["se_name"]),
                    se_description = Convert.ToString(streamProvider.FormData["se_description"]),
                    



                    service_category_id = Convert.ToInt32(streamProvider.FormData["service_category_id"]),
                    se_price = Convert.ToInt32(streamProvider.FormData["se_price"]),
                    se_saleoff = Convert.ToInt32(streamProvider.FormData["se_saleoff"]),

                    se_type = Convert.ToByte(streamProvider.FormData["se_type"]),
                   
                    


                };
               
                // mapping view model to entity
                var createdservice = _mapper.Map<service>(serviceCreateViewModel);
                createdservice.se_thumbnai = fileName;

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
        [Route("api/services/update/")]

        public async Task<IHttpActionResult> Updateservice(int? se_id)
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
                // save file
                string fileName = "";
                if (streamProvider.FileData.Count > 0)
                {
                    foreach (MultipartFileData fileData in streamProvider.FileData)
                    {
                        fileName = FileExtension.SaveFileOnDisk(fileData);
                    }
                }

                // get data from formdata
                ServiceUpdateViewModel serviceUpdateViewModel = new ServiceUpdateViewModel
                {
                    se_id = Convert.ToInt32(streamProvider.FormData["se_id"]),
                    se_code = Convert.ToString(streamProvider.FormData["se_code"]),
                    se_name = Convert.ToString(streamProvider.FormData["se_name"]),
                    se_description = Convert.ToString(streamProvider.FormData["se_description"]),




                    service_category_id = Convert.ToInt32(streamProvider.FormData["service_category_id"]),
                    se_price = Convert.ToInt32(streamProvider.FormData["se_price"]),
                    se_saleoff = Convert.ToInt32(streamProvider.FormData["se_saleoff"]),

                    se_type = Convert.ToByte(streamProvider.FormData["se_type"]),

                };


                var existservice = _serviceservice.Find(se_id);

                if (fileName != "")
                {
                    serviceUpdateViewModel.se_thumbnai = fileName;
                }
                else
                {

                    serviceUpdateViewModel.se_thumbnai = existservice.se_thumbnai;
                }

                // mapping view model to entity
                var updatedservice = _mapper.Map<service>(serviceUpdateViewModel);



                // update service
                _serviceservice.Update(updatedservice, se_id);

                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = updatedservice;
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
        [Route("api/services/delete")]
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