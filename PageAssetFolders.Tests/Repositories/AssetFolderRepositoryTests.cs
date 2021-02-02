using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.CMS;
using CMS.Tests;
using KenticoCommunity.PageAssetFolders.Interfaces;
using KenticoCommunity.PageAssetFolders.Repositories;
using NUnit.Framework;
using System;
using Tests.DocumentEngine;

namespace KenticoCommunity.PageAssetFolders.Tests.Repositories
{
    [TestFixture, NonParallelizable]
    public class AssetFolderRepositoryTests: UnitTests
    {
        private readonly IAssetFolderRepository repository = new AssetFolderRepository();

        [SetUp]
        public void TestSetup()
        {
            DocumentGenerator.RegisterDocumentType<Folder>(Folder.CLASS_NAME);
            Fake().DocumentType<Folder>(Folder.CLASS_NAME);
        }

        [TestCase(null, "className", "childName")]
        public void AddChildNode_Throws_ArgumentNullException_If_ParentNode_Is_Null(TreeNode parentNode, string className, string childName)
        {
            Assert.Throws<ArgumentNullException>(delegate
             {
                 this.repository.AddChildNode(parentNode, className, childName);
             });
        }

        [TestCase(null, "childName")]
        [TestCase("className", null)]
        public void AddChildNode_Throws_ArgumentNullException_If_Parameter_Is_Null(string className, string childName)
        {
            var parentNode = CreateFolder();
            Assert.Throws<ArgumentNullException>(delegate
             {
                 this.repository.AddChildNode(parentNode, className, childName);
             });
        }

        [TestCase("", "childName")]
        [TestCase("className", "")]
        public void AddChildNode_Throws_ArgumentException_If_Parameter_Is_Empty(string className, string childName)
        {
            var parentNode = CreateFolder();
            Assert.Throws<ArgumentException>(delegate
             {
                 this.repository.AddChildNode(parentNode, className, childName);
             });
        }

        [Test]
        public void GetChildNodesByClass_Throws_ArgumentNullException_If_ParentNode_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(delegate
             {
                 this.repository.GetChildNodesByClass(null, "className");
             });
        }

        [Test]
        public void GetChildNodesByClass_Throws_ArgumentNullException_If_ChildClassName_Is_Null()
        {
            var parentNode = CreateFolder();
            Assert.Throws<ArgumentNullException>(delegate
             {
                 this.repository.GetChildNodesByClass(parentNode, null);
             });
        }

        [Test]
        public void GetChildNodesByClass_Throws_ArgumentException_If_ChildClassName_Is_Empty()
        {
            var parentNode = CreateFolder();
            Assert.Throws<ArgumentException>(delegate
             {
                 this.repository.GetChildNodesByClass(parentNode, "");
             });
        }

        [Test]
        public void MoveNodeToTop_Throws_ArgumentNullException_If_TreeNode_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(delegate
             {
                 this.repository.MoveNodeToTop(null);
             });
        }


        [Test]
        public void GetNodeByAliasPath_Throws_ArgumentNullException_If_AliasPath_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(delegate
             {
                 this.repository.GetNodeByAliasPath(null);
             });
        }

        [Test]
        public void GetNodeByAliasPath_Throws_ArgumentException_If_AliasPath_Is_Empty()
        {
            Assert.Throws<ArgumentException>(delegate
             {
                 this.repository.GetNodeByAliasPath("");
             });
        }

        [TestCase(0, "dental", "en-us")]
        [TestCase(1, "", "en-us")]
        [TestCase(1, "dental", "")]
        public void GetNodeByID_Throws_ArgumentException_If_Invalid_Parameter(int nodeId, string siteCode, string cultureCode)
        {
            Assert.Throws<ArgumentException>(() => this.repository.GetNodeById(nodeId, siteCode, cultureCode));
        }

        [TestCase(1, null, "en-us")]
        [TestCase(1, "dental", null)]
        public void GetNodeByID_Throws_ArgumentNullException_If_Null_Parameter(int nodeId, string siteCode, string cultureCode)
        {
            Assert.Throws<ArgumentNullException>(() => this.repository.GetNodeById(nodeId, siteCode, cultureCode));
        }

        private TreeNode CreateFolder()
        {
            var folder = TreeNode.New(Folder.CLASS_NAME).With(p =>
            {
                p.NodeAlias = "Test Folder";
                p.DocumentCulture = "en-US";
            });
            return folder;
        }
    }
}
