using System.Linq;
using System.Web.Routing;
using Xunit;

namespace Epinova.QuickExport.Tests
{
    public class InitializerTests
    {
        [Fact]
        public void Initialize_AddsRoute()
        {
            var sut = new Initializer();
            sut.Initialize(null);

            Assert.True(RouteTable.Routes.AsQueryable().Cast<Route>().Any(r => r.Url == "QuickExport/{action}"));
        }
    }
}
