using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.ExportDB;
using ERP.Data.ModelsERP.ModelView.Sms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Repository.Repositories.IRepositories
{
    public interface ICustomerRepository : IGenericRepository<customer>
    {
        PagedResults<customerviewmodel> GetAllPage(int pageNumber, int pageSize);
        PagedResults<customerviewmodel> GetAllPageBySource(int pageNumber, int pageSize, int source_id);
        PagedResults<customerviewmodel> GetAllPageByType(int pageNumber, int pageSize, int cu_type);
        PagedResults<customerviewmodel> GetAllPageByGroup(int pageNumber, int pageSize, int customer_group_id);
        PagedResults<customerviewmodel> GetAllPageSearch(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, string name);
        PagedResults<customerview> ExportCustomer(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, string name);
        PagedResults<smscustomerviewmodel> GetAllPageSearchSms(int pageNumber, int pageSize, int? source_id, int? cu_type, int? customer_group_id, string name);
        
        customerviewmodel GetInfor(int cu_id);
        List<dropdown> GetAllType();

    }
}
