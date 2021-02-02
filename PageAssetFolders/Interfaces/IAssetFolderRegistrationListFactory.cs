using KenticoCommunity.PageAssetFolders.Models;
using System.Collections.Generic;

namespace KenticoCommunity.PageAssetFolders.Interfaces
{
    public interface IAssetFolderRegistrationListFactory
    {
        List<AssetFolderRegistration> GetAssetFolderRegistrations();
    }
}
