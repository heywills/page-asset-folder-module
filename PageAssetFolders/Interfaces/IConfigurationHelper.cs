using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenticoCommunity.PageAssetFolders.Interfaces
{
    /// <summary>
    /// Helper to provide the correct configuration file for the current context.
    /// </summary>
    public interface IConfigurationHelper
    {
        /// <summary>
        /// Get the .NET Configuration object for the current context.
        /// </summary>
        /// <returns></returns>
        Configuration GetWebConfiguration();
    }
}
