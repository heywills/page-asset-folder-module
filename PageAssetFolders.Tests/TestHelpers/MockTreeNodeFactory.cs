using CMS.Base;
using Moq;

namespace Patterson.Content.Kentico.Tests.TestHelpers
{
    public static class MockTreeNodeFactory
    {
        private const string NodeAliasFieldName = "NodeAlias";
        public static ITreeNode Create(string className)
        {
            return Mock.Of<ITreeNode>(n => n.ClassName == className);
        }

        public static ITreeNode Create(string className, string aliasPath, string originalAlias, string newAlias)
        {
            var mock = new Mock<ITreeNode>();
            mock.Setup(n => n.ClassName).Returns(className);
            mock.Setup(n => n.NodeAliasPath).Returns(aliasPath);
            var dataContainerMock = mock.As<IAdvancedDataContainer>();
            dataContainerMock.Setup(d => d.GetOriginalValue(NodeAliasFieldName)).Returns(originalAlias);
            dataContainerMock.Setup(d => d.GetValue(NodeAliasFieldName)).Returns(newAlias);
            return (ITreeNode)dataContainerMock.Object;
        }
    }
}
