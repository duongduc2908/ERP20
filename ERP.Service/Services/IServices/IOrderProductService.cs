﻿using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Service.Services.IServices
{
    public interface IOrderProductService : IGenericService<order_product>
    {
        PagedResults<order_product> CreatePagedResults(int pageNumber, int pageSize);
    }
}
