using account.details.common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace account.details.api.middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await WriteRespose(context, e);
            }
        }

        private async Task WriteRespose(HttpContext context, Exception exception)
        {
            if (exception != null)
            {
                context.Response.ContentType = Constants.Header.ExceptionContentType;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var traceId = Activity.Current?.Id ?? context?.TraceIdentifier;
                var responseData = new
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Message = exception.Message,
                    TraceId = traceId,
                };
                var stream = context.Response.Body;
                await JsonSerializer.SerializeAsync(stream, responseData);
            }
        }
    }
}
