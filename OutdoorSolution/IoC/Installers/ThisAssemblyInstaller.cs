using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace OutdoorSolution.IoC.Installers
{
    public class ThisAssemblyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                                      .InNamespace(@"OutdoorSolution.Links")
                                      .LifestyleTransient());
        }
    }
}