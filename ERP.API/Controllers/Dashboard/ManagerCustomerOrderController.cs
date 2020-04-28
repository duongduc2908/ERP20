using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Excel;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Customer;
using ERP.Data.ModelsERP.ModelView.CustomerOrder;
using ERP.Data.ModelsERP.ModelView.Excutor;
using ERP.Data.ModelsERP.ModelView.ExportDB;
using ERP.Data.ModelsERP.ModelView.OrderService;
using ERP.Data.ModelsERP.ModelView.Service;
using ERP.Extension.Extensions;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ERP.API.Controllers.Dashboard
{
    [EnableCors("*", "*", "*")]
    [Authorize]
    public class ManagerCustomerOrderController : BaseController
    {
        private readonly ICustomerOrderService _customer_orderservice;
        private readonly ICustomerService _customerservice;
        private readonly IOrderProductService _order_productservice;
        private readonly IOrderServiceService _orderserviceservice;
        private readonly IShipAddressService _shipaddressservice;
        private readonly IExecutorService _executorservice;
        private readonly IServiceTimeService _servicetimeservice;
        private readonly IServiceService _serviceservice;
        private readonly ICustomerPhoneService _customerphoneservice;

        private readonly IMapper _mapper;

        public ManagerCustomerOrderController()
        {

        }
        public ManagerCustomerOrderController(IServiceService serviceservice, ICustomerPhoneService customerphoneservice, IServiceTimeService servicetimeservice, IExecutorService executorservice, IOrderServiceService orderserviceservice, ICustomerOrderService customer_orderservice, ICustomerService customerservice, IOrderProductService order_productservice, IShipAddressService shipAddressService, IMapper mapper)
        {

            this._order_productservice = order_productservice;
            this._shipaddressservice = shipAddressService;
            this._customer_orderservice = customer_orderservice;
            this._customerservice = customerservice;
            this._orderserviceservice = orderserviceservice;
            this._executorservice = executorservice;
            this._servicetimeservice = servicetimeservice;
            this._serviceservice = serviceservice;
            this._customerphoneservice = customerphoneservice;
            this._mapper = mapper;

        }


        #region[ "method"]
        [HttpGet]
        [Route("api/customer-orders/infor")]
        public IHttpActionResult GetInforById(int id)
        {
            ResponseDataDTO<customerordermodelview> response = new ResponseDataDTO<customerordermodelview>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customer_orderservice.GetAllOrderById(id);
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
        [Route("api/customer_order_service/get_by_id")]
        public IHttpActionResult GetInforServiceById(int cuo_id)
        {
            ResponseDataDTO<servicercustomerorderviewmodel> response = new ResponseDataDTO<servicercustomerorderviewmodel>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customer_orderservice.GetAllOrderServiceById(cuo_id);
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
        [Route("api/customer-orders/status")]
        public IHttpActionResult GetllStatus()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customer_orderservice.GetAllStatus();
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
        [Route("api/customer-orders/get-all-payment")]
        public IHttpActionResult GetAllSPayment()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customer_orderservice.GetAllPayment();
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
        [Route("api/customer-orders/search")]
        public IHttpActionResult GetAllSearch(int pageNumber, int pageSize, int? payment_type_id, DateTime? start_date, DateTime? end_date, string code)
        {
            ResponseDataDTO<PagedResults<customerorderviewmodel>> response = new ResponseDataDTO<PagedResults<customerorderviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customer_orderservice.GetAllSearch(pageNumber: pageNumber, pageSize: pageSize, payment_type_id: payment_type_id, start_date, end_date, code);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = "Không tìm thấy";
                response.Data = null;

                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("api/customer_order_service/search")]
        public IHttpActionResult GetAllSearchCustomerOrderService(int pageNumber, int pageSize, DateTime? start_date, DateTime? end_date, string search_name)
        {
            ResponseDataDTO<PagedResults<servicercustomerorderviewmodel>> response = new ResponseDataDTO<PagedResults<servicercustomerorderviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customer_orderservice.GetAllSearchCustomerOrderService(pageNumber: pageNumber, pageSize: pageSize, start_date, end_date, search_name);
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
        [Route("api/customer-orders/page")]
        public IHttpActionResult Getcustomer_ordersPaging(int pageSize, int pageNumber)
        {
            ResponseDataDTO<PagedResults<customerorderviewmodel>> response = new ResponseDataDTO<PagedResults<customerorderviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customer_orderservice.CreatePagedResults(pageNumber, pageSize);
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
        [Route("api/customer_order_service/get_staffs_free")]
        public IHttpActionResult GetStaffFree(work_time_view c, string fullName)
        {
            ResponseDataDTO<List<dropdown_salary>> response = new ResponseDataDTO<List<dropdown_salary>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customer_orderservice.Get_staff_free(c, fullName);
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
        [Route("api/customer_order_service/gen_work_time")]
        public IHttpActionResult GenWorkTime(int pageNumber, int pageSize,service_time c)
        {
            ResponseDataDTO< PagedResults<work_time_view>> response = new ResponseDataDTO<PagedResults<work_time_view>>();
            try
            {
                List<DateTime> lst = GenDateOrderService.Gen(c.st_custom_start, c.st_custom_end, c.st_repeat_type, c.st_repeat_every, c.st_sun_flag, c.st_mon_flag, c.st_tue_flag, c.st_wed_flag, c.st_thu_flag, c.st_fri_flag, c.st_sat_flag, c.st_on_day_flag, c.st_on_day, c.st_on_the_flag, c.st_on_the);
                var skipAmount = pageSize * pageNumber;
                var totalNumberOfRecords = lst.Count;
                var list = lst.Skip(skipAmount).Take(pageSize).ToList();
                var mod = totalNumberOfRecords % pageSize;
                var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);
                List<work_time_view> res = new List<work_time_view>();
                foreach(DateTime dt in list)
                {
                    work_time_view wt = new work_time_view();
                    wt.start_time = c.st_start_time;
                    wt.end_time = c.st_end_time;
                    wt.work_time = dt;
                    res.Add(wt);
                }
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = new PagedResults<work_time_view>
                {
                    Results = res,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalNumberOfPages = totalPageCount,
                    TotalNumberOfRecords = totalNumberOfRecords
                };
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
        [Route("api/customer-orders/create")]
        public async Task<IHttpActionResult> CreateCustomerOrderProduct([FromBody] CustomerOrderProductViewModel customer_order)
        {
            ResponseDataDTO<customer_order> response = new ResponseDataDTO<customer_order>();
            try
            {
                var c = customer_order;
                //Id user now

                var current_id = BaseController.get_id_current();
                //Thoong tin khach hang 
                if (c.customer.cu_id == 0)
                {
                    #region[Create Customer]
                    #region["Check null"]

                    if (c.customer.cu_fullname == null || c.customer.cu_fullname.Trim() == "")
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Họ và tên không được để trống";
                        response.Error = "cu_fullname";
                        return Ok(response);
                    }
                    if (c.customer.cu_type == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Loại khách hàng không được để trống";
                        response.Error = "cu_type";
                        return Ok(response);
                    }

                    if (c.customer.customer_group_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nhóm khách hàng không được để trống";
                        response.Error = "customer_group_id";
                        return Ok(response);
                    }

                    if (c.customer.cu_flag_order == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Đặt dịch vụ không được để trống";
                        response.Error = "cu_flag_order";
                        return Ok(response);
                    }
                    if (c.customer.source_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nguồn khách hàng không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.cu_birthday == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Ngày sinh không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.cu_flag_used == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Sử dụng dịch vụ  không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.cu_email != null)
                    {
                        if (c.customer.cu_email.Trim() != "")
                        {
                            if (!Utilis.IsValidEmail(c.customer.cu_email))
                            {
                                response.Code = HttpCode.NOT_FOUND;
                                response.Message = "Email sai định dạng";
                                response.Error = "cu_email";
                                return Ok(response);
                            }
                        }

                    }

                    if (c.customer.sha_detail_now == null || c.customer.sha_detail_now.Trim() == "")
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Địa chỉ chi tiết không được để trống";
                        response.Error = "sha_detail_now";
                        return Ok(response);
                    }

                    #endregion
                    //Kiểm tra các trường rằng buộc
                    foreach (customer_phonejson cp in c.customer.list_customer_phone)
                    {
                        var temp = _customerphoneservice.GetAllIncluing(t => t.cp_phone_number.Equals(cp.cp_phone_number));
                        if (temp.Count() != 0)
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

                    customer_create.cu_fullname = c.customer.cu_fullname;
                    customer_create.customer_group_id = c.customer.customer_group_id;
                    customer_create.cu_birthday = c.customer.cu_birthday;
                    customer_create.cu_email = c.customer.cu_email;
                    customer_create.cu_flag_order = c.customer.cu_flag_order;
                    customer_create.cu_flag_used = c.customer.cu_flag_used;
                    customer_create.cu_status = c.customer.cu_status;
                    customer_create.source_id = c.customer.source_id;
                    customer_create.cu_note = c.customer.cu_note;
                    customer_create.cu_type = c.customer.cu_type;

                    //Thong tin lien he

                    //Lấy ra bản ghi cuối cùng tạo mã code 
                    var x = _customerservice.GetLast();
                    if (x == null) customer_create.cu_code = "KH000000";
                    else customer_create.cu_code = Utilis.CreateCodeByCode("KH", x.cu_code, 8);

                    customer_create.cu_create_date = DateTime.Now;
                    customer_create.staff_id = BaseController.get_id_current();
                    customer_create.cu_thumbnail = "/Uploads/Images/default/customer.png";

                    // save new customer
                    _customerservice.Create(customer_create);
                    customer customer_last1 = _customerservice.GetLast();
                    //save address hiện tại 
                    ship_address address_now = new ship_address();
                    address_now.customer_id = customer_last1.cu_id;
                    address_now.sha_ward = c.customer.sha_ward_now;
                    address_now.sha_province = c.customer.sha_province_now;
                    address_now.sha_district = c.customer.sha_district_now;
                    address_now.sha_geocoding = c.customer.sha_geocoding_now;
                    address_now.sha_detail = c.customer.sha_detail_now;
                    address_now.sha_note = c.customer.sha_note_now;
                    address_now.sha_flag_center = 1;
                    _shipaddressservice.Create(address_now);
                    //Save list sdt
                    foreach (customer_phonejson cup in c.customer.list_customer_phone)
                    {
                        customer_phone cup_create = new customer_phone();
                        cup_create.customer_id = customer_last1.cu_id;
                        cup_create.cp_type = cup.cp_type;
                        cup_create.cp_phone_number = cup.cp_phone_number;
                        cup_create.cp_note = cup.cp_note;
                        _customerphoneservice.Create(cup_create);
                    }
                    //Save shipaddress
                    foreach (customer_ship_addressjson add in c.customer.list_ship_address)
                    {
                        ship_address add_create = new ship_address();
                        add_create.customer_id = customer_last1.cu_id;
                        add_create.sha_ward = add.sha_ward;
                        add_create.sha_province = add.sha_province;
                        add_create.sha_district = add.sha_district;
                        add_create.sha_geocoding = add.sha_geocoding;
                        add_create.sha_detail = add.sha_detail;
                        add_create.sha_note = add.sha_note;
                        add_create.sha_flag_center = 0;
                        _shipaddressservice.Create(add_create);
                    }

                    c.customer.cu_id = customer_last1.cu_id;
                    #endregion
                }
                else
                {
                    #region[Updatecustomer]
                    #region["Check null"]

                    if (c.customer.cu_fullname == null || c.customer.cu_fullname.Trim() == "")
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Họ và tên không được để trống";
                        response.Error = "cu_fullname";
                        return Ok(response);
                    }
                    if (c.customer.cu_type == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Loại khách hàng không được để trống";
                        response.Error = "cu_type";
                        return Ok(response);
                    }

                    if (c.customer.customer_group_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nhóm khách hàng không được để trống";
                        response.Error = "customer_group_id";
                        return Ok(response);
                    }

                    if (c.customer.cu_flag_order == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Đặt dịch vụ không được để trống";
                        response.Error = "cu_flag_order";
                        return Ok(response);
                    }
                    if (c.customer.source_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nguồn khách hàng không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.cu_birthday == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Ngày sinh không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.cu_flag_used == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Sử dụng dịch vụ  không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.sha_detail_now == null || c.customer.sha_detail_now.Trim() == "")
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Địa chỉ chi tiết không được để trống";
                        response.Error = "sha_detail_now";
                        return Ok(response);
                    }

                    if (c.customer.cu_email != null)
                    {
                        if (c.customer.cu_email.Trim() != "")
                        {
                            if (!Utilis.IsValidEmail(c.customer.cu_email))
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
                    customer exists_customer = _customerservice.Find(c.customer.cu_id);
                    //Thong tin chung 

                    exists_customer.cu_fullname = c.customer.cu_fullname;
                    exists_customer.customer_group_id = c.customer.customer_group_id;
                    exists_customer.cu_birthday = c.customer.cu_birthday;
                    exists_customer.cu_email = c.customer.cu_email;
                    exists_customer.cu_flag_order = c.customer.cu_flag_order;
                    exists_customer.cu_flag_used = c.customer.cu_flag_used;
                    exists_customer.cu_status = c.customer.cu_status;
                    exists_customer.source_id = c.customer.source_id;
                    exists_customer.cu_note = c.customer.cu_note;
                    exists_customer.cu_type = c.customer.cu_type;

                    // save new customer
                    _customerservice.Update(exists_customer, exists_customer.cu_id);
                    //save address hiện tại 
                    ship_address exists_address = _shipaddressservice.GetAllIncluing(x => x.customer_id == c.customer.cu_id && x.sha_flag_center == 1).FirstOrDefault();

                    exists_address.sha_ward = c.customer.sha_ward_now;
                    exists_address.sha_province = c.customer.sha_province_now;
                    exists_address.sha_district = c.customer.sha_district_now;
                    exists_address.sha_geocoding = c.customer.sha_geocoding_now;
                    exists_address.sha_detail = c.customer.sha_detail_now;
                    exists_address.sha_note = c.customer.sha_note_now;
                    _shipaddressservice.Update(exists_address, exists_address.sha_id);
                    //Update list sdt
                    List<customer_phone> lts_cp_db = _customerphoneservice.GetAllIncluing(x => x.customer_id == c.customer.cu_id).ToList();
                    List<customer_phonejson> lts_cp_v = new List<customer_phonejson>(c.customer.list_customer_phone);
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
                            cup_create.customer_id = c.customer.cu_id;
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
                    List<ship_address> lts_sha_db = _shipaddressservice.GetAllIncluing(x => x.customer_id == c.customer.cu_id && x.sha_flag_center == 0).ToList();
                    List<customer_ship_addressjson> lts_sha_v = new List<customer_ship_addressjson>(c.customer.list_ship_address);
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
                            create_address.customer_id = c.customer.cu_id;
                            create_address.sha_flag_center = 0;
                            _shipaddressservice.Create(create_address);
                        }
                    }
                    foreach (ship_address sha_d in lts_sha_db)
                    {
                        _shipaddressservice.Delete(sha_d);
                    }


                    #endregion
                }


                // get data from formdata
                CustomerOrderCreateViewModel customer_orderCreateViewModel = new CustomerOrderCreateViewModel { };
                customer_orderCreateViewModel.customer_id = c.customer.cu_id;
                customer_orderCreateViewModel.staff_id = Convert.ToInt32(current_id);
                customer_orderCreateViewModel.cuo_payment_status = c.cuo_payment_status;
                customer_orderCreateViewModel.cuo_payment_type = c.cuo_payment_type;
                customer_orderCreateViewModel.cuo_ship_tax = c.cuo_ship_tax;
                customer_orderCreateViewModel.cuo_total_price = c.cuo_total_price;
                customer_orderCreateViewModel.cuo_discount = c.cuo_discount;
                customer_orderCreateViewModel.cuo_status = c.cuo_status;
                customer_orderCreateViewModel.cuo_address = c.cuo_address;


                customer_orderCreateViewModel.cuo_date = DateTime.Now;
                // mapping view model to entity
                var createdcustomer_order = _mapper.Map<customer_order>(customer_orderCreateViewModel);
                var op_last1 = _customer_orderservice.GetLast();
                if (op_last1 == null) createdcustomer_order.cuo_code = Utilis.CreateCode("ORP", 0, 7);
                else createdcustomer_order.cuo_code = Utilis.CreateCode("ORP", op_last1.cuo_id, 7);

                // save new customer_order
                _customer_orderservice.Create(createdcustomer_order);
                var op_last = _customer_orderservice.GetLast();
                //create order product

                foreach (productorderviewmodel i in c.list_product)
                {
                    OrderProductCreateViewModel orderCreateViewModel = new OrderProductCreateViewModel { };
                    orderCreateViewModel.customer_order_id = op_last.cuo_id;
                    orderCreateViewModel.op_discount = i.op_discount;
                    orderCreateViewModel.op_note = i.op_note;
                    orderCreateViewModel.op_quantity = i.op_quantity;
                    orderCreateViewModel.product_id = i.pu_id;
                    orderCreateViewModel.op_total_value = i.op_total_value;

                    var createdorderproduct = _mapper.Map<order_product>(orderCreateViewModel);

                    _order_productservice.Create(createdorderproduct);
                }


                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = null;
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
        [HttpPost]
        [Route("api/customer_order_service/create")]
        public async Task<IHttpActionResult> CreateCustomerOrderService([FromBody] CustomerOrderServiceViewModelCreate customer_order)
        {
            ResponseDataDTO<customer_order> response = new ResponseDataDTO<customer_order>();
            try
            {
                var c = customer_order;

                var current_id = BaseController.get_id_current();
                //Thoong tin khach hang 
                if (c.customer.cu_id == 0)
                {
                    #region[Create Customer]
                    #region["Check null"]

                    if (c.customer.cu_fullname == null || c.customer.cu_fullname.Trim() == "")
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Họ và tên không được để trống";
                        response.Error = "cu_fullname";
                        return Ok(response);
                    }
                    if (c.customer.cu_type == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Loại khách hàng không được để trống";
                        response.Error = "cu_type";
                        return Ok(response);
                    }

                    if (c.customer.customer_group_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nhóm khách hàng không được để trống";
                        response.Error = "customer_group_id";
                        return Ok(response);
                    }

                    if (c.customer.cu_flag_order == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Đặt dịch vụ không được để trống";
                        response.Error = "cu_flag_order";
                        return Ok(response);
                    }
                    if (c.customer.source_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nguồn khách hàng không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.cu_birthday == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Ngày sinh không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.cu_flag_used == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Sử dụng dịch vụ  không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.cu_email != null)
                    {
                        if (c.customer.cu_email.Trim() != "")
                        {
                            if (!Utilis.IsValidEmail(c.customer.cu_email))
                            {
                                response.Code = HttpCode.NOT_FOUND;
                                response.Message = "Email sai định dạng";
                                response.Error = "cu_email";
                                return Ok(response);
                            }
                        }

                    }
                    
                    if (c.customer.sha_detail_now == null || c.customer.sha_detail_now.Trim() == "")
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Địa chỉ chi tiết không được để trống";
                        response.Error = "sha_detail_now";
                        return Ok(response);
                    }

                    #endregion
                    //Kiểm tra các trường rằng buộc
                    foreach (customer_phonejson cp in c.customer.list_customer_phone)
                    {
                        var temp = _customerphoneservice.GetAllIncluing(t => t.cp_phone_number.Equals(cp.cp_phone_number));
                        if (temp.Count() != 0)
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

                    customer_create.cu_fullname = c.customer.cu_fullname;
                    customer_create.customer_group_id = c.customer.customer_group_id;
                    customer_create.cu_birthday = c.customer.cu_birthday;
                    customer_create.cu_email = c.customer.cu_email;
                    customer_create.cu_flag_order = c.customer.cu_flag_order;
                    customer_create.cu_flag_used = c.customer.cu_flag_used;
                    customer_create.cu_status = c.customer.cu_status;
                    customer_create.source_id = c.customer.source_id;
                    customer_create.cu_note = c.customer.cu_note;
                    customer_create.cu_type = c.customer.cu_type;

                    //Thong tin lien he

                    //Lấy ra bản ghi cuối cùng tạo mã code 
                    var x = _customerservice.GetLast();
                    if (x == null) customer_create.cu_code = "KH000000";
                    else customer_create.cu_code = Utilis.CreateCodeByCode("KH", x.cu_code, 8);

                    customer_create.cu_create_date = DateTime.Now;
                    customer_create.staff_id = BaseController.get_id_current();
                    customer_create.cu_thumbnail = "/Uploads/Images/default/customer.png";

                    // save new customer
                    _customerservice.Create(customer_create);
                    customer customer_last1 = _customerservice.GetLast();
                    //save address hiện tại 
                    ship_address address_now = new ship_address();
                    address_now.customer_id = customer_last1.cu_id;
                    address_now.sha_ward = c.customer.sha_ward_now;
                    address_now.sha_province = c.customer.sha_province_now;
                    address_now.sha_district = c.customer.sha_district_now;
                    address_now.sha_geocoding = c.customer.sha_geocoding_now;
                    address_now.sha_detail = c.customer.sha_detail_now;
                    address_now.sha_note = c.customer.sha_note_now;
                    address_now.sha_flag_center = 1;
                    _shipaddressservice.Create(address_now);
                    //Save list sdt
                    foreach (customer_phonejson cup in c.customer.list_customer_phone)
                    {
                        customer_phone cup_create = new customer_phone();
                        cup_create.customer_id = customer_last1.cu_id;
                        cup_create.cp_type = cup.cp_type;
                        cup_create.cp_phone_number = cup.cp_phone_number;
                        cup_create.cp_note = cup.cp_note;
                        _customerphoneservice.Create(cup_create);
                    }
                    //Save shipaddress
                    foreach (customer_ship_addressjson add in c.customer.list_ship_address)
                    {
                        ship_address add_create = new ship_address();
                        add_create.customer_id = customer_last1.cu_id;
                        add_create.sha_ward = add.sha_ward;
                        add_create.sha_province = add.sha_province;
                        add_create.sha_district = add.sha_district;
                        add_create.sha_geocoding = add.sha_geocoding;
                        add_create.sha_detail = add.sha_detail;
                        add_create.sha_note = add.sha_note;
                        add_create.sha_flag_center = 0;
                        _shipaddressservice.Create(add_create);
                    }
                    
                    c.customer.cu_id = customer_last1.cu_id;
                    #endregion
                }
                else
                {
                    #region[Updatecustomer]
                    #region["Check null"]

                    if (c.customer.cu_fullname == null || c.customer.cu_fullname.Trim() == "")
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Họ và tên không được để trống";
                        response.Error = "cu_fullname";
                        return Ok(response);
                    }
                    if (c.customer.cu_type == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Loại khách hàng không được để trống";
                        response.Error = "cu_type";
                        return Ok(response);
                    }

                    if (c.customer.customer_group_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nhóm khách hàng không được để trống";
                        response.Error = "customer_group_id";
                        return Ok(response);
                    }

                    if (c.customer.cu_flag_order == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Đặt dịch vụ không được để trống";
                        response.Error = "cu_flag_order";
                        return Ok(response);
                    }
                    if (c.customer.source_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nguồn khách hàng không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.cu_birthday == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Ngày sinh không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.cu_flag_used == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Sử dụng dịch vụ  không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.sha_detail_now == null || c.customer.sha_detail_now.Trim() == "")
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Địa chỉ chi tiết không được để trống";
                        response.Error = "sha_detail_now";
                        return Ok(response);
                    }

                    if (c.customer.cu_email != null)
                    {
                        if (c.customer.cu_email.Trim() != "")
                        {
                            if (!Utilis.IsValidEmail(c.customer.cu_email))
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
                    customer exists_customer = _customerservice.Find(c.customer.cu_id);
                    //Thong tin chung 

                    exists_customer.cu_fullname = c.customer.cu_fullname;
                    exists_customer.customer_group_id = c.customer.customer_group_id;
                    exists_customer.cu_birthday = c.customer.cu_birthday;
                    exists_customer.cu_email = c.customer.cu_email;
                    exists_customer.cu_flag_order = c.customer.cu_flag_order;
                    exists_customer.cu_flag_used = c.customer.cu_flag_used;
                    exists_customer.cu_status = c.customer.cu_status;
                    exists_customer.source_id = c.customer.source_id;
                    exists_customer.cu_note = c.customer.cu_note;
                    exists_customer.cu_type = c.customer.cu_type;

                    // save new customer
                    _customerservice.Update(exists_customer, exists_customer.cu_id);
                    //save address hiện tại 
                    ship_address exists_address = _shipaddressservice.GetAllIncluing(x => x.customer_id == c.customer.cu_id && x.sha_flag_center == 1).FirstOrDefault();

                    exists_address.sha_ward = c.customer.sha_ward_now;
                    exists_address.sha_province = c.customer.sha_province_now;
                    exists_address.sha_district = c.customer.sha_district_now;
                    exists_address.sha_geocoding = c.customer.sha_geocoding_now;
                    exists_address.sha_detail = c.customer.sha_detail_now;
                    exists_address.sha_note = c.customer.sha_note_now;
                    _shipaddressservice.Update(exists_address, exists_address.sha_id);
                    //Update list sdt
                    List<customer_phone> lts_cp_db = _customerphoneservice.GetAllIncluing(x => x.customer_id == c.customer.cu_id).ToList();
                    List<customer_phonejson> lts_cp_v = new List<customer_phonejson>(c.customer.list_customer_phone);
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
                            cup_create.customer_id = c.customer.cu_id;
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
                    List<ship_address> lts_sha_db = _shipaddressservice.GetAllIncluing(x => x.customer_id == c.customer.cu_id && x.sha_flag_center == 0).ToList();
                    List<customer_ship_addressjson> lts_sha_v = new List<customer_ship_addressjson>(c.customer.list_ship_address);
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
                            create_address.customer_id = c.customer.cu_id;
                            create_address.sha_flag_center = 0;
                            _shipaddressservice.Create(create_address);
                        }
                    }
                    foreach (ship_address sha_d in lts_sha_db)
                    {
                        _shipaddressservice.Delete(sha_d);
                    }
                    

                    #endregion
                }
                
                //Customer order service 
                customer_order customer_order_service = new customer_order();
                customer_order_service.staff_id = current_id;
                customer_order_service.customer_id = c.customer.cu_id;
                customer_order_service.cuo_discount = c.cuo_discount;
                customer_order_service.cuo_color_show = c.cuo_color_show;
                customer_order_service.cuo_date = DateTime.Now;
                customer_order_service.cuo_infor_time =c.cuo_infor_time;
                customer_order_service.cuo_address = c.cuo_address;

                // mapping view model to entity
                var op_temp = _customer_orderservice.GetLast();
                if (op_temp == null) customer_order_service.cuo_code = "ORS00000";
                else customer_order_service.cuo_code = Utilis.CreateCodeByCode("ORS", op_temp.cuo_code, 8);
                // save new customer_order
                _customer_orderservice.Create(customer_order_service);
                var op_last = _customer_orderservice.GetLast();
                //service time 
                //service time 
                #region create service time
                ServiceTimeCreateViewModel serviceTimeCreate = new ServiceTimeCreateViewModel();
                serviceTimeCreate.customer_order_id = op_last.cuo_id;
                serviceTimeCreate.st_start_time = c.st_start_time;
                serviceTimeCreate.st_end_time = c.st_end_time;
                serviceTimeCreate.st_start_date = c.st_start_date;
                serviceTimeCreate.st_end_date = c.st_end_date;
                serviceTimeCreate.st_repeat_type = c.st_repeat_type;
                serviceTimeCreate.st_sun_flag = c.st_sun_flag;
                serviceTimeCreate.st_mon_flag = c.st_mon_flag;
                serviceTimeCreate.st_tue_flag = c.st_tue_flag;
                serviceTimeCreate.st_wed_flag = c.st_wed_flag;
                serviceTimeCreate.st_thu_flag = c.st_thu_flag;
                serviceTimeCreate.st_fri_flag = c.st_fri_flag;
                serviceTimeCreate.st_sat_flag = c.st_sat_flag;
                serviceTimeCreate.st_repeat = c.st_repeat;
                serviceTimeCreate.st_repeat_every = c.st_repeat_every;
                serviceTimeCreate.st_on_the = c.st_on_the;
                serviceTimeCreate.st_on_day_flag = c.st_on_day_flag;
                serviceTimeCreate.st_on_day = c.st_on_day;
                serviceTimeCreate.st_on_the_flag = c.st_on_the_flag;
                serviceTimeCreate.st_custom_start = c.st_custom_start;
                if (c.st_custom_end == null)
                    serviceTimeCreate.st_custom_end = c.st_custom_start;
                else
                    serviceTimeCreate.st_custom_end = c.st_custom_end;

                var createServiceTime = _mapper.Map<service_time>(serviceTimeCreate);
                _servicetimeservice.Create(createServiceTime);
                #endregion
                var service_time_last = _servicetimeservice.GetLast();
                //thong tin dich vu 
                foreach (servicejson se in c.list_service)
                {
                    if (Utilis.IsNumber(se.se_id))
                    {
                        order_service create_order_service = new order_service();
                        create_order_service.customer_order_id = op_last.cuo_id;
                        create_order_service.service_id = Convert.ToInt32(se.se_id);
                        _orderserviceservice.Create(create_order_service);
                    }
                    else
                    {
                        service create_service = new service();
                        create_service.service_category_id = se.service_category_id;
                        create_service.se_description = se.se_description;
                        create_service.se_name = se.se_name;
                        create_service.se_note = se.se_note;
                        create_service.se_price = se.se_price;
                        create_service.se_saleoff = se.se_saleoff;
                        create_service.se_type = se.se_type;


                        var t = _serviceservice.GetLast();
                        if (t == null) create_service.se_code = "DV000000";
                        else create_service.se_code = Utilis.CreateCodeByCode("DV", t.se_code, 8);
                        _serviceservice.Create(create_service);
                        service se_last = _serviceservice.GetLast();

                        order_service create_order_service = new order_service();
                        create_order_service.customer_order_id = op_last.cuo_id;
                        create_order_service.service_id = se_last.se_id;
                        _orderserviceservice.Create(create_order_service);
                    }

                }
                //thong tin lich lam viec
                foreach (executorjson ex in c.list_executor)
                {
                    executor create_executor = new executor();
                    create_executor.customer_order_id = op_last.cuo_id;
                    create_executor.staff_id = ex.staff_id;
                    create_executor.work_time = ex.work_time;
                    create_executor.start_time = ex.start_time;
                    create_executor.end_time = ex.end_time;
                    create_executor.exe_flag_overtime = ex.exe_flag_overtime;
                    create_executor.exe_time_overtime = ex.exe_time_overtime;
                    create_executor.exe_status = ex.exe_status;
                    create_executor.exe_evaluate = ex.exe_evaluate;
                    create_executor.service_time_id = service_time_last.st_id;
                    create_executor.exe_note = ex.exe_note;
                    _executorservice.Create(create_executor);

                }
                
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = null;
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
        [Route("api/customer_order_service/update")]
        public async Task<IHttpActionResult> UpdateCustomerOrderService([FromBody] CustomerOrderServiceViewModelUpdate customer_order)
        {
            ResponseDataDTO<customer_order> response = new ResponseDataDTO<customer_order>();
            try
            {
                var c = customer_order;

                var current_id = BaseController.get_id_current();
                //Thoong tin khach hang 
                if (c.customer.cu_id == 0)
                {
                    #region[Create Customer]
                    #region["Check null"]

                    if (c.customer.cu_fullname == null || c.customer.cu_fullname.Trim() == "")
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Họ và tên không được để trống";
                        response.Error = "cu_fullname";
                        return Ok(response);
                    }
                    if (c.customer.cu_type == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Loại khách hàng không được để trống";
                        response.Error = "cu_type";
                        return Ok(response);
                    }

                    if (c.customer.customer_group_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nhóm khách hàng không được để trống";
                        response.Error = "customer_group_id";
                        return Ok(response);
                    }

                    if (c.customer.cu_flag_order == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Đặt dịch vụ không được để trống";
                        response.Error = "cu_flag_order";
                        return Ok(response);
                    }
                    if (c.customer.source_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nguồn khách hàng không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.cu_birthday == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Ngày sinh không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.cu_flag_used == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Sử dụng dịch vụ  không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }

                    if (c.customer.cu_email != null)
                    {
                        if (c.customer.cu_email.Trim() != "")
                        {
                            if (!Utilis.IsValidEmail(c.customer.cu_email))
                            {
                                response.Code = HttpCode.NOT_FOUND;
                                response.Message = "Email sai định dạng";
                                response.Error = "cu_email";
                                return Ok(response);
                            }
                        }

                    }
                    if (c.customer.sha_detail_now == null || c.customer.sha_detail_now.Trim() == "")
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Địa chỉ chi tiết không được để trống";
                        response.Error = "sha_detail_now";
                        return Ok(response);
                    }

                    #endregion
                    //Kiểm tra các trường rằng buộc
                    foreach (customer_phonejson cp in c.customer.list_customer_phone)
                    {
                        var temp = _customerphoneservice.GetAllIncluing(t => t.cp_phone_number.Equals(cp.cp_phone_number));
                        if (temp.Count() != 0)
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

                    customer_create.cu_fullname = c.customer.cu_fullname;
                    customer_create.customer_group_id = c.customer.customer_group_id;
                    customer_create.cu_birthday = c.customer.cu_birthday;
                    customer_create.cu_email = c.customer.cu_email;
                    customer_create.cu_flag_order = c.customer.cu_flag_order;
                    customer_create.cu_flag_used = c.customer.cu_flag_used;
                    customer_create.cu_status = c.customer.cu_status;
                    customer_create.source_id = c.customer.source_id;
                    customer_create.cu_note = c.customer.cu_note;
                    customer_create.cu_type = c.customer.cu_type;

                    //Thong tin lien he

                    //Lấy ra bản ghi cuối cùng tạo mã code 
                    var x = _customerservice.GetLast();
                    if (x == null) customer_create.cu_code = "KH000000";
                    else customer_create.cu_code = Utilis.CreateCodeByCode("KH", x.cu_code, 8);

                    customer_create.cu_create_date = DateTime.Now;
                    customer_create.staff_id = BaseController.get_id_current();
                    customer_create.cu_thumbnail = "/Uploads/Images/default/customer.png";
                    // save new customer
                    _customerservice.Create(customer_create);
                    customer customer_last1 = _customerservice.GetLast();
                    //save address hiện tại 
                    ship_address address_now = new ship_address();
                    address_now.customer_id = customer_last1.cu_id;
                    address_now.sha_ward = c.customer.sha_ward_now;
                    address_now.sha_province = c.customer.sha_province_now;
                    address_now.sha_district = c.customer.sha_district_now;
                    address_now.sha_geocoding = c.customer.sha_geocoding_now;
                    address_now.sha_detail = c.customer.sha_detail_now;
                    address_now.sha_note = c.customer.sha_note_now;
                    address_now.sha_flag_center = 1;
                    _shipaddressservice.Create(address_now);
                    //Save list sdt
                    foreach (customer_phonejson cup in c.customer.list_customer_phone)
                    {
                        customer_phone cup_create = new customer_phone();
                        cup_create.customer_id = customer_last1.cu_id;
                        cup_create.cp_type = cup.cp_type;
                        cup_create.cp_phone_number = cup.cp_phone_number;
                        cup_create.cp_note = cup.cp_note;
                        _customerphoneservice.Create(cup_create);
                    }
                    //Save shipaddress
                    foreach (customer_ship_addressjson add in c.customer.list_ship_address)
                    {
                        ship_address add_create = new ship_address();
                        add_create.customer_id = customer_last1.cu_id;
                        add_create.sha_ward = add.sha_ward;
                        add_create.sha_province = add.sha_province;
                        add_create.sha_district = add.sha_district;
                        add_create.sha_geocoding = add.sha_geocoding;
                        add_create.sha_detail = add.sha_detail;
                        add_create.sha_note = add.sha_note;
                        add_create.sha_flag_center = 0;
                        _shipaddressservice.Create(add_create);
                    }
                    c.customer.cu_id = customer_last1.cu_id;

                    #endregion
                }
                else
                {
                    #region[Updatecustomer]
                    #region["Check null"]

                    if (c.customer.cu_fullname == null || c.customer.cu_fullname.Trim() == "")
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Họ và tên không được để trống";
                        response.Error = "cu_fullname";
                        return Ok(response);
                    }
                    if (c.customer.cu_type == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Loại khách hàng không được để trống";
                        response.Error = "cu_type";
                        return Ok(response);
                    }

                    if (c.customer.customer_group_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nhóm khách hàng không được để trống";
                        response.Error = "customer_group_id";
                        return Ok(response);
                    }

                    if (c.customer.cu_flag_order == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Đặt dịch vụ không được để trống";
                        response.Error = "cu_flag_order";
                        return Ok(response);
                    }
                    if (c.customer.source_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nguồn khách hàng không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.cu_birthday == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Ngày sinh không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.cu_flag_used == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Sử dụng dịch vụ  không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.sha_detail_now == null || c.customer.sha_detail_now.Trim() == "")
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Địa chỉ chi tiết không được để trống";
                        response.Error = "sha_detail_now";
                        return Ok(response);
                    }

                    if (c.customer.cu_email != null)
                    {
                        if (c.customer.cu_email.Trim() != "")
                        {
                            if (!Utilis.IsValidEmail(c.customer.cu_email))
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
                    customer exists_customer = _customerservice.Find(c.customer.cu_id);
                    //Thong tin chung 

                    exists_customer.cu_fullname = c.customer.cu_fullname;
                    exists_customer.customer_group_id = c.customer.customer_group_id;
                    exists_customer.cu_birthday = c.customer.cu_birthday;
                    exists_customer.cu_email = c.customer.cu_email;
                    exists_customer.cu_flag_order = c.customer.cu_flag_order;
                    exists_customer.cu_flag_used = c.customer.cu_flag_used;
                    exists_customer.cu_status = c.customer.cu_status;
                    exists_customer.source_id = c.customer.source_id;
                    exists_customer.cu_note = c.customer.cu_note;
                    exists_customer.cu_type = c.customer.cu_type;

                    // save new customer
                    _customerservice.Update(exists_customer, exists_customer.cu_id);
                    //save address hiện tại 
                    ship_address exists_address = _shipaddressservice.GetAllIncluing(x => x.customer_id == c.customer.cu_id && x.sha_flag_center == 1).FirstOrDefault();

                    exists_address.sha_ward = c.customer.sha_ward_now;
                    exists_address.sha_province = c.customer.sha_province_now;
                    exists_address.sha_district = c.customer.sha_district_now;
                    exists_address.sha_geocoding = c.customer.sha_geocoding_now;
                    exists_address.sha_detail = c.customer.sha_detail_now;
                    exists_address.sha_note = c.customer.sha_note_now;
                    _shipaddressservice.Update(exists_address, exists_address.sha_id);
                    //Update list sdt
                    List<customer_phone> lts_cp_db = _customerphoneservice.GetAllIncluing(x => x.customer_id == c.customer.cu_id).ToList();
                    List<customer_phonejson> lts_cp_v = new List<customer_phonejson>(c.customer.list_customer_phone);
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
                            cup_create.customer_id = c.customer.cu_id;
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
                    List<ship_address> lts_sha_db = _shipaddressservice.GetAllIncluing(x => x.customer_id == c.customer.cu_id && x.sha_flag_center == 0).ToList();
                    List<customer_ship_addressjson> lts_sha_v = new List<customer_ship_addressjson>(c.customer.list_ship_address);
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
                            create_address.customer_id = c.customer.cu_id;
                            create_address.sha_flag_center = 0;
                            _shipaddressservice.Create(create_address);
                        }
                    }
                    foreach (ship_address sha_d in lts_sha_db)
                    {
                        _shipaddressservice.Delete(sha_d);
                    }

                    #endregion
                }
                var customer_last = _customerservice.GetLast();
                //Customer order service 
                customer_order customer_order_service = _customer_orderservice.Find(c.cuo_id);
                customer_order_service.customer_id = c.customer.cu_id;
                customer_order_service.cuo_discount = c.cuo_discount;
                customer_order_service.cuo_color_show = c.cuo_color_show;
                customer_order_service.cuo_infor_time = c.cuo_infor_time;
                customer_order_service.cuo_address = c.cuo_address;
                // save new customer_order
                _customer_orderservice.Update(customer_order_service, customer_order_service.cuo_id);
                #region update service time
                service_time update_set = _servicetimeservice.GetAllIncluing(x => x.customer_order_id == c.cuo_id).FirstOrDefault();

                update_set.st_start_time = c.st_start_time;
                update_set.st_end_time = c.st_end_time;
                update_set.st_start_date = c.st_start_date;
                update_set.st_end_date = c.st_end_date;
                update_set.st_repeat_type = c.st_repeat_type;
                update_set.st_sun_flag = c.st_sun_flag;
                update_set.st_mon_flag = c.st_mon_flag;
                update_set.st_tue_flag = c.st_tue_flag;
                update_set.st_wed_flag = c.st_wed_flag;
                update_set.st_thu_flag = c.st_thu_flag;
                update_set.st_fri_flag = c.st_fri_flag;
                update_set.st_sat_flag = c.st_sat_flag;
                update_set.st_repeat = c.st_repeat;
                update_set.st_repeat_every = c.st_repeat_every;
                update_set.st_on_the = c.st_on_the;
                update_set.st_on_day_flag = c.st_on_day_flag;
                update_set.st_on_day = c.st_on_day;
                update_set.st_on_the_flag = c.st_on_the_flag;
                update_set.st_custom_start = c.st_custom_start;
                if (c.st_custom_end == null)
                    update_set.st_custom_end = c.st_custom_start;
                else
                    update_set.st_custom_end = c.st_custom_end;
                _servicetimeservice.Update(update_set, update_set.st_id);

                #endregion
                //thong tin dich vu 
                //Xóa bản ghi cũ update cái mới 
                List<order_service> lts_order_service_db = _orderserviceservice.GetAllIncluing(x => x.customer_order_id == c.cuo_id).ToList();
                List<servicejson> lts_service_v = new List<servicejson>(c.list_service);
                foreach (servicejson tr_f in lts_service_v)
                {
                    if (Utilis.IsNumber(tr_f.se_id))
                    {
                        int temp = 0;
                        int _id = Convert.ToInt32(tr_f.se_id);
                        foreach (order_service tr in lts_order_service_db)
                        {
                            if (tr.service_id == _id)
                            {
                                //update
                                service exist_tr = _serviceservice.Find(tr.service_id);


                                _serviceservice.Update(exist_tr, exist_tr.se_id);

                                lts_order_service_db.Remove(tr);
                                temp = 1;
                                break;
                            }
                        }
                        if (temp == 0)
                        {
                            //Khi view trả về những cái chọn mà k có trong db thì thêm phần này 
                            order_service create_trs = new order_service();
                            create_trs.customer_order_id = c.cuo_id;
                            create_trs.service_id = _id;
                            _orderserviceservice.Create(create_trs);
                        }

                    }
                    else
                    {
                        //Create service 
                        service create_service = new service();
                        create_service.service_category_id = tr_f.service_category_id;
                        create_service.se_description = tr_f.se_description;
                        create_service.se_name = tr_f.se_name;
                        create_service.se_note = tr_f.se_note;
                        create_service.se_price = tr_f.se_price;
                        create_service.se_saleoff = tr_f.se_saleoff;
                        create_service.se_type = tr_f.se_type;


                        var t = _serviceservice.GetLast();
                        if (t == null) create_service.se_code = "DV000000";
                        else create_service.se_code = Utilis.CreateCodeByCode("DV", t.se_code, 8);
                        _serviceservice.Create(create_service);
                        service tr_last = _serviceservice.GetLast();
                        //Create service staff
                        order_service create_trs = new order_service();
                        create_trs.customer_order_id = c.cuo_id;
                        create_trs.service_id = tr_last.se_id;
                        _serviceservice.Create(tr_last);
                    }
                }
                foreach (order_service trs in lts_order_service_db)
                {
                    _orderserviceservice.Delete(trs);
                }

                //thong tin lich lam viec
                List<executor> lts_ul_db = _executorservice.GetAllIncluing(x => x.customer_order_id == c.cuo_id).ToList();
                List<executorjson> lts_ul_v = new List<executorjson>(c.list_executor);
                foreach (executorjson ul_f in lts_ul_v)
                {
                    if (Utilis.IsNumber(ul_f.exe_id))
                    {
                        int _id = Convert.ToInt32(ul_f.exe_id);
                        foreach (executor ul in lts_ul_db)
                        {
                            if (ul.exe_id == _id)
                            {
                                //update
                                executor exist_address = _executorservice.Find(_id);
                                exist_address.work_time = ul_f.work_time;
                                exist_address.staff_id = ul_f.staff_id;
                                exist_address.start_time = ul_f.start_time;
                                exist_address.end_time = ul_f.end_time;
                                exist_address.exe_flag_overtime = ul_f.exe_flag_overtime;
                                exist_address.exe_time_overtime = ul_f.exe_time_overtime;
                                exist_address.exe_status = ul_f.exe_status;
                                exist_address.exe_evaluate = ul_f.exe_evaluate;
                                exist_address.exe_note = ul_f.exe_note;
                                _executorservice.Update(exist_address, exist_address.exe_id);
                                lts_ul_db.Remove(_executorservice.Find(ul.exe_id));
                                break;
                            }
                        }
                    }
                    else
                    {
                        //Create
                        executor create_executor = new executor();
                        create_executor.customer_order_id = c.cuo_id;
                        create_executor.staff_id = ul_f.staff_id;
                        create_executor.work_time = ul_f.work_time;
                        create_executor.start_time = ul_f.start_time;
                        create_executor.end_time = ul_f.end_time;
                        create_executor.exe_flag_overtime = ul_f.exe_flag_overtime;
                        create_executor.exe_time_overtime = ul_f.exe_time_overtime;
                        create_executor.exe_status = ul_f.exe_status;
                        create_executor.exe_evaluate = ul_f.exe_evaluate;
                        create_executor.service_time_id = update_set.st_id;
                        create_executor.exe_note = ul_f.exe_note;
                        _executorservice.Create(create_executor);
                    }
                }
                foreach (executor ul_d in lts_ul_db)
                {
                    _executorservice.Delete(ul_d);
                }

                
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = null;
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
        [Route("api/customer-orders/update")]
        public async Task<IHttpActionResult> UpdateCustomerOder([FromBody] CustomerOrderProductViewModelUpdate customer_order_update)
        {
            ResponseDataDTO<bool> response = new ResponseDataDTO<bool>();

            try
            {

                //Id user now
                var c = customer_order_update;

                var current_id = BaseController.get_id_current();
                //Thoong tin khach hang 
                if (c.customer.cu_id == 0)
                {
                    #region[Create Customer]
                    #region["Check null"]

                    if (c.customer.cu_fullname == null || c.customer.cu_fullname.Trim() == "")
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Họ và tên không được để trống";
                        response.Error = "cu_fullname";
                        return Ok(response);
                    }
                    if (c.customer.cu_type == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Loại khách hàng không được để trống";
                        response.Error = "cu_type";
                        return Ok(response);
                    }

                    if (c.customer.customer_group_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nhóm khách hàng không được để trống";
                        response.Error = "customer_group_id";
                        return Ok(response);
                    }

                    if (c.customer.cu_flag_order == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Đặt dịch vụ không được để trống";
                        response.Error = "cu_flag_order";
                        return Ok(response);
                    }
                    if (c.customer.source_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nguồn khách hàng không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.cu_birthday == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Ngày sinh không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.cu_flag_used == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Sử dụng dịch vụ  không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }

                    if (c.customer.cu_email != null)
                    {
                        if (c.customer.cu_email.Trim() != "")
                        {
                            if (!Utilis.IsValidEmail(c.customer.cu_email))
                            {
                                response.Code = HttpCode.NOT_FOUND;
                                response.Message = "Email sai định dạng";
                                response.Error = "cu_email";
                                return Ok(response);
                            }
                        }

                    }
                    if (c.customer.sha_detail_now == null || c.customer.sha_detail_now.Trim() == "")
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Địa chỉ chi tiết không được để trống";
                        response.Error = "sha_detail_now";
                        return Ok(response);
                    }

                    #endregion
                    //Kiểm tra các trường rằng buộc
                    foreach (customer_phonejson cp in c.customer.list_customer_phone)
                    {
                        var temp = _customerphoneservice.GetAllIncluing(t => t.cp_phone_number.Equals(cp.cp_phone_number));
                        if (temp.Count() != 0)
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

                    customer_create.cu_fullname = c.customer.cu_fullname;
                    customer_create.customer_group_id = c.customer.customer_group_id;
                    customer_create.cu_birthday = c.customer.cu_birthday;
                    customer_create.cu_email = c.customer.cu_email;
                    customer_create.cu_flag_order = c.customer.cu_flag_order;
                    customer_create.cu_flag_used = c.customer.cu_flag_used;
                    customer_create.cu_status = c.customer.cu_status;
                    customer_create.source_id = c.customer.source_id;
                    customer_create.cu_note = c.customer.cu_note;
                    customer_create.cu_type = c.customer.cu_type;

                    //Thong tin lien he

                    //Lấy ra bản ghi cuối cùng tạo mã code 
                    var x = _customerservice.GetLast();
                    if (x == null) customer_create.cu_code = "KH000000";
                    else customer_create.cu_code = Utilis.CreateCodeByCode("KH", x.cu_code, 8);

                    customer_create.cu_create_date = DateTime.Now;
                    customer_create.staff_id = BaseController.get_id_current();
                    customer_create.cu_thumbnail = "/Uploads/Images/default/customer.png";
                    // save new customer
                    _customerservice.Create(customer_create);
                    customer customer_last1 = _customerservice.GetLast();
                    //save address hiện tại 
                    ship_address address_now = new ship_address();
                    address_now.customer_id = customer_last1.cu_id;
                    address_now.sha_ward = c.customer.sha_ward_now;
                    address_now.sha_province = c.customer.sha_province_now;
                    address_now.sha_district = c.customer.sha_district_now;
                    address_now.sha_geocoding = c.customer.sha_geocoding_now;
                    address_now.sha_detail = c.customer.sha_detail_now;
                    address_now.sha_note = c.customer.sha_note_now;
                    address_now.sha_flag_center = 1;
                    _shipaddressservice.Create(address_now);
                    //Save list sdt
                    foreach (customer_phonejson cup in c.customer.list_customer_phone)
                    {
                        customer_phone cup_create = new customer_phone();
                        cup_create.customer_id = customer_last1.cu_id;
                        cup_create.cp_type = cup.cp_type;
                        cup_create.cp_phone_number = cup.cp_phone_number;
                        cup_create.cp_note = cup.cp_note;
                        _customerphoneservice.Create(cup_create);
                    }
                    //Save shipaddress
                    foreach (customer_ship_addressjson add in c.customer.list_ship_address)
                    {
                        ship_address add_create = new ship_address();
                        add_create.customer_id = customer_last1.cu_id;
                        add_create.sha_ward = add.sha_ward;
                        add_create.sha_province = add.sha_province;
                        add_create.sha_district = add.sha_district;
                        add_create.sha_geocoding = add.sha_geocoding;
                        add_create.sha_detail = add.sha_detail;
                        add_create.sha_note = add.sha_note;
                        add_create.sha_flag_center = 0;
                        _shipaddressservice.Create(add_create);
                    }
                    c.customer.cu_id = customer_last1.cu_id;

                    #endregion
                }
                else
                {
                    #region[Updatecustomer]
                    #region["Check null"]

                    if (c.customer.cu_fullname == null || c.customer.cu_fullname.Trim() == "")
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Họ và tên không được để trống";
                        response.Error = "cu_fullname";
                        return Ok(response);
                    }
                    if (c.customer.cu_type == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Loại khách hàng không được để trống";
                        response.Error = "cu_type";
                        return Ok(response);
                    }

                    if (c.customer.customer_group_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nhóm khách hàng không được để trống";
                        response.Error = "customer_group_id";
                        return Ok(response);
                    }

                    if (c.customer.cu_flag_order == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Đặt dịch vụ không được để trống";
                        response.Error = "cu_flag_order";
                        return Ok(response);
                    }
                    if (c.customer.source_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nguồn khách hàng không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.cu_birthday == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Ngày sinh không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.cu_flag_used == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Sử dụng dịch vụ  không được để trống";
                        response.Error = "source_id";
                        return Ok(response);
                    }
                    if (c.customer.sha_detail_now == null || c.customer.sha_detail_now.Trim() == "")
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Địa chỉ chi tiết không được để trống";
                        response.Error = "sha_detail_now";
                        return Ok(response);
                    }

                    if (c.customer.cu_email != null)
                    {
                        if (c.customer.cu_email.Trim() != "")
                        {
                            if (!Utilis.IsValidEmail(c.customer.cu_email))
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
                    customer exists_customer = _customerservice.Find(c.customer.cu_id);
                    //Thong tin chung 

                    exists_customer.cu_fullname = c.customer.cu_fullname;
                    exists_customer.customer_group_id = c.customer.customer_group_id;
                    exists_customer.cu_birthday = c.customer.cu_birthday;
                    exists_customer.cu_email = c.customer.cu_email;
                    exists_customer.cu_flag_order = c.customer.cu_flag_order;
                    exists_customer.cu_flag_used = c.customer.cu_flag_used;
                    exists_customer.cu_status = c.customer.cu_status;
                    exists_customer.source_id = c.customer.source_id;
                    exists_customer.cu_note = c.customer.cu_note;
                    exists_customer.cu_type = c.customer.cu_type;

                    // save new customer
                    _customerservice.Update(exists_customer, exists_customer.cu_id);
                    //save address hiện tại 
                    ship_address exists_address = _shipaddressservice.GetAllIncluing(x => x.customer_id == c.customer.cu_id && x.sha_flag_center == 1).FirstOrDefault();

                    exists_address.sha_ward = c.customer.sha_ward_now;
                    exists_address.sha_province = c.customer.sha_province_now;
                    exists_address.sha_district = c.customer.sha_district_now;
                    exists_address.sha_geocoding = c.customer.sha_geocoding_now;
                    exists_address.sha_detail = c.customer.sha_detail_now;
                    exists_address.sha_note = c.customer.sha_note_now;
                    _shipaddressservice.Update(exists_address, exists_address.sha_id);
                    //Update list sdt
                    List<customer_phone> lts_cp_db = _customerphoneservice.GetAllIncluing(x => x.customer_id == c.customer.cu_id).ToList();
                    List<customer_phonejson> lts_cp_v = new List<customer_phonejson>(c.customer.list_customer_phone);
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
                            cup_create.customer_id = c.customer.cu_id;
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
                    List<ship_address> lts_sha_db = _shipaddressservice.GetAllIncluing(x => x.customer_id == c.customer.cu_id && x.sha_flag_center == 0).ToList();
                    List<customer_ship_addressjson> lts_sha_v = new List<customer_ship_addressjson>(c.customer.list_ship_address);
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
                            create_address.customer_id = c.customer.cu_id;
                            create_address.sha_flag_center = 0;
                            _shipaddressservice.Create(create_address);
                        }
                    }
                    foreach (ship_address sha_d in lts_sha_db)
                    {
                        _shipaddressservice.Delete(sha_d);
                    }

                    #endregion
                }


                var existscustomerorder = _customer_orderservice.Find(c.cuo_id);

                existscustomerorder.customer_id = c.customer.cu_id;
                existscustomerorder.staff_id = Convert.ToInt32(current_id);
                existscustomerorder.cuo_payment_status = c.cuo_payment_status;
                existscustomerorder.cuo_payment_type = c.cuo_payment_type;
                existscustomerorder.cuo_ship_tax = c.cuo_ship_tax;
                existscustomerorder.cuo_total_price = c.cuo_total_price;
                existscustomerorder.cuo_discount = c.cuo_discount;
                existscustomerorder.cuo_status = c.cuo_status;
                existscustomerorder.cuo_address = c.cuo_address;




                // update customer order
                _customer_orderservice.Update(existscustomerorder, existscustomerorder.cuo_id);
                var list_product_old = _order_productservice.GetAllIncluing(op => op.customer_order_id == existscustomerorder.cuo_id);
                foreach (order_product i in list_product_old)
                {
                    _order_productservice.Delete(i);
                }
                //update order product

                OrderProductCreateViewModel orderCreateViewModel = new OrderProductCreateViewModel { };

                foreach (productorderviewmodel i in c.list_product)
                {


                    orderCreateViewModel.customer_order_id = c.cuo_id;
                    orderCreateViewModel.op_discount = i.op_discount;
                    orderCreateViewModel.op_note = i.op_note;
                    orderCreateViewModel.op_quantity = i.op_quantity;
                    orderCreateViewModel.product_id = i.product_id;
                    orderCreateViewModel.op_total_value = i.op_total_value;


                    var createdorderproduct = _mapper.Map<order_product>(orderCreateViewModel);

                    _order_productservice.Create(createdorderproduct);
                }


                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = false;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = true;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }

        }

        [HttpPut]
        [Route("api/customer-orders/update-status")]
        public async Task<IHttpActionResult> UpdateStatusCustomerOrder()
        {
            ResponseDataDTO<customer_order> response = new ResponseDataDTO<customer_order>();
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);
                int cuo_id = Convert.ToInt32(streamProvider.FormData["cuo_id"]);
                var cuo_update = _customer_orderservice.Find(cuo_id);
                cuo_update.cuo_status = Convert.ToByte(streamProvider.FormData["cuo_status"]);
                // update address
                _customer_orderservice.Update(cuo_update, cuo_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = cuo_update;
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
        [Route("api/customer_orders/delete")]
        public IHttpActionResult Deletecustomer_order(int customer_orderId)
        {
            ResponseDataDTO<customer_order> response = new ResponseDataDTO<customer_order>();
            try
            {
                var customer_orderDeleted = _customer_orderservice.Find(customer_orderId);
                if (customer_orderDeleted != null)
                {
                    _customer_orderservice.Delete(customer_orderDeleted);

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
                    response.Message = "Không tìm thấy mã khách hàng order trong hệ thống.";
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
        [HttpDelete]
        [Route("api/customer_order_service/delete")]
        public IHttpActionResult DeleteCustomerOrderService(int cuo_id)
        {
            ResponseDataDTO<customer_order> response = new ResponseDataDTO<customer_order>();
            try
            {
                var customer_orderDeleted = _customer_orderservice.Find(cuo_id);
                if (customer_orderDeleted != null)
                {
                    _customer_orderservice.Delete(customer_orderDeleted);

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
                    response.Message = "Không tìm thấy mã khách hàng order trong hệ thống.";
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


        #region["Dịch vụ"]
        [HttpGet]
        [Route("api/customer-orders/service_by_date")]
        public IHttpActionResult GetServiceByDay(DateTime start_date, DateTime to_date)
        {
            ResponseDataDTO<List<order_service_view>> response = new ResponseDataDTO<List<order_service_view>>();
            try
            {
                int staff_id = BaseController.get_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customer_orderservice.GetServiceByDay(staff_id, start_date, to_date);
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

        #region["Export Excel"]
        [HttpGet]
        [Route("api/customer-order/export")]
        public async Task<IHttpActionResult> ExportCustomerOrder(int pageNumber, int pageSize, int? payment_type_id, DateTime? start_date, DateTime? end_date, string name)
        {
            ResponseDataDTO<string> response = new ResponseDataDTO<string>();
            try
            {
                var list_customer_order = new List<customerorderview>();

                //Đưa ra danh sách staff trong trang nào đó 
                var objRT_Mst_Customer_Order = _customer_orderservice.ExportCustomerOrder(pageNumber, pageSize, payment_type_id, start_date, end_date, name);
                if (objRT_Mst_Customer_Order != null)
                {
                    list_customer_order.AddRange(objRT_Mst_Customer_Order.Results);

                    Dictionary<string, string> dicColNames = GetImportDicColums();

                    string url = "";
                    string filePath = GenExcelExportFilePath(string.Format(typeof(customer_order).Name), ref url);

                    ExcelExport.ExportToExcelFromList(list_customer_order, dicColNames, filePath, string.Format("Đặt hàng"));
                    //Input: http://27.72.147.222:1230/TempFiles/2020-03-11/department_200311210940.xlsx
                    //"D:\\BootAi\\ERP20\\ERP.API\\TempFiles\\2020-03-12\\department_200312092643.xlsx"

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

        #region["DicColums"]
        private Dictionary<string, string> GetImportDicColums()
        {
            return new Dictionary<string, string>()
            {

                 {"cuo_code","MDH" },
                 {"cuo_date","Ngày tạo"},
                 {"cuo_total_price","Tổng tiền"},
                 {"cuo_status_name","Trạng thái đơn hàng"},
                 {"customer_name","Khách hàng"},
                 {"cuo_payment_type_name","Loại thanh toán"},
                 {"cuo_payment_status_name","Trạng thái thanh toán"},
                 {"cuo_ship_tax","Phí vận chuyển"},
                 {"staff_name","Người tạo đơn"},
                 {"cuo_address","Địa chỉ"},
                 {"cuo_note","Chú ý"}


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

        #region dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _customer_orderservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}