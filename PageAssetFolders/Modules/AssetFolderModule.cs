using System;
using CMS;
using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using KenticoCommunty.PageAssetFolders.Modules;
using KenticoCommunty.PageAssetFolders.Helpers;
using KenticoCommunty.PageAssetFolders.Interfaces;
using KenticoCommunty.PageAssetFolders.Repositories;
using KenticoCommunty.PageAssetFolders.Services;

[assembly: RegisterModule(typeof(AssetFolderModule))]

namespace KenticoCommunty.PageAssetFolders.Modules
{
    /// <summary>
    /// Module for managing the creation and maintenance of page and type-specific
    /// content folders.  This 
    /// - prevents authors from having to manually create them
    /// - ensures each page or type only has one root folder
    /// - ensures the content folder is at the top, and easy to find
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

        private const string FeatureContent = "Feature content";
        private const string PageContent = "Page assets";
        private const string RecursionKeyPrefix = "BlueModusAssetFolderModule_";

        public AssetFolderModule()
            : base(nameof(Module))
        {
        }

        protected override void OnPreInit()
        {
            base.OnPreInit();
            // Registered required types
            Service.Use<IAssetFolderService, AssetFolderService>();
            Service.Use<IAssetFolderRepository, AssetFolderRepository>();
            Service.Use<IEventLoggingHelper, EventLoggingHelper>();
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
            RegisterAssetFolderTypes();
            DocumentEvents.Insert.After += DocumentInsertAfter;
            DocumentEvents.Update.After += DocumentUpdateAfter;
            DocumentEvents.Insert.Before += DocumentInsertBefore;
        }

        /// <summary>
        /// Add to the list of page types for which to automatically add child folders here.
        /// </summary>
        private void RegisterAssetFolderTypes()
        {
            _assetFolderService.RegisterAssetFolderType("Chfa.LandingPage", "Chfa.LandingPageAssetsFolder", "Page assets");
        }


        /// <summary>
        /// A new page is created. See if it needs a content folder
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
        /// A page is updated. See if it needs a content folder
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
        /// Before saving a new node, ensure that it isn't a duplicate content folder node
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
