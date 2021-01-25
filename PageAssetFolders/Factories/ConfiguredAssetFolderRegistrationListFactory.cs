using KenticoCommunity.PageAssetFolders.Interfaces;
using KenticoCommunity.PageAssetFolders.Models;
using System;
using System.Collections.Generic;

namespace KenticoCommunity.PageAssetFolders.Factories
{
    public class ConfiguredAssetFolderRegistrationListFactory : IAssetFolderRegistrationListFactory
    {
        public List<AssetFolderRegistration> GetAssetFolderRegistrations()
        {
            return new List<AssetFolderRegistration>()
            {
                new AssetFolderRegistration()
                    {
                        ParentClass = "Sample.Parent",
                        ChildClass = "Sample.Child",
                        ChildName = "Asset folder"
                    }
            };
        }
    }
}
