using System.Globalization;
using System.Web.Mvc;

namespace Examples.Web.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple=true)]
    public class SetCultureAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cultureCode = LocalizationHelper.GetCurrentLanguage(filterContext);

            var culture = new CultureInfo(cultureCode);

            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
	}
    }
}