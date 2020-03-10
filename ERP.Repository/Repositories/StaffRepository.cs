using AutoMapper;
using ERP.Common.Constants;
using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Repository.Repositories.IRepositories;
using OfficeOpenXml;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.Repository.Repositories
{
    public class StaffRepository : GenericRepository<staff>, IStaffRepository
    {
        private readonly IMapper _mapper;
        public StaffRepository(ERPDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._mapper = mapper;
        }
        public PagedResults<staff> CreatePagedResults(int pageNumber, int pageSize)
        {
            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.staffs.OrderBy(t => t.sta_id).Skip(skipAmount).Take(pageSize);
            var total = _dbContext.staffs.Count();
           

            var results = list.ToList();

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<staff>
            {
                Results = results,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = total
            };
        }
        public void ChangePassword(ChangePasswordBindingModel model, int id)
        {

            var current_user = _dbContext.staffs.FirstOrDefault(x => x.sta_id == id);
            var new_user = current_user;
            if (current_user != null)
            {
                if (current_user.sta_password.Contains(HashMd5.convertMD5(model.OldPassword)))
                {
                    new_user.sta_password = HashMd5.convertMD5(model.NewPassword);
                    _dbContext.Entry(current_user).CurrentValues.SetValues(new_user);
                    _dbContext.SaveChanges();
                }
            }
        }
        
        public PagedResults<staffviewmodel> GetAllPage(int pageNumber, int pageSize)
        {
            List<staffviewmodel> res = new List<staffviewmodel>();

            var skipAmount = pageSize * pageNumber;

            var list = _dbContext.staffs.OrderBy(t => t.sta_id).Skip(skipAmount).Take(pageSize);
            var total = _dbContext.staffs.Count();
            var results = list.ToList();
            foreach(staff i in results)
            {
                var staffview = _mapper.Map<staffviewmodel>(i);
                var deparment = _dbContext.departments.FirstOrDefault(x => x.de_id == i.department_id);
                var position = _dbContext.positions.FirstOrDefault(x => x.pos_id == i.position_id);
                //var group_role = _dbContext.group_role.FirstOrDefault(x => x.gr_id == i.group_role_id);
                staffview.department_name = deparment.de_name;
                staffview.position_name = position.pos_name;
                //staffview.group_name = group_role.gr_name;
                res.Add(staffview);
            }

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<staffviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = total
            };
        }
        public PagedResults<staffviewmodel> GetAllPageSearch(int pageNumber, int pageSize, int? status, string name)
        {
            if(name != null)
            {
                name = name.Trim();
            }
            
            List<staffviewmodel> res = new List<staffviewmodel>();
            var skipAmount = pageSize * pageNumber;
            var list = _dbContext.staffs.Where(t => t.sta_status == status && t.sta_fullname.Contains(name) || t.sta_mobile.Contains(name) || t.sta_email.Contains(name) || t.sta_code.Contains(name) || t.sta_username.Contains(name)).OrderBy(t => t.sta_id).Skip(skipAmount).Take(pageSize);
            if (status == null)
            {
                if (name != null)
                {
                    list = _dbContext.staffs.Where(t => t.sta_fullname.Contains(name) || t.sta_mobile.Contains(name) || t.sta_email.Contains(name) || t.sta_code.Contains(name) || t.sta_username.Contains(name)).OrderBy(t => t.sta_id).Skip(skipAmount).Take(pageSize);
                }
                else
                {
                    list = _dbContext.staffs.OrderBy(t => t.sta_id).Skip(skipAmount).Take(pageSize);
                }
            }
            if(name == null)
            {
                if (status != null)
                {
                    list = _dbContext.staffs.Where(t => t.sta_status == status).OrderBy(t => t.sta_id).Skip(skipAmount).Take(pageSize);
                }
                else
                {
                    list = _dbContext.staffs.OrderBy(t => t.sta_id).Skip(skipAmount).Take(pageSize);
                }
            }

            var total = _dbContext.staffs.Count();
            

            var results = list.ToList();
            foreach (staff i in results)
            {
                var staffview = _mapper.Map<staffviewmodel>(i);
                var deparment = _dbContext.departments.FirstOrDefault(x => x.de_id == i.department_id);
                var position = _dbContext.positions.FirstOrDefault(x => x.pos_id == i.position_id);
                //var group_role = _dbContext.group_role.FirstOrDefault(x => x.gr_id == i.group_role_id);
                staffview.department_name = deparment.de_name;
                staffview.position_name = position.pos_name;
                //staffview.group_name = group_role.gr_name;
                
                //Lay dia chi 
                var list_address = _dbContext.undertaken_location.Where(s => s.staff_id == i.sta_id).ToList();
                List<undertakenlocationviewmodel> lst_add = new List<undertakenlocationviewmodel>();
                foreach (undertaken_location s in list_address)
                {
                    undertakenlocationviewmodel  add = _mapper.Map<undertakenlocationviewmodel>(s);
                    add.ward_id = _dbContext.ward.Where(t => t.Name.Contains(s.unl_ward)).FirstOrDefault().Id;
                    add.district_id = _dbContext.district.Where(t => t.Name.Contains(s.unl_district)).FirstOrDefault().Id;
                    add.province_id = _dbContext.province.Where(t => t.Name.Contains(s.unl_province)).FirstOrDefault().Id;
                    lst_add.Add(add);
                }
                staffview.list_address = lst_add;
                res.Add(staffview);
            }

            

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<staffviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = total
            };
        }
     
        public PagedResults<staffviewmodel> GetAllActive(int pageNumber, int pageSize, int status)
        {
            List<staffviewmodel> res = new List<staffviewmodel>();

            var skipAmount = pageSize * pageNumber;

           var list = _dbContext.staffs.OrderBy(t => t.sta_id).Skip(skipAmount).Take(pageSize);

            if (status == 0)
            {
                list = _dbContext.staffs.Where(t => t.sta_status == 0).OrderBy(t => t.sta_id).Skip(skipAmount).Take(pageSize);
            }
            var total = _dbContext.staffs.Count();
            

            var results = list.ToList();
            foreach (staff i in results)
            {
                var staffview = _mapper.Map<staffviewmodel>(i);
                var deparment = _dbContext.departments.FirstOrDefault(x => x.de_id == i.department_id);
                var position = _dbContext.positions.FirstOrDefault(x => x.pos_id == i.position_id);
                //var group_role = _dbContext.group_role.FirstOrDefault(x => x.gr_id == i.group_role_id);
                staffview.department_name = deparment.de_name;
                staffview.position_name = position.pos_name;
                //staffview.group_name = group_role.gr_name;
                res.Add(staffview);
            }

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<staffviewmodel>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = total
            };
        }
        public staffviewmodel GetInforById(int id)
        {
            staffviewmodel res = new staffviewmodel();
            var staff_cur = _dbContext.staffs.Where(i => i.sta_id == id).FirstOrDefault();
            var satffview = _mapper.Map<staffviewmodel>(staff_cur);
            res = satffview;
            var deparment = _dbContext.departments.FirstOrDefault(x => x.de_id == staff_cur.department_id);
            res.department_name = deparment.de_name;
            var position = _dbContext.positions.FirstOrDefault(x => x.pos_id == staff_cur.position_id);
            res.position_name = position.pos_name;

            res.group_name = _dbContext.group_role.FirstOrDefault(x => x.gr_id == staff_cur.group_role_id).gr_name;
            var list_address = _dbContext.undertaken_location.Where(i => i.staff_id == staff_cur.sta_id).ToList();
            List<undertakenlocationviewmodel> lst = new List<undertakenlocationviewmodel>();
            foreach (undertaken_location i in list_address)
            {
                undertakenlocationviewmodel add = _mapper.Map<undertakenlocationviewmodel>(i);
                add.ward_id = _dbContext.ward.Where(t => t.Name.Contains(i.unl_ward)).FirstOrDefault().Id;
                add.district_id = _dbContext.district.Where(t => t.Name.Contains(i.unl_district)).FirstOrDefault().Id;
                add.province_id = _dbContext.province.Where(t => t.Name.Contains(i.unl_province)).FirstOrDefault().Id;
                lst.Add(add);
            }
            res.list_address = lst;
            return res;
            
        }
        public List<dropdown> GetInforManager()
        {
            List<dropdown> res = new List<dropdown>();
            var staff = _dbContext.staffs.Where(s => s.sta_leader_flag == 1).ToList();
            var totalNumberOfRecords = staff.Count();

            foreach(staff i in staff)
            {
                dropdown dr = new dropdown();
                dr.id = i.sta_id;
                dr.name = i.sta_fullname;
                res.Add(dr);
            }
            return res;
        }
    }
}