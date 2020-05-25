using ERP.Common.GenericService;
using ERP.Common.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Repository.Repositories.IRepositories;
using ERP.Service.Services.IServices;
using System;
using System.Collections.Generic;
namespace ERP.Service.Services
{
    public class AttachmentsService : GenericService<attachment>, IAttachmentsService
    {
        private readonly IAttachmentsRepository _repository;
        public AttachmentsService(IAttachmentsRepository repository) : base(repository)
        {
            this._repository = repository;
        }


       

        public List<dropdown> GetAllDropDown()
        {
            return this._repository.GetAllDropDown();
        }
    }
}