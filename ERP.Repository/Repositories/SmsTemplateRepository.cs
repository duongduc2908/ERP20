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
        public PagedResults<smstemplatemodelview> CreatePagedResults(int pageNumber, int pageSize)
        {
            List<smstemplatemodelview> results = new List<smstemplatemodelview>();
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.sms_template.OrderBy(t => t.smt_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.sms_template.Count();

            var list_smstem = list.ToList();
            foreach(sms_template i in list_smstem)
            {
                var smstem = _mapper.Map<smstemplatemodelview>(i);
                smstem.list_field = new List<field>();
                smstem.staff_fullname = _dbContext.staffs.Where(s => s.sta_id == i.staff_id).FirstOrDefault().sta_fullname;
                var lst_fieldtem = _dbContext.field_template.Where(ft => ft.sms_template_id == i.smt_id);
                if(lst_fieldtem != null)
                {
                    foreach ( field_template f in lst_fieldtem.ToList())
                    {
                        field t = (_dbContext.fields.Where(m => m.fie_id == f.field_id).FirstOrDefault());
                        smstem.list_field.Add(t);
                    }
                }
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