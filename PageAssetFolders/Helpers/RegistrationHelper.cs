using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Core;
using Microsoft.Extensions.DependencyInjection;

namespace KenticoCommunity.PageAssetFolders.Helpers
{
    /// <summary>
    /// Helper for managing dependency injection registration in the
    /// Kentico Xperience CMS app
    /// </summary>
    public static class RegistrationHelper
    {
        public static bool IsRegistered<TService>()
        {
            var serviceType = typeof(TService);
            IServiceCollection serviceDescriptors = new ServiceCollection();
            Service.MergeDescriptors(serviceDescriptors);
            return serviceDescriptors.Where(p => p.ServiceType == serviceType).FirstOrDefault() != null;
        }
    }
}
