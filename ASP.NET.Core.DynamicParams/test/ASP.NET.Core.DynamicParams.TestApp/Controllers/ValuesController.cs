using System.Collections.Generic;
using ASP.NET.Core.DynamicParams.Attributes;
using ASP.NET.Core.DynamicParams.Filter;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET.Core.DynamicParams.TestApp.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // POST api/values
        [HttpPost, DynamicParams, Route("extractPerson")]
        public IActionResult PostSingleObject([FromBody, ToDynamic] dynamic value)
        {
            Person p = value.User;
            return Ok(p);
        }

        [HttpPost, DynamicParams(ParamDetectionMode.Implicit), Route("extractPersonImplicit")]
        public IActionResult PostSingleObjectImplicit([FromBody] dynamic value)
        {
            Person p = value.User;
            return Ok(p);
        }

        [HttpPost, DynamicParams, Route("collection")]
        public IActionResult PostCollection([FromBody, ToDynamicList] List<dynamic> value)
        {
            Person p = value[0].User;
            return Ok(p);
        }

        [HttpPost, DynamicParams(ParamDetectionMode.Implicit), Route("collectionImplicit")]
        public IActionResult PostCollectionImplicit([FromBody] List<dynamic> value)
        {
            Person p = value[0].User;
            return Ok(p);
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}