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

namespace ERP.API.Controllers.Dashboard
{
    public class ManagerNotificationController : ApiController
    {
        private readonly INotificationService _notificationservice;

        private readonly IMapper _mapper;

        public ManagerNotificationController() { }
        public ManagerNotificationController(INotificationService notificationservice, IMapper mapper)
        {
            this._notificationservice = notificationservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/notifications/all")]
        public IHttpActionResult Getnotifications()
        {
            ResponseDataDTO<IEnumerable<notification>> response = new ResponseDataDTO<IEnumerable<notification>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _notificationservice.GetAll();
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

        [Route("api/notifications/page")]
        public IHttpActionResult GetnotificationsPaging(int pageSize, int pageNumber)
        {
            ResponseDataDTO<PagedResults<notification>> response = new ResponseDataDTO<PagedResults<notification>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _notificationservice.CreatePagedResults(pageNumber, pageSize);
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

        [HttpPost]
        [Route("api/notifications/create")]

        public async Task<IHttpActionResult> Createnotification()
        {
            ResponseDataDTO<notification> response = new ResponseDataDTO<notification>();
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
                NotificationCreateViewModel notificationCreateViewModel = new NotificationCreateViewModel
                {
                    ntf_title = Convert.ToString(streamProvider.FormData["ntf_title"]),
                    ntf_description = Convert.ToString(streamProvider.FormData["ntf_description"]),

                    ntf_datetime = Convert.ToDateTime(streamProvider.FormData["ntf_datetime"]),
                    ntf_confim_datetime = Convert.ToDateTime(streamProvider.FormData["ntf_confim_datetime"]),

                    staff_id = Convert.ToInt32(streamProvider.FormData["staff_id"]),

                };

                // mapping view model to entity
                var creatednotification = _mapper.Map<notification>(notificationCreateViewModel);


                // save new notification
                _notificationservice.Create(creatednotification);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = creatednotification;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = MessageResponse.FAIL;
                response.Data = null;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }

        }




        [HttpPut]
        [Route("api/notifications/update")]

        public async Task<IHttpActionResult> Updatenotification(int? ntf_id)
        {
            ResponseDataDTO<notification> response = new ResponseDataDTO<notification>();
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
                NotificationUpdateViewModel notificationUpdateViewModel = new NotificationUpdateViewModel
                {
                    ntf_id = Convert.ToInt32(streamProvider.FormData["ntf_id"]),
                    ntf_title = Convert.ToString(streamProvider.FormData["ntf_title"]),
                    ntf_description = Convert.ToString(streamProvider.FormData["ntf_description"]),

                    ntf_datetime = Convert.ToDateTime(streamProvider.FormData["ntf_datetime"]),
                    ntf_confim_datetime = Convert.ToDateTime(streamProvider.FormData["ntf_confim_datetime"]),

                    staff_id = Convert.ToInt32(streamProvider.FormData["staff_id"]),

                };



                // mapping view model to entity
                var updatednotification = _mapper.Map<notification>(notificationUpdateViewModel);



                // update notification
                _notificationservice.Update(updatednotification, ntf_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = updatednotification;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = MessageResponse.FAIL;
                response.Data = null;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }
        }

        [HttpDelete]
        [Route("api/notifications/delete/{notificationId}")]
        public IHttpActionResult Deletenotification(int notificationId)
        {
            ResponseDataDTO<notification> response = new ResponseDataDTO<notification>();
            try
            {
                var notificationDeleted = _notificationservice.Find(notificationId);
                if (notificationDeleted != null)
                {
                    _notificationservice.Delete(notificationDeleted);

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
                response.Message = MessageResponse.FAIL;
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
                _notificationservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}