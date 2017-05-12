using AgeRanger.DbContext.Entities;
using AgeRanger.Service.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.Service.Contract
{
    public interface IService : IDisposable
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
        IEnumerable<PersonModel> GetPeople(string filterCriteria);
    }
}
