using System.Diagnostics; 
using System.Web.Mvc; 
 
namespace MvcFilters.Infrastructure.Filters
{ 
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple=true)] 
    public class ProfileAttribute : FilterAttribute, IActionFilter
    { 
        private Stopwatch timer;

        public void OnActionExecuting(ActionExecutingContext filterContext)
        { 
            timer = Stopwatch.StartNew();     
        } 
 
        public void OnActionExecuted(ActionExecutedContext filterContext)
        { 
            timer.Stop(); 
            if (filterContext.Exception == null)
            { 
                filterContext.HttpContext.Response.Write( 
                    string.Format("Action method elapsed time: {0}",  
                        timer.Elapsed.TotalSeconds)); 
            } 
        } 
    } 
}