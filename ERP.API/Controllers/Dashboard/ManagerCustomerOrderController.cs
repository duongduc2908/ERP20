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

        public async Task<IHttpActionResult> Createcustomer_order([FromBody] order_product order_product)
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
                if(streamProvider.FormData["customer_id"] == null)
                {
                    CustomerCreateViewModel customerViewModel = new CustomerCreateViewModel
                    {
                        cu_fullname = Convert.ToString(streamProvider.FormData["cu_name"]),
                        cu_mobile = Convert.ToString(streamProvider.FormData["cu_mobile"]),
                        cu_email = Convert.ToString(streamProvider.FormData["cu_mail"]),

                        cu_type = Convert.ToByte(streamProvider.FormData["cu_type"]),
                        customer_group_id = Convert.ToInt32(streamProvider.FormData["customer_group_id"]),

                    };
                    customerViewModel.cu_create_date = DateTime.Now;
                    customerViewModel.cu_status = 1;
                    // mapping view model to entity
                    var createdcustomer = _mapper.Map<customer>(customerViewModel);

                    // save new customer_order
                    _customerservice.Create(createdcustomer);
                }
                // get data from formdata
                CustomerOrderCreateViewModel customer_orderCreateViewModel = new CustomerOrderCreateViewModel
                {
                    cuo_code = Convert.ToString(streamProvider.FormData["cuo_code"]),

                    cuo_total_price = Convert.ToInt32(streamProvider.FormData["cuo_total_price"]),
                    customer_id = Convert.ToInt32(streamProvider.FormData["customer_id"]),
                    
                    cuo_discount = Convert.ToInt32(streamProvider.FormData["cuo_discount"]),
                    cuo_ship_tax = Convert.ToInt32(streamProvider.FormData["cuo_ship_tax"]),
                    staff_id = Convert.ToInt32(streamProvider.FormData["staff_id"]),

                    cuo_payment_type = Convert.ToByte(streamProvider.FormData["cuo_payment_type"]),
                    cuo_status = Convert.ToByte(streamProvider.FormData["cuo_status"]),
                    cuo_payment_status = Convert.ToByte(streamProvider.FormData["cuo_payment_status"]),
                };
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