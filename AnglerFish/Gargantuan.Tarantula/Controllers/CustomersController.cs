using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Gargantuan.Tarantula.Models;

namespace Gargantuan.Tarantula.Controllers
{
    public class CustomersController : ApiController
    {
        [HttpGet]
        public Record[] Customers()
        {
            return new Record[]
            {
                new Record(name:"Alfreds Futterkiste",country:"Germany")
                , new Record(name:"Ana Trujillo Emparedados y helados",country:"Mexico")
            };
        }
    }
}
