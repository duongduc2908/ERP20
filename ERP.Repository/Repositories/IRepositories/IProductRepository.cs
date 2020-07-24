using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.ExportDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Repository.Repositories.IRepositories
{
    public interface IProductRepository : IGenericRepository<product>
    {
        PagedResults<productviewmodel> GetAllPage(int pageNumber, int pageSize);
        productviewmodel GetAllPageById(int id);
        PagedResults<productviewmodel> GetProducts(int pageNumber, int pageSize, DateTime? start_date, DateTime? end_date, string search_name, int? category_id, int company_id);
        PagedResults<productview> ExportProduct(int pageNumber, int pageSize, DateTime? start_date, DateTime? end_date, string search_name, int? category_id, int company_id);
        List<dropdown> GetUnit(int company_id);
    }
}
