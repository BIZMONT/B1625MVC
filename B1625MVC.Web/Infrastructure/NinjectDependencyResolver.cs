using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;

namespace B1625MVC.Web.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            BindAll();
        }

        private void BindAll()
        {
            //TODO: Here you can add DI binds
        }

        public object GetService(Type serviceType)
        {
            return _kernel.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
    }
}