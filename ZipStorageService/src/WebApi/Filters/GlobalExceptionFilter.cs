using Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public GlobalExceptionFilter()
        {

        }

        void IExceptionFilter.OnException(ExceptionContext context)
        {
            var errorResponse = new GlobalErrorResponse { Error = context.Exception.Message };

            switch (context.Exception)
            {
                case FileAlreadyExistsException:
                    context.Result = new ConflictObjectResult(errorResponse);
                    break;
                case InvalidFileStructureException:
                case InvalidFileFormatException:
                case ValidationException:
                    context.Result = new BadRequestObjectResult(errorResponse);
                    break;
                case FileNotFoundException:
                    context.Result = new NotFoundObjectResult(errorResponse);
                    break;
                default:
                    context.Result = new StatusCodeResult(500);
                    break;
            }
        }
    }
}
