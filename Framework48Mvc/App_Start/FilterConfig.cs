using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using Framework48Mvc.Filters;

namespace Framework48Mvc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new GlobalExceptionFilter());
        }
    }
}
