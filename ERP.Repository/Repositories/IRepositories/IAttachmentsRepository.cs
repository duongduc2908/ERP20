using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using System;
using System.Collections.Generic;

namespace ERP.Repository.Repositories.IRepositories
{
    public interface IAttachmentsRepository : IGenericRepository<attachment>
    {
        List<dropdown> GetAllDropDown();
    }
}
