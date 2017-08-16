using System.Web.Mvc;
using System.Web.Routing;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Initialization.Internal;

namespace Epinova.QuickExport
{
    [InitializableModule]
    [ModuleDependency(typeof(PlugInInitialization))]
    public class Initializer : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            RouteTable.Routes.MapRoute(
                "QuickExport",
                "QuickExport/{action}",
                new { controller = "QuickExport", action = "Index" });
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void Preload(string[] parameters)
        {
        }
    }
}