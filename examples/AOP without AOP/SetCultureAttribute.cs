using System.Globalization;
using System.Web.Mvc;

namespace Examples.Web.ActionFilters
{
    /// <summary>
    /// Set culture for current thread dependent on user selection or default value
    /// </summary>
    public class SetCultureAttribute : FilterAttribute, IActionFilter
    {
        /// <summary>
        /// Set culture for current Thread
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuting(ActionExecutingContext
            filterContext)
        {
            var cultureCode = LocalizationHelper.GetCurrentLanguage(filterContext);

            var culture = new CultureInfo(cultureCode);
            System.Threading.Thread.CurrentThread.CurrentCulture =
                culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture =
                culture;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
	}
    }
}