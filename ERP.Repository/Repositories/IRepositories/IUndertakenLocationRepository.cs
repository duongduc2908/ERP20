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
    public interface IUndertakenLocationRepository : IGenericRepository<undertaken_location>
    {
        PagedResults<undertaken_location> CreatePagedResults(int pageNumber, int pageSize);
        List<dropdown> GetAllProvince();
        List<dropdown> GetAllDistrictByIdPro(int? province_id);
        List<dropdown> GetAllWardByIdDis(int? district_id);
    }
}
