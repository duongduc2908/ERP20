using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Excel;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.ExportDB;
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
        private readonly ICustomerService _customerservice;
        private readonly IMapper _mapper;

        public ManagerTransactionController() { }
        public ManagerTransactionController(ITransactionService transactionservice, ICustomerService customerservice, IMapper mapper)
        {
            this._transactionservice = transactionservice;
            this._customerservice = customerservice;
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
        [HttpGet]
        [Route("api/transaction/get_transaction_rate")]
        public IHttpActionResult GetTransactionRate()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _transactionservice.GetTransactionRate();
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

        public async Task<IHttpActionResult> CreateTransaction ([FromBody] TransactionViewModelCreate transaction)
        {
            ResponseDataDTO<transaction> response = new ResponseDataDTO<transaction>();
            try
            {
                var tra = transaction;
                var current_id = BaseController.get_id_current();
                if (tra.customer.cu_id == 0)
                {
                    #region[Create Customer]
                    //Cach truong bat buoc 
                    if (tra.customer.cu_fullname == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Họ và tên không được để trống";
                        response.Data = null;
                        return Ok(response);
                    }
                    if (tra.customer.cu_mobile == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Số điện thoại không được để trống";
                        response.Data = null;
                        return Ok(response);
                    }
                    if (tra.customer.cu_email == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Email không được để trống";
                        response.Data = null;
                        return Ok(response);
                    }
                    if (tra.customer.cu_type == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Loại khách hàng không được để trống";
                        response.Data = null;
                        return Ok(response);
                    }
                    if (tra.customer.customer_group_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nhóm khách hàng không được để trống";
                        response.Data = null;
                        return Ok(response);
                    }
                    if (tra.customer.source_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nguồn không được để trống";
                        response.Data = null;
                        return Ok(response);
                    }
                    // get data from formdata
                    CustomerCreateViewModel customerCreateViewModel = new CustomerCreateViewModel
                    {
                        cu_mobile = Convert.ToString(tra.customer.cu_mobile),
                        cu_email = Convert.ToString(tra.customer.cu_email),
                        cu_fullname = Convert.ToString(tra.customer.cu_fullname),

                        customer_group_id = Convert.ToInt32(tra.customer.customer_group_id),
                        source_id = Convert.ToInt32(tra.customer.source_id),

                        cu_type = Convert.ToByte(tra.customer.cu_type),

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
                    if (tra.customer.cu_birthday == null)
                    {
                        customerCreateViewModel.cu_birthday = null;
                    }
                    else
                    {
                        customerCreateViewModel.cu_birthday = Convert.ToDateTime(tra.customer.cu_birthday);
                    }
                    if (tra.customer.cu_address == null)
                    {
                        customerCreateViewModel.cu_address = null;
                    }
                    else
                    {
                        customerCreateViewModel.cu_address = Convert.ToString(tra.customer.cu_address);
                    }
                    if (tra.customer.cu_note == null)
                    {
                        customerCreateViewModel.cu_note = null;
                    }
                    else
                    {
                        customerCreateViewModel.cu_note = Convert.ToString(tra.customer.cu_note);
                    }
                    if (tra.customer.cu_geocoding == null)
                    {
                        customerCreateViewModel.cu_geocoding = null;
                    }
                    else
                    {
                        customerCreateViewModel.cu_geocoding = Convert.ToString(tra.customer.cu_geocoding);
                    }
                    if (tra.customer.cu_curator_id == 0)
                    {
                        customerCreateViewModel.cu_curator_id = null;
                    }
                    else
                    {
                        customerCreateViewModel.cu_curator_id = Convert.ToInt32(tra.customer.cu_curator_id);
                    }
                    if (tra.customer.cu_age == 0)
                    {
                        customerCreateViewModel.cu_age = null;
                    }
                    else
                    {
                        customerCreateViewModel.cu_age = Convert.ToInt32(tra.customer.cu_age);
                    }
                    if (tra.customer.cu_status == 0)
                    {
                        customerCreateViewModel.cu_status = null;
                    }
                    else
                    {
                        customerCreateViewModel.cu_status = Convert.ToByte(tra.customer.cu_status);
                    }

                    customerCreateViewModel.staff_id = Convert.ToInt32(current_id);
                    customerCreateViewModel.cu_create_date = DateTime.Now;
                    var cu = _customerservice.GetLast();
                    if (cu == null) customerCreateViewModel.cu_code = Utilis.CreateCode("CU", 0, 7);
                    else customerCreateViewModel.cu_code = Utilis.CreateCode("CU", cu.cu_id, 7);
                    // mapping view model to entity
                    var createdcustomer = _mapper.Map<customer>(customerCreateViewModel);

                    // save new customer
                    _customerservice.Create(createdcustomer);
                    var cu_last = _customerservice.GetLast();
                    tra.customer.cu_id = cu_last.cu_id;
                    #endregion
                }

                // get data from formdata
                TransactionCreateViewModel transactionCreateViewModel = new TransactionCreateViewModel();
                
                transactionCreateViewModel.tra_content = tra.tra_content;
                transactionCreateViewModel.tra_title = tra.tra_title;
                transactionCreateViewModel.tra_rate= tra.tra_rate;
                transactionCreateViewModel.tra_type= tra.tra_type;
                
                transactionCreateViewModel.tra_result= tra.tra_result;
                transactionCreateViewModel.tra_priority= tra.tra_priority;
                transactionCreateViewModel.staff_id = current_id;
                transactionCreateViewModel.customer_id= tra.customer.cu_id;
                transactionCreateViewModel.tra_status= tra.tra_status;

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

        public async Task<IHttpActionResult> UpdateTransaction([FromBody] TransactionViewModelUpdate transaction_update)
        {
            ResponseDataDTO<bool> response = new ResponseDataDTO<bool>();
            try
            { //Id user now
                new BaseController();
                var current_id = BaseController.get_id_current();
                if (transaction_update.customer.cu_id == 0)
                {
                    #region[Create Customer]
                    //Cach truong bat buoc 
                    if (transaction_update.customer.cu_fullname == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Họ và tên không được để trống";
                        response.Data = false;
                        return Ok(response);
                    }
                    if (transaction_update.customer.cu_mobile == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Số điện thoại không được để trống";
                        response.Data = false;
                        return Ok(response);
                    }
                    if (transaction_update.customer.cu_email == null)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Email không được để trống";
                        response.Data = false;
                        return Ok(response);
                    }
                    if (transaction_update.customer.cu_type == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Loại khách hàng không được để trống";
                        response.Data = false;
                        return Ok(response);
                    }
                    if (transaction_update.customer.customer_group_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nhóm khách hàng không được để trống";
                        response.Data = false;
                        return Ok(response);
                    }
                    if (transaction_update.customer.source_id == 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Nguồn không được để trống";
                        response.Data = false;
                        return Ok(response);
                    }
                    // get data from formdata
                    CustomerCreateViewModel customerCreateViewModel = new CustomerCreateViewModel
                    {
                        cu_mobile = Convert.ToString(transaction_update.customer.cu_mobile),
                        cu_email = Convert.ToString(transaction_update.customer.cu_email),
                        cu_fullname = Convert.ToString(transaction_update.customer.cu_fullname),

                        customer_group_id = Convert.ToInt32(transaction_update.customer.customer_group_id),
                        source_id = Convert.ToInt32(transaction_update.customer.source_id),

                        cu_type = Convert.ToByte(transaction_update.customer.cu_type),

                    };
                    //Bat cac dieu kien rang buoc
                    if (CheckEmail.IsValidEmail(customerCreateViewModel.cu_email) == false && customerCreateViewModel.cu_email == "")
                    {
                        response.Message = "Định dạng email không hợp lệ !";
                        response.Data = false;
                        return Ok(response);
                    }

                    if (CheckNumber.IsPhoneNumber(customerCreateViewModel.cu_mobile) == false && customerCreateViewModel.cu_mobile == "")
                    {
                        response.Message = "Số điện thoại không hợp lệ";
                        response.Data = false;
                        return Ok(response);
                    }


                    //bat cac truog con lai 
                    if (transaction_update.customer.cu_birthday == null)
                    {
                        customerCreateViewModel.cu_birthday = null;
                    }
                    else
                    {
                        customerCreateViewModel.cu_birthday = Convert.ToDateTime(transaction_update.customer.cu_birthday);
                    }
                    if (transaction_update.customer.cu_address == null)
                    {
                        customerCreateViewModel.cu_address = null;
                    }
                    else
                    {
                        customerCreateViewModel.cu_address = Convert.ToString(transaction_update.customer.cu_address);
                    }
                    if (transaction_update.customer.cu_note == null)
                    {
                        customerCreateViewModel.cu_note = null;
                    }
                    else
                    {
                        customerCreateViewModel.cu_note = Convert.ToString(transaction_update.customer.cu_note);
                    }
                    if (transaction_update.customer.cu_geocoding == null)
                    {
                        customerCreateViewModel.cu_geocoding = null;
                    }
                    else
                    {
                        customerCreateViewModel.cu_geocoding = Convert.ToString(transaction_update.customer.cu_geocoding);
                    }
                    if (transaction_update.customer.cu_curator_id == 0)
                    {
                        customerCreateViewModel.cu_curator_id = null;
                    }
                    else
                    {
                        customerCreateViewModel.cu_curator_id = Convert.ToInt32(transaction_update.customer.cu_curator_id);
                    }
                    if (transaction_update.customer.cu_age == 0)
                    {
                        customerCreateViewModel.cu_age = null;
                    }
                    else
                    {
                        customerCreateViewModel.cu_age = Convert.ToInt32(transaction_update.customer.cu_age);
                    }
                    if (transaction_update.customer.cu_status == 0)
                    {
                        customerCreateViewModel.cu_status = null;
                    }
                    else
                    {
                        customerCreateViewModel.cu_status = Convert.ToByte(transaction_update.customer.cu_status);
                    }

                    customerCreateViewModel.staff_id = Convert.ToInt32(current_id);
                    customerCreateViewModel.cu_create_date = DateTime.Now;
                    var cu = _customerservice.GetLast();
                    if (cu == null) customerCreateViewModel.cu_code = Utilis.CreateCode("CU", 0, 7);
                    else customerCreateViewModel.cu_code = Utilis.CreateCode("CU", cu.cu_id, 7);
                    // mapping view model to entity
                    var createdcustomer = _mapper.Map<customer>(customerCreateViewModel);

                    // save new customer
                    _customerservice.Create(createdcustomer);
                    var cu_last = _customerservice.GetLast();
                    transaction_update.customer.cu_id = cu_last.cu_id;
                    #endregion
                }


                var existstransaction = _transactionservice.Find(transaction_update.tra_id);

                existstransaction.tra_content = transaction_update.tra_content;
                existstransaction.tra_title = transaction_update.tra_title;
                existstransaction.tra_rate = transaction_update.tra_rate;
                existstransaction.tra_type = transaction_update.tra_type;

                existstransaction.tra_result = transaction_update.tra_result;
                existstransaction.tra_priority = transaction_update.tra_priority;
                existstransaction.staff_id = current_id;
                existstransaction.customer_id = transaction_update.customer.cu_id;
                existstransaction.tra_status = transaction_update.tra_status;

               
                // update product
                _transactionservice.Update(existstransaction, existstransaction.tra_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;
                response.Data = false;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }
        }
        [HttpPut]
        [Route("api/transaction/update-status")]
        public async Task<IHttpActionResult> UpdateStatusTransaction()
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
                int tra_id = Convert.ToInt32(streamProvider.FormData["tra_id"]);
                var tra_update = _transactionservice.Find(tra_id);
                tra_update.tra_status = Convert.ToByte(streamProvider.FormData["tra_status"]);
                // update address
                _transactionservice.Update(tra_update, tra_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = tra_update;
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
        #region["Export Excel"]
        [HttpGet]
        [Route("api/transaction/export")]
        public async Task<IHttpActionResult> ExportTransaction(int pageNumber, int pageSize, string search_name)
        {
            ResponseDataDTO<string> response = new ResponseDataDTO<string>();
            try
            {
                var listTransaction = new List<transactionview>();

                //Đưa ra danh sách staff trong trang nào đó 
                var objRT_Mst_Transaction = _transactionservice.ExportTransaction(pageNumber: pageNumber, pageSize: pageSize, search_name);
                if (objRT_Mst_Transaction != null)
                {
                    listTransaction.AddRange(objRT_Mst_Transaction.Results);

                    Dictionary<string, string> dicColNames = GetImportDicColums();

                    string url = "";
                    string filePath = GenExcelExportFilePath(string.Format(typeof(transaction).Name), ref url);

                    ExcelExport.ExportToExcelFromList(listTransaction, dicColNames, filePath, string.Format("Chăm sóc khách hàng"));
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
                 {"tra_title","Tiêu đề" },
                 {"tra_content","Nội dung"},
                 {"tra_rate_name","Đánh giá"},
                 {"tra_type_name","Loại giao dịch"},
                 {"tra_datetime","Thời gian"},
                 {"tra_result","Kết quả"},
                 {"tra_priority_name","Mức độ ưu tiên"},
                 {"staff_name","Người giao việc"},
                 {"customer_name","Khách hàng"},
                 {"tra_status_name","Trạng thái"},
                
            };
        }
       
        #endregion

        #endregion

    }
}