using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;

using B1625MVC.Model.Abstract;
using B1625MVC.Model.Repositories;

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
            _kernel.Bind<IB1625Repository>().To<B1625DbRepository>();
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