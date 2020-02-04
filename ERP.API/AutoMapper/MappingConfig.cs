using AutoMapper;
using ERP.API.Models;
using ERP.Data.ModelsERP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.API.AutoMapper
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMappingFromEntitiesToViewModels();
            CreateMappingFromViewModelsToEntities();

        }

        private void CreateMappingFromViewModelsToEntities()
        {
            CreateMap<StaffCreateViewModel, staff>();
            CreateMap<StaffUpdateViewModel, staff>(); 

            CreateMap<CustomerCreateViewModel, customer>();
            CreateMap<CustomerUpdateViewModel, customer>();

            CreateMap<ProductCreateViewModel, product>();
            CreateMap<ProductUpdateViewModel, product>();

            CreateMap<ServiceCreateViewModel, service>();
            CreateMap<ServiceUpdateViewModel, service>();

            CreateMap<GroupRoleCreateViewModel, group_role>();
            CreateMap<GroupRoleUpdateViewModel, group_role>();

            CreateMap<DepartmentCreateViewModel, department>();
            CreateMap<DepartmentUpdateViewModel, department>();

            CreateMap<PositionCreateViewModel, position>();
            CreateMap<PositionUpdateViewModel, position>();

            CreateMap<NotificationUpdateViewModel, notification>();
            CreateMap<NotificationCreateViewModel, notification>();

            CreateMap<SmsCreateViewModel, sms>();
            CreateMap<SmsUpdateViewModel, sms>();

            CreateMap<SmsTemplateCreateViewModel, sms_template>();
            CreateMap<SmsTemplateUpdateViewModel, sms_template>();

            CreateMap<SmsStrategyCreateViewModel, sms_strategy>();
            CreateMap<SmsStrategyUpdateViewModel, sms_strategy>();

            CreateMap<EmailCreateViewModel, email>();
            CreateMap<EmailUpdateViewModel, email>();

            CreateMap<EmailTemplateCreateViewModel, email_template>();
            CreateMap<EmailTemplateUpdateViewModel, email_template>();

            CreateMap<EmailStrategyCreateViewModel, email_strategy>();
            CreateMap<EmailStrategyUpdateViewModel, email_strategy>();
        }

        private void CreateMappingFromEntitiesToViewModels()
        {
            // YOUR code
        }
    }
}