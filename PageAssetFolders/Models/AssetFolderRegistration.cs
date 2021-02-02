namespace KenticoCommunity.PageAssetFolders.Models
{
    /// <summary>
    /// Represents a registered asset folder type that should be created under specific a parent type. 
    /// </summary>
    public class AssetFolderRegistration
    {
        /// <summary>
        /// The parent type that should have an asset folder created under it.
        /// </summary>
        public string ParentClass { get; set; }
        /// <summary>
        /// The type of the folder to create under the parent page.
        /// </summary>
        public string ChildClass { get; set; }
        /// <summary>
        /// The name to use when creating new asset folders.
        /// </summary>
        public string DefaultName { get; set; }
    }
}
