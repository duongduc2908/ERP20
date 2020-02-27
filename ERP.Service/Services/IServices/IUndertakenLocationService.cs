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
    public interface IUndertakenLocationService : IGenericService<undertaken_location>
    {
        PagedResults<undertaken_location> CreatePagedResults(int pageNumber, int pageSize);
        List<dropdown> GetAllProvince();
        List<dropdown> GetAllDistrictByIdPro(int? province_id);
        List<dropdown> GetAllWardByIdDis(int? district_id);

    }
}
