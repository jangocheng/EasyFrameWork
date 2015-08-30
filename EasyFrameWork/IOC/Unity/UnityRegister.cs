﻿using System;
using Easy.Extend;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Easy.Reflection;

namespace Easy.IOC.Unity
{
    public sealed class UnityRegister : AssemblyInfo
    {
        private readonly IUnityContainer _container;
        public UnityRegister(IUnityContainer container)
        {
            _container = container;
            PublicTypes.Each(p =>
            {
                if (p.IsClass && !p.IsAbstract && !p.IsInterface && !p.IsGenericType)
                {
                    if ((KnownTypes.DependencyType.IsAssignableFrom(p) ||
                        KnownTypes.EntityType.IsAssignableFrom(p)) && !KnownTypes.FreeDependencyType.IsAssignableFrom(p))
                    {
                        if (KnownTypes.EntityType.IsAssignableFrom(p))
                        {
                            _container.RegisterType(p, GetLifetimeManager(p));
                        }
                        else
                        {
                            foreach (var inter in p.GetInterfaces())
                            {
                                _container.RegisterType(inter, p, GetLifetimeManager(p));
                                _container.RegisterType(inter, p, inter.Name + p.FullName, GetLifetimeManager(p));
                            }
                        }
                    }

                }
            });
        }

        public void Regist()
        {
            var locator = new UnityServiceLocator(_container);
            ServiceLocator.SetLocatorProvider(() => locator);
        }


        LifetimeManager GetLifetimeManager(Type lifeTimeType)
        {
            LifetimeManager lifetimeManager;
            if (KnownTypes.SingleInstanceType.IsAssignableFrom(lifeTimeType))
            {
                lifetimeManager = new ContainerControlledLifetimeManager();
            }
            else if (KnownTypes.PerRequestType.IsAssignableFrom(lifeTimeType))
            {
                lifetimeManager = new PerRequestLifetimeManager();
            }
            else
            {
                lifetimeManager = new PerResolveLifetimeManager();
            }
            return lifetimeManager;
        }
    }
}