using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Company;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ERP.API.Controllers.Dashboard
{
    [EnableCors("*", "*", "*")]
    [Authorize]
    public class ManagerCompanyController : BaseController
    {
        // GET: ManagerCompany
        private readonly ICompanyService _companyservice;
        private readonly ICompanyFunctionService _companyfunctionservice;

        private readonly IMapper _mapper;
        public ManagerCompanyController(ICompanyFunctionService companyfunctionservice, ICompanyService companyservice, IMapper mapper)
        {
            this._companyservice = companyservice;
            this._companyfunctionservice = companyfunctionservice;
            this._mapper = mapper;
        }

        #region methods

        #region["GetAll"]
        [HttpGet]
        [Route("api/company/get_all")]
        public IHttpActionResult GetAllDropDown()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _companyservice.GetAllDropDown();
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
        #endregion

        #region["Search"]
        [HttpGet]
        [Route("api/company/search")]
        public IHttpActionResult GetCompanys(int pageNumber, int pageSize, string search_name)
        {
            ResponseDataDTO<PagedResults<companyviewmodel>> response = new ResponseDataDTO<PagedResults<companyviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _companyservice.GetAllSearch(pageNumber,pageSize,search_name);
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
        #endregion

        #region["Get by id"]
        [Route("api/company/get_by_id")]
        public IHttpActionResult GetCompanys(int co_id)
        {
            ResponseDataDTO<companyviewmodel> response = new ResponseDataDTO<companyviewmodel>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _companyservice.GetById(co_id);
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
        #endregion

        #region["Create"]
        [HttpPost]
        [Route("api/company/create")]
        public async Task<IHttpActionResult> Create([FromBody] CompanyCreateViewModelJson create_company)
        {
            ResponseDataDTO<company> response = new ResponseDataDTO<company>();
            try
            {
                var company = create_company;
                #region["Check null"]

                if (company.co_name == null || company.co_name.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên không được để trống";
                    response.Error = "co_name";
                    return Ok(response);
                }
                if (company.co_vision == null || company.co_vision.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tầm nhìn không được để trống";
                    response.Error = "co_vision";
                    return Ok(response);
                }
                if (company.co_mission == null || company.co_mission.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Nhiệm vụ không được để trống";
                    response.Error = "co_mission";
                    return Ok(response);
                }
                if (company.co_address == null || company.co_address.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Địa chỉ không được để trống";
                    response.Error = "co_address";
                    return Ok(response);
                }
                if (company.co_target == null || company.co_target.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Địa chỉ không được để trống";
                    response.Error = "co_target";
                    return Ok(response);
                }
                #endregion

                #region["Kiểm tra rằng buộc "]
                var temp = _companyservice.GetAllIncluing(t => t.co_name.ToLower().Equals(company.co_name.Trim().ToLower()));
                if (temp.Count() != 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên công ty đã tồn tại ";
                    response.Error = "co_name";
                    return Ok(response);
                }
                #endregion

                //end kiểm tra
                #region["Save Database"]
                company company_create = new company();
                //Thong tin chung 
                company_create.co_name = company.co_name.Trim();
                company_create.co_vision = company.co_vision.Trim();
                company_create.co_mission = company.co_mission.Trim();
                company_create.co_address = company.co_address.Trim();
                company_create.co_target = company.co_target.Trim();
                company_create.co_price = company.co_price;
                company_create.co_duration = company.co_duration;
                company_create.co_discount= company.co_discount;
                company_create.co_no_of_employees = BaseController.get_id_current();
                if (company.co_description != null) company_create.co_description = company.co_description.Trim();
                if (company.co_revenue != null) company_create.co_revenue = company.co_revenue;

                var x = _companyservice.GetLast();
                if (x == null) company_create.co_code = "CT0000";
                else company_create.co_code = Utilis.CreateCodeByCode("CT", x.co_code, 6);
                _companyservice.Create(company_create);
                #endregion

                #region["Save function to database"]
                int company_last_id = _companyservice.GetLast().co_id;
                foreach (functionjson fun in company.list_function)
                {
                    if (Utilis.IsNumber(fun.fun_id))
                    {
                        company_funtion cof_create = new company_funtion();
                        cof_create.company_id = company_last_id;
                        cof_create.fun_id = Convert.ToInt32(fun.fun_id);
                        _companyfunctionservice.Create(cof_create);
                    }
                }
                #endregion
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = company_create;
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

        #region["Update"]
        [HttpPut]
        [Route("api/company/update")]
        public async Task<IHttpActionResult> Update([FromBody] CompanyUpdateViewModelJson update_company)
        {
            ResponseDataDTO<company> response = new ResponseDataDTO<company>();
            try
            {
                var company = update_company;

                #region["Check null"]

                if (company.co_name == null || company.co_name.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên không được để trống";
                    response.Error = "co_name";
                    return Ok(response);
                }
                if (company.co_vision == null || company.co_vision.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tầm nhìn không được để trống";
                    response.Error = "co_vision";
                    return Ok(response);
                }
                if (company.co_mission == null || company.co_mission.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Nhiệm vụ không được để trống";
                    response.Error = "co_mission";
                    return Ok(response);
                }
                if (company.co_address == null || company.co_address.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Địa chỉ không được để trống";
                    response.Error = "co_address";
                    return Ok(response);
                }
                if (company.co_target == null || company.co_target.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Địa chỉ không được để trống";
                    response.Error = "co_target";
                    return Ok(response);
                }
                #endregion

                #region["Kiểm tra rằng buộc "]
                if (check_name_update(company.co_name.Trim(), company.co_id))
                {
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = "Đã có tên công ty '" + company.co_name + " ' trong hệ thống.";
                    response.Error = "co_name";
                    return Ok(response);
                }
                
                #endregion

                //end kiểm tra
                #region["Save Database"]
                company company_exists = _companyservice.Find(company.co_id);
                //Thong tin chung 
                company_exists.co_name = company.co_name.Trim();
                company_exists.co_vision = company.co_vision.Trim();
                company_exists.co_mission = company.co_mission.Trim();
                company_exists.co_address = company.co_address.Trim();
                company_exists.co_target = company.co_target.Trim();
                company_exists.co_price = company.co_price;
                company_exists.co_duration = company.co_duration;
                company_exists.co_discount = company.co_discount;
                company_exists.co_no_of_employees = BaseController.get_id_current();
                if (company.co_description != null) company_exists.co_description = company.co_description.Trim();
                if (company.co_revenue != null) company_exists.co_revenue = company.co_revenue;
                _companyservice.Update(company_exists, company_exists.co_id);
                #endregion

                #region["Update function to database"]
                //update function 
                //Xóa bản ghi cũ update cái mới 
                List<company_funtion> lts_company_function_db = _companyfunctionservice.GetAllIncluing(x => x.company_id == company.co_id).ToList();
                List<functionjson> lts_function_v = new List<functionjson>(company.list_function);
                foreach (functionjson tr_f in lts_function_v)
                {
                    if (Utilis.IsNumber(tr_f.fun_id))
                    {
                        int check = 0;
                        int _id = Convert.ToInt32(tr_f.fun_id);
                        foreach (company_funtion tr in lts_company_function_db)
                        {
                            if (tr.fun_id == _id)
                            {
                                //update
                                lts_company_function_db.Remove(tr);
                                check = 1;
                                break;
                            }
                        }
                        if (check == 0)
                        {
                            //Khi view trả về những cái chọn mà k có trong db thì thêm phần này 
                            company_funtion create_trs = new company_funtion();
                            create_trs.fun_id = _id;
                            create_trs.company_id = company.co_id;

                            _companyfunctionservice.Create(create_trs);
                        }

                    }
                    
                }
                foreach (company_funtion trs in lts_company_function_db)
                {
                    _companyfunctionservice.Delete(trs);
                }
                #endregion
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = company_exists;
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

        #region["Delete"]
        [HttpDelete]
        [Route("api/company/delete")]
        public IHttpActionResult Delete(int co_id)
        {
            ResponseDataDTO<company> response = new ResponseDataDTO<company>();
            try
            {
                var staffDeleted = _companyservice.Find(co_id);
                if (staffDeleted != null)
                {
                    _companyservice.Delete(staffDeleted);

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
                _companyservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        

        #endregion
        private bool check_name_update(string _username, int sta_id)
        {
            List<company> lts_st = _companyservice.GetAllIncluing().ToList();
            company update = _companyservice.Find(sta_id);
            lts_st.Remove(update);
            bool res = lts_st.Exists(x => x.co_name == _username);
            return res;
        }
    }
}