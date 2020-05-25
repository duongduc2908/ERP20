using AutoMapper;
using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Repository.Repositories.IRepositories;
using System.Collections.Generic;
using System.Linq;
namespace ERP.Repository.Repositories
{
    public class AttachmentsRepository : GenericRepository<attachment>, IAttachmentsRepository
    {
        private readonly IMapper _mapper;
        public AttachmentsRepository(ERPDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._mapper = mapper;
        }
        public AttachmentsRepository(ERPDbContext dbContext) : base(dbContext)
        {
        }
        

        public List<dropdown> GetAllDropDown()
        {
            List<dropdown> res = new List<dropdown>();
            var list_company = _dbContext.attachments.ToList();
            foreach (var co in list_company)
            {
                dropdown dr = new dropdown();
                //Do something
                dr.id = co.ast_id;
                dr.name = co.ast_filename;
                res.Add(dr);
            }
            return res;
        }
    }
}