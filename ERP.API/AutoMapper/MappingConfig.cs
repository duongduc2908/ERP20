using AutoMapper;
using ERP.API.Models;
using ERP.Data.ModelsERP;
using ERP.Data.ModelsERP.ModelView;
using ERP.Data.ModelsERP.ModelView.Customer;
using ERP.Data.ModelsERP.ModelView.Excutor;
using ERP.Data.ModelsERP.ModelView.ExportDB;
using ERP.Data.ModelsERP.ModelView.OrderService;
using ERP.Data.ModelsERP.ModelView.Product;
using ERP.Data.ModelsERP.ModelView.Service;
using ERP.Data.ModelsERP.ModelView.Sms;
using ERP.Data.ModelsERP.ModelView.Statistics;
using ERP.Data.ModelsERP.ModelView.StatisticStaff;
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
            CreateMap< staff, statisticstaffviewmodel>();
            CreateMap< staff, staffview>();
            CreateMap< staff, servicestaffviewmodel>();
            

            CreateMap<CustomerCreateViewModel, customer>();
            CreateMap<CustomerUpdateViewModel, customer>();
            CreateMap<customer, customerviewmodel>();
            CreateMap<customer, customerviewexport>();
            CreateMap<customer, customeraddressviewmodel>();
            CreateMap<customer, transactioncustomerviewmodel>();
            CreateMap<customer, smscustomerviewmodel>();
            CreateMap<customer, servicesearchcustomerviewmodel>();

            CreateMap<CustomerOrderCreateViewModel, customer_order>();
            CreateMap<CustomerOrderUpdateViewModel, customer_order>();

            CreateMap<customer_order, customerorderviewmodel>();
            CreateMap<customer_order, customerorderhistoryviewmodel>();
            CreateMap<customer_order, statisticsorderviewmodel>();
            CreateMap<customer_order, customerorderview>();
            CreateMap<customer_order, servicercustomerorderviewmodel>();

            CreateMap<customer_phone, customer_phoneviewmodel>();

            CreateMap<service_time, servicercustomerorderviewmodel>();
            CreateMap<service_time, serviceinforviewmodel>();
            CreateMap<ServiceTimeCreateViewModel, service_time>();
            CreateMap<ServiceTimeUpdateViewModel, service_time>();

            CreateMap<customer_group, customergroupviewmodel>();
            CreateMap<CustomerGroupCreateViewModel, customer_group>();
            CreateMap<CustomerGroupUpdateViewModel, customer_group>();
           

            CreateMap<ProductCreateViewModel, product>();
            CreateMap<ProductUpdateViewModel, product>();
            CreateMap<product, productviewmodel>();
            CreateMap<product, productview>();

            CreateMap<ShipAddressCreateViewModel, ship_address>();
            CreateMap<ShipAddressUpdateViewModel, ship_address>();
            CreateMap<ship_address, shipaddressviewmodel>();
            CreateMap<shipaddressviewmodel, ship_address>();

            CreateMap<UndertakenLocationCreateViewModel, undertaken_location>();
            CreateMap<UndertakenLocationUpdateViewModel, undertaken_location>();
            CreateMap<undertaken_location, undertakenlocationviewmodel>();

            CreateMap<OrderProductCreateViewModel, order_product>();
            CreateMap<OrderProductUpdateViewModel, order_product>();
            CreateMap<order_product,productorderviewmodel >();
            CreateMap<order_product,orderproducthistoryviewmodel >();
            CreateMap<order_product,transactionorderproductviewmodel >();

            CreateMap<OrderServiceCreateViewModel, order_service>();
            CreateMap<OrderServiceUpdateViewModel, order_service>();

            CreateMap<ExecutorCreateViewModel, executor>();
            CreateMap<ExecutorUpdateViewModel, executor>();
            CreateMap<executor, orderservice_day>();
            CreateMap<executor, executorviewmodel>();


            CreateMap<ServiceCreateViewModel, service>();
            CreateMap<ServiceUpdateViewModel, service>();
            CreateMap<service, serviceviewmodel>();
            CreateMap<service, serviceinforviewmodel>();

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
            CreateMap<transaction, transactionview>();
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