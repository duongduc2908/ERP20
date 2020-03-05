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
                smstem.list_field = new List<field>();
                smstem.staff_fullname = _dbContext.staffs.Where(s => s.sta_id == i.staff_id).FirstOrDefault().sta_fullname;
                var lst_fieldtem = _dbContext.fields.Where(f => f.fie_type == 1).ToList();
                smstem.list_field = lst_fieldtem;
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
    }
}