using AgeRanger.DbContext.Entities;
using AgeRanger.Service.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Service.ModelMappers.Profiles
{
    public class PersonProfile : AutoMapper.Profile
    {
        public PersonProfile()
        {
            // Model to Entity
            CreateMap<PersonModel, Person>();

            // Entity to Model, ignore AgeGroup property
            CreateMap<Person, PersonModel>().ForMember(x => x.AgeGroup, opt => opt.Ignore());
        }
    }
}
