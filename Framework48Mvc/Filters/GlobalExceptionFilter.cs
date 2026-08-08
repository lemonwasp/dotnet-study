using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using log4net;

namespace Framework48Mvc.Filters
{
    public class GlobalExceptionFilter : FilterAttribute, IExceptionFilter
    {
        private static readonly ILog _logger =
            LogManager.GetLogger(typeof(GlobalExceptionFilter));

        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null || filterContext.ExceptionHandled)
            {
                return;
            }

            var exception = filterContext.Exception;

            if (exception is ArgumentException)
            {
                HandleException(
                    filterContext,
                    exception,
                    HttpStatusCode.BadRequest,
                    exception.Message,
                    isUnexpected: false);

                return;
            }

            if (exception is KeyNotFoundException)
            {
                HandleException(
                    filterContext,
                    exception,
                    HttpStatusCode.NotFound,
                    exception.Message,
                    isUnexpected: false);

                return;
            }

            HandleException(
                filterContext,
                exception,
                HttpStatusCode.InternalServerError,
                "An unexpected error occurred.",
                isUnexpected: true);
        }

        private static void HandleException(
            ExceptionContext context,
            Exception exception,
            HttpStatusCode statusCode,
            string responseMessage,
            bool isUnexpected)
        {
            if (isUnexpected)
            {
                _logger.Error(responseMessage, exception);
            }
            else
            {
                _logger.Warn(exception.Message, exception);
            }

            context.HttpContext.Response.StatusCode = (int)statusCode;
            context.HttpContext.Response.TrySkipIisCustomErrors = true;

            context.Result = new JsonResult
            {
                Data = new
                {
                    success = false,
                    message = responseMessage
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            context.ExceptionHandled = true;
        }
    }
}