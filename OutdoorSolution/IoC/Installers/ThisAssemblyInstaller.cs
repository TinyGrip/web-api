using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OutdoorSolution.Dal;
using OutdoorSolution.Domain.Models;
using OutdoorSolution.Helpers;
using OutdoorSolution.Services;

namespace OutdoorSolution.IoC.Installers
{
    public class ThisAssemblyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                                      .InNamespace(@"OutdoorSolution.Links")
                                      .LifestyleTransient());

            container.Register(Component.For<TGUserManager>()
                                        .OnCreate((kernel, instance) => IdentityHelper.InitUserManager(instance)) );
        }
    }
}