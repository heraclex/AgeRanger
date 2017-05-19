using AgeRanger.DbContext.Entities;
using AgeRanger.Service.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeRanger.UnitTest.Helper
{
    public class TestingDataDource
    {
        public static PersonModel InvalidNewPersonModel
        {
            get
            {
                return new PersonModel() { Id = 0, FirstName = string.Empty, LastName = string.Empty, Age = -1 };
            }
        }

        public static PersonModel ValidNewPersonModel
        {
            get
            {
                return new PersonModel() { Id = 0, FirstName = "World", LastName = "Hello", Age = 999999 };
            }
        }

        public static PersonModel ExistingPersonModel
        {
            get
            {
                return PersonModelList.First();
            }
        }

        public static List<PersonModel> PersonModelList
        {
            get
            {
                return PersonList.Select(x => new PersonModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age,
                    AgeGroup = AgeGroups.Find(g=> x.Age >= g.MinAge.Value
                    && ((!g.MaxAge.HasValue) || (g.MaxAge.HasValue && g.MaxAge.Value >= x.Age))).Description
                }).ToList();
            }
        }

        public static List<Person> PersonList
        {
            get
            {
                return new List<Person>()
                {
                    new Person() { Id = 1, FirstName = "David", LastName = "Cantona", Age = 29 },
                    new Person() { Id = 2, FirstName = "Eric", LastName = "Lee", Age = 139},
                    new Person() { Id = 3, FirstName = "Cantona", LastName = "David", Age = 139},
                    new Person() { Id = 4, FirstName = "Toan", LastName = "Le", Age = 999}
                };
            }
        }

        public static List<AgeGroup> AgeGroups
        {
            get
            {
                return new List<AgeGroup>
                {
                    new AgeGroup() { Id = 1, MinAge = 0  , MaxAge = 2, Description = "Toddler" },
                    new AgeGroup() { Id = 2, MinAge =  2    , MaxAge = 14, Description = "Child" },
                    new AgeGroup() { Id = 3, MinAge =  14   , MaxAge = 20, Description = "Teenager" },
                    new AgeGroup() { Id = 4, MinAge =  20   , MaxAge = 25, Description = "Early twenties" },
                    new AgeGroup() { Id = 5, MinAge =  25   , MaxAge = 30, Description = "Almost thirty" },
                    new AgeGroup() { Id = 6, MinAge =  30   , MaxAge = 50, Description = "Very adult" },
                    new AgeGroup() { Id = 7, MinAge =  50   , MaxAge = 70, Description = "Kinda old" },
                    new AgeGroup() { Id = 8, MinAge =  70   , MaxAge = 99, Description = "Old" },
                    new AgeGroup() { Id = 9, MinAge =  99   , MaxAge = 110, Description = "Very old" },
                    new AgeGroup() { Id = 10, MinAge = 110  , MaxAge = 199, Description = "Crazy ancient" },
                    new AgeGroup() { Id = 11, MinAge = 199  , MaxAge = 4999, Description = "Vampire" },
                    new AgeGroup() { Id = 12, MinAge = 4999 , MaxAge = null, Description = "Kauri tree" }
                };
            }
        }
                
    }
}
