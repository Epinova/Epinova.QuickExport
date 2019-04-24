using Xunit;

namespace Epinova.QuickExport.Tests
{
    public class ModuleTests
    {
        [Fact]
        public void CreateViewModel_FallsBackToDefaultTimeout()
        {
            var sut = new Module("test", null, null);
            var model = sut.CreateViewModel(null, null) as ViewModel;
            Assert.Equal(60000, model.Timeout);
        }

        [Fact]
        public void CreateViewModel_FallsBackToDefaultRoles()
        {
            var sut = new Module("test", null, null);
            var model = sut.CreateViewModel(null, null) as ViewModel;
            Assert.Contains("WebAdmins", model.AllowedRoles);
            Assert.Contains("Administrators", model.AllowedRoles);
        }

        [Fact]
        public void CreateViewModel_RespectsTimeoutInConfig()
        {
            var sut = new Module("test", null, null)
            {
                GetAppSetting = _ => "12345"
            };

            var model = sut.CreateViewModel(null, null) as ViewModel;
            Assert.Equal(12345, model.Timeout);
        }

        [Fact]
        public void CreateViewModel_RespectsRolesInConfig()
        {
            var sut = new Module("test", null, null)
            {
                GetAppSetting = _ => "Foo,Bar"
            };

            var model = sut.CreateViewModel(null, null) as ViewModel;
            Assert.DoesNotContain("WebAdmins", model.AllowedRoles);
            Assert.DoesNotContain("Administrators", model.AllowedRoles);
            Assert.Contains("Foo", model.AllowedRoles);
            Assert.Contains("Bar", model.AllowedRoles);
        }
    }
}
