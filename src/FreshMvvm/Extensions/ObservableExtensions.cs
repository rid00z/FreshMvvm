using System;
using System.Reactive;
using System.Reactive.Linq;
using Xamarin.Forms;

namespace FreshMvvm.Extensions
{
    public static class ObservableExtensions
    {
        public static IDisposable SubscribeWeakly<T, TTarget>(this IObservable<T> observable, TTarget target, Action<TTarget, T> onNext) where TTarget : class
        {
            var reference = new WeakReference(target);

            if (onNext.Target != null)
            {
                throw new ArgumentException("onNext must refer to a static method, or else the subscription will still hold a strong reference to target");
            }

            IDisposable subscription = null;
            subscription = observable.Subscribe(item =>
            {
                var currentTarget = reference.Target as TTarget;
                if (currentTarget != null)
                {
                    onNext(currentTarget, item);
                }
                else
                {
                    subscription.Dispose();
                }
            });

            return subscription;
        }
    }
}
