using AutoMapper;
using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView.Sms;
using ERP.Repository.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Repository.Repositories
{
    public class SmsTemplateRepository : GenericRepository<sms_template>, ISmsTemplateRepository
    {
        private readonly IMapper _mapper;
        public SmsTemplateRepository(ERPDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }
        public PagedResults<smstemplatemodelview> CreatePagedResults(int pageNumber, int pageSize, string search_name)
        {
            List<smstemplatemodelview> results = new List<smstemplatemodelview>();
            var skipAmount = pageSize * pageNumber;
            List<sms_template> list = new List<sms_template>();
            if (search_name == null)
            {
                list = _dbContext.sms_template.OrderBy(t => t.smt_id).Skip(skipAmount).Take(pageSize).ToList();
            }
            else { 
                search_name = search_name.Trim(); 
                list = _dbContext.sms_template.Where(x => x.smt_title.Contains(search_name) || x.smt_content.Contains(search_name)).OrderBy(t => t.smt_id).Skip(skipAmount).Take(pageSize).ToList(); 
            }
            var totalNumberOfRecords = _dbContext.sms_template.Count();
            var list_smstem = list.ToList();
            foreach(sms_template i in list_smstem)
            {
                var smstem = _mapper.Map<smstemplatemodelview>(i);
                var staff = _dbContext.staffs.Where(s => s.sta_id == i.staff_id).FirstOrDefault();
                if (staff != null) smstem.staff_fullname = staff.sta_fullname;
                var modifier = _dbContext.staffs.Where(s => s.sta_id == i.smt_modifier).FirstOrDefault();
                if (modifier != null) smstem.smt_modify_name = modifier.sta_fullname;
                results.Add(smstem);
            }

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<smstemplatemodelview>
            {
                Results = results,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }

        public PagedResults<smstemplatestrategyviewmodel> CreatePagedSmsTrategy(int pageNumber, int pageSize)
        {
            List<smstemplatestrategyviewmodel> results = new List<smstemplatestrategyviewmodel>();
            var skipAmount = pageSize * pageNumber;
            List<sms_template> list = new List<sms_template>();
            list = _dbContext.sms_template.OrderBy(x => x.smt_id).Skip(skipAmount).Take(pageSize).ToList();            
            var totalNumberOfRecords = _dbContext.sms_template.Count();
            foreach (sms_template i in list)
            {
                var smstem = _mapper.Map<smstemplatestrategyviewmodel>(i);
                var staff = _dbContext.staffs.Where(s => s.sta_id == i.staff_id).FirstOrDefault();
                if(staff != null) smstem.staff_name = staff.sta_fullname;
                var modifier = _dbContext.staffs.Where(s => s.sta_id == i.smt_modifier).FirstOrDefault();
                if(modifier != null ) smstem.smt_modify_name = modifier.sta_fullname;
                
                results.Add(smstem);
            }

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<smstemplatestrategyviewmodel>
            {
                Results = results,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        } 
        public PagedResults<field> Get_All_Fields(int pageNumber, int pageSize)
        {
            List<field> results = new List<field>();
            var skipAmount = pageSize * pageNumber;
            results = _dbContext.fields.OrderBy(x => x.fie_id).Skip(skipAmount).Take(pageSize).ToList();            
            var totalNumberOfRecords = _dbContext.fields.Count();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<field>
            {
                Results = results,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
    }
}