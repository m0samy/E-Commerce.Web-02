using System.Text.Json;
using DomainLayer.Exceptions;
using Shared.ErrorModels;

namespace E_Commerce.Web.CustomMiddleWares
{
    public class CustomExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleWare> _logger;

        public CustomExceptionHandlerMiddleWare(RequestDelegate Next , ILogger<CustomExceptionHandlerMiddleWare> logger)
        {
            _next = Next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
                await HandleNotFoundEndPointAsync(httpContext);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Something Went Wrong");
                await HandleExceptionAsync(httpContext, ex);

            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            //3. Create Response Object بعمل كلاس في الشير لاير
            var Response = new ErrorToReturn()
            {
                //StatusCode = httpContext.Response.StatusCode,
                ErrorMessage = ex.Message
            };

            //1.Set StatusCode For Response 
            Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                BadRequestException badRequestException => GetBadRequestException(badRequestException , Response),
                _ => StatusCodes.Status500InternalServerError
            };

            //2.Set Content type For Response
            //httpContext.Response.ContentType = "application/json"; //ممكن مكتبهاش عشان ال WriteAsJsonAsync بتعمل الخطوه دي

            httpContext.Response.StatusCode = Response.StatusCode;
            //4. Return Object As JSON
            await httpContext.Response.WriteAsJsonAsync(Response);
        }

        private static int GetBadRequestException(BadRequestException badRequestException, ErrorToReturn response)
        {
            response.Errors = badRequestException.Errors;
            return StatusCodes.Status400BadRequest;
        }

        private static async Task HandleNotFoundEndPointAsync(HttpContext httpContext)
        {
            if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var Response = new ErrorToReturn()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = $"End Point {httpContext.Request.Path} is Not Found"
                };

                await httpContext.Response.WriteAsJsonAsync(Response);
            }
        }



    }
}
