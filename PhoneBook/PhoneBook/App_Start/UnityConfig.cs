using PhoneBook.Controllers;
using PhoneBook.Models;
using PhoneBook.Models.Repository;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace PhoneBook
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            //container.RegisterType(typeof(IRepository<BusinessCard>), typeof(BusinessCardDbReopsitory));

            container.RegisterType<IRepository<BusinessCard>, BusinessCardDbReopsitory>();

            container.RegisterType<AccountController>(new InjectionConstructor());

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));


          
        }
    }
}