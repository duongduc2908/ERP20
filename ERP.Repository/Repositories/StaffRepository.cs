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
using ERP.Data.ModelsERP.ModelView.Staff;
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
        public PagedResults<staffviewmodel> GetAllPageSearch(int pageNumber, int pageSize, int? status, DateTime? start_date, DateTime? end_date, string name,int? sta_working_status)
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
                end_date = end_date.Value.AddDays(1);
                list_res = list_res.Where(x => x.sta_created_date <= end_date).ToList();
            }
            if (name != null)
            {
                list_res = list_res.Where(t => t.sta_fullname.Contains(name) || t.sta_mobile.Contains(name) || t.sta_code.Contains(name) ).ToList();
            }
            if(sta_working_status != null)
            {
                list_res = list_res.Where(t => t.sta_working_status == sta_working_status).ToList();

            }
            var total = list_res.Count();
            list = list_res.OrderByDescending(t => t.sta_id).Skip(skipAmount).Take(pageSize).ToList();
            
            

            var results = list.ToList();
            foreach (staff i in results)
            {
                staffviewmodel staffview = _mapper.Map<staffviewmodel>(i);
                department de = _dbContext.departments.Where(x => x.de_id == i.department_id).FirstOrDefault();
                position po = _dbContext.positions.Where(x => x.pos_id == i.position_id).FirstOrDefault();
                group_role gr = _dbContext.group_role.Where(x => x.gr_id == i.group_role_id).FirstOrDefault();
                if(de != null)
                {
                    staffview.department_name = de.de_name;
                }
                if(po != null)
                {
                    staffview.position_name = po.pos_name;
                }
                if(gr != null)
                {
                    staffview.group_role_name = gr.gr_name;
                }
               
                
                for (int j = 1; j < EnumsStaff.sta_status.Length + 1; j++)
                {
                    if (j == staffview.sta_status)
                    {
                        staffview.status_name = EnumsStaff.sta_status[j - 1];
                    }
                }
                for (int j = 1; j < EnumsStaff.sta_sex.Length + 1; j++)
                {
                    if (j == staffview.sta_sex)
                    {
                        staffview.sta_sex_name = EnumsStaff.sta_sex[j - 1];
                    }
                }
                for (int j = 1; j < EnumsStaff.sta_working_status_name.Length + 1; j++)
                {
                    if (j == staffview.sta_type_contact)
                    {
                        staffview.sta_working_status_name = EnumsStaff.sta_working_status_name[j - 1];
                    }
                }
                //lấy ra thông tin loại hợp đồng 
                
                //var list_swt = _dbContext.staff_work_times.Where(s => s.staff_id == i.sta_id).ToList();
                
                //staffview.list_staff_work_time = list_swt;


                //Lấy ra địa chỉ thường chú 
                undertaken_location address_permanent = _dbContext.undertaken_location.Where(x => x.staff_id == staffview.sta_id && x.unl_flag_center == 1).FirstOrDefault();
                if(address_permanent != null)
                {
                    staffview.unl_id_permanent = address_permanent.unl_id;
                    staffview.unl_ward_permanent = address_permanent.unl_ward;
                    staffview.unl_province_permanent = address_permanent.unl_province;
                    staffview.unl_district_permanent = address_permanent.unl_district;
                    staffview.unl_geocoding_permanent = address_permanent.unl_geocoding;
                    staffview.unl_detail_permanent = address_permanent.unl_detail;
                    staffview.unl_note_permanent = address_permanent.unl_note;
                }
                
                //Lấy ra address hiện tại 
                undertaken_location address_now = _dbContext.undertaken_location.Where(x => x.staff_id == staffview.sta_id && x.unl_flag_center == 2).FirstOrDefault();
                if(address_now != null)
                {
                    staffview.unl_id_now= address_now.unl_id;
                    staffview.unl_ward_now = address_now.unl_ward;
                    staffview.unl_province_now = address_now.unl_province;
                    staffview.unl_district_now = address_now.unl_district;
                    staffview.unl_geocoding_now = address_now.unl_geocoding;
                    staffview.unl_detail_now = address_now.unl_detail;
                    staffview.unl_note_now = address_now.unl_note;
                }
               
                //Lấy ra danh sách training
                List<stafftraningviewmodel> res_training = new List<stafftraningviewmodel>();
                var lts_cg = (from ex in _dbContext.training_staffs
                              join od in _dbContext.trainings on ex.training_id equals od.tn_id
                              where ex.staff_id == staffview.sta_id
                              select new
                              {
                                  od.tn_id, od.tn_name,od.tn_purpose,od.tn_start_date,od.tn_end_date,od.tn_content,od.tn_code,ex.ts_evaluate
                              }).ToList();
                if(lts_cg != null)
                {
                    for (int j = 0; j < lts_cg.Count; j++)
                    {
                        stafftraningviewmodel tr = new stafftraningviewmodel();
                        tr.tn_id = lts_cg[j].tn_id;
                        tr.tn_name = lts_cg[j].tn_name;
                        tr.tn_purpose = lts_cg[j].tn_purpose;
                        tr.tn_start_date = lts_cg[j].tn_start_date;
                        tr.tn_end_date = lts_cg[j].tn_end_date;
                        tr.tn_content = lts_cg[j].tn_content;
                        tr.tn_code = lts_cg[j].tn_code;
                        tr.ts_evaluate = lts_cg[j].ts_evaluate;
                        if(tr.ts_evaluate != null) tr.ts_evaluate_name = EnumTraningStaff.ts_evaluate[Convert.ToInt32(tr.ts_evaluate) - 1];

                        res_training.Add(tr);
                    }
                    staffview.list_training = res_training;
                }
                
                //Lay dia chi ship
                var list_address = _dbContext.undertaken_location.Where(s => s.staff_id == i.sta_id && s.unl_flag_center == 0).ToList();
                List<undertakenlocationviewmodel> lst_add = new List<undertakenlocationviewmodel>();
                foreach (undertaken_location s in list_address)
                {
                    undertakenlocationviewmodel  add = _mapper.Map<undertakenlocationviewmodel>(s);
                    add.ward_id = _dbContext.ward.Where(t => t.Name.Contains(s.unl_ward)).FirstOrDefault().Id;
                    add.district_id = _dbContext.district.Where(t => t.Name.Contains(s.unl_district)).FirstOrDefault().Id;
                    add.province_id = _dbContext.province.Where(t => t.Name.Contains(s.unl_province)).FirstOrDefault().Id;
                    lst_add.Add(add);
                }
                staffview.list_undertaken_location = lst_add;

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
            staff i = _dbContext.staffs.Find(id);
            staffviewmodel staffview = _mapper.Map<staffviewmodel>(i);
            department de = _dbContext.departments.Where(x => x.de_id == i.department_id).FirstOrDefault();
            position po = _dbContext.positions.Where(x => x.pos_id == i.position_id).FirstOrDefault();
            group_role gr = _dbContext.group_role.Where(x => x.gr_id == i.group_role_id).FirstOrDefault();
            if (de != null)
            {
                staffview.department_name = de.de_name;
            }
            if (po != null)
            {
                staffview.position_name = po.pos_name;
            }
            if (gr != null)
            {
                staffview.group_role_name = gr.gr_name;
            }
            for (int j = 1; j < EnumsStaff.sta_status.Length + 1; j++)
            {
                if (j == staffview.sta_status)
                {
                    staffview.status_name = EnumsStaff.sta_status[j - 1];
                }
            }
            for (int j = 1; j < EnumsStaff.sta_sex.Length + 1; j++)
            {
                if (j == staffview.sta_sex)
                {
                    staffview.sta_sex_name = EnumsStaff.sta_sex[j - 1];
                }
            }
            for (int j = 1; j < EnumsStaff.sta_working_status_name.Length + 1; j++)
            {
                if (j == staffview.sta_type_contact)
                {
                    staffview.sta_working_status_name = EnumsStaff.sta_working_status_name[j - 1];
                }
            }
            //lấy ra thông tin loại hợp đồng 
            var list_swt = _dbContext.staff_work_times.Where(s => s.staff_id == i.sta_id).ToList();

            staffview.list_staff_work_time = list_swt;

            //Lấy ra địa chỉ thường chú 
            undertaken_location address_permanent = _dbContext.undertaken_location.Where(x => x.staff_id == staffview.sta_id && x.unl_flag_center == 1).FirstOrDefault();
            if (address_permanent != null)
            {
                staffview.unl_id_permanent = address_permanent.unl_id;
                staffview.unl_ward_permanent = address_permanent.unl_ward;
                staffview.unl_province_permanent = address_permanent.unl_province;
                staffview.unl_district_permanent = address_permanent.unl_district;
                staffview.unl_geocoding_permanent = address_permanent.unl_geocoding;
                staffview.unl_detail_permanent = address_permanent.unl_detail;
                staffview.unl_note_permanent = address_permanent.unl_note;
            }

            //Lấy ra address hiện tại 
            undertaken_location address_now = _dbContext.undertaken_location.Where(x => x.staff_id == staffview.sta_id && x.unl_flag_center == 2).FirstOrDefault();
            if (address_now != null)
            {
                staffview.unl_id_now = address_now.unl_id;
                staffview.unl_ward_now = address_now.unl_ward;
                staffview.unl_province_now = address_now.unl_province;
                staffview.unl_district_now = address_now.unl_district;
                staffview.unl_geocoding_now = address_now.unl_geocoding;
                staffview.unl_detail_now = address_now.unl_detail;
                staffview.unl_note_now = address_now.unl_note;
            }

            //Lấy ra danh sách training
            List<stafftraningviewmodel> res_training = new List<stafftraningviewmodel>();
            var lts_cg = (from ex in _dbContext.training_staffs
                          join od in _dbContext.trainings on ex.training_id equals od.tn_id
                          where ex.staff_id == staffview.sta_id
                          select new
                          {
                              od.tn_id,
                              od.tn_name,
                              od.tn_purpose,
                              od.tn_start_date,
                              od.tn_end_date,
                              od.tn_content,
                              od.tn_code,
                              ex.ts_evaluate
                          }).ToList();
            if (lts_cg != null)
            {
                for (int j = 0; j < lts_cg.Count; j++)
                {
                    stafftraningviewmodel tr = new stafftraningviewmodel();
                    tr.tn_id = lts_cg[j].tn_id;
                    tr.tn_name = lts_cg[j].tn_name;
                    tr.tn_purpose = lts_cg[j].tn_purpose;
                    tr.tn_start_date = lts_cg[j].tn_start_date;
                    tr.tn_end_date = lts_cg[j].tn_end_date;
                    tr.tn_content = lts_cg[j].tn_content;
                    tr.tn_code = lts_cg[j].tn_code;
                    tr.ts_evaluate = lts_cg[j].ts_evaluate;
                    res_training.Add(tr);
                }
                staffview.list_training = res_training;
            }

            //Lay dia chi ship
            var list_address = _dbContext.undertaken_location.Where(s => s.staff_id == i.sta_id && s.unl_flag_center == 0).ToList();
            List<undertakenlocationviewmodel> lst_add = new List<undertakenlocationviewmodel>();
            foreach (undertaken_location s in list_address)
            {
                undertakenlocationviewmodel add = _mapper.Map<undertakenlocationviewmodel>(s);
                add.ward_id = _dbContext.ward.Where(t => t.Name.Contains(s.unl_ward)).FirstOrDefault().Id;
                add.district_id = _dbContext.district.Where(t => t.Name.Contains(s.unl_district)).FirstOrDefault().Id;
                add.province_id = _dbContext.province.Where(t => t.Name.Contains(s.unl_province)).FirstOrDefault().Id;
                lst_add.Add(add);
            }
            staffview.list_undertaken_location = lst_add;
            return staffview;
            
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
        public PagedResults<staffview> ExportStaff(int pageNumber, int pageSize, int? status, DateTime? start_date, DateTime? end_date, string name, int? sta_working_status)
        {
            if (name != null)
            {
                name = name.Trim();
            }
            List<staff> list_res;
            List<staff> list;
            List<staffview> res = new List<staffview>();
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
                end_date = end_date.Value.AddDays(1);
                list_res = list_res.Where(x => x.sta_created_date <= end_date).ToList();
            }
            if (name != null)
            {
                list_res = list_res.Where(t => t.sta_fullname.Contains(name) || t.sta_mobile.Contains(name) || t.sta_email.Contains(name) || t.sta_code.Contains(name) || t.sta_username.Contains(name)).ToList();
            }
            if (sta_working_status != null)
            {
                list_res = list_res.Where(t => t.sta_working_status == sta_working_status).ToList();

            }
            var total = list_res.Count();
            list = list_res.OrderByDescending(t => t.sta_id).Skip(skipAmount).Take(pageSize).ToList();



            var results = list.ToList();
            foreach (staff i in results)
            {
                staffview staffview = _mapper.Map<staffview>(i);
                department de = _dbContext.departments.Where(x => x.de_id == i.department_id).FirstOrDefault();
                position po = _dbContext.positions.Where(x => x.pos_id == i.position_id).FirstOrDefault();
                group_role gr = _dbContext.group_role.Where(x => x.gr_id == i.group_role_id).FirstOrDefault();
                if (de != null)
                {
                    staffview.department_name = de.de_name;
                }
                if (po != null)
                {
                    staffview.position_name = po.pos_name;
                }
                if (gr != null)
                {
                    staffview.group_role_name = gr.gr_name;
                }


                for (int j = 1; j < EnumsStaff.sta_status.Length + 1; j++)
                {
                    if (j == staffview.sta_status)
                    {
                        staffview.status_name = EnumsStaff.sta_status[j - 1];
                    }
                }
                for (int j = 1; j < EnumsStaff.sta_sex.Length + 1; j++)
                {
                    if (j == staffview.sta_sex)
                    {
                        staffview.sta_sex_name = EnumsStaff.sta_sex[j - 1];
                    }
                }
                


                //Lấy ra địa chỉ thường chú 
               var address_permanent = _dbContext.undertaken_location.Where(x => x.staff_id == staffview.sta_id && x.unl_flag_center == 1).FirstOrDefault();
                if (address_permanent != null) staffview.sta_address_permanent = String.Concat(address_permanent.unl_province, "-", address_permanent.unl_district, "-", address_permanent.unl_ward, "-", address_permanent.unl_detail);
                

                //Lấy ra address hiện tại 
                var address_now = _dbContext.undertaken_location.Where(x => x.staff_id == staffview.sta_id && x.unl_flag_center == 2).FirstOrDefault();
                if (address_now != null) staffview.sta_address_now = String.Concat(address_now.unl_province, "-", address_now.unl_district, "-", address_now.unl_ward, "-", address_now.unl_detail);
                if (i.sta_type_contact == 0) staffview.sta_type_contact_name = "Fulltime";
                if (i.sta_type_contact == 1) staffview.sta_type_contact_name = "Partime";
                res.Add(staffview);
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
        public bool Check_location(undertaken_location un)
        {
            if (un.unl_province != null)
            {
                if (_dbContext.province.FirstOrDefault(x => x.Name.Equals(un.unl_province)) == null)
                    return false;
            }
            if (un.unl_district != null)
            {
                if (_dbContext.district.FirstOrDefault(x => x.Name.Equals(un.unl_district)) == null)
                    return false;
            }
            if (un.unl_ward != null)
            {
                if (_dbContext.ward.FirstOrDefault(x => x.Name.Equals(un.unl_ward)) == null)
                    return false;
            }
            return true;
        }
    }
}