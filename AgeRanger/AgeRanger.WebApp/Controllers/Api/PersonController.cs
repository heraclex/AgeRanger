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

        public HttpResponseMessage Get(string name)
        {
            var result = this.service.GetPeople(name);

            // TODO: should be move to BaseController
            if (result == null || !result.Any())
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(result);
        }

        public HttpResponseMessage Post(PersonModel personModel)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, 
                    ModelState.Values.SelectMany(modelState => modelState.Errors));
            }

            var result = this.service.SavePerson(personModel);

            var response = Request.CreateResponse(HttpStatusCode.Created, result);
            //string uri = Url.Link("DefaultApi", new { id = product.Id });
            //response.Headers.Location = new Uri(uri);

            return response;
        }
    }
}
