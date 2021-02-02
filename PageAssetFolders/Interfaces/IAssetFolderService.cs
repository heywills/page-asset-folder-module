using CMS.Base;
using CMS.DocumentEngine;

namespace KenticoCommunity.PageAssetFolders.Interfaces
{
    public interface IAssetFolderService
    {
        bool AssetFolderAlreadyExists(TreeNode parentNode, TreeNode newChildNode);
        void EnsureAssetFolderIfRegistered(TreeNode parentNode);
        void EnsureDefaultAssetFolder(TreeNode parentNode, string childClassName, string childName);
    }
}
