using System.Net;
using Horizon.OData.Attributes;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.OData
{
    public class Controller : ODataController, IController
    {
        [StatusCode(HttpStatusCode.OK)]
        public override OkResult Ok()
        {
            return base.Ok();
        }

        [StatusCode(HttpStatusCode.OK)]
        public override OkObjectResult Ok(object value)
        {
            return base.Ok(value);
        }
    }
}