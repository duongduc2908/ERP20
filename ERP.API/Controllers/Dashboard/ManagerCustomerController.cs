using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Constants.Enums;
using ERP.Common.Excel;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Customer;
using ERP.Data.ModelsERP.ModelView.ExportDB;
using ERP.Data.ModelsERP.ModelView.Service;
using ERP.Data.ModelsERP.ModelView.Sms;
using ERP.Data.ModelsERP.ModelView.Transaction;
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
    [Authorize]
    public class ManagerCustomerController : BaseController
    {
        private readonly ICustomerService _customerservice;
        private readonly ICustomerGroupService _customergroupservice;
        private readonly ISourceService _sourceservice;
        private readonly IShipAddressService _shipaddressservice;
        private readonly ICustomerPhoneService _customerphoneservice;

        private readonly IMapper _mapper;

        public ManagerCustomerController()
        {

        }
        public ManagerCustomerController(ICustomerGroupService customergroupservice,ICustomerPhoneService customerphoneservice, ISourceService sourceservice, ICustomerService customerservice, IShipAddressService shipaddressservice, IMapper mapper)
        {
            this._customerservice = customerservice;
            this._shipaddressservice = shipaddressservice;
            this._customerphoneservice = customerphoneservice;
            this._customergroupservice = customergroupservice;
            this._sourceservice = sourceservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/customer/getall")]
        public IHttpActionResult GetAllDropdown()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                int company_id = BaseController.get_company_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customerservice.GetAllDropdown(company_id);
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
        [Route("api/customer/search_by_curator")]
        public IHttpActionResult GetCustomerByCurator(int pageSize, int pageNumber, int? cu_curator_id, string search_name)
        {
            ResponseDataDTO<PagedResults<customeraddressviewmodel>> response = new ResponseDataDTO<PagedResults<customeraddressviewmodel>>();
            try
            {
                int company_id = BaseController.get_company_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customerservice.GetCustomerByCurator(pageSize,pageNumber,cu_curator_id,search_name,company_id);
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
        [Route("api/customer/get_by_id")]
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
        [HttpGet]
        [Route("api/customer-service/infor")]
        public IHttpActionResult GetServiceInforCustomer(int cu_id)
        {
            ResponseDataDTO<servicesearchcustomerviewmodel> response = new ResponseDataDTO<servicesearchcustomerviewmodel>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customerservice.GetServiceInforCustomer(cu_id);
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
        [Route("api/transaction-customers/infor")]
        public IHttpActionResult GetInforCustomerTransaction(int cu_id)
        {
            ResponseDataDTO<transactioncustomerviewmodel> response = new ResponseDataDTO<transactioncustomerviewmodel>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customerservice.GetInforCustomerTransaction(cu_id);
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
        [Route("api/customer/search")]
        public IHttpActionResult GetSearch(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, DateTime? start_date, DateTime? end_date, string name)
        {
            ResponseDataDTO<PagedResults<customerviewmodel>> response = new ResponseDataDTO<PagedResults<customerviewmodel>>();
            try
            {
                int company_id = BaseController.get_company_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customerservice.GetAllPageSearch(pageNumber, pageSize, source_id, cu_type, customer_group_id,start_date,end_date, name,company_id);
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
                int company_id = BaseController.get_company_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customerservice.GetAllPageSearchSms(pageNumber, pageSize, source_id, cu_type, customer_group_id, name,company_id);
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
        [Route("api/customer-service/search")]
        public IHttpActionResult GetAllPageSearchService(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, string name)
        {
            ResponseDataDTO<PagedResults<servicesearchcustomerviewmodel>> response = new ResponseDataDTO<PagedResults<servicesearchcustomerviewmodel>>();
            try
            {
                int company_id = BaseController.get_company_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customerservice.GetAllPageSearchService(pageNumber, pageSize, source_id, cu_type, customer_group_id, name,company_id);
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
                int company_id = BaseController.get_company_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customerservice.GetAllType(company_id);
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
        [Route("api/customer/create")]
        public async Task<IHttpActionResult> CreateCustomer([FromBody] CustomerCreateViewModelJson create_customer)
        {
            ResponseDataDTO<customer> response = new ResponseDataDTO<customer>();
            try
            {
                var customer = create_customer;

                #region["Check null"]

                if (customer.cu_fullname == null || customer.cu_fullname.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Họ và tên không được để trống";
                    response.Error = "cu_fullname";
                    return Ok(response);
                }
                if (customer.cu_type == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Loại khách hàng không được để trống";
                    response.Error = "cu_type";
                    return Ok(response);
                }

                if (customer.customer_group_id == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Nhóm khách hàng không được để trống";
                    response.Error = "customer_group_id";
                    return Ok(response);
                }

                if (customer.cu_flag_order == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Đặt dịch vụ không được để trống";
                    response.Error = "cu_flag_order";
                    return Ok(response);
                }
                if (customer.source_id == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Nguồn khách hàng không được để trống";
                    response.Error = "source_id";
                    return Ok(response);
                }
                if (customer.cu_flag_used == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Sử dụng dịch vụ  không được để trống";
                    response.Error = "source_id";
                    return Ok(response);
                }

                if (customer.cu_email != null)
                {
                    if (customer.cu_email.Trim() != "")
                    {
                        if (!Utilis.IsValidEmail(customer.cu_email))
                        {
                            response.Code = HttpCode.NOT_FOUND;
                            response.Message = "Email sai định dạng";
                            response.Error = "cu_email";
                            return Ok(response);
                        }
                    }

                }
                if (customer.sha_detail_now == null || customer.sha_detail_now.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Địa chỉ chi tiết không được để trống";
                    response.Error = "sha_detail_now";
                    return Ok(response);
                }

                #endregion
                //Kiểm tra các trường rằng buộc
                foreach (customer_phonejson cp in customer.list_customer_phone)
                {
                    var temp = _customerphoneservice.GetAllIncluing(t => t.cp_phone_number.Equals(cp.cp_phone_number));
                    if(temp.Count() != 0 )
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Số điện thoại bị trùng hoặc đã tồn tại ";
                        response.Error = "list_customer_phone";
                        return Ok(response);
                    }
                }
                //end kiểm tra
                //Save customer to database
                customer customer_create = new customer();
                //Thong tin chung 

                if(customer.cu_fullname != null) customer_create.cu_fullname = customer.cu_fullname.Trim();
                customer_create.customer_group_id = customer.customer_group_id;
                if (customer.cu_birthday != null) customer_create.cu_birthday = customer.cu_birthday;
                if(customer.cu_email != null) customer_create.cu_email = customer.cu_email.Trim();
                customer_create.cu_flag_order = customer.cu_flag_order;
                customer_create.cu_flag_used = customer.cu_flag_used;
                customer_create.cu_status = customer.cu_status;
                customer_create.source_id = customer.source_id;
                customer_create.cu_note = customer.cu_note;
                customer_create.cu_type = customer.cu_type;
                
                //Thong tin lien he
               
                //Lấy ra bản ghi cuối cùng tạo mã code 
                var x = _customerservice.GetLast();
                if (x == null) customer_create.cu_code = "KH000000";
                else customer_create.cu_code = Utilis.CreateCodeByCode("KH", x.cu_code, 8);
                
                customer_create.cu_create_date = DateTime.Now;
                customer_create.staff_id = BaseController.get_id_current();
                customer_create.company_id = BaseController.get_company_id_current();
                customer_create.cu_thumbnail = "/Uploads/Images/default/customer.png";
                // save new customer
                _customerservice.Create(customer_create);
                customer customer_last = _customerservice.GetLast();
                //save address hiện tại 
                ship_address address_now = new ship_address();
                address_now.customer_id = customer_last.cu_id;
                address_now.sha_ward = customer.sha_ward_now;
                address_now.sha_province = customer.sha_province_now;
                address_now.sha_district = customer.sha_district_now;
                address_now.sha_geocoding = customer.sha_geocoding_now;
                if(customer.sha_detail_now != null) address_now.sha_detail = customer.sha_detail_now.Trim();
                address_now.sha_note = customer.sha_note_now;
                address_now.sha_flag_center = 1;
                _shipaddressservice.Create(address_now);
                //Save list sdt
                foreach (customer_phonejson cup in customer.list_customer_phone)
                {
                    customer_phone cup_create = new customer_phone();
                    cup_create.customer_id = customer_last.cu_id;
                    cup_create.cp_type = cup.cp_type;
                    cup_create.cp_phone_number = cup.cp_phone_number;
                    cup_create.cp_note = cup.cp_note;
                    _customerphoneservice.Create(cup_create);
                }
                //Save shipaddress
                foreach (customer_ship_addressjson add in customer.list_ship_address)
                {
                    ship_address add_create = new ship_address();
                    add_create.customer_id = customer_last.cu_id;
                    add_create.sha_ward = add.sha_ward;
                    add_create.sha_province = add.sha_province;
                    add_create.sha_district = add.sha_district;
                    add_create.sha_geocoding = add.sha_geocoding;
                    add_create.sha_detail = add.sha_detail;
                    add_create.sha_note = add.sha_note;
                    add_create.sha_flag_center = 0;
                    _shipaddressservice.Create(add_create);
                }

                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = customer_create;
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

        #region[Update]

        [HttpPut]
        [Route("api/customer/update")]

        public async Task<IHttpActionResult> UpdateCustomer([FromBody] CustomerUpdateViewModelJson update_customer)
        {
            ResponseDataDTO<customer> response = new ResponseDataDTO<customer>();
            try
            {
                var customer = update_customer;

                #region["Check null"]

                if (customer.cu_fullname == null || customer.cu_fullname.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Họ và tên không được để trống";
                    response.Error = "cu_fullname";
                    return Ok(response);
                }
                if (customer.cu_type == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Loại khách hàng không được để trống";
                    response.Error = "cu_type";
                    return Ok(response);
                }

                if (customer.customer_group_id == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Nhóm khách hàng không được để trống";
                    response.Error = "customer_group_id";
                    return Ok(response);
                }

                if (customer.cu_flag_order == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Đặt dịch vụ không được để trống";
                    response.Error = "cu_flag_order";
                    return Ok(response);
                }
                if (customer.source_id == 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Nguồn khách hàng không được để trống";
                    response.Error = "source_id";
                    return Ok(response);
                }
                if (customer.cu_flag_used == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Sử dụng dịch vụ  không được để trống";
                    response.Error = "source_id";
                    return Ok(response);
                }
                if (customer.sha_detail_now == null || customer.sha_detail_now.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Địa chỉ chi tiết không được để trống";
                    response.Error = "sha_detail_now";
                    return Ok(response);
                }

                if (customer.cu_email != null)
                {
                    if (customer.cu_email.Trim() != "")
                    {
                        if (!Utilis.IsValidEmail(customer.cu_email))
                        {
                            response.Code = HttpCode.NOT_FOUND;
                            response.Message = "Email sai định dạng";
                            response.Error = "cu_email";
                            return Ok(response);
                        }
                    }

                }


                #endregion
                //Kiểm tra các trường rằng buộc
                //end kiểm tra
                //Save customer to database
                customer exists_customer = _customerservice.Find(customer.cu_id);
                //Thong tin chung 
                if(customer.cu_fullname != null) exists_customer.cu_fullname = customer.cu_fullname.Trim();
                exists_customer.customer_group_id = customer.customer_group_id;
                if(customer.cu_birthday != null) exists_customer.cu_birthday = customer.cu_birthday;
                if(customer.cu_email != null) exists_customer.cu_email = customer.cu_email.Trim();
                exists_customer.cu_flag_order = customer.cu_flag_order;
                exists_customer.cu_flag_used = customer.cu_flag_used;
                exists_customer.cu_status = customer.cu_status;
                exists_customer.source_id = customer.source_id;
                exists_customer.cu_note = customer.cu_note;
                exists_customer.cu_type = customer.cu_type;

                // save new customer
                _customerservice.Update(exists_customer,exists_customer.cu_id);
                //save address hiện tại 
                ship_address exists_address = _shipaddressservice.GetAllIncluing(x => x.customer_id == customer.cu_id && x.sha_flag_center == 1).FirstOrDefault();
                if(exists_address != null)
                {
                    exists_address.sha_ward = customer.sha_ward_now;
                    exists_address.sha_province = customer.sha_province_now;
                    exists_address.sha_district = customer.sha_district_now;
                    exists_address.sha_geocoding = customer.sha_geocoding_now;
                    if (customer.sha_detail_now != null) exists_address.sha_detail = customer.sha_detail_now.Trim();
                    exists_address.sha_note = customer.sha_note_now;
                    _shipaddressservice.Update(exists_address, exists_address.sha_id);
                }
                
                //Update list sdt
                List<customer_phone> lts_cp_db = _customerphoneservice.GetAllIncluing(x => x.customer_id == customer.cu_id ).ToList();
                List<customer_phonejson> lts_cp_v = new List<customer_phonejson>(customer.list_customer_phone);
                foreach (customer_phonejson cp_f in lts_cp_v)
                {
                    if (Utilis.IsNumber(cp_f.cp_id))
                    {
                        int _id = Convert.ToInt32(cp_f.cp_id);
                        foreach (customer_phone cp in lts_cp_db)
                        {
                            if (cp.cp_id == _id)
                            {
                                //update
                                customer_phone exist_cp = _customerphoneservice.Find(_id);
                                exist_cp.cp_type = cp_f.cp_type;
                                exist_cp.cp_phone_number = cp_f.cp_phone_number;
                                exist_cp.cp_note = cp_f.cp_note;
                                _customerphoneservice.Update(exist_cp, exist_cp.cp_id);
                                lts_cp_db.Remove(_customerphoneservice.Find(cp.cp_id));
                                break;
                            }
                        }
                    }
                    else
                    {
                        //Create
                        customer_phone cup_create = new customer_phone();
                        cup_create.customer_id = customer.cu_id;
                        cup_create.cp_type = cp_f.cp_type;
                        cup_create.cp_phone_number = cp_f.cp_phone_number;
                        cup_create.cp_note = cp_f.cp_note;
                        _customerphoneservice.Create(cup_create);
                    }
                }
                foreach (customer_phone cp_d in lts_cp_db)
                {
                    _customerphoneservice.Delete(cp_d);
                }

                //update shipaddress
                List<ship_address> lts_sha_db = _shipaddressservice.GetAllIncluing(x => x.customer_id == customer.cu_id && x.sha_flag_center == 0).ToList();
                List<customer_ship_addressjson> lts_sha_v = new List<customer_ship_addressjson>(customer.list_ship_address);
                foreach (customer_ship_addressjson sha_f in lts_sha_v)
                {
                    if (Utilis.IsNumber(sha_f.sha_id))
                    {
                        int _id = Convert.ToInt32(sha_f.sha_id);
                        foreach (ship_address sha in lts_sha_db)
                        {
                            if (sha.sha_id == _id)
                            {
                                //update
                                ship_address exist_address = _shipaddressservice.Find(_id);
                                exist_address.sha_ward = sha_f.sha_ward;
                                exist_address.sha_province = sha_f.sha_province;
                                exist_address.sha_district = sha_f.sha_district;
                                exist_address.sha_geocoding = sha_f.sha_geocoding;
                                exist_address.sha_detail = sha_f.sha_detail;
                                exist_address.sha_note = sha_f.sha_note;
                                _shipaddressservice.Update(exist_address, exist_address.sha_id);
                                lts_sha_db.Remove(_shipaddressservice.Find(sha.sha_id));
                                break;
                            }
                        }
                    }
                    else
                    {
                        //Create
                        ship_address create_address = new ship_address();
                        create_address.sha_ward = sha_f.sha_ward;
                        create_address.sha_province = sha_f.sha_province;
                        create_address.sha_district = sha_f.sha_district;
                        create_address.sha_geocoding = sha_f.sha_geocoding;
                        create_address.sha_detail = sha_f.sha_detail;
                        create_address.sha_note = sha_f.sha_note;
                        create_address.customer_id = customer.cu_id;
                        create_address.sha_flag_center = 0;
                        _shipaddressservice.Create(create_address);
                    }
                }
                foreach (ship_address sha_d in lts_sha_db)
                {
                    _shipaddressservice.Delete(sha_d);
                }

                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = exists_customer;
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

        #region[Delete]
        [HttpDelete]
        [Route("api/customer/delete")]
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
                    response.Message = "Không có mã khách hàng "+ current_id.ToString()+" trong hệ thống.";
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
            List<int> list_customer_create = new List<int>();
            ResponseDataDTO<customerviewexport> response = new ResponseDataDTO<customerviewexport>();
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
                var list = new List<customerviewexport>();
                fileName = "D:/coerp" + fileName;
                //fileName = "D:/ERP20/ERP.API" + fileName;
                var dataset = ExcelImport.ImportExcelXLS(fileName, true);
                DataTable table = (DataTable)dataset.Tables[0];
                if (table != null && table.Rows.Count > 0)
                {

                    if (table.Columns.Count != 10)
                    {
                        exitsData = "File excel import không hợp lệ!";
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = exitsData;
                        response.Data = null;
                        return Ok(response);
                    }

                    #region["Check duplicate"]
                    for (var i = 0; i < table.Rows.Count; i++)
                    {
                        var Cu_code_cur = table.Rows[i]["cu_code"].ToString().Trim();
                        for (var j = 0; j < table.Rows.Count; j++)
                        {
                            if (i != j)
                            {
                                var _cuCodeCur = table.Rows[j]["cu_code"].ToString().Trim();
                                if (Cu_code_cur.Equals(_cuCodeCur))
                                {
                                    exitsData = "Mã khách hàng'" + Cu_code_cur + "' bị lặp trong file excel!";
                                    response.Code = HttpCode.NOT_FOUND;
                                    response.Message = exitsData;
                                    response.Error = "cu_code";
                                    return Ok(response);
                                }
                            }
                        }
                    }
                    #endregion

                    list = DataTableCmUtils.ToListof<customerviewexport>(table);
                    foreach (customerviewexport i in list)
                    {
                        #region["Check tồn tại"]
                        var us = _customerphoneservice.GetAllIncluing(t => t.cp_phone_number.Equals(i.cu_mobile)).FirstOrDefault();
                        if (us != null)
                        {
                            Delete_Customer(list_customer_create);
                            exitsData = "Đã có số '" + i.cu_mobile + "' tồn tại trong cơ sở dữ liệu!";
                            response.Code = HttpCode.NOT_FOUND;
                            response.Message = exitsData;
                            response.Error = "cp_phone_number";
                            return Ok(response);
                        }
                        #endregion
                        var source_exits = _sourceservice.GetAllIncluing(y => y.src_name.Contains(i.source_name)).FirstOrDefault();
                        if(source_exits == null)
                        {
                            Delete_Customer(list_customer_create);
                            exitsData = "Nguồn khách hàng '" + i.source_name + "' không tồn tại trong cơ sở dữ liệu!";
                            response.Code = HttpCode.NOT_FOUND;
                            response.Message = exitsData;
                            response.Error = "source_name";
                            return Ok(response);
                        }
                        i.source_id = source_exits.src_id;
                        var cg_exits = _customergroupservice.GetAllIncluing(y => y.cg_name.Contains(i.customer_group_name)).FirstOrDefault();
                        if (cg_exits == null)
                        {
                            Delete_Customer(list_customer_create);
                            exitsData = "Nhóm khách hàng '" + i.customer_group_name + "' không tồn tại trong cơ sở dữ liệu!";
                            response.Code = HttpCode.NOT_FOUND;
                            response.Message = exitsData;
                            response.Error = "source_name";
                            return Ok(response);
                        }
                        i.customer_group_id = cg_exits.cg_id;
                        var cu_exits = _customerservice.GetAllIncluing(y => y.cu_code.Contains(i.cu_code)).FirstOrDefault();
                        if (cu_exits != null)
                        {
                            Delete_Customer(list_customer_create);
                            exitsData = "khách hàng '" + i.cu_code + "'tồn tại trong cơ sở dữ liệu!";
                            response.Code = HttpCode.NOT_FOUND;
                            response.Message = exitsData;
                            response.Error = "cu_code";
                            return Ok(response);
                        }
                        #region["Create customer"]
                        customer customer_create = new customer();
                        customer_create = _mapper.Map<customer>(i);
                        for (int u = 0; u < EnumCustomer.cu_type.Length; u++)
                        {
                            if (i.cu_type_name != null)
                            {
                                if (i.cu_type_name.Trim().ToLower().Contains(EnumCustomer.cu_type[u].Trim().ToLower()))
                                {
                                    customer_create.cu_type=u + 1;

                                }
                            }

                        }
                        
                        customer_create.cu_flag_order = 1;
                        customer_create.cu_status = 1;
                        customer_create.cu_flag_used = 1;
                        //Lấy ra bản ghi cuối cùng tạo mã code 
                        if(i.cu_code == null)
                        {
                            var x = _customerservice.GetLast();
                            if (x == null) customer_create.cu_code = "KH000000";
                            else customer_create.cu_code = Utilis.CreateCodeByCode("KH", x.cu_code, 8);
                        }
                        else
                        {
                            customer_create.cu_code = i.cu_code;
                        }
                        
                        customer_create.cu_create_date = DateTime.Now;
                        customer_create.staff_id = BaseController.get_id_current();
                        customer_create.company_id = BaseController.get_company_id_current();
                        customer_create.cu_thumbnail = "/Uploads/Images/default/customer.png";
                        _customerservice.Create(customer_create);
                        int last_id = _customerservice.GetLast().cu_id;
                        list_customer_create.Add(last_id);
                        #endregion

                        #region["Xử lý địa chỉ hiện tại"]
                        try
                        {
                            if (i.cu_address != null)
                            {
                                var dc1 = i.cu_address.Trim().Split('-');
                                string[] diachi = new string[dc1.Length];
                                for (int j = 0; j < dc1.Length; j++)
                                {
                                    diachi[j] = dc1[j];
                                }
                                ship_address un_create = new ship_address();
                                if (diachi[0] != null)
                                    un_create.sha_province = diachi[0].Trim();
                                if (diachi[1] != null)
                                    un_create.sha_district = diachi[1].Trim();
                                if (diachi[2] != null)
                                    un_create.sha_ward = diachi[2].Trim();
                                if (diachi[3] != null)
                                    un_create.sha_detail = diachi[3].Trim();
                                un_create.sha_flag_center = 1;
                                un_create.customer_id = last_id;
                                if (_customerservice.Check_location(un_create))
                                {
                                    _shipaddressservice.Create(un_create);
                                }
                            }
                        }
                        catch
                        {
                            foreach(int cu_id in list_customer_create)
                            {
                                _customerservice.Delete(_customerservice.Find(cu_id));
                            }
                            
                            exitsData = "Nhập lại địa chỉ khách hàng '" + i.cu_fullname + " ' !";
                            response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                            response.Message = exitsData;
                            response.Data = null;
                            return Ok(response);
                        }


                        #endregion

                        #region["Xử lý điện thoại"]
                        customer_phone cup_create = new customer_phone();
                        cup_create.customer_id = last_id;
                        cup_create.cp_type = 1;
                        cup_create.cp_phone_number = i.cu_mobile;
                        _customerphoneservice.Create(cup_create);
                        #endregion

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
        private void Delete_Customer(List<int> lst_add)
        {
            if (lst_add != null)
            {
                foreach (int i in lst_add)
                {
                    var cu = _customerservice.Find(i);
                    _customerservice.Delete(cu);
                }
            }
        }
        #endregion

        #region["Export Excel"]
        [HttpGet]
        [Route("api/customer/export")]
        public async Task<IHttpActionResult> Export(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, DateTime? start_date, DateTime? end_date, string name)
        {
            ResponseDataDTO<string> response = new ResponseDataDTO<string>();
            try
            {
                int company_id = BaseController.get_company_id_current();
                var listStaff = new List<customerviewexport>();

                //Đưa ra danh sách staff trong trang nào đó 
                var objRT_Mst_Staff = _customerservice.ExportCustomer(pageNumber, pageSize, source_id, cu_type, customer_group_id,start_date,end_date, name,company_id);
                if (objRT_Mst_Staff != null)
                {
                    listStaff.AddRange(objRT_Mst_Staff.Results);

                    Dictionary<string, string> dicColNames = GetImportDicColums();

                    string url = "";
                    string filePath = GenExcelExportFilePath(string.Format(typeof(customer).Name), ref url);

                    ExcelExport.ExportToExcelFromList(listStaff, dicColNames, filePath, string.Format("Khách hàng"));
                    //Tach chuoi
                    filePath = filePath.Replace("\\", "/");
                    int index = filePath.IndexOf("TempFiles");
                    filePath = filePath.Substring(index);
                    response.Code = HttpCode.OK;
                    response.Message = "Đã xuất excel thành công!";
                    response.Data = filePath;
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

        #region["Export Template"]
        [HttpGet]
        [Route("api/customer/export_template")]
        public async Task<IHttpActionResult> ExportTemplate()
        {

            ResponseDataDTO<string> response = new ResponseDataDTO<string>();
            try
            {
                var listStaff = new List<staffview>();
                Dictionary<string, string> dicColNames = GetImportDicColumsTemplate();

                string url = "";
                string filePath = GenExcelExportFilePath(string.Format(typeof(customer).Name), ref url);

                ExcelExport.ExportToExcelTemplate(listStaff, dicColNames, filePath, string.Format("Khách hàng"));

                filePath = filePath.Replace("\\", "/");
                int index = filePath.IndexOf("TempFiles");
                filePath = filePath.Substring(index);
                response.Code = HttpCode.OK;
                response.Message = "Đã xuất excel thành công!";
                response.Data = filePath;
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
                 {"cu_code","Mã khách hàng"},
                 {"cu_fullname","Họ và tên "},
                 {"cu_mobile","SĐT" },
                 {"cu_email","Email"},
                 {"cu_type_name","Loại khách hàng"},
                 {"source_name","Nguồn khách hàng"},
                 {"customer_group_name","Nhóm khách hàng"},
                 {"cu_address","Địa chỉ"},
                 {"cu_birthday","Ngày sinh"},
                 {"cu_note","Chú ý"},
                 {"cu_flag_used_name","Sử dụng dịch vụ"},
                 {"cu_flag_order_name","Đặt dịch vụ"},
                 {"cu_status_name","Trạng thái"},

            };
        }
        private Dictionary<string, string> GetImportDicColumsTemplate()
        {
            return new Dictionary<string, string>()
            {
                 {"cu_code","Mã khách hàng"},
                 {"cu_fullname","Họ và tên "},
                 {"cu_mobile","SĐT" },
                 {"cu_email","Email"},
                 {"source_name","Nguồn khách hàng"},
                 {"cu_type_name","Loại khách hàng"},
                 {"customer_group_name","Nhóm khách hàng"},
                 {"cu_address","Địa chỉ"},
                 {"cu_birthday","Ngày sinh"},
                 {"cu_note","Chú ý"}
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