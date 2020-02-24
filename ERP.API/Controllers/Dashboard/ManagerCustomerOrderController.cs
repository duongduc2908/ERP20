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
    public class ManagerCustomerOrderController : ApiController
    {
        private readonly ICustomerOrderService _customer_orderservice;
        private readonly ICustomerService _customerservice;

        private readonly IMapper _mapper;

        public ManagerCustomerOrderController() 
        {
            
        }
        public ManagerCustomerOrderController(ICustomerOrderService customer_orderservice, ICustomerService _customerservice, IMapper mapper)
        {
            this._customer_orderservice = customer_orderservice;
            this._customerservice = _customerservice;
            this._mapper = mapper;
        }


        #region methods
        [HttpGet]
        [Route("api/customer-orders/infor")]
        public IHttpActionResult GetInforById(int id)
        {
            ResponseDataDTO<PagedResults<customer_order>> response = new ResponseDataDTO<PagedResults<customer_order>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customer_orderservice.GetAllOrderById(id);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = "Không tìm thấy nhân sự";
                response.Data = null;

                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }
        [HttpGet]
        [Route("api/customer-orders/search")]
        public IHttpActionResult GetAllSearch(int pageNumber, int pageSize, int? payment_type_id, string code)
        {
            ResponseDataDTO<PagedResults<customerorderviewmodel>> response = new ResponseDataDTO<PagedResults<customerorderviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _customer_orderservice.GetAllSearch(pageNumber:pageNumber, pageSize:pageSize, payment_type_id:payment_type_id, code);
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
        [HttpPost]
        [Route("api/customer-orders/test")]
        public IHttpActionResult Test([FromBody] CustomerOrderProductViewModel customer_order)
        {
            var c= customer_order;
            ResponseDataDTO<PagedResults<customerorderviewmodel>> response = new ResponseDataDTO<PagedResults<customerorderviewmodel>>();
            
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
        [Route("api/customer-orders/create")]

        public async Task<IHttpActionResult> Createcustomer_order([FromBody] CustomerOrderProductViewModel customer_order)
        {
            ResponseDataDTO<customer_order> response = new ResponseDataDTO<customer_order>();
            try
            {
                var c = customer_order;
                //Id user now
                new BaseController();
                var current_id = BaseController.current_id;
                if (c.customer.cu_id == 0)
                {
                    //Cach truong bat buoc 
                    if (c.customer.cu_fullname == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Họ và tên không được để trống";
                        response.Data = null;
                        return Ok(response);
                    }
                    if (c.customer.cu_mobile == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Số điện thoại không được để trống";
                        response.Data = null;
                        return Ok(response);
                    }
                    if (c.customer.cu_email == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Email không được để trống";
                        response.Data = null;
                        return Ok(response);
                    }
                    if (c.customer.cu_type == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Loại khách hàng không được để trống";
                        response.Data = null;
                        return Ok(response);
                    }
                    if (c.customer.customer_group_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nhóm khách hàng không được để trống";
                        response.Data = null;
                        return Ok(response);
                    }
                    if (c.customer.source_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nguồn không được để trống";
                        response.Data = null;
                        return Ok(response);
                    }
                    // get data from formdata
                    CustomerCreateViewModel customerCreateViewModel = new CustomerCreateViewModel
                    {
                        cu_mobile = Convert.ToString(c.customer.cu_mobile),
                        cu_email = Convert.ToString(c.customer.cu_email),
                        cu_fullname = Convert.ToString(c.customer.cu_fullname),

                        customer_group_id = Convert.ToInt32(c.customer.customer_group_id),
                        source_id = Convert.ToInt32(c.customer.source_id),

                        cu_type = Convert.ToByte(c.customer.cu_type),

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
                    if (c.customer.cu_birthday == null)
                    {
                        customerCreateViewModel.cu_birthday = null;
                    }
                    else
                    {
                        customerCreateViewModel.cu_birthday = Convert.ToDateTime(c.customer.cu_birthday);
                    }
                    if (c.customer.cu_address == null)
                    {
                        customerCreateViewModel.cu_address = null;
                    }
                    else
                    {
                        customerCreateViewModel.cu_address = Convert.ToString(c.customer.cu_address);
                    }
                    if (c.customer.cu_note == null)
                    {
                        customerCreateViewModel.cu_note = null;
                    }
                    else
                    {
                        customerCreateViewModel.cu_note = Convert.ToString(c.customer.cu_note);
                    }
                    if (c.customer.cu_geocoding == null)
                    {
                        customerCreateViewModel.cu_geocoding = null;
                    }
                    else
                    {
                        customerCreateViewModel.cu_geocoding = Convert.ToString(c.customer.cu_geocoding);
                    }
                    if (c.customer.cu_curator_id == 0)
                    {
                        customerCreateViewModel.cu_curator_id = null;
                    }
                    else
                    {
                        customerCreateViewModel.cu_curator_id = Convert.ToInt32(c.customer.cu_curator_id);
                    }
                    if (c.customer.cu_age == 0)
                    {
                        customerCreateViewModel.cu_age = null;
                    }
                    else
                    {
                        customerCreateViewModel.cu_age = Convert.ToInt32(c.customer.cu_age);
                    }
                    if (c.customer.cu_status == 0)
                    {
                        customerCreateViewModel.cu_status = null;
                    }
                    else
                    {
                        customerCreateViewModel.cu_status = Convert.ToByte(c.customer.cu_status);
                    }
                    
                    customerCreateViewModel.staff_id = Convert.ToInt32(current_id);
                    customerCreateViewModel.cu_create_date = DateTime.Now;
                    var cu = _customerservice.GetLast();
                    customerCreateViewModel.cu_code = Utilis.CreateCode("CU", cu.cu_id, 7);
                    // mapping view model to entity
                    var createdcustomer = _mapper.Map<customer>(customerCreateViewModel);
                    
                    // save new customer
                    _customerservice.Create(createdcustomer);
                    var cu_last = _customerservice.GetLast();
                    c.customer.cu_id = cu_last.cu_id;
                }

                // get data from formdata
                CustomerOrderCreateViewModel customer_orderCreateViewModel = new CustomerOrderCreateViewModel
                {
                    //cuo_code = Convert.ToString(c.customer.cuo_code),

                    //cuo_total_price = Convert.ToInt32(c.customer.cuo_total_price),
                    
                    
                    //cuo_discount = Convert.ToInt32(c.customer.cuo_discount),
                    //cuo_ship_tax = Convert.ToInt32(c.customer.cuo_ship_tax),
                    

                    //cuo_payment_type = Convert.ToByte(c.customer.cuo_payment_type),
                    //cuo_status = Convert.ToByte(c.customer.cuo_status),
                    //cuo_payment_status = Convert.ToByte(c.customer.cuo_payment_status),
                };
                customer_orderCreateViewModel.customer_id = c.customer.cu_id;
                customer_orderCreateViewModel.staff_id = Convert.ToInt32(current_id);
                customer_orderCreateViewModel.cuo_payment_status = c.cuo_payment_status;
                customer_orderCreateViewModel.cuo_payment_type = c.cuo_payment_type;
                customer_orderCreateViewModel.cuo_ship_tax = c.cuo_ship_tax;
                customer_orderCreateViewModel.cuo_total_price = c.cuo_total_price;
                customer_orderCreateViewModel.cuo_discount = c.cuo_discount;
                customer_orderCreateViewModel.cuo_status = c.cuo_status;



                customer_orderCreateViewModel.cuo_date = DateTime.Now;

                // mapping view model to entity
                var createdcustomer_order = _mapper.Map<customer_order>(customer_orderCreateViewModel);


                // save new customer_order
                _customer_orderservice.Create(createdcustomer_order);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = createdcustomer_order;
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
        [Route("api/customer_orders/update")]

        public async Task<IHttpActionResult> Updatecustomer_order(int? ema_id)
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


                // get data from formdata
                CustomerOrderUpdateViewModel customer_orderUpdateViewModel = new CustomerOrderUpdateViewModel
                {
                    

                };



                // mapping view model to entity
                var updatedcustomer_order = _mapper.Map<customer_order>(customer_orderUpdateViewModel);



                // update customer_order
                _customer_orderservice.Update(updatedcustomer_order, ema_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = updatedcustomer_order;
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
                _customer_orderservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}