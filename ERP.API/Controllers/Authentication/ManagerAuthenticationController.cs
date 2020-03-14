using AutoMapper;
using ERP.API.Controllers.Dashboard;
using ERP.Common.Constants;
using ERP.Data.Dto;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.IO;
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
        private static string new_pass { get; set; }

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
                response.Message = "Yêu cầu thay đổi mật khẩu của bạn đã được thực hiện, voi lòng kiểm tra email.";
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
                string text1 = File.ReadAllText("D:/ERP20/ERP.Common/TemplateMail/ResetPassWord/Reset1.txt");
                string text2 = File.ReadAllText("D:/ERP20/ERP.Common/TemplateMail/ResetPassWord/Reset2.txt");
                string text3 = File.ReadAllText("D:/ERP20/ERP.Common/TemplateMail/ResetPassWord/Reset3.txt");
                var text_send = text1 + text2 + new_pass + text3;
                BaseController.send_mail(text_send, email, "Update PassWord");
                new_pass = "";
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
