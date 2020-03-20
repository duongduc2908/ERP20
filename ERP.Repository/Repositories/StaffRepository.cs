using AutoMapper;
using ERP.Common.Constants;
using ERP.Common.Constants.Enums;
using ERP.Common.GenericRepository;
using ERP.Common.Models;
using ERP.Data;
using ERP.Data.DbContext;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.ExportDB;
using ERP.Data.ModelsERP.ModelView.StatisticStaff;
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
        public bool ChangePassword(ChangePasswordBindingModel model, int id)
        {

            var current_user = _dbContext.staffs.FirstOrDefault(x => x.sta_id == id);
            staff new_user = new staff();
            new_user = current_user;
            if (current_user != null)
            {
                if (current_user.sta_password.Contains(HashMd5.convertMD5(model.OldPassword)))
                {
                    new_user.sta_password = HashMd5.convertMD5(model.NewPassword);
                    if (new_user.sta_login == true)
                    {
                        new_user.sta_login = false;
                        
                    }
                    _dbContext.Entry(current_user).CurrentValues.SetValues(new_user);
                    _dbContext.SaveChanges();
                    return true;
                }
            }
            return false;
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
        public PagedResults<staffviewmodel> GetAllPageSearch(int pageNumber, int pageSize, int? status, DateTime? start_date, DateTime? end_date, string name)
        {
            if(name != null)
            {
                name = name.Trim();
            }
            List<staff> list_res;
            List<staff> list;
            List<staffviewmodel> res = new List<staffviewmodel>();
            var skipAmount = pageSize * pageNumber;
            if(status == null)
            {
                list_res = _dbContext.staffs.ToList();
            }
            else list_res = _dbContext.staffs.Where(x => x.sta_status == status).ToList();
            if (start_date != null)
            {
                list_res = list_res.Where(x => x.sta_created_date >= start_date).ToList();
            }
            if (end_date != null)
            {
                list_res = list_res.Where(x => x.sta_created_date <= end_date).ToList();
            }
            if (name != null)
            {
                list_res = list_res.Where(t => t.sta_fullname.Contains(name) || t.sta_mobile.Contains(name) || t.sta_email.Contains(name) || t.sta_code.Contains(name) || t.sta_username.Contains(name)).ToList();
            }
            list = list_res.OrderBy(t => t.sta_id).Skip(skipAmount).Take(pageSize).ToList();
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
        public statisticstaffviewmodel GetInfor(int staff_id)
        {
            statisticstaffviewmodel res = new statisticstaffviewmodel();
           
            var staff_cur = _dbContext.staffs.Find(staff_id);
            if(staff_cur != null)
            {
                //Lay ra thong tin nhan su 
                var satffview = _mapper.Map<statisticstaffviewmodel>(staff_cur);
                res = satffview;
                var deparment = _dbContext.departments.FirstOrDefault(x => x.de_id == staff_cur.department_id);
                res.department_name = deparment.de_name;
                for (int j = 1; j < EnumsStaff.sta_status.Length + 1; j++)
                {
                    if (j == satffview.sta_status)
                    {
                        res.sta_status_name = EnumsStaff.sta_status[j - 1];
                    }
                }
                //Lấy ra thông tin mạng xã hội
                var social = _dbContext.socials.Where(t => t.staff_id == staff_id).FirstOrDefault();
                if(social != null)
                {
                    res.social = social;
                }
                //Lấy ra thông tin thống kê 
                statisticviemodel statistic = new statisticviemodel();
                DateTime curr_day = DateTime.Now;
                DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 1);
                DateTime firstMonth = Utilis.GetFirstDayOfMonth(curr_day);
                DateTime firstWeek = Utilis.GetFirstDayOfWeek(curr_day);
                //Admin // user 
                if (staff_cur.group_role_id == 1)
                {
                    statistic.totalRevenueByMonth = _dbContext.customer_order.Where(i => i.cuo_date <= DateTime.Now && i.cuo_date >= firstMonth).Sum(i => i.cuo_total_price);
                    statistic.totalRevenueByWeek = _dbContext.customer_order.Where(i => i.cuo_date <= DateTime.Now && i.cuo_date >= firstWeek).Sum(i => i.cuo_total_price);
                    statistic.totalRevenueByDay = _dbContext.customer_order.Where(i => i.cuo_date <= DateTime.Now && i.cuo_date >= firstDay).Sum(i => i.cuo_total_price);
                    statistic.totalRevenue = _dbContext.customer_order.Sum(i => i.cuo_total_price);
                }
                else
                {
                    statistic.totalRevenueByMonth = _dbContext.customer_order.Where(i => i.cuo_date <= DateTime.Now && i.cuo_date >= firstMonth && i.staff_id == staff_id).Sum(i => i.cuo_total_price);
                    statistic.totalRevenueByWeek = _dbContext.customer_order.Where(i => i.cuo_date <= DateTime.Now && i.cuo_date >= firstWeek && i.staff_id == staff_id).Sum(i => i.cuo_total_price);
                    statistic.totalRevenueByDay = _dbContext.customer_order.Where(i => i.cuo_date <= DateTime.Now && i.cuo_date >= firstDay && i.staff_id == staff_id).Sum(i => i.cuo_total_price);
                    statistic.totalRevenue = _dbContext.customer_order.Where(i => i.staff_id == staff_id).Sum(i => i.cuo_total_price);
                }
                res.statistic = statistic;

            }
            
            return res;
        }
        public PagedResults<staffview> ExportStaff(int pageNumber, int pageSize, int? status, DateTime? start_date, DateTime? end_date, string name)
        {
            if (name != null)
            {
                name = name.Trim();
            }

            List<staffview> res = new List<staffview>();
            List<staff> list_res;
            List<staff> list;
            var skipAmount = pageSize * pageNumber;
            if (status == null)
            {
                list_res = _dbContext.staffs.ToList();
            }
            else list_res = _dbContext.staffs.Where(x => x.sta_status == status).ToList();
            if (start_date != null)
            {
                list_res = list_res.Where(x => x.sta_created_date >= start_date).ToList();
            }
            if (end_date != null)
            {
                list_res = list_res.Where(x => x.sta_created_date <= end_date).ToList();
            }
            if (name != null)
            {
                list_res = list_res.Where(t => t.sta_fullname.Contains(name) || t.sta_mobile.Contains(name) || t.sta_email.Contains(name) || t.sta_code.Contains(name) || t.sta_username.Contains(name)).ToList();
            }
            list = list_res.OrderBy(t => t.sta_id).Skip(skipAmount).Take(pageSize).ToList();


            var total = _dbContext.staffs.Count();


            var results = list.ToList();
            foreach (staff i in results)
            {
                var staffex = _mapper.Map<staffview>(i);
                var deparment = _dbContext.departments.FirstOrDefault(x => x.de_id == i.department_id);
                var position = _dbContext.positions.FirstOrDefault(x => x.pos_id == i.position_id);
                staffex.department_name = deparment.de_name;
                staffex.position_name = position.pos_name;
                staffex.sta_sex_name = EnumsStaff.sta_sex[Convert.ToInt32(staffex.sta_sex)];
                staffex.sta_status_name = EnumsStaff.sta_status[Convert.ToInt32(staffex.sta_status)];
                if (staffex.sta_leader_flag == 1) staffex.sta_leader_name = "Có";
                else staffex.sta_leader_name = "Không";
                res.Add(staffex);
            }

            var mod = total % pageSize;

            var totalPageCount = (total / pageSize) + (mod == 0 ? 0 : 1);

            return new PagedResults<staffview>
            {
                Results = res,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = total
            };
        }

    }
}