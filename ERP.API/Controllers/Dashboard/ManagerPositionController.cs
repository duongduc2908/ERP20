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
    public class ManagerPositionController : ApiController
    {
        private readonly IPositionService _positionservice;

        private readonly IMapper _mapper;

        public ManagerPositionController() { }
        public ManagerPositionController(IPositionService positionservice, IMapper mapper)
        {
            this._positionservice = positionservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/positions/all")]
        public IHttpActionResult Getpositions()
        {
            ResponseDataDTO<IEnumerable<position>> response = new ResponseDataDTO<IEnumerable<position>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _positionservice.GetAll();
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

        [Route("api/positions/page")]
        public IHttpActionResult GetpositionsPaging(int pageSize, int pageNumber)
        {
            ResponseDataDTO<PagedResults<position>> response = new ResponseDataDTO<PagedResults<position>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _positionservice.CreatePagedResults(pageNumber, pageSize);
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
        [Route("api/positions/create")]

        public async Task<IHttpActionResult> Createposition()
        {
            ResponseDataDTO<position> response = new ResponseDataDTO<position>();
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
                PositionCreateViewModel positionCreateViewModel = new PositionCreateViewModel
                {
                    pos_name = Convert.ToString(streamProvider.FormData["pos_name"]),
                    pos_competence = Convert.ToString(streamProvider.FormData["pos_competence"]),
                    pos_abilty = Convert.ToString(streamProvider.FormData["pos_abilty"]),
                    pos_authority = Convert.ToString(streamProvider.FormData["pos_authority"]),
                    pos_responsibility = Convert.ToString(streamProvider.FormData["pos_responsibility"]),
                    pos_description = Convert.ToString(streamProvider.FormData["pos_description"]),

                };

                // mapping view model to entity
                var createdposition = _mapper.Map<position>(positionCreateViewModel);


                // save new position
                _positionservice.Create(createdposition);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = createdposition;
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
        [Route("api/positions/update")]

        public async Task<IHttpActionResult> Updateposition(int? pos_id)
        {
            ResponseDataDTO<position> response = new ResponseDataDTO<position>();
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
                PositionUpdateViewModel positionUpdateViewModel = new PositionUpdateViewModel
                {
                    pos_id = Convert.ToInt32(streamProvider.FormData["pos_id"]),
                    pos_name = Convert.ToString(streamProvider.FormData["pos_name"]),
                    pos_competence = Convert.ToString(streamProvider.FormData["pos_competence"]),
                    pos_abilty = Convert.ToString(streamProvider.FormData["pos_abilty"]),
                    pos_authority = Convert.ToString(streamProvider.FormData["pos_authority"]),
                    pos_responsibility = Convert.ToString(streamProvider.FormData["pos_responsibility"]),
                    pos_description = Convert.ToString(streamProvider.FormData["pos_description"]),

                };



                // mapping view model to entity
                var updatedposition = _mapper.Map<position>(positionUpdateViewModel);



                // update position
                _positionservice.Update(updatedposition, pos_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = updatedposition;
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
        [Route("api/positions/delete/{positionId}")]
        public IHttpActionResult Deleteposition(int positionId)
        {
            ResponseDataDTO<position> response = new ResponseDataDTO<position>();
            try
            {
                var positionDeleted = _positionservice.Find(positionId);
                if (positionDeleted != null)
                {
                    _positionservice.Delete(positionDeleted);

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
                _positionservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}