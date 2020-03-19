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
    public class ManagerOrderProductController : ApiController
    {
        private readonly IOrderProductService _order_productservice;

        private readonly IMapper _mapper;

        public ManagerOrderProductController() { }
        public ManagerOrderProductController(IOrderProductService order_productservice, IMapper mapper)
        {
            this._order_productservice = order_productservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/order-products/infor")]
        public IHttpActionResult GetInforById(int customer_order_id)
        {
            ResponseDataDTO<PagedResults<orderproductviewmodel>> response = new ResponseDataDTO<PagedResults<orderproductviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _order_productservice.GetAllOrderProduct(customer_order_id);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = "Lỗi tham số truyền";
                response.Data = null;

                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("api/order_products/page")]
        public IHttpActionResult Getorder_productsPaging(int pageSize, int pageNumber)
        {
            ResponseDataDTO<PagedResults<order_product>> response = new ResponseDataDTO<PagedResults<order_product>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _order_productservice.CreatePagedResults(pageNumber, pageSize);
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

        //[HttpPost]
        //[Route("api/order_products/create")]

        //public async Task<IHttpActionResult> Createorder_product()
        //{
        //    ResponseDataDTO<order_product> response = new ResponseDataDTO<order_product>();
        //    try
        //    {
        //        var path = Path.GetTempPath();

        //        if (!Request.Content.IsMimeMultipartContent("form-data"))
        //        {
        //            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
        //        }

        //        MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

        //        await Request.Content.ReadAsMultipartAsync(streamProvider);

        //        // get data from formdata
        //        OrderProductCreateViewModel order_productCreateViewModel = new OrderProductCreateViewModel
        //        {
        //            //op_code = Convert.ToString(streamProvider.FormData["op_code"]),
        //            op_note = Convert.ToString(streamProvider.FormData["op_note"]),
                   


        //            op_quantity = Convert.ToInt32(streamProvider.FormData["op_quantity"]),
        //            //staff_id = Convert.ToInt32(streamProvider.FormData["staff_id"]),
        //            product_id = Convert.ToInt32(streamProvider.FormData["product_id"]),
        //            //customer_id = Convert.ToInt32(streamProvider.FormData["customer_id"]),

        //            op_datetime = Convert.ToDateTime(streamProvider.FormData["op_datetime"]),

        //            op_status = Convert.ToByte(streamProvider.FormData["op_status"]),

        //        };

        //        // mapping view model to entity
        //        var createdorder_product = _mapper.Map<order_product>(order_productCreateViewModel);


        //        // save new order_product
        //        _order_productservice.Create(createdorder_product);
        //        // return response
        //        response.Code = HttpCode.OK;
        //        response.Message = MessageResponse.SUCCESS;
        //        response.Data = createdorder_product;
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
        //        response.Message = ex.Message;
        //        response.Data = null;
        //        Console.WriteLine(ex.ToString());

        //        return Ok(response);
        //    }

        //}


        //[HttpPut]
        //[Route("api/order_products/update")]

        //public async Task<IHttpActionResult> Updateorder_product(int? op_id)
        //{
        //    ResponseDataDTO<order_product> response = new ResponseDataDTO<order_product>();
        //    try
        //    {
        //        var path = Path.GetTempPath();

        //        if (!Request.Content.IsMimeMultipartContent("form-data"))
        //        {
        //            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
        //        }

        //        MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

        //        await Request.Content.ReadAsMultipartAsync(streamProvider);


        //        // get data from formdata
        //        OrderProductUpdateViewModel order_productUpdateViewModel = new OrderProductUpdateViewModel
        //        {

        //            //op_code = Convert.ToString(streamProvider.FormData["op_code"]),
        //            op_note = Convert.ToString(streamProvider.FormData["op_note"]),



        //            op_quantity = Convert.ToInt32(streamProvider.FormData["op_quantity"]),
        //            op_id = Convert.ToInt32(streamProvider.FormData["op_id"]),
        //            //staff_id = Convert.ToInt32(streamProvider.FormData["staff_id"]),
        //            product_id = Convert.ToInt32(streamProvider.FormData["product_id"]),
        //            //customer_id = Convert.ToInt32(streamProvider.FormData["customer_id"]),

        //            op_datetime = Convert.ToDateTime(streamProvider.FormData["op_datetime"]),

        //            op_status = Convert.ToByte(streamProvider.FormData["op_status"]),
        //        };



        //        // mapping view model to entity
        //        var updatedorder_product = _mapper.Map<order_product>(order_productUpdateViewModel);



        //        // update order_product
        //        _order_productservice.Update(updatedorder_product, op_id);
        //        // return response
        //        response.Code = HttpCode.OK;
        //        response.Message = MessageResponse.SUCCESS;
        //        response.Data = updatedorder_product;
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
        //        response.Message = ex.Message;
        //        response.Data = null;
        //        Console.WriteLine(ex.ToString());

        //        return Ok(response);
        //    }
        //}

        //[HttpDelete]
        //[Route("api/order_products/delete")]
        //public IHttpActionResult Deleteorder_product(int order_productId)
        //{
        //    ResponseDataDTO<order_product> response = new ResponseDataDTO<order_product>();
        //    try
        //    {
        //        var order_productDeleted = _order_productservice.Find(order_productId);
        //        if (order_productDeleted != null)
        //        {
        //            _order_productservice.Delete(order_productDeleted);

        //            // return response
        //            response.Code = HttpCode.OK;
        //            response.Message = MessageResponse.SUCCESS;
        //            response.Data = null;
        //            return Ok(response);
        //        }
        //        else
        //        {
        //            // return response
        //            response.Code = HttpCode.NOT_FOUND;
        //            response.Message = MessageResponse.FAIL;
        //            response.Data = null;

        //            return Ok(response);
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
        //        response.Message = ex.Message;
        //        response.Data = null;
        //        Console.WriteLine(ex.ToString());

        //        return Ok(response);
        //    }
        //}
        #endregion

        #region dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _order_productservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}