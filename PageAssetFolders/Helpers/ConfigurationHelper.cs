using CMS.Base;
using KenticoCommunity.PageAssetFolders.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace KenticoCommunity.PageAssetFolders.Helpers
{
    /// <summary>
    /// Helper class to provide the correct configuration file for the current context. This will ensure the web.config file
    /// for the CMS app is used, even if the module is running in ContinuousIntegration.exe.
    /// </summary>
    public class ConfigurationHelper : IConfigurationHelper
    {
        /// <summary>
        /// Get the .NET Configuration object for the CMSApp app's web.config. This will load the web.config file whether running
        /// in the Kentico Web App or running in ContinuousIntegration.exe
        /// </summary>
        /// <returns></returns>
        public Configuration GetWebConfiguration()
        {
            var webDirectoryPath = SystemContext.WebApplicationPhysicalPath;
            return OpenConfiguration(webDirectoryPath);
        }

        /// <summary>
        /// Open the configuration file in the given application path withthe given configuration file name.
        /// </summary>
        /// <param name="appPath"></param>
        /// <param name="configFileName"></param>
        /// <returns></returns>
        public Configuration OpenConfiguration(string appPath, string configFileName = "web.config")
        {
            var mapping = new VirtualDirectoryMapping(appPath, true, configFileName);
            var webConfigurationFileMap = new WebConfigurationFileMap();
            webConfigurationFileMap.VirtualDirectories.Add(string.Empty, mapping);
            try
            {
                return WebConfigurationManager.OpenMappedWebConfiguration(webConfigurationFileMap, string.Empty);
            }
            catch
            {
                return ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap
                {
                    ExeConfigFilename = Path.Combine(appPath, configFileName)
                }, ConfigurationUserLevel.None);
            }
        }
    }
}
