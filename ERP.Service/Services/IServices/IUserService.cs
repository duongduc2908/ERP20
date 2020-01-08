using Land.Common.GenericService;
using Land.Data.ModelsLand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace ManagementLibrary.Service.Services.IServices
{
    interface IUserService : IGenericService<>
    {
        PagedResults<Company> CreatePagedResults(int pageNumber, int pageSize);
    }
}
