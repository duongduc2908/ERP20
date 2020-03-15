using AutoMapper;
using ERP.API.Models;
using ERP.Common.Constants;
using ERP.Common.Models;
using ERP.Data.Dto;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView.Sms;
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
    public class ManagerSmsStrategyController : ApiController
    {
        private readonly ISmsStrategyService _smsstrategyservice;

        private readonly IMapper _mapper;

        public ManagerSmsStrategyController() { }
        public ManagerSmsStrategyController(ISmsStrategyService sms_strategyservice, IMapper mapper)
        {
            this._smsstrategyservice = sms_strategyservice;
            this._mapper = mapper;
        }

        #region methods
        [HttpGet]
        [Route("api/sms-strategys/all")]
        public IHttpActionResult Getsms_strategys()
        {
            ResponseDataDTO<IEnumerable<sms_strategy>> response = new ResponseDataDTO<IEnumerable<sms_strategy>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _smsstrategyservice.GetAll();
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

        [Route("api/sms-strategys/page-search")]
        public IHttpActionResult GetSmsStrategysPaging(int pageSize, int pageNumber, string search_name)
        {
            ResponseDataDTO<PagedResults<smsstrategyviewmodel>> response = new ResponseDataDTO<PagedResults<smsstrategyviewmodel>>();
            try
            {
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = _smsstrategyservice.GetAllPageSearch(pageNumber, pageSize, search_name);
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

        [HttpPost]
        [Route("api/sms-strategys/create")]

        public async Task<IHttpActionResult> Createsms_strategy([FromBody] SmsStrategyViewModelCreate smsstrategy)
        {
            ResponseDataDTO<List<sms_strategy>> response = new ResponseDataDTO<List<sms_strategy>>();
            try
            {
                List<sms_strategy> data = new List<sms_strategy>();
                var smsstr = smsstrategy;
                for(int i = 0;  i< smsstr.customer_group_id.Length ; i++)
                {
                    SmsStrategyCreateViewModel sms_strategyCreateViewModel = new SmsStrategyCreateViewModel();
                    sms_strategyCreateViewModel.customer_group_id = smsstr.customer_group_id[i];
                    sms_strategyCreateViewModel.sms_template_id = smsstr.sms_template_id;
                    sms_strategyCreateViewModel.smss_title = smsstr.smss_title;

                    // mapping view model to entity
                    var createdsms_strategy = _mapper.Map<sms_strategy>(sms_strategyCreateViewModel);
                    var x = _smsstrategyservice.GetLast();
                    if (x == null) createdsms_strategy.smss_code = Utilis.CreateCode("SMSS", 0, 7);
                    else createdsms_strategy.smss_code = Utilis.CreateCode("SMSS", x.smss_id, 7);
                    createdsms_strategy.smss_created_date = DateTime.Now;
                    createdsms_strategy.staff_id = BaseController.get_id_current();
                    createdsms_strategy.smss_status = 1;


                    // save new sms_strategy
                    _smsstrategyservice.Create(createdsms_strategy);
                    data.Add(createdsms_strategy);
                }
                // get data from formdata
                
                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = data;
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
        [Route("api/sms-strategys/update")]

        public async Task<IHttpActionResult> Updatesms_strategy([FromBody] SmsStrategyViewModelUpdate smsstrategy)
        {
            ResponseDataDTO<List<sms_strategy>> response = new ResponseDataDTO<List<sms_strategy>>();
            try
            {
                List<sms_strategy> data = new List<sms_strategy>();
                var smsstr = smsstrategy;
                //Delete sms_staragety old 
                var sms_strategy_old = _smsstrategyservice.Find(smsstr.smss_id);
                _smsstrategyservice.Delete(sms_strategy_old);
                for (int i = 0; i < smsstr.customer_group_id.Length; i++)
                {
                    SmsStrategyCreateViewModel sms_strategyCreateViewModel = new SmsStrategyCreateViewModel();
                    sms_strategyCreateViewModel.customer_group_id = smsstr.customer_group_id[i];
                    sms_strategyCreateViewModel.sms_template_id = smsstr.sms_template_id;
                    sms_strategyCreateViewModel.smss_title = smsstr.smss_title;

                    // mapping view model to entity
                    var createdsms_strategy = _mapper.Map<sms_strategy>(sms_strategyCreateViewModel);
                    var x = _smsstrategyservice.GetLast();
                    if (x == null) createdsms_strategy.smss_code = Utilis.CreateCode("SMSS", 0, 7);
                    else createdsms_strategy.smss_code = Utilis.CreateCode("SMSS", x.smss_id, 7);
                    createdsms_strategy.smss_created_date = DateTime.Now;
                    createdsms_strategy.staff_id = BaseController.get_id_current();
                    createdsms_strategy.smss_status = 1;


                    // save new sms_strategy
                    _smsstrategyservice.Create(createdsms_strategy);
                    data.Add(createdsms_strategy);
                }
                // get data from formdata

                // return response
                response.Code = HttpCode.OK;
                response.Message = MessageResponse.SUCCESS;
                response.Data = data;
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
        [Route("api/sms-strategys/delete")]
        public IHttpActionResult Deletesms_strategy(int sms_strategyId)
        {
            ResponseDataDTO<sms_strategy> response = new ResponseDataDTO<sms_strategy>();
            try
            {
                var sms_strategyDeleted = _smsstrategyservice.Find(sms_strategyId);
                if (sms_strategyDeleted != null)
                {
                    _smsstrategyservice.Delete(sms_strategyDeleted);

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
                _smsstrategyservice.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}