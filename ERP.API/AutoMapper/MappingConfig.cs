using AutoMapper;
using ERP.API.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Customer;
using ERP.Data.ModelsERP.ModelView.Product;
using ERP.Data.ModelsERP.ModelView.Sms;
using ERP.Data.ModelsERP.ModelView.Transaction;
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
            CreateMap< staff, staffviewmodel>();
            

            CreateMap<CustomerCreateViewModel, customer>();
            CreateMap<CustomerUpdateViewModel, customer>();
            CreateMap<customer, customerviewmodel>();
            CreateMap<customer, customeraddressviewmodel>();
            CreateMap<customer, transactioncustomerviewmodel>();
            CreateMap<customer, smscustomerviewmodel>();

            CreateMap<CustomerOrderCreateViewModel, customer_order>();
            CreateMap<CustomerOrderUpdateViewModel, customer_order>();

            CreateMap<customer_order, customerorderviewmodel>();
            CreateMap<customer_order, customerorderhistoryviewmodel>();
            
            CreateMap<customer_group, customergroupviewmodel>();
           

            CreateMap<ProductCreateViewModel, product>();
            CreateMap<ProductUpdateViewModel, product>();
            CreateMap<product, productviewmodel>();

            CreateMap<ShipAddressCreateViewModel, ship_address>();
            CreateMap<ShipAddressUpdateViewModel, ship_address>();
            CreateMap<ship_address, shipaddressviewmodel>();

            CreateMap<UndertakenLocationCreateViewModel, undertaken_location>();
            CreateMap<UndertakenLocationUpdateViewModel, undertaken_location>();
            CreateMap<undertaken_location, undertakenlocationviewmodel>();

            CreateMap<OrderProductCreateViewModel, order_product>();
            CreateMap<OrderProductUpdateViewModel, order_product>();
            CreateMap<order_product,productorderviewmodel >();
            CreateMap<order_product,orderproducthistoryviewmodel >();
            CreateMap<order_product,transactionorderproductviewmodel >();
            

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
            CreateMap<sms_template, smstemplatemodelview>();
            CreateMap<sms_template, smstemplatestrategyviewmodel>();

            CreateMap<SmsStrategyCreateViewModel, sms_strategy>();
            CreateMap<SmsStrategyUpdateViewModel, sms_strategy>();
            CreateMap<sms_strategy, smsstrategyviewmodel>();

            CreateMap<EmailCreateViewModel, email>();
            CreateMap<EmailUpdateViewModel, email>();

            CreateMap<EmailTemplateCreateViewModel, email_template>();
            CreateMap<EmailTemplateUpdateViewModel, email_template>();

            CreateMap<EmailStrategyCreateViewModel, email_strategy>();
            CreateMap<EmailStrategyUpdateViewModel, email_strategy>();

            CreateMap<transaction, transactionviewmodel>();
            CreateMap<transaction, customertransactionviewmodel>();
            CreateMap<TransactionCreateViewModel, transaction>();
            CreateMap<TransactionUpdateViewModel, transaction>();
        }

        private void CreateMappingFromEntitiesToViewModels()
        {
            // YOUR code
        }
    }
}