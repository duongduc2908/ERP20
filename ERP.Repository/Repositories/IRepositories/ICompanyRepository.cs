using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Company;
using System;
using System.Collections.Generic;

namespace ERP.Repository.Repositories.IRepositories
{
    public interface ICompanyRepository : IGenericRepository<company>
    {
        PagedResults<company> CreatePagedResults(int pageNumber, int pageSize);
        PagedResults<companyviewmodel> GetAllSearch(int pageNumber, int pageSize, string search_name);
        companyviewmodel GetById(int id,bool? login = false);
        List<dropdown> GetAllDropDown();
    }
}
