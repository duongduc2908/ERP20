using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.ExportDB;
using ERP.Data.ModelsERP.ModelView.StatisticStaff;
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
        bool ChangePassword(ChangePasswordBindingModel model, int id);
        
        PagedResults<staffviewmodel> GetAllPage(int pageNumber, int pageSize);
        PagedResults<staffviewmodel> GetAllPageSearch(int pageNumber, int pageSize, int? status, DateTime? start_date, DateTime? end_date, string name);
        PagedResults<staffview> ExportStaff(int pageNumber, int pageSize, int? status, DateTime? start_date, DateTime? end_date,string name);
        staffviewmodel GetInforById(int id);
        PagedResults<staffviewmodel> GetAllActive(int pageNumber, int pageSize, int status);

        List<dropdown> GetInforManager();
        statisticstaffviewmodel GetInfor(int staff_id);
    }
}
