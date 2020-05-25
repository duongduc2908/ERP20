using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using System.Collections.Generic;
namespace ERP.Repository.Repositories.IRepositories
{
    public interface IRelativesStaffRepository : IGenericRepository<relatives_staff>
    {
        PagedResults<relatives_staff> CreatePagedResults(int pageNumber, int pageSize);
        List<dropdown> GetAllDropDown();
    }
}
