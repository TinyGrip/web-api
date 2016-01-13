using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using OutdoorSolution.Controllers;

namespace OutdoorSolution.IoC.Installers
{
    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().Pick()
                                .If(Component.IsInSameNamespaceAs<RootController>())
                                .If(t => t.Name.EndsWith("Controller"))
                                .LifestylePerWebRequest());
        }

    }
}