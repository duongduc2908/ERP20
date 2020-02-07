using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Repository.Repositories.IRepositories
{
    public interface IStaffRepository : IGenericRepository<staff>
    {
        PagedResults<staff> CreatePagedResults(int pageNumber, int pageSize);
        void ChangePassword(ChangePasswordBindingModel model, int id);
        PagedResults<staffviewmodel> GetAllPage(int pageNumber, int pageSize);
        PagedResults<staffviewmodel> GetInforById(int id);
        PagedResults<string> GetInforManager();
    }
}
