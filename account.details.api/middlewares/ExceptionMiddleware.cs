using account.details.common;
using account.details.common.exceptions;
using account.details.common.models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                await WriteRespose(context, e);
            }
        }

        private async Task WriteRespose(HttpContext context, Exception exception)
        {
            Error errResponse;
            if (exception != null)
            {
                context.Response.ContentType = Constants.Header.ExceptionContentType;
                var traceId = Activity.Current?.Id ?? context?.TraceIdentifier;
                context.Response.Headers.TryAdd("x-correlation-id", traceId);
                if (exception is ApiException)
                {
                    var ex = exception as ApiException;
                    context.Response.StatusCode = (int)ex.Code;
                    errResponse = new Error
                    {
                        Code = $"ERR-{(int)ex.Code}",
                        ErrorDetails = new {
                            Message = ex.Message,
                        },
                    };
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errResponse = new Error
                    {
                        Code = $"ERR-{(int)HttpStatusCode.InternalServerError}",
                        ErrorDetails = new
                        {
                            Message = exception.Message,
                        },
                    };
                }

                var stream = context.Response.Body;
                await JsonSerializer.SerializeAsync(stream, errResponse);
            }
        }
    }
}
