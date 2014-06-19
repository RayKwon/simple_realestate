using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using SimpleRealestate.Dao;

namespace SimpleRealestate
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<IPropertyDao, PropertyDao>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}