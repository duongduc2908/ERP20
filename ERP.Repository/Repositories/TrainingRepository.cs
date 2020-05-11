using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Repository.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Repository.Repositories
{
    public class TrainingRepository : GenericRepository<training>, ITrainingRepository
    {
        public TrainingRepository(ERPDbContext dbContext) : base(dbContext)
        {
        }
        
        public PagedResults<training> CreatePagedResults(int pageNumber, int pageSize)
        {
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.trainings.OrderBy(t => t.tn_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.companies.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<training>
            {
                Results = results,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public PagedResults<training> GetAllSearch(int pageNumber, int pageSize, string search_name)
        {
            if (search_name != null)
            {
                search_name = search_name.Trim().ToLower();
            }
            List<training> list_res;
            List<training> list;
            var skipAmount = pageSize * pageNumber;
            if (search_name == null)
            {
                list_res = _dbContext.trainings.ToList();
            }
            else list_res = _dbContext.trainings.Where(x => x.tn_name.ToLower().Contains(search_name)|| x.tn_code.ToLower().Contains(search_name)).ToList();
            
            var total = list_res.Count();
            list = list_res.OrderByDescending(t => t.tn_id).Skip(skipAmount).Take(pageSize).ToList();

            var totalNumberOfRecords = list_res.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<training>
            {
                Results = list,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public List<dropdown> GetAllName()
        {
            List<dropdown> res = new List<dropdown>();
            List<training> lts_tr = _dbContext.trainings.ToList();
            foreach(training tr in lts_tr)
            {
                dropdown dr = new dropdown();
                dr.id = tr.tn_id;
                dr.name = tr.tn_name;
                res.Add(dr);
            }
            return res;
        }
        public training GetById(int tn_id)
        {
            training res = _dbContext.trainings.Find(tn_id);
            
            return res;
        }
    }
}