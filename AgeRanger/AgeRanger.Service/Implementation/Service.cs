﻿using System;
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
    /// <summary>
    /// Represents for Service layer which is responsible for business logic
    /// </summary>
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
        public IEnumerable<PersonModel> FindPeople(string filterCriteria)
        {
            var personList = !string.IsNullOrEmpty(filterCriteria) ?
                this.personRepo.List(x => x.FirstName.Contains(filterCriteria) || x.LastName.Contains(filterCriteria))
                : this.personRepo.GetAll();

            // convert to model list 
            var result = personList.MapTo<PersonModel>();

            // retrieve age Group description
            // TODO: should be refactoring by joining both table by logic code and execute from db level. Avoifing 2 rounds hiting Database
            if (result.Any())
            {
                var ageGroups = this.GetAgeGroupsByAgeRange(personList.Min(p => p.Age).Value, personList.Max(p => p.Age).Value);
                foreach (var item in result)
                {
                    // Min Age is expected to has a value, ignore validation on minAge
                    item.AgeGroup = ageGroups.First(g => item.Age >= g.MinAge.Value
                    && ((!g.MaxAge.HasValue) || (g.MaxAge.HasValue && g.MaxAge.Value >= item.Age))).Description;
                }                
            }

            return result;
        }

        /// <summary>
        /// Get Person By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Person Model</returns>
        public PersonModel GetPersonById(long id)
        {
            var result = this.personRepo.Get(id).MapTo<PersonModel>();
            result.AgeGroup = this.GetAgeGroupsByAgeRange(result.Age.Value, result.Age.Value).First().Description;
            return result;
        }

        public bool IsPersonExisted(long id)
        {
            return this.personRepo.Query(x => x.Id == id).Any();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
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

        private IEnumerable<AgeGroup> GetAgeGroupsByAgeRange(long minAge, long maxAge)
        {
            return this.ageGroupRepo.Query(g => (!g.MaxAge.HasValue && g.MinAge.Value <= minAge)
            || (g.MaxAge.HasValue && g.MaxAge.Value >= minAge && g.MinAge.Value <= maxAge)).ToList();
        }

        #endregion
    }
}

