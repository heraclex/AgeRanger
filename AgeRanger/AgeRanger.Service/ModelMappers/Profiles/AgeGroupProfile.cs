using AgeRanger.DbContext.Entities;
using AgeRanger.Service.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Service.ModelMappers.Profiles
{
    public class AgeGroupProfile : AutoMapper.Profile
    {
        public AgeGroupProfile()
        {
            CreateMap<AgeGroupModel, AgeGroup>();
            CreateMap<AgeGroup, AgeGroupModel>();
        }
    }
}
