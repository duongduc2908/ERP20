using AutoMapper;
using ERP.API.Models;
using ERP.API.Models.Excel;
using ERP.Common.Constants;
using ERP.Common.Excel;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Extension.Extensions;
using ERP.Service.Services;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ERP.API.Controllers.Dashboard
{
    [EnableCors("*", "*", "*")]
    [Authorize]
    public class ManagerDepartmentController  : BaseController
    {
        private readonly IDepartmentService _departmentservice;

        private readonly IMapper _mapper;

        public ManagerDepartmentController() { }
        public ManagerDepartmentController(IDepartmentService departmentservice, IMapper mapper)
        {
            this._departmentservice = departmentservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/department/all")]
        public IHttpActionResult Getdepartments()
        {
            ResponseDataDTO<IEnumerable<department>> response = new ResponseDataDTO<IEnumerable<department>>();
            try
            {
                int company_id = BaseController.get_company_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _departmentservice.GetAll();
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;;
                response.Data = null;

                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }

        [Route("api/departments/page")]
        public IHttpActionResult GetdepartmentsPaging(int pageSize, int pageNumber)
        {
            ResponseDataDTO<PagedResults<department>> response = new ResponseDataDTO<PagedResults<department>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _departmentservice.CreatePagedResults(pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;;
                response.Data = null;

                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }
        [HttpGet]
        [Route("api/department/getall")]
        public IHttpActionResult GetLevelOne()
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                int company_id = BaseController.get_company_id_current();
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _departmentservice.Get_Level_One(company_id);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;;
                response.Data = null;

                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }
        [HttpGet]
        [Route("api/department/get_children")]
        public IHttpActionResult GetChilden(int id)
        {
            ResponseDataDTO<List<dropdown>> response = new ResponseDataDTO<List<dropdown>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _departmentservice.Get_Children(id);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message; ;
                response.Data = null;

                Console.WriteLine(ex.ToString());
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("api/departments/create")]

        public async Task<IHttpActionResult> Createdepartment()
        {
            ResponseDataDTO<department> response = new ResponseDataDTO<department>();
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);
                var x = streamProvider.FormData["de_thumbnail"];
                // save file
                string fileName = "";
                
                foreach (MultipartFileData fileData in streamProvider.FileData)
                {
                    fileName = (FileExtension.SaveFileOnDisk(fileData));
                }
                if (streamProvider.FormData["de_name"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên phòng ban không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["de_manager"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Trưởng phòng không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                // get data from formdata
                DepartmentCreateViewModel departmentCreateViewModel = new DepartmentCreateViewModel
                {
                    de_name = Convert.ToString(streamProvider.FormData["de_name"]),
                    de_description = Convert.ToString(streamProvider.FormData["de_description"]),
                    de_manager = Convert.ToString(streamProvider.FormData["de_manager"]),

                    company_id = Convert.ToByte(streamProvider.FormData["company_id"]),

                };
                

                if (streamProvider.FormData["company_id"] == null)
                {
                    departmentCreateViewModel.company_id = null;
                }

                // mapping view model to entity
                var createddepartment = _mapper.Map<department>(departmentCreateViewModel);
                createddepartment.de_thumbnail = fileName;

                // save new department
                _departmentservice.Create(createddepartment);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = createddepartment;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;;
                response.Data = null;
                return Ok(response);
            }

        }

        [HttpPut]
        [Route("api/departments/update")]

        public async Task<IHttpActionResult> Updatedepartment(int? de_id)
        {
            ResponseDataDTO<department> response = new ResponseDataDTO<department>();
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);

                // save file
                string fileName = "";
                if (streamProvider.FileData.Count > 0)
                {
                    foreach (MultipartFileData fileData in streamProvider.FileData)
                    {
                        fileName = FileExtension.SaveFileOnDisk(fileData);
                    }
                }
                if (streamProvider.FormData["de_name"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Tên phòng ban không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                if (streamProvider.FormData["de_manager"] == null)
                {
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = "Trưởng phòng không được để trống";
                    response.Data = null;
                    return Ok(response);
                }
                // get data from formdata
                DepartmentUpdateViewModel departmentUpdateViewModel = new DepartmentUpdateViewModel
                {
                    de_id = Convert.ToInt32(streamProvider.FormData["de_id"]),

                    de_name = Convert.ToString(streamProvider.FormData["de_name"]),
                    de_description = Convert.ToString(streamProvider.FormData["de_description"]),
                    de_manager = Convert.ToString(streamProvider.FormData["de_manager"]),

                    company_id = Convert.ToByte(streamProvider.FormData["company_id"]),

                };
                var existstaff = _departmentservice.Find(de_id);
                if (fileName != "")
                {
                    departmentUpdateViewModel.de_thumbnail = fileName;
                }
                else
                {

                    departmentUpdateViewModel.de_thumbnail = existstaff.de_thumbnail;
                }
                if (streamProvider.FormData["company_id"] == null)
                {
                    departmentUpdateViewModel.company_id = null;
                }

                // mapping view model to entity
                var updateddepartment = _mapper.Map<department>(departmentUpdateViewModel);



                // update department
                _departmentservice.Update(updateddepartment, de_id);
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = updateddepartment;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;;
                response.Data = null;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }
        }

        [HttpDelete]
        [Route("api/departments/delete")]
        public IHttpActionResult Deletedepartment(int departmentId)
        {
            ResponseDataDTO<department> response = new ResponseDataDTO<department>();
            try
            {
                var departmentDeleted = _departmentservice.Find(departmentId);
                if (departmentDeleted != null)
                {
                    _departmentservice.Delete(departmentDeleted);

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
                    response.Message = "Không tìm thấy mã phòng ban "+departmentId.ToString()+" trong hệ thống.";
                    response.Data = null;

                    return Ok(response);
                }


            }
            catch (Exception ex)
            {
                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                response.Message = ex.Message;;
                response.Data = null;
                Console.WriteLine(ex.ToString());

                return Ok(response);
            }
        }
        #endregion

        #region["Import Excel"]
        [HttpPost]
        [Route("api/departments/import")]
        public async Task<IHttpActionResult> Import()
        {
            ResponseDataDTO<department> response = new ResponseDataDTO<department>();
            var exitsData = "";
            try
            {
                var path = Path.GetTempPath();

                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
                }

                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(path);

                await Request.Content.ReadAsMultipartAsync(streamProvider);

                // save file
                string fileName = "";
                if (streamProvider.FileData.Count > 0)
                {
                    foreach (MultipartFileData fileData in streamProvider.FileData)
                    {
                        fileName = FileExtension.SaveFileOnDisk(fileData);
                        //if (fileData.Headers.ContentDisposition.FileName.Equals("Test.xlsx") || fileData.Headers.ContentDisposition.FileName.Equals("Test.xls"))
                        //{
                        //    fileName = FileExtension.SaveFileOnDisk(fileData);
                        //}
                        //else
                        //{
                        //    throw new Exception("File excel import không hợp lệ!");
                        //}
                        
                    }
                }
                
                var list = new List<department>();
                fileName = @"D:\ERP20\ERP.API\" + fileName;
                var dataset = ExcelImport.ImportExcelXLS(fileName, true);
                DataTable table = (DataTable)dataset.Tables[0];
                if (table != null && table.Rows.Count > 0)
                {
                    if (table.Columns.Count != 2)
                    {
                        exitsData = "File excel import không hợp lệ!";
                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                        response.Message = exitsData;
                        response.Data = null;
                        return Ok(response);
                    }
                    else
                    {
                        #region["Check null"]
                        foreach (DataRow dr in table.Rows)
                        {
                            if (dr["email"] is null)
                            {
                                exitsData = "Email phòng ban không được trống!";
                                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                                response.Message = exitsData;
                                response.Data = null;
                                return Ok(response);
                            }
                            if (dr["id"] is null)
                            {
                                exitsData = "ma phòng ban không được trống!";
                                response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                                response.Message = exitsData;
                                response.Data = null;
                                return Ok(response);
                            }
                        }
                        #endregion

                        #region["Check duplicate"]
                        for (var i = 0; i < table.Rows.Count; i++)
                        {
                            var DepartmentCodeCur = table.Rows[i]["id"].ToString().Trim();
                            for (var j = 0; j < table.Rows.Count; j++)
                            {
                                if (i != j)
                                {
                                    var _idDepartmentCur = table.Rows[j]["id"].ToString().Trim();
                                    if (DepartmentCodeCur.Equals(_idDepartmentCur))
                                    {
                                        exitsData = "Mã bộ phận phòng ban'" + DepartmentCodeCur + "' bị lặp trong file excel!";
                                        response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                                        response.Message = exitsData;
                                        response.Data = null;
                                        return Ok(response);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    list = DataTableCmUtils.ToListof<department>(table); ;
                    // Gọi hàm save data
                    if (list != null && list.Count > 0)
                    {
                        
                    }
                    exitsData = "Đã nhập dữ liệu excel thành công!";
                    response.Code = HttpCode.OK;
                    response.Message = exitsData;
                    response.Data = null;
                    return Ok(response);
                }
                else
                {
                    exitsData = "File excel import không có dữ liệu!";
                    response.Code = HttpCode.INTERNAL_SERVER_ERROR;
                    response.Message = exitsData;
                    response.Data = null;
                    return Ok(response);
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

        #region["Export Excel"]
        [HttpGet]
        [Route("api/departments/export")]
        public async Task<IHttpActionResult> Export()
        {
            ResponseDataDTO<department> response = new ResponseDataDTO<department>();
            try
            {
                var listDepartment = new List<department>();

                var objRQ_Department = new RQ_Department()
                {
                    // WARQBase
                    Tid = GetNextTId(),
                    GwUserCode = GwUserCode,
                    GwPassword = GwPassword,
                    Ft_RecordStart = Ft_RecordStart,
                    Ft_RecordCount = Ft_RecordCount,
                    Ft_WhereClause = "",
                    Ft_Cols_Upd = null,
                    FuncType = null,
                    // RQ_Mst_Department
                    Rt_Cols_Mst_Department = "*",
                    mst_department = null
                };
                var objRT_Mst_Department = _departmentservice.CreatePagedResults(0,10);
                if (objRT_Mst_Department != null)
                {
                    listDepartment.AddRange(objRT_Mst_Department.Results);

                    Dictionary<string, string> dicColNames = GetImportDicColums();

                    string url = "";
                    string filePath = GenExcelExportFilePath(string.Format(typeof(department).Name), ref url);

                    ExcelExport.ExportToExcelFromList(listDepartment, dicColNames, filePath, string.Format("Department"));

                    response.Code = HttpCode.OK;
                    response.Message = "Đã xuất excel thành công!";
                    response.Data = null;
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

        [HttpPost]
        [Route("api/departments/export_template")]
        public async Task<IHttpActionResult> ExportTemplate()
        {
            ResponseDataDTO<department> response = new ResponseDataDTO<department>();
            try
            {
                var list = new List<department>();
                Dictionary<string, string> dicColNames = GetImportDicColumsTemplate();
                string url = "";
                string filePath = GenExcelExportFilePath(string.Format(typeof(department).Name), ref url);
                ExcelExport.ExportToExcelFromList(list, dicColNames, filePath, string.Format("Department"));

                response.Code = HttpCode.OK;
                response.Message = "Đã xuất excel thành công!";
                response.Data = null;

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
                 {"email","Email phong ban"},
                 {"id","Ma bộ phận phòng ban"}
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
                _departmentservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}