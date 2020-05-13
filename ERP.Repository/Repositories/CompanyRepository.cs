using AutoMapper;
using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Company;
using ERP.Repository.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Repository.Repositories
{
    public class CompanyRepository : GenericRepository<company>, ICompanyRepository
    {
        private readonly IMapper _mapper;
        public CompanyRepository(ERPDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._mapper = mapper;
        }
        public CompanyRepository(ERPDbContext dbContext) : base(dbContext)
        {
        }
        public PagedResults<company> CreatePagedResults(int pageNumber, int pageSize)
        {
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.companies.OrderBy(t => t.co_id).Skip(skipAmount).Take(pageSize);

            var totalNumberOfRecords = _dbContext.companies.Count();

            var results = list.ToList();

            var mod = totalNumberOfRecords % pageSize;

            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<company>
            {
                Results = results,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords
            };
        }
        public PagedResults<companyviewmodel> GetAllSearch(int pageNumber, int pageSize, string search_name)
        {
            if (search_name != null) search_name = search_name.Trim().ToLower();
            List<company> list;
            List<companyviewmodel> res = new List<companyviewmodel>();
            var skipAmount = pageSize * pageNumber;
            if (search_name == null)
            {
                list = _dbContext.companies.ToList();
            }
            else list = _dbContext.companies.Where(x => x.co_name.ToLower().Contains(search_name) || x.co_code.ToLower().Contains(search_name)).ToList();


            var total = list.Count();

            var results = list.OrderByDescending(t => t.co_id).Skip(skipAmount).Take(pageSize);
            foreach (company co in results)
            {
                companyviewmodel companyview = _mapper.Map<companyviewmodel>(co);
                var staff = _dbContext.staffs.Find(companyview.co_no_of_employees);
                if (staff != null) companyview.sta_name = staff.sta_fullname;


                //List package fuction 
                /*
                 * select p.pac_name, f.fun_name, f.fun_id,p.pac_id
                    from company 
                    inner join company_funtion co_f on co_f.company_id = 1
                    inner join function1 f on f.fun_id = co_f.fun_id 
                    inner join package p on p.funtion_id = f.fun_id 
                    group by  p.pac_name, f.fun_name, f.fun_id,p.pac_id
                 */
                var list_function = (from com in _dbContext.companies
                                     join co_f in _dbContext.company_funtion on co.co_id equals co_f.company_id
                                     join f in _dbContext.functions on co_f.fun_id equals f.fun_id
                                     join p in _dbContext.packages on f.package_id equals p.pac_id
                                     group new { f, p } by new { f.fun_name, f.fun_id,f.fun_code,p.pac_id, p.pac_name,p.pac_code} into temp
                                     select temp
                                     ).ToList();
                List<packagefunctionviewmodel> lts_pac_f = new List<packagefunctionviewmodel>();

                foreach(var fun in list_function)
                {
                    packagefunctionviewmodel pac_f = new packagefunctionviewmodel();
                    pac_f.fun_name = fun.Key.fun_name;
                    pac_f.pac_name = fun.Key.pac_name;
                    pac_f.fun_id = fun.Key.fun_id;
                    pac_f.pac_id = fun.Key.pac_id;
                    pac_f.fun_code = fun.Key.fun_code;
                    pac_f.pac_code = fun.Key.pac_code;
                    lts_pac_f.Add(pac_f);
                }
                companyview.list_package_function = lts_pac_f;
                res.Add(companyview);
            }

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<companyviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = total
            };
        }
        public companyviewmodel GetById(int id, bool? login = false)
        {
            companyviewmodel companyview;
            company co = _dbContext.companies.Find(id);
            if(login==false)
            {
                companyview = _mapper.Map<companyviewmodel>(co);
            }
            else
            {
                companyview = new companyviewmodel();
                companyview.co_id = co.co_id;
                companyview.co_no_of_employees = co.co_no_of_employees;
            }
            var staff = _dbContext.staffs.Find(companyview.co_no_of_employees);
            if (staff != null) companyview.sta_name = staff.sta_fullname;


            //List package fuction 
            /*
             * select p.pac_name, f.fun_name, f.fun_id,p.pac_id
                from company 
                inner join company_funtion co_f on co_f.company_id = 1
                inner join function1 f on f.fun_id = co_f.fun_id 
                inner join package p on p.funtion_id = f.fun_id 
                group by  p.pac_name, f.fun_name, f.fun_id,p.pac_id
             */
            var list_function = (from com in _dbContext.companies
                                 join co_f in _dbContext.company_funtion on co.co_id equals co_f.company_id
                                 join f in _dbContext.functions on co_f.fun_id equals f.fun_id
                                 join p in _dbContext.packages on f.package_id equals p.pac_id
                                 group new { f, p } by new { f.fun_name, f.fun_id,f.fun_code, p.pac_id, p.pac_name,p.pac_code } into temp
                                 select temp
                                 ).ToList();
            List<packagefunctionviewmodel> lts_pac_f = new List<packagefunctionviewmodel>();

            foreach (var fun in list_function)
            {
                packagefunctionviewmodel pac_f = new packagefunctionviewmodel();
                pac_f.fun_name = fun.Key.fun_name;
                pac_f.pac_name = fun.Key.pac_name;
                pac_f.fun_id = fun.Key.fun_id;
                pac_f.pac_id = fun.Key.pac_id;
                pac_f.fun_code = fun.Key.fun_code;
                pac_f.pac_code = fun.Key.pac_code;
                lts_pac_f.Add(pac_f);
            }
            companyview.list_package_function = lts_pac_f;
            return companyview;
        }
        public List<dropdown> GetAllDropDown()
        {
            List<dropdown> res = new List<dropdown>();
            var list_company = _dbContext.companies.ToList();
            foreach(var co in list_company)
            {
                dropdown dr = new dropdown();
                dr.id = co.co_id;
                dr.name = co.co_name;
                res.Add(dr);
            }
            return res;
        }
    }
}