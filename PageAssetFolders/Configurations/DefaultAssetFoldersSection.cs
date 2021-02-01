using System.Configuration;

namespace KenticoCommunity.PageAssetFolders.Configurations
{
    /// <summary>
    /// A configuration section for registering default asset folders by
    /// the type of parent page and the type of asset folder.
    /// This configuration section contains one default collection.
    /// </summary>
    public class DefaultAssetFoldersSection: ConfigurationSection
    {
        public const string DefaultAssetFoldersSectionName = "defaultAssetFolders";
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public AssetFolderRegistrationElementCollection AssetFolderRegistrations 
        { 
            get 
            { 
                return this[""] as AssetFolderRegistrationElementCollection; 
            } 
        }

    }
}
