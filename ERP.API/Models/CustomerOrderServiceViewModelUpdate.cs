using ERP.Data.ModelsERP.ModelView.Customer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERP.API.Models
{
    public class CustomerOrderServiceViewModelUpdate
    {
        [Key]
        public int cuo_id { get; set; }

        public int[] list_service_id { get; set; }
        public int[] list_staff_id { get; set; }

        public customeraddressviewmodel customer { get; set; }
        public TimeSpan? st_start_time { get; set; }

        public TimeSpan? st_end_time { get; set; }

        [Column(TypeName = "date")]
        public DateTime? st_start_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? st_end_date { get; set; }

        public byte? st_repeat_type { get; set; }

        public bool? st_sun_flag { get; set; }

        public bool? st_mon_flag { get; set; }

        public bool? st_tue_flag { get; set; }

        public bool? st_wed_flag { get; set; }

        public bool? st_thu_flag { get; set; }

        public bool? st_fri_flag { get; set; }

        public bool? st_sat_flag { get; set; }

        public bool? st_repeat { get; set; }

        public bool? st_repeat_every { get; set; }

        public byte? st_on_the { get; set; }

        public bool? st_on_day_flag { get; set; }

        public int? st_on_day { get; set; }


        public byte? cuo_evaluation { get; set; }

        public string cuo_feedback { get; set; }

    }
}