using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Service.Services.IServices
{
    public interface IDepartmentService : IGenericService<department>
    {
        PagedResults<department> CreatePagedResults(int pageNumber, int pageSize);
        List<dropdown> Get_Level_One(int company_id);
        List<dropdown> Get_Children(int id);


    }
}
