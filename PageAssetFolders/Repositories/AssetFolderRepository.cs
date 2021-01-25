using CMS.Base;
using CMS.DocumentEngine;
using KenticoCommunity.PageAssetFolders.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using CMS;
using CMS.DataEngine;
using CMS.Localization;
using KenticoCommunity.PageAssetFolders.Helpers;

namespace KenticoCommunity.PageAssetFolders.Repositories
{
    public class AssetFolderRepository : IAssetFolderRepository
    {
        private const string NodeIdName = "NodeID";
        /// <summary>
        /// Create a new content folder under the provided parent node using the provided childName as the document name
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="childClassName"></param>
        /// <param name="childName"></param>
        /// <param name="aliasName"></param>
        /// <remarks>Using ITreeNode as a parameter type so this repository can be abstracted and faked. However, the
        /// concrete implementation requires the parameter to be TreeNode so that it can be passed to the treeNode.Insert method</remarks>
        public void AddChildNode(TreeNode parentNode, string childClassName, string childName, string aliasName = null)
        {
            Guard.ArgumentNotNull(parentNode, nameof(parentNode));
            Guard.ArgumentNotNullOrEmpty(childClassName, nameof(childClassName));
            Guard.ArgumentNotNullOrEmpty(childName, nameof(childName));
            if (!(parentNode is TreeNode))
            {
                throw new ArgumentException("parentNode must be a TreeNode", nameof(parentNode));
            }

            var concreteParent = (TreeNode)parentNode;
            var culture = LocalizationContext.CurrentCulture.CultureCode;
            var childFolder = TreeNode.New(childClassName);
            childFolder.DocumentName = childName;
            childFolder.DocumentCulture = culture;
            if (!string.IsNullOrWhiteSpace(aliasName))
            {
                childFolder.NodeAlias = aliasName;
            }
            childFolder.Insert(concreteParent);
        }

        /// <summary>
        /// Get the immediate child nodes of the provided parent node that have the specified class name.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="childClassName"></param>
        /// <returns></returns>
        /// <remarks>Using ITreeNode as a parameter type so this repository can be abstracted and faked. However, the
        /// concrete implementation requires the parameter to be TreeNode so that it's NodeHasChildren property can be used.</remarks>
        public List<TreeNode> GetChildNodesByClass(TreeNode parentNode, string childClassName)
        {
            Guard.ArgumentNotNull(parentNode, nameof(parentNode));
            Guard.ArgumentNotNullOrEmpty(childClassName, nameof(childClassName));
            if (!(parentNode is TreeNode))
            {
                throw new ArgumentException("parentNode must be a TreeNode", nameof(parentNode));
            }
            var concreteParent = (TreeNode)parentNode;
            var returnList = new List<TreeNode>();
            if (concreteParent.NodeHasChildren)
            {
                returnList = DocumentHelper.GetDocuments(childClassName)
                                           .Path(parentNode.NodeAliasPath, PathTypeEnum.Children)
                                           .NestingLevel(1)
                                           .Culture(LocalizationContext.CurrentCulture.CultureCode)
                                           .CombineWithAnyCulture()
                                           .OnCurrentSite()
                                           .ToList();
            }
            return returnList;
        }

        /// <summary>
        /// Get the a node of the provided alias path.
        /// </summary>
        /// <param name="aliasPath"></param>
        /// <returns></returns>
        /// <remarks>Using ITreeNode as a parameter type so this repository can be abstracted and faked.</remarks>
        public TreeNode GetNodeByAliasPath(string aliasPath)
        {
            Guard.ArgumentNotNullOrEmpty(aliasPath, nameof(aliasPath));

            var nodeList = DocumentHelper.GetDocuments()
                                         .Path(aliasPath, PathTypeEnum.Single)
                                         .Culture(LocalizationContext.CurrentCulture.CultureCode)
                                         .CombineWithAnyCulture()
                                         .OnCurrentSite()
                                         .ToList();
            return nodeList.FirstOrDefault();
        }

        /// <summary>
        /// Move the treeNode to the top of all its siblings.
        /// </summary>
        /// <param name="treeNode"></param>
        public void MoveNodeToTop(TreeNode treeNode)
        {
            Guard.ArgumentNotNull(treeNode, nameof(treeNode));
            treeNode.TreeProvider.SetNodeOrder(treeNode, DocumentOrderEnum.First);
        }

        /// <summary>
        /// Create a transaction scope, used to ensure repository calls run within a single transaction.
        /// This object should be disposed by the calling code.
        /// </summary>
        /// <returns></returns>
        public ITransactionScope CreateTransactionScope()
        {
            return new CMSTransactionScope();
        }

        /// <summary>
        /// Get the a node of the provided node ID
        /// </summary>
        /// <param name="nodeId">The unique identifier for the desired node</param>
        /// <param name="siteCode">The code of the Kentico site to query</param>
        /// <param name="cultureCode">The identifying code for the culture</param>
        /// <returns>an ITreeNode object</returns>
        /// <remarks>Using ITreeNode as a parameter type so this repository can be abstracted and faked.</remarks>
        public virtual TreeNode GetNodeById(int nodeId, string siteCode, string cultureCode)
        {
            Guard.ArgumentGreaterThanZero(nodeId);
            Guard.ArgumentNotNullOrEmpty(siteCode);
            Guard.ArgumentNotNullOrEmpty(cultureCode);

            var nodeList = DocumentHelper.GetDocuments()
                                         .Where(NodeIdName, QueryOperator.Equals, nodeId)
                                         .Culture(cultureCode)
                                         .OnSite(siteCode)
                                         .ToList();
            return nodeList.FirstOrDefault();
        }
    }
}
