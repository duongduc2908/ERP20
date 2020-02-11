using ERP.Common.GenericRepository;
using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Repository.Repositories;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Service.Services
{
    public class StaffService : GenericService<staff>, IStaffService
    {
        private readonly IStaffRepository _repository;
        public StaffService(IStaffRepository repository) : base(repository)
        {
            this._repository = repository;
        }

        public PagedResults<staff> CreatePagedResults(int pageNumber, int pageSize)
        {
            return this._repository.CreatePagedResults(pageNumber, pageSize);
        }
        public void ChangePassword(ChangePasswordBindingModel model, int id)
        {
            this._repository.ChangePassword(model, id);
        }
        public PagedResults<staffviewmodel> GetAllPage(int pageNumber, int pageSize)
        {
            return this._repository.GetAllPage(pageNumber, pageSize);
        }
        public PagedResults<staffviewmodel> GetInforById(int id)
        {
            return this._repository.GetInforById(id);
        }
        public PagedResults<staffviewmodel> GetAllActive(int status, int pageNumber, int pageSize)
        {
            return this._repository.GetAllActive(status, pageNumber, pageSize);
        }
        public void Export(int pageNumber, int pageSize)
        {
            this._repository.Export(pageSize,pageNumber);
        }

        public PagedResults<string> GetInforManager()
        {
            return this._repository.GetInforManager();
        }
        public PagedResults<staffviewmodel> Import(string Path, string sheetname)
        {
            return this._repository.Import(Path, sheetname);
        }
    }
}