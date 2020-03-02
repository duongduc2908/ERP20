using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Transaction;
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
    public class ManagerTransactionController : BaseController
    {
        private readonly ITransactionService _transactionservice;

        private readonly IMapper _mapper;

        public ManagerTransactionController() { }
        public ManagerTransactionController(ITransactionService transactionservice, IMapper mapper)
        {
            this._transactionservice = transactionservice;
            this._mapper = mapper;
        }
        #region[method]
        [HttpGet]
        [Route("api/transactions/search")]
        public IHttpActionResult GetAllPageSearch(int pageNumber, int pageSize, string search_name)
        {
            ResponseDataDTO<PagedResults<transactionviewmodel>> response = new ResponseDataDTO<PagedResults<transactionviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _transactionservice.GetAllPageSearch(pageNumber: pageNumber, pageSize: pageSize, search_name: search_name);
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
        [Route("api/transactions/get_transaction_type")]
        public IHttpActionResult GetTransactionType()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _transactionservice.GetTransactionType();
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
        [Route("api/transactions/get_transaction_priority")]
        public IHttpActionResult GetTransactionPriority()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _transactionservice.GetTransactionPriority();
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
        [Route("api/transactions/get_transaction_status")]
        public IHttpActionResult GetTransactionStatus()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _transactionservice.GetTransactionStatus();
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
        [Route("api/transactions/create")]

        public async Task<IHttpActionResult> Createproduct()
        {
            ResponseDataDTO<transaction> response = new ResponseDataDTO<transaction>();
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
                TransactionCreateViewModel transactionCreateViewModel = new TransactionCreateViewModel
                {
                    tra_title = Convert.ToString(streamProvider.FormData["tra_title"]),

                    tra_content = Convert.ToString(streamProvider.FormData["tra_content"]),
                    tra_rate = Convert.ToString(streamProvider.FormData["tra_rate"]),
                    tra_type = Convert.ToByte(streamProvider.FormData["tra_type"]),

                    tra_datetime = Convert.ToDateTime(streamProvider.FormData["tra_datetime"]),
                    tra_result = Convert.ToString(streamProvider.FormData["tra_result"]),
                    tra_priority = Convert.ToByte(streamProvider.FormData["tra_priority"]),
                    staff_id = Convert.ToInt32(streamProvider.FormData["staff_id"]),
                    customer_id = Convert.ToInt32(streamProvider.FormData["customer_id"]),
                    tra_status = Convert.ToByte(streamProvider.FormData["tra_status"]),

                };
                
               
                //Create date
                transactionCreateViewModel.tra_datetime = DateTime.Now;

                // mapping view model to entity
                var createTransaction = _mapper.Map<transaction>(transactionCreateViewModel);


                // save new product
                _transactionservice.Create(createTransaction);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = createTransaction;
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
        [Route("api/transactions/update")]

        public async Task<IHttpActionResult> Updateproduct()
        {
            ResponseDataDTO<transaction> response = new ResponseDataDTO<transaction>();
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
                TransactionUpdateViewModel transactionUpdateViewModel = new TransactionUpdateViewModel
                {
                    tra_id = Convert.ToInt32(streamProvider.FormData["tra_id"]),
                    tra_title = Convert.ToString(streamProvider.FormData["tra_title"]),

                    tra_content = Convert.ToString(streamProvider.FormData["tra_content"]),
                    tra_rate = Convert.ToString(streamProvider.FormData["tra_rate"]),
                    tra_type = Convert.ToByte(streamProvider.FormData["tra_type"]),

                    
                    tra_result = Convert.ToString(streamProvider.FormData["tra_result"]),
                    tra_priority = Convert.ToByte(streamProvider.FormData["tra_priority"]),
                    staff_id = Convert.ToInt32(streamProvider.FormData["staff_id"]),
                    customer_id = Convert.ToInt32(streamProvider.FormData["customer_id"]),
                    tra_status = Convert.ToByte(streamProvider.FormData["tra_status"]),

                };
                var exitscurrent = _transactionservice.Find(transactionUpdateViewModel.tra_id);
                if(exitscurrent == null)
                {
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = MessageResponse.FAIL;
                    response.Data = null;
                    return Ok(response);
                }
                
                // mapping view model to entity
                var updatedtransaction = _mapper.Map<transaction>(transactionUpdateViewModel);
                updatedtransaction.tra_datetime = exitscurrent.tra_datetime;
                // update product
                _transactionservice.Update(updatedtransaction, transactionUpdateViewModel.tra_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = updatedtransaction;
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
        #region [Delete]
        [HttpDelete]
        [Route("api/transactions/delete")]
        public IHttpActionResult Deleteproduct(int transactionId)
        {
            ResponseDataDTO<transaction> response = new ResponseDataDTO<transaction>();
            try
            {
                var transactionDeleted = _transactionservice.Find(transactionId);
                if (transactionDeleted != null)
                {
                    _transactionservice.Delete(transactionDeleted);

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
                    response.Message = "Không thấy được sản phẩm";
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
        #endregion

    }
}