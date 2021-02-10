using CMS.Base;
using KenticoCommunity.PageAssetFolders.Helpers;
using KenticoCommunity.PageAssetFolders.Tests.TestHelpers;
using NUnit.Framework;
using CMS.Core;
using CMS;
using KenticoCommunity.PageAssetFolders.Factories;
using KenticoCommunity.PageAssetFolders.Interfaces;

namespace KenticoCommunity.PageAssetFolders.Tests.Helpers
{
    [TestFixture]
    public class RegistrationHelperTests
    {
        [SetUp]
        public void TestSetup()
        {
            Service.Use<IAssetFolderRegistrationListFactory, ConfiguredAssetFolderRegistrationListFactory>();
        }

        [Test]
        public void IsRegistered_Returns_True_If_Type_Is_Already_Registered()
        {
            var result = RegistrationHelper.IsRegistered<IAssetFolderRegistrationListFactory>();
            Assert.IsTrue(result);
        }

        [Test]
        public void IsRegistered_Returns_False_If_Type_Is_Not_Registered()
        {
            var result = RegistrationHelper.IsRegistered<IConfigurationHelper>();
            Assert.IsFalse(result);
        }
    }
}
