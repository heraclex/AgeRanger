using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgeRanger.Service.Contract;
using AgeRanger.Service.Contract.Models;
using AgeRanger.Repository;
using AgeRanger.DbContext.Entities;

namespace AgeRanger.Service.Implementation
{
    public class Service : IService
    {

        private readonly IRepository<AgeGroup> ageGroupRepo;
        private readonly IRepository<Person> personRepo;

        public Service(IRepository<AgeGroup> ageGroupRepo, IRepository<Person> personRepo)
        {
            this.ageGroupRepo = ageGroupRepo;
            this.personRepo = personRepo;

            this.ConfigAutoMapper();
        }

        /// <summary>
        /// Add/Edit a person
        /// </summary>
        /// <param name="model"></param>
        /// <returns>return true if success</returns>
        public PersonModel SavePerson(PersonModel model)
        {
            var personAdded = this.personRepo.SaveOrUpdate(model.MapTo<Person>());

            return personAdded.MapTo<PersonModel>();
        }

        /// <summary>
        /// Allow filtering people base on first/last name
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PersonModel> GetPeople(string filterCriteria)
        {
            //return (new List<Person>() { new Person()}).MapTo<PersonModel>();

            ICollection<Person> results;
            if (!string.IsNullOrEmpty(filterCriteria))
            {
                results = this.personRepo.List(x => x.FirstName.Contains(filterCriteria) || x.LastName.Contains(filterCriteria));
            }
            else
            {
                results = this.personRepo.GetAll();
            }

            return results.MapTo<PersonModel>();
        }
        
        public void Dispose()
        {
            this.personRepo.Dispose();
            this.ageGroupRepo.Dispose();
        }

        #region private methods
        private void ConfigAutoMapper()
        {
            var profiles = this.GetType().Assembly.GetTypes().Where(x => typeof(AutoMapper.Profile).IsAssignableFrom(x));
            foreach (var profile in profiles)
            {
                // Register Profiles for AutoMapper
                AutoMapper.Mapper.Initialize(x => x.AddProfile(Activator.CreateInstance(profile) as AutoMapper.Profile));
            }
        }
        
        #endregion
    }
}

