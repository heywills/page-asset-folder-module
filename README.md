# Kentico Xperience Page Asset Folder Module
Improve content authoring with this Kentico Xperience module. It allows providing automatic content asset folders for pages in the Xperience content tree.

## Background
A common way way to improve content authoring in Kentico Xperience is to allow multiple pieces of child content to be organized with each page in the content tree. This provides an easy way for authors to organize the child content that belongs to each page.

This solution uses Xperience's global events to automatically create asset folders for authors whenever they create a new page. This solution is configurable, so that developers can determine which types of pages are given asset folders, and the page type used to create each asset folders.  By allowing the use of a separate folder type for each page type, the solution allows you to control what child types are allowed under each page or folder. This provides authors a nicely trimmed list of options when they add content to a folder, so they do not have overwhelming choices, and are not allowed to create unsupported content.

## Compatibility
* .NET 4.6.1 or higher
* Kentico Xperience versions
  - 12.0.29 or higher (use KenticoCommunity.UpsertAlternativeForms 12.0.0)
  - 13.0.0 or higher (use KenticoCommunity.UpsertAlternativeForms 13.0.0)

## Installation
To install, add the NuGet package, "KenticoCommunity.KenticoCommunity.PageAssetFolders", to your CMS project and then add the web.config sections described below.

## Usage
After adding the NuGet package to your CMS project, the Page Asset Folder module is installed. It will look for a web.config section to determine what default folders to create. 

**Note**: You can also register your own `IAssetFolderRegistrationListFactory`, if you'd rather configure the module in code. If that's your preference, jump to the section, [Create your own configuration factory](#create-your-own-configuration-factory).

### Register Configuration Section

Add the following line to the `configSections` element of the web.config.

```
<section name="defaultAssetFolders" type="KenticoCommunity.PageAssetFolders.Configurations.DefaultAssetFoldersSection,KenticoCommunity.PageAssetFolders" />
```

### Configuration

Here's an example of adding configurations for two types of pages. Notice for each type of page, the type of folder is specified, as well as the default name for new folders.

```
  <defaultAssetFolders>
    <assetFolder parentClass="cms.menuitem" childClass="cms.folder" defaultName="Page assets"/>
    <assetFolder parentClass="Acme.LandingPage" childClass="Acme.LandingPageAssetFolder" defaultName="Page assets"/>
  </defaultAssetFolders>

```

### Create your own configuration factory
If you'd prefer to configure this module in code, simply create your own implementation of `IAssetFolderRegistrationListFactory` and register it using Kentico's `RegisterImplementation` assembly attribute. Here's a sample:
```
using CMS;
using CMSApp.Acme.Factories;
using KenticoCommunity.PageAssetFolders.Interfaces;
using KenticoCommunity.PageAssetFolders.Models;
using System.Collections.Generic;

[assembly: RegisterImplementation(typeof(IAssetFolderRegistrationListFactory), typeof(AssetFolderRegistrationListFactory))]
namespace CMSApp.Acme.Factories
{
    public class AssetFolderRegistrationListFactory : IAssetFolderRegistrationListFactory
    {
        public List<AssetFolderRegistration> GetAssetFolderRegistrations()
            => new List<AssetFolderRegistration>()
                {
                    new AssetFolderRegistration()
                    {
                        ParentClass = "cms.menuitem",
                        ChildClass = "cms.folder",
                        DefaultName = "Assets"
                    },
                    new AssetFolderRegistration()
                    {
                        ParentClass = "Acme.LandingPage",
                        ChildClass = "Acme.LandingPageAssetFolder",
                        DefaultName = "Assets"
                    }
                };
    }
}
```

## License

This project uses a standard MIT license which can be found [here](https://github.com/heywills/page-asset-folder-module/master/LICENSE).

## Contribution

Contributions are welcome. Feel free to submit pull requests to the repo.

## Support

Please report bugs as issues in this GitHub repo.  We'll respond as soon as possible.
