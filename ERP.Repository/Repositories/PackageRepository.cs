using AutoMapper;
using ERP.Common.Constants.Enums;
using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Company;
using ERP.Data.ModelsERP.ModelView.Package;
using ERP.Repository.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Repository.Repositories
{
    public class PackageRepository : GenericRepository<package>, IPackageRepository
    {
        private readonly IMapper _mapper;
        public PackageRepository(ERPDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._mapper = mapper;
        }
        public PagedResults<package> CreatePagedResults(int pageNumber, int pageSize)
        {
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.packages.OrderBy(t => t.pac_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.packages.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<package>
            {
                Results = results,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public PagedResults<packageviewmodel> GetAllSearch(int pageNumber, int pageSize, string search_name)
        {
            if (search_name != null) search_name = search_name.Trim().ToLower();
            List<package> list;
            List<packageviewmodel> res = new List<packageviewmodel>();
            var skipAmount = pageSize * pageNumber;
            if (search_name == null)
            {
                list = _dbContext.packages.ToList();
            }
            else list = _dbContext.packages.Where(x => x.pac_name.ToLower().Contains(search_name) || x.pac_code.ToLower().Contains(search_name)).ToList();
            var total = list.Count();

            var results = list.OrderByDescending(t => t.pac_id).Skip(skipAmount).Take(pageSize);
            foreach (package pa in results)
            {
                packageviewmodel packageview = _mapper.Map<packageviewmodel>(pa);
                if (packageview.pac_status != 0 || packageview.pac_status != null)
                    packageview.pac_status_name = EnumPackage.pac_status[Convert.ToInt32(packageview.pac_status) - 1];
                List<function> lts_fun = _dbContext.functions.Where(x => x.package_id == pa.pac_id).ToList();
                packageview.list_function = lts_fun;
                res.Add(packageview);
            }

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<packageviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = total
            };
        }
        public packageviewmodel GetById(int id)
        {
            package pa = _dbContext.packages.Find(id);
            packageviewmodel packageview = _mapper.Map<packageviewmodel>(pa);
            if (packageview.pac_status != 0 || packageview.pac_status != null)
                packageview.pac_status_name = EnumPackage.pac_status[Convert.ToInt32(packageview.pac_status) - 1];
            List<function> lts_fun = _dbContext.functions.Where(x => x.package_id == pa.pac_id).ToList();
            packageview.list_function = lts_fun;
            return packageview;
        }
        public List<packageviewmodel> GetAllDropDown()
        {
            List<packageviewmodel> res = new List<packageviewmodel>();
            var results = _dbContext.packages.ToList();
            foreach (package pa in results)
            {
                packageviewmodel packageview = _mapper.Map<packageviewmodel>(pa);
                if (packageview.pac_status != 0 || packageview.pac_status != null)
                    packageview.pac_status_name = EnumPackage.pac_status[Convert.ToInt32(packageview.pac_status) - 1];
                List<function> lts_fun = _dbContext.functions.Where(x => x.package_id == pa.pac_id).ToList();
                packageview.list_function = lts_fun;
                res.Add(packageview);
            }

            return res;
        }
    }
}