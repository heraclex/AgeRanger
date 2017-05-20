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

        private readonly IAgeRangerService service;

        public PersonController(IAgeRangerService service)
        {
            this.service = service;
        }

        // GET: api/Person/5
        [HttpGet]
        [Route("api/person/{id}")]
        public HttpResponseMessage Get(long id)
        {
            var result = this.service.GetPersonById(id);

            // TODO: should be move to BaseController
            if (result == null)
            {
                // TODO: Error message should be move to common class
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(result);
        }

        // GET: api/Person/{filter}
        [HttpGet]
        [Route("api/person/filter/{name?}")]
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
                
        public HttpResponseMessage Post(PersonModel personModel)
        {
            // TODO: Refactor Works: This validation should be move to ValidationModel in Action Executing Filter
            // https://docs.microsoft.com/en-us/aspnet/web-api/overview/formats-and-model-binding/model-validation-in-aspnet-web-api
            // Model State does not check nullble
            if (personModel == null || !ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable,
                    ModelState.Values.Select(modelState => modelState.Errors).ToList());
            }

            var personModelCreated = this.service.SavePerson(personModel);
            var response = Request.CreateResponse(HttpStatusCode.Created, personModelCreated);
            return response;
        }

        // POST: api/person/update/{id}
        [Route("api/person/update/{id}")]
        [HttpPost]
        public HttpResponseMessage Update(long id, PersonModel personModel)
        {
            if (!this.service.IsPersonExisted(id))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            // Model State does not check nullble
            if (personModel == null || !ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable,
                    ModelState.Values.Select(modelState => modelState.Errors).ToList());
            }

            var personModelCreated = this.service.SavePerson(personModel);
            var response = Request.CreateResponse(HttpStatusCode.OK, personModelCreated);
            return response;
        }

        // DELTE: api/person/delete/{id}
        [Route("api/person/delete/{id}")]
        public HttpResponseMessage Delete(long id)
        {
            if (!this.service.IsPersonExisted(id))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            this.service.DeletePersonById(id);

            var response = Request.CreateResponse(HttpStatusCode.OK, id);
            return response;
        }
    }
}
