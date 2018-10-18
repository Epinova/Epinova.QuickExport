using EPiServer.Framework.Web.Resources;
using EPiServer.Security;
using EPiServer.Shell.Modules;
using System.Collections.Generic;
using System.Linq;

namespace Epinova.QuickExport
{
    public class ViewModel : ModuleViewModel
    {
        public ViewModel(ShellModule module, IClientResourceService clientResourceService)
            : base(module, clientResourceService)
        {
        }

        public int Timeout { get; set; }

        public bool HasAccess => AllowedRoles.Any(PrincipalInfo.CurrentPrincipal.IsInRole);

        internal List<string> AllowedRoles { get; set; } = new List<string>();
    }
}