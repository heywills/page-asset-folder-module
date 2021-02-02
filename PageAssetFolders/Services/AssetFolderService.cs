using CMS.Core;
using CMS.DocumentEngine;
using KenticoCommunity.PageAssetFolders.Helpers;
using KenticoCommunity.PageAssetFolders.Interfaces;
using KenticoCommunity.PageAssetFolders.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KenticoCommunity.PageAssetFolders.Services
{
    /// <summary>
    /// Helper class for managing parent type and asset folder type relationships. This class can
    /// be used to check if the class is configured to have an automatic, default asset folder
    /// and automatically enforce the relationship every time its EnsureAssetFolder method is called.
    /// This allows managing the creation and maintenance of page and type-specific
    /// asset folders.
    /// </summary>
    public class AssetFolderService : IAssetFolderService
    {
        private readonly IAssetFolderRepository _assetFolderRepository;
        private readonly IEventLogService _eventLogService;
        private readonly List<AssetFolderRegistration> _registeredAssetFolderTypes;

        public AssetFolderService(IAssetFolderRepository assetFolderRepository, IAssetFolderRegistrationListFactory assetFolderRegistrationListFactory, IEventLogService eventLogService)
        {
            _assetFolderRepository = assetFolderRepository;
            _eventLogService = eventLogService;
            _registeredAssetFolderTypes = assetFolderRegistrationListFactory.GetAssetFolderRegistrations();
        }

        /// <summary>
        /// Check if there is a asset folder class registered for the provided parent node class
        /// name. If true, ensure the parent node has one child that is of the registered asset
        /// folder type.
        /// </summary>
        /// <param name="parentNode"></param>
        public void EnsureAssetFolderIfRegistered(TreeNode parentNode)
        {
            Guard.ArgumentNotNull(parentNode, nameof(parentNode));
            var registration = GetRegistrationByParentClass(parentNode.ClassName);
            if (registration != null)
            {
                EnsureDefaultAssetFolder(parentNode, registration.ChildClass, registration.DefaultName);
            }
        }

        private AssetFolderRegistration GetRegistrationByParentClass(string parentClassName)
        {
            return _registeredAssetFolderTypes.FirstOrDefault(r => r.ParentClass.Equals(parentClassName, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// For the provided parentNode, ensure a child node with the provided childClassName exists.
        /// If it doesn't exist, create one using the provided childName as the document name.
        /// Ensure the child node is the first child under the parent, so that authors can easily
        /// find the node's child asset.
        ///
        /// DO NOT allow exceptions to be raised, because we do not want to fail the creation of
        /// a page, just because the automatic creation of a child node fails.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="childClassName"></param>
        /// <param name="childName"></param>
        /// <remarks>
        /// The ITreeNode interface only covers some of the implemented properties. Cases that
        /// require concrete-only members are not covered by unit tests.
        /// </remarks>
        public void EnsureDefaultAssetFolder(TreeNode parentNode, string childClassName, string childName)
        {
            Guard.ArgumentNotNull(parentNode, nameof(parentNode));
            Guard.ArgumentNotNullOrEmpty(childClassName, nameof(childClassName));
            Guard.ArgumentNotNullOrEmpty(childName, nameof(childName));
            try
            {
                using (var transactionScope = _assetFolderRepository.CreateTransactionScope())
                {

                    var matchingChildren = _assetFolderRepository.GetChildNodesByClass(parentNode, childClassName);
                    if (matchingChildren.Count == 0)
                    {
                        _assetFolderRepository.AddChildNode(parentNode, childClassName, childName);
                    }
                    else if (matchingChildren.Count == 1)
                    {
                        var firstNode = matchingChildren.First();
                        if (firstNode.NodeOrder != 1)
                        {
                            _assetFolderRepository.MoveNodeToTop(firstNode);
                        }
                    }
                    else
                    {
                        _eventLogService.LogEvent("W",
                                                  nameof(AssetFolderService), 
                                                  "TooManyAssetFolders",
                                                  $"The page '{parentNode.NodeAliasPath}' should only contain one folder of type '{childClassName}'.)");
                    }
                    transactionScope.Commit();
                }
            }
            catch (Exception ex)
            {
                _eventLogService.LogException(nameof(AssetFolderService), nameof(EnsureDefaultAssetFolder), ex);
            }

        }


        /// <summary>
        /// Return true if the parent node already has a child with a class name matching
        /// the class name of the new child node.
        /// This method is used to determine if saving a new child node should be cancelled.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="newChildNode"></param>
        /// <returns></returns>
        public bool AssetFolderAlreadyExists(TreeNode parentNode, TreeNode newChildNode)
        {
            Guard.ArgumentNotNull(parentNode, nameof(parentNode));
            Guard.ArgumentNotNull(newChildNode, nameof(newChildNode));
            var matchingChildren = _assetFolderRepository.GetChildNodesByClass(parentNode, newChildNode.ClassName);
            if (matchingChildren.Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
