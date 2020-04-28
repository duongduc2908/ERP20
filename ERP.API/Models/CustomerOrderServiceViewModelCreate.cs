
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Customer;
using ERP.Data.ModelsERP.ModelView.CustomerOrder;
using ERP.Data.ModelsERP.ModelView.Excutor;
using ERP.Data.ModelsERP.ModelView.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.API.Models
{
    public class CustomerOrderServiceViewModelCreate
    {
        //Thong in order 
        public string cuo_address { get; set; }
        public string cuo_infor_time { get; set; }
        public string cuo_color_show {get;set;}
        public int cuo_discount { get; set; }

        //Thông tin khách hàng và thông tin địa chỉ 
        public CustomerUpdateViewModelJson customer { get; set; }
        //Thông tin dịch vụ
        public List<servicejson> list_service { get; set; }
        //Thông tin ngày làm việc
        public List<executorjson> list_executor { get; set; }
        public TimeSpan st_start_time { get; set; }

        public TimeSpan st_end_time { get; set; }

        [Column(TypeName = "date")]
        public DateTime st_start_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime st_end_date { get; set; }

        public byte st_repeat_type { get; set; }

        public bool st_sun_flag { get; set; }

        public bool st_mon_flag { get; set; }

        public bool st_tue_flag { get; set; }

        public bool st_wed_flag { get; set; }

        public bool st_thu_flag { get; set; }

        public bool st_fri_flag { get; set; }

        public bool st_sat_flag { get; set; }

        public bool st_repeat { get; set; }

        public int st_repeat_every { get; set; }

        public bool st_on_the_flag { get; set; }

        public int st_on_the { get; set; }

        public bool st_on_day_flag { get; set; }

        public int st_on_day { get; set; }
        //public int customer_order_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime st_custom_start { get; set; }

        [Column(TypeName = "date")]
        public DateTime st_custom_end { get; set; }
    }
}