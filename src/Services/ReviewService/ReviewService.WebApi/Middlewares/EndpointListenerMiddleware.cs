﻿namespace ReviewService.WebApi.Middlewares
{
    public class EndpointListenerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<EndpointListenerMiddleware> _logger;

        public EndpointListenerMiddleware(RequestDelegate next, ILogger<EndpointListenerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {          
            _logger.LogInformation("Request Path: {Path}", httpContext.Request.Path.Value);
            await _next(httpContext);
        }
    }
}