using System;
using System.Collections.Generic;

namespace Features.ServiceLocators.Core
{
    public static class ServiceLocator
    {
        private static readonly IDictionary<Type, object> Container = new Dictionary<Type, object>();
        
        public static void Register<T>(T service) where T : class
        {
            var type = typeof(T);
            Container[type] = service;
        }
        
        public static T Resolve<T>() where T : class
        {
            var type = typeof(T);
            if (!Container.ContainsKey(type))
            {
                return null;
            }

            return (T) Container[type];
        }
    }
}
