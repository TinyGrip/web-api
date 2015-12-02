using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using OutdoorSolution.Dal;
using OutdoorSolution.Services;
using OutdoorSolution.Services.Interfaces;

namespace OutdoorSolution.IoC.Installers
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                                      .InNamespace(@"OutdoorSolution.Mapping")
                                      .LifestyleTransient());

            container.Register(Component.For<IUnitOfWork>()
                                        .ImplementedBy<ApplicationDbContext>()
                                        .Properties(PropertyFilter.IgnoreAll)
                                        .LifestylePerWebRequest());

            // TODO: think about lifestyle for services!
            container.Register(Classes.FromAssemblyContaining<IService>()
                                      .BasedOn<IService>()
                                      .WithService.FromInterface()
                                      .LifestyleTransient());
        }
    }
}