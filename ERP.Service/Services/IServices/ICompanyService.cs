using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Company;
using System;
using System.Collections.Generic;
namespace ERP.Service.Services.IServices
{
    public interface ICompanyService : IGenericService<company>
    {
        PagedResults<company> CreatePagedResults(int pageNumber, int pageSize);
        PagedResults<companyviewmodel> GetAllSearch(int pageNumber, int pageSize, string search_name);
        companyviewmodel GetById(int id, bool? login = false);
        List<dropdown> GetAllDropDown();
    }
}
