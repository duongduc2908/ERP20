using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Service.Services
{
    public class TrainingService : GenericService<training>, ITrainingService
    {
        private readonly ITrainingRepository _repository;
        public TrainingService(ITrainingRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<training> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        } 
        public PagedResults<training> GetAllSearch(int pageNumber, int pageSize, string search_name)
        {
            return this._repository.GetAllSearch(pageNumber, pageSize,search_name);
        } 
        public List<dropdown> GetAllName()
        {
            return this._repository.GetAllName();
        }
        public training GetById(int tn_id)
        {
            return this._repository.GetById(tn_id);
        }

    }
}