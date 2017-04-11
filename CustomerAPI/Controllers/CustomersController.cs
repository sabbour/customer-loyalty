using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CustomerAPI.Models;
using Newtonsoft.Json;
using System.Web.Hosting;

namespace CustomerAPI.Controllers
{
    public class CustomersController : ApiController
    {
        IList<Customer> customers;
        public CustomersController()
        {
            var customersJson = System.IO.File.ReadAllText(HostingEnvironment.MapPath(@"~/App_Data/customers.json"));
            customers = JsonConvert.DeserializeObject<IList<Customer>>(customersJson);
        }

        /// <summary>
        /// Returns all customers
        /// </summary>
        /// <returns></returns>
        [Route("customers")]
        public IList<Customer> Get()
        {
            return customers;
        }

        /// <summary>
        /// Returns a customer by searching for their phone number
        /// </summary>
        /// <param name="phonenumber"></param>
        /// <returns></returns>
        [Route("customers/{phonenumber}")]
        public Customer Get(string phonenumber)
        {
            var customer = customers.Where(c => c.PhoneNumber == phonenumber).FirstOrDefault();
            return customer;
        }
    }
}
