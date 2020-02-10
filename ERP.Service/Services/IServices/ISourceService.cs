using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Service.Services.IServices
{
    public interface ISourceService : IGenericService<source>
    {
        PagedResults<source> GetAllPage(int pageNumber, int pageSize);
        PagedResults<source> GetAllPageById(int pageNumber, int pageSize, int id);
        PagedResults<source> GetSources(string search_name);
    }
}
