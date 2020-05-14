using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Company;
using ERP.Data.ModelsERP.ModelView.Package;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
namespace ERP.API.Controllers.Dashboard
{
    public class ManagerPackageController : BaseController
    {
        // GET: ManagerPackage
        private readonly IPackageService _packageservice;
        private readonly IFunctionService _functionservice;

        private readonly IMapper _mapper;
        public ManagerPackageController(IPackageService packageservice, IFunctionService functionservice, IMapper mapper)
        {
            this._packageservice = packageservice;
            this._functionservice = functionservice;
            this._mapper = mapper;
        }

        #region methods
        #region["GetAll"]
        [HttpGet]
        [Route("api/package/get_all")]
        public IHttpActionResult GetAllDropDown()
        {
            ResponseDataDTO<List<packageviewmodel>> response = new ResponseDataDTO<List<packageviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _packageservice.GetAllDropDown();
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
        [Route("api/package/search")]
        public IHttpActionResult GetPackages(int pageNumber, int pageSize, string search_name)
        {
            ResponseDataDTO<PagedResults<packageviewmodel>> response = new ResponseDataDTO<PagedResults<packageviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _packageservice.GetAllSearch(pageNumber, pageSize, search_name);
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
        [Route("api/package/get_by_id")]
        public IHttpActionResult GetPackages(int pac_id)
        {
            ResponseDataDTO<packageviewmodel> response = new ResponseDataDTO<packageviewmodel>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _packageservice.GetById(pac_id);
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
        [Route("api/package/create")]
        public async Task<IHttpActionResult> Create([FromBody] PackageCreateViewModelJson create_package)
        {
            ResponseDataDTO<package> response = new ResponseDataDTO<package>();
            try
            {
                var package = create_package;
                #region["Check null"]

                if (package.pac_name == null || package.pac_name.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên không được để trống";
                    response.Error = "pac_name";
                    return Ok(response);
                }
                if (package.pac_status == null )
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Trạng thái không được để trống";
                    response.Error = "pac_status";
                    return Ok(response);
                }
                if (package.pac_price == null )
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Giá không được để trống";
                    response.Error = "pac_price";
                    return Ok(response);
                }
               
                #endregion

                #region["Kiểm tra rằng buộc "]
                var temp = _packageservice.GetAllIncluing(t => t.pac_name.ToLower().Equals(package.pac_name.Trim().ToLower()));
                if (temp.Count() != 0)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên gói đã tồn tại ";
                    response.Error = "pac_name";
                    return Ok(response);
                }
                foreach(functionjson fun in package.list_function)
                {
                    var check_code_function = _functionservice.GetAllIncluing(t => t.fun_code.ToLower().Equals(fun.fun_code.Trim().ToLower()));
                    if (check_code_function.Count() != 0)
                    {
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = "Tên fuction đã tồn tại ";
                        response.Error = "fun_code";
                        return Ok(response);
                    }
                }
                #endregion

                //end kiểm tra
                #region["Save Database"]
                package package_create = new package();
                //Thong tin chung 
                package_create.pac_name = package.pac_name.Trim();
                package_create.pac_status = package.pac_status;
                
                package_create.pac_price = package.pac_price;
                

                var x = _packageservice.GetLast();
                if (x == null) package_create.pac_code = "G00000";
                else package_create.pac_code = Utilis.CreateCodeByCode("G", x.pac_code, 6);
                _packageservice.Create(package_create);
                #endregion

                #region["Save function to database"]
                int package_last_id = _packageservice.GetLast().pac_id;
                foreach (functionjson fun in package.list_function)
                {
                   
                    function fun_create = new function();
                    fun_create.fun_name = fun.fun_name;
                    fun_create.fun_code = fun.fun_code;
                    fun_create.package_id = package_last_id;
                        
                    _functionservice.Create(fun_create);
                    
                }
                #endregion
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = package_create;
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
        [Route("api/package/update")]
        public async Task<IHttpActionResult> Update([FromBody] PackageUpdateViewModelJson update_package)
        {
            ResponseDataDTO<package> response = new ResponseDataDTO<package>();
            try
            {
                var package = update_package;
                #region["Check null"]

                if (package.pac_name == null || package.pac_name.Trim() == "")
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên không được để trống";
                    response.Error = "pac_name";
                    return Ok(response);
                }
                if (package.pac_status == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Trạng thái không được để trống";
                    response.Error = "pac_status";
                    return Ok(response);
                }
                if (package.pac_price == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Giá không được để trống";
                    response.Error = "pac_price";
                    return Ok(response);
                }

                #endregion
                
                #region["Kiểm tra rằng buộc "]
                if (check_name_package_update(package.pac_name.Trim(), package.pac_id))
                {
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = "Đã có tên công ty '" + package.pac_name + " ' trong hệ thống.";
                    response.Error = "pac_name";
                    return Ok(response);
                }
                foreach(functionjson fun in package.list_function)
                {
                    if (Utilis.IsNumber(fun.fun_id))
                    {
                        if (check_name_function_update(fun.fun_name.Trim(), Convert.ToInt32(fun.fun_id)))
                        {
                            response.Code = HttpCode.NOT_FOUND;
                            response.Message = "Đã có tên '" + fun.fun_name + " ' trong hệ thống.";
                            response.Error = "fun_code";
                            return Ok(response);
                        }
                    }
                    else
                    {
                        var check_code_function = _functionservice.GetAllIncluing(t => t.fun_code.ToLower().Equals(fun.fun_code.Trim().ToLower()));
                        if (check_code_function.Count() != 0)
                        {
                            response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                            response.Message = "Tên fuction đã tồn tại ";
                            response.Error = "fun_code";
                            return Ok(response);
                        }
                    }

                        
                }
                #endregion

                //end kiểm tra
                #region["Save Database"]
                package package_exists = _packageservice.Find(package.pac_id);
                //Thong tin chung 
                package_exists.pac_name = package.pac_name.Trim();
                package_exists.pac_status = package.pac_status;

                package_exists.pac_price = package.pac_price;

               
                _packageservice.Update(package_exists, package_exists.pac_id);
                #endregion
                #region["Update function to database"]
                //update function 
                //Xóa bản ghi cũ update cái mới 
                List<function> lts_function_db = _functionservice.GetAllIncluing(x => x.package_id == package.pac_id).ToList();
                List<functionjson> lts_function_v = new List<functionjson>(package.list_function);
                foreach (functionjson tr_f in lts_function_v)
                {
                    if (Utilis.IsNumber(tr_f.fun_id))
                    {
                        int check = 0;
                        int _id = Convert.ToInt32(tr_f.fun_id);
                        foreach (function tr in lts_function_db)
                        {
                            if (tr.fun_id == _id)
                            {
                                //update
                                lts_function_db.Remove(tr);
                                check = 1;
                                break;
                            }
                        }
                        if (check == 0)
                        {
                            //Khi view trả về những cái chọn mà k có trong db thì thêm phần này 
                            function create_fun = new function();
                            create_fun.fun_name = tr_f.fun_name;
                            create_fun.fun_code = tr_f.fun_code;
                            create_fun.package_id = package.pac_id;

                           
                            _functionservice.Create(create_fun);
                        }

                    }

                }
                foreach (function trs in lts_function_db)
                {
                    _functionservice.Delete(trs);
                }
                #endregion
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = package_exists;
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
        [Route("api/package/delete")]
        public IHttpActionResult Delete(int pac_id)
        {
            ResponseDataDTO<package> response = new ResponseDataDTO<package>();
            try
            {
                var staffDeleted = _packageservice.Find(pac_id);
                if (staffDeleted != null)
                {
                    _packageservice.Delete(staffDeleted);

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
                _packageservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion



        #endregion
        private bool check_name_function_update(string _username, int sta_id)
        {
            List<function> lts_st = _functionservice.GetAllIncluing().ToList();
            function update = _functionservice.Find(sta_id);
            lts_st.Remove(update);
            bool res = lts_st.Exists(x => x.fun_name == _username);
            return res;
        }
        private bool check_name_package_update(string _username, int sta_id)
        {
            List<package> lts_st = _packageservice.GetAllIncluing().ToList();
            package update = _packageservice.Find(sta_id);
            lts_st.Remove(update);
            bool res = lts_st.Exists(x => x.pac_name == _username);
            return res;
        }
    }
}