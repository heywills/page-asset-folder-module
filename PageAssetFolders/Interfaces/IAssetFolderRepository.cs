using System.Collections.Generic;
using CMS.Base;
using CMS.DataEngine;
using CMS.DocumentEngine;

namespace KenticoCommunity.PageAssetFolders.Interfaces
{
    public interface IAssetFolderRepository
    {
        /// <summary>
        /// Get the immediate child nodes of the provided parent node that have the specified class name.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="childClassName"></param>
        /// <returns></returns>
        List<TreeNode> GetChildNodesByClass(TreeNode parentNode, string childClassName);

        /// <summary>
        /// Create a new child node under the provided parent node using the provided childName as the document name
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="childClassName"></param>
        /// <param name="childName"></param>
        /// <param name="aliasName"></param>
        void AddChildNode(TreeNode parentNode, string childClassName, string childName, string aliasName = null);

        /// <summary>
        /// Move the treeNode to the top of all its siblings.
        /// </summary>
        /// <param name="treeNode"></param>
        void MoveNodeToTop(TreeNode treeNode);

        /// <summary>
        /// Get the a node of the provided alias path.
        /// </summary>
        /// <param name="aliasPath"></param>
        /// <returns></returns>
        /// <remarks>Using ITreeNode as a parameter type so this repository can be abstracted and faked.</remarks>
        TreeNode GetNodeByAliasPath(string aliasPath);

        /// <summary>
        /// Create a transaction scope, used to ensure repository calls run within a single transaction.
        /// This object should be disposed by the calling code.
        /// </summary>
        /// <returns></returns>
        ITransactionScope CreateTransactionScope();

        /// <summary>
        /// Get the a node of the provided node ID
        /// </summary>
        /// <param name="nodeId">The unique identifier for the desired node</param>
        /// <param name="siteCode">The code of the Kentico site to query</param>
        /// <param name="cultureCode">The identifying code for the culture</param>
        /// <returns></returns>
        /// <remarks>Using ITreeNode as a parameter type so this repository can be abstracted and faked.</remarks>
        TreeNode GetNodeById(int nodeId, string siteCode, string cultureCode);
    }
}
