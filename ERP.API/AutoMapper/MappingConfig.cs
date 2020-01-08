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
            CreateMap<StaffCreateViewModel, staff>();

        }

        private void CreateMappingFromEntitiesToViewModels()
        {
            // YOUR code
        }
    }
}