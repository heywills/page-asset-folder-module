using CMS.Base;
using KenticoCommunity.PageAssetFolders.Helpers;
using KenticoCommunity.PageAssetFolders.Tests.TestHelpers;
using NUnit.Framework;

namespace KenticoCommunity.PageAssetFolders.Tests.Helpers
{
    [TestFixture]
    public class ConfigurationHelperTests
    {
        private readonly string _testProcessPath = PathHelper.GetTestConfigFilesDirectoryPath();

        [SetUp]
        public void TestSetup()
        {
            SystemContext.WebApplicationPhysicalPath = _testProcessPath;
        }

        [Test]
        public void GetWebConfiguration_Returns_Configuration_With_File_For_WebConfig()
        {
            var configurationHelper = new ConfigurationHelper();
            var configuration = configurationHelper.GetWebConfiguration();
            Assert.IsTrue(configuration.HasFile);
        }

        [Test]
        public void OpenConfiguration_Returns_Configuration_With_File_For_Configuration()
        {
            var configFileName = ConfigFileName.CorrectConfig;
            var configurationHelper = new ConfigurationHelper();
            var configuration = configurationHelper.OpenConfiguration(_testProcessPath, configFileName);
            Assert.IsTrue(configuration.HasFile);
        }
        [Test]
        public void OpenConfiguration_Returns_Configuration_Without_File_If_Invalid_FileName()
        {
            var configFileName = "invalid-file-name.config";
            var configurationHelper = new ConfigurationHelper();
            var configuration = configurationHelper.OpenConfiguration(_testProcessPath, configFileName);
            Assert.IsFalse(configuration.HasFile);
        }

    }
}
