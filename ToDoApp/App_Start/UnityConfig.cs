using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using ToDoApp.Services;

namespace ToDoApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IZadanieRepository, ZadanieRepository>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}