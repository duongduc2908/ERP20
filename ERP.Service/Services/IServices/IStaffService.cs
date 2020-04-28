using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.ExportDB;
using ERP.Data.ModelsERP.ModelView.Service;
using ERP.Data.ModelsERP.ModelView.StatisticStaff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Service.Services.IServices
{
    public interface IStaffService : IGenericService<staff>
    {
        PagedResults<staff> CreatePagedResults(int pageNumber, int pageSize);
        bool ChangePassword(ChangePasswordBindingModel model, int id);
        PagedResults<staffviewmodel> GetAllPage(int pageNumber, int pageSize);
        PagedResults<staffviewmodel> GetAllPageSearch(int pageNumber, int pageSize, int ? status,DateTime ? start_date, DateTime ? end_date, string name, int? sta_working_status);
        //PagedResults<servicestaffviewmodel> GetAllPageSearchStaffFree(int pageNumber, int pageSize,DateTime ? start_date, DateTime ? end_date);
        PagedResults<staffview> ExportStaff(int pageNumber, int pageSize, int? status, DateTime? start_date, DateTime? end_date, string name, int? sta_working_status);
        staffviewmodel GetInforById(int id);
        bool Check_location(undertaken_location un);  
        PagedResults<staffviewmodel> GetAllActive(int pageNumber, int pageSize, int status);
       
        List<dropdown> GetInforManager();
        statisticstaffviewmodel GetInfor(int staff_id);
    }
}
