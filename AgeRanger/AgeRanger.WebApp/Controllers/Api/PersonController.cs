using AgeRanger.DbContext.Entities;
using AgeRanger.Repository;
using AgeRanger.Service.Contract;
using AgeRanger.Service.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AgeRanger.WebApp.Controllers.Api
{
    public class PersonController : ApiController
    {

        private readonly IService service;

        public PersonController(IService service)
        {
            this.service = service;
        }

        // GET: api/Person/5
        public HttpResponseMessage Get(long personId)
        {
            var result = this.service.GetPersonById(personId);

            // TODO: should be move to BaseController
            if (result == null)
            {
                // TODO: Error message should be move to common class
                return Request.CreateResponse(HttpStatusCode.NotFound, "Person does not exist");
            }
            return Request.CreateResponse(result);
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage Filter(string name)
        {
            var result = this.service.FindPeople(name);

            // TODO: should be move to BaseController
            if (result == null || !result.Any())
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(result);
        }

        // POST: api/Person
        public HttpResponseMessage Post(PersonModel personModel)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable,
                    ModelState.Values.Select(modelState => modelState.Errors).ToList());
            }

            var personModelCreated = this.service.SavePerson(personModel);
            var response = Request.CreateResponse(HttpStatusCode.Created, personModelCreated);
            return response;
        }

        // PUT: api/Person/5
        public HttpResponseMessage Put(long id, [FromBody]PersonModel personModel)
        {
            if (!this.service.IsPersonExisted(id))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Person does not exist");
            }

            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable,
                    ModelState.Values.Select(modelState => modelState.Errors).ToList());
            }

            var personModelCreated = this.service.SavePerson(personModel);
            var response = Request.CreateResponse(HttpStatusCode.OK, personModelCreated);
            return response;
        }
    }
}
