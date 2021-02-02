using KenticoCommunity.PageAssetFolders.Interfaces;
using KenticoCommunity.PageAssetFolders.Models;
using KenticoCommunity.PageAssetFolders.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;


namespace KenticoCommunity.PageAssetFolders.Factories
{
    public class ConfiguredAssetFolderRegistrationListFactory : IAssetFolderRegistrationListFactory
    {
        private readonly List<AssetFolderRegistration> _assetFolderRegistrations = null;
        public ConfiguredAssetFolderRegistrationListFactory(IConfigurationHelper configurationHelper)
        {
            var configuration = configurationHelper.GetWebConfiguration();
            var defaultAssetFoldersSection =
                configuration?.GetSection(DefaultAssetFoldersSection.DefaultAssetFoldersSectionName) as
                DefaultAssetFoldersSection;
            _assetFolderRegistrations =
                defaultAssetFoldersSection?.AssetFolderRegistrations
                    .Where(x => !(string.IsNullOrWhiteSpace(x.ParentClass) 
                                  || string.IsNullOrWhiteSpace(x.ChildClass)
                                  || string.IsNullOrWhiteSpace(x.DefaultName)))
                    .Select(x => new AssetFolderRegistration
                        { 
                            ParentClass = x.ParentClass.Trim(), 
                            ChildClass = x.ChildClass.Trim(),
                            DefaultName = x.DefaultName.Trim()
                        })
                    .ToList();
            if(_assetFolderRegistrations == null)
            {
                _assetFolderRegistrations = new List<AssetFolderRegistration>();
            }
        }
        public List<AssetFolderRegistration> GetAssetFolderRegistrations() => _assetFolderRegistrations;
    }
}
