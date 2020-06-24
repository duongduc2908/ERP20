using ERP.Common.GenericRepository;
using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.ExportDB;
using ERP.Data.ModelsERP.ModelView.StatisticStaff;
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
        public bool ChangePassword(ChangePasswordBindingModel model, int id)
        {
            return this._repository.ChangePassword(model, id);
        }
        public PagedResults<staffviewmodel> GetAllPage(int pageNumber, int pageSize)
        {
            return this._repository.GetAllPage(pageNumber, pageSize);
        }
        public PagedResults<staffviewmodel> GetAllPageSearch(int pageNumber, int pageSize, int? status, DateTime? start_date, DateTime? end_date, string name, int? sta_working_status, int company_id)
        {
            return this._repository.GetAllPageSearch(pageNumber, pageSize, status, start_date, end_date, name, sta_working_status,company_id);
        }
        public PagedResults<staffview> ExportStaff(int pageNumber, int pageSize, int? status, DateTime? start_date, DateTime? end_date, string name, int? sta_working_status)
        {
            return this._repository.ExportStaff(pageNumber, pageSize, status, start_date, end_date, name, sta_working_status);
        }
        public staffviewmodel GetInforById(int id)
        {
            return this._repository.GetInforById(id);
        }
        public PagedResults<staffviewmodel> GetAllActive(int status, int pageNumber, int pageSize)
        {
            return this._repository.GetAllActive(status, pageNumber, pageSize);
        }
        
        public bool Check_location(undertaken_location un)
        {
            return this._repository.Check_location(un);
        }
        public List<dropdown> GetInforManager()
        {
            return this._repository.GetInforManager();
        }
        
        public statisticstaffviewmodel GetInfor(int staff_id)
        {
            return this._repository.GetInfor(staff_id);
        }
        public List<dropdown> GetAllDropdown(int company_id)
        {
            return this._repository.GetAllDropdown(company_id);
        }

        
    }
}