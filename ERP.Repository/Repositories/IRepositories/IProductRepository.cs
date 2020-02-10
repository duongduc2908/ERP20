using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
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
        PagedResults<productviewmodel> GetAllPageById(int id);
        PagedResults<product> GetProducts(string search_name);
    }
}
