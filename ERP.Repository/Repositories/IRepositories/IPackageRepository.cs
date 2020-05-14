using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Repository.Repositories.IRepositories
{
    public interface IPackageRepository : IGenericRepository<package>
    {
        PagedResults<package> CreatePagedResults(int pageNumber, int pageSize);
        PagedResults<packageviewmodel> GetAllSearch(int pageNumber, int pageSize, string search_name);
        packageviewmodel GetById(int id);
        List<packageviewmodel> GetAllDropDown();
    }
}
