using Microsoft.AspNetCore.Mvc.Filters;

namespace BarcodeService.Api.Attributes;

public class TestAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        Console.WriteLine("OnActionExecuting");
    }

    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
        Console.WriteLine("OnActionExecuted");
    }

    public override void OnResultExecuting(ResultExecutingContext filterContext)
    {
        Console.WriteLine("OnResultExecuting");
    }

    public override void OnResultExecuted(ResultExecutedContext filterContext)
    {
        Console.WriteLine("OnResultExecuted");
    }
}