using System;
using System.Reactive;
using System.Reactive.Linq;
using Xamarin.Forms;

namespace FreshMvvm
{
    public static class PageExtensions
    {
        public static FreshBasePageModel GetModel(this Page page)
        {
            var pageModel = page.BindingContext as FreshBasePageModel;
            return pageModel;
        }

        public static void NotifyAllChildrenPopped(this NavigationPage navigationPage)
        {
            foreach (var page in navigationPage.Navigation.ModalStack)
            {
                var pageModel = page.GetModel();
                if (pageModel != null)
                    pageModel.RaisePageWasPopped();
            }

            foreach (var page in navigationPage.Navigation.NavigationStack)
            {
                var pageModel = page.GetModel();
                if (pageModel != null)
                    pageModel.RaisePageWasPopped();
            }
        }

		public static IObservable<EventPattern<EventArgs>> ObserveAppearing(this Page page)
		{
			return Observable.FromEventPattern<EventHandler, EventArgs>(
				handler => page.Appearing += handler,
				handler => page.Appearing -= handler);
		}

		public static IObservable<EventPattern<EventArgs>> ObserveDisappearing(this Page page)
		{
			return Observable.FromEventPattern<EventHandler, EventArgs>(
				handler => page.Disappearing += handler,
				handler => page.Disappearing -= handler);
		}

		public static IObservable<EventPattern<NavigationEventArgs>> ObservePopped(this NavigationPage page)
		{
			return Observable.FromEventPattern<EventHandler<NavigationEventArgs>, NavigationEventArgs>(
				handler => page.Popped += handler,
				handler => page.Popped -= handler);
		}
    }
}

