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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ERP.API.Controllers.Dashboard
{
    [EnableCors("*", "*", "*")]
    [Authorize]
    public class ManagerGroupRoleController : ApiController
    {
        private readonly IGroupRoleService _group_roleservice;

        private readonly IMapper _mapper;

        public ManagerGroupRoleController() { }
        public ManagerGroupRoleController(IGroupRoleService group_roleservice, IMapper mapper)
        {
            this._group_roleservice = group_roleservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/group-roles/all")]
        public IHttpActionResult Getgroup_roles()
        {
            ResponseDataDTO<IEnumerable<group_role>> response = new ResponseDataDTO<IEnumerable<group_role>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _group_roleservice.GetAll();
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
        [Route("api/group-role/dropdown")]
        public IHttpActionResult GetDropdown()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _group_roleservice.GetDropdown();
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

        [Route("api/group-roles/page")]
        public IHttpActionResult Getgroup_rolesPaging(int pageSize, int pageNumber)
        {
            ResponseDataDTO<PagedResults<group_role>> response = new ResponseDataDTO<PagedResults<group_role>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _group_roleservice.CreatePagedResults(pageNumber, pageSize);
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
        [Route("api/group-roles/create")]

        public async Task<IHttpActionResult> Creategroup_role()
        {
            ResponseDataDTO<group_role> response = new ResponseDataDTO<group_role>();
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
                GroupRoleCreateViewModel group_roleCreateViewModel = new GroupRoleCreateViewModel
                {
                    gr_name = Convert.ToString(streamProvider.FormData["gr_name"]),
                    gr_description = Convert.ToString(streamProvider.FormData["gr_description"]),

                    gr_status = Convert.ToByte(streamProvider.FormData["gr_status"]),



                };

                // mapping view model to entity
                var createdgroup_role = _mapper.Map<group_role>(group_roleCreateViewModel);
                createdgroup_role.gr_thumbnail = fileName;

                // save new group_role
                _group_roleservice.Create(createdgroup_role);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = createdgroup_role;
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
        [Route("api/group-roles/update")]

        public async Task<IHttpActionResult> Updategroup_role(int? gr_id)
        {
            ResponseDataDTO<group_role> response = new ResponseDataDTO<group_role>();
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
                GroupRoleUpdateViewModel group_roleUpdateViewModel = new GroupRoleUpdateViewModel
                {
                    gr_id = Convert.ToInt32(streamProvider.FormData["gr_id"]),
                    gr_name = Convert.ToString(streamProvider.FormData["gr_name"]),
                    gr_description = Convert.ToString(streamProvider.FormData["gr_description"]),

                    gr_status = Convert.ToByte(streamProvider.FormData["gr_status"]),

                };
                var existstaff = _group_roleservice.Find(gr_id);
                if (fileName != "")
                {
                    group_roleUpdateViewModel.gr_thumbnail = fileName;
                }
                else
                {

                    group_roleUpdateViewModel.gr_thumbnail = existstaff.gr_thumbnail;
                }

                // mapping view model to entity
                var updatedgroup_role = _mapper.Map<group_role>(group_roleUpdateViewModel);



                // update group_role
                _group_roleservice.Update(updatedgroup_role, gr_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = updatedgroup_role;
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
        [Route("api/group-roles/delete")]
        public IHttpActionResult Deletegroup_role(int group_roleId)
        {
            ResponseDataDTO<group_role> response = new ResponseDataDTO<group_role>();
            try
            {
                var group_roleDeleted = _group_roleservice.Find(group_roleId);
                if (group_roleDeleted != null)
                {
                    _group_roleservice.Delete(group_roleDeleted);

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
                _group_roleservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}