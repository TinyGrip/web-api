using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using OutdoorSolution.Dal;
using OutdoorSolution.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OutdoorSolution.IoC.Installers
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                                      .InNamespace(@"OutdoorSolution.Mapping")
                                      .LifestyleTransient());

            // TODO: think about lifestyle for services!
            container.Register(Classes.FromAssemblyContaining<PermissionsService>().Pick().LifestyleSingleton());
            
            container.Register(Component.For<ApplicationDbContext>()
                                        .Properties(PropertyFilter.IgnoreAll)
                                        .LifestyleTransient());
        }
    }
}