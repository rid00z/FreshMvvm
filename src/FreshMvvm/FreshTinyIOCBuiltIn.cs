using System;
using FreshTinyIoC;

namespace FreshMvvm
{
    /// <summary>
    /// Built in TinyIOC for ease of use
    /// </summary>
    public class FreshTinyIOCBuiltIn : IFreshIOC
    {
        public static FreshTinyIoCContainer Current
        {
            get 
            {
                return FreshTinyIoCContainer.Current;
            }
        }

        public void Register<RegisterType>(RegisterType instance, string name) where RegisterType : class
        {
            FreshTinyIoCContainer.Current.Register (instance, name);
        }

        public void Register<RegisterType>(RegisterType instance) where RegisterType : class
        {
            FreshTinyIoCContainer.Current.Register (instance);
        }

        public ResolveType Resolve<ResolveType>(string name) where ResolveType : class
        {
            return FreshTinyIoCContainer.Current.Resolve<ResolveType> (name);
        }

        public ResolveType Resolve<ResolveType>() where ResolveType : class
        {
            return FreshTinyIoCContainer.Current.Resolve<ResolveType> ();
        }

        public void Register<RegisterType, RegisterImplementation> ()
            where RegisterType : class
            where RegisterImplementation : class, RegisterType
        {
            FreshTinyIoCContainer.Current.Register<RegisterType, RegisterImplementation>();
        }

        public object Resolve(Type resolveType)
        {
            return FreshTinyIoCContainer.Current.Resolve (resolveType);
        }
    }
}

