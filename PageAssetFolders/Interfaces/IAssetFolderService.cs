using CMS.Base;
using CMS.DocumentEngine;

namespace KenticoCommunty.PageAssetFolders.Interfaces
{
    public interface IAssetFolderService
    {
        bool AssetFolderAlreadyExists(TreeNode parentNode, TreeNode newChildNode);
        void EnsureAssetFolderIfRegistered(TreeNode parentNode);
        void EnsureDefaultAssetFolder(TreeNode parentNode, string childClassName, string childName);
        void RegisterAssetFolderType(string parentClassName, string childClassName, string defaultChildName);
    }
}
