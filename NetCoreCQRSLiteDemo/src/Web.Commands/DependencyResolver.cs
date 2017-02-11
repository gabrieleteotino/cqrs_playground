using CQRSlite.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Commands
{
    public class DependencyResolver : IServiceLocator
    {
        private readonly IServiceProvider _serviceProvider;
        public DependencyResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #region IServiceLocator
        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                return null;
            }
            try
            {
                return _serviceProvider.GetService(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        } 
        #endregion
    }
}
