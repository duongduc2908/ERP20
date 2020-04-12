using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Service.Services
{
    public class TrainingStaffService : GenericService<training_staff>, ITrainingStaffService
    {
        private readonly ITrainingStaffRepository _repository;
        public TrainingStaffService(ITrainingStaffRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<training_staff> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
    }
}