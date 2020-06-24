using ERP.Data.Dto;
using AutoMapper;
using ERP.API.Models;
using ERP.API.Providers;
using ERP.Common.Constants;
using ERP.Common.Excel;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;

using System.Web.Http;
using System.Web.Http.Cors;
using ERP.Data.ModelsERP.ModelView;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net;
using ERP.Extension.Extensions;
using System.Web.Script.Serialization;

namespace ERP.API.Controllers.Dashboard
{
    public class ManagerBankController : ApiController
    {
        // GET: ManagerCompany
        private readonly IBankService _bankservice;
        private readonly IBankCategoryService _bankcategoryservice;
        private readonly IBankBranchService _bankbranchservice;

        private readonly IMapper _mapper;

        public ManagerBankController() { }
        public ManagerBankController(IBankBranchService bankbranchservice, IBankCategoryService bankcategoryservice,IBankService bankservice, IMapper mapper)
        {
            this._bankservice = bankservice;
            this._bankcategoryservice = bankcategoryservice;
            this._bankbranchservice = bankbranchservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/bank/getall")]
        public IHttpActionResult GetAllName(int? bank_category_id, string search)
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _bankservice.GetAllDropDown(bank_category_id,search);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = MessageResponse.FAIL;
                response.Data = null;

                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }
        /*
        [HttpGet]
        [Route("api/bank/search")]
        public IHttpActionResult Getcompanys(int pageNumber, int pageSize, string search_name)
        {
            ResponseDataDTO<PagedResults<bank>> response = new ResponseDataDTO<PagedResults<bank>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _bankservice.GetAllSearch(pageNumber, pageSize, search_name);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = MessageResponse.FAIL;
                response.Data = null;

                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }
        [HttpGet]
        [Route("api/bank/get_by_id")]
        public IHttpActionResult GetById(int ba_id)
        {
            ResponseDataDTO<bank> response = new ResponseDataDTO<bank>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _bankservice.GetById(ba_id);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = MessageResponse.FAIL;
                response.Data = null;

                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }
        */
        //[HttpPost]
        //[Route("api/bank/create")]

        //public async Task<IHttpActionResult> Create()
        //{


        //    ResponseDataDTO<string> response = new ResponseDataDTO<string>();
        //    try
        //    {

        //        string json = string.Empty;
                
        //        // read json string from file
        //        using (StreamReader reader = new StreamReader("D:/ERP20/ERP.API/Uploads/bankList.txt"))
        //        {
        //            json = reader.ReadToEnd();
        //        }

        //        JavaScriptSerializer jss = new JavaScriptSerializer();

        //        // convert json string to dynamic type
        //        var obj = jss.Deserialize<dynamic>(json);

        //        foreach(var js in obj)
        //        {
        //            bank_category bc = new bank_category();
        //            bc.bac_name = js["TenLoaiNganHang"];
        //            //bc.bac_id = Convert.ToInt32(js["Loai"]);
        //            _bankcategoryservice.Create(bc);
        //            var _id = _bankcategoryservice.GetLast().bac_id;
        //            foreach (var ba in js["bankList"])
        //            {
        //                bank create_ba = new bank();
        //                create_ba.ba_code = ba["MaNganHang"];
        //                //create_ba.ba_id = Convert.ToInt32(ba["MaNganHang"]);
        //                create_ba.ba_name = ba["TenNH"];
        //                create_ba.bank_category_id = _id;
        //                _bankservice.Create(create_ba);
        //                try
        //                {
        //                    var id = _bankservice.GetLast().ba_id;
        //                    foreach (var ba_br in ba["province"])
        //                    {
        //                        bank_branch create_ba_br = new bank_branch();
        //                        create_ba_br.bank_id = id;
        //                        //create_ba_br.bbr_id =Convert.ToInt32(ba_br["Code"]);
        //                        create_ba_br.province_id = Convert.ToInt32(ba_br["Code"]);
        //                        foreach (var bb in ba_br["branch"])
        //                        {
        //                            create_ba_br.bbr_name = bb["TenChiNhanh"];
        //                            create_ba_br.bbr_code = bb["MaChiNhanh"];
        //                            create_ba_br.bbr_address = bb["DiaChi"];
        //                            try
        //                            {
        //                                _bankbranchservice.Create(create_ba_br);
        //                            }
        //                            catch(Exception exx)
        //                            {
        //                                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
        //                                response.Message = bb["TenChiNhanh"];
        //                                response.Data = null;

        //                                return Ok(response);
        //                            }
        //                        }

        //                    }
        //                }
        //                catch (Exception ec)
        //                {
        //                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
        //                    response.Message = "sai provice";
        //                    response.Data = null;

        //                    return Ok(response);
        //                }
                        


        //            }

        //        }
        //        // return response
        //        response.Code = HttpCode.OK;
        //        response.Message = MessageResponse.SUCCESS;
        //        response.Data = "Thanh cong";
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


        [HttpPut]
        [Route("api/bank/update")]

        public async Task<IHttpActionResult> Update()
        {
            ResponseDataDTO<bank> response = new ResponseDataDTO<bank>();
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);
                int ba_id = Convert.ToInt32(streamProvider.FormData["ba_id"]);
                bank exists = _bankservice.Find(ba_id);
                exists.ba_name = Convert.ToString(streamProvider.FormData["ba_name"]);
                exists.ba_description = Convert.ToString(streamProvider.FormData["ba_description"]);
                exists.bank_category_id = Convert.ToInt32(streamProvider.FormData["bank_category_id"]);


                // update bank
                _bankservice.Update(exists, exists.ba_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = exists;
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
        [Route("api/bank/delete")]
        public IHttpActionResult Deletebank(int ba_id)
        {
            ResponseDataDTO<bank> response = new ResponseDataDTO<bank>();
            try
            {
                var bankDelete = _bankservice.Find(ba_id);
                if (bankDelete != null)
                {
                    _bankservice.Delete(bankDelete);

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
    }
}