using ERP.Common.GenericService;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Repository.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Repository.Repositories
{
    public class TestRepository: GenericService<customer>, ITestRepository
    {
        public TestRepository(ERPDbContext dbContext) : base(dbContext)
        {
        }
    }
}