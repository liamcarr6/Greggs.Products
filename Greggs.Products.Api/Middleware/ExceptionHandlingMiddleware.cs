using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Middleware
{
    /// <summary>
    /// Global exception handling middleware for consistent error responses
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Catches unhandled exceptions and converts them to JSON error responses
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred while processing the request");
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Maps exceptions to appropriate HTTP status codes and JSON responses
        /// </summary>
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Map exception types to HTTP status codes
            var (statusCode, message) = exception switch
            {
                ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
                InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),
                NotImplementedException => (HttpStatusCode.NotImplemented, "Feature not implemented"),
                _ => (HttpStatusCode.InternalServerError, "An internal server error occurred")
            };

            context.Response.StatusCode = (int)statusCode;

            // Standardized error response format
            var response = new
            {
                error = new
                {
                    message,
                    statusCode = (int)statusCode,
                    timestamp = DateTime.UtcNow
                }
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
        }
    }
}