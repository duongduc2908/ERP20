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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ERP.API.Controllers.Dashboard
{
    [EnableCors("*", "*", "*")]
    [Authorize]
    public class ManagerCustomerController : ApiController
    {
        private readonly ICustomerService _customerservice;

        private readonly IMapper _mapper;

        public ManagerCustomerController()
        {

        }
        public ManagerCustomerController(ICustomerService customerservice, IMapper mapper)
        {
            this._customerservice = customerservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/customer/all")]
        public IHttpActionResult Getcustomers()
        {
            ResponseDataDTO<IEnumerable<customer>> response = new ResponseDataDTO<IEnumerable<customer>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customerservice.GetAll();
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
        [Route("api/customers/page")]
        public IHttpActionResult GetcustomersPaging(int pageSize, int pageNumber)
        {
            ResponseDataDTO<PagedResults<customer>> response = new ResponseDataDTO<PagedResults<customer>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customerservice.CreatePagedResults(pageNumber, pageSize);
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
        [Route("api/customers/search")]
        public IHttpActionResult GetInforByName(string search_name)
        {
            ResponseDataDTO<PagedResults<customer>> response = new ResponseDataDTO<PagedResults<customer>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customerservice.GetInfor(search_name);
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
        [Route("api/customers/create")]

        public async Task<IHttpActionResult> Createcustomer()
        {
            ResponseDataDTO<customer> response = new ResponseDataDTO<customer>();
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
                CustomerCreateViewModel customerCreateViewModel = new CustomerCreateViewModel
                {
                    cu_code = Convert.ToString(streamProvider.FormData["cu_code"]),
                    cu_mobile = Convert.ToString(streamProvider.FormData["cu_mobile"]),
                    cu_email = Convert.ToString(streamProvider.FormData["cu_email"]),
                    cu_fullname = Convert.ToString(streamProvider.FormData["cu_fullname"]),
                    cu_address = Convert.ToString(streamProvider.FormData["cu_address"]),
                    cu_note = Convert.ToString(streamProvider.FormData["cu_note"]),


                    cu_geocoding = Convert.ToString(streamProvider.FormData["cu_geocoding"]),
                    customer_group_id = Convert.ToInt32(streamProvider.FormData["customer_group_id"]),
                    source_id = Convert.ToInt32(streamProvider.FormData["source_id"]),
                    staff_id = Convert.ToInt32(streamProvider.FormData["staff_id"]),
                    cu_age = Convert.ToInt32(streamProvider.FormData["cu_age"]),
                    
                    cu_create_date = Convert.ToDateTime(streamProvider.FormData["cu_create_date"]),

                    cu_type = Convert.ToByte(streamProvider.FormData["cu_type"]),
                    cu_status = Convert.ToByte(streamProvider.FormData["cu_status"]),


                };
                //md5

                if (CheckEmail.IsValidEmail(customerCreateViewModel.cu_email) == false && customerCreateViewModel.cu_email == "")
                {
                    response.Message = "Định dạng email không hợp lệ !";
                    response.Data = null;
                    return Ok(response);
                }
                //check_phone_number
                if (CheckNumber.IsPhoneNumber(customerCreateViewModel.cu_mobile) == false && customerCreateViewModel.cu_mobile == "")
                {
                    response.Message = "Số điện thoại không hợp lệ";
                    response.Data = null;
                    return Ok(response);
                }
                // mapping view model to entity
                var createdcustomer = _mapper.Map<customer>(customerCreateViewModel);
                createdcustomer.cu_thumbnail = fileName;

                // save new customer
                _customerservice.Create(createdcustomer);

                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = createdcustomer;
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
        [Route("api/customers/update/")]

        public async Task<IHttpActionResult> Updatecustomer(int? cu_id)
        {
            ResponseDataDTO<customer> response = new ResponseDataDTO<customer>();
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
                CustomerUpdateViewModel customerUpdateViewModel = new CustomerUpdateViewModel
                {
                    cu_id = Convert.ToInt32(streamProvider.FormData["cu_id"]),
                    cu_code = Convert.ToString(streamProvider.FormData["cu_code"]),
                    cu_mobile = Convert.ToString(streamProvider.FormData["cu_mobile"]),
                    cu_email = Convert.ToString(streamProvider.FormData["cu_email"]),
                    cu_fullname = Convert.ToString(streamProvider.FormData["cu_fullname"]),
                    cu_address = Convert.ToString(streamProvider.FormData["cu_address"]),
                    cu_note = Convert.ToString(streamProvider.FormData["cu_note"]),


                    cu_geocoding = Convert.ToString(streamProvider.FormData["cu_geocoding"]),
                    customer_group_id = Convert.ToInt32(streamProvider.FormData["customer_group_id"]),
                    source_id = Convert.ToInt32(streamProvider.FormData["source_id"]),
                    staff_id = Convert.ToInt32(streamProvider.FormData["staff_id"]),
                    cu_age = Convert.ToInt32(streamProvider.FormData["cu_age"]),


                    cu_create_date = Convert.ToDateTime(streamProvider.FormData["cu_create_date"]),

                    cu_type = Convert.ToByte(streamProvider.FormData["cu_type"]),
                    cu_status = Convert.ToByte(streamProvider.FormData["cu_status"]),

                };


                var existcustomer = _customerservice.Find(cu_id);

                if (fileName != "")
                {
                    customerUpdateViewModel.cu_thumbnail = fileName;
                }
                else
                {

                    customerUpdateViewModel.cu_thumbnail = existcustomer.cu_thumbnail;
                }
                //md5

                if (CheckEmail.IsValidEmail(customerUpdateViewModel.cu_email) == false && customerUpdateViewModel.cu_email == "")
                {
                    response.Message = "Định dạng email không hợp lệ !";
                    response.Data = null;
                    return Ok(response);
                }
                //check_phone_number
                if (CheckNumber.IsPhoneNumber(customerUpdateViewModel.cu_mobile) == false && customerUpdateViewModel.cu_mobile == "")
                {
                    response.Message = "Số điện thoại không hợp lệ";
                    response.Data = null;
                    return Ok(response);
                }
                // mapping view model to entity
                var updatedcustomer = _mapper.Map<customer>(customerUpdateViewModel);



                // update customer
                _customerservice.Update(updatedcustomer, cu_id);

                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = updatedcustomer;
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
        [Route("api/customers/delete/{customerId}")]
        public IHttpActionResult Deletecustomer(int customerId)
        {
            ResponseDataDTO<customer> response = new ResponseDataDTO<customer>();
            try
            {
                var customerDeleted = _customerservice.Find(customerId);
                if (customerDeleted != null)
                {
                    _customerservice.Delete(customerDeleted);

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
                _customerservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}