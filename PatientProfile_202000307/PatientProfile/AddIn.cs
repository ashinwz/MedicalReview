using System.Linq;
using System.Reflection;
using PerkinElmer.Signals.Analytics.AppCommon;
using PerkinElmer.Signals.Analytics.AppCommon.AppRegistry;
using Spotfire.Dxp.Application.Extension;
using SpotfireAddin = Spotfire.Dxp.Application.Extension.AddIn;

namespace PatientProfile
{
    public class AddIn : SpotfireAddin
    {
        protected override void OnGlobalServicesRegistered(ServiceProvider serviceProvider)
        {
            base.OnGlobalServicesRegistered(serviceProvider);
            var appRegistry = serviceProvider.GetService<AppRegistryService>();
            appRegistry.Register(
                AddInTools.GetAppMetadata<App>().Name,
                typeof(App),
                AddInTools.GetAppStoreStartAppData<App>());
        }
    }
}
