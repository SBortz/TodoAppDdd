using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TodoAppDdd.Api.Filter
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
	{
		public override void OnException(ExceptionContext context)
		{
//			Log.Error(context.Exception, "ApiException occured: {@Exception}", context.Exception);

			var code = HttpStatusCode.InternalServerError;

			context.HttpContext.Response.ContentType = "application/json";
			context.HttpContext.Response.StatusCode = (int)code;

			var errorResult = new ErrorResult
			{
				Message = context.Exception.Message,
				ExceptionType = context.Exception.GetType().ToString(),
				StackTrace = context.Exception.StackTrace,
			};

			context.Result = new JsonResult(errorResult);
		}
	}
}
