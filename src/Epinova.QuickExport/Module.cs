using EPiServer.Framework.TypeScanner;
using EPiServer.Framework.Web.Resources;
using EPiServer.Shell.Modules;
using System;
using System.Web.Configuration;
using System.Web.Hosting;

namespace Epinova.QuickExport
{
    public class Module : ShellModule
    {
        private const int defaultTimeout = 60000;
        private const string defaultRoles = "WebAdmins,Administrators";

        public Module(string name, string routeBasePath, string resourceBasePath)
            : base(name, routeBasePath, resourceBasePath)
        {
        }

        public Module(string name, string routeBasePath, string resourceBasePath, ITypeScannerLookup typeScannerLookup, VirtualPathProvider virtualPathProvider) 
            : base(name, routeBasePath, resourceBasePath, typeScannerLookup, virtualPathProvider)
        {
        }

        public override ModuleViewModel CreateViewModel(ModuleTable moduleTable, IClientResourceService clientResourceService)
        {
            var viewModel = new ViewModel(this, clientResourceService);

            if (!Int32.TryParse(GetAppSetting("QuickExport:Timeout"), out int timeout))
            {
                timeout = defaultTimeout;
            }

            viewModel.Timeout = timeout;

            var rolesConfig = GetAppSetting("QuickExport:AllowedRoles");
            var roles = String.IsNullOrEmpty(rolesConfig) ? defaultRoles : rolesConfig;

            viewModel.AllowedRoles.AddRange(roles.Split(','));

            return viewModel;
        }

        internal Func<string, string> GetAppSetting = key => WebConfigurationManager.AppSettings.Get(key);
    }
}