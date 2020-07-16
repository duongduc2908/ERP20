using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.CustomerGroup;
using ERP.Data.ModelsERP.ModelView.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Service.Services.IServices
{
    public interface ICustomerGroupService : IGenericService<customer_group>
    {
        PagedResults<customergroupviewmodel> GetAllPageSearch(int pageNumber, int pageSize, int? cg_id, string name,int company_id);
        List<piechartview> GetPieChart(int commpanyid);

        bool CheckUniqueName(string name,int id);
        List<statisticrevenueviewmodel> GetRevenueCustomerGroup(int staff_id);
        List<dropdown> GetAllDropdown(int company_id);
        customergroupviewmodel GetById(int cg_id);
    }
}
