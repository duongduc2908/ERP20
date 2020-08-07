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
    public interface ITrainingService : IGenericService<training>
    {
        PagedResults<training> CreatePagedResults(int pageNumber, int pageSize);
        PagedResults<training> GetAllSearch(int pageNumber, int pageSize,string search_name, DateTime? start_date, DateTime? end_date);
        training GetById(int tn_id);
        List<dropdown> GetAllName();


    }
}
