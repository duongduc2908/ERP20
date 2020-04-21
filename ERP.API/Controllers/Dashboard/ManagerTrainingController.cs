using ERP.Data.Dto;
using AutoMapper;
using ERP.API.Models;
using ERP.API.Providers;
using ERP.Common.Constants;
using ERP.Common.Excel;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;

using System.Web.Http;
using System.Web.Http.Cors;
using ERP.Data.ModelsERP.ModelView;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net;
using ERP.Extension.Extensions;

namespace ERP.API.Controllers.Dashboard
{
    [EnableCors("*", "*", "*")]
    [Authorize]
    public class ManagerTrainingController : ApiController
    {
        // GET: ManagerCompany
        private readonly ITrainingService _trainingservice;

        private readonly IMapper _mapper;

        public ManagerTrainingController() { }
        public ManagerTrainingController(ITrainingService trainingservice, IMapper mapper)
        {
            this._trainingservice = trainingservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/training/getall")]
        public IHttpActionResult GetAllName()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _trainingservice.GetAllName();
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
        [HttpGet]
        [Route("api/training/search")]
        public IHttpActionResult Getcompanys(int pageNumber, int pageSize, string search_name)
        {
            ResponseDataDTO<PagedResults<training>> response = new ResponseDataDTO<PagedResults<training>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _trainingservice.GetAllSearch(pageNumber,pageSize,search_name);
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
        [HttpGet]
        [Route("api/training/get_by_id")]
        public IHttpActionResult GetById(int tn_id)
        {
            ResponseDataDTO<training> response = new ResponseDataDTO<training>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _trainingservice.GetById(tn_id);
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
        [Route("api/training/create")]

        public async Task<IHttpActionResult> Create()
        {
            ResponseDataDTO<training> response = new ResponseDataDTO<training>();
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);

                training create_training = new training();
                create_training.tn_name = Convert.ToString(streamProvider.FormData["tn_name"]);
                create_training.tn_content = Convert.ToString(streamProvider.FormData["tn_content"]);
                create_training.tn_start_date = Convert.ToDateTime(streamProvider.FormData["tn_start_date"]);
                create_training.tn_end_date = Convert.ToDateTime(streamProvider.FormData["tn_end_date"]);
                create_training.tn_purpose = Convert.ToString(streamProvider.FormData["tn_purpose"]);

                //Tạo mã code
                var x = _trainingservice.GetLast();
                if (x == null) create_training.tn_code = "DT000000";
                else create_training.tn_code = Utilis.CreateCodeByCode("DT", x.tn_code, 8);
                // save new group_role
                _trainingservice.Create(create_training);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = create_training;
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
        [Route("api/training/update")]

        public async Task<IHttpActionResult> Update()
        {
            ResponseDataDTO<training> response = new ResponseDataDTO<training>();
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);
                int tn_id = Convert.ToInt32(streamProvider.FormData["tn_id"]);
                training existstaff = _trainingservice.Find(tn_id);
                existstaff.tn_name = Convert.ToString(streamProvider.FormData["tn_name"]);
                existstaff.tn_content = Convert.ToString(streamProvider.FormData["tn_content"]);
                existstaff.tn_start_date = Convert.ToDateTime(streamProvider.FormData["tn_start_date"]);
                existstaff.tn_end_date = Convert.ToDateTime(streamProvider.FormData["tn_end_date"]);
                existstaff.tn_purpose = Convert.ToString(streamProvider.FormData["tn_purpose"]);


                // update group_role
                _trainingservice.Update(existstaff, existstaff.tn_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = existstaff;
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
        [Route("api/training/delete")]
        public IHttpActionResult Deletegroup_role(int tn_id)
        {
            ResponseDataDTO<group_role> response = new ResponseDataDTO<group_role>();
            try
            {
                var trainingDelete = _trainingservice.Find(tn_id);
                if (trainingDelete != null)
                {
                    _trainingservice.Delete(trainingDelete);

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
    }
}