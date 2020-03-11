using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Constants.Enums;
using ERP.Common.Excel;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.ExportDB;
using ERP.Data.ModelsERP.ModelView.Sms;
using ERP.Extension.Extensions;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ERP.API.Controllers.Dashboard
{
    [EnableCors("*", "*", "*")]
    //[Authorize]
    public class ManagerCustomerController : BaseController
    {
        private readonly ICustomerService _customerservice;
        private readonly ICustomerGroupService _customergroupservice;
        private readonly ISourceService _sourceservice;
        private readonly IShipAddressService _shipaddressservice;

        private readonly IMapper _mapper;

        public ManagerCustomerController()
        {

        }
        public ManagerCustomerController(ICustomerService customerservice, IShipAddressService shipaddressservice, IMapper mapper)
        {
            this._customerservice = customerservice;
            this._shipaddressservice = shipaddressservice;
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
        #endregion

        #region [Get All Page]
        [HttpGet]
        [Route("api/customers/page")]
        public IHttpActionResult GetcustomersPaging(int pageSize, int pageNumber)
        {
            ResponseDataDTO<PagedResults<customerviewmodel>> response = new ResponseDataDTO<PagedResults<customerviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customerservice.GetAllPage(pageNumber, pageSize);
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

        #region [Get Infor by Id]
        [HttpGet]
        [Route("api/customers/infor")]
        public IHttpActionResult GetIforId(int cu_id)
        {
            ResponseDataDTO<customerviewmodel> response = new ResponseDataDTO<customerviewmodel>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customerservice.GetInfor(cu_id);
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

        #region [Search source, type, group, name Page]
        [HttpGet]
        [Route("api/customers/search")]
        public IHttpActionResult GetInforByName(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, string name)
        {
            ResponseDataDTO<PagedResults<customerviewmodel>> response = new ResponseDataDTO<PagedResults<customerviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customerservice.GetAllPageSearch(pageNumber, pageSize, source_id, cu_type, customer_group_id, name);
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
        [Route("api/customers-sms/search")]
        public IHttpActionResult GetSmsInforByName(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, string name)
        {
            ResponseDataDTO<PagedResults<smscustomerviewmodel>> response = new ResponseDataDTO<PagedResults<smscustomerviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customerservice.GetAllPageSearchSms(pageNumber, pageSize, source_id, cu_type, customer_group_id, name);
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

        #region [Get Type]
        [HttpGet]
        [Route("api/customers/type")]
        public IHttpActionResult GetAllType()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {

                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customerservice.GetAllType();
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

        #region[Create Customer]
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
                //Cach truong bat buoc 
                if (streamProvider.FormData["cu_fullname"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Họ và tên không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["cu_mobile"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Số điện thoại không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["cu_email"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Email không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["cu_type"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Loại khách hàng không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["customer_group_id"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Nhóm khách hàng không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["source_id"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Nguồn không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                // get data from formdata
                CustomerCreateViewModel customerCreateViewModel = new CustomerCreateViewModel
                {
                    cu_mobile = Convert.ToString(streamProvider.FormData["cu_mobile"]),
                    cu_email = Convert.ToString(streamProvider.FormData["cu_email"]),
                    cu_fullname = Convert.ToString(streamProvider.FormData["cu_fullname"]),
                    
                    customer_group_id = Convert.ToInt32(streamProvider.FormData["customer_group_id"]),
                    source_id = Convert.ToInt32(streamProvider.FormData["source_id"]),
                    
                    cu_type = Convert.ToByte(streamProvider.FormData["cu_type"]),
                    
                };
                //Bat cac dieu kien rang buoc
                if (CheckEmail.IsValidEmail(customerCreateViewModel.cu_email) == false && customerCreateViewModel.cu_email == "")
                {
                    response.Message = "Định dạng email không hợp lệ !";
                    response.Data = null;
                    return Ok(response);
                }
               
                if (CheckNumber.IsPhoneNumber(customerCreateViewModel.cu_mobile) == false && customerCreateViewModel.cu_mobile == "")
                {
                    response.Message = "Số điện thoại không hợp lệ";
                    response.Data = null;
                    return Ok(response);
                }
                

                //bat cac truog con lai 
                if (streamProvider.FormData["cu_birthday"] == null)
                {
                    customerCreateViewModel.cu_birthday = null;
                }
                else
                {
                    customerCreateViewModel.cu_birthday = Convert.ToDateTime(streamProvider.FormData["cu_birthday"]);
                }
                if (streamProvider.FormData["cu_address"] == null)
                {
                    customerCreateViewModel.cu_address = null;
                }
                else
                {
                    customerCreateViewModel.cu_address = Convert.ToString(streamProvider.FormData["cu_address"]);
                }
                if (streamProvider.FormData["cu_note"] == null)
                {
                    customerCreateViewModel.cu_note = null;
                }
                else
                {
                    customerCreateViewModel.cu_note = Convert.ToString(streamProvider.FormData["cu_note"]);
                }
                if (streamProvider.FormData["cu_geocoding"] == null)
                {
                    customerCreateViewModel.cu_geocoding = null;
                }
                else
                {
                    customerCreateViewModel.cu_geocoding = Convert.ToString(streamProvider.FormData["cu_geocoding"]);
                }
                if (streamProvider.FormData["cu_curator_id"] == null)
                {
                    customerCreateViewModel.cu_curator_id = null;
                }
                else
                {
                    customerCreateViewModel.cu_curator_id = Convert.ToInt32(streamProvider.FormData["cu_curator_id"]);
                }
                if (streamProvider.FormData["cu_age"] == null)
                {
                    customerCreateViewModel.cu_age = null;
                }
                else
                {
                    customerCreateViewModel.cu_age = Convert.ToInt32(streamProvider.FormData["cu_age"]);
                }
                if (streamProvider.FormData["cu_status"] == null)
                {
                    customerCreateViewModel.cu_status = null;
                }
                else
                {
                    customerCreateViewModel.cu_status = Convert.ToByte(streamProvider.FormData["cu_status"]);
                }
                var current_id = BaseController.get_id_current();
                customerCreateViewModel.staff_id = Convert.ToInt32(current_id);
                customerCreateViewModel.cu_create_date = DateTime.Now;
                var cu = _customerservice.GetLast();
                if(cu == null) customerCreateViewModel.cu_code = Utilis.CreateCode("CU", 0, 7);
                else customerCreateViewModel.cu_code = Utilis.CreateCode("CU", cu.cu_id, 7);
                // mapping view model to entity
                var createdcustomer = _mapper.Map<customer>(customerCreateViewModel);
                // save file
                string fileName = "";
                foreach (MultipartFileData fileData in streamProvider.FileData)
                {
                    fileName = (FileExtension.SaveFileCustomerOnDisk(fileData, createdcustomer.cu_code));
                }
                if (fileName == null)
                {
                    createdcustomer.cu_thumbnail = "/Uploads/Images/default/customer.png";
                }
                else createdcustomer.cu_thumbnail = fileName;
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
        #endregion

        #region[Update]

        [HttpPut]
        [Route("api/customers/update/")]

        public async Task<IHttpActionResult> Updatecustomer()
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
                //Cach truong bat buoc 
                if (streamProvider.FormData["cu_fullname"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Họ và tên không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["cu_mobile"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Số điện thoại không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["cu_email"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Email không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["cu_type"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Loại khách hàng không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["customer_group_id"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Nhóm khách hàng không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["source_id"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Nguồn không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                // get data from formdata
                CustomerUpdateViewModel customerUpdateViewModel = new CustomerUpdateViewModel
                {
                    cu_id = Convert.ToInt32(streamProvider.FormData["cu_id"]),
                    cu_mobile = Convert.ToString(streamProvider.FormData["cu_mobile"]),
                    cu_email = Convert.ToString(streamProvider.FormData["cu_email"]),
                    cu_fullname = Convert.ToString(streamProvider.FormData["cu_fullname"]),

                    customer_group_id = Convert.ToInt32(streamProvider.FormData["customer_group_id"]),
                    source_id = Convert.ToInt32(streamProvider.FormData["source_id"]),

                    cu_type = Convert.ToByte(streamProvider.FormData["cu_type"]),
                };


                var existcustomer = _customerservice.Find(customerUpdateViewModel.cu_id);
                customerUpdateViewModel.cu_code = existcustomer.cu_code;
                customerUpdateViewModel.cu_create_date = existcustomer.cu_create_date;
                if(streamProvider.FormData["cu_thumbnail"] != null)
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
                //bat cac truog con lai 
                if (streamProvider.FormData["cu_birthday"] == null)
                {
                    if(existcustomer.cu_birthday != null)
                    {
                        customerUpdateViewModel.cu_birthday = existcustomer.cu_birthday;
                    }
                    else
                    {
                        customerUpdateViewModel.cu_birthday = null;
                    }
                    
                }
                else
                {
                    customerUpdateViewModel.cu_birthday = Convert.ToDateTime(streamProvider.FormData["cu_birthday"]);
                }
                if (streamProvider.FormData["cu_address"] == null)
                {
                    customerUpdateViewModel.cu_address = null;
                }
                else
                {
                    customerUpdateViewModel.cu_address = Convert.ToString(streamProvider.FormData["cu_address"]);
                }
                if (streamProvider.FormData["cu_note"] == null)
                {
                    customerUpdateViewModel.cu_note = null;
                }
                else
                {
                    customerUpdateViewModel.cu_note = Convert.ToString(streamProvider.FormData["cu_note"]);
                }
                if (streamProvider.FormData["cu_geocoding"] == null)
                {
                    customerUpdateViewModel.cu_geocoding = null;
                }
                else
                {
                    customerUpdateViewModel.cu_geocoding = Convert.ToString(streamProvider.FormData["cu_geocoding"]);
                }
                if (streamProvider.FormData["cu_curator_id"] == null)
                {
                    customerUpdateViewModel.cu_curator_id = null;
                }
                else
                {
                    customerUpdateViewModel.cu_curator_id = Convert.ToInt32(streamProvider.FormData["cu_curator_id"]);
                }
                if (streamProvider.FormData["cu_age"] == null)
                {
                    customerUpdateViewModel.cu_age = null;
                }
                else
                {
                    customerUpdateViewModel.cu_age = Convert.ToInt32(streamProvider.FormData["cu_age"]);
                }


                if (streamProvider.FormData["cu_status"] == null)
                {
                    customerUpdateViewModel.cu_status = null;
                }
                else
                {
                    customerUpdateViewModel.cu_status = Convert.ToByte(streamProvider.FormData["cu_status"]);
                }

                // mapping view model to entity
                var updatedcustomer = _mapper.Map<customer>(customerUpdateViewModel);


                // update customer
                _customerservice.Update(updatedcustomer, customerUpdateViewModel.cu_id);

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
        #endregion

        #region[Delete]
        [HttpDelete]
        [Route("api/customers/delete")]
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

        #region["Import Excel"]
        [HttpPost]
        [Route("api/customer/import")]
        public async Task<IHttpActionResult> Import()
        {
            ResponseDataDTO<customer> response = new ResponseDataDTO<customer>();
            var exitsData = "";
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
                        fileName = fileData.Headers.ContentDisposition.FileName;
                        //fileName = fileName.Replace(@"","");

                        string fileFormat = Utilis.GetFileFormat(fileName);
                        if (fileFormat.Equals("xlsm") || fileFormat.Equals("xlsx"))
                        {
                            fileName = FileExtension.SaveFileCustomerOnDiskExcel(fileData,"Test",BaseController.folder(),BaseController.get_timestamp());
                        }
                        else
                        {
                            throw new Exception("File excel import không hợp lệ!");
                        }

                    }
                }
                var list = new List<customer>();
                fileName = "C:/inetpub/wwwroot/coerp" + fileName;
                //fileName = "D:/ERP20/ERP.API" + fileName;
                var dataset = ExcelImport.ImportExcelXLS(fileName, true);
                DataTable table = (DataTable)dataset.Tables[7];
                if (table != null && table.Rows.Count > 0)
                {
                    if (table.Columns.Count != 11)
                    {
                        exitsData = "File excel import không hợp lệ!";
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = exitsData;
                        response.Data = null;
                        return Ok(response);
                    }
                    else
                    {
                        #region["Check null"]
                        foreach (DataRow dr in table.Rows)
                        {
                            if (dr.IsNull("cu_fullname"))
                            {
                                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                                response.Message = "Họ và tên không được để trống";
                                response.Data = null;
                                return Ok(response);
                            }
                            if (dr.IsNull("cu_type"))
                            {
                                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                                response.Message = "Loai khach hang khong duoc de trong !";
                                response.Data = null;
                                return Ok(response);
                            }

                            if (dr.IsNull("cu_mobile"))
                            {
                                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                                response.Message = "Số điện thoại không được để trống";
                                response.Data = null;
                                return Ok(response);
                            }

                            if (dr.IsNull("cu_address"))
                            {
                                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                                response.Message = "Dia chi khong duoc de trong! ";
                                response.Data = null;
                                return Ok(response);
                            }

                            if (dr.IsNull("customer_group_id"))
                            {
                                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                                response.Message = "Nhom khach hang không được để trống";
                                response.Data = null;
                                return Ok(response);
                            }
                            if (dr.IsNull("source_id"))
                            {
                                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                                response.Message = "Nguon khach hang không được để trống";
                                response.Data = null;
                                return Ok(response);
                            }
                        }
                        #endregion

                        #region["Check duplicate"]
                        for (var i = 0; i < table.Rows.Count; i++)
                        {
                            var sdtCur = table.Rows[i]["cu_mobile"].ToString().Trim();
                            var emailCur = table.Rows[i]["cu_email"].ToString().Trim();
                            for (var j = 0; j < table.Rows.Count; j++)
                            {
                                if (i != j)
                                {
                                    var _sdtCur = table.Rows[j]["cu_mobile"].ToString().Trim();
                                    var _emailCur = table.Rows[j]["cu_email"].ToString().Trim();
                                    if (sdtCur.Equals(_sdtCur))
                                    {
                                        exitsData = "So dien thoai '" + sdtCur + "' bị lặp trong file excel!";
                                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                                        response.Message = exitsData;
                                        response.Data = null;
                                        return Ok(response);
                                    }
                                    if (emailCur.Equals(_emailCur))
                                    {
                                        exitsData = "Email '" + emailCur + "' bị lặp trong file excel!";
                                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                                        response.Message = exitsData;
                                        response.Data = null;
                                        return Ok(response);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region["Check logic"]
                        foreach (DataRow dr in table.Rows)
                        {
                            int i = 1;
                            if (i == 2)
                            {
                                if (!check_type(Convert.ToInt32(dr["cu_type"])))
                                {
                                    exitsData = "Khong co ma phong ban trong csdl!";
                                    response.Code = HttpCode.NOT_FOUND;
                                    response.Message = exitsData;
                                    response.Data = null;
                                    return Ok(response);
                                }
                                if (!check_group(Convert.ToInt32(dr["customer_group_id"])))
                                {
                                    exitsData = "Khong co ma bo phan trong csdl!";
                                    response.Code = HttpCode.NOT_FOUND;
                                    response.Message = exitsData;
                                    response.Data = null;
                                    return Ok(response);
                                }
                                if (!check_source(Convert.ToInt32(dr["source_id"])))
                                {
                                    exitsData = "Khong co ma nhom quyen trong csdl!";
                                    response.Code = HttpCode.NOT_FOUND;
                                    response.Message = exitsData;
                                    response.Data = null;
                                    return Ok(response);
                                }
                            }
                            i++;
                        }
                        #endregion
                    }
                    list = DataTableCmUtils.ToListof<customer>(table);
                    // Gọi hàm save data
                    foreach (customer i in list)
                    {
                        var x = _customerservice.GetLast();
                        if(x == null) i.cu_code = Utilis.CreateCode("CU", 0, 7);
                        else i.cu_code = Utilis.CreateCode("CU", x.cu_id, 7);
                        i.cu_thumbnail = "/Uploads/Images/default/customer.png";
                        _customerservice.Create(i);
                    }
                    exitsData = "Đã nhập dữ liệu excel thành công!";
                    response.Code = HttpCode.OK;
                    response.Message = exitsData;
                    response.Data = null;
                    return Ok(response);
                }
                else
                {
                    exitsData = "File excel import không có dữ liệu!";
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = exitsData;
                    response.Data = null;
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message; ;
                response.Data = null;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }
        }
        #endregion

        #region["Common funtion"]
        private bool check_type(int _id)
        {
            bool res;
            if (_id <= EnumCustomer.cu_type.Length)
                res = true;
            else res = false;
            return res;
        }

        private bool check_group(int _id)
        {
            bool res = _customergroupservice.Exist(x => x.cg_id == _id);
            return res;
        }

        private bool check_source(int _id)
        {
            bool res = _sourceservice.Exist(x => x.src_id == _id);
            return res;
        }
        #endregion
        #region["Export Excel"]
        [HttpGet]
        [Route("api/customers/export")]
        public async Task<IHttpActionResult> Export(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, string name)
        {
            ResponseDataDTO<customerview> response = new ResponseDataDTO<customerview>();
            try
            {
                var listStaff = new List<customerview>();

                //Đưa ra danh sách staff trong trang nào đó 
                var objRT_Mst_Staff = _customerservice.ExportCustomer(pageNumber, pageSize, source_id, cu_type, customer_group_id, name);
                if (objRT_Mst_Staff != null)
                {
                    listStaff.AddRange(objRT_Mst_Staff.Results);

                    Dictionary<string, string> dicColNames = GetImportDicColums();

                    string url = "";
                    string filePath = GenExcelExportFilePath(string.Format(typeof(department).Name), ref url);

                    ExcelExport.ExportToExcelFromList(listStaff, dicColNames, filePath, string.Format("Customers"));

                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = "Đã xuất excel thành công!";
                    response.Data = null;
                }
                else
                {
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = "File excel import không có dữ liệu!";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message; ;
                response.Data = null;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }
            return Ok(response);
        }
        #endregion

        #region["DicColums"]
        private Dictionary<string, string> GetImportDicColums()
        {
            return new Dictionary<string, string>()
            {
                 {"cu_code","MKH"},
                 {"cu_fullname","Họ và tên "},
                 {"cu_mobile","SĐT" },
                 {"cu_email","Email"},
                 {"cu_type_name","Loại khách hàng"},
                 {"source_name","Nguồn khách hàng"},
                 {"customer_group_name","Nhóm khách hàng"},
                 {"cu_address","Địa chỉ"},
                 {"cu_birthday","Ngày sinh"},
                 {"cu_curator_name","Người phụ trách"},
                 {"staff_name","Người tạo"},
                 {"cu_note","Chú ý"},
                 {"cu_create_date","Ngày tạo"},
                 {"cu_status_name","Trạng thái"},

            };
        }
        private Dictionary<string, string> GetImportDicColumsTemplate()
        {
            return new Dictionary<string, string>()
            {
                  {"email","Email phong ban"},
                 {"id","Ma bộ phận phòng ban"}
            };
        }
        #endregion

       

        #region["Update Avatar"]
        [HttpPut]
        [Route("api/customers/update_avatar")]
        public async Task<IHttpActionResult> UpdateAvatar()
        {
            ResponseDataDTO<string> response = new ResponseDataDTO<string>();
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
                if (streamProvider.FormData["cu_id"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Ma id không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                var cu_id = int.Parse(streamProvider.FormData["cu_id"]);
                var customer = _customerservice.Find(cu_id);
                if (customer == null)
                {
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = MessageResponse.FAIL;
                    response.Data = null;
                    return Ok(response);
                }
                foreach (MultipartFileData fileData in streamProvider.FileData)
                {
                    fileName = FileExtension.SaveFileCustomerOnDisk(fileData, customer.cu_code);
                }
                customer.cu_thumbnail = fileName;
                _customerservice.Update(customer, cu_id);
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = fileName;
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