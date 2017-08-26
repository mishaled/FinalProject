using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace Common
{
    public static class DIFactory
    {
        private static object _lock = new object();
        private static UnityContainer _container;

        private static UnityContainer Container
        {
            get
            {
                if (_container == null)
                {
                    lock (_lock)
                    {
                        if (_container == null)
                        {
                            _container = new UnityContainer();
                        }
                    }
                }

                return _container;
            }
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public static void Register<T, G>() where G : T
        {
            Container.RegisterType<T, G>();
        }

        public static void Register<T>(T instance)
        {
            Container.RegisterInstance<T>(instance);
        }
    }
}
