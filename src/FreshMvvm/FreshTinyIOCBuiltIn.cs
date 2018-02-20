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
            var result = FreshTinyIoCContainer.Current.Resolve<ResolveType>(name);
            BuildUp(ref result);
            return result;
        }

        public ResolveType Resolve<ResolveType>() where ResolveType : class
        {
            var result = FreshTinyIoCContainer.Current.Resolve<ResolveType>();
            BuildUp(ref result);
            return result;
        }

        public IRegisterOptions Register<RegisterType, RegisterImplementation> ()
            where RegisterType : class
            where RegisterImplementation : class, RegisterType
        {
            return FreshTinyIoCContainer.Current.Register<RegisterType, RegisterImplementation>();
        }

        public object Resolve(Type resolveType)
        {
            var result = FreshTinyIoCContainer.Current.Resolve (resolveType);
            BuildUp(ref result);
            return result;
        }

        public void BuildUp<ResolveType>(ref ResolveType input) where ResolveType : class
        {
            FreshTinyIoCContainer.Current.BuildUp(input);
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

