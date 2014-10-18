using System.Web.Mvc;
using Raven.Client;

namespace SqlSaturdayCode.Controllers
{
    public class RavenBaseController : Controller
    {
        protected IDocumentSession DocumentSession { get; set; }

        // This can also be handled by an IoC Container
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            DocumentSession = MvcApplication.DocumentStore.OpenSession();
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.Exception == null)
            {
                // any exception during request rollbacks changes
                DocumentSession.SaveChanges();
            }

            DocumentSession.Dispose();
        }
    }
}