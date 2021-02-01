using System.Collections.Generic;
using System.Configuration;

namespace KenticoCommunity.PageAssetFolders.Configurations
{
    /// <summary>
    /// Represents a collection of asset folder registrations in the configuration, to provide a 
    /// list of page types that should should have a default asset folder, and to provide the
    /// page type and default name of the desired asset folders.
    /// </summary>
    [ConfigurationCollection(typeof(AssetFolderRegistrationElement), AddItemName = AssetFolderRegistrationElementName)]
    public class AssetFolderRegistrationElementCollection : ConfigurationElementCollection, IEnumerable<AssetFolderRegistrationElement>
    {
        private const string AssetFolderRegistrationElementName = "assetFolder";

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        /// <remarks>
        /// Do not simply cast the collection, because it will cause an infinite loop.
        /// Both Enumerable.Cast<TResult>() and Enumerable.AsEnumerable<TResult>() extensions
        /// call IEnumerator<TResult> GetEnumerator()
        /// </remarks>
        public new IEnumerator<AssetFolderRegistrationElement> GetEnumerator()
        {
            var count = Count;
            for (var i = 0; i < count; i++) yield return BaseGet(i) as AssetFolderRegistrationElement;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new AssetFolderRegistrationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AssetFolderRegistrationElement)element).ParentClass;
        }
    }
}
