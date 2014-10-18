using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using SqlSaturdayCode.Models;

namespace SqlSaturdayCode
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IDocumentStore DocumentStore { get; private set; }

        protected void Application_Start()
        {
            SetupRavenDb();
            #region Std MVC stuff
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            #endregion
        }

        private void SetupRavenDb()
        {
            DocumentStore = new DocumentStore()
            {
                ConnectionStringName = "RavenDB"
            }.Initialize();

            IndexCreation.CreateIndexes(typeof(SessionsPerSpeaker).Assembly, DocumentStore);
        }
    }
}
