using KenticoCommunity.PageAssetFolders.Configurations;
using KenticoCommunity.PageAssetFolders.Helpers;
using KenticoCommunity.PageAssetFolders.Interfaces;
using KenticoCommunity.PageAssetFolders.Factories;
using KenticoCommunity.PageAssetFolders.Repositories;
using KenticoCommunity.PageAssetFolders.Tests.TestHelpers;
using Moq;
using NUnit.Framework;
using System.Configuration;
using System.Linq;


namespace KenticoCommunity.PageAssetFolders.Tests.Factories
{
    [TestFixture]
    public class ConfiguredAssetFolderRegistrationListFactoryTests
    {
        private readonly string _testProcessPath = PathHelper.GetTestConfigFilesDirectoryPath();
        private readonly ConfigurationHelper _configurationHelper = new ConfigurationHelper();

        [TestCase(ConfigFileName.CorrectConfig, 2)]
        [TestCase(ConfigFileName.WebConfig, 1)]
        [TestCase(ConfigFileName.EmptyCollection, 0)]
        [TestCase(ConfigFileName.MissingSection, 0)]
        [TestCase(ConfigFileName.NoConfig, 0)]
        [TestCase(ConfigFileName.BlankAttributesConfig, 0)]
        [TestCase(ConfigFileName.BadSectionName, 0)]
        public void GetAssetFolderRegistrations_Returns_Expected_List_Count(string configFileName, int expectedCount)
        {
            var mockConfigurationHelper = CreateMockConfigurationHelperForFile(configFileName);
            var factory = new ConfiguredAssetFolderRegistrationListFactory(mockConfigurationHelper.Object);
            var assetFolderRegistrations = factory.GetAssetFolderRegistrations();
            Assert.AreEqual(expectedCount, assetFolderRegistrations?.Count);
        }

        [Test()]
        public void GetAssetFolderRegistrations_Returns_Trimmed_Name()
        {
            var mockConfigurationHelper = CreateMockConfigurationHelperForFile(ConfigFileName.Untrimmed);
            var factory = new ConfiguredAssetFolderRegistrationListFactory(mockConfigurationHelper.Object);
            var assetFolderRegistrations = factory.GetAssetFolderRegistrations();
            var assetFolderRegistration = assetFolderRegistrations?.FirstOrDefault();
            Assert.AreEqual("cms.menuitem", assetFolderRegistration?.ParentClass);
            Assert.AreEqual("cms.folder", assetFolderRegistration?.ChildClass);
            Assert.AreEqual("Assets", assetFolderRegistration?.DefaultName);
        }

        [TestCase(ConfigFileName.MissingAttributes)]
        [TestCase(ConfigFileName.BadElementNames)]
        [TestCase(ConfigFileName.DuplicateKeys)]
        public void ConfiguredAssetFolderRegistrationListFactory_Constructor_Throws_ConfigurationErrorsException_If_InvalidConfiguration(string configFileName)
        {
            var mockConfigurationHelper = CreateMockConfigurationHelperForFile(configFileName);
            Assert.That(() => (new ConfiguredAssetFolderRegistrationListFactory(mockConfigurationHelper.Object)), Throws.TypeOf<ConfigurationErrorsException>());
        }

        private Mock<IConfigurationHelper> CreateMockConfigurationHelperForFile(string configFileName)
        {
            var configuration = _configurationHelper.OpenConfiguration(_testProcessPath, configFileName);
            var mockConfigurationHelper = new Mock<IConfigurationHelper>();
            mockConfigurationHelper.Setup(m => m.GetWebConfiguration()).Returns(configuration);
            return mockConfigurationHelper;
        }

    }
}
