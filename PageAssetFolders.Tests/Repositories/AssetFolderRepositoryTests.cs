using CMS.DocumentEngine;
using NUnit.Framework;
using KenticoCommunity.PageAssetFolders.Interfaces;
using KenticoCommunity.PageAssetFolders.Repositories;
using KenticoCommunity.PageAssetFolders.Tests.TestHelpers;
using System;

namespace KenticoCommunity.PageAssetFolders.Tests.Repositories
{
    [TestFixture, NonParallelizable]
    public class AssetFolderRepositoryTests
    {
        private readonly IAssetFolderRepository repository = new AssetFolderRepository();

        [TestCase(null, "className", "childName")]
        public void AddChildNode_NodeNullCheck(TreeNode parentNode, string className, string childName)
        {
            Assert.Throws<ArgumentNullException>(delegate
             {
                 this.repository.AddChildNode(parentNode, className, childName);
             });
        }

        [TestCase(null, "childName"),
         TestCase("className", null)]
        public void AddChildNode_StringNullCheck(string className, string childName)
        {
            var parentNode = MockTreeNodeFactory.Create("parentClass");
            Assert.Throws<ArgumentNullException>(delegate
             {
                 this.repository.AddChildNode(parentNode, className, childName);
             });
        }

        [TestCase("", "childName"),
         TestCase("className", "")]
        public void AddChildNode_StringEmptyCheck(string className, string childName)
        {
            var parentNode = MockTreeNodeFactory.Create("parentClass");
            Assert.Throws<ArgumentException>(delegate
             {
                 this.repository.AddChildNode(parentNode, className, childName);
             });
        }

        [Test]
        public void GetChildNodesByClass_NodeNullCheck()
        {
            Assert.Throws<ArgumentNullException>(delegate
             {
                 this.repository.GetChildNodesByClass(null, "className");
             });
        }

        [Test]
        public void GetChildNodesByClass_StringNullCheck()
        {
            var parentNode = MockTreeNodeFactory.Create("parentClass");
            Assert.Throws<ArgumentNullException>(delegate
             {
                 this.repository.GetChildNodesByClass(parentNode, null);
             });
        }

        [Test]
        public void GetChildNodesByClass_StringEmptyCheck()
        {
            var parentNode = MockTreeNodeFactory.Create("parentClass");
            Assert.Throws<ArgumentException>(delegate
             {
                 this.repository.GetChildNodesByClass(parentNode, "");
             });
        }

        [Test]
        public void MoveNodeToTop_NodeNullCheck()
        {
            Assert.Throws<ArgumentNullException>(delegate
             {
                 this.repository.MoveNodeToTop(null);
             });
        }


        [Test]
        public void GetNodeByAliasPath_StringNullCheck()
        {
            Assert.Throws<ArgumentNullException>(delegate
             {
                 this.repository.GetNodeByAliasPath(null);
             });
        }

        [Test]
        public void GetNodeByAliasPath_StringEmptyCheck()
        {
            Assert.Throws<ArgumentException>(delegate
             {
                 this.repository.GetNodeByAliasPath("");
             });
        }

        [Test]
        public void GetNodeByID_Throws_ArgumentNotNull_Exception_When_Zero_NodeID_Provided()
        {
            Assert.Throws<ArgumentException>(() => this.repository.GetNodeById(0, "dental", "en-us"));
        }

        [Test]
        public void GetNodeByID_Throws_ArgumentNotNull_Exception_When_Null_Culture_Provided()
        {
            Assert.Throws<ArgumentNullException>(() => this.repository.GetNodeById(1, "dental", null));
        }

        [Test]
        public void GetNodeByID_Throws_ArgumentNotNull_Exception_When_Empty_Culture_Provided()
        {
            Assert.Throws<ArgumentException>(() => this.repository.GetNodeById(0, "dental", string.Empty));
        }
        [Test]
        public void GetNodeByID_Throws_ArgumentNotNull_Exception_When_Null_SiteCode_Provided()
        {
            Assert.Throws<ArgumentNullException>(() => this.repository.GetNodeById(1, null, "en-us"));
        }

        [Test]
        public void GetNodeByID_Throws_ArgumentNotNull_Exception_When_Empty_SiteCode_Provided()
        {
            Assert.Throws<ArgumentException>(() => this.repository.GetNodeById(0,  string.Empty, "en-us"));
        }
    }
}
