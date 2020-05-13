using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Package;
using System;
using System.Collections.Generic;
namespace ERP.Service.Services.IServices
{
    public interface IPackageService : IGenericService<package>
    {
        PagedResults<package> CreatePagedResults(int pageNumber, int pageSize);
        PagedResults<packageviewmodel> GetAllSearch(int pageNumber, int pageSize, string search_name);
        packageviewmodel GetById(int id);
        List<dropdown> GetAllDropDown();
    }
}
