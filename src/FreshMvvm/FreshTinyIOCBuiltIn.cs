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

        public IRegisterOptions Register<RegisterType>(RegisterType instance, string name) where RegisterType : class
        {
            return FreshTinyIoCContainer.Current.Register (instance, name);
        }

        public IRegisterOptions Register<RegisterType>(RegisterType instance) where RegisterType : class
        {
            return FreshTinyIoCContainer.Current.Register (instance);
        }

        public ResolveType Resolve<ResolveType>(string name) where ResolveType : class
        {
            return FreshTinyIoCContainer.Current.Resolve<ResolveType> (name);
        }

        public ResolveType Resolve<ResolveType>() where ResolveType : class
        {
            return FreshTinyIoCContainer.Current.Resolve<ResolveType> ();
        }

        public IRegisterOptions Register<RegisterType, RegisterImplementation> ()
            where RegisterType : class
            where RegisterImplementation : class, RegisterType
        {
            return FreshTinyIoCContainer.Current.Register<RegisterType, RegisterImplementation>();
        }

        public object Resolve(Type resolveType)
        {
            return FreshTinyIoCContainer.Current.Resolve (resolveType);
        }

        public void Unregister<RegisterType>()
        {
            FreshTinyIoCContainer.Current.Unregister<RegisterType>();
        }

        public void Unregister<RegisterType>(string name)
        {
            FreshTinyIoCContainer.Current.Unregister<RegisterType>(name);
        }
    }
}

