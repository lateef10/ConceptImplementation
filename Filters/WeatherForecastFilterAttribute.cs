using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeConcept.API.Filters
{
    public class WeatherForecastFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            int? id;
            if (context.ActionArguments.ContainsKey("val"))
                id = (int?)context.ActionArguments["val"];
            else
            {
                context.Result = new BadRequestObjectResult("Bad val parameter");
                return;
            }

            if(id < 1 || id > 5 || id == null)
            {
                context.Result = new NotFoundResult();
            }

            base.OnActionExecuting(context);
        }
    }
}
