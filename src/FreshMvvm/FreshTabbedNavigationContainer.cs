using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace FreshMvvm
{
    public class FreshTabbedNavigationContainer : TabbedPage, IFreshNavigationService
    {
        public FreshTabbedNavigationContainer ()
        {				
            RegisterNavigation ();
        }

        protected void RegisterNavigation ()
        {
            FreshIOC.Container.Register<IFreshNavigationService> (this);
        }

        public virtual Page AddTab<T> (string title, string icon, object data = null) where T : FreshBasePageModel
        {
            var page = FreshPageModelResolver.ResolvePageModel<T> (data);
            var navigationContainer = CreateContainerPage (page);
            navigationContainer.Title = title;
            if (!string.IsNullOrWhiteSpace(icon))
                navigationContainer.Icon = icon;
            Children.Add (navigationContainer);
            return navigationContainer;
        }

        protected virtual Page CreateContainerPage (Page page)
        {
            return new NavigationPage (page);
        }

		public async System.Threading.Tasks.Task PushPage (Xamarin.Forms.Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
        {
            if (modal)
                await this.CurrentPage.Navigation.PushModalAsync (CreateContainerPage (page));
            else
                await this.CurrentPage.Navigation.PushAsync (page);
        }

		public async System.Threading.Tasks.Task PopPage (bool modal = false, bool animate = true)
        {
            if (modal)
                await this.CurrentPage.Navigation.PopModalAsync (animate);
            else
                await this.CurrentPage.Navigation.PopAsync (animate);
        }

    }
}

