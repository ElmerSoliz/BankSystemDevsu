using BankDevsu.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BankDevsu.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            try { await _next(context); }
            catch (DomainException ex)
            {
                var pd = new ProblemDetails
                {
                    Title = ex.GetType().Name,
                    Detail = ex.Message,
                    Status = ex switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        ValidationException => StatusCodes.Status400BadRequest,
                        BalanceUnavailableException => StatusCodes.Status409Conflict,
                        DailyLimitExceededException => StatusCodes.Status409Conflict,
                        _ => StatusCodes.Status400BadRequest
                    }
                };
                context.Response.StatusCode = pd.Status!.Value;
                await context.Response.WriteAsJsonAsync(pd);
            }
            catch (Exception ex)
            {
                var pd = new ProblemDetails { Title = "ServerError", Detail = ex.Message, Status = 500 };
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(pd);
            }
        }
    }
}
