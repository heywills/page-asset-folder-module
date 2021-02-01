using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.CMS;
using CMS.Tests;
using KenticoCommunity.PageAssetFolders.Interfaces;
using KenticoCommunity.PageAssetFolders.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Tests.DocumentEngine;

namespace KenticoCommunity.PageAssetFolders.Tests.Services
{
    [TestFixture, NonParallelizable]

    public class AssetFolderServiceTests: UnitTests
    {
        private readonly Mock<IAssetFolderRepository> _assetFolderRepository = new Mock<IAssetFolderRepository>();
        private readonly Mock<IEventLogService> _eventLogService = new Mock<IEventLogService>();
        private readonly Mock<IAssetFolderRegistrationListFactory> _assetFolderRegistrationListFactory = new Mock<IAssetFolderRegistrationListFactory>();
        private IAssetFolderService _assetFolderService = null;

        [SetUp]
        public void TestSetup()
        {
            SetGetChildNodesByClassMockReturn(new List<TreeNode>());
            SetGetNodeByAliasPathMockReturn(null);
            _assetFolderRepository.Setup(x => x.CreateTransactionScope())
                .Returns((new Mock<ITransactionScope>()).Object);

            _eventLogService.Setup(x => x.LogException(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Exception>(), null));
            _eventLogService.Setup(x => x.LogEvent(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            _assetFolderRegistrationListFactory.Setup(x => x.GetAssetFolderRegistrations())
                .Returns(new List<Models.AssetFolderRegistration>()
                {
                    new Models.AssetFolderRegistration()
                    {
                        ParentClass = Folder.CLASS_NAME,
                        ChildClass = Folder.CLASS_NAME,
                        ChildName = "ChildDocumentName"
                    }
                });
            DocumentGenerator.RegisterDocumentType<Folder>(Folder.CLASS_NAME);
            Fake().DocumentType<Folder>(Folder.CLASS_NAME);
            DocumentGenerator.RegisterDocumentType<MenuItem>(MenuItem.CLASS_NAME);
            Fake().DocumentType<MenuItem>(MenuItem.CLASS_NAME);

            _assetFolderService = new AssetFolderService(_assetFolderRepository.Object,
                                                         _assetFolderRegistrationListFactory.Object,
                                                         _eventLogService.Object);
        }

        /// <summary>
        /// Test that EnsureDefaultAssetFolder will create a new child when needed.
        /// </summary>
        [Test]
        public void EnsureDefaultAssetFolder_RequiresNewContentFolder()
        {
            var treeNodeRepository = _assetFolderRepository;
            var parentClassName = MenuItem.CLASS_NAME;
            var childClassName = Folder.CLASS_NAME;
            var childDocumentName = "ChildDocumentName";
            var parentNode = TreeNode.New(parentClassName);
            SetGetChildNodesByClassMockReturn(new List<TreeNode>());

            _assetFolderService.EnsureDefaultAssetFolder(parentNode, childClassName, childDocumentName);

            treeNodeRepository.Verify((m => m.AddChildNode(parentNode, childClassName, childDocumentName, null)), Times.Once);
        }


        /// <summary>
        /// Test that EnsureDefaultAssetFolder will not create a child node, if one already exists.
        /// </summary>
        [Test]
        public void EnsureDefaultAssetFolder_ExistingContentFolder()
        {
            var treeNodeRepository = _assetFolderRepository;
            var parentClassName = MenuItem.CLASS_NAME;
            var childClassName = Folder.CLASS_NAME;
            var childDocumentName = "ChildDocumentName";
            var parentNode = TreeNode.New(parentClassName);
            var childNode = TreeNode.New(childClassName);

            SetGetChildNodesByClassMockReturn(new List<TreeNode>(new[] { childNode }));

            _assetFolderService.EnsureDefaultAssetFolder(parentNode, childClassName, childDocumentName);

            treeNodeRepository.Verify((m => m.AddChildNode(parentNode, childClassName, childDocumentName, null)), Times.Never);
        }

        /// <summary>
        /// Test that EnsureDefaultAssetFolder will log a warning if more than one asset folder exists.
        /// </summary>
        [Test]
        public void EnsureDefaultAssetFolder_TooManyContentFolders()
        {
            var treeNodeRepository = _assetFolderRepository;
            var eventLoggingHelper = _eventLogService;
            var parentClassName = MenuItem.CLASS_NAME;
            var childClassName = Folder.CLASS_NAME;
            var childDocumentName = "ChildDocumentName";
            var parentNode = TreeNode.New(parentClassName);
            var childNode = TreeNode.New(childClassName);

            SetGetChildNodesByClassMockReturn(new List<TreeNode>(new[] { childNode, childNode }));
            _eventLogService.Invocations.Clear();
            _assetFolderService.EnsureDefaultAssetFolder(parentNode, childClassName, childDocumentName);

            treeNodeRepository.Verify((m => m.AddChildNode(parentNode, childClassName, childDocumentName, null)), Times.Never);
            eventLoggingHelper.Verify(m => m.LogEvent("W", nameof(AssetFolderService), "TooManyAssetFolders", It.IsAny<string>()),
                                      Times.Once);
        }


        /// <summary>
        /// Test that AssetFolderAlreadyExists will return true if the parent node
        /// already has a child with the same class name.
        /// </summary>
        [Test]
        public void AssetFolderAlreadyExists_True()
        {
            var parentClassName = MenuItem.CLASS_NAME;
            var childClassName = Folder.CLASS_NAME;
            var parentNode = TreeNode.New(parentClassName);
            var childNode = TreeNode.New(childClassName);
            var existingChild = TreeNode.New(childClassName);

            SetGetChildNodesByClassMockReturn(new List<TreeNode>(new[] { existingChild }));
            var result = _assetFolderService.AssetFolderAlreadyExists(parentNode, childNode);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Test that AssetFolderAlreadyExists will return true if the parent node
        /// already has a child with the same class name.
        /// </summary>
        [Test]
        public void AssetFolderAlreadyExists_False()
        {
            var parentClassName = MenuItem.CLASS_NAME;
            var childClassName = Folder.CLASS_NAME;
            var parentNode = TreeNode.New(parentClassName);
            var childNode = TreeNode.New(childClassName);

            SetGetChildNodesByClassMockReturn(new List<TreeNode>(new List<TreeNode>()));

            var result = _assetFolderService.AssetFolderAlreadyExists(parentNode, childNode);
            Assert.IsFalse(result);
        }

        [Test]
        public void AssetFolderAlreadyExists_NewChildNodeNullCheck()
        {
            var parentNode = TreeNode.New(MenuItem.CLASS_NAME);
            Assert.Throws<ArgumentNullException>(delegate
            {
                _assetFolderService.AssetFolderAlreadyExists(parentNode, null);
            });
        }

        [Test]
        public void AssetFolderAlreadyExists_ParentNodeNullCheck()
        {
            var childNode = TreeNode.New(MenuItem.CLASS_NAME);
            Assert.Throws<ArgumentNullException>(delegate
            {
                _assetFolderService.AssetFolderAlreadyExists(null, childNode);
            });
        }



        [TestCase(null, "className", "childName")]
        public void EnsureDefaultAssetFolder_NodeNullCheck(TreeNode parentNode, string className, string childName)
        {
            Assert.Throws<ArgumentNullException>(delegate
            {
                _assetFolderService.EnsureDefaultAssetFolder(parentNode, className, childName);
            });
        }

        [TestCase(null, "childName"),
        TestCase("className", null)]
        public void EnsureDefaultAssetFolder_StringNullCheck(string className, string childName)
        {
            var parentNode = TreeNode.New(MenuItem.CLASS_NAME);
            Assert.Throws<ArgumentNullException>(delegate
            {
                _assetFolderService.EnsureDefaultAssetFolder(parentNode, className, childName);
            });
        }

        [TestCase("", "childName"),
         TestCase("className", "")]
        public void EnsureDefaultAssetFolder_StringEmptyCheck(string className, string childName)
        {
            var parentNode = TreeNode.New(MenuItem.CLASS_NAME);
            Assert.Throws<ArgumentException>(delegate
            {
                _assetFolderService.EnsureDefaultAssetFolder(parentNode, className, childName);
            });
        }

        /// <summary>
        /// Test that EnsureDefaultAssetFolder will log an exception and not throw it
        /// </summary>
        [Test]
        public void EnsureDefaultAssetFolder_LogException()
        {
            var treeNodeRepository = _assetFolderRepository;
            var eventLoggingHelper = _eventLogService;
            var parentClassName = MenuItem.CLASS_NAME;
            var childClassName = Folder.CLASS_NAME;
            var childDocumentName = "ChildDocumentName";
            var parentNode = TreeNode.New(parentClassName);

            treeNodeRepository.Invocations.Clear();
            _eventLogService.Invocations.Clear();
            treeNodeRepository.Setup(x => x.GetChildNodesByClass(It.IsAny<TreeNode>(), It.IsAny<string>()))
                .Throws(new IOException("An unexpected IO error occurred"));

            _assetFolderService.EnsureDefaultAssetFolder(parentNode, childClassName, childDocumentName);

            treeNodeRepository.Verify((m => m.AddChildNode(parentNode, childClassName, childDocumentName, null)), Times.Never);
            eventLoggingHelper.Verify(m => m.LogException(nameof(AssetFolderService), nameof(_assetFolderService.EnsureDefaultAssetFolder), It.IsAny<IOException>(), null), Times.Once);
        }

        /// <summary>
        /// Set what will be returned by the mock of IAssetFolderRepository.GetChildNodesByClass
        /// </summary>
        /// <param name="nodes"></param>
        public void SetGetChildNodesByClassMockReturn(List<TreeNode> nodes)
        {
            _assetFolderRepository.Invocations.Clear();
            _assetFolderRepository.Setup(x => x.GetChildNodesByClass(It.IsAny<TreeNode>(), It.IsAny<string>()))
                .Returns(new List<TreeNode>(nodes));

        }

        /// <summary>
        /// Set what will be returned by the mock of IAssetFolderRepository.GetNodeByAliasPath
        /// </summary>
        /// <param name="node"></param>
        public void SetGetNodeByAliasPathMockReturn(TreeNode node)
        {
            _assetFolderRepository.Invocations.Clear();
            _assetFolderRepository.Setup(x => x.GetNodeByAliasPath(It.IsAny<string>()))
                .Returns(node);

        }

        /// <summary>
        /// Set what will be returned by the mock of IAssetFolderRepository.GetNodeByAliasPath
        /// when a particular alias path is provided
        /// </summary>
        /// <param name="aliasPath"></param>
        /// <param name="node"></param>
        public void SetGetNodeByAliasPathMockReturn(string aliasPath, TreeNode node)
        {
            _assetFolderRepository.Setup(x => x.GetNodeByAliasPath(aliasPath))
                .Returns(node);
        }

    }
}
