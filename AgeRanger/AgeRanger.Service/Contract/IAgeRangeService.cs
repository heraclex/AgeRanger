using AgeRanger.DbContext.Entities;
using AgeRanger.Service.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Service.Contract
{
    /// <summary>
    /// Represents for Service contract
    /// </summary>
    public interface IAgeRangeService : IDisposable
    {
        /// <summary>
        /// Add/Edit a person
        /// </summary>
        /// <param name="model"></param>
        /// <returns>return true if success</returns>
        PersonModel SavePerson(PersonModel model);

        /// <summary>
        /// Allow filtering people base on first/last name
        /// </summary>
        /// <returns></returns>
        IEnumerable<PersonModel> FindPeople(string filterCriteria);

        /// <summary>
        /// Get Person By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Person Model</returns>
        PersonModel GetPersonById(long id);

        /// <summary>
        /// Delete Person By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if success</returns>
        void DeletePersonById(long id);

        /// <summary>
        /// Verify Person by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsPersonExisted(long id);
    }
}
