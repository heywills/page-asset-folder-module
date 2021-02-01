using System.Configuration;

namespace KenticoCommunity.PageAssetFolders.Configurations
{
    /// <summary>
    /// Represents an element in the configuration for storing an asset folder registration,
    /// defining a page type that should should have a default asset folder, and including the
    /// asset folder's page type and default name.
    /// </summary>
    public class AssetFolderRegistrationElement: ConfigurationElement
    {
        private const string ParentClassAttributeName = "parentClass";
        private const string ChildClassAttributeName = "childClass";
        private const string DefaultNameAttributeName = "defaultName";

        [ConfigurationProperty(ParentClassAttributeName, DefaultValue = "", IsKey = true, IsRequired = true)]
        public string ParentClass
        {
            get => (string)this[ParentClassAttributeName];
            set => this[ParentClassAttributeName] = value;
        }

        [ConfigurationProperty(ChildClassAttributeName, DefaultValue = "", IsKey = false, IsRequired = true)]
        public string ChildClass
        {
            get => (string)this[ChildClassAttributeName];
            set => this[ChildClassAttributeName] = value;
        }

        [ConfigurationProperty(DefaultNameAttributeName, DefaultValue = "", IsKey = false, IsRequired = true)]
        public string DefaultName
        {
            get => (string)this[DefaultNameAttributeName];
            set => this[DefaultNameAttributeName] = value;
        }
    }
}
