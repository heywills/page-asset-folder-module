using CMS;
using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using KenticoCommunity.PageAssetFolders.Factories;
using KenticoCommunity.PageAssetFolders.Helpers;
using KenticoCommunity.PageAssetFolders.Interfaces;
using KenticoCommunity.PageAssetFolders.Modules;
using KenticoCommunity.PageAssetFolders.Repositories;
using KenticoCommunity.PageAssetFolders.Services;
using System;

[assembly: RegisterModule(typeof(AssetFolderModule))]

namespace KenticoCommunity.PageAssetFolders.Modules
{
    /// <summary>
    /// Module for managing the creation and maintenance of page and type-specific
    /// asset folders.  This 
    /// - prevents authors from having to manually create them
    /// - ensures each page or type only has one root folder
    /// - ensures the asset folder is at the top, and easy to find
    /// </summary>
    /// <remarks>
    ///     IMPORTANT: Add the following to your **web.config**, so that this module doesn't
    ///     automatically create nodes during staging operations. Otherwise, this will cause
    ///     staging conflicts.
    ///     
    ///     <add key="CMSStagingUseTreeCustomHandlers" value="false" />
    /// </remarks>
    public class AssetFolderModule : Module
    {
        private  IAssetFolderService _assetFolderService;

        private const string RecursionKeyPrefix = "KenticoCommunityAssetFolderModule_";

        public AssetFolderModule()
            : base(nameof(Module))
        {
        }

        /// <summary>
        /// Register the types required by this module.
        /// </summary>
        /// <remarks>
        /// Developers are allowed to register their own IAssetFolderRegistrationListFactory, so
        /// this code checks if one is already registered.
        /// </remarks>
        protected override void OnPreInit()
        {
            base.OnPreInit();
            Service.Use<IAssetFolderService, AssetFolderService>();
            Service.Use<IAssetFolderRepository, AssetFolderRepository>();
            Service.Use<IConfigurationHelper, ConfigurationHelper>();
            if (!RegistrationHelper.IsRegistered<IAssetFolderRegistrationListFactory>())
            {
                Service.Use<IAssetFolderRegistrationListFactory, ConfiguredAssetFolderRegistrationListFactory>();
            }
        }

        /// <summary>
        /// During initialization, register document events
        /// </summary>
        protected override void OnInit()
        {
            base.OnInit();
            // In a module, we have to use Service.Resolve for the top object of the
            // dependency chain. All others will be constructor injected.
            _assetFolderService = Service.Resolve<IAssetFolderService>();
            DocumentEvents.Insert.After += DocumentInsertAfter;
            DocumentEvents.Update.After += DocumentUpdateAfter;
            DocumentEvents.Insert.Before += DocumentInsertBefore;
        }

        /// <summary>
        /// A new page is created. See if it needs an asset folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DocumentInsertAfter(object sender, DocumentEventArgs e)
        {
            var page = e.Node;
            if (page != null)
            {
                if (e.DetectRecursion(RecursionKeyPrefix + page.NodeAliasPath))
                {
                    return;
                }
                _assetFolderService.EnsureAssetFolderIfRegistered(page);
            }
        }

        /// <summary>
        /// A page is updated. See if it needs an asset folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DocumentUpdateAfter(object sender, DocumentEventArgs e)
        {
            var page = e.Node;
            if (page != null)
            {
                if (e.DetectRecursion(RecursionKeyPrefix + page.NodeAliasPath))
                {
                    return;
                }
                _assetFolderService.EnsureAssetFolderIfRegistered(page);
            }
        }

        /// <summary>
        /// Before saving a new node, ensure that it isn't a duplicate asset folder node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DocumentInsertBefore(object sender, DocumentEventArgs e)
        {
            var child = e.Node;
            var parent = e.TargetParentNode; // On insert, there will always be a parent node

            if ((parent == null) || (child == null))
            {
                return;
            }
            if (e.DetectRecursion(RecursionKeyPrefix + child.NodeAliasPath))
            {
                if (_assetFolderService.AssetFolderAlreadyExists(parent, child))
                {
                    e.Cancel();
                    throw new ApplicationException($"There is already a, {child.ClassName}, node under this page.");
                }
            }
        }

    }
}
