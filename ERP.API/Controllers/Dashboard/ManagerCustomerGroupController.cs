using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.CustomerGroup;
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
    public class ManagerCustomerGroupController : ApiController
    {
        private readonly ICustomerGroupService _customer_groupservice;

        private readonly IMapper _mapper;

        public ManagerCustomerGroupController() { }
        public ManagerCustomerGroupController(ICustomerGroupService customer_groupservice, IMapper mapper)
        {
            this._customer_groupservice = customer_groupservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/customer-groups/search")]
        public IHttpActionResult Getcustomer_groupsPaging(int pageSize, int pageNumber, int? cg_id, string name)
        {
            ResponseDataDTO<PagedResults<customergroupviewmodel>> response = new ResponseDataDTO<PagedResults<customergroupviewmodel>>();
            try
            {
                int company_id = BaseController.get_company_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customer_groupservice.GetAllPageSearch(pageNumber, pageSize, cg_id, name,company_id);
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
        [Route("api/customer-groups/info")]
        public IHttpActionResult GetById(int cg_id)
        {
            ResponseDataDTO<customergroupviewmodel> response = new ResponseDataDTO<customergroupviewmodel>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customer_groupservice.GetById(cg_id);
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
        [Route("api/customer_group/getall")]
        public IHttpActionResult GetAllDropdown()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                int company_id = BaseController.get_company_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customer_groupservice.GetAllDropdown(company_id);
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
        [Route("api/customer-groups/get-pie-chart")]
        public IHttpActionResult GetPieChart()
        {
            ResponseDataDTO<List<piechartview>> response = new ResponseDataDTO<List<piechartview>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customer_groupservice.GetPieChart();
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

       
        #region [Create]
        [HttpPost]
        [Route("api/customer-group/create")]

        public async Task<IHttpActionResult> CreateCustomerGroup()
        {
            ResponseDataDTO<customer_group> response = new ResponseDataDTO<customer_group>();
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }
;
                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);
                
                //Các trường bắt buộc 
                if (streamProvider.FormData["cg_name"] == null )
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên nhóm không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                // get data from formdata
                CustomerGroupCreateViewModel customerGroupCreateViewModel = new CustomerGroupCreateViewModel
                {
                    cg_name = Convert.ToString(streamProvider.FormData["cg_name"]),

                };
                if (streamProvider.FormData["cg_description"] == null)
                {
                    customerGroupCreateViewModel.cg_description = null;
                }
                else
                {
                    customerGroupCreateViewModel.cg_description = Convert.ToString(streamProvider.FormData["cg_description"]);
                }

                //Create date
                customerGroupCreateViewModel.cg_created_date = DateTime.Now;
                customerGroupCreateViewModel.staff_id = BaseController.get_id_current();
                if(_customer_groupservice.CheckUniqueName(streamProvider.FormData["cg_name"], 0) == false)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên nhóm "+ Convert.ToString(streamProvider.FormData["cg_name"])+" đã có trong hệ thống.";
                    response.Data = null;
                    return Ok(response);
                }

                // mapping view model to entity
                var create_customer_group = _mapper.Map<customer_group>(customerGroupCreateViewModel);
                // save file
                string fileName = "";
                foreach (MultipartFileData fileData in streamProvider.FileData)
                {
                    fileName = (FileExtension.SaveFileCustomerGroupOnDisk(fileData, "CG"+Convert.ToString(create_customer_group.cg_id)));
                }
                if (fileName == null)
                {
                    create_customer_group.cg_thumbnail = "/Uploads/Images/default/customergroup.png";
                }
                else create_customer_group.cg_thumbnail = fileName;


                create_customer_group.company_id = BaseController.get_company_id_current();
                // save new product
                _customer_groupservice.Create(create_customer_group);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = create_customer_group;
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
        #region [Create]
        [HttpPut]
        [Route("api/customer-group/update")]

        public async Task<IHttpActionResult> UpdateCustomerGroup()
        {
            ResponseDataDTO<customer_group> response = new ResponseDataDTO<customer_group>();
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);
                string fileName = "";
                
                //Các trường bắt buộc 
                if (streamProvider.FormData["cg_name"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên nhóm không được để trống";
                    response.Data = null;
                    return Ok(response);
                }




                // get data from formdata
                CustomerGroupUpdateViewModel customerGroupUpdateViewModel = new CustomerGroupUpdateViewModel
                {
                    cg_name = Convert.ToString(streamProvider.FormData["cg_name"]),
                    cg_id = Convert.ToInt32(streamProvider.FormData["cg_id"]),


                };
                if (streamProvider.FormData["cg_description"] == null)
                {
                    customerGroupUpdateViewModel.cg_description = null;
                }
                else
                {
                    customerGroupUpdateViewModel.cg_description = Convert.ToString(streamProvider.FormData["cg_description"]);
                }
                foreach (MultipartFileData fileData in streamProvider.FileData)
                {
                    fileName = (FileExtension.SaveFileCustomerGroupOnDisk(fileData, "CG" + Convert.ToString(customerGroupUpdateViewModel.cg_id)));
                }
                //Create date
                customerGroupUpdateViewModel.cg_created_date = DateTime.Now;
                customerGroupUpdateViewModel.staff_id = BaseController.get_id_current();
                var existscg = _customer_groupservice.Find(customerGroupUpdateViewModel.cg_id);

                if (fileName != "")
                {
                    customerGroupUpdateViewModel.cg_thumbnail = fileName;
                }
                else
                {
                    customerGroupUpdateViewModel.cg_thumbnail = existscg.cg_thumbnail;
                }
                
                if (_customer_groupservice.CheckUniqueName(streamProvider.FormData["cg_name"], customerGroupUpdateViewModel.cg_id) == false)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên nhóm "+Convert.ToString(streamProvider.FormData["cg_name"])+" đã có trong hệ thống.";
                    response.Data = null;
                    return Ok(response);
                }

                // mapping view model to entity
                var update_customer_group = _mapper.Map<customer_group>(customerGroupUpdateViewModel);
                // save file



                // save new product
                _customer_groupservice.Update(update_customer_group, update_customer_group.cg_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = update_customer_group;
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

        [HttpDelete]
        [Route("api/customer-group/delete")]
        public IHttpActionResult Deletestaff(int cg_id)
        {
            ResponseDataDTO<staff> response = new ResponseDataDTO<staff>();
            try
            {
                var cgDeleted = _customer_groupservice.Find(cg_id);
                if (cgDeleted != null)
                {
                    _customer_groupservice.Delete(cgDeleted);

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
                    response.Message = "Không có mã nhóm khách hàng "+cg_id.ToString()+" trong hệ thống.";
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
                _customer_groupservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}