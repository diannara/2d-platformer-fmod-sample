using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace TIGD.Services
{
    public class ServiceLocator
    {
        private static Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

        public static T Get<T>() where T : IService
        {
            Type type = typeof(T);
            if(!_services.ContainsKey(type))
            {
                return default(T);
            }
            return (T)_services[type];
        }

        public static bool TryGet<T>(out T service) where T : IService
        {
            Type type = typeof(T);
            if(!_services.ContainsKey(type))
            {
                service = default(T);
                return false;
            }
            service = (T)_services[type];
            return true;
        }

        public static void Register(IService service)
        {
            Type type = service.GetType();
            if(_services.ContainsKey(type))
            {
                // Logger.Warning("ServiceLocator", "Register", $"Service of type {type.Name} is already registered!");
                return;
            }

            _services.Add(type, service);
            // Logger.Info("ServiceLocator", "Register", $"Service of type {type.Name} was registered!");
        }

        public static void Unregister(IService service)
        {
            Type type = service.GetType();
            if(!_services.ContainsKey(type))
            {
                // Logger.Warning("ServiceLocator", "Register", $"Service of type {type.Name} was not previously registered!");
                return;
            }
            _services.Remove(type);
            // Logger.Info("ServiceLocator", "Register", $"Service of type {type.Name} was unregistered!");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Initialize()
        {
            // Call shutdown when the application quits
            Application.quitting += Shutdown;

            // Get all types that implement the IService interface
            Type[] serviceTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IService).IsAssignableFrom(type) && type.IsClass && !type.IsInterface && !type.IsAbstract)
                .ToArray();

            // Create an instance of each type and call its Initialize method
            foreach(Type serviceType in serviceTypes)
            {
                // Logger.Info("ServiceLocator", "Initialize", $"Initializing service of type: {serviceType.Name} ({serviceType})!");

                IService serviceInstance = (IService)Activator.CreateInstance(serviceType);
                serviceInstance.Initialize();
            }
        }

        public static void Shutdown()
        {
            // Remove callback just in case
            Application.quitting -= Shutdown;

            List<Type> serviceTypes = _services.Keys.ToList();
            foreach(Type serviceType in serviceTypes)
            {
                IService service = _services[serviceType];
                // Logger.Info("ServiceLocator", "Shutdown", $"Calling shutdown on service of type: {serviceType.Name} ({serviceType})!");
                service.Shutdown();
            }

            _services.Clear();
        }
    }
}