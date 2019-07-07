using System;
using FreshMvvm.Ioc;
using FreshTinyIoC;

namespace FreshMvvm
{
    /// <summary>
    /// Built in TinyIOC for ease of use
    /// </summary>
    public class FreshTinyIocBuiltIn : IFreshIoc
    {
        public static FreshTinyIocContainer Current
        {
            get 
            {
                return FreshTinyIocContainer.Current;
            }
        }

        public IRegisterOptions Register<RegisterType>(RegisterType instance, string name) where RegisterType : class
        {
            return FreshTinyIocContainer.Current.Register (instance, name);
        }

        public IRegisterOptions Register<RegisterType>(RegisterType instance) where RegisterType : class
        {
            return FreshTinyIocContainer.Current.Register (instance);
        }

        public ResolveType Resolve<ResolveType>(string name) where ResolveType : class
        {
            return FreshTinyIocContainer.Current.Resolve<ResolveType> (name);
        }

        public ResolveType Resolve<ResolveType>() where ResolveType : class
        {
            return FreshTinyIocContainer.Current.Resolve<ResolveType> ();
        }

        public IRegisterOptions Register<RegisterType, RegisterImplementation> ()
            where RegisterType : class
            where RegisterImplementation : class, RegisterType
        {
            return FreshTinyIocContainer.Current.Register<RegisterType, RegisterImplementation>();
        }

        public object Resolve(Type resolveType)
        {
            return FreshTinyIocContainer.Current.Resolve (resolveType);
        }

        public void Unregister<RegisterType>()
        {
            FreshTinyIocContainer.Current.Unregister<RegisterType>();
        }

        public void Unregister<RegisterType>(string name)
        {
            FreshTinyIocContainer.Current.Unregister<RegisterType>(name);
        }
    }
}

