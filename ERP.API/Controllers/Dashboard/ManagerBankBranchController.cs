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

namespace ERP.API.Controllers.Dashboard
{
    public class ManagerBankBranchController : ApiController
    {
        // GET: ManagerCompany
        private readonly IBankBranchService _bankbranchservice;

        private readonly IMapper _mapper;

        public ManagerBankBranchController() { }
        public ManagerBankBranchController(IBankBranchService bankbranchservice, IMapper mapper)
        {
            this._bankbranchservice = bankbranchservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/bank_branch/getall")]
        public IHttpActionResult GetAllName(int? bank_id,string search)
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _bankbranchservice.GetAllDropDown(bank_id, search);
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
        [Route("api/bank_branch/search")]
        public IHttpActionResult Getcompanys(int pageNumber, int pageSize, string search_name)
        {
            ResponseDataDTO<PagedResults<bank_branch>> response = new ResponseDataDTO<PagedResults<bank_branch>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _bank_branchservice.GetAllSearch(pageNumber, pageSize, search_name);
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
        [Route("api/bank_branch/get_by_id")]
        public IHttpActionResult GetById(int ba_id)
        {
            ResponseDataDTO<bank_branch> response = new ResponseDataDTO<bank_branch>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _bank_branchservice.GetById(ba_id);
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
        [HttpPost]
        [Route("api/bank_branch/create")]

        public async Task<IHttpActionResult> Create()
        {
            ResponseDataDTO<bank_branch> response = new ResponseDataDTO<bank_branch>();
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);

                bank_branch create_bank_branch = new bank_branch();
                create_bank_branch.bbr_name = Convert.ToString(streamProvider.FormData["bbr_name"]);
                create_bank_branch.bbr_description = Convert.ToString(streamProvider.FormData["bbr_description"]);
                create_bank_branch.province_id = Convert.ToInt32(streamProvider.FormData["province_id"]);
                create_bank_branch.bbr_address = Convert.ToString(streamProvider.FormData["bbr_address"]);
                create_bank_branch.bank_id= Convert.ToInt32(streamProvider.FormData["bank_id"]);

                //Tạo mã code
                var x = _bankbranchservice.GetLast();
                if (x == null) create_bank_branch.bbr_code = "BBR0000";
                else create_bank_branch.bbr_code = Utilis.CreateCodeByCode("BBR", x.bbr_code, 7);
                // save new bank_branch
                _bankbranchservice.Create(create_bank_branch);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = create_bank_branch;
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
        [Route("api/bank_branch/update")]

        public async Task<IHttpActionResult> Update()
        {
            ResponseDataDTO<bank_branch> response = new ResponseDataDTO<bank_branch>();
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);
                int bbr_id = Convert.ToInt32(streamProvider.FormData["bbr_id"]);
                bank_branch exists = _bankbranchservice.Find(bbr_id);
                exists.bbr_name = Convert.ToString(streamProvider.FormData["bbr_name"]);
                exists.bbr_description = Convert.ToString(streamProvider.FormData["bbr_description"]);
                exists.province_id = Convert.ToInt32(streamProvider.FormData["province_id"]);
                exists.bbr_address = Convert.ToString(streamProvider.FormData["bbr_address"]);
                exists.bank_id = Convert.ToInt32(streamProvider.FormData["bank_id"]);


                // update bank_branch
                _bankbranchservice.Update(exists, exists.bbr_id);
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
        [Route("api/bank_branch/delete")]
        public IHttpActionResult Deletebank_branch(int ba_id)
        {
            ResponseDataDTO<bank_branch> response = new ResponseDataDTO<bank_branch>();
            try
            {
                var bank_branchDelete = _bankbranchservice.Find(ba_id);
                if (bank_branchDelete != null)
                {
                    _bankbranchservice.Delete(bank_branchDelete);

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