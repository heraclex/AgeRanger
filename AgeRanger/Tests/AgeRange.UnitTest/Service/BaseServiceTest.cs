using AgeRange.UnitTest.Helper;
using AgeRanger.DbContext.Entities;
using AgeRanger.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AgeRange.UnitTest.Service
{
    [TestClass]
    public class BaseServiceTest
    {
        protected Mock<IRepository<Person>> mockPersonRepository;
        protected Mock<IRepository<AgeGroup>> mockAgeGroupRepository;

        [TestInitialize]
        public virtual void Setup()
        {
            this.mockPersonRepository = new Mock<IRepository<Person>>();
            this.mockAgeGroupRepository = new Mock<IRepository<AgeGroup>>();

            this.AgeGroupRepoSetup();
            this.PersonRepoSetup();
        }

        private void AgeGroupRepoSetup()
        {
            this.mockAgeGroupRepository.Setup(m => m.Query(It.IsAny<Expression<Func<AgeGroup, bool>>>())).
                Returns((Expression<Func<AgeGroup, bool>> @where) =>
                {
                    return TestingDataDource.AgeGroups.AsQueryable().Where(@where);
                });
        }

        private void PersonRepoSetup()
        {
            this.mockPersonRepository.Setup(m => m.GetAll())
                .Returns(() => { return TestingDataDource.PersonList; });

            this.mockPersonRepository.Setup(m => m.List(It.IsAny<Expression<Func<Person, bool>>>()))
                .Returns((Expression<Func<Person, bool>> @where) => {
                    return TestingDataDource.PersonList.AsQueryable().Where(@where).ToList();
                });

            this.mockPersonRepository.Setup(m => m.SaveOrUpdate(It.IsAny<Person>()))
               .Returns((Person entity) => {
                   if (entity.Id == 0)
                       entity.Id = 999;
                   return entity;
               });

            this.mockPersonRepository.Setup(m => m.ForceSaveOrUpdateImmediately(It.IsAny<Person>()))
               .Returns((Person entity) => {
                   if (entity.Id == 0)
                       entity.Id = 999;
                   return entity;
               });

        }
    }
}
