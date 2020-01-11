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

        }

        private void CreateMappingFromEntitiesToViewModels()
        {
            // YOUR code
        }
    }
}