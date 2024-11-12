using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NZWalks.Api.MiddleWare
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger ,RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context){
            
            try{
                await next(context);

            } catch(Exception ex){
                var errorId = Guid.NewGuid().ToString();
                //log this information
                logger.LogError(ex, $"{errorId}: {ex.Message}");
                //return a cutom error
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var error = new {
                    Id= errorId,
                    ErrorMessage = "Something went worng , we are looking into it "
                };

                await context.Response.WriteAsJsonAsync(error);

            }

        }
    }
}