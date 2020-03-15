using AutoMapper;
using ERP.Common.Constants.Enums;
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
    public class SmsStrategyRepository : GenericRepository<sms_strategy>, ISmsStrategyRepository
    {
        private readonly IMapper _mapper;
        public SmsStrategyRepository(ERPDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._mapper = mapper;
        }
        //public PagedResults<smsstrategyviewmodel> GetAllPageSearch(int pageNumber, int pageSize, string search_name)
        //{
        //    List<smsstrategyviewmodel> res = new List<smsstrategyviewmodel>();

        //    var skipAmount = pageSize * pageNumber;
        //    List<sms_strategy> list = new List<sms_strategy>();

        //    if (search_name == null)
        //    {
        //        list = _dbContext.sms_strategy.OrderBy(t => t.smss_id).Skip(skipAmount).Take(pageSize).ToList();
        //    }
        //    else
        //    {
        //        search_name = search_name.Trim();
        //        list = _dbContext.sms_strategy.Where(x => x.smss_title.Contains(search_name)).OrderBy(t => t.smss_id).Skip(skipAmount).Take(pageSize).ToList();
        //    }

        //    var totalNumberOfRecords = _dbContext.sms_strategy.Count();
        //    foreach (sms_strategy i in list)
        //    {
        //        var smsstrategyview = _mapper.Map<smsstrategyviewmodel>(i);
        //        var customer_group = _dbContext.customer_group.FirstOrDefault(x => x.cg_id == i.customer_group_id);
        //        if (customer_group != null) smsstrategyview.customer_group_name = customer_group.cg_name;
        //        var staff = _dbContext.staffs.FirstOrDefault(x => x.sta_id == i.staff_id);
        //        if (staff != null) smsstrategyview.staff_name = staff.sta_fullname;
        //        for(int j =1; j < EnumSms.smss_status.Length+1; j++)
        //        {
        //            if (smsstrategyview.smss_status == j) smsstrategyview.smss_status_name = EnumSms.smss_status[j - 1];
        //        }
        //        res.Add(smsstrategyview);
        //    }

        //    var mod = totalNumberOfRecords % pageSize;

        //    var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

        //    return new PagedResults<smsstrategyviewmodel>
        //    {
        //        Results = res,
        //        PageNumber = pageNumber,
        //        PageSize = pageSize,
        //        TotalNumberOfPages = totalPageCount,
        //        TotalNumberOfRecords = totalNumberOfRecords
        //    };
        //}

        public PagedResults<smsstrategyviewmodel> GetAllPageSearch(int pageNumber, int pageSize, string search_name)
        {
            List<smsstrategyviewmodel> res = new List<smsstrategyviewmodel>();

            var skipAmount = pageSize * pageNumber;
            List<sms_strategy> list = new List<sms_strategy>();

            if (search_name == null)
            {
                list = _dbContext.sms_strategy.OrderBy(t => t.smss_id).Skip(skipAmount).Take(pageSize).ToList();
            }
            else
            {
                search_name = search_name.Trim();
                list = _dbContext.sms_strategy.Where(x => x.smss_title.Contains(search_name)).OrderBy(t => t.smss_id).Skip(skipAmount).Take(pageSize).ToList();
            }

            var totalNumberOfRecords = _dbContext.sms_strategy.Count();
            foreach (sms_strategy i in list)
            {
                var smsstrategyview = _mapper.Map<smsstrategyviewmodel>(i);
                var customer_group = _dbContext.customer_group.FirstOrDefault(x => x.cg_id == i.customer_group_id);
                if (customer_group != null) smsstrategyview.customer_group_name = customer_group.cg_name;
                var staff = _dbContext.staffs.FirstOrDefault(x => x.sta_id == i.staff_id);
                if (staff != null) smsstrategyview.staff_name = staff.sta_fullname;
                for (int j = 1; j < EnumSms.smss_status.Length + 1; j++)
                {
                    if (smsstrategyview.smss_status == j) smsstrategyview.smss_status_name = EnumSms.smss_status[j - 1];
                }
                res.Add(smsstrategyview);
            }

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<smsstrategyviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
    }
}