using System;

namespace FreshMvvm
{
    public interface IFreshIOC
    {
        object Resolve(Type resolveType);
        void Register<RegisterType>(RegisterType instance) where RegisterType : class;
        ResolveType Resolve<ResolveType>() where ResolveType : class;
        void Register<RegisterType, RegisterImplementation> ()
            where RegisterType : class
            where RegisterImplementation : class, RegisterType;
    }
}

