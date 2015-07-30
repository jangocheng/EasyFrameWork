﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Easy.Extend;
using Easy.IOC;

namespace Easy.Reflection
{
    public abstract class AssemblyInfo
    {
        public class KnownTypes
        {
            public static readonly Type DependencyType = typeof(IDependency);
            public static readonly Type FreeDependencyType = typeof(IFreeDependency);
            public static readonly Type EntityType = typeof(IEntity);
        }



        private List<Assembly> _assemblies;
        public virtual IEnumerable<Assembly> Assemblies
        {
            get
            {
                if (_assemblies != null) return _assemblies;
                _assemblies = new List<Assembly>();
                AppDomain.CurrentDomain.GetAssemblies()
                    .Each(_assemblies.Add);
                return _assemblies.Where(assembly => !assembly.GlobalAssemblyCache);
            }
        }

        private IEnumerable<Type> _currentTypes;
        public virtual IEnumerable<Type> CurrentTypes
        {
            get { return _currentTypes ?? (_currentTypes = Assemblies.ConcreteTypes()); }
        }
        private IEnumerable<Type> _publicTypes;
        public virtual IEnumerable<Type> PublicTypes
        {
            get { return _publicTypes ?? (_publicTypes = Assemblies.PublicTypes()); }
        }
    }
}