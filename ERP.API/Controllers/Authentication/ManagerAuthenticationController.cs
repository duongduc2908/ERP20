using AutoMapper;
using ERP.API.Controllers.Dashboard;
using ERP.Common.Constants;
using ERP.Data.Dto;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ERP.API.Controllers.Authentication
{
    [EnableCors("*", "*", "*")]
    public class ManagerAuthenticationController : BaseController
    {
        private readonly IStaffService _staffservice;
        private readonly IMapper _mapper;
        private string new_pass;

        public ManagerAuthenticationController() { }
        public ManagerAuthenticationController(IStaffService staffservice, IMapper mapper)
        {
            this._staffservice = staffservice;
            this._mapper = mapper;
        }
        [HttpPut]
        [Route("api/authentication/forgotpassword")]
        public async Task<IHttpActionResult> ForgotPassword(string email)
        {
            ResponseDataDTO<bool> response = new ResponseDataDTO<bool>();
            try
            {
                var staff = _staffservice.GetAllIncluing(x => x.sta_email == email).FirstOrDefault();
                if (staff == null)
                {
                    response.Code = HttpCode.NOT_FOUND;
                    response.Message = "Email khong co trong he thong";
                    response.Data = false;
                    return Ok(response);
                }
                new_pass = Utilis.MakeRandomPassword(8);
                staff.sta_password = HashMd5.convertMD5(new_pass);
                staff.sta_login = true;
                _staffservice.Update(staff, staff.sta_id);
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
        [HttpPost]
        [Route("api/authentication/sendmail_forgot")]
        public IHttpActionResult send_mail_created(string email)
        {
            ResponseDataDTO<string> response = new ResponseDataDTO<string>();
            try
            {
                BaseController.send_mail("Mat khau cua ban duoc update " + new_pass, email, "Update PassWord");
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = null;
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
    }
}
