using ModuleA.ViewModels;
using ModuleA.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace ModuleA
{
    public class ModuleAProfile : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA,ViewAViewModel>();
            containerRegistry.RegisterDialog<ViewC, ViewCViewModel>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
           
        }
    }
}
